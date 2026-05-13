using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Defines the core properties and configuration for chart legend rendering and behavior.
    /// </summary>
    /// <remarks>
    /// This interface is internal and not intended for direct use in user code.
    /// It provides the foundation for legend positioning, styling, and interactivity in chart components.
    /// </remarks>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public interface ILegendBase
    {
        /// <summary>
        /// Gets or sets the width of the legend in pixels or percentage.
        /// </summary>
        /// <value>A string representing the legend width (e.g., "100px", "50%").</value>
        public string Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the legend in pixels or percentage.
        /// </summary>
        /// <value>A string representing the legend height (e.g., "100px", "50%").</value>
        public string Height { get; set; }

        /// <summary>
        /// Gets or sets the padding around the entire legend element in pixels.
        /// </summary>
        /// <value>The padding value in pixels. Default is 0.</value>
        public double Padding { get; set; }

        /// <summary>
        /// Gets or sets the padding between individual legend items in pixels.
        /// </summary>
        /// <value>The item padding value in pixels. Default is 0.</value>
        public double ItemPadding { get; set; }

        /// <summary>
        /// Gets or sets the height of the legend symbol shape in pixels.
        /// </summary>
        /// <value>The shape height in pixels.</value>
        public double ShapeHeight { get; set; }

        /// <summary>
        /// Gets or sets the width of the legend symbol shape in pixels.
        /// </summary>
        /// <value>The shape width in pixels.</value>
        public double ShapeWidth { get; set; }

        /// <summary>
        /// Gets or sets the padding between the legend symbol and its associated text in pixels.
        /// </summary>
        /// <value>The shape padding value in pixels.</value>
        public double ShapePadding { get; set; }

        /// <summary>
        /// Gets or sets the position of the legend relative to the chart.
        /// </summary>
        /// <value>A <see cref="LegendPosition"/> enumeration value indicating the legend position.</value>
        public LegendPosition Position { get; set; }

        /// <summary>
        /// Gets or sets the horizontal or vertical alignment of legend items.
        /// </summary>
        /// <value>An <see cref="Alignment"/> enumeration value.</value>
        public Alignment Alignment { get; set; }

        /// <summary>
        /// Gets or sets the background color of the legend.
        /// </summary>
        /// <value>A string representing a color value (e.g., "#FF5733", "rgba(255, 87, 51, 0.8)").</value>
        public string Background { get; set; }

        /// <summary>
        /// Gets or sets the tab index for keyboard navigation.
        /// </summary>
        /// <value>The tab index value. Default is 0.</value>
        public double TabIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the legend allows toggling series visibility on item click.
        /// </summary>
        /// <value><c>true</c> if toggle visibility is enabled; otherwise, <c>false</c>. Default is <c>false</c>.</value>
        public bool ToggleVisibility { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the legend element.
        /// </summary>
        /// <value>A value between 0 and 1, where 0 is fully transparent and 1 is fully opaque. Default is 1.</value>
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether legend items should be highlighted on hover.
        /// </summary>
        /// <value><c>true</c> if highlight on hover is enabled; otherwise, <c>false</c>. Default is <c>false</c>.</value>
        public bool EnableHighlight { get; set; }
    }

    /// <summary>
    /// Defines methods for legend measurement and positioning calculations.
    /// </summary>
    /// <remarks>
    /// This interface is internal and not intended for direct use in user code.
    /// It provides methods for calculating legend bounds and render points based on available space and content.
    /// </remarks>
    public interface ILegendMethods
    {
        /// <summary>
        /// Calculates and updates the render point for a legend item based on available space and previous item positioning.
        /// </summary>
        /// <param name="legendOption">The legend item configuration.</param>
        /// <param name="start">The starting position for the render point calculation.</param>
        /// <param name="textPadding">The padding around the legend item text in pixels.</param>
        /// <param name="prevLegend">The previous legend item's configuration for sequential positioning.</param>
        /// <param name="count">The index of the current legend item.</param>
        /// <param name="firstLegend">The index of the first legend item in the current row or column.</param>
        public void GetRenderPoint(LegendOption legendOption, ChartEventLocation start, double textPadding, LegendOption prevLegend, int count, int firstLegend);

        /// <summary>
        /// Calculates the overall bounds of the legend container based on available space and content dimensions.
        /// </summary>
        /// <param name="availableSize">The available space for the legend.</param>
        /// <param name="rect">The rectangle defining the legend container bounds.</param>
        /// <param name="maxLabelSize">The maximum size of a legend label text element.</param>
        public void GetLegendBounds(Size availableSize, Rect rect, Size maxLabelSize);
    }

    /// <summary>
    /// Represents the visual components and metadata for a single legend symbol entry.
    /// </summary>
    /// <remarks>
    /// This class encapsulates a legend item's symbol shapes, associated text, optional custom template, and index.
    /// It is used internally for rendering and managing individual legend items within the legend container.
    /// </remarks>
    public class LegendSymbols
    {
        /// <summary>
        /// Gets or sets the primary symbol (shape) for the legend item.
        /// </summary>
        /// <value>A <see cref="SymbolOptions"/> instance containing the primary symbol configuration.</value>
        public SymbolOptions FirstSymbol { get; set; } = new SymbolOptions();

        /// <summary>
        /// Gets or sets the text options associated with the legend item label.
        /// </summary>
        /// <value>A <see cref="TextOptions"/> instance containing text rendering options.</value>
        public TextOptions TextOption { get; set; } = new TextOptions();

        /// <summary>
        /// Gets or sets an optional secondary symbol for the legend item.
        /// </summary>
        /// <value>A <see cref="SymbolOptions"/> instance, or <c>null</c> if no secondary symbol is required.</value>
        public SymbolOptions SecondSymbol { get; set; } = null!;

        /// <summary>
        /// Gets or sets the zero-based index of this legend symbol within the legend items collection.
        /// </summary>
        /// <value>The index of the legend item.</value>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets an optional custom Blazor render fragment for the legend item.
        /// </summary>
        /// <value>A <see cref="RenderFragment"/>, or <c>null</c> if the default template is used.</value>
        public RenderFragment? Template { get; set; }
    }
}
