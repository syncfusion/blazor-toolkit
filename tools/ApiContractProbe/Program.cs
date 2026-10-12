using System.Reflection;
using System.Runtime.Loader;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

if (args.Length is < 1 or > 2 || !File.Exists(args[0]) ||
    (args.Length == 2 && args[1] != "--historical"))
{
    Console.Error.WriteLine("Usage: ApiContractProbe <Toolkit assembly> [--historical]");
    return 2;
}
System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
System.Globalization.CultureInfo.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
var historical = args.Length == 2;

// A separate process is used for each version: never load both into one context.
var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.GetFullPath(args[0]));
var types = assembly.GetTypes(); // Fail closed if dependencies/types cannot be loaded.
const BindingFlags declared = BindingFlags.Public | BindingFlags.NonPublic |
    BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
bool Visible(MethodBase? method) => method is not null &&
    (method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly);
bool VisibleType(Type type) => type.IsNested
    ? (type.IsNestedPublic || type.IsNestedFamily || type.IsNestedFamORAssem) && VisibleType(type.DeclaringType!)
    : type.IsPublic;
string TypeName(Type type) => type.IsGenericType
    ? $"{type.GetGenericTypeDefinition().FullName}[{string.Join(",", type.GetGenericArguments().Select(TypeName))}]"
    : type.FullName ?? type.Name;
string Attributes(IEnumerable<CustomAttributeData> attributes) => string.Join(";", attributes
    .Where(a => a.AttributeType.FullName is not ("System.Runtime.CompilerServices.CompilerGeneratedAttribute"
        or "System.Diagnostics.DebuggerStepThroughAttribute"
        or "System.Runtime.CompilerServices.AsyncStateMachineAttribute"
        or "System.Runtime.CompilerServices.IteratorStateMachineAttribute"
        or "System.Runtime.CompilerServices.AsyncIteratorStateMachineAttribute"))
    .Select(a => a.ToString()).Order(StringComparer.Ordinal));
var surface = new SortedSet<string>(StringComparer.Ordinal);
var parameters = new SortedDictionary<string, string>(StringComparer.Ordinal);
var callbacks = new SortedDictionary<string, string>(StringComparer.Ordinal);
foreach (var type in types.OrderBy(TypeName, StringComparer.Ordinal))
{
    if (VisibleType(type))
    {
        surface.Add($"T {TypeName(type)} base={(type.BaseType is null ? "" : TypeName(type.BaseType))} abstract={type.IsAbstract} sealed={type.IsSealed} {Attributes(type.CustomAttributes)}");
        foreach (var implemented in type.GetInterfaces().Select(TypeName).Order(StringComparer.Ordinal))
            surface.Add($"I {TypeName(type)} {implemented}");
        foreach (var generic in type.GetGenericArguments().Where(t => t.IsGenericParameter))
            surface.Add($"G {TypeName(type)} {generic.Name} {generic.GenericParameterAttributes} {string.Join(",", generic.GetGenericParameterConstraints().Select(TypeName))} {Attributes(generic.CustomAttributes)}");
        foreach (var method in type.GetMethods(declared).Cast<MethodBase>().Concat(type.GetConstructors(declared)).Where(Visible))
        {
            var signature = $"{TypeName(type)}.{method.Name}({string.Join(",", method.GetParameters().Select(p => $"{TypeName(p.ParameterType)} {p.Name} {p.Attributes} default={JsonSerializer.Serialize(p.HasDefaultValue ? p.DefaultValue?.ToString() : null)} {Attributes(p.CustomAttributes)}"))})";
            var genericArguments = method is MethodInfo genericMethod ? genericMethod.GetGenericArguments() : Type.EmptyTypes;
            signature += $" arity={genericArguments.Length}";
            surface.Add($"M {signature} {method.Attributes} return={(method is MethodInfo m ? TypeName(m.ReturnType) + " " + Attributes(m.ReturnParameter.CustomAttributes) : "void")} {Attributes(method.CustomAttributes)}");
            foreach (var generic in genericArguments)
                surface.Add($"MG {signature} {generic.Name} {generic.GenericParameterAttributes} {string.Join(",", generic.GetGenericParameterConstraints().Select(TypeName).Order(StringComparer.Ordinal))} {Attributes(generic.CustomAttributes)}");
        }
        foreach (var property in type.GetProperties(declared).Where(p => Visible(p.GetMethod) || Visible(p.SetMethod)))
        {
            surface.Add($"P {TypeName(type)}.{property.Name} {TypeName(property.PropertyType)} {Attributes(property.CustomAttributes)}");
            if (property.CustomAttributes.Any(a => a.AttributeType.FullName == "Microsoft.AspNetCore.Components.ParameterAttribute"))
                parameters[$"{TypeName(type)}.{property.Name}"] = $"{TypeName(property.PropertyType)} get={Visible(property.GetMethod)} set={Visible(property.SetMethod)} {Attributes(property.CustomAttributes)}";
        }
        foreach (var field in type.GetFields(declared).Where(f => f.IsPublic || f.IsFamily || f.IsFamilyOrAssembly))
            surface.Add($"F {TypeName(type)}.{field.Name} {TypeName(field.FieldType)} {field.Attributes} constant={(field.IsLiteral ? field.GetRawConstantValue()?.ToString() : "-")} {Attributes(field.CustomAttributes)}");
    }
    foreach (var method in type.GetMethods(declared))
    {
        var attribute = method.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == "Microsoft.JSInterop.JSInvokableAttribute");
        if (attribute is null) continue;
        var identifier = attribute.ConstructorArguments.Count == 0 ? method.Name : attribute.ConstructorArguments[0].Value?.ToString() ?? method.Name;
        callbacks[$"{TypeName(type)}.{method.Name}"] = $"{identifier} static={method.IsStatic} ({string.Join(",", method.GetParameters().Select(p => TypeName(p.ParameterType)))}) -> {TypeName(method.ReturnType)}";
    }
}

