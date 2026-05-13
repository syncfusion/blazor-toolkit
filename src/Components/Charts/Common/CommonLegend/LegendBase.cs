using Syncfusion.Blazor.Toolkit.Charts.Internal;
using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Provides shared legend layout, measurement, and rendering logic for chart components.
    /// </summary>
    /// <remarks>
    /// This type is intended for internal infrastructure use and is not part of the public API surface.
    /// </remarks>
    /// <example>
    /// This class is used by chart renderers to compute legend layout and render legend items.
    /// </example>
    /// <exclude />
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public class LegendBase : ChartRenderer
    {
        #region Constants
        const double PAGE_BUTTON_SIZE = 8;
        #endregion

        #region Fields
        string _baseControl { get; set; } = string.Empty;
        string _pagingTransform { get; set; } = string.Empty;
        protected CultureInfo culture { get; set; } = CultureInfo.InvariantCulture;
        #endregion

        #region Properties

        /// <summary>
        /// Represents the legend template options for an individual legend item.
        /// </summary>
        internal class LegendItemTemplateOptions
        {
            /// <summary>
            /// Gets or sets the template element identifier.
            /// </summary>
            /// <value>The template element identifier.</value>
            public string? Id { get; set; }

            /// <summary>
            /// Gets or sets the template inline style string.
            /// </summary>
            /// <value>The inline style string.</value>
            public string? style { get; set; }

            /// <summary>
            /// Gets or sets the legend template fragment.
            /// </summary>
            /// <value>The legend template fragment.</value>
            public RenderFragment? LegendTemplate { get; set; }
        }

        /// <summary>
        /// Gets or sets the collection of legend options.
        /// </summary>
        /// <value>The legend option collection.</value>
        internal List<LegendOption> LegendCollection { get; set; } = new List<LegendOption>();

        /// <summary>
        /// Gets or sets the legend position.
        /// </summary>
        /// <value>The legend position.</value>
        internal LegendPosition Position { get; set; }

        /// <summary>
        /// Gets or sets the legend bounds.
        /// </summary>
        /// <value>The legend bounds.</value>
        internal Rect LegendBounds { get; set; } = new Rect();

        /// <summary>
        /// Gets or sets the maximum row width.
        /// </summary>
        /// <value>The maximum row width.</value>
        internal double MaxRowWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the legend is rendered vertically.
        /// </summary>
        /// <value><see langword="true"/> if vertical; otherwise <see langword="false"/>.</value>
        protected bool IsVertical { get; set; }

        /// <summary>
        /// Gets or sets the item padding for legend layout.
        /// </summary>
        /// <value>The item padding value.</value>
        protected double ItemPadding { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether top/bottom legends are vertical.
        /// </summary>
        /// <value><see langword="true"/> if vertical; otherwise <see langword="false"/>.</value>
        protected bool IsTopBottomVertical { get; set; }

        /// <summary>
        /// Gets or sets the row heights collection.
        /// </summary>
        /// <value>The row heights collection.</value>
        protected List<double>? RowHeights { get; set; }

        /// <summary>
        /// Gets or sets the page heights collection.
        /// </summary>
        /// <value>The page heights collection.</value>
        protected List<double>? PageHeights { get; set; }

        /// <summary>
        /// Gets or sets the column heights collection.
        /// </summary>
        /// <value>The column heights collection.</value>
        protected List<double>? ColumnHeights { get; set; }

        /// <summary>
        /// Gets or sets the chart row count.
        /// </summary>
        /// <value>The chart row count.</value>
        protected int ChartRowCount { get; set; }

        /// <summary>
        /// Gets or sets the current legend page location index.
        /// </summary>
        /// <value>The current legend page location index.</value>
        protected int CurrentLegendPageLocationIndex { get; set; }

        /// <summary>
        /// Gets or sets the maximum item height.
        /// </summary>
        /// <value>The maximum item height.</value>
        protected double MaxItemHeight { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether paging is enabled.
        /// </summary>
        /// <value><see langword="true"/> if paging is enabled; otherwise <see langword="false"/>.</value>
        protected bool IsPaging { get; set; }

        /// <summary>
        /// Gets or sets the page X collections.
        /// </summary>
        /// <value>The page X collections.</value>
        protected List<double> PageXCollections { get; set; } = new List<double>();

        /// <summary>
        /// Gets or sets the paging regions.
        /// </summary>
        /// <value>The paging regions.</value>
        protected List<Rect> PagingRegions { get; set; } = new List<Rect>();

        /// <summary>
        /// Gets or sets the legend element identifier.
        /// </summary>
        /// <value>The legend element identifier.</value>
        internal string? LegendID { get; set; }

        /// <summary>
        /// Gets or sets the maximum width.
        /// </summary>
        /// <value>The maximum width.</value>
        protected double MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the clip path height.
        /// </summary>
        /// <value>The clip path height.</value>
        protected double ClipPathHeight { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        /// <value>The current page number.</value>
        protected double CurrentPageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the page up button identifier.
        /// </summary>
        /// <value>The page up button identifier.</value>
        protected string? PageUpID { get; set; }

        /// <summary>
        /// Gets or sets the page down button identifier.
        /// </summary>
        /// <value>The page down button identifier.</value>
        protected string? PageDownID { get; set; }

        /// <summary>
        /// Gets or sets the page number identifier.
        /// </summary>
        /// <value>The page number identifier.</value>
        protected string? PageNumberID { get; set; }

        /// <summary>
        /// Gets or sets the legend translate group identifier.
        /// </summary>
        /// <value>The legend translate group identifier.</value>
        protected string? LegendTranslateID { get; set; }

        /// <summary>
        /// Gets or sets the legend base reference.
        /// </summary>
        /// <value>The legend base reference.</value>
        protected ILegendMethods? BaseLegendRef { get; set; }

        /// <summary>
        /// Gets or sets the transform string for legend paging.
        /// </summary>
        /// <value>The transform string.</value>
        protected string Transform { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the total page count.
        /// </summary>
        /// <value>The total page count.</value>
        protected double TotalPageCount { get; set; }

        /// <summary>
        /// Gets or sets the row count per page.
        /// </summary>
        /// <value>The row count per page.</value>
        protected double RowCountPerPage { get; set; }

        /// <summary>
        /// Gets or sets the theme style.
        /// </summary>
        /// <value>The chart theme style.</value>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected ChartThemeStyle? ThemeStyle { get; set; }

        /// <summary>
        /// Gets or sets the legend symbol options.
        /// </summary>
        /// <value>The legend symbol options.</value>
        internal List<LegendSymbols> LegendOptions { get; set; } = new List<LegendSymbols>();

        /// <summary>
        /// Gets or sets the template options.
        /// </summary>
        /// <value>The template options.</value>
        internal List<LegendItemTemplateOptions> TemplateOptions { get; set; } = new List<LegendItemTemplateOptions>();

        /// <summary>
        /// Gets or sets the paging options.
        /// </summary>
        /// <value>The paging options.</value>
        protected List<LegendSymbols> PagingOptions { get; set; } = new List<LegendSymbols>();

        /// <summary>
        /// Gets or sets the legend instance.
        /// </summary>
        /// <value>The legend instance.</value>
        internal ILegendBase? Legend { get; set; }

        /// <summary>
        /// Gets or sets the legend item rectangles.
        /// </summary>
        /// <value>The legend item rectangles.</value>
        internal List<Rect> LegendCollectionRect { get; set; } = new List<Rect>();

        /// <summary>
        /// Gets or sets the paging rectangle.
        /// </summary>
        /// <value>The paging rectangle.</value>
        internal Rect? PagingRect { get; set; }

        /// <summary>
        /// Gets or sets the chart identifier.
        /// </summary>
        /// <value>The chart identifier.</value>
        protected string? ChartId { get; set; }

        /// <summary>
        /// Enable / Disable for inverse the legend text and symbol.
        /// </summary>
        /// <value><see langword="true"/> to inverse; otherwise <see langword="false"/>.</value>
        protected bool IsInverse { get; set; }

        /// <summary>
        /// Whether find the RTL is enable in the chart.
        /// </summary>
        /// <value><see langword="true"/> if RTL; otherwise <see langword="false"/>.</value>
        protected bool IsRTL { get; set; }

        /// <summary>
        /// Enable / Disable for reverse the legend group order.
        /// </summary>
        /// <value><see langword="true"/> to reverse order; otherwise <see langword="false"/>.</value>
        protected bool Reverse { get; set; }

        /// <summary>
        /// Specifies the legend border width.
        /// </summary>
        /// <value>The legend border width.</value>
        protected double BorderWidth { get; set; }

        /// <summary>
        /// Gets or sets the legend text style.
        /// </summary>
        /// <value>The legend text style.</value>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected ChartFontOptions? LegendTextStyle { get; set; }

        /// <summary>
        /// Gets or sets the page start Y value.
        /// </summary>
        /// <value>The page start Y value.</value>
        protected double PageStartY { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates custom legend template options for the given legend item.
        /// </summary>
        /// <param name="legendOption">The legend option instance.</param>
        /// <param name="index">The legend item index.</param>
        /// <param name="pointerValue">The pointer cursor value.</param>
        /// <param name="legend">The legend instance.</param>
        void CreateCustomLegendTemplate(LegendOption legendOption, int index, string pointerValue, ILegendBase legend)
        {
            TextOptions currentTextOption = LegendOptions[index].TextOption;
            string id = Owner?.ID + "_chart_legend_template_" + Convert.ToString(legendOption.SeriesIndex, null);
            legendOption.TemplateID = id;
            string visibility = legendOption.TemplateSize?.Width > 0 && legendOption.TemplateSize.Height > 0 ? "visible" : "hidden";
            string style = "position: absolute; visibility: " + visibility + ";" + " left: " + currentTextOption.X + "px;" + " top: " + currentTextOption.Y + "px;" + " pointer-events: bounding-box; cursor: " + pointerValue + "; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;";

            TemplateOptions.Add(new LegendItemTemplateOptions()
            {
                Id = id,
                style = style,
                LegendTemplate = legendOption.LegendTemplate
            });
        }

        /// <summary>
        /// Calculates paging elements with text options.
        /// </summary>
        /// <param name="textOption">The text options used for paging.</param>
        void CalculatePagingElements(TextOptions textOption)
        {
            PagingRegions = new List<Rect>();
            CurrentPageNumber = CurrentPageNumber > 1 && CurrentPageNumber > TotalPageCount ? TotalPageCount : CurrentPageNumber;

            ChartFontOptions font = new ChartFontOptions
            {
                Size = textOption.FontSize,
                Color = textOption.Fill,
                FontFamily = textOption.FontFamily,
                FontStyle = textOption.FontStyle,
                FontWeight = textOption.FontWeight
            };

            Size size = ChartHelper.MeasureText(TotalPageCount + "/" + TotalPageCount, font);
            double iconSize = PAGE_BUTTON_SIZE;
            double y = LegendBounds.Y + ClipPathHeight + ((LegendBounds.Height - ClipPathHeight) / 2);
            double transformX = IsRTL ? BorderWidth + (iconSize / 2) : LegendBounds.Width - ((2 * (iconSize + 8)) + 8 + size.Width);
            _pagingTransform = "translate(" + transformX.ToString(culture) + ", " + 0 + ")";

            InitializePagingSymbols(textOption, size, iconSize, y);
            InitializePagingRegions(textOption, size, iconSize, y);
        }

        /// <summary>
        /// Initializes paging symbols and text options.
        /// </summary>
        /// <param name="textOption">The text options.</param>
        /// <param name="size">The text size.</param>
        /// <param name="iconSize">The icon size.</param>
        /// <param name="y">The y location.</param>
        void InitializePagingSymbols(TextOptions textOption, Size size, double iconSize, double y)
        {
            PathOptions symbolOption = new PathOptions(!IsRTL ? PageUpID ?? string.Empty : PageDownID ?? string.Empty, string.Empty, string.Empty, 5, "#545454", 1, Constants.Transparent, string.Empty, string.Empty, "Legend paging", "-1");
            double x = LegendBounds.X + (iconSize / 2);

            PagingOptions.Add(new LegendSymbols()
            {
                FirstSymbol = CalculateSymbol(new ChartEventLocation(x, y), "LeftArrow", new Size(iconSize, iconSize), string.Empty, symbolOption)
            });

            textOption.X = Convert.ToString(x + (iconSize / 2) + 8, culture);
            textOption.Y = Convert.ToString(y + (size.Height / 4), culture);
            textOption.Id = PageNumberID ?? string.Empty;
            textOption.Text = !IsRTL ? CurrentPageNumber + "/" + TotalPageCount : TotalPageCount + "/" + CurrentPageNumber;
            PagingOptions[0].TextOption = textOption;

            x = Convert.ToDouble(textOption.X, culture) + 8 + (iconSize / 2) + size.Width;
            symbolOption = new PathOptions(!IsRTL ? PageDownID ?? string.Empty : PageUpID ?? string.Empty, string.Empty, string.Empty, 5, Constants.Transparent, 1, "#545454");
            PagingOptions[0].SecondSymbol = CalculateSymbol(new ChartEventLocation(x, y), "RightArrow", new Size(iconSize, iconSize), string.Empty, symbolOption);
        }

        /// <summary>
        /// Initializes paging hit regions and bounding rectangle.
        /// </summary>
        /// <param name="textOption">The text options.</param>
        /// <param name="size">The text size.</param>
        /// <param name="iconSize">The icon size.</param>
        /// <param name="y">The y location.</param>
        void InitializePagingRegions(TextOptions textOption, Size size, double iconSize, double y)
        {
            double regionOffset = LegendBounds.Width - ((2 * (iconSize + 8)) + 8 + size.Width);
            double x = LegendBounds.X + (iconSize / 2);

            PagingRegions.Add(new Rect(x + regionOffset - (iconSize * 0.5), y - (iconSize * 0.5), iconSize, iconSize));

            x = Convert.ToDouble(textOption.X, culture) + 8 + (iconSize / 2) + size.Width;
            PagingRegions.Add(new Rect(x + regionOffset - (iconSize * 0.5), y - (iconSize * 0.5), iconSize, iconSize));

            PagingRect = new Rect()
            {
                X = LegendBounds.X + (iconSize / 2) + regionOffset - (iconSize * 0.5),
                Y = y - (iconSize * 0.5) - size.Height / 4,
                Width = size.Width * 2.7,
                Height = size.Height
            };
        }

        /// <summary>
        /// Renders a symbol element based on the provided symbol options.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        /// <param name="symbolOption">The symbol options.</param>
        void RenderSymbol(RenderTreeBuilder builder, SymbolOptions symbolOption)
        {
            if (symbolOption.ShapeName == ShapeName.ellipse)
            {
                Owner?._svgRenderer?.RenderEllipse(builder, symbolOption.EllipseOption);
            }
            else if (symbolOption.ShapeName == ShapeName.path)
            {
                Owner?._svgRenderer?.RenderPath(builder, symbolOption.PathOption);
            }
            else if (symbolOption.ShapeName == ShapeName.image)
            {
                Owner?._svgRenderer?.RenderImage(builder, symbolOption.ImageOption);
            }
        }

        /// <summary>
        /// Renders paging elements for the legend.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        /// <param name="svgRenderer">The SVG renderer.</param>
        void RenderPagingElements(RenderTreeBuilder builder, SvgRendering svgRenderer)
        {
            svgRenderer.OpenGroupElement(builder, LegendID + "_navigation", _pagingTransform, string.Empty, "e-legend-cursor");
            foreach (LegendSymbols pagingOption in PagingOptions.ToArray())
            {
                pagingOption.FirstSymbol.PathOption.TabIndex = "0";
                RenderSymbol(builder, pagingOption.FirstSymbol);
                RenderSymbol(builder, pagingOption.SecondSymbol);
                svgRenderer.RenderText(builder, pagingOption.TextOption);
            }

            builder.CloseElement();
        }

        /// <summary>
        /// Determines the X coordinate for legend text based on RTL, inverse layout, and template placement requirements.
        /// </summary>
        /// <param name="legendOption">The legend option containing location and template metadata.</param>
        /// <param name="textWidth">The measured width of the legend text in pixels.</param>
        /// <returns>The resolved X coordinate expressed as an invariant culture string.</returns>
        string ResolveLegendTextX(LegendOption legendOption, double textWidth)
        {
            if (IsInverse && !IsRTL)
            {
                return Convert.ToString(legendOption.Location.X - (legendOption.LegendTemplate is null ? ((Legend?.ShapeWidth ?? 0) / 2) : 0), culture);
            }
            else if (IsRTL && !IsInverse)
            {
                return Convert.ToString(legendOption.Location.X - (textWidth + (legendOption.LegendTemplate is null ? (Legend?.ShapeWidth / 2) + Legend?.ShapePadding : 0)), culture) ?? null!;
            }
            else if (IsInverse && IsRTL)
            {
                return Convert.ToString(legendOption.Location.X - (textWidth - (Legend?.ShapeWidth / 2)), culture) ?? null!; ;
            }
            else
            {
                return Convert.ToString(legendOption.Location.X + (legendOption.LegendTemplate is null ? (Legend?.ShapeWidth / 2) + (Legend?.ShapePadding ?? 0) : 0), culture) ?? null!;
            }
        }

        /// <summary>
        /// Renders the legend container, clip-path, and collection grouping elements that host legend content.
        /// </summary>
        /// <param name="builder">The render tree builder that receives the SVG element hierarchy.</param>
        /// <param name="svgRenderer">The SVG renderer responsible for drawing primitive shapes.</param>
        /// <param name="legendBorder">The configured border settings applied to the legend background.</param>
        void CreateLegendElements(RenderTreeBuilder builder, SvgRendering svgRenderer, ChartDefaultBorder legendBorder)
        {
            string clipPath = LegendID + "_clipPath";
            RectOptions Option = new RectOptions(LegendID + "_element", LegendBounds.X, LegendBounds.Y, LegendBounds.Width, LegendBounds.Height, legendBorder.Width, legendBorder.Color, Legend?.Background ?? string.Empty, 0, 0, Legend?.Opacity ?? 1, string.Empty, "pointer-events: none; cursor: " + (Legend is not null && Legend.ToggleVisibility ? "default" : "pointer"));
            svgRenderer.RenderRect(builder, Option);
            svgRenderer.OpenClipPath(builder, svgRenderer.Seq++, clipPath);
            Option.Id = clipPath + "_rect";
            Option.Width = LegendBounds.Width;

            if (IsPaging)
            {
                Option.Height = LegendCollection.Any(a => a.LegendTemplate is not null) ? ClipPathHeight : Math.Max(1, RowCountPerPage - 1) * (MaxItemHeight + (Legend?.Padding ?? 0));
                Transform = "translate(0,-" + (ClipPathHeight * (CurrentPageNumber - 1)).ToString(culture) + ")";
            }
            else
            {
                Transform = "translate(0, -0)";
            }

            svgRenderer.RenderRect(builder, Option);
            builder.CloseElement();
            svgRenderer.OpenGroupElement(builder, LegendID + "_collections", string.Empty, "url(#" + clipPath + ")", string.Empty, string.Empty, string.Empty, "false");
            svgRenderer.OpenGroupElement(builder, LegendTranslateID ?? string.Empty, Transform, string.Empty, string.Empty, string.Empty, string.Empty, "false");
        }

        /// <summary>
        /// Resolves the legend marker fill color for gradient series.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <param name="defaultFill">The default fill color.</param>
        /// <returns>The resolved fill color.</returns>
        string ResolveLegendMarkerFill(LegendOption legendOption, string defaultFill)
        {
            ChartSeries? series = Owner?._visibleSeriesRenderers?.ElementAtOrDefault((int)legendOption.SeriesIndex)?.Series;
            if (ChartHelper.NeedsLegendHorizontalLineGradient(series))
            {
                return Owner?._visibleSeriesRenderers?.ElementAtOrDefault((int)legendOption.SeriesIndex)?.Interior ?? defaultFill;
            }
            return defaultFill;
        }

        /// <summary>
        /// Finds the index of the first renderable legend item.
        /// </summary>
        /// <returns>The first legend index.</returns>
        int FindFirstLegendPosition()
        {
            int count = 0;
            foreach (LegendOption Legend in LegendCollection.ToArray())
            {
                if (Legend.Render && (!string.IsNullOrEmpty(Legend.Text) || Legend.LegendTemplate is not null))
                {
                    break;
                }

                count++;
            }

            return count;
        }

        /// <summary>
        /// Gets position (Left/Right/Top/Bottom) for legend defaulting behavior.
        /// </summary>
        /// <param name="availableSize">Available chart size.</param>
        /// <param name="_baseControl">Base control name.</param>
        /// <param name="seriesType">Series type.</param>
        void GetPosition(Size availableSize, string _baseControl, string seriesType)
        {
            if (_baseControl == "Chart")
            {
                Position = (Position != LegendPosition.Auto) ? Position : LegendPosition.Bottom;
            }
            else
            {
                if (Position == LegendPosition.Auto && (seriesType == "Funnel" || seriesType == "Pyramid"))
                {
                    Position = LegendPosition.Top;
                }

                Position = (Position != LegendPosition.Auto) ? Position : (availableSize.Width > availableSize.Height ? LegendPosition.Right : LegendPosition.Bottom);
            }
        }

        /// <summary>
        /// Setup start point and paging metrics.
        /// </summary>
        /// <param name="firstLegend">Index of first legend.</param>
        /// <param name="textPadding">Output text padding.</param>
        /// <param name="pointerValue">Output pointer value.</param>
        /// <returns>Computed start point for legend items.</returns>
        ChartEventLocation SetupStartAndPagingMetrics(int firstLegend, out double textPadding, out string pointerValue)
        {
            int count = 0;
            double x_Align = 0;

            if (!string.IsNullOrEmpty(Legend?.Width) && MaxRowWidth < LegendBounds.Width && !IsVertical)
            {
                x_Align = (LegendBounds.Width - MaxRowWidth) / 2;
            }

            double x_Location = (!IsRTL) ? LegendBounds.X + x_Align + (Legend?.Padding ?? 0) + (LegendCollection[0].LegendTemplate is null ? ((Legend?.ShapeWidth ?? 0) / 2) : 0)
                : (LegendBounds.X + LegendBounds.Width) - ((Legend?.Padding ?? 0) + (LegendCollection[0].LegendTemplate is null ? ((Legend?.ShapeWidth ?? 0) / 2) : 0) + x_Align);

            ChartEventLocation start = new ChartEventLocation(x_Location, LegendBounds.Y + (Legend?.Padding ?? 0) + (MaxItemHeight / 2));
            textPadding = (LegendCollection[0].LegendTemplate is null ? ((Legend?.ShapePadding ?? 0) + (Legend?.ShapeWidth ?? 0)) : 0) + (!IsVertical ? ItemPadding : (Legend?.Padding ?? 0));
            LegendCollection[firstLegend].Location = start;

            ClipPathHeight = IsPaging ? Math.Max(1, RowCountPerPage - 1) * (MaxItemHeight + (Legend?.Padding ?? 0)) : LegendBounds.Height;
            PageStartY = 0;

            if (LegendCollection.Any(legend => legend.LegendTemplate is not null))
            {
                CurrentLegendPageLocationIndex = 1;
                TotalPageCount = 1;
            }

            pointerValue = Owner is not null && Owner._legendRenderer is not null && Owner._legendRenderer.LegendSettings is not null && Owner._legendRenderer.LegendSettings.ToggleVisibility ? "pointer" : "default";

            return start;
        }

        /// <summary>
        /// Build symbol and template data for the legend collection.
        /// </summary>
        /// <param name="start">Start location.</param>
        /// <param name="textPadding">Text padding.</param>
        /// <param name="pointerValue">Pointer cursor string.</param>
        /// <param name="firstLegend">First renderable legend index.</param>
        void BuildLegendSymbolsAndTemplates(ChartEventLocation start, double textPadding, string pointerValue, int firstLegend)
        {
            int count = 0;
            LegendOption PreviousLegend = LegendCollection[firstLegend];

            foreach (LegendOption legendOption in LegendCollection.ToArray())
            {
                if (legendOption.Render && (!string.IsNullOrEmpty(legendOption.Text) || legendOption.LegendTemplate is not null))
                {
                    int legendIndex = !Reverse ? count : (LegendCollection.Count - 1) - count;
                    BaseLegendRef?.GetRenderPoint(legendOption, start, textPadding, PreviousLegend, count, firstLegend);
                    List<SymbolOptions> symbols = CalculateLegendOptions(legendOption, (int)legendOption.SeriesIndex);

                    LegendOptions.Add(new LegendSymbols()
                    {
                        FirstSymbol = symbols[0],
                        SecondSymbol = symbols[1],
                        TextOption = CalculateText(legendOption, (int)legendOption.SeriesIndex, ThemeStyle?.LegendLabel ?? null!),
                        Index = (int)legendOption.SeriesIndex,
                        Template = legendOption.LegendTemplate
                    });

                    if (legendOption.LegendTemplate is not null)
                    {
                        CreateCustomLegendTemplate(legendOption, count, pointerValue, Legend ?? null!);
                    }

                    PreviousLegend = legendOption;
                }

                count++;
            }
        }

        /// <summary>
        /// Build rectangle hit areas for legend items.
        /// </summary>
        void BuildLegendRects()
        {
            int count = 0;

            foreach (LegendOption legendOption in LegendCollection.ToArray())
            {
                LegendCollectionRect.Add(new Rect(
                    legendOption.Location.X - (legendOption.LegendTemplate is null ? (Legend?.ShapeWidth ?? 0) : 0) - ItemPadding,
                    legendOption.Location.Y, 0, 0 )
                );

                if (count < LegendCollection.Count - 1)
                {
                    LegendCollectionRect[count].Width = LegendCollection[count + 1].Location.X - LegendCollection[count].Location.X;
                }
                else
                {
                    LegendCollectionRect[count].Width = LegendBounds.X + LegendBounds.Width - ItemPadding - LegendCollection[count].Location.X;
                }

                count++;
            }
        }

        /// <summary>
        /// Handles legend paging: template visibility and text offsets.
        /// </summary>
        /// <param name="pointerValue">Pointer cursor string.</param>
        void HandlePaging(string pointerValue)
        {
            if (!IsPaging)
            {
                return;
            }

            double yValue;

            foreach (LegendOption legendOption in LegendCollection)
            {
                if (CurrentPageNumber > 1 && legendOption.LegendTemplate is not null)
                {
                    TextOptions? currentTextOption = LegendOptions.Find(l => l.Index == legendOption.SeriesIndex)?.TextOption;

                    yValue = Convert.ToDouble(currentTextOption?.Y, null);
                    yValue -= ClipPathHeight * (CurrentPageNumber - 1);

                    if (currentTextOption is not null)
                    {
                        currentTextOption.Y = yValue.ToString(culture);
                    }

                    string id = Owner?.ID + "_chart_legend_template_" + Convert.ToString(legendOption.SeriesIndex, null);
                    legendOption.TemplateID = id;

                    string visibility = "visible";
                    if (legendOption.LocatedPageIndex != CurrentPageNumber)
                    {
                        visibility = "hidden";
                    }

                    LegendItemTemplateOptions? currentTemplateOption = TemplateOptions.Find(t => t.Id == legendOption.TemplateID);
                    if (currentTemplateOption is not null)
                    {
                        currentTemplateOption.style =
                            "position: absolute; visibility: " + visibility + ";" +
                            " left: " + currentTextOption?.X + "px;" +
                            " top: " + currentTextOption?.Y + "px;" +
                            " pointer-events: bounding-box; cursor: " + pointerValue + ";" +
                            " white-space: nowrap; overflow: hidden; text-overflow: ellipsis;";
                    }
                }
                else
                {
                    if (legendOption.LegendTemplate is not null && legendOption.LocatedPageIndex > CurrentPageNumber || legendOption.LocatedPageIndex < CurrentPageNumber)
                    {
                        TextOptions? currentTextOption = LegendOptions.Find(l => l.Index == legendOption.SeriesIndex)?.TextOption;
                        LegendItemTemplateOptions? currentTemplateOption = TemplateOptions.Find(t => t.Id == legendOption.TemplateID);

                        if (currentTemplateOption is not null)
                        {
                            currentTemplateOption.style =
                                "position: absolute; visibility: hidden;" +
                                " left: " + currentTextOption?.X + "px;" +
                                " top: " + currentTextOption?.Y + "px;" +
                                " pointer-events: bounding-box; cursor: " + pointerValue + ";" +
                                " white-space: nowrap; overflow: hidden; text-overflow: ellipsis;";
                        }
                    }
                }
            }

            TextOptions textOption = new TextOptions()
            {
                FontSize = LegendCollection[0].TextStyle.GetFontSize(Owner?._chartThemeStyle ?? null!),
                FontFamily = LegendCollection[0].TextStyle.GetFontFamily(Owner?._chartThemeStyle ?? null!),
                FontStyle = LegendCollection[0].TextStyle.FontStyle,
                FontWeight = LegendCollection[0].TextStyle.GetFontWeight(Owner?._chartThemeStyle ?? null!),
                TextAnchor = Owner is not null && Owner.EnableRtl ? "end" : "start",
                Fill = Owner?._chartThemeStyle?.LegendLabel ?? string.Empty
            };

            CalculatePagingElements(textOption);
        }

        /// <summary>
        /// Calculates text options for a legend item.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <param name="index">The legend index.</param>
        /// <param name="legendLabelColor">The legend label color.</param>
        /// <returns>The text options.</returns>
        TextOptions CalculateText(LegendOption legendOption, int index, string legendLabelColor)
        {
            if (legendOption is null)
            {
                return new TextOptions();
            }

            string fill = legendOption.Visible ? ChartHelper.FindThemeColor(legendOption.TextStyle.Color, legendLabelColor) : "#D3D3D3";
            double textWidth = IsRTL ? (legendOption.Text.Contains("...", StringComparison.InvariantCulture) ? ChartHelper.MeasureText(legendOption.Text, legendOption.TextStyle.GetChartFontOptions(Owner?._chartThemeStyle ?? null!)).Width : legendOption.TextSize.Width) : 0;
            string xLoc = ResolveLegendTextX(legendOption, textWidth);

            return new TextOptions()
            {
                Id = LegendID + GenerateId("_text_", index, legendOption), 
                Text = legendOption.Text,
                X = xLoc,
                Y = Convert.ToString(legendOption.Location.Y + (legendOption.LegendTemplate is null ? (MaxItemHeight / 4) : -(MaxItemHeight / 2)), culture),
                Fill = !string.IsNullOrEmpty(fill) ? fill : "black",
                FontFamily = legendOption.TextStyle.GetFontFamily(Owner?._chartThemeStyle ?? null!),
                FontSize = legendOption.TextStyle.GetFontSize(Owner?._chartThemeStyle ?? null!),
                FontStyle = legendOption.TextStyle.FontStyle,
                FontWeight = legendOption.TextStyle.GetFontWeight(Owner?._chartThemeStyle ?? null!),
                TextAnchor = (Owner is not null && !Owner.EnableRtl) ? string.Empty : "end",
                TextCollection = legendOption.TextCollection ?? null!,
                Font = legendOption.TextStyle.GetChartFontOptions(Owner?._chartThemeStyle ?? null!),
                TabIndex = "-1"
            };
        }

        /// <summary>
        /// Determine symbol color based on visibility.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <returns>Symbol color.</returns>
        string DetermineSymbolColor(LegendOption legendOption)
        {
            return legendOption.Visible ? legendOption.Fill : "#D3D3D3";
        }

        /// <summary>
        /// Determine the legend shape name for the symbol.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <returns>Shape name string.</returns>
        string DetermineShape(LegendOption legendOption)
        {
            string shape;
            if (_baseControl == "Chart")
            {
                shape = legendOption.Shape == LegendShape.SeriesType ? legendOption.Type : Convert.ToString(legendOption.Shape, null) ?? string.Empty;
            }
            else
            {
                shape = legendOption.Shape == LegendShape.SeriesType ? Convert.ToString(legendOption.AccType, null) : Convert.ToString(legendOption.Shape, null) ?? string.Empty;
            }

            return shape == "Scatter" ? Convert.ToString(legendOption.MarkerShape, null) ?? string.Empty : shape;
        }

        /// <summary>
        /// Determines whether stroke width should reflect series width for line types.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <returns>True if series line uses stroke width.</returns>
        bool IsStrokeWidthSeriesLine(LegendOption legendOption)
        {
            if (legendOption.Shape != LegendShape.SeriesType)
            {
                return false;
            }

            string typeString = legendOption.Type.ToString(culture).ToLower(culture);
            return typeString.Contains("line", StringComparison.InvariantCulture) && !typeString.Contains("area", StringComparison.InvariantCulture);
        }

        /// <summary>
        /// Detects custom border requirement for scatter/bubble series.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <returns>True when custom border needed.</returns>
        bool IsCustomBorderSeries(LegendOption legendOption)
        {
            return legendOption.Type == ChartSeriesType.Scatter.ToString() || legendOption.Type == ChartSeriesType.Bubble.ToString();
        }

        /// <summary>
        /// Get base stroke width.
        /// </summary>
        /// <param name="isStrokeWidth">Indicates whether stroke width is needed.</param>
        /// <param name="seriesWidth">Series stroke width value.</param>
        /// <returns>Stroke width value.</returns>
        double GetBaseStrokeWidth(bool isStrokeWidth, double seriesWidth)
        {
            return isStrokeWidth ? seriesWidth : 1;
        }

        /// <summary>
        /// Gets dash array for applicable series shapes.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <returns>Dash array or empty string.</returns>
        string GetDashArray(LegendOption legendOption)
        {
            bool needsDashArray = legendOption.Shape == LegendShape.SeriesType &&
                (legendOption.Type == ChartSeriesType.Line.ToString() ||
                 legendOption.Type == ChartSeriesType.MultiColoredLine.ToString() ||
                 legendOption.Type == ChartSeriesType.StackingLine.ToString() ||
                 legendOption.Type == ChartSeriesType.StackingLine100.ToString());

            return needsDashArray ? legendOption.StrokeDashArray : string.Empty;
        }

        /// <summary>
        /// Applies custom border settings and returns (width, color).
        /// </summary>
        /// <param name="isCustomBorder">Whether custom border applies.</param>
        /// <param name="strokeWidth">Stroke width value.</param>
        /// <param name="legendOption">Legend option.</param>
        /// <param name="symbolColor">Symbol color fallback.</param>
        /// <returns>Tuple stroke width and border color.</returns>
        (double StrokeWidth, string BorderColor) ApplyCustomBorder(bool isCustomBorder, double strokeWidth, LegendOption legendOption, string symbolColor)
        {
            string borderColor = string.Empty;
            double width = strokeWidth;

            if (isCustomBorder)
            {
                borderColor = !string.IsNullOrEmpty(legendOption.SeriesBorderColor) ? legendOption.SeriesBorderColor : symbolColor;
                width = !double.IsNaN(legendOption.SeriesBorderWidth) ? legendOption.SeriesBorderWidth : 1;
            }

            return (width, borderColor);
        }

        /// <summary>
        /// Get legend shape padding value.
        /// </summary>
        /// <param name="legendOption">Legend option.</param>
        /// <returns>Padding value.</returns>
        double GetLegendPadding(LegendOption legendOption)
        {
            return legendOption.LegendTemplate is null ? (Legend?.ShapePadding ?? 0) : 0;
        }

        /// <summary>
        /// Create symbol location considering inverse/text alignment.
        /// </summary>
        /// <param name="legendOption">Legend option.</param>
        /// <param name="padding">Padding value.</param>
        /// <returns>Symbol location.</returns>
        ChartEventLocation CreateSymbolLocation(LegendOption legendOption, double padding)
        {
            ChartEventLocation symbolLocation = new ChartEventLocation(0, 0)
            {
                X = IsInverse ? (IsRTL ? legendOption.Location.X - (legendOption.TextSize.Width + padding) : legendOption.Location.X + legendOption.TextSize.Width + padding) : legendOption.Location.X,
                Y = legendOption.Location.Y
            };
            return symbolLocation;
        }

        /// <summary>
        /// Create common path options for a legend symbol.
        /// </summary>
        /// <param name="legendOption">Legend option.</param>
        /// <param name="index">Index for id generation.</param>
        /// <param name="dashArray">Dash array.</param>
        /// <param name="strokeWidth">Stroke width.</param>
        /// <param name="isCustomBorder">Custom border flag.</param>
        /// <param name="borderColor">Border color.</param>
        /// <param name="symbolColor">Symbol color.</param>
        /// <returns>PathOptions instance.</returns>
        PathOptions CreateSymbolPathOptions(LegendOption legendOption, int index, string dashArray, double strokeWidth, bool isCustomBorder, string borderColor, string symbolColor)
        {
            return new PathOptions(
                LegendID + GenerateId("_shape_", index, legendOption),
                string.Empty,
                dashArray,
                strokeWidth,
                isCustomBorder ? borderColor : symbolColor,
                1,
                symbolColor);
        }

        /// <summary>
        /// Try to create marker symbol for series that require a marker.
        /// </summary>
        /// <param name="currentShape">Current shape name.</param>
        /// <param name="legendOption">Legend option.</param>
        /// <param name="index">Index for id generation.</param>
        /// <param name="strokeWidth">Stroke width.</param>
        /// <param name="symbolColor">Symbol color.</param>
        /// <param name="symbolLocation">Base location.</param>
        /// <param name="symbolOption">Path options for symbol.</param>
        /// <returns>A tuple of marker symbol and resolved shape.</returns>
        (SymbolOptions MarkerSymbol, string Shape) TryCreateMarkerSymbol(string currentShape, LegendOption legendOption, int index, double strokeWidth, string symbolColor, ChartEventLocation symbolLocation, PathOptions symbolOption)
        {
            SymbolOptions markerSymbol = null!;
            string shape = currentShape;

            if ((currentShape == "Line" && legendOption.MarkerVisibility && legendOption.MarkerShape != ChartShape.Image) || legendOption.AccType == "Doughnut")
            {
                shape = Convert.ToString(legendOption.AccType == "Doughnut" ? "Circle" : legendOption.MarkerShape.ToString(), null);
                PathOptions markerOption = new PathOptions(
                    LegendID + GenerateId("_shape_" + "marker_", index, legendOption),
                    string.Empty,
                    string.Empty,
                    strokeWidth,
                    legendOption.Visible ? ResolveLegendMarkerFill(legendOption, symbolColor) : symbolColor,
                    1,
                    legendOption.AccType == "Doughnut" ? "#FFFFFF" : (legendOption.Visible ? ResolveLegendMarkerFill(legendOption, symbolOption.Fill) : symbolOption.Fill));

                markerSymbol = CalculateSymbol(
                    symbolLocation,
                    shape,
                    new Size((Legend?.ShapeWidth ?? 0) / 2, (Legend?.ShapeHeight ?? 0) / 2),
                    string.Empty,
                    markerOption);
            }

            return (markerSymbol, shape);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Calculates legend symbol options based on the given legend option.
        /// </summary>
        /// <param name="location">The symbol location.</param>
        /// <param name="shape">The shape name.</param>
        /// <param name="size">The symbol size.</param>
        /// <param name="url">The URL for image symbols.</param>
        /// <param name="option">The path options.</param>
        /// <returns>The calculated symbol options.</returns>
        internal static SymbolOptions CalculateSymbol(ChartEventLocation location, string shape, Size size, string url, PathOptions option)
        {
            SymbolOptions shapeoption = ChartHelper.CalculateShapes(location, size, shape, url, option, false);

            if (shapeoption.ShapeName == ShapeName.path)
            {
                shapeoption.PathOption.Visibility = option.Visibility;
            }

            if (shapeoption.ShapeName == ShapeName.ellipse)
            {
                shapeoption.EllipseOption.Visibility = option.Visibility;
            }

            if (shapeoption.ShapeName == ShapeName.image)
            {
                shapeoption.ImageOption.Visibility = option.Visibility;
            }

            return shapeoption;
        }

        /// <summary>
        /// Calculates render tree builder options for the legend.
        /// </summary>
        internal void CalculateRenderTreeBuilderOptions()
        {
            LegendCollectionRect = new List<Rect>();
            int firstLegend = FindFirstLegendPosition();

            MaxItemHeight = Math.Max(ChartHelper.MeasureText(LegendCollection[0].TextCollection?.Count != 0
                ? (string.IsNullOrEmpty(LegendCollection[0].TextCollection[0]) ? "MeasureText" : LegendCollection[0].TextCollection?[0] ?? null!)
                : (string.IsNullOrEmpty(LegendCollection[0].Text) ? "MeasureText" : LegendCollection[0].Text), LegendTextStyle ?? null!).Height,
                (LegendCollection[0].LegendTemplate is null ? (Legend?.ShapeHeight ?? 0) : 0)
            );

            ChartRowCount = 1;

            if (firstLegend == LegendCollection.Count)
            {
                return;
            }

            PageXCollections = new List<double>();

            ChartEventLocation start = SetupStartAndPagingMetrics(firstLegend, out double textPadding, out string pointerValue);
            BuildLegendSymbolsAndTemplates(start, textPadding, pointerValue, firstLegend);
            BuildLegendRects();
            HandlePaging(pointerValue);
        }

        /// <summary>
        /// Generates a unique identifier for legend elements.
        /// </summary>
        /// <param name="prefix">The identifier prefix.</param>
        /// <param name="count">The index count.</param>
        /// <param name="option">The legend option.</param>
        /// <returns>The generated identifier.</returns>
        internal string GenerateId(string prefix, int count, LegendOption option = null!)
        {
            return _baseControl == "Chart" ? prefix + count : prefix + option.PointIndex;
        }

        /// <summary>
        /// Renders the legend into the render tree.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        /// <param name="svgRenderer">The SVG renderer.</param>
        /// <param name="border">The legend border.</param>
        internal void RenderLegend(RenderTreeBuilder builder, SvgRendering svgRenderer, ChartDefaultBorder border)
        {
            CreateLegendElements(builder, svgRenderer, border);

            foreach (LegendSymbols legendOption in LegendOptions.ToArray())
            {
                string ariaLabel = !string.IsNullOrEmpty(Owner?._legendRenderer?.LegendSettings?.AccessibilityDescriptionFormat) ? (Owner._legendRenderer.LegendSettings.AccessibilityDescriptionFormat.Contains("${value}", StringComparison.Ordinal) ? Owner._legendRenderer.LegendSettings.AccessibilityDescriptionFormat.Replace("${value}", legendOption.TextOption?.Text, StringComparison.Ordinal) : Owner._legendRenderer.LegendSettings.AccessibilityDescriptionFormat) : "Show " + legendOption.TextOption?.Text;
                svgRenderer.OpenGroupElement(builder, LegendID + GenerateId( "_g_", legendOption.Index), string.Empty, string.Empty, (Legend is not null && !Legend.ToggleVisibility ? "e-legend-default" : "e-legend-pointer"), (Owner?._legendRenderer?.LegendSettings is not null && Owner.Focusable && Owner._legendRenderer.LegendSettings.Focusable) && legendOption.Index == 0 ? "0" : "", ariaLabel, "false", string.Empty, !string.IsNullOrEmpty(Owner?._legendRenderer?.LegendSettings?.AccessibilityRole) ? Owner._legendRenderer.LegendSettings.AccessibilityRole : "button", "true");

                if (legendOption.FirstSymbol is not null && legendOption.Template is null)
                {
                    RenderSymbol(builder, legendOption.FirstSymbol);
                }

                if (legendOption.SecondSymbol is not null && legendOption.Template is null)
                {
                    RenderSymbol(builder, legendOption.SecondSymbol);
                }

                if (legendOption.TextOption is not null && legendOption.Template is null)
                {
                    ChartHelper.TextElement(builder, svgRenderer, legendOption.TextOption);
                }

                builder.CloseElement();
            }

            builder.CloseElement();
            builder.CloseElement();
            if (IsPaging)
            {
                RenderPagingElements(builder, svgRenderer);
            }
        }

        /// <summary>
        /// Calculates legend symbol options for a legend entry.
        /// </summary>
        /// <param name="legendOption">The legend option.</param>
        /// <param name="index">The legend index.</param>
        /// <returns>The list of symbol options.</returns>
        internal List<SymbolOptions> CalculateLegendOptions(LegendOption legendOption, int index)
        {
            SymbolOptions legendSymbol = null!, legendMarkerSymbol = null!;
            string borderColor = string.Empty;
            string symbolColor = DetermineSymbolColor(legendOption);
            string shape = DetermineShape(legendOption);

            bool isStrokeWidth = IsStrokeWidthSeriesLine(legendOption);
            bool isCustomBorder = IsCustomBorderSeries(legendOption);
            double strokeWidth = GetBaseStrokeWidth(isStrokeWidth, legendOption.SeriesWidth);
            string dashArray = GetDashArray(legendOption);

            (double StrokeWidth, string BorderColor) borderResult = ApplyCustomBorder(isCustomBorder, strokeWidth, legendOption, symbolColor);
            strokeWidth = borderResult.StrokeWidth;
            borderColor = borderResult.BorderColor;

            double padding = GetLegendPadding(legendOption);
            ChartEventLocation symbolLocation = CreateSymbolLocation(legendOption, padding);
            PathOptions symbolOption = CreateSymbolPathOptions(legendOption, index, dashArray, strokeWidth, isCustomBorder, borderColor, symbolColor);

            legendSymbol = CalculateSymbol(symbolLocation, shape, new Size(Legend?.ShapeWidth ?? 0, Legend?.ShapeHeight ?? 0), string.Empty, symbolOption);

            (SymbolOptions MarkerSymbol, string Shape) markerResult = TryCreateMarkerSymbol(shape, legendOption, index, strokeWidth, symbolColor, symbolLocation, symbolOption);
            legendMarkerSymbol = markerResult.MarkerSymbol;
            shape = markerResult.Shape;

            return new List<SymbolOptions>() { legendSymbol, legendMarkerSymbol };
        }

        /// <summary>
        /// Calculates legend bounds and updates position based on available size.
        /// </summary>
        /// <param name="rect">The chart rectangle.</param>
        /// <param name="availableSize">The available size.</param>
        /// <param name="marginTop">The top margin.</param>
        /// <param name="baseComponent">The base component name.</param>
        /// <param name="seriesType">The series type.</param>
        internal void CalculateLegendBounds(Rect rect, Size availableSize, double marginTop, string baseComponent, string seriesType)
        {
            _baseControl = baseComponent;
            LegendID = ChartId + "_chart_legend";
            PageUpID = LegendID + "_pageup";
            PageDownID = LegendID + "_pagedown";
            PageNumberID = LegendID + "_pagenumber";
            LegendTranslateID = LegendID + "_translate_g";
            GetPosition(availableSize, _baseControl, seriesType);

            LegendBounds = new Rect() { X = rect.X, Y = rect.Y, Width = 0, Height = 0 };
            string DefaultValue = (_baseControl == "BulletChart") ? "40%" : "20%";
            IsVertical = Position == LegendPosition.Left || Position == LegendPosition.Right;
            ItemPadding = !double.IsNaN(Legend?.ItemPadding ?? 0) ? Legend?.ItemPadding ?? 0 : IsVertical ? 8 : 20;

            if (IsVertical)
            {
                LegendBounds.Height = DataVizCommonHelper.StringToNumber(Legend?.Height ?? string.Empty, availableSize.Height - (rect.Y - marginTop));
                LegendBounds.Height = !double.IsNaN(LegendBounds.Height) ? LegendBounds.Height : rect.Height;
                LegendBounds.Width = DataVizCommonHelper.StringToNumber(!string.IsNullOrEmpty(Legend?.Width) ? Legend.Width : DefaultValue, availableSize.Width);
            }
            else
            {
                LegendBounds.Width = DataVizCommonHelper.StringToNumber(Legend?.Width ?? string.Empty, availableSize.Width);
                LegendBounds.Width = !double.IsNaN(LegendBounds.Width) ? LegendBounds.Width : rect.Width;
                LegendBounds.Height = DataVizCommonHelper.StringToNumber(!string.IsNullOrEmpty(Legend?.Height) ? Legend.Height : DefaultValue, availableSize.Height);
            }

            BaseLegendRef?.GetLegendBounds(availableSize, rect, null!);
        }

        /// <summary>
        /// Gets the legend location based on position and alignment.
        /// </summary>
        /// <param name="rect">The chart rectangle.</param>
        /// <param name="availableSize">The available size.</param>
        /// <param name="legendMargin">The legend margin.</param>
        /// <param name="chartMargin">The chart margin.</param>
        /// <param name="legendBorder">The legend border.</param>
        /// <param name="location">The custom location.</param>
        internal void GetLocation(Rect rect, Size availableSize, ChartDefaultMargin legendMargin, ChartDefaultMargin chartMargin, ChartDefaultBorder legendBorder, ChartDefaultLocation location)
        {
            if (rect is null || availableSize is null || chartMargin is null)
            {
                return;
            }

            double padding = legendBorder.Width;
            double legendHeight = LegendBounds.Height + padding + legendMargin.Top + legendMargin.Bottom;
            double legendWidth = LegendBounds.Width + padding + legendMargin.Left + legendMargin.Right, marginBottom = chartMargin.Bottom;

            if (Position == LegendPosition.Bottom)
            {
                LegendBounds.X = AlignLegend(LegendBounds.X, availableSize.Width, LegendBounds.Width, Legend?.Alignment ?? Alignment.Center);
                LegendBounds.Y = rect.Y + (rect.Height - legendHeight) + padding + legendMargin.Top + RangeNavigatorHeight();
                ChartHelper.SubtractThickness(rect, new Thickness(0, 0, 0, legendHeight));
            }
            else if (Position == LegendPosition.Top)
            {
                LegendBounds.X = AlignLegend(LegendBounds.X, availableSize.Width, LegendBounds.Width, Legend?.Alignment ?? Alignment.Center);
                LegendBounds.Y = rect.Y + (padding / 2) + legendMargin.Top;
                ChartHelper.SubtractThickness(rect, new Thickness(0, 0, legendHeight, 0));
            }
            else if (Position == LegendPosition.Right)
            {
                LegendBounds.X = rect.X + (rect.Width - LegendBounds.Width) - legendMargin.Right;
                LegendBounds.Y = rect.Y + AlignLegend(0, availableSize.Height - (rect.Y + (!double.IsNaN(chartMargin.Bottom) ? chartMargin.Bottom : (SyncfusionService is not null && SyncfusionService.IsDeviceMode ? 5 : 10))), LegendBounds.Height, Legend?.Alignment ?? Alignment.Center);
                ChartHelper.SubtractThickness(rect, new Thickness(0, legendWidth, 0, 0));
            }
            else if (Position == LegendPosition.Left)
            {
                LegendBounds.X = LegendBounds.X + legendMargin.Left;
                LegendBounds.Y = rect.Y + AlignLegend(0, availableSize.Height - (rect.Y + (!double.IsNaN(chartMargin.Bottom) ? chartMargin.Bottom : (SyncfusionService is not null && SyncfusionService.IsDeviceMode ? 5 : 10))), LegendBounds.Height, Legend?.Alignment ?? Alignment.Center);
                ChartHelper.SubtractThickness(rect, new Thickness(legendWidth, 0, 0, 0));
            }
            else
            {
                LegendBounds.X = location.X;
                LegendBounds.Y = location.Y;
                ChartHelper.SubtractThickness(rect, new Thickness(0, 0, 0, 0));
            }
        }

        /// <summary>
        /// Aligns the legend within the available size.
        /// </summary>
        /// <param name="start">The starting position.</param>
        /// <param name="size">The available size.</param>
        /// <param name="legendSize">The legend size.</param>
        /// <param name="alignment">The alignment.</param>
        /// <returns>The aligned starting position.</returns>
        internal static double AlignLegend(double start, double size, double legendSize, Alignment alignment)
        {
            if (alignment == Alignment.Far)
            {
                start = size - legendSize - start;
            }
            else if (alignment == Alignment.Center)
            {
                start = (size - legendSize) / 2;
            }

            return start;
        }

        /// <summary>
        /// Returns the range navigator height.
        /// </summary>
        /// <returns>The range navigator height.</returns>
        internal double RangeNavigatorHeight()
        {
            return 0;
        }

        /// <summary>
        /// Sets the legend bounds based on computed size.
        /// </summary>
        /// <param name="computedWidth">The computed width.</param>
        /// <param name="computedHeight">The computed height.</param>
        internal void SetBounds(double computedWidth, double computedHeight)
        {
            computedWidth = computedWidth < LegendBounds.Width ? computedWidth : LegendBounds.Width;
            computedHeight = computedHeight < LegendBounds.Height ? computedHeight : LegendBounds.Height;
            LegendBounds.Width = string.IsNullOrEmpty(Legend?.Width) ? computedWidth : LegendBounds.Width;
            LegendBounds.Height = string.IsNullOrEmpty(Legend?.Height) ? computedHeight : LegendBounds.Height;
            RowCountPerPage = Math.Max(1, Math.Ceiling(Math.Round((LegendBounds.Height - (Legend?.Padding ?? 0)) / (MaxItemHeight + (Legend?.Padding ?? 0)), 2)));
        }
        #endregion
    }
}
