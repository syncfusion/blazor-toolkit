using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// An extension class which provides various extension methods to reflect the data from an object by storing the accessors for faster reflection.
    /// </summary>
    /// <exclude/>
    public static class FastReflectionExtension
    {
        /// <summary>
        /// Creates and returns <see cref="IPropertyAccessor"/> that stores the property accessor of a specfied property.
        /// </summary>
        /// <param name="propertyInfo">The info that provides access to property metadata.</param>
        /// <returns><see cref="IPropertyAccessor"/></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyInfo"/> is null.</exception>
        /// <remarks>This method throws <see cref="ArgumentNullException"/> if <paramref name="propertyInfo"/> is null.</remarks>
        public static IPropertyAccessor CreateAccessor(PropertyInfo propertyInfo)
        {
            ArgumentNullException.ThrowIfNull(propertyInfo);
            return (IPropertyAccessor)Activator.CreateInstance(typeof(PropertyAccessor<,>).MakeGenericType(propertyInfo?.DeclaringType, propertyInfo.PropertyType), propertyInfo);
        }

        /// <summary>
        /// Creates and returns <see cref="IPropertyAccessor"/> that stores the property accessor of a specfied property.
        /// </summary>
        /// <param name="objectType">Type of object.</param>
        /// <param name="propertyName">The string containing the name of the public property.</param>
        /// <returns><see cref="IPropertyAccessor"/></returns>
        /// <remarks>If <paramref name="propertyName"/> is null, empty, or the property cannot be found on <paramref name="objectType"/>,
        /// this method returns a non-functional accessor of type <c>PropertyAccessor&lt;object, object&gt;</c> whose `GetValue` returns null.</remarks>
        public static IPropertyAccessor CreateAccessor(Type objectType, string propertyName)
        {
            PropertyInfo? propertyInfo = null;
            if (!string.IsNullOrEmpty(propertyName))
            {
                propertyInfo = objectType?.GetProperty(propertyName);
            }
            //Adding null check and returning null setter for chart alone. Chart passes empty property names for reflection.
            if (propertyInfo is null)
            {
                return (IPropertyAccessor)Activator.CreateInstance(typeof(PropertyAccessor<,>).MakeGenericType(typeof(object), typeof(object)), propertyInfo);
            }
            return CreateAccessor(propertyInfo);
        }
    }

    /// <summary>
    /// Interface stores the property accessor for static types.
    /// </summary>
    /// <exclude/>
    public interface IPropertyAccessor : IDisposable
    {
        /// <summary>
        /// Gets or sets the info that provides access to property metadata.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Returns the property value of a specified object.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <returns>The property value of the specified object.</returns>
        object GetValue(object obj);
    }

    /// <summary>
    /// A class that holds get and set accessor of a property which is used to retrive the field values using reflection.
    /// </summary>
    /// <typeparam name="TObject">Type of object.</typeparam>
    /// <typeparam name="TValue">Type of property. </typeparam>
    /// <remarks>
    /// Use <see cref="FastReflectionExtension.CreateAccessor(PropertyInfo)"/> to create accessor based on type.
    /// </remarks>
    internal class PropertyAccessor<TObject, TValue> : IPropertyAccessor
    {
        private Func<TObject, TValue>? _getMethod;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            Init();
        }

        private void Init()
        {
            if (PropertyInfo is null)
            {
                return;
            }

            MethodInfo? methodInfo = PropertyInfo.GetGetMethod(true);
            if (methodInfo is not null)
            {
                _getMethod = (Func<TObject, TValue>)Delegate.CreateDelegate(typeof(Func<TObject, TValue>), methodInfo);
            }
        }

        public object GetValue(object source)
        {
            return _getMethod is null ? null : _getMethod((TObject)source);
        }

        public void Dispose()
        {
            PropertyInfo = null;
            _getMethod = null;
        }

        [AllowNull]
        public PropertyInfo PropertyInfo { get; private set; }
    }
}
