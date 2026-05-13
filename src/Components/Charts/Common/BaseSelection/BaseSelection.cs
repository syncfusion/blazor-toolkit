using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Provides base selection functionality for chart components, including pattern definition and rendering.
    /// </summary>
    /// <remarks>
    /// This internal class manages SVG pattern creation for various selection styles (dots, chessboard, stripes, etc.)
    /// and coordinates with <see cref="SelectionStyleComponent"/> for rendering.
    /// </remarks>
    class BaseSelection
    {
        #region Properties

        // Gets or sets the unique identifier for the style element.
        internal string? StyleId { get; set; }

        // Gets or sets the CSS class representing the unselected state.
        protected string Unselected { get; set; } = string.Empty;

        // Gets or sets the inner HTML content for the selection element.
        protected string? InnerHTML { get; set; }

        // Gets the culture information used for formatting.
        protected CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;

        // Gets the collection of pattern options required for rendering.
        internal List<PatternOptions> ReqPatterns { get; set; } = new List<PatternOptions>();

        // Gets the selection style component responsible for rendering patterns.
        internal SelectionStyleComponent StyleRender { get; set; } = new SelectionStyleComponent();
        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a new <see cref="PatternOptions"/> with a sanitized Id.
        /// </summary>
        /// <param name="color">Color string with characters safe for an element Id (no '#').</param>
        /// <param name="index">Index to ensure uniqueness.</param>
        /// <param name="patternName">Pattern enumeration value.</param>
        /// <returns>A configured <see cref="PatternOptions"/> instance.</returns>
        static PatternOptions CreatePatternOptions(string color, int index, SelectionPattern patternName)
        {
            return new PatternOptions()
            {
                Id = patternName + "_Selection" + "_" + color + "_" + index,
                PatternUnits = "userSpaceOnUse"
            };
        }

        /// <summary>
        /// Builds the CSS url() reference for a generated pattern.
        /// </summary>
        /// <param name="patternName">Pattern enumeration value.</param>
        /// <param name="color">Color string safe for ids.</param>
        /// <param name="index">Index for uniqueness.</param>
        /// <returns>String suitable for SVG fill like "url(#...)"</returns>
        static string BuildPatternUrl(SelectionPattern patternName, string color, int index)
        {
            return "url(#" + patternName + "_Selection_" + color + "_" + index + ")";
        }

        /// <summary>
        /// Configures the pattern and populates the provided <paramref name="pathOptions"/>.
        /// </summary>
        /// <param name="patternName">The pattern type to configure.</param>
        /// <param name="patternGroup">The pattern options container to populate.</param>
        /// <param name="pathOptions">Mutable list to receive shape descriptors.</param>
        /// <param name="color">Requested color (used for stroke/fill).</param>
        /// <param name="opacity">Requested opacity for shapes.</param>
        static void ConfigurePattern(SelectionPattern patternName, PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            switch (patternName)
            {
                case SelectionPattern.Dots:
                    ConfigureDotsPattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Pacman:
                    ConfigurePacmanPattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Chessboard:
                    ConfigureChessboardPattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Crosshatch:
                    ConfigureCrosshatchPattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.DiagonalForward:
                    ConfigureDiagonalForwardPattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.DiagonalBackward:
                    ConfigureDiagonalBackwardPattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Grid:
                    ConfigureGridPattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Turquoise:
                    ConfigureTurquoisePattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Star:
                    ConfigureStarPattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Triangle:
                    ConfigureTrianglePattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Circle:
                    ConfigureCirclePattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Tile:
                    ConfigureTilePattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.HorizontalDash:
                    ConfigureHorizontalDashPattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.VerticalDash:
                    ConfigureVerticalDashPattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Rectangle:
                    ConfigureRectanglePattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Box:
                    ConfigureBoxPattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.HorizontalStripe:
                    ConfigureHorizontalStripePattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.VerticalStripe:
                    ConfigureVerticalStripePattern(patternGroup, pathOptions, color, opacity);
                    break;
                case SelectionPattern.Bubble:
                    ConfigureBubblePattern(patternGroup, pathOptions, color, opacity);
                    break;
            }
        }

        /// <summary>
        /// Configures a dots pattern with circular elements on a white background.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background rectangle and ellipse dot.</param>
        /// <param name="color">The fill color for the dot element.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureDotsPattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Width = patternGroup.Height = 6;
            pathOptions.Add(new RectOptions("PatternStroke", 0, 0, 7, 7, 0, "0.0000001", "#ffffff", 0, 0, opacity));
            pathOptions.Add(new EllipseOptions(string.Empty, "2", "2", "3", "3", string.Empty, 1, color));
        }

        /// <summary>
        /// Configures a Pacman pattern with a distinctive Pacman character shape.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background rectangle and Pacman path.</param>
        /// <param name="color">The fill color for the Pacman shape.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigurePacmanPattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = 18.384;
            patternGroup.Width = 17.917;
            pathOptions.Add(new RectOptions(string.Empty, 0, 0, 18.384, 17.917, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new PathOptions(null!, "M9.081,9.194l5.806-3.08c-0.812-1.496-2.403-3.052-4.291-3.052H8.835C6.138,3.063,3,6.151,3,8.723v1.679   c0,2.572,3.138,5.661,5.835,5.661h1.761c2.085,0,3.835-1.76,4.535-3.514L9.081,9.194z", null!, 1, color, opacity, color));
        }

        /// <summary>
        /// Configures a chessboard pattern with alternating square tiles.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and alternating square tiles.</param>
        /// <param name="color">The fill color for the dark squares.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureChessboardPattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 10;
            pathOptions.Add(new RectOptions(string.Empty, 0, 0, 10, 10, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new RectOptions(string.Empty, 0, 0, 5, 5, 0, null!, color, 0, 0, opacity));
            pathOptions.Add(new RectOptions(string.Empty, 5, 5, 5, 5, 0, null!, color, 0, 0, opacity));
        }

        /// <summary>
        /// Configures a crosshatch pattern with intersecting diagonal lines.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and crosshatch lines.</param>
        /// <param name="color">The stroke color for the hatch lines.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureCrosshatchPattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 8;
            pathOptions.Add(new RectOptions(string.Empty, 0, 0, 8, 8, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new PathOptions(string.Empty, "M0 0L8 8ZM8 0L0 8Z", string.Empty, 1, color, 1));
        }

        /// <summary>
        /// Configures a diagonal forward pattern with lines running from lower-left to upper-right.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and diagonal lines.</param>
        /// <param name="color">The stroke color for the diagonal lines.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureDiagonalForwardPattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 6;
            pathOptions.Add(new RectOptions(null!, 0, 0, 6, 6, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new PathOptions(null!, "M 3 -3 L 9 3 M 6 6 L 0 0 M 3 9 L -3 3", null!, 2, color, opacity));
        }

        /// <summary>
        /// Configures a diagonal backward pattern with lines running from upper-left to lower-right.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and diagonal lines.</param>
        /// <param name="color">The stroke color for the diagonal lines.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureDiagonalBackwardPattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 6;
            pathOptions.Add(new RectOptions(null!, 0, 0, 6, 6, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new PathOptions(null!, "M 3 -3 L -3 3 M 0 6 L 6 0 M 9 3 L 3 9", null!, 2, color, opacity));
        }

        /// <summary>
        /// Configures a grid pattern with horizontal and vertical lines.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and grid lines.</param>
        /// <param name="color">The stroke color for the grid lines.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureGridPattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 6;
            pathOptions.Add(new RectOptions(null!, 0, 0, 6, 6, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new PathOptions(null!, "M1 3.5L11 3.5 M0 3.5L11 3.5 M0 7.5L11 7.5 M0 11.5L11 11.5 M5.5 0L5.5 12 M11.5 0L11.5 12Z", null!, 1, color, opacity));
        }

        /// <summary>
        /// Configures a turquoise pattern with circular elements arranged in a grid.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and turquoise circles.</param>
        /// <param name="color">The fill color for the circles.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureTurquoisePattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 17;
            pathOptions.Add(new RectOptions(null!, 0, 0, 17, 17, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new PathOptions(null!, "M0.5739999999999998,2.643a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null!, 1, color, opacity, color, "10"));
            pathOptions.Add(new PathOptions(null!, "M11.805,2.643a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null!, 1, color, opacity, color, "10"));
            pathOptions.Add(new PathOptions(null!, "M6.19,2.643a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null!, 1, color, opacity, color, "10"));
            pathOptions.Add(new PathOptions(null!, "M11.805,8.217a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null!, 1, color, opacity, color, "10"));
            pathOptions.Add(new PathOptions(null!, "M6.19,8.217a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null!, 1, color, opacity, color, "10"));
            pathOptions.Add(new PathOptions(null!, "M11.805,13.899a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null!, 1, color, opacity, color, "10"));
            pathOptions.Add(new PathOptions(null!, "M6.19,13.899a2.123,2.111 0 1,0 4.246,0a2.123,2.111 0 1,0 -4.246,0", null!, 1, color, opacity, color, "10"));
        }

        /// <summary>
        /// Configures a star pattern with star shapes repeated across the pattern.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and star shape.</param>
        /// <param name="color">The fill color for the star.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureStarPattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 21;
            pathOptions.Add(new RectOptions(null!, 0, 0, 21, 21, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new PathOptions(null!, "M15.913,18.59L10.762 12.842 5.613 18.75 8.291 11.422 0.325 9.91 8.154 8.33 5.337 0.91 10.488 6.658 15.637 0.75 12.959 8.078 20.925 9.59 13.096 11.17 z", null!, 1, color, opacity, color));
        }

        /// <summary>
        /// Configures a triangle pattern with triangle shapes repeated across the pattern.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and triangle shape.</param>
        /// <param name="color">The fill color for the triangle.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureTrianglePattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 10;
            pathOptions.Add(new RectOptions(null!, 0, 0, 10, 10, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new PathOptions(null!, "M4.987,0L7.48 4.847 9.974 9.694 4.987 9.694 0 9.694 2.493 4.847 z", null!, 1, color, opacity, color));
        }

        /// <summary>
        /// Configures a circle pattern with circular elements repeated across the pattern.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and circle ellipse.</param>
        /// <param name="color">The stroke color for the circle.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureCirclePattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            double circleNum = 9;
            patternGroup.Height = patternGroup.Width = circleNum;
            pathOptions.Add(new RectOptions(null!, 0, 0, circleNum, circleNum, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new EllipseOptions(null!, "3.625", "3.625", "5.125", "3.875", null!, 1, null!, opacity, color));
        }

        /// <summary>
        /// Configures a tile pattern with diagonal tile elements.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and tile paths.</param>
        /// <param name="color">The fill color for the tiles.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureTilePattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            double tileNum = 18;
            patternGroup.Height = patternGroup.Width = tileNum;
            pathOptions.Add(new RectOptions(null!, 0, 0, tileNum, tileNum, 0, null!, "#ffffff", opacity));
            pathOptions.Add(new PathOptions(null!, "M0,9L0 0 9 0 z", null!, 1, color, opacity, color));
            pathOptions.Add(new PathOptions(null!, "M9,9L9 0 18 0 z", null!, 1, color, opacity, color));
            pathOptions.Add(new PathOptions(null!, "M0,18L0 9 9 9 z", null!, 1, color, opacity, color));
            pathOptions.Add(new PathOptions(null!, "M9,18L9 9 18 9 z", null!, 1, color, opacity, color));
        }

        /// <summary>
        /// Configures a horizontal dash pattern with evenly spaced horizontal lines.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and horizontal dash lines.</param>
        /// <param name="color">The stroke color for the dashes.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureHorizontalDashPattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 12;
            pathOptions.Add(new RectOptions(null!, 0, 0, 12, 12, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new PathOptions(null!, "M0,1.5 L10 1.5 M0,5.5 L10 5.5 M0,9.5 L10 9.5 z", null!, 1, color, opacity, color));
        }

        /// <summary>
        /// Configures a vertical dash pattern with evenly spaced vertical lines.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and vertical dash lines.</param>
        /// <param name="color">The stroke color for the dashes.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureVerticalDashPattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 12;
            pathOptions.Add(new RectOptions(null!, 0, 0, 12, 12, 0, null!, "#ffffff", 0, 9, opacity));
            pathOptions.Add(new PathOptions(null!, "M1.5,0 L1.5 10 M5.5,0 L5.5 10 M9.5,0 L9.5 10 z", null!, 1, color, opacity, color));
        }

        /// <summary>
        /// Configures a rectangle pattern with rectangular elements.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and rectangle shapes.</param>
        /// <param name="color">The fill color for the rectangles.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureRectanglePattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = 12;
            patternGroup.Width = 10;
            pathOptions.Add(new RectOptions(null!, 0, 0, 10, 12, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new RectOptions(null!, 1, 2, 9, 4, 0, null!, color, 0, 0, opacity));
            pathOptions.Add(new RectOptions(null!, 7, 2, 9, 4, 0, null!, color, 0, 0, opacity));
        }

        /// <summary>
        /// Configures a box pattern with box elements.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and box shape.</param>
        /// <param name="color">The fill color for the box.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureBoxPattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 10;
            pathOptions.Add(new RectOptions(null!, 0, 0, 10, 12, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new RectOptions(null!, 1, 2, 9, 4, 0, null!, color, 0, 0, opacity));
        }

        /// <summary>
        /// Configures a horizontal stripe pattern with evenly spaced horizontal stripes.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and horizontal stripe lines.</param>
        /// <param name="color">The stroke color for the stripes.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureHorizontalStripePattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = 12;
            patternGroup.Width = 10;
            pathOptions.Add(new RectOptions(null!, 0, 0, 10, 12, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new PathOptions(null!, "M0,0.5 L10 0.5 M0,4.5 L10 4.5 M0,8.5 L10 8.5 z", null!, 1, color, opacity, color));
        }

        /// <summary>
        /// Configures a vertical stripe pattern with evenly spaced vertical stripes.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and vertical stripe lines.</param>
        /// <param name="color">The stroke color for the stripes.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureVerticalStripePattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = 12;
            patternGroup.Width = 10;
            pathOptions.Add(new RectOptions(null!, 0, 0, 10, 12, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new PathOptions(null!, "M0.5,0 L0.5 10 M4.5,0 L4.5 10 M8.5,0 L8.5 10 z", null!, 1, color, opacity, color));
        }

        /// <summary>
        /// Configures a bubble pattern with overlapping circular bubble shapes.
        /// </summary>
        /// <param name="patternGroup">The pattern options container to configure with dimensions.</param>
        /// <param name="pathOptions">The shape list to receive the background and bubble shapes.</param>
        /// <param name="color">The fill color for primary bubbles.</param>
        /// <param name="opacity">The opacity level for the shapes.</param>
        static void ConfigureBubblePattern(PatternOptions patternGroup, List<object> pathOptions, string color, double opacity)
        {
            patternGroup.Height = patternGroup.Width = 20;
            pathOptions.Add(new RectOptions(null!, 0, 0, 20, 20, 0, null!, "#ffffff", 0, 0, opacity));
            pathOptions.Add(new EllipseOptions(null!, "3.429", "3.429", "5.217", "11.325", null!, 1, null!, opacity, "#D0A6D1"));
            pathOptions.Add(new EllipseOptions(null!, "4.884", "4.884", "13.328", "6.24", null!, 1, null!, 1, color));
            pathOptions.Add(new EllipseOptions(null!, "3.018", "3.018", "13.277", "14.66", null!, 1, null!, opacity, "#D0A6D1"));
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Generates an SVG pattern for the specified selection style and returns a URL reference.
        /// </summary>
        /// <param name="color">The hex color code for the pattern (e.g., "#FF0000"). Must not be <see langword="null"/> or empty.</param>
        /// <param name="index">The zero-based index used to ensure unique pattern IDs across multiple instances. Must be non-negative.</param>
        /// <param name="patternName">The type of selection pattern to generate.</param>
        /// <param name="opacity">The opacity level (0.0 to 1.0) for the pattern elements.</param>
        /// <returns>A URL reference string in the format "url(#[PatternId])" for use in SVG fill attributes.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="color"/> is <see langword="null"/>, empty, or <paramref name="index"/> is negative.</exception>
        protected string FindPattern(string color, int index, SelectionPattern patternName, double opacity)
        {
            List<object> pathOptions = new List<object>();
            PatternOptions patternGroup = CreatePatternOptions(color, index, patternName);

            ConfigurePattern(patternName, patternGroup, pathOptions, color, opacity);

            patternGroup.ShapeOptions = pathOptions;
            ReqPatterns?.Add(patternGroup);
            return BuildPatternUrl(patternName, color, index);
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Appends the required selection patterns to the style renderer.
        /// </summary>
        /// <remarks>
        /// Triggers pattern drawing in the associated <see cref="SelectionStyleComponent"/>.
        /// This method should be called after all patterns have been added via <see cref="FindPattern"/>.
        /// </remarks>
        internal void AppendSelectionPattern()
        {
            StyleRender?.DrawPattern(ReqPatterns);
        }

        /// <summary>
        /// Disposes resources associated with this selection utility.
        /// </summary>
        /// <remarks>
        /// Clears the pattern list and style renderer to free memory. Call this when the component is disposed.
        /// </remarks>
        internal virtual void Dispose()
        {
            ReqPatterns?.Clear();
            StyleRender?.GivenPattern?.Clear();
            StyleRender = null!;
        }
        #endregion
    }
}