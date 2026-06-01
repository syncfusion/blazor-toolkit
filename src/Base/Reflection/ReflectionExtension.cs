using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Provides extension methods for reflecting property values from static, dynamic,
    /// and expando objects, including support for nested (dot-delimited) property paths.
    /// </summary>
    /// <exclude/>
    public static class ReflectionExtension
    {
        /// <summary>
        /// Returns the property value of a specified object of any type, including static types,
        /// <see cref="DynamicObject"/>, and <see cref="ExpandoObject"/> instances.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <param name="reflectComplexProperty">
        /// <see langword="true"/> to resolve nested properties delimited by dot (<c>.</c>); otherwise <see langword="false"/>.
        /// </param>
        /// <returns>
        /// The property value of the specified object, or <see langword="null"/> if
        /// <paramref name="obj"/> or <paramref name="propertyName"/> is <see langword="null"/> or empty.
        /// </returns>
        /// <remarks>
        /// For accessing complex or nested property values, provide the <paramref name="propertyName"/>
        /// with field names delimited by a dot (for example, <c>"Address.City"</c>).
        /// </remarks>
        public static object? GetValue(object obj, string propertyName, bool reflectComplexProperty = true)
        {
            if (string.IsNullOrEmpty(propertyName) || obj is null)
            {
                return null;
            }

            if (!reflectComplexProperty || !propertyName.Contains('.', StringComparison.InvariantCulture))
            {
                return GetValueForDirectProperty(obj, propertyName);
            }

            string[] splits = propertyName.Split('.');
            object? value = obj;
            for (int i = 0; i < splits.Length; i++)
            {
                value = GetValueForDirectProperty(value, splits[i]);
                if (value is null)
                {
                    break;
                }
            }

            return value;
        }

        private static object? GetValueForDirectProperty(object obj, string propertyName)
        {
            Type dataObjectType = obj.GetType();
            bool isDynamic = typeof(IDynamicMetaObjectProvider).IsAssignableFrom(dataObjectType);
            if (isDynamic)
            {
                return GetValueFromIDynamicMetaObject(obj, propertyName);
            }
            PropertyInfo? pinfo = dataObjectType.GetProperty(propertyName);
            return pinfo is not null ? pinfo.GetValue(obj) : (dataObjectType.GetField(propertyName)?.GetValue(obj));
        }

        /// <summary>
        /// Returns the property value of a specified dynamic or expando object.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <param name="reflectComplexProperty">
        /// <see langword="true"/> to resolve nested properties delimited by dot (<c>.</c>); otherwise <see langword="false"/>.
        /// </param>
        /// <returns>
        /// The property value of the specified object, or <see langword="null"/> if <paramref name="obj"/> is <see langword="null"/>
        /// or the property cannot be resolved.
        /// </returns>
        /// <remarks>
        /// This method dispatches to <see cref="GetValueFromExpandoObject"/>, <see cref="GetValueFromDynamicObject"/>,
        /// or <see cref="GetValueFromDynamicMetaObjectProvider"/> based on the runtime type of <paramref name="obj"/>.
        /// </remarks>
        public static object? GetValueFromIDynamicMetaObject(object obj, string propertyName, bool reflectComplexProperty = false)
        {
            if (obj is ExpandoObject expandoObject)
            {
                return GetValueFromExpandoObject(expandoObject!, propertyName, reflectComplexProperty);
            }

            if (obj is DynamicObject dynamicObject)
            {
                return GetValueFromDynamicObject(dynamicObject, propertyName, reflectComplexProperty);
            }

            if (obj is IDynamicMetaObjectProvider dynamicMetaObjectProvider)
            {
                return GetValueFromDynamicMetaObjectProvider(dynamicMetaObjectProvider, propertyName, reflectComplexProperty);
            }

            throw new NotImplementedException(obj?.GetType().Name ?? "obj is null");
        }

        /// <summary>
        /// Returns the property value of a specified <see cref="DynamicObject"/>.
        /// </summary>
        /// <param name="obj">The dynamic object whose property value will be returned.</param>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <param name="reflectComplexProperty">
        /// <see langword="true"/> to resolve nested properties delimited by dot (<c>.</c>); otherwise <see langword="false"/>.
        /// </param>
        /// <returns>The property value of the specified object.</returns>
        public static object? GetValueFromDynamicObject(DynamicObject obj, string propertyName, bool reflectComplexProperty = false)
        {
            if (obj is null || string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            object? value;

            if (!reflectComplexProperty || !propertyName.Contains('.', StringComparison.InvariantCulture))
            {
                _ = obj.TryGetMember(new DataMemberBinder(propertyName, false), out value);
            }
            else
            {
                string[] splits = propertyName.Split('.');
                value = obj;
                for (int i = 0; i < splits.Length; i++)
                {
                    if (i is 0)
                    {
                        if (!obj.TryGetMember(new DataMemberBinder(splits[i], false), out value))
                        {
                            value = null;
                        }
                    }
                    else
                    {
                        value = GetValueForDirectProperty(value, splits[i]);
                    }
                    if (value is null)
                    {
                        return null;
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// Returns the property value of a specified object implementing <see cref="IDynamicMetaObjectProvider"/>.
        /// </summary>
        /// <param name="obj">The object implementing <see cref="IDynamicMetaObjectProvider"/> whose property value will be returned.</param>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <param name="reflectComplexProperty">
        /// <see langword="true"/> to resolve nested properties delimited by dot (<c>.</c>); otherwise <see langword="false"/>.
        /// </param>
        /// <returns>The property value of the specified object.</returns>
        public static object? GetValueFromDynamicMetaObjectProvider(IDynamicMetaObjectProvider obj, string propertyName, bool reflectComplexProperty = false)
        {
            if (obj is null || string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            object? value = null;

            if (!reflectComplexProperty || !propertyName.Contains('.', StringComparison.InvariantCulture))
            {
                CallSiteBinder binder = Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, propertyName, obj.GetType(),
                    [CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)]);
                CallSite<Func<CallSite, object, object>> callSite = CallSite<Func<CallSite, object, object>>.Create(binder);
                value = callSite.Target(callSite, obj);
            }
            else
            {
                string[] splits = propertyName.Split('.');
                value = obj;
                for (int i = 0; i < splits.Length; i++)
                {
                    if (i is 0)
                    {
                        CallSiteBinder binder = Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, splits[i], obj.GetType(),
                            [CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)]);
                        CallSite<Func<CallSite, object, object>> callSite = CallSite<Func<CallSite, object, object>>.Create(binder);
                        value = callSite.Target(callSite, obj);
                    }
                    else
                    {
                        value = GetValueForDirectProperty(value, splits[i]);
                    }
                    if (value is null)
                    {
                        return null;
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// Returns the property value of a specified <see cref="ExpandoObject"/> (or any <see cref="IDictionary{TKey, TValue}"/> with string keys).
        /// </summary>
        /// <param name="obj">The dictionary whose property value will be returned.</param>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <param name="reflectComplexProperty">
        /// <see langword="true"/> to resolve nested properties delimited by dot (<c>.</c>); otherwise <see langword="false"/>.
        /// </param>
        /// <returns>The property value of the specified object, or <see langword="null"/> if the key is not found.</returns>
        public static object? GetValueFromExpandoObject(IDictionary<string, object> obj, string propertyName, bool reflectComplexProperty = false)
        {
            if (obj is null || string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            object? value = null;
            if (!reflectComplexProperty || !propertyName.Contains('.', StringComparison.InvariantCulture))
            {
                _ = obj.TryGetValue(propertyName, out value);
            }
            else
            {
                string[] splits = propertyName.Split('.');
                for (int i = 0; i < splits.Length; i++)
                {
                    if (i is 0)
                    {
                        if (!obj.TryGetValue(splits[i], out value))
                        {
                            value = null;
                        }
                    }
                    else
                    {
                        value = GetValueForDirectProperty(value!, splits[i]);
                    }
                    if (value is null)
                    {
                        return null;
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// Creates an instance of the specified type using its first available constructor.
        /// Parameter values are initialized with default instances using recursion where necessary.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        /// <param name="createsubtypes">
        /// <see langword="true"/> to also initialize writable, non-primitive nested properties with new instances; otherwise <see langword="false"/>.
        /// </param>
        /// <returns>
        /// A reference to the newly created object, or <see langword="null"/> if creation fails due to an exception.
        /// </returns>
        /// <remarks>
        /// When <paramref name="createsubtypes"/> is <see langword="true"/>, nested properties of interface or complex types are also
        /// recursively initialized to facilitate deep-copy or factory scenarios.
        /// </remarks>
        public static object? TryCreateInstance(Type type, bool createSubtypes = false)
        {
            try
            {
                ConstructorInfo[]? constructors = type?.GetConstructors();
                ConstructorInfo? constructor = constructors?.FirstOrDefault();
                object obj;
                if (constructor is not null)
                {
                    ParameterInfo[] parameters = constructor.GetParameters();
                    object[] parameterValues = new object[parameters.Length];

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (parameters[i].ParameterType == typeof(string))
                        {
                            parameterValues[i] = string.Empty;
                        }
                        else if (parameters[i].ParameterType.IsInterface)
                        {
                            parameterValues[i] = Activator.CreateInstance(type);
                        }
                        else if (parameters[i].ParameterType == type)
                        {
                            // Use LINQ to find the first suitable non-recursive parameter type from any constructor.
                            ParameterInfo validConstructorParameter = constructors
                                .SelectMany(c => c.GetParameters()) // Flatten all parameters from all constructors
                                .FirstOrDefault(p =>
                                { 
                                    // Only consider parameters that are not the recursive type
                                    if (p.ParameterType != type)
                                    {
                                        // Attempt to create an instance. If successful, this parameter is suitable.
                                        //true signifies that the ParameterInfo(p) currently being evaluated meets the 
                                        //criteria(it's not the recursive type, and its instance can be successfully created).
                                        return true;
                                    }
                                    //false signifies that the ParameterInfo(p) does not meet the criteria.
                                    return false; // Not a suitable parameter if it's the recursive type
                                });

                            parameterValues[i] = validConstructorParameter is not null ? TryCreateInstance(validConstructorParameter.ParameterType) :
                                Activator.CreateInstance(parameters[i].ParameterType);
                        }
                        else
                        {
                            parameterValues[i] = TryCreateInstance(parameters[i].ParameterType);
                        }
                    }
                    obj = constructor.Invoke(parameterValues);
                }
                else
                {
                    obj = Activator.CreateInstance(type);
                }
                if (!createSubtypes || obj is null)
                {
                    return obj;
                }

                PropertyInfo[] propertiesInfos = obj.GetType().GetProperties();
                foreach (PropertyInfo propertyInfo in propertiesInfos)
                {
                    bool isDynamicProp = typeof(IDynamicMetaObjectProvider).IsAssignableFrom(propertyInfo.PropertyType);
                    if (isDynamicProp || propertyInfo.PropertyType.IsPrimitive || propertyInfo.PropertyType.IsEnum ||
                        propertyInfo.PropertyType.IsAbstract || !propertyInfo.CanWrite || propertyInfo.SetMethod is null)
                    {
                        continue;
                    }

                    object? propertyValue = TryCreateInstance(propertyInfo.PropertyType, createSubtypes);
                    if (propertyValue is not null)
                    {
                        propertyInfo.SetValue(obj, propertyValue);
                    }
                }
                return obj;
            }
            catch
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Provides a dynamic binder for retrieving member values from <see cref="DynamicObject"/> instances.
    /// </summary>
    /// <exclude/>
    internal class DataMemberBinder(string name, bool ignoreCase) : GetMemberBinder(name, ignoreCase)
    {
        public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Provides a dynamic binder for setting member values on <see cref="DynamicObject"/> instances.
    /// </summary>
    /// <exclude/>
    internal class DataSetMemberBinder(string name, bool ignoreCase) : SetMemberBinder(name, ignoreCase)
    {
        public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
