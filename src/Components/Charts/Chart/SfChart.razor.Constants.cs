using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides constant values used throughout the chart component.
    /// </summary>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public static partial class Constants
    {
        #region JavaScript Interop Constants
        internal const string FocusTarget = "focusTarget";
        internal const string GetElementBoundsById = "getElementBoundsById";
        internal const string GetParentElementBoundsById = "getParentElementBoundsById";
        internal const string SetAttribute = "setAttribute";
        internal const string SetHighlightSelectionOptions = "setHighlightSelectionOptions";
        internal const string DragRemove = "dragRemove";
        internal const string SetTooltipOptions = "setTooltipOptions";
        internal const string SelectDataIndex = "selectDataIndex";
        internal const string DoInitialAnimation = "doInitialAnimation";
        internal const string GetDatalabelTemplateBoundsById = "getDatalabelTemplateSize";
        internal const string SetSvgDimensions = "setSvgDimensions";
        internal const string UpdateZoomingOptions = "updateZoomingOptions";
        internal const string RemoveScrollbarSvg = "removeScrollbarSvg";
        #endregion

        #region UI Element Constants
        internal const string Transparent = "transparent";
        internal const string PrimaryXAxis = "PrimaryXAxis";
        internal const string PrimaryYAxis = "PrimaryYAxis";
        internal const string Svg = "_svg";
        internal const string Underscore = "_";
        #endregion

        #region Event Name Constants

        internal const string PointRender = "OnPointRender";
        internal const string AxisMultiLevelLabelRender = "OnAxisMultiLevelLabelRender";
        internal const string PointerClick = "PointerClick";
        internal const string OnZooming = "OnZooming";
        internal const string OnZoomStart = "OnZoomStart";
        internal const string OnZoomEnd = "OnZoomEnd";
        internal const string OnSelectionChanged = "OnSelectionChanged";
        #endregion

        #region Grid Line Constants
        internal const string MajorGridLine = "MajorGridLines";
        internal const string MajorTickLine = "MajorTickLine";
        internal const string MinorGridLine = "MinorGridLine";
        internal const string MinorTickLine = "MinorTickLine";
        #endregion

        #region Numeric Constants
        internal const int AxisPadding = 5;
        internal const int ScrollbarPadding = 10;
        internal const int ChartMarkerCount = 10;
        internal const int TouchOffset = 20;
        internal const int MouseOffset = 30;
        internal const int ClickThersholdMs = 200;
        internal const int PointerThersholdMs = 300;
        internal const int UpdateThersholdMs = 100;
        internal const int ClickDelayMs = 300;
        #endregion

        #region Chart Default Value Constants
        internal const double MarkerSize = 5;
        internal const double DefaultOpacity = 1;
        internal const double DefaultBorderWidth = 1;
        internal const double DefaultCornerRadius = 5;
        internal const double DefaultMargin = 5;
        internal const double TrendlinePeriod = 2;
        internal const double TrendlinePolynomialOrder = 2;
        internal const double TrendlineWidth = 1;
        internal const double DefaultBrighten = 1;
        #endregion

        #region Regular Expression Patterns
        internal const string SubPattern = @"~\d+~";
        internal const string SupPattern = @"\^\d+\^";
        internal const string NumPattern = @"[^0-9\.]+";

        [GeneratedRegex(SubPattern)]
        internal static partial Regex SubRegex();

        [GeneratedRegex(SupPattern)]
        internal static partial Regex SupRegex();

        [GeneratedRegex(NumPattern)]
        internal static partial Regex NumRegex();
        #endregion
    }
}