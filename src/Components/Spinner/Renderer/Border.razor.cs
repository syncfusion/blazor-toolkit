using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Spinner.Internal
{
    /// <summary>
    /// Renders the spinner SVG element for Fluent theme.
    /// </summary>
    /// <remarks>
    /// It inherits from <see cref="SpinnerBase"/> and configures the spinner's arc and circle paths based on the parent component's theme.
    /// This component is an internal implementation detail and should not be used directly.
    /// </remarks>
    /// <exclude/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public partial class Border : SpinnerBase
    {
        #region Constants

        /// <summary>
        /// CSS class for the spinner arc path.
        /// </summary>
        private const string PathArcClass = "e-path-arc";

        /// <summary>
        /// SVG class for Fluent theme styling.
        /// </summary>
        private const string SvgClassFluent = "e-spin-fluent";

        /// <summary>
        /// Starting angle for arc calculation (315 degrees).
        /// </summary>
        private const int StartArcValue = 315;

        /// <summary>
        /// Ending angle for arc calculation (45 degrees).
        /// </summary>
        private const int EndArcValue = 45;

        #endregion

        #region Fields

        /// <summary>
        /// CSS class for the arc path element.
        /// </summary>
        private readonly string _pathArcClass = PathArcClass;

        /// <summary>
        /// SVG path data for the circle base.
        /// </summary>
        private string? _pathCircleData;

        /// <summary>
        /// Computed inline CSS style for stroke width.
        /// </summary>
        private string? _strokeWidth;

        /// <summary>
        /// SVG path data for the rotating arc.
        /// </summary>
        private string? _pathArcData;

        /// <summary>
        /// Calculated radius of the spinner.
        /// </summary>
        private int _radius;

        /// <summary>
        /// Optional logger for error reporting during border rendering.
        /// </summary>
        [Inject]
        private ILogger<Border>? Logger { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cascading parameter reference to the parent <see cref="SfSpinner"/> component.
        /// </summary>
        /// <value>
        /// A reference to the parent <see cref="SfSpinner"/> component. The default value is <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// This cascading parameter is automatically provided by the parent SfSpinner component.
        /// </remarks>
        [CascadingParameter]
        private SfSpinner? BaseParent { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the spinner component by calculating its dimensions and setting its visual properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous initialization operation.</returns>
        /// <remarks>
        /// This method calculates the spinner's radius, determines the appropriate SVG class based on the spinner type, 
        /// generates a unique ID, and computes the SVG path attributes for circle and arc visualization.
        /// </remarks>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Calculate spinner dimensions based on parent's size parameter
                _radius = CalculateRadius(BaseParent?.Size);
                SpinnerSvgClass = SvgClassFluent;
                SvgId = SfBaseUtils.GenerateID("SVG");

                // Calculate diameter and build style attributes
                int diameter = _radius * 2;
                string transformOrigin = $"{diameter / 2}px";

                // Build SVG path data
                _pathCircleData = BuildCirclePath(_radius, _radius);
                _pathArcData = BuildArcPath(_radius, _radius, StartArcValue, EndArcValue);

                // Set SVG viewBox and styles
                ViewBox = $"0 0 {diameter} {diameter}";
                SvgStyle = $"width: {diameter}px; height: {diameter}px; transform-origin: {transformOrigin} {transformOrigin} {transformOrigin};";

                // Set the circle border thickness
                string thickness = BaseParent?.Thickness ?? "4px";
                // Only append px if the thickness is a bare number
                if (!string.IsNullOrEmpty(thickness) && !thickness.Any(c => char.IsLetter(c) || c == '%'))
                {
                    thickness = $"{thickness}px";
                }
                _strokeWidth = $"stroke-width: {thickness};";


                await base.OnInitializedAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error during Border initialization");
                throw;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Builds the SVG path for the circular base of the spinner.
        /// </summary>
        /// <param name="centerX">The X coordinate of the circle center.</param>
        /// <param name="centerY">The Y coordinate of the circle center.</param>
        /// <returns>The SVG path string for the circle.</returns>
        /// <remarks>
        /// This method creates a complete circle path that serves as the base for the spinner animation.
        /// </remarks>
        private string BuildCirclePath(int centerX, int centerY)
        {
            // Build circle path: move to center, create two arcs to form complete circle
            int diameter = _radius * 2;
            int negRadius = -_radius;
            return $"M {centerX} {centerY} m {negRadius} 0 a {_radius} {_radius} 0 1 0 {diameter} 0 a {_radius} {_radius} 0 1 0 {negRadius * 2} 0";
        }

        /// <summary>
        /// Builds the SVG path for the arc portion of the spinner that rotates.
        /// </summary>
        /// <param name="centerX">The X coordinate of the arc center.</param>
        /// <param name="centerY">The Y coordinate of the arc center.</param>
        /// <param name="startAngle">The starting angle for the arc in degrees.</param>
        /// <param name="endAngle">The ending angle for the arc in degrees.</param>
        /// <returns>The SVG path string for the arc.</returns>
        /// <remarks>
        /// This method creates an arc path that will be animated to create the spinning effect.
        /// </remarks>
        private string BuildArcPath(int centerX, int centerY, int startAngle, int endAngle)
        {
            // Calculate start and end points for the arc
            ArcPoints startPoint = DefineArcPoints(centerX, centerY, _radius, endAngle);
            ArcPoints endPoint = DefineArcPoints(centerX, centerY, _radius, startAngle);

            // Build arc path: move to start point, draw arc to end point
            return FormattableString.Invariant($"M {startPoint.PointX} {startPoint.PointY} A {_radius} {_radius} 0 0 0 {endPoint.PointX} {endPoint.PointY}");
        }

        #endregion

        #region Disposal

        /// <summary>
        /// Releases the resources used by the Border component.
        /// </summary>
        /// <remarks>
        /// This method disposes of the component's resources when the component is destroyed,
        /// preventing memory leaks and properly cleaning up references.
        /// </remarks>
        protected override ValueTask DisposeAsyncCore()
        {
            try
            {
                // Clear parent reference to avoid memory leaks
                if (BaseParent is not null)
                {
                    BaseParent = null;
                }
            }
            catch (ObjectDisposedException ex)
            {
                Logger?.LogError(ex, "Error during Border disposal");
            }

            return base.DisposeAsyncCore();
        }

        #endregion
    }

}
