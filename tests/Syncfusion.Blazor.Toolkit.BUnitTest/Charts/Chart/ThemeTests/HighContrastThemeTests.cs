using System.Reflection;
using Bunit;
using Syncfusion.Blazor.Toolkit.Charts;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using Syncfusion.Blazor.Toolkit.Internal;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Charts.Themes
{
    /// <summary>
    /// Tests for the <c>Theme.HighContrast</c> value on <see cref="SfChart"/>.
    /// Verifies that:
    /// <list type="bullet">
    /// <item><description><c>ChartHelper.GetChartThemeStyle</c> returns the high-contrast token set when the theme is <c>HighContrast</c>.</description></item>
    /// <item><description><c>ChartHelper.GetSeriesColor</c> returns the high-contrast palette when the theme is <c>HighContrast</c>.</description></item>
    /// <item><description><c>ChartHelper.GetScrollbarThemeColor</c> returns the high-contrast scrollbar colors.</description></item>
    /// <item><description>Setting <c>SfChart.Theme="Theme.HighContrast"</c> re-derives the chart's internal <c>_chartThemeStyle</c>.</description></item>
    /// </list>
    /// </summary>
    public class HighContrastThemeTests : BunitTestContext
    {
        private const string HighContrast = "HighContrast";
        private const string Fluent = "Fluent";
        private const string FluentDark = "FluentDark";

        #region ChartHelper.GetChartThemeStyle

        [Fact(DisplayName = "GetChartThemeStyle returns a high-contrast Background (#000000) for HighContrast theme")]
        public void GetChartThemeStyle_HighContrast_HasBlackBackground()
        {
            ChartThemeStyle style = ChartHelper.GetChartThemeStyle(HighContrast);
            string? background = GetInternalString(style, "Background");
            Assert.Equal("#000000", background);
        }

        [Fact(DisplayName = "GetChartThemeStyle returns white AxisLine/AxisLabel for HighContrast theme")]
        public void GetChartThemeStyle_HighContrast_HasWhiteTextTokens()
        {
            ChartThemeStyle style = ChartHelper.GetChartThemeStyle(HighContrast);
            Assert.Equal("#FFFFFF", GetInternalString(style, "AxisLine"));
            Assert.Equal("#FFFFFF", GetInternalString(style, "AxisLabel"));
            Assert.Equal("#FFFFFF", GetInternalString(style, "ChartTitle"));
            Assert.Equal("#FFFFFF", GetInternalString(style, "LegendLabel"));
        }

        [Fact(DisplayName = "GetChartThemeStyle returns yellow focus tokens (#FFFF00) for HighContrast theme")]
        public void GetChartThemeStyle_HighContrast_HasYellowFocusTokens()
        {
            ChartThemeStyle style = ChartHelper.GetChartThemeStyle(HighContrast);
            Assert.Equal("#FFFF00", GetInternalString(style, "ErrorBar"));
            Assert.Equal("#FFFF00", GetInternalString(style, "SelectionRectStroke"));
        }

        [Fact(DisplayName = "GetChartThemeStyle returns #000000 major grid lines for HighContrast theme")]
        public void GetChartThemeStyle_HighContrast_HasBlackGridLines()
        {
            ChartThemeStyle style = ChartHelper.GetChartThemeStyle(HighContrast);
            Assert.Equal("#000000", GetInternalString(style, "MajorGridLine"));
            Assert.Equal("#000000", GetInternalString(style, "MinorGridLine"));
        }

        [Fact(DisplayName = "GetChartThemeStyle still returns light tokens for Fluent (no regression)")]
        public void GetChartThemeStyle_Fluent_StillReturnsLightTokens()
        {
            ChartThemeStyle style = ChartHelper.GetChartThemeStyle(Fluent);
            Assert.Equal("#FFFFFF", GetInternalString(style, "Background"));
            Assert.Equal("#242424", GetInternalString(style, "AxisLabel"));
        }

        [Fact(DisplayName = "GetChartThemeStyle still returns dark tokens for FluentDark (no regression)")]
        public void GetChartThemeStyle_FluentDark_StillReturnsDarkTokens()
        {
            ChartThemeStyle style = ChartHelper.GetChartThemeStyle(FluentDark);
            Assert.Equal("#1c1b1f", GetInternalString(style, "Background"));
            Assert.Equal("#FFFFFF", GetInternalString(style, "AxisLabel"));
        }

        #endregion

        #region ChartHelper.GetSeriesColor

        [Fact(DisplayName = "GetSeriesColor returns the HighContrast palette for HighContrast theme")]
        public void GetSeriesColor_HighContrast_ReturnsHighContrastPalette()
        {
            string[] palette = ChartHelper.GetSeriesColor(HighContrast);
            Assert.NotNull(palette);
            Assert.NotEmpty(palette);
            // First color is the canonical primary: yellow #FFFF00
            Assert.Equal("#FFFF00", palette[0]);
        }

        [Fact(DisplayName = "GetSeriesColor HighContrast palette contains cyan, magenta, and green for visual differentiation")]
        public void GetSeriesColor_HighContrast_PaletteHasDistinctColors()
        {
            string[] palette = ChartHelper.GetSeriesColor(HighContrast);
            Assert.Contains("#00FFFF", palette); // cyan
            Assert.Contains("#FF00FF", palette); // magenta
            Assert.Contains("#00FF00", palette); // green
        }

        [Fact(DisplayName = "GetSeriesColor still returns the Fluent palette when theme is Fluent")]
        public void GetSeriesColor_Fluent_StillReturnsFluentPalette()
        {
            string[] palette = ChartHelper.GetSeriesColor(Fluent);
            Assert.Equal("#6200EE", palette[0]);
        }

        #endregion

        #region ChartHelper.GetScrollbarThemeColor

        [Fact(DisplayName = "GetScrollbarThemeColor returns a yellow arrow/grip and black backRect for HighContrast theme")]
        public void GetScrollbarThemeColor_HighContrast_ReturnsHighContrastTokens()
        {
            ScrollbarThemeStyle style = ChartHelper.GetScrollbarThemeColor(HighContrast);
            Assert.Equal("#000000", style.BackRect);
            Assert.Equal("#FFFF00", style.Arrow);
            Assert.Equal("#FFFF00", style.Grip);
        }

        [Fact(DisplayName = "GetScrollbarThemeColor still returns the dark variant when theme is FluentDark")]
        public void GetScrollbarThemeColor_FluentDark_StillReturnsDarkTokens()
        {
            ScrollbarThemeStyle style = ChartHelper.GetScrollbarThemeColor(FluentDark);
            Assert.Equal("#0A0A0A", style.BackRect);
            Assert.Equal("#D6D6D6", style.Arrow);
        }

        #endregion

        #region SfChart parameter binding

        [Fact(DisplayName = "SfChart with Theme=HighContrast derives a high-contrast _chartThemeStyle")]
        public void SfChart_HighContrast_AppliesHighContrastThemeStyle()
        {
            var cut = RenderComponent<SfChart>(parameters => parameters.Add(p => p.Theme, Theme.HighContrast));

            ChartThemeStyle? style = GetInternalChartThemeStyle(cut.Instance);
            Assert.NotNull(style);
            Assert.Equal("#000000", GetInternalString(style!, "Background"));
            Assert.Equal("#FFFF00", GetInternalString(style!, "ErrorBar"));
        }

        [Fact(DisplayName = "SfChart switches from Fluent to HighContrast and updates _chartThemeStyle")]
        public void SfChart_ThemeParameter_UpdatesChartThemeStyle()
        {
            var cut = RenderComponent<SfChart>();

            // First render: default is Fluent → white background.
            ChartThemeStyle? initialStyle = GetInternalChartThemeStyle(cut.Instance);
            Assert.NotNull(initialStyle);
            Assert.Equal("#FFFFFF", GetInternalString(initialStyle!, "Background"));

            // Switch to HighContrast → black background.
            cut.SetParametersAndRender(parameters => parameters.Add(p => p.Theme, Theme.HighContrast));
            ChartThemeStyle? highContrastStyle = GetInternalChartThemeStyle(cut.Instance);
            Assert.NotNull(highContrastStyle);
            Assert.Equal("#000000", GetInternalString(highContrastStyle!, "Background"));
            Assert.Equal("#FFFF00", GetInternalString(highContrastStyle!, "ErrorBar"));
        }

        #endregion

        #region Test helpers

        /// <summary>
        /// Reads a string property declared as <c>internal</c> on a public type via reflection.
        /// </summary>
        private static string? GetInternalString(object instance, string propertyName)
        {
            PropertyInfo? property = instance.GetType().GetProperty(
                propertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Assert.NotNull(property);
            return property!.GetValue(instance) as string;
        }

        /// <summary>
        /// Reads the <c>_chartThemeStyle</c> field on the <see cref="SfChart"/> instance.
        /// </summary>
        private static ChartThemeStyle? GetInternalChartThemeStyle(SfChart chart)
        {
            FieldInfo? field = typeof(SfChart).GetField(
                "_chartThemeStyle",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field);
            return field!.GetValue(chart) as ChartThemeStyle;
        }

        #endregion
    }
}
