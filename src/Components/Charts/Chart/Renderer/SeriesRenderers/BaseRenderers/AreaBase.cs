namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Provides shared rendering helpers for area-like series renderers.
    /// </summary>
    /// <remarks>
    /// This class centralizes path-direction calculations used by area and polar-area renderers.
    /// It preserves behavior from the original implementation while adding argument guards and
    /// XML documentation for clarity.
    /// </remarks>
    internal class AreaBaseSeriesRenderer : LineBaseSeriesRenderer
    {
        #region Internal Methods

        /// <summary>
        /// Appends a path direction segment for an area series when a start point is not provided.
        /// </summary>
        /// <param name="x">The raw X value in data coordinates.</param>
        /// <param name="y">The raw Y value in data coordinates.</param>
        /// <param name="series">The series instance associated with the points.</param>
        /// <param name="isInverted">Indicates whether the chart axes are inverted.</param>
        /// <param name="startPoint">The starting visible point; if not <c>null</c>, no segment is appended.</param>
        /// <param name="startPath">Path command to append (for example, "L").</param>
        internal virtual void GetAreaPathDirection(double x, double y, ChartSeries series, bool isInverted, ChartEventLocation startPoint, string startPath)
        {
            if (startPoint is null)
            {
                ChartEventLocation firstPoint = ChartHelper.GetPoint
                (
                    XAxisRenderer.GetPointValue(x),
                    YAxisRenderer.GetPointValue(y),
                    XAxisRenderer, YAxisRenderer,
                    XLength,
                    YLength,
                    isInverted
                );

                int px = Convert.ToInt32(!double.IsNaN(firstPoint.X) && firstPoint.X >= int.MinValue && firstPoint.X <= int.MaxValue ? firstPoint.X : 0);
                int py = Convert.ToInt32(!double.IsNaN(firstPoint.Y) ? firstPoint.Y : 0);
                _ = Direction.Append(string.Join(string.Empty, startPath, SPACE, px.ToString(Culture), SPACE, py.ToString(Culture), SPACE));
            }
        }

        /// <summary>
        /// Appends a path direction segment for polar area series when a start point is not provided.
        /// </summary>
        /// <param name="x">The raw angular or radial X coordinate.</param>
        /// <param name="y">The raw radial or angular Y coordinate.</param>
        /// <param name="series">The series instance associated with the points.</param>
        /// <param name="isInverted">Indicates whether the chart axes are inverted.</param>
        /// <param name="startPoint">The starting visible point; if not <c>null</c>, no segment is appended.</param>
        /// <param name="startPath">Path command to append (for example, "L").</param>
        internal virtual void GetPolarAreaPathDirection(double x, double y, ChartSeries series, bool isInverted, ChartEventLocation startPoint, string startPath)
        {
            if (startPoint is null)
            {
                ChartEventLocation firstPoint = ChartHelper.TransformToVisible(x, y, XAxisRenderer.Axis ?? null!, YAxisRenderer.Axis ?? null!, Series ?? null!);
                _ = Direction.Append(string.Join(string.Empty, startPath, SPACE, Convert.ToInt32(firstPoint.X).ToString(Culture), SPACE, Convert.ToInt32(firstPoint.Y).ToString(Culture), SPACE));
            }
        }

        /// <summary>
        /// Builds path direction segments for a pair of empty area points.
        /// </summary>
        /// <param name="firstPoint">The first visible point of the empty segment.</param>
        /// <param name="secondPoint">The second visible point of the empty segment.</param>
        /// <param name="series">The series instance associated with the points.</param>
        /// <param name="isInverted">Indicates whether the chart axes are inverted.</param>
        internal virtual void GetAreaEmptyDirection(ChartEventLocation firstPoint, ChartEventLocation secondPoint, ChartSeries series, bool isInverted)
        {
            GetAreaPathDirection(firstPoint.X, firstPoint.Y, series, isInverted, null!, "L");
            GetAreaPathDirection(secondPoint.X, secondPoint.Y, series, isInverted, null!, "L");
        }

        /// <summary>
        /// Updates rendering options when series customization properties change.
        /// </summary>
        /// <param name="property">The name of the property that changed (e.g., "Fill", "Color").</param>
        internal override void UpdateCustomization(string property)
        {
            base.UpdateCustomization(property);
            RendererShouldRender = true;

            if (_options is not null)
            {
                switch (property)
                {
                    case "Fill":
                        _options.Fill = Interior ?? string.Empty;
                        Series?.Marker.Renderer?.MarkerColorChanged();
                        break;
                    case "Color":
                        _options.Stroke = Series?.Border.Color ?? string.Empty;
                        break;
                    case "DashArray":
                        _options.StrokeDashArray = Series?.DashArray ?? string.Empty;
                        break;
                    case "Width":
                        _options.StrokeWidth = Series?.Width ?? 0;
                        break;
                    case "Opacity":
                        _options.Opacity = Series?.Opacity ?? 1;
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion
    }
}