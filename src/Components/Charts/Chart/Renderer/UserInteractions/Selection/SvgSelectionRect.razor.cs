using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Represents an SVG selection rectangle used by chart selection interactions.
    /// </summary>
    public partial class SvgSelectionRect
    {
        #region Fields
        private string _cursorStyle = "cursor:move";
        private CultureInfo _culture = CultureInfo.InvariantCulture;
        internal bool _isDrawCloseIcon;
        #endregion

        #region Properties

        /// <summary>
        /// Cascading parent collection that manages multiple selection rectangles.
        /// </summary>
        [CascadingParameter]
        private SvgSelectionRectCollection? Parent { get; set; }

        /// <summary>
        /// Gets or sets the identifier for this selection rectangle.
        /// </summary>
        /// <value>The id attribute rendered for this rect. Default: <c>string.Empty</c>.</value>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rectangle geometry describing the selection area.
        /// </summary>
        /// <value>The <see cref="Rect"/> used to render the selection rectangle.</value>
        [Parameter]
        public Rect DragRect { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stroke color for the rectangle border.
        /// </summary>
        /// <value>CSS color string used for stroke.</value>
        [Parameter]
        public string Stroke { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the stroke width for the rectangle border.
        /// </summary>
        /// <value>Numeric or CSS value for stroke width.</value>
        [Parameter]
        public string StrokeWidth { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the fill color for the rectangle.
        /// </summary>
        /// <value>CSS color string used for fill.</value>
        [Parameter]
        public string Fill { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the close icon options associated with the selection rect.
        /// </summary>
        /// <value>A <see cref="CloseOptions"/> instance representing close button attributes.</value>
        [Parameter]
        public CloseOptions Close { get; set; } = null!;

        /// <summary>
        /// Event callback invoked when <see cref="DragRect"/> changes.
        /// </summary>
        [Parameter]
        public EventCallback<Rect> DragRectChanged { get; set; }

        /// <summary>
        /// Event callback invoked when <see cref="Close"/> changes.
        /// </summary>
        [Parameter]
        public EventCallback<CloseOptions> CloseChanged { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the component and registers with the parent collection if present.
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent?.AddRectReference(this);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the rectangle geometry and notifies consumers.
        /// </summary>
        /// <param name="rect">The new rectangle geometry.</param>
        internal async Task ChangeRectangleAsync(Rect rect)
        {
            DragRect = rect;
            _isDrawCloseIcon = false;
            await DragRectChanged.InvokeAsync(DragRect).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the close icon attributes and notifies consumers to draw the icon.
        /// </summary>
        /// <param name="circleAttr">Circle attributes for the close icon.</param>
        /// <param name="pathAttr">Path attributes for the close icon.</param>
        internal async Task DrawCloseIconAsync(CircleOptions circleAttr, PathOptions pathAttr)
        {
            Close.Circle = circleAttr;
            Close.Path = pathAttr;
            _isDrawCloseIcon = true;
            await CloseChanged.InvokeAsync(Close).ConfigureAwait(true);
        }

        /// <summary>
        /// Updates the computed cursor style and requests a UI refresh.
        /// </summary>
        /// <param name="cursor">CSS cursor value (e.g., "move", "pointer").</param>
        internal async Task ChangeCursorAsync(string cursor)
        {
            if (Parent is not null)
            {
                _cursorStyle = "cursor:" + cursor;
                await InvokeAsync(StateHasChanged).ConfigureAwait(true);
            }
        }
        #endregion
    }
}