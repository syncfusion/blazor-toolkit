
namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    internal partial class CartesianAxisLayout
    {
        #region Private Methods

        /// <summary>
        /// Gets the axis offset value based on position and plot offset.
        /// </summary>
        /// <param name="position1">Primary position value (may be NaN).</param>
        /// <param name="position2">Secondary position value (may be NaN).</param>
        /// <param name="plotOffset">Default plot offset fallback.</param>
        /// <returns>The calculated offset value.</returns>
        private static double GetAxisOffsetValue(double position1, double position2, double plotOffset)
        {
            return !double.IsNaN(position1)
                ? (position1 + (!double.IsNaN(position2) ? position2 : plotOffset))
                : !double.IsNaN(position2) ? position2 + plotOffset : 2 * plotOffset;
        }

        /// <summary>
        /// Gets font options from a style, preserving all properties.
        /// </summary>
        /// <param name="style">The source <see cref="ChartFontOptions"/>.</param>
        /// <returns>A new <see cref="ChartFontOptions"/> with copied properties.</returns>
        private static ChartFontOptions GetFontOptions(ChartFontOptions style)
        {
            return new ChartFontOptions
            {
                Color = style.Color,
                Size = style.Size,
                FontFamily = style.FontFamily,
                FontWeight = style.FontWeight,
                FontStyle = style.FontStyle,
                TextAlignment = style.TextAlignment,
                TextOverflow = style.TextOverflow
            };
        }

        /// <summary>
        /// Converts a degree value to radians.
        /// </summary>
        /// <param name="degree">The angle in degrees.</param>
        /// <returns>The angle in radians.</returns>
        private static double DegreeToRadian(double degree)
        {
            return degree * (Math.PI / 180);
        }

        /// <summary>
        /// Gets the four corner points of a rectangle.
        /// </summary>
        /// <param name="rect">The <see cref="Rect"/> to convert.</param>
        /// <returns>Array of four <see cref="ChartEventLocation"/> points.</returns>
        private static ChartEventLocation[] GetRectanglePoints(Rect rect)
        {
            return
            [
                new(rect.X, rect.Y),
                new(rect.X + rect.Width, rect.Y),
                new(rect.X + rect.Width, rect.Y + rect.Height),
                new(rect.X, rect.Y + rect.Height)
            ];
        }

        /// <summary>
        /// Rotates rectangle corner coordinates around a center point.
        /// </summary>
        /// <param name="actualPoints">Array of points to rotate (modified in-place).</param>
        /// <param name="centerX">X coordinate of rotation center.</param>
        /// <param name="centerY">Y coordinate of rotation center.</param>
        /// <param name="angle">Rotation angle in degrees.</param>
        /// <returns>Array of rotated coordinates.</returns>
        private static ChartEventLocation[] GetRotatedRectangleCoordinates(ChartEventLocation[] actualPoints, double centerX, double centerY, double angle)
        {
            List<ChartEventLocation> coordinatesAfterRotation = [];
            double angleRad = DegreeToRadian(angle);
            double cosA = Math.Cos(angleRad);
            double sinA = Math.Sin(angleRad);

            for (int i = 0; i < 4; i++)
            {
                double tempX = actualPoints[i].X - centerX;
                double tempY = actualPoints[i].Y - centerY;
                actualPoints[i].X = (tempX * cosA) - (tempY * sinA) + centerX;
                actualPoints[i].Y = (tempX * sinA) + (tempY * cosA) + centerY;
                coordinatesAfterRotation.Add(new ChartEventLocation(actualPoints[i].X, actualPoints[i].Y));
            }

            return [.. coordinatesAfterRotation];
        }

        /// <summary>
        /// Checks if two rotated rectangles intersect using the Separating Axis Theorem.
        /// </summary>
        /// <param name="a">First polygon as array of <see cref="ChartEventLocation"/>.</param>
        /// <param name="b">Second polygon as array of <see cref="ChartEventLocation"/>.</param>
        /// <returns><see langword="true"/> if the polygons intersect; otherwise <see langword="false"/>.</returns>
        private static bool IsRotatedRectIntersect(ChartEventLocation[] a, ChartEventLocation[] b)
        {
            List<ChartEventLocation[]> polygons = [a, b];

            for (int i = 0; i < polygons.Count; i++)
            {
                for (int k = 0; k < polygons[i].Length; k++)
                {
                    ChartEventLocation p1 = polygons[i][k];
                    ChartEventLocation p2 = polygons[i][(k + 1) % polygons[i].Length];
                    ChartEventLocation normal = new(p2.Y - p1.Y, p1.X - p2.X);

                    (double minA, double maxA) = ProjectPolygon(normal, a);
                    (double minB, double maxB) = ProjectPolygon(normal, b);

                    if (maxA < minB || maxB < minA)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Projects a polygon onto an axis and returns min/max projection values.
        /// </summary>
        /// <param name="axis">Axis vector used for projection.</param>
        /// <param name="polygon">Polygon points to project.</param>
        /// <returns>Tuple containing minimum and maximum projection values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="axis"/> or <paramref name="polygon"/> is <see langword="null"/>.</exception>
        private static (double min, double max) ProjectPolygon(ChartEventLocation axis, ChartEventLocation[] polygon)
        {
            double min = double.NaN;
            double max = double.NaN;

            foreach (ChartEventLocation point in polygon)
            {
                double projected = (axis.X * point.X) + (axis.Y * point.Y);
                if (double.IsNaN(min) || projected < min)
                {
                    min = projected;
                }

                if (double.IsNaN(max) || projected > max)
                {
                    max = projected;
                }
            }

            return (min, max);
        }

        /// <summary>
        /// Returns translate offsets for a rotated template bounding box.
        /// </summary>
        /// <param name="angleDegrees">Rotation angle in degrees.</param>
        /// <param name="width">Original width.</param>
        /// <param name="height">Original height.</param>
        /// <returns>Tuple with translateX and translateY.</returns>
        private static (double translateX, double translateY) GetRotateTemplateStyles(double angleDegrees, double width, double height)
        {
            double angleRadians = angleDegrees * Math.PI / 180.0;

            (double x, double y)[] corners =
            [
                (0, 0), (width, 0), (width, height), (0, height)
            ];

            (double x, double y)[] rotatedCorners = [.. corners.Select(corner =>
            {
                double x = (corner.x * Math.Cos(angleRadians)) - (corner.y * Math.Sin(angleRadians));
                double y = (corner.x * Math.Sin(angleRadians)) + (corner.y * Math.Cos(angleRadians));
                return (x, y);
            })];

            double minX = rotatedCorners.Min(c => c.x);
            double minY = rotatedCorners.Min(c => c.y);

            return (Math.Round(-minX, 2), Math.Round(-minY, 2));
        }

        /// <summary>
        /// Gets the label text, handling both plain and wrapped labels.
        /// </summary>
        /// <param name="label">The <see cref="VisibleLabels"/> instance.</param>
        /// <param name="axis">The <see cref="ChartAxis"/> context.</param>
        /// <param name="intervalLength">Available interval length for text.</param>
        /// <returns>List of label text strings.</returns>
        private static List<string> GetLabelText(VisibleLabels label, ChartAxis axis, double intervalLength)
        {
            if (ChartHelper.IsBreakLabel(label.OriginalText) || (axis.Renderer?.LabelIntersectAction == LabelIntersectAction.Wrap))
            {
                List<string> result = [];
                for (int index = 0; index < label.TextArr.Length; index++)
                {
                    result.Add(FindAxisLabel(axis, label.TextArr[index], intervalLength));
                }

                return result;
            }
            else
            {
                return [FindAxisLabel(axis, label.Text, intervalLength)];
            }
        }

        /// <summary>
        /// Finds and trims/fits axis label text based on intersection action.
        /// </summary>
        /// <param name="axis">The <see cref="ChartAxis"/> context.</param>
        /// <param name="label">The label text.</param>
        /// <param name="width">Available width for the label.</param>
        /// <returns>Processed label text.</returns>
        private static string FindAxisLabel(ChartAxis axis, string label, double width)
        {
            ChartFontOptions axisLabelStyle = axis.LabelStyle.GetChartFontOptions(axis.Renderer?.Owner?._chartThemeStyle ?? null!);
            return axis.Renderer?.LabelIntersectAction == LabelIntersectAction.Trim && axis.Renderer.Angle % 360 == 0 && !axis.EnableTrim
                ? ChartHelper.TextTrim(width, label, axisLabelStyle)
                : label;
        }

        /// <summary>
        /// Calculates log numeric interval based on axis value type.
        /// </summary>
        /// <param name="axis">The <see cref="ChartAxis"/> context.</param>
        /// <param name="logPosition">Logarithmic position value.</param>
        /// <param name="interval">Current interval.</param>
        /// <param name="labelIndex">Index of the current label.</param>
        /// <returns>Adjusted interval value.</returns>
        private static double FindLogNumeric(ChartAxis axis, double logPosition, double interval, int labelIndex)
        {
            if (axis.ValueType == ValueType.DateTime && axis.Renderer is not null)
            {
                interval += axis.Renderer.DateTimeInterval / (axis.MinorTicksPerInterval + 1);
            }
            else if (axis.ValueType == ValueType.Logarithmic)
            {
                interval = ChartHelper.LogBase(logPosition, axis.LogBase);
            }
            else if (axis.ValueType == ValueType.DateTimeCategory)
            {
                double padding = axis.LabelPlacement == LabelPlacement.BetweenTicks ? 0.5 : 0;
                interval += ((labelIndex + 1 < axis.Renderer?.VisibleLabels.Count ? axis.Renderer.VisibleLabels[labelIndex + 1].Value - padding : (axis.Renderer?.VisibleRange.End ?? 0)) -
                (labelIndex < axis.Renderer?.VisibleLabels.Count ? axis.Renderer.VisibleLabels[labelIndex].Value - padding : (axis.Renderer?.VisibleRange.Start ?? 0))) / (axis.MinorTicksPerInterval + 1);
            }
            else
            {
                if (axis.Renderer is not null)
                {
                    interval += axis.Renderer.VisibleInterval / (axis.MinorTicksPerInterval + 1);
                }
            }

            return interval;
        }

        #endregion
    }
}