var wire = new SortedDictionary<string, JsonNode?>(StringComparer.Ordinal);
Type Resolve(string name) => assembly.GetType("Syncfusion.Blazor.Toolkit." + name, throwOnError: true)!;
void RoundTrip(string name, string json, JsonSerializerOptions options)
{
    var type = Resolve(name);
    try
    {
        var value = JsonSerializer.Deserialize(json, type, options);
        wire[name + "|" + json] = JsonNode.Parse(JsonSerializer.Serialize(value, type, options));
    }
    catch (Exception error) when (error is NotSupportedException or InvalidOperationException)
    {
        // Outgoing-only DTOs need not support deserialization. Never count these as passing round trips.
        wire[name + "|" + json] = new JsonObject { ["unsupported"] = error.GetType().Name };
    }
}
var browser = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true };
var point = new JsonSerializerOptions { NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals };
foreach (var name in new[] { "Inputs.SelectedEventArgs", "Inputs.BeforeUploadEventArgs", "Charts.ZoomingEventArgs", "Charts.SharedTooltipRenderEventArgs", "Data.DataManagerRequest" })
{
    foreach (var json in new[] { "{}", "null" }) RoundTrip(name, json, browser);
}
RoundTrip("Inputs.SelectedEventArgs", "{\"cancel\":true,\"filesData\":[{\"name\":\"sample.txt\",\"size\":12}],\"modifiedFilesData\":[]}", browser);
RoundTrip("Inputs.SelectedEventArgs", "{\"filesData\":null,\"modifiedFilesData\":[]}", browser);
RoundTrip("Charts.SharedTooltipRenderEventArgs", "{\"text\":[\"one\",\"two\"],\"data\":[]}", browser);
RoundTrip("Charts.ZoomingEventArgs", "{\"axisCollection\":[{\"axisName\":\"x\",\"zoomFactor\":0.5,\"zoomPosition\":0.2}]}", browser);
RoundTrip("Data.DataManagerRequest", "{\"group\":[\"Team\"],\"select\":[\"Name\"],\"where\":[],\"sorted\":[],\"search\":[],\"aggregates\":[]}", browser);
foreach (var name in new[] { "Charts.Internal.IChartPoint", "Charts.Internal.IBubblePoint", "Charts.Internal.IFinancialPoint" })
    RoundTrip(name, "{\"s\":[{\"x\":2,\"y\":3}],\"r\":[{\"x\":1,\"y\":2,\"w\":3,\"h\":4}],\"xV\":1,\"yV\":\"NaN\",\"x\":\"Jan\",\"y\":null,\"t\":null,\"iX\":1,\"sI\":5,\"h\":8,\"l\":2,\"o\":3,\"c\":4,\"v\":9}", point);

