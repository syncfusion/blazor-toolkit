using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Charts.Internal
{
    /// <summary>
    /// Renders chart titles and subtitles with support for multiple positions, text wrapping, and styling.
    /// </summary>
    /// <remarks>
    /// This renderer handles layout calculations, font metrics, and SVG rendering for chart titles.
    /// It manages both title and subtitle collections, supports RTL rendering, and integrates with the theme system.
    /// </remarks>
    public class ChartTitleRenderer : ChartRenderer
    {
        #region Fields
        private Rect? _availableRect;
        private double _maxWidth;
        private CultureInfo _culture = CultureInfo.InvariantCulture;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the collection of title text lines after wrapping.
        /// </summary>
        /// <value>A list of title strings, typically containing one entry unless wrapping occurs.</value>
        internal List<string> TitleCollection { get; set; } = [];

        /// <summary>
        /// Gets the collection of subtitle text lines after wrapping.
        /// </summary>
        /// <value>A list of subtitle strings, typically containing one entry unless wrapping occurs.</value>
        internal List<string> SubTitleCollection { get; set; } = [];

        /// <summary>
        /// Gets the measured size of the title text.
        /// </summary>
        /// <value>The width and height of the rendered title.</value>
        internal Size TitleSize { get; set; } = new(0, 0);

        /// <summary>
        /// Gets the measured size of the subtitle text.
        /// </summary>
        /// <value>The width and height of the rendered subtitle.</value>
        internal Size SubTitleSize { get; set; } = new(0, 0);

        /// <summary>
        /// Gets or sets the padding between title and subtitle.
        /// </summary>
        /// <value>Padding value in pixels.</value>
        internal int Padding { get; set; }

        /// <summary>
        /// Gets or sets the styling configuration for the title.
        /// </summary>
        /// <value>The title style properties, or <c>null</c> if not initialized.</value>
        internal ChartTitleStyle? TitleStyle { get; set; }

        /// <summary>
        /// Gets or sets the styling configuration for the subtitle.
        /// </summary>
        /// <value>The subtitle style properties, or <c>null</c> if not initialized.</value>
        internal ChartSubTitleStyle? SubTitleStyle { get; set; }

        /// <summary>
        /// Gets the tab index for keyboard navigation.
        /// </summary>
        /// <value>The owner's tab index, or 0 if the owner is not available.</value>
        private double TabIndex => Owner?.TabIndex ?? 0;
        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Initializes the renderer and registers it with the chart owner.
        /// </summary>
        protected override void OnInitialized()
        {
            if (Owner is { })
            {
                Owner._chartTitleRenderer = this;
            }
            AddToRenderQueue(this);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Creates font options for title rendering.
        /// </summary>
        /// <returns>A <see cref="ChartFontOptions"/> object configured with title style properties.</returns>
        private ChartFontOptions GetTitleFontOptions()
        {
            return new ChartFontOptions { Color = TitleStyle?.Color ?? string.Empty, Size = TitleStyle?.GetFontSize(Owner?._chartThemeStyle ?? null!) ?? string.Empty, FontFamily = TitleStyle?.GetFontFamily(Owner?._chartThemeStyle ?? null!) ?? string.Empty, FontWeight = TitleStyle?.GetFontWeight(Owner?._chartThemeStyle ?? null!) ?? string.Empty, FontStyle = TitleStyle?.FontStyle ?? string.Empty, TextAlignment = TitleStyle?.TextAlignment ?? Alignment.Center, TextOverflow = TitleStyle?.TextOverflow ?? TextOverflow.Trim };
        }

        /// <summary>
        /// Creates font options for subtitle rendering.
        /// </summary>
        /// <returns>A <see cref="ChartFontOptions"/> object configured with subtitle style properties.</returns>
        private ChartFontOptions GetSubTitleFontOptions()
        {
            return new ChartFontOptions { Color = SubTitleStyle?.Color ?? string.Empty, Size = SubTitleStyle?.GetFontSize(Owner?._chartThemeStyle ?? null!) ?? string.Empty, FontFamily = SubTitleStyle?.GetFontFamily(Owner?._chartThemeStyle ?? null!) ?? string.Empty, FontWeight = SubTitleStyle?.GetFontWeight(Owner?._chartThemeStyle ?? null!) ?? string.Empty, FontStyle = SubTitleStyle?.FontStyle ?? string.Empty, TextAlignment = SubTitleStyle?.TextAlignment ?? Alignment.Center, TextOverflow = SubTitleStyle?.TextOverflow ?? TextOverflow.Trim };
        }

        /// <summary>
        /// Determines whether title handling is required based on chart state.
        /// </summary>
        /// <param name="rect">The available rectangle for rendering.</param>
        /// <returns><see langword="true"/> if title handling is required; otherwise <see langword="false"/>.</returns>
        private bool IsTitleHandlingRequired(Rect rect)
        {
            if (rect is null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(Owner?.Title))
            {
                return false;
            }

            bool isSmall = Owner._widthCategory == ChartWidthCategory.Small || Owner._heightCategory == ChartHeightCategory.Small;

            return !isSmall;
        }

        /// <summary>
        /// Calculates padding based on title position.
        /// </summary>
        /// <returns>The padding value in pixels.</returns>
        private double CalculatePadding()
        {
            bool isTopOrBottom = TitleStyle?.Position is ChartTitlePosition.Top or ChartTitlePosition.Bottom;
            return isTopOrBottom ? 15 : 5;
        }

        /// <summary>
        /// Calculates the total title height including all lines.
        /// </summary>
        /// <param name="padding">The padding value to add.</param>
        /// <returns>The total height required for the title.</returns>
        private double CalculateTitleHeight(double padding)
        {
            return ((TitleSize.Height * TitleCollection?.Count) ?? 0) + padding;
        }

        /// <summary>
        /// Processes subtitle if present and dimensions permit.
        /// </summary>
        /// <param name="rect">The available rectangle for rendering.</param>
        /// <param name="titleHeight">The current title height (ref parameter to accumulate subtitle height).</param>
        private void ProcessSubtitleIfNeeded(Rect rect, ref double titleHeight)
        {
            if (!string.IsNullOrEmpty(Owner?.SubTitle) && !IsSubtitleDimensionTooSmall())
            {
                UpdateMaxTitleWidth();

                SubTitleCollection = ChartHelper.GetTitle(Owner.SubTitle, GetSubTitleFontOptions(), rect.Width);
                SubTitleSize = ChartHelper.MeasureText(Owner.SubTitle, GetSubTitleFontOptions());
                titleHeight += ((SubTitleSize.Height * SubTitleCollection?.Count) ?? 0) + 15;
            }
        }

        /// <summary>
        /// Determines whether subtitle dimensions are too small for rendering.
        /// </summary>
        /// <returns><see langword="true"/> if subtitle dimensions are too small; otherwise <see langword="false"/>.</returns>
        private bool IsSubtitleDimensionTooSmall()
        {
            return Owner?._widthCategory == ChartWidthCategory.Medium || Owner?._heightCategory == ChartHeightCategory.Medium;
        }

        /// <summary>
        /// Updates the maximum width to accommodate all title lines.
        /// </summary>
        private void UpdateMaxTitleWidth()
        {
            if (TitleCollection is null || TitleCollection.Count == 0)
            {
                return;
            }

            foreach (string titleText in TitleCollection)
            {
                double titleWidth = ChartHelper.MeasureText(titleText, GetTitleFontOptions()).Width;
                _maxWidth = titleWidth > _maxWidth ? titleWidth : _maxWidth;
            }
        }

        /// <summary>
        /// Adjusts the rendering rectangle based on title position.
        /// </summary>
        /// <param name="rect">The rectangle to adjust (ref parameter).</param>
        /// <param name="titleHeight">The height consumed by the title.</param>
        private void AdjustRectForTitlePosition(ref Rect rect, double titleHeight)
        {
            switch (TitleStyle?.Position)
            {
                case ChartTitlePosition.Top:
                    rect.Y += titleHeight;
                    rect.Height -= titleHeight;
                    break;

                case ChartTitlePosition.Bottom:
                    rect.Height -= titleHeight;
                    break;

                case ChartTitlePosition.Left:
                    rect.X += titleHeight;
                    rect.Width -= titleHeight;
                    break;

                case ChartTitlePosition.Right:
                    rect.Width -= titleHeight;
                    break;
                case ChartTitlePosition.Custom:
                case null:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Updates the available rectangle if it has changed.
        /// </summary>
        /// <param name="rect">The new rectangle dimensions.</param>
        private void UpdateAvailableRectIfChanged(Rect rect)
        {
            if (_availableRect != rect)
            {
                _availableRect = rect;
            }
        }

        /// <summary>
        /// Ensures the title style is initialized with default values.
        /// </summary>
        private void SetDefaultTitleStyle()
        {
            if (TitleStyle is not null)
            {
                return;
            }

            TitleStyle = new ChartTitleStyle();
        }

        /// <summary>
        /// Ensures the subtitle style is initialized with default values.
        /// </summary>
        private void SetDefaultSubTitleStyle()
        {
            if (SubTitleStyle is not null)
            {
                return;
            }

            SubTitleStyle = new ChartSubTitleStyle();
        }

        /// <summary>
        /// Renders the title element to the SVG.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        private void RenderTitle(RenderTreeBuilder builder)
        {
            if (!IsValidForTitleRendering() || Owner is null)
            {
                return;
            }

            ChartMargin margin = Owner._margin ?? null!;
            Alignment alignment = TitleStyle?.TextAlignment ?? Alignment.Center;
            string textAnchor = ChartHelper.GetTextAnchor(alignment, Owner.EnableRtl);

            Rect titleBounds = CalculateTitleBounds(margin);

            double positionX = ChartHelper.TitlePositionX(titleBounds, alignment);
            double positionY = CalculateInitialPositionY(margin);
            string rotation = string.Empty;

            switch (TitleStyle?.Position)
            {
                case ChartTitlePosition.Top:
                case ChartTitlePosition.Bottom:
                    CalculateTitlePositionForTopOrBottom(ref positionX, ref positionY, ref textAnchor, margin);
                    break;
                case ChartTitlePosition.Left:
                case ChartTitlePosition.Right:
                    rotation = CalculateTitlePositionForLeftOrRight(ref positionX, ref positionY, ref textAnchor, alignment, margin);
                    break;
                case ChartTitlePosition.Custom:
                    CalculateTitlePositionForCustom(ref positionX, ref positionY, ref textAnchor);
                    break;
                default:
                    break;
            }

            TextOptions titleOptions = CreateTitleTextOptions(positionX, positionY, textAnchor, rotation);

            ChartHelper.TextElement(builder, Owner._svgRenderer ?? null!, titleOptions);

            if (ShouldRenderSubTitle())
            {
                RenderSubTitle(builder, titleOptions);
            }
        }

        /// <summary>
        /// Determines whether title rendering is valid based on chart state.
        /// </summary>
        /// <returns><see langword="true"/> if title rendering is valid; otherwise <see langword="false"/>.</returns>
        private bool IsValidForTitleRendering()
        {
            return !string.IsNullOrEmpty(Owner?.Title) && !(Owner._widthCategory == ChartWidthCategory.Small || Owner._heightCategory == ChartHeightCategory.Small);
        }

        /// <summary>
        /// Gets the default margin based on device mode.
        /// </summary>
        /// <returns>The default margin value in pixels.</returns>
        private double GetDefaultMargin()
        {
            return (SyncfusionService is not null && SyncfusionService.IsDeviceMode) ? 5 : 10;
        }

        /// <summary>
        /// Gets the margin value or returns default if invalid.
        /// </summary>
        /// <param name="marginValue">The margin value to validate.</param>
        /// <returns>The valid margin value or default.</returns>
        private double GetMarginValueOrDefault(double? marginValue)
        {
            return !double.IsNaN(marginValue ?? double.NaN) ? marginValue!.Value : GetDefaultMargin();
        }

        /// <summary>
        /// Gets the border width from the chart border renderer.
        /// </summary>
        /// <returns>The border width, or <c>null</c> if not available.</returns>
        private double? GetBorderWidthOrNull()
        {
            return Owner?._chartBorderRenderer?.ChartBorder?.Width;
        }

        /// <summary>
        /// Calculates the title bounds based on margins.
        /// </summary>
        /// <param name="margin">The chart margin configuration.</param>
        /// <returns>A rectangle representing the title bounds.</returns>
        private Rect CalculateTitleBounds(ChartMargin margin)
        {
            if (Owner is null)
            {
                return new Rect(0, 0, 0, 0);
            }
            double leftMargin = GetMarginValueOrDefault(margin.Left);
            double rightMargin = GetMarginValueOrDefault(margin.Right);
            double width = Owner.AvailableSize.Width - leftMargin - rightMargin;

            return new Rect(leftMargin, 0, width, 0);
        }

        /// <summary>
        /// Calculates the initial Y position for the title.
        /// </summary>
        /// <param name="margin">The chart margin configuration.</param>
        /// <returns>The Y coordinate for title positioning.</returns>
        private double CalculateInitialPositionY(ChartMargin margin)
        {
            double topMargin = GetMarginValueOrDefault(margin.Top);
            return topMargin + (TitleSize.Height * 3 / 4);
        }

        /// <summary>
        /// Calculates title position for top or bottom alignment.
        /// </summary>
        /// <param name="positionX">The X coordinate (ref parameter).</param>
        /// <param name="positionY">The Y coordinate (ref parameter).</param>
        /// <param name="textAnchor">The text anchor attribute (ref parameter).</param>
        /// <param name="margin">The chart margin configuration.</param>
        private void CalculateTitlePositionForTopOrBottom(ref double positionX, ref double positionY, ref string textAnchor, ChartMargin margin)
        {
            positionX += textAnchor switch
            {
                "start" => Owner?._chartBorderRenderer?.ChartBorder?.Width ?? 0,
                "end" => Owner?._chartBorderRenderer?.ChartBorder?.Width ?? 0,
                _ => 0
            };
            if (Owner is null)
            {
                return;
            }
            Rect adaptiveRect = Owner._isAdaptiveRendering
                ? new Rect(Owner.GetChartMargin(), 0, Owner.AvailableSize.Width - (Owner.GetChartMargin() * 2), 0)
                : new Rect(GetMarginValueOrDefault(Owner._margin.Left), 0, Owner.AvailableSize.Width - GetMarginValueOrDefault(Owner._margin.Left) - GetMarginValueOrDefault(Owner._margin.Right), 0);

            positionX = ChartHelper.TitlePositionX(adaptiveRect, TitleStyle!.TextAlignment);
            double topPosition = Owner._isAdaptiveRendering ? Owner.GetChartMargin(true) : GetMarginValueOrDefault(Owner._margin.Top);
            positionY = topPosition + (TitleSize.Height * 3 / 4);

            if (TitleStyle.Position == ChartTitlePosition.Bottom)
            {
                double bottomMargin = GetMarginValueOrDefault(margin.Bottom);
                positionY = Owner.AvailableSize.Height - bottomMargin - SubTitleSize.Height - (TitleSize.Height / 2);
            }
        }

        /// <summary>
        /// Calculates title position for left or right alignment.
        /// </summary>
        /// <param name="positionX">The X coordinate (ref parameter).</param>
        /// <param name="positionY">The Y coordinate (ref parameter).</param>
        /// <param name="textAnchor">The text anchor attribute (ref parameter).</param>
        /// <param name="alignment">The text alignment.</param>
        /// <param name="margin">The chart margin configuration.</param>
        /// <returns>The rotation transform string for side-aligned titles.</returns>
        private string CalculateTitlePositionForLeftOrRight(ref double positionX, ref double positionY, ref string textAnchor, Alignment alignment, ChartMargin margin)
        {
            if (Owner is null)
            {
                return string.Empty;
            }
            double relevantMargin = (TitleStyle!.Position == ChartTitlePosition.Left) ? margin.Left : margin.Right;
            double marginValue = GetMarginValueOrDefault(relevantMargin);

            positionX = (TitleStyle.Position == ChartTitlePosition.Left) ? marginValue + (TitleSize.Height * 3 / 4) : Owner.AvailableSize.Width - marginValue - (TitleSize.Height * 3 / 4);

            double bottomMargin = GetMarginValueOrDefault(margin.Bottom);
            double? nearY = bottomMargin + GetBorderWidthOrNull();
            double? farY = Owner.AvailableSize.Height - bottomMargin - GetBorderWidthOrNull();

            positionY = alignment switch
            {
                Alignment.Near => nearY ?? 0,
                Alignment.Far => farY ?? 0,
                Alignment.Center => 0,
                _ => Owner.AvailableSize.Height / 2
            };

            textAnchor = alignment switch
            {
                Alignment.Near => (TitleStyle.Position == ChartTitlePosition.Left) ? "end" : "start",
                Alignment.Far => (TitleStyle.Position == ChartTitlePosition.Left) ? "start" : "end",
                Alignment.Center => "",
                _ => "middle"
            };

            if (Owner.EnableRtl)
            {
                textAnchor = textAnchor switch
                {
                    "start" => "end",
                    "end" => "start",
                    _ => textAnchor
                };
            }

            return (TitleStyle.Position == ChartTitlePosition.Left) ? $"rotate(-90,{positionX},{positionY})" : $"rotate(90,{positionX},{positionY})";
        }

        /// <summary>
        /// Calculates title position for custom alignment.
        /// </summary>
        /// <param name="positionX">The X coordinate (ref parameter).</param>
        /// <param name="positionY">The Y coordinate (ref parameter).</param>
        /// <param name="textAnchor">The text anchor attribute (ref parameter).</param>
        private void CalculateTitlePositionForCustom(ref double positionX, ref double positionY, ref string textAnchor)
        {
            positionX = (TitleSize.Width / 2) + TitleStyle!.X;
            positionY = TitleSize.Height + TitleStyle!.Y;
            textAnchor = "middle";
        }

        /// <summary>
        /// Creates text options for title rendering.
        /// </summary>
        /// <param name="positionX">The X coordinate.</param>
        /// <param name="positionY">The Y coordinate.</param>
        /// <param name="textAnchor">The text anchor attribute.</param>
        /// <param name="rotation">The rotation transform.</param>
        /// <returns>A <see cref="TextOptions"/> object configured for title rendering.</returns>
        private TextOptions CreateTitleTextOptions(double positionX, double positionY, string textAnchor, string rotation)
        {
            ChartFontOptions firstCallTitleStyle = GetTitleFontOptions();
            if (Owner is null)
            {
                return null!;
            }
            string color = !string.IsNullOrEmpty(TitleStyle?.Color) ? TitleStyle!.Color : Owner._chartThemeStyle?.ChartTitle ?? string.Empty;
            string accessibilityDescription = !string.IsNullOrEmpty(TitleStyle?.AccessibilityDescription) ? TitleStyle!.AccessibilityDescription : Owner.Title;
            string accessibilityRole = !string.IsNullOrEmpty(TitleStyle?.AccessibilityRole) ? TitleStyle!.AccessibilityRole : "text";
            string tabIndex = (Owner.Focusable && TitleStyle is not null && TitleStyle.Focusable) ? TabIndex.ToString(_culture) : "-1";

            TextOptions titleOptions = new(
                Convert.ToString(positionX, _culture),
                Convert.ToString(positionY, _culture),
                color,
                firstCallTitleStyle,
                Owner.Title,
                textAnchor,
                Owner.ID + "_ChartTitle",
                rotation,
                string.Empty,
                "auto",
                accessibilityDescription,
                accessibilityRole,
                tabIndex)
            {
                TextCollection = TitleCollection,
                Font = GetTitleFontOptions()
            };

            return titleOptions;
        }

        /// <summary>
        /// Determines whether subtitle should be rendered.
        /// </summary>
        /// <returns><see langword="true"/> if subtitle rendering is valid; otherwise <see langword="false"/>.</returns>
        private bool ShouldRenderSubTitle()
        {
            return !string.IsNullOrEmpty(Owner?.SubTitle) && !(Owner._widthCategory == ChartWidthCategory.Medium || Owner._heightCategory == ChartHeightCategory.Medium);
        }

        /// <summary>
        /// Renders the subtitle element below the title.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        /// <param name="options">The title rendering options.</param>
        private void RenderSubTitle(RenderTreeBuilder builder, TextOptions options)
        {
            const int padding = 10;
            UpdateMaxWidthForOverflow();
            string textAnchor = ChartHelper.GetTextAnchor(SubTitleStyle?.TextAlignment ?? Alignment.Center, Owner?.EnableRtl ?? false);
            Alignment alignment = TitleStyle?.TextAlignment ?? Alignment.Center;

            Rect rect = CalculateInitialRect(options, alignment);
            rect.X = ApplyCustomPositionAdjustment(rect.X, options);
            rect.X = ApplyLeftPositionAdjustment(rect.X, options, alignment);

            double positionY = CalculateSubTitlePositionY(options, padding);

            TextOptions subtitleOptions = BuildSubtitleTextOptions(options, rect, positionY, textAnchor);

            ChartHelper.TextElement(builder, Owner?._svgRenderer ?? null!, subtitleOptions);
        }

        /// <summary>
        /// Updates the maximum width to handle text overflow cases.
        /// </summary>
        private void UpdateMaxWidthForOverflow()
        {
            _maxWidth = SubTitleStyle?.TextOverflow == TextOverflow.None
                ? Math.Max(ChartHelper.MeasureText(Owner?.Title ?? string.Empty, GetTitleFontOptions()).Width, ChartHelper.MeasureText(Owner?.SubTitle ?? string.Empty, GetSubTitleFontOptions()).Width)
                : _maxWidth;
        }

        /// <summary>
        /// Calculates the initial rectangle for subtitle positioning.
        /// </summary>
        /// <param name="options">The title rendering options.</param>
        /// <param name="alignment">The text alignment.</param>
        /// <returns>A rectangle configured for subtitle positioning.</returns>
        private Rect CalculateInitialRect(TextOptions options, Alignment alignment)
        {
            double xPosition = CalculateRectXByAlignment(options, alignment);
            return new Rect(xPosition, 0, _maxWidth, 0);
        }

        /// <summary>
        /// Calculates the X coordinate based on alignment.
        /// </summary>
        /// <param name="options">The title rendering options.</param>
        /// <param name="alignment">The text alignment.</param>
        /// <returns>The calculated X coordinate.</returns>
        private double CalculateRectXByAlignment(TextOptions options, Alignment alignment)
        {
            double optionsX = Convert.ToDouble(options.X, _culture);
            return alignment == Alignment.Center ? (optionsX - (_maxWidth * 0.5)) : alignment == Alignment.Far ? (optionsX - _maxWidth) : optionsX;
        }

        /// <summary>
        /// Applies custom position adjustment to X coordinate.
        /// </summary>
        /// <param name="currentX">The current X coordinate.</param>
        /// <param name="options">The title rendering options.</param>
        /// <returns>The adjusted X coordinate.</returns>
        private double ApplyCustomPositionAdjustment(double currentX, TextOptions options)
        {
            if (TitleStyle?.Position == ChartTitlePosition.Custom)
            {
                double optionsX = Convert.ToDouble(options.X, _culture);
                return optionsX - (_maxWidth * 0.5);
            }
            return currentX;
        }

        /// <summary>
        /// Applies left position adjustment to X coordinate.
        /// </summary>
        /// <param name="currentX">The current X coordinate.</param>
        /// <param name="options">The title rendering options.</param>
        /// <param name="alignment">The text alignment.</param>
        /// <returns>The adjusted X coordinate.</returns>
        private double ApplyLeftPositionAdjustment(double currentX, TextOptions options, Alignment alignment)
        {
            if (TitleStyle?.Position == ChartTitlePosition.Left)
            {
                double optionsX = Convert.ToDouble(options.X, _culture);
                return alignment == Alignment.Center ? (optionsX - (_maxWidth * 0.5)) : alignment == Alignment.Far ? (GetLeftMarginOffset() + (SubTitleSize.Height * 3 / 4)) : (optionsX - _maxWidth);
            }
            return currentX;
        }

        /// <summary>
        /// Gets the left margin offset for positioning.
        /// </summary>
        /// <returns>The left margin offset in pixels.</returns>
        private double GetLeftMarginOffset()
        {
            return (Owner is not null && !double.IsNaN(Owner._margin.Left)) ? Owner._margin.Left : (SyncfusionService is not null && SyncfusionService.IsDeviceMode ? 5 : 10);
        }

        /// <summary>
        /// Calculates the Y position for subtitle rendering.
        /// </summary>
        /// <param name="options">The title rendering options.</param>
        /// <param name="padding">The padding value for subtitle positioning.</param>
        /// <returns>The calculated Y coordinate.</returns>
        private double CalculateSubTitlePositionY(TextOptions options, int padding)
        {
            if (TitleStyle?.Position == ChartTitlePosition.Bottom)
            {
                double optionsY = Convert.ToDouble(options.Y, _culture);
                return (optionsY * options.TextCollection.Count) + (padding / 2) + (TitleSize.Height / 2) + (SubTitleSize.Height / 2);
            }

            double baseY = Convert.ToDouble(options.Y, _culture);
            double titleHeightAccumulation = TitleCollection.Count > 1 ? ((TitleCollection.Count - 1) * TitleSize.Height) : 0;
            return baseY + titleHeightAccumulation + (SubTitleSize.Height * 3 / 4) + 10;
        }

        /// <summary>
        /// Builds text options for subtitle rendering.
        /// </summary>
        /// <param name="options">The title rendering options.</param>
        /// <param name="rect">The subtitle bounds rectangle.</param>
        /// <param name="positionY">The Y coordinate for positioning.</param>
        /// <param name="textAnchor">The text anchor attribute.</param>
        /// <returns>A <see cref="TextOptions"/> object configured for subtitle rendering.</returns>
        private TextOptions BuildSubtitleTextOptions(TextOptions options, Rect rect, double positionY, string textAnchor)
        {
            double xPosition = ChartHelper.TitlePositionX(rect, SubTitleStyle?.TextAlignment ?? Alignment.Center);
            string subtitle = SubTitleCollection.Count > 0 ? SubTitleCollection[0] : string.Empty;
            string color = SubTitleStyle?.Color ?? Owner?._chartThemeStyle?.ChartSubTitle ?? string.Empty;
            string accessibilityDescription = !string.IsNullOrEmpty(SubTitleStyle?.AccessibilityDescription) ? SubTitleStyle.AccessibilityDescription : Owner?.SubTitle ?? string.Empty;
            string accessibilityRole = !string.IsNullOrEmpty(SubTitleStyle?.AccessibilityRole) ? SubTitleStyle.AccessibilityRole : "text";
            string tabIndexValue = (Owner is not null && Owner.Focusable && SubTitleStyle is not null && SubTitleStyle.Focusable) ? TabIndex.ToString(_culture) : "-1";

            TextOptions subtitleOptions = new(
                xPosition.ToString(_culture),
                Convert.ToString(positionY, _culture),
                color,
                GetSubTitleFontOptions(),
                subtitle,
                textAnchor,
                Owner?.ID + "_ChartSubTitle",
                options.Transform,
                string.Empty,
                "auto",
                accessibilityDescription,
                accessibilityRole,
                tabIndexValue)
            {
                TextCollection = SubTitleCollection,
                Font = GetSubTitleFontOptions()
            };
            return subtitleOptions;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Builds the render tree with title and subtitle elements.
        /// </summary>
        /// <param name="builder">The render tree builder.</param>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (_availableRect is not null && builder is not null)
            {
                RenderTitle(builder);
                RendererShouldRender = false;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Generates a unique font key for the title based on style properties.
        /// </summary>
        /// <returns>A concatenated string of font weight, style, and family.</returns>
        internal string GetTitleFontKey()
        {
            SetDefaultTitleStyle();
            return TitleStyle?.GetFontWeight(Owner?._chartThemeStyle ?? null!) + Constants.Underscore + TitleStyle?.FontStyle + Constants.Underscore + TitleStyle?.GetFontFamily(Owner?._chartThemeStyle ?? null!);
        }

        /// <summary>
        /// Generates a unique font key for the subtitle based on style properties.
        /// </summary>
        /// <returns>A concatenated string of font weight, style, and family.</returns>
        internal string GetSubTitleFontKey()
        {
            SetDefaultSubTitleStyle();
            return SubTitleStyle?.GetFontWeight(Owner?._chartThemeStyle ?? null!) + Constants.Underscore + SubTitleStyle?.FontStyle + Constants.Underscore + SubTitleStyle?.GetFontFamily(Owner?._chartThemeStyle ?? null!);
        }

        /// <summary>
        /// Populates the title collection with wrapped text lines.
        /// </summary>
        /// <param name="rect">The available rectangle for layout.</param>
        internal void SetTitleCollection(Rect rect)
        {
            double titleSize = (TitleStyle?.Position is ChartTitlePosition.Left or ChartTitlePosition.Right) ? rect.Height : rect.Width;
            TitleCollection = ChartHelper.GetTitle(Owner?.Title ?? string.Empty, GetTitleFontOptions(), titleSize);
            TitleSize = ChartHelper.MeasureText(Owner?.Title ?? string.Empty, GetTitleFontOptions());
        }

        /// <summary>
        /// Responds to theme changes and triggers re-rendering.
        /// </summary>
        internal void OnThemeChanged()
        {
            RendererShouldRender = true;
            ProcessRenderQueue();
        }

        /// <summary>
        /// Sets up default renderer values and calculates initial layout.
        /// </summary>
        internal override void SetDefaultRendererValues()
        {
            SetDefaultTitleStyle();
            SetDefaultSubTitleStyle();
            HandleChartSizeChange(Owner?.InitialRect ?? new Rect(0, 0, 0, 0));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles chart size changes and recalculates title layout.
        /// </summary>
        /// <param name="rect">The available rectangle for rendering.</param>
        public override void HandleChartSizeChange(Rect rect)
        {
            if (rect is null)
            {
                return;
            }
            if (IsTitleHandlingRequired(rect))
            {
                IsRendererUpdate = true;
                SetTitleCollection(rect);

                double padding = CalculatePadding();
                double titleHeight = CalculateTitleHeight(padding);

                ProcessSubtitleIfNeeded(rect, ref titleHeight);
                AdjustRectForTitlePosition(ref rect, titleHeight);
                UpdateAvailableRectIfChanged(rect);
            }

            RendererShouldRender = true;
        }

        #endregion
    }
}
