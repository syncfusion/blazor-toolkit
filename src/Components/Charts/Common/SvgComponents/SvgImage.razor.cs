using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents an SVG image element wrapper used by DataViz components.
    /// </summary>
    public partial class SvgImage
    {
        #region Fields
        private CultureInfo _culture { get; set; } = CultureInfo.InvariantCulture;
        #endregion

        #region Properties

        /// <summary>
        /// Element id used in the rendered SVG image element.
        /// </summary>
        /// <value>An identifier string.</value>
        [Parameter]
        public string Id { get; set; } = null!;

        /// <summary>
        /// X-coordinate of the image's top-left corner.
        /// </summary>
        /// <value>Numeric X position in px.</value>
        [Parameter]
        public double X { get; set; }

        /// <summary>
        /// Y-coordinate of the image's top-left corner.
        /// </summary>
        /// <value>Numeric Y position in px.</value>
        [Parameter]
        public double Y { get; set; }

        /// <summary>
        /// Width of the image in px.
        /// </summary>
        /// <value>Non-negative width; defaults to 0.</value>
        [Parameter]
        public double Width { get; set; }

        /// <summary>
        /// Height of the image in px.
        /// </summary>
        /// <value>Non-negative height; defaults to 0.</value>
        [Parameter]
        public double Height { get; set; }

        /// <summary>
        /// Source reference (href) for the image element.
        /// </summary>
        /// <value>URL or data URI pointing to the image resource.</value>
        [Parameter]
        public string Href { get; set; } = null!;

        /// <summary>
        /// Visibility attribute for the SVG image element.
        /// </summary>
        /// <value>Visibility CSS or SVG value; empty indicates default visibility.</value>
        [Parameter]
        public string Visibility { get; set; } = null!;

        /// <summary>
        /// The preserveAspectRatio attribute for the image element.
        /// </summary>
        /// <value>Defaults to 'xMidYMid meet' for predictable scaling.</value>
        [Parameter]
        public string PreserveAspectRatio { get; set; } = null!;
        #endregion
    }
}