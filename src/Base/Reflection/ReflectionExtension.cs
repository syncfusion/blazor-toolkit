using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// An extension class which provides various extension methods to reflect the data from an object.
    /// </summary>
    /// <exclude/>
    public static class ReflectionExtension
    {
        /// <summary>
        /// Returns the property value of a specified object of any type includes static types, <see cref="DynamicObject"/> and <see cref="ExpandoObject"/> types.
        /// </summary>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="reflectComplexProperty">true, if need to reflect complex property. </param>
        /// <returns>The property value of the specified object. Also, returns null if <paramref name="obj"/> and <paramref name="propertyName"/> is null or empty.</returns>
        /// <remarks>For accessing complex/nested property value, given the propertyName with field names delimited by dot(.).</remarks>
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
        /// Returns the property value of a specified object of type <see cref="DynamicObject"/> or <see cref="ExpandoObject"/>.
        /// </summary>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="reflectComplexProperty">true, if need to reflect complex property. </param>
        /// <returns>The property value of the specified object.</returns>
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
        /// Returns the property value of a specified object of type <see cref="DynamicObject"/>.
        /// </summary>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="reflectComplexProperty">true, if need to reflect complex property. </param>
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
        /// Returns the property value of a specified object of type <see cref="IDynamicMetaObjectProvider"/>.
        /// </summary>
        /// <param name="obj">The object implementing <see cref="IDynamicMetaObjectProvider"/> whose property value will be returned.</param>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <param name="reflectComplexProperty">true, if need to reflect complex property. </param>
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
        /// Returns the property value of a specified object of type <see cref="ExpandoObject"/>.
        /// </summary>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="reflectComplexProperty">true, if need to reflect complex property. </param>
        /// <returns>The property value of the specified object.</returns>
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
        /// Creates an instance of the specified type using that type's parameterless constructor.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        /// <param name="createSubtypes">true, if nested properties also should be initialized with instance.</param>
        /// <returns>A reference to the newly created object.</returns>
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
    /// Defines the data member binder for getting dynamic object property.
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
    /// Defines the data member binder for setting dynamic object property.
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
