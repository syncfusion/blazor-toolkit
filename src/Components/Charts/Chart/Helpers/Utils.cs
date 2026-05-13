using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides extension methods for Blazor component rendering and cascading value creation.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Creates a cascading value component in the render tree with the specified value and child content.
        /// </summary>
        /// <typeparam name="TValue">The type of the cascading value.</typeparam>
        /// <param name="componentBase">The component instance used to invoke the extension method.</param>
        /// <param name="builder">The render tree builder used to add components to the render tree.</param>
        /// <param name="componentSequence">The sequence number for the cascading value component in the render tree.</param>
        /// <param name="valueAttributeSequence">The sequence number for the Value attribute.</param>
        /// <param name="cascadingValue">The value to cascade down to child components.</param>
        /// <param name="childContentSequence">The sequence number for the ChildContent attribute.</param>
        /// <param name="childContent">The child content to render inside the cascading value component.</param>
        public static void CreateCascadingValue<TValue>(this ComponentBase componentBase, RenderTreeBuilder builder, int componentSequence, int valueAttributeSequence, TValue cascadingValue, int childContentSequence, RenderFragment childContent)
        {
            _ = componentBase;
            builder.OpenComponent<CascadingValue<TValue>>(componentSequence);
            builder.AddAttribute(valueAttributeSequence, "Value", cascadingValue);
            builder.AddAttribute(childContentSequence, "ChildContent", childContent);
            builder.CloseComponent();
        }
    }
}
