using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents an SVG pattern element component for defining reusable shape patterns within data visualizations.
    /// </summary>
    /// <remarks>
    /// The <see cref="SvgPattern"/> component allows composition of multiple shape options into a single pattern definition that can be referenced and applied to multiple SVG elements. Supports dynamic shape rendering based on type.
    /// </remarks>
    public partial class SvgPattern
    {
        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for the pattern element.
        /// </summary>
        /// <value>
        /// A string representing the SVG pattern's ID attribute. Required. Default: <see langword="null"/>.
        /// </value>
        [Parameter]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the height of the pattern tile.
        /// </summary>
        /// <value>
        /// A positive numeric value representing the pattern height in SVG units. Default: <c>0</c>.
        /// </value>
        [Parameter]
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the width of the pattern tile.
        /// </summary>
        /// <value>
        /// A positive numeric value representing the pattern width in SVG units. Default: <c>0</c>.
        /// </value>
        [Parameter]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the coordinate system for the pattern (e.g., "userSpaceOnUse" or "objectBoundingBox").
        /// </summary>
        /// <value>
        /// A string specifying the SVG pattern units coordinate system. Default: <see langword="null"/>.
        /// </value>
        [Parameter]
        public string PatternUnits { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of shape objects to render within the pattern.
        /// </summary>
        /// <value>
        /// A list of shape option objects (typically <see cref="RectOptions"/>, <see cref="PathOptions"/>, or <see cref="EllipseOptions"/>).
        /// </value>
        /// <remarks>
        /// Each shape is dynamically rendered based on its type. Supported types include
        /// <see cref="RectOptions"/>, <see cref="PathOptions"/>, and <see cref="EllipseOptions"/>.
        /// </remarks>
        [Parameter]
        public List<object>? ShapeOptions { get; set; }

        #endregion
    }
}