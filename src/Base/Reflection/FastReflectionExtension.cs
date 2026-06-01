using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// Provides extension methods to create cached property accessors for faster reflection,
    /// using compiled delegates instead of runtime reflection on each get operation.
    /// </summary>
    /// <exclude/>
    public static class FastReflectionExtension
    {
        /// <summary>
        /// Creates and returns an <see cref="IPropertyAccessor"/> that stores the property accessor of a specified property.
        /// </summary>
        /// <param name="propertyInfo">The metadata that provides access to the property.</param>
        /// <returns>An <see cref="IPropertyAccessor"/> that can read the property value from an object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="propertyInfo"/> is null.</exception>
        /// <remarks>This method throws <see cref="ArgumentNullException"/> if <paramref name="propertyInfo"/> is null.</remarks>
        public static IPropertyAccessor CreateAccessor(PropertyInfo propertyInfo)
        {
            ArgumentNullException.ThrowIfNull(propertyInfo);
            return (IPropertyAccessor)Activator.CreateInstance(typeof(PropertyAccessor<,>).MakeGenericType(propertyInfo?.DeclaringType, propertyInfo.PropertyType), propertyInfo);
        }

        /// <summary>
        /// Creates and returns an <see cref="IPropertyAccessor"/> that stores the property accessor of a specified property.
        /// </summary>
        /// <param name="objectType">The type that declares the property.</param>
        /// <param name="propertyName">The name of the public property to access.</param>
        /// <returns>An <see cref="IPropertyAccessor"/> that can read the property value from an object.</returns>
        /// <remarks>
        /// If <paramref name="propertyName"/> is <c>null</c> or empty, a no-op accessor is returned.
        /// </remarks>
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
    /// Defines a cached property accessor that reads values from an object using a compiled delegate.
    /// </summary>
    /// <exclude/>
    public interface IPropertyAccessor : IDisposable
    {
        /// <summary>
        /// Gets the metadata that provides access to the reflected property.
        /// </summary>
        PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Returns the property value of the specified object.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <returns>The property value of the specified object.</returns>
        object GetValue(object obj);
    }

    /// <summary>
    /// Implements <see cref="IPropertyAccessor"/> by compiling a strongly-typed delegate
    /// for reading a property value, avoiding repeated runtime reflection overhead.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <typeparam name="TObject">The type that declares the property.</typeparam>
    /// <remarks>
    /// Use <see cref="FastReflectionExtension.CreateAccessor(PropertyInfo)"/> to create an accessor based on type metadata.
    /// </remarks>
    internal class PropertyAccessor<TObject, TValue> : IPropertyAccessor
    {
        private Func<TObject, TValue>? _getMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAccessor{TObject, TValue}"/> class
        /// and compiles the getter delegate for the specified property.
        /// </summary>
        /// <param name="propertyInfo">The metadata of the property to reflect.</param>
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

        /// <summary>
        /// Returns the property value from the specified source object.
        /// </summary>
        /// <param name="source">The instance to read the property from.</param>
        /// <returns>The property value, or <see langword="null"/> if the getter is unavailable.</returns>
        public object GetValue(object source)
        {
            return _getMethod is null ? null : _getMethod((TObject)source);
        }

         /// <summary>
        /// Releases references to the compiled delegate and property metadata.
        /// </summary>
        public void Dispose()
        {
            PropertyInfo = null;
            _getMethod = null;
        }

        /// <summary>
        /// Gets the metadata that provides access to the reflected property.
        /// </summary>
        [AllowNull]
        public PropertyInfo PropertyInfo { get; private set; }
    }
}
