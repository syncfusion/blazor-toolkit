using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Spinner
{
    /// <summary>
    /// Contains the member and parameter definitions for the <see cref="SfSpinner"/> component.
    /// </summary>
    /// <remarks>
    /// This partial class defines all public parameters and properties that can be used to configure
    /// and control the behavior of the Spinner component.
    /// </remarks>
    public partial class SfSpinner : SfBaseComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the label text displayed below the spinner animation.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> that specifies the label for the spinner. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// This property provides descriptive text for the spinner, enhancing user experience and providing context.
        /// If not provided, the default "Loading" aria-label is automatically applied for accessibility compliance.
        /// This improves accessibility for screen readers and assistive technologies.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner Label="Processing..." />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the CSS class names to be appended to the root element of the spinner.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing one or more CSS class names separated by spaces. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// This property allows you to apply custom CSS classes to the spinner for customized appearance and styling.
        /// Multiple classes should be space-separated. Classes are dynamically added and removed as needed.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner CssClass="custom-spinner large-spinner" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? CssClass { get; set; }

        /// <summary>
        /// Gets or sets the child content to be rendered within the spinner container.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> representing the nested content. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// This parameter allows you to define nested content within the spinner component, such as child components or HTML elements.
        /// The child content is rendered inside the spinner container using a cascading value for communication.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner Visible="true">
        ///     <div>Additional content here</div>
        /// </SfSpinner>
        /// ]]></code>
        /// </example>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the spinner component is visible on the screen.
        /// </summary>
        /// <value>
        /// <see langword="true"/> to display the spinner; <see langword="false"/> to hide it. The default value is <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// When set to <see langword="true"/>, the spinner is displayed and rendered. When set to <see langword="false"/>, it is hidden from view.
        /// This property supports two-way binding via the <see cref="VisibleChanged"/> event callback.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner @bind-Visible="@isLoading" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the event callback invoked when the <see cref="Visible"/> property changes.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{TValue}"/> that is invoked when the spinner's visibility state is modified.
        /// </value>
        /// <remarks>
        /// This property is used to support two-way data binding with the <see cref="Visible"/> parameter using the <c>@bind-Visible</c> directive.
        /// When the visibility changes, this callback is invoked to update the parent component's state.
        /// </remarks>
        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

        /// <summary>
        /// Gets or sets the size of the spinner's animation element.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> specifying the size. Valid values include pixel measurements (e.g., "40px"), percentages (e.g., "100%"), or <see langword="null"/> for default. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// The size determines both the width and height of the spinner's animation element, maintaining a square aspect ratio.
        /// If not specified, the component uses a default size appropriate for the chosen theme.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner Size="50px" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? Size { get; set; }

        /// <summary>
        /// Gets or sets the CSS z-index value for the spinner, controlling its stacking order relative to other elements.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the z-index value (e.g., "1000", "auto"). The default value is <c>"auto"</c>.
        /// </value>
        /// <remarks>
        /// A higher z-index value places the spinner in front of elements with lower z-index values. This is particularly useful 
        /// for overlay or modal scenarios where the spinner needs to appear above other content.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner ZIndex="1000" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string ZIndex { get; set; } = Auto;

        /// <summary>
        /// Gets or sets the thickness of the spinner's stroke line.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> specifying the thickness value. Valid values include pixel measurements (e.g., "4px") or percentages. The default value is <see langword="null"/>, which applies a 4px thickness.
        /// </value>
        /// <remarks>
        /// The thickness determines the width of the spinner's animation line. This property helps customize the visual appearance of the spinner to match your application's design.
        /// If not specified, the component uses a default thickness appropriate for the chosen theme.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfSpinner Thickness="6px" Visible="true" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? Thickness { get; set; }

        #endregion
    }
}