// These are outgoing point DTOs: compare serialization of nonempty CLR values too.
foreach (var name in new[] { "Charts.Internal.IChartPoint", "Charts.Internal.IBubblePoint", "Charts.Internal.IFinancialPoint" })
{
    var type = Resolve(name);
    var value = Activator.CreateInstance(type)!;
    type.GetProperty("XValue")!.SetValue(value, 1d);
    type.GetProperty("YValue")!.SetValue(value, double.NaN);
    type.GetProperty("X")!.SetValue(value, "Jan");
    type.GetProperty("Text")!.SetValue(value, null);
    var locationType = Resolve("Charts.Internal.IChartInternalLocation");
    var locations = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(locationType))!;
    locations.Add(Activator.CreateInstance(locationType, 2d, 3d));
    type.GetProperty("SymbolLocations")!.SetValue(value, locations);
    type.GetProperty("Size")?.SetValue(value, 5d);
    type.GetProperty("High")?.SetValue(value, 8d);
    wire[name + "|outgoing"] = JsonNode.Parse(JsonSerializer.Serialize(value, type, point));
}

// Exercise the actual HTTP serializer settings without sending network requests.
foreach (var method in new[] { HttpMethod.Post, HttpMethod.Patch })
{
    var requestType = Resolve("Data.RequestOptions");
    var request = Activator.CreateInstance(requestType)!;
    requestType.GetProperty("Url")!.SetValue(request, "https://example.invalid/api");
    requestType.GetProperty("RequestMethod")!.SetValue(request, method);
    requestType.GetProperty("Data")!.SetValue(request, new { Label = "sample", Optional = (string?)null, Count = 0, Day = DayOfWeek.Monday });
    using var message = (HttpRequestMessage)Resolve("Data.HttpHandler").GetMethod("PrepareRequest")!.Invoke(null, new[] { request })!;
    wire["http|" + method.Method] = JsonNode.Parse(await message.Content!.ReadAsStringAsync());
}

// Exercise current generated metadata, falling back only in explicit historical mode.
var generated = new SortedDictionary<string, JsonNode?>(StringComparer.Ordinal);
void Geometry(string contextName, Type type, string json, string key)
{
    var contextType = assembly.GetType("Syncfusion.Blazor.Toolkit." + contextName);
    var context = contextType?.GetProperty("Default", BindingFlags.Public | BindingFlags.Static)?.GetValue(null) as JsonSerializerContext;
    if (!historical && (context is null || context.GetTypeInfo(type) is null))
        throw new InvalidOperationException($"Required generated metadata missing: {contextName} / {type}");
    var value = context is null ? JsonSerializer.Deserialize(json, type) : JsonSerializer.Deserialize(json, type, context);
    generated[key] = JsonNode.Parse(JsonSerializer.Serialize(value, type));
}
var location = Resolve("Charts.Internal.SymbolLocation");
var size = Resolve("Charts.Size");
Geometry("Charts.Internal.ChartInteropJsonContext", size, "{\"Width\":600,\"Height\":450}", "resize");
Geometry("Charts.Internal.ChartInteropJsonContext", typeof(List<>).MakeGenericType(size), "[{\"Width\":12,\"Height\":18}]", "axis-labels");
Geometry("Charts.Internal.ChartInteropJsonContext", typeof(List<>).MakeGenericType(location), "[{\"X\":12,\"Y\":18}]", "labels");
Geometry("Charts.Internal.ChartInteropJsonContext", typeof(Dictionary<,>).MakeGenericType(typeof(string), location), "{\"font\":{\"X\":12,\"Y\":18}}", "font-measurements");
Geometry("Charts.Internal.ChartInteropJsonContext", typeof(string[]), "[\"12\",\"13\"]", "characters");
foreach (var json in new[] { "{\"width\":\"400px\",\"height\":\"200px\",\"X\":12,\"Y\":24}", "{}", "{\"width\":null,\"height\":null,\"X\":null,\"Y\":null}" })
    Geometry("Popups.DialogInteropJsonContext", typeof(Dictionary<string, object>), json, "dialog|" + json);
var identity = new
{
    name = assembly.GetName().Name,
    version = assembly.GetName().Version?.ToString(),
    framework = assembly.GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>()?.FrameworkName
};
Console.WriteLine(JsonSerializer.Serialize(new { identity, surface, parameters, callbacks, wire, generated }, new JsonSerializerOptions { WriteIndented = true }));
return 0;
