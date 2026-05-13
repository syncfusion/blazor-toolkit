using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Collection container for SVG selection rectangles and lasso paths rendered in the chart.
    /// </summary>
    public partial class SvgSelectionRectCollection
    {
        #region Fields
        private List<SvgSelectionRect> _tempRectsReference = [];
        private List<SvgSelectionPath> _tempPathsReference = [];
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of selection options (rectangles or lasso paths).
        /// </summary>
        /// <value>Collection of <see cref="SelectionOptions"/>. Default: empty list.</value>
        [Parameter]
        public List<SelectionOptions> SelectedRectangles { get; set; } = [];

        /// <summary>
        /// Runtime references to active rectangle components keyed by Id.
        /// </summary>
        internal Dictionary<string, SvgSelectionRect> RectsReference { get; set; } = [];

        /// <summary>
        /// Runtime references to active path components keyed by Id.
        /// </summary>
        internal Dictionary<string, SvgSelectionPath> PathsReference { get; set; } = [];
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Called after component has finished rendering.
        /// </summary>
        /// <param name="firstRender">Indicates whether this is the first render.</param>
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (!firstRender)
            {
                bool isModified = false;
                if (RectsReference.Count > 0 && _tempRectsReference.Count > 0)
                {
                    isModified = HandleReference(false);
                }
                else if (PathsReference.Count > 0 && _tempPathsReference.Count > 0)
                {
                    isModified = HandleReference(true);
                }

                if (isModified)
                {
                    _ = InvokeAsync(StateHasChanged);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Compares live references against the cloned snapshot and updates any replaced components.
        /// </summary>
        /// <param name="isLasso">When <see langword="true"/>, compares path references; otherwise rectangle references.</param>
        /// <returns><see langword="true"/> if any replacement occurred; otherwise <see langword="false"/>.</returns>
        private bool HandleReference(bool isLasso)
        {
            bool isModified = false;
            if (!isLasso)
            {
                UpdateRectsReference(ref isModified);
            }
            else
            {
                UpdatePathsReference(ref isModified);
            }
            return isModified;
        }

        /// <summary>
        /// Compares and updates rectangle runtime references when their component instances are replaced.
        /// </summary>
        /// <param name="isModified">A reference flag set to <c>true</c> when any replacement occurs.</param>
        private void UpdateRectsReference(ref bool isModified)
        {
            foreach (KeyValuePair<string, SvgSelectionRect> keyValuePair in RectsReference)
            {
                SvgSelectionRect rect = keyValuePair.Value;
                if (rect.Id != keyValuePair.Key)
                {
                    SvgSelectionRect clonedRect = _tempRectsReference.FirstOrDefault(r => r.Id == keyValuePair.Key) ?? null!;
                    if (clonedRect is not null)
                    {
                        RectsReference[keyValuePair.Key] = clonedRect;
                        RectsReference[keyValuePair.Key]._isDrawCloseIcon = true;
                        isModified = true;
                    }
                }
            }
        }

        /// <summary>
        /// Compares and updates path runtime references when their component instances are replaced.
        /// </summary>
        /// <param name="isModified">A reference flag set to <c>true</c> when any replacement occurs.</param>
        private void UpdatePathsReference(ref bool isModified)
        {
            foreach (KeyValuePair<string, SvgSelectionPath> keyValuePair in PathsReference)
            {
                SvgSelectionPath path = keyValuePair.Value;
                if (path.Id != keyValuePair.Key)
                {
                    SvgSelectionPath clonedpath = _tempPathsReference.FirstOrDefault(p => p.Id == keyValuePair.Key) ?? null!;
                    if (clonedpath is not null)
                    {
                        PathsReference[keyValuePair.Key] = clonedpath;
                        PathsReference[keyValuePair.Key]._isDrawCloseIcon = true;
                        isModified = true;
                    }
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Adds a rectangle component reference and stores a shallow clone for change detection.
        /// </summary>
        /// <param name="rect">The rectangle component reference.</param>
        internal void AddRectReference(SvgSelectionRect rect)
        {
            if (rect is not null)
            {
                _ = RectsReference.TryAdd(rect.Id, rect);
                SvgSelectionRect clonedSelectionRect = new()
                {
                    Id = rect.Id,
                    DragRect = new Rect(rect.DragRect.X, rect.DragRect.Y, rect.DragRect.Width, rect.DragRect.Height),
                    Close = rect.Close,
                    Fill = rect.Fill,
                    Stroke = rect.Stroke,
                    StrokeWidth = rect.StrokeWidth,
                    _isDrawCloseIcon = rect._isDrawCloseIcon,
                    CloseChanged = rect.CloseChanged,
                    DragRectChanged = rect.DragRectChanged
                };
                _tempRectsReference.Add(clonedSelectionRect);
            }
        }

        /// <summary>
        /// Removes a rectangle reference by id.
        /// </summary>
        /// <param name="rectId">The identifier of the rectangle to remove.</param>
        internal void RemoveRectReferenceAsync(string rectId)
        {
            _ = RectsReference.Remove(rectId);
        }

        /// <summary>
        /// Adds a path component reference and stores a shallow clone for change detection.
        /// </summary>
        /// <param name="path">The path component reference.</param>
        internal void AddPathReference(SvgSelectionPath path)
        {
            if (path is not null)
            {
                _ = PathsReference.TryAdd(path.Id, path);
                SvgSelectionPath clonedSelectionPath = new()
                {
                    Id = path.Id,
                    Path = path.Path,
                    Close = path.Close,
                    Fill = path.Fill,
                    Stroke = path.Stroke,
                    StrokeWidth = path.StrokeWidth,
                    _isDrawCloseIcon = path._isDrawCloseIcon,
                    CloseChanged = path.CloseChanged,
                    PathChanged = path.PathChanged
                };
                _tempPathsReference.Add(clonedSelectionPath);
            }
        }

        /// <summary>
        /// Removes a path reference by id.
        /// </summary>
        /// <param name="rectId">The identifier of the path to remove.</param>
        internal void RemovePathReferenceAsync(string rectId)
        {
            _ = PathsReference.Remove(rectId);
        }

        /// <summary>
        /// Adds a new selection option and notifies consumers.
        /// </summary>
        /// <param name="rect">The selection option to add.</param>
        internal void DrawNewRectangle(SelectionOptions rect)
        {
            SelectedRectangles.Add(rect);
            _ = InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Removes a rectangle item and updates references.
        /// </summary>
        /// <param name="currentRect">The rectangle component instance to remove.</param>
        internal void RemoveCurrentElement(SvgSelectionRect currentRect)
        {
            SelectionOptions selectedRect = SelectedRectangles.Find(x => x.Id == currentRect.Id) ?? null!;
            if (selectedRect is not null)
            {
                _ = SelectedRectangles.Remove(selectedRect);
            }
            RemoveRectReferenceAsync(currentRect.Id);
            foreach (SvgSelectionRect rect in RectsReference.Values)
            {
                rect._isDrawCloseIcon = true;
            }
            _ = InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Removes a path item and updates references.
        /// </summary>
        /// <param name="currentPath">The path component instance to remove.</param>
        internal void RemoveCurrentElement(SvgSelectionPath currentPath)
        {
            SelectionOptions selectedRect = SelectedRectangles.Find(x => x.Id == currentPath.Id) ?? null!;
            if (selectedRect is not null)
            {
                _ = SelectedRectangles.Remove(selectedRect);
            }
            RemovePathReferenceAsync(currentPath.Id);
            foreach (SvgSelectionPath rect in PathsReference.Values)
            {
                rect._isDrawCloseIcon = true;
            }
            _ = InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Clears all selection elements and all runtime references.
        /// </summary>
        internal void ClearElements()
        {
            SelectedRectangles.Clear();
            PathsReference.Clear();
            PathsReference.Clear();
            RectsReference.Clear();
            RectsReference.Clear();
            _tempRectsReference.Clear();
            _tempPathsReference.Clear();
            _ = InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}