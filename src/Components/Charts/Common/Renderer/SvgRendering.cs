using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Reflection;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>Provides utilities for rendering SVG elements in Blazor components via RenderTreeBuilder.</summary>
    /// <remarks>
    /// This class manages collections of SVG elements (text, paths, rects, circles, etc.) and orchestrates 
    /// their rendering through the Blazor RenderTreeBuilder API. It caches property reflection data for 
    /// performance optimization and maintains element collections for later reference and cleanup.
    /// </remarks>
    public class SvgRendering
    {
        #region Constants
        const string ELEMENT_GROUP = "g";
        const string ELEMENT_DEFS = "defs";
        const string ELEMENT_CLIP_PATH = "clipPath";
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the sequence number for RenderTreeBuilder operations.
        /// </summary>
        /// <remarks>This is incremented for each rendering operation to maintain unique sequence ordering. Instance-level to ensure deterministic sequences per render cycle.</remarks>
        internal int Seq { get; set; }

        /// <summary>
        /// Gets or sets the collection of rendered SVG text elements.
        /// </summary>
        internal List<SvgText>? TextElementList { get; set; } = new List<SvgText>();

        /// <summary>
        /// Gets or sets the collection of rendered SVG path elements.
        /// </summary>
        internal List<SvgPath>? PathElementList { get; set; } = new List<SvgPath>();

        /// <summary>
        /// Gets or sets the collection of rendered SVG ellipse elements.
        /// </summary>
        internal List<SvgEllipse>? EllipseElementList { get; set; } = new List<SvgEllipse>();

        /// <summary>
        /// Gets or sets the collection of rendered SVG rectangle elements.
        /// </summary>
        internal List<SvgRect>? RectElementList { get; set; } = new List<SvgRect>();

        /// <summary>
        /// Gets or sets the collection of rendered SVG image elements.
        /// </summary>
        internal List<SvgImage>? ImageCollection { get; set; } = new List<SvgImage>();

        /// <summary>
        /// Gets or sets the collection of rendered SVG circle elements.
        /// </summary>
        internal List<SvgCircle>? CircleCollection { get; set; } = new List<SvgCircle>();

        /// <summary>
        /// Gets or sets the collection of group element references.
        /// </summary>
        internal List<ElementReference>? GroupCollection { get; set; } = new List<ElementReference>();
        #endregion

        #region Internal Methods

        /// <summary>
        /// Resets the sequence counter for a new render cycle.
        /// </summary>
        /// <remarks>Call this method at the start of each BuildRenderTree to ensure deterministic, contiguous sequence numbers for Blazor's diff algorithm.</remarks>
        internal void ResetSequence()
        {
            Seq = 0;
        }

        /// <summary>
        /// Clears and reinitializes all element collections.
        /// </summary>
        /// <remarks>Call this method to reset the rendering state before starting a new render cycle.</remarks>
        internal void RefreshElementList()
        {
            Seq = 0;
            TextElementList = new List<SvgText>();
            PathElementList = new List<SvgPath>();
            EllipseElementList = new List<SvgEllipse>();
            RectElementList = new List<SvgRect>();
            ImageCollection = new List<SvgImage>();
            CircleCollection = new List<SvgCircle>();
            GroupCollection = new List<ElementReference>();
        }

        /// <summary>
        /// Renders an SVG text element.
        /// </summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="textOptions">Options for configuring the text element.</param>
        /// <param name="ariaHidden">The aria-hidden attribute value. Default: <c>"true"</c>.</param>
        /// <param name="clipPath">The clip-path reference identifier. Default: empty string.</param>
        internal void RenderText(RenderTreeBuilder renderTreeBuilder, TextOptions textOptions, string ariaHidden = "true", string clipPath = "")
        {
            renderTreeBuilder.OpenComponent<SvgText>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(textOptions));
            renderTreeBuilder.AddAttribute(Seq++, "AriaHidden", ariaHidden);
            renderTreeBuilder.AddAttribute(Seq++, ELEMENT_CLIP_PATH, clipPath);
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { TextElementList?.Add((SvgText)ins); });
            renderTreeBuilder.CloseComponent();
        }

        /// <summary>
        /// Renders an SVG rectangle element.
        /// </summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="rectOptions">Options for configuring the rectangle element.</param>
        internal void RenderRect(RenderTreeBuilder renderTreeBuilder, RectOptions rectOptions)
        {
            renderTreeBuilder.OpenComponent<SvgRect>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(rectOptions));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { RectElementList?.Add((SvgRect)ins); });
            renderTreeBuilder.CloseComponent();
        }

        /// <summary>
        /// Renders an SVG path element with additional styling and data attributes.
        /// </summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="pathOptions">Options for configuring the path element.</param>
        /// <param name="pointerClass">CSS class for pointer events.</param>
        /// <param name="data">Data point identifier. Default: empty string.</param>
        internal void RenderPath(RenderTreeBuilder renderTreeBuilder, PathOptions pathOptions, string pointerClass = "", string data = "")
        {
            renderTreeBuilder.OpenComponent<SvgPath>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(pathOptions));
            if (!string.IsNullOrEmpty(pointerClass))
            {
                renderTreeBuilder.AddAttribute(Seq++, "Class", pointerClass);
            }
            renderTreeBuilder.AddAttribute(Seq++, "DataPoint", data);
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { PathElementList?.Add((SvgPath)ins); });
            renderTreeBuilder.CloseComponent();
        }

        /// <summary>
        /// Renders an SVG path element with manual path options.
        /// </summary>
        internal SvgPath RenderPath(RenderTreeBuilder renderTreeBuilder, PathOptions pathOptions)
        {
            SvgPath reference = null!;
            renderTreeBuilder.OpenComponent<SvgPath>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(pathOptions));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { PathElementList?.Add((SvgPath)ins); });
            renderTreeBuilder.CloseComponent();
            return reference;
        }

        /// <summary>Opens an SVG group element with optional styling and accessibility attributes.</summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="id">The unique identifier for the group.</param>
        /// <param name="transform">The SVG transform attribute. Default: empty string.</param>
        /// <param name="clippath">The clip-path reference identifier. Default: empty string.</param>
        /// <param name="svgStyleClass">CSS styles. Default: empty string.</param>
        /// <param name="tabIndex">Tab index for keyboard navigation. Default: empty string.</param>
        /// <param name="accessText">Accessibility label (aria-label). Default: empty string.</param>
        /// <param name="ariaHidden">The aria-hidden attribute value. Default: empty string.</param>
        /// <param name="dataPoints">Data point identifier. Default: empty string.</param>
        /// <param name="role">The ARIA role. Default: empty string.</param>
        /// <param name="ariaPressed">The aria-pressed state. Default: <c>"false"</c>.</param>
        internal void OpenGroupElement(RenderTreeBuilder renderTreeBuilder, string id, string transform = "", string clippath = "", string svgStyleClass = "", string tabIndex = "", string accessText = "", string ariaHidden = "", string dataPoints = "", string role = "", string ariaPressed = "false")
        {
            renderTreeBuilder.OpenElement(Seq++, ELEMENT_GROUP);
            renderTreeBuilder.AddAttribute(Seq++, "id", id);
            renderTreeBuilder.AddAttribute(Seq++, "transform", transform);
            renderTreeBuilder.AddAttribute(Seq++, "clip-path", clippath);
            renderTreeBuilder.AddAttribute(Seq++, "tabindex", tabIndex);
            renderTreeBuilder.AddAttribute(Seq++, "aria-label", accessText);
            renderTreeBuilder.AddAttribute(Seq++, "aria-hidden", ariaHidden);
            renderTreeBuilder.AddAttribute(Seq++, "data-point", dataPoints);
            renderTreeBuilder.AddAttribute(Seq++, "role", role);

            if (!string.IsNullOrEmpty(svgStyleClass))
            {
                renderTreeBuilder.AddAttribute(Seq++, "class", svgStyleClass);
            }

            if (role == "button")
            {
                renderTreeBuilder.AddAttribute(Seq++, "aria-pressed", ariaPressed);
            }
            renderTreeBuilder.AddElementReferenceCapture(Seq++, ins => { GroupCollection?.Add(ins); });
        }

        /// <summary>Renders an SVG path element with manual path options.</summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="id">The unique identifier for the path element.</param>
        /// <param name="direction">The path direction (e.g., "M 0 0 L 10 10").</param>
        /// <param name="strokeDasharray">The stroke dash array pattern.</param>
        /// <param name="strokeWidth">The width of the stroke.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="opacity">The opacity level (0-1). Default: 1.</param>
        /// <param name="fill">The fill color. Default: <c>"transparent"</c>.</param>
        /// <param name="accessText">Accessibility label. Default: empty string.</param>
        /// <param name="tabIndex">Tab index for keyboard navigation. Default: empty string.</param>
        /// <param name="data">Data point identifier. Default: empty string.</param>
        internal void RenderPath(RenderTreeBuilder renderTreeBuilder, string id, string direction, string strokeDasharray, double strokeWidth, string stroke, double opacity = 1, string fill = "transparent", string accessText = "", string tabIndex = "", string data = "")
        {
            renderTreeBuilder.OpenComponent<SvgPath>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(new PathOptions(id, direction, strokeDasharray, strokeWidth, stroke, opacity, fill, string.Empty, string.Empty, accessText, string.Empty, tabIndex)));
            renderTreeBuilder.AddAttribute(Seq++, "DataPoint", data);
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { PathElementList?.Add((SvgPath)ins); });
            renderTreeBuilder.CloseComponent();
        }

        /// <summary>
        /// Renders an SVG clip path definition with a rectangular clipping region.
        /// </summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="id">The unique identifier for the clip path.</param>
        /// <param name="rect">The rectangular clipping region.</param>
        /// <param name="visibility">The visibility attribute. Default: <c>"visible"</c>.</param>
        internal void RenderClipPath(RenderTreeBuilder renderTreeBuilder, string id, Rect rect, string visibility = "visible")
        {
            renderTreeBuilder.OpenElement(Seq++, ELEMENT_DEFS);
            renderTreeBuilder.OpenElement(Seq++, ELEMENT_CLIP_PATH);
            renderTreeBuilder.AddAttribute(Seq++, "id", id);
            RenderRect(renderTreeBuilder, new RectOptions(id + "_Rect", rect.X, rect.Y, rect.Width, rect.Height, 1, "transparent", "transparent", 0, 0, 1, visibility));
            renderTreeBuilder.CloseElement();
            renderTreeBuilder.CloseElement();
        }

        /// <summary>
        /// Renders an SVG clip path definition with a circular clipping region.
        /// </summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="id">The unique identifier for the clip path.</param>
        /// <param name="options">Options for configuring the circular clipping region.</param>
        internal void RenderCircularClipPath(RenderTreeBuilder renderTreeBuilder, string id, CircleOptions options)
        {
            renderTreeBuilder.OpenElement(Seq++, ELEMENT_DEFS);
            renderTreeBuilder.OpenElement(Seq++, ELEMENT_CLIP_PATH);
            renderTreeBuilder.AddAttribute(Seq++, "id", id);
            RenderCircle(renderTreeBuilder, options);
            renderTreeBuilder.CloseElement();
            renderTreeBuilder.CloseElement();
        }

        /// <summary>Extracts properties from an options object and returns them as a dictionary of attributes.</summary>
        /// <remarks>
        /// Uses reflection with caching to minimize performance overhead on repeated calls 
        /// with the same object type.
        /// </remarks>
        /// <param name="obj">The options object to extract properties from.</param>
        /// <returns>A dictionary mapping property names to their values.</returns>
        internal Dictionary<string, object> GetOptions(object obj)
        {
            PropertyInfo[] _propertyInfos = obj.GetType().GetProperties();
            Dictionary<string, object> attributes = new Dictionary<string, object> { };
            foreach (PropertyInfo property in _propertyInfos)
            {
                attributes.Add(property.Name, property.GetValue(obj) ?? null!);
            }
            return attributes;
        }

        /// <summary>
        /// Renders an SVG polygon element.
        /// </summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="seq">The sequence number for RenderTreeBuilder operations.</param>
        /// <param name="id">The unique identifier for the polygon.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="points">The polygon points string (e.g., "0,0 10,0 5,10").</param>
        internal void RenderPolygon(RenderTreeBuilder renderTreeBuilder, int seq, string id, string fill, string points)
        {
            renderTreeBuilder.OpenComponent<SvgPolygon>(seq);
            renderTreeBuilder.AddMultipleAttributes(seq + 1, new Dictionary<string, object>() { { "Id", id }, { "Fill", fill }, { "points", points } });
            renderTreeBuilder.CloseComponent();
        }

        /// <summary>
        /// Opens a clip path element.
        /// </summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="seq">The sequence number for RenderTreeBuilder operations.</param>
        /// <param name="id">The unique identifier for the clip path.</param>
        internal void OpenClipPath(RenderTreeBuilder renderTreeBuilder, int seq, string id)
        {
            renderTreeBuilder.OpenElement(seq, ELEMENT_CLIP_PATH);
            renderTreeBuilder.AddAttribute(seq + 1, "id", id);
        }

        /// <summary>
        /// Renders an SVG ellipse element with optional data binding.
        /// </summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="ellipesOption">Options for configuring the ellipse element.</param>
        /// <param name="data">Data point identifier. Default: empty string.</param>
        internal void RenderEllipse(RenderTreeBuilder renderTreeBuilder, EllipseOptions ellipesOption, string data = "")
        {
            renderTreeBuilder.OpenComponent<SvgEllipse>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(ellipesOption));
            if (!string.IsNullOrEmpty(data))
            {
                renderTreeBuilder.AddAttribute(Seq++, "DataPoint", data);
            }
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { EllipseElementList?.Add((SvgEllipse)ins); });
            renderTreeBuilder.CloseComponent();
        }

        /// <summary>
        /// Renders an SVG circle element.
        /// </summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="ellipesOption">Options for configuring the circle element.</param>
        internal void RenderCircle(RenderTreeBuilder renderTreeBuilder, CircleOptions ellipesOption)
        {
            renderTreeBuilder.OpenComponent<SvgCircle>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(ellipesOption));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { CircleCollection?.Add((SvgCircle)ins); });
            renderTreeBuilder.CloseComponent();
        }

        /// <summary>
        /// Renders an SVG image element.
        /// </summary>
        /// <param name="renderTreeBuilder">The RenderTreeBuilder used to construct the render tree.</param>
        /// <param name="ImageOption">Options for configuring the image element.</param>
        internal void RenderImage(RenderTreeBuilder renderTreeBuilder, ImageOptions ImageOption)
        {
            renderTreeBuilder.OpenComponent<SvgImage>(Seq++);
            renderTreeBuilder.AddMultipleAttributes(Seq++, GetOptions(ImageOption));
            renderTreeBuilder.AddComponentReferenceCapture(Seq++, ins => { ImageCollection?.Add((SvgImage)ins); });
            renderTreeBuilder.CloseComponent();
        }

        /// <summary>
        /// Disposes all managed resources and clears element collections.
        /// </summary>
        internal void Dispose()
        {
            TextElementList?.ForEach(item => item?.Dispose());
            TextElementList = null;
            PathElementList = null;
            EllipseElementList = null;
            RectElementList = null;
            ImageCollection = null;
            CircleCollection = null;
            GroupCollection = null;
        }
        #endregion
    }
}
