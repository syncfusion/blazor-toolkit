using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents an SVG selection path used by the chart selection UI.
    /// </summary>
    /// <remarks>
    /// This component renders the selection path and an optional close icon. It exposes parameters
    /// for visual attributes and events for path/close updates.
    /// </remarks>
    public partial class SvgSelectionPath
    {
        #region Fields
        private string _cursorStyle = "cursor:move";
        private CultureInfo _culture = CultureInfo.InvariantCulture;
        internal bool _isDrawCloseIcon;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cascading parent collection that owns this path instance.
        /// </summary>
        /// <value>The parent <see cref="SvgSelectionRectCollection"/> instance. Default: <c>null</c>.</value>
        [CascadingParameter]
        public SvgSelectionRectCollection Parent { get; set; } = null!;

        /// <summary>
        /// Gets or sets the element id for the path group.
        /// </summary>
        /// <value>The id attribute rendered on the path element.</value>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stroke color applied to the path.
        /// </summary>
        /// <value>SVG stroke color.</value>
        [Parameter]
        public string Stroke { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stroke width applied to the path.
        /// </summary>
        /// <value>SVG stroke width.</value>
        [Parameter]
        public string StrokeWidth { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the fill color applied to the path.
        /// </summary>
        /// <value>SVG fill color.</value>
        [Parameter]
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the path data (the "d" attribute) for the SVG path element.
        /// </summary>
        /// <value>A string containing path commands.</value>
        [Parameter]
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets close icon attributes.
        /// </summary>
        /// <value>The <see cref="CloseOptions"/> instance used to render the close icon.</value>
        [Parameter]
        public CloseOptions Close { get; set; } = null!;

        /// <summary>
        /// Event callback invoked when the path changes.
        /// </summary>
        [Parameter]
        public EventCallback<string> PathChanged { get; set; }

        /// <summary>
        /// Event callback invoked when close icon attributes change or when close action occurs.
        /// </summary>
        [Parameter]
        public EventCallback<CloseOptions> CloseChanged { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Performs component initialization.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent?.AddPathReference(this);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the rendered path and notifies consumers.
        /// </summary>
        /// <param name="path">The new SVG path data.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ChangePathAsync(string path)
        {
            _isDrawCloseIcon = false;
            Path = path;
            await PathChanged.InvokeAsync(Path).ConfigureAwait(true);
        }

        /// <summary>
        /// Requests that the close icon be drawn with provided attributes.
        /// </summary>
        /// <param name="circleAttr">Circle rendering attributes.</param>
        /// <param name="pathAttr">Path rendering attributes for the close glyph.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task DrawCloseIconAsync(CircleOptions circleAttr, PathOptions pathAttr)
        {
            Close.Circle = circleAttr;
            Close.Path = pathAttr;
            _isDrawCloseIcon = true;
            await CloseChanged.InvokeAsync(Close).ConfigureAwait(true);
        }
        #endregion
    }
}