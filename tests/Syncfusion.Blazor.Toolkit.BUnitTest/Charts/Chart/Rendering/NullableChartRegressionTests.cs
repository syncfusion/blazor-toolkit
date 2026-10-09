using System.Globalization;
using System.Reflection;
using System.Text.Json;
using Bunit;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using Syncfusion.Blazor.Toolkit.Data;
using Xunit;
using ChartValueType = Syncfusion.Blazor.Toolkit.ValueType;

namespace Syncfusion.Blazor.Toolkit.Tests.Charts.Chart.Rendering
{
    public class NullableChartRegressionTests
    {
        [Fact]
        public void AppendTextElements_WithoutOwner_ReturnsInvariantCoordinates()
        {
            // Axis/stripline renderers can call this helper before an owner is available.
            CultureInfo previousCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
                Assert.Equal(new[] { "12.5", "-3.25" },
                    ChartHelper.AppendTextElements(null, "unowned-label", 12.5, -3.25));
            }
            finally
            {
                CultureInfo.CurrentCulture = previousCulture;
            }
        }

        [Fact]
        public void AppendTextElements_InteractiveRedraw_PreservesPreviousCoordinates()
        {
            // This test project targets net8, where static-server detection returns false.
            var chart = new SfChart();
            Assert.Equal(new[] { "10", "20" }, ChartHelper.AppendTextElements(chart, "label", 10, 20));
            chart._redraw = true;
            Assert.Equal(new[] { "10", "20" }, ChartHelper.AppendTextElements(chart, "label", 30, 40));
            var animation = chart._textAnimationElements["label"];
            Assert.Equal(30, animation.CurLocationX);
            Assert.Equal(40, animation.CurLocationY);
        }

        [Fact]
        public void DataEditingSettings_AreOptional_AndCanBeCleared()
        {
            var series = new ChartSeries();
            Assert.Null(series.ChartDataEditSettings);
            var settings = SetParameters(new ChartDataEditSettings(), (nameof(ChartDataEditSettings.Enable), true));
            settings._isPropertyChanged = true;
            SetParameters(series, (nameof(ChartSeries.ChartDataEditSettings), settings));
            Assert.True(settings.Enable);
            Assert.Same(settings, series.ChartDataEditSettings);
            Assert.False(settings._isPropertyChanged);
            SetParameters(series, (nameof(ChartSeries.ChartDataEditSettings), null));
            Assert.Null(series.ChartDataEditSettings);
        }

        [Fact]
        public void PointModels_PreserveNullValues_AndExistingStringDefaults()
        {
            var point = new Point();
            Assert.Null(point.X);
            Assert.Null(point.Y);
            Assert.Equal(string.Empty, point.Text);
            Assert.Equal(string.Empty, point.Tooltip);
            point.Text = null;
            point.Tooltip = null;
            var wirePoint = new IChartPoint { X = point.X, Y = point.Y, Text = point.Text, Tooltip = point.Tooltip };
            Assert.Null(wirePoint.X);
            Assert.Null(wirePoint.Y);
            Assert.Null(wirePoint.Text);
            Assert.Null(wirePoint.Tooltip);
            Assert.Null(new BubblePoint().Size);
            Assert.Null(new IBubblePoint().Size);
        }

        [Fact]
        public void JsonNull_RemainsNull_NotAnEmptyString()
        {
            using var document = JsonDocument.Parse("{\"null\":null,\"empty\":\"\",\"number\":2.5}");
            Assert.Null(ChartHelper.GetObjectValue(document.RootElement.GetProperty("null")));
            Assert.Equal(string.Empty, ChartHelper.GetObjectValue(document.RootElement.GetProperty("empty")));
            Assert.Equal(2.5, ChartHelper.GetObjectValue(document.RootElement.GetProperty("number")));
        }

        [Fact]
        public void NestedPropertyLookup_PreservesMissingAndNullValues()
        {
            var data = new NullableDatum();
            Assert.Null(ChartSeriesRenderer.GetPropertyValue(null, "Y"));
            Assert.Null(ChartSeriesRenderer.GetPropertyValue(data, null));
            Assert.Null(ChartSeriesRenderer.GetPropertyValue(data, "Missing"));
            Assert.Null(ChartSeriesRenderer.GetPropertyValue(data, "Child.Y"));
            Assert.Null(ChartSeriesRenderer.GetPropertyValue(data, "Y"));
            data.Child = new NullableDatum { Y = 7 };
            Assert.Equal(7, ChartSeriesRenderer.GetPropertyValue(data, "Child.Y"));
        }

        [Theory]
        [InlineData(ListSortDirection.Ascending)]
        [InlineData(ListSortDirection.Descending)]
        public void Sorting_PreservesNullVersusEmptyKeys_AndWirePointOrder(ListSortDirection direction)
        {
            var chart = new SfChart();
            chart._sorting.SetSortKeyAndDirection("X", direction);
            var renderer = new LineSeriesRenderer
            {
                XAxisRenderer = new CategoryAxisRenderer
                {
                    Axis = SetParameters(new ChartPrimaryXAxis(), (nameof(ChartPrimaryXAxis.ValueType), ChartValueType.Category))
                },
                Points = new List<Point> { new() { X = "b" }, new() { X = "" }, new() { X = null }, new() { X = "a" } },
                ChartPoints = new List<IChartPoint> { new() { X = "b" }, new() { X = "" }, new() { X = null }, new() { X = "a" } }
            };
            var container = new ChartSeriesRendererContainer { Owner = chart };
            container.Renderers.Add(renderer);
            container.Sorting();

            object?[] expected = direction == ListSortDirection.Ascending
                ? new object?[] { null, "", "a", "b" }
                : new object?[] { "b", "a", "", null };
            Assert.NotNull(renderer.Points);
            Assert.Equal(expected, renderer.Points.Select(point => point.X));
            Assert.NotNull(renderer.ChartPoints);
            Assert.Equal(expected, renderer.ChartPoints.Select(point => point.X));
            Assert.Equal(expected.OfType<string>(), renderer.XAxisRenderer.Labels);
            for (int i = 0; i < renderer.Points.Count; i++)
            {
                var point = renderer.Points[i];
                var wirePoint = renderer.ChartPoints[i];
                Assert.Equal(i, point.Index);
                Assert.Equal(i, wirePoint.Index);
                Assert.Equal(point.XValue, wirePoint.XValue);
                if (point.X is string label)
                {
                    Assert.Equal(label, renderer.XAxisRenderer.Labels[(int)point.XValue]);
                }
            }
        }

        [Theory]
        [InlineData(ListSortDirection.Ascending, 0, "X")]
        [InlineData(ListSortDirection.Ascending, 1, "X")]
        [InlineData(ListSortDirection.Ascending, 3, "X")]
        [InlineData(ListSortDirection.Descending, 0, "X")]
        [InlineData(ListSortDirection.Descending, 1, "X")]
        [InlineData(ListSortDirection.Descending, 3, "X")]
        [InlineData(ListSortDirection.Ascending, 0, "Y")]
        [InlineData(ListSortDirection.Ascending, 1, "Y")]
        [InlineData(ListSortDirection.Ascending, 3, "Y")]
        [InlineData(ListSortDirection.Descending, 0, "Y")]
        [InlineData(ListSortDirection.Descending, 1, "Y")]
        [InlineData(ListSortDirection.Descending, 3, "Y")]
        public void Sorting_DateCategories_NullXDoesNotCreateLabelGaps(ListSortDirection direction, int nullIndex, string sortKey)
        {
            var early = new DateTime(2026, 1, 2);
            var late = new DateTime(2026, 1, 3);
            var keys = new List<object?> { late, early, early };
            keys.Insert(nullIndex, null);
            var chart = new SfChart();
            chart._sorting.SetSortKeyAndDirection(sortKey, direction);
            var renderer = new LineSeriesRenderer
            {
                XAxisRenderer = new DateTimeCategoryAxisRenderer
                {
                    Axis = SetParameters(new ChartPrimaryXAxis(), (nameof(ChartPrimaryXAxis.ValueType), ChartValueType.DateTimeCategory))
                },
                Points = keys.Select((key, index) => new Point
                {
                    X = key, Y = index, Index = index, XValue = key is null ? double.NaN : index,
                    Visible = key is not null, SumOfSameIndex = index
                }).ToList(),
                ChartPoints = keys.Select((key, index) => new IChartPoint
                {
                    X = key, Y = index, Index = index, XValue = key is null ? double.NaN : index,
                    SumOfSameIndex = index
                }).ToList()
            };
            var container = new ChartSeriesRendererContainer { Owner = chart };
            container.Renderers.Add(renderer);

            // Exercise both sorting stages, then repeat to check stable point/wire alignment.
            // Sorting by Y also leaves null X values between valid date points.
            for (int pass = 0; pass < 2; pass++)
            {
                container.Sorting();
                object?[] expected = direction == ListSortDirection.Ascending
                    ? new object?[] { null, early, early, late }
                    : new object?[] { late, early, early, null };
                if (sortKey == "Y")
                {
                    expected = direction == ListSortDirection.Ascending ? keys.ToArray() : keys.AsEnumerable().Reverse().ToArray();
                }
                Assert.NotNull(renderer.Points);
                Assert.NotNull(renderer.ChartPoints);
                Assert.Equal(expected, renderer.Points.Select(point => point.X));
                Assert.Equal(expected, renderer.ChartPoints.Select(point => point.X));
                // Preserve the existing one-label-per-date-point behavior, including duplicate dates.
                Assert.Equal(expected.OfType<DateTime>().Select(date =>
                    ChartHelper.GetTime(date).ToString(CultureInfo.InvariantCulture)), renderer.XAxisRenderer.Labels);
                Assert.DoesNotContain(string.Empty, renderer.XAxisRenderer.Labels);
                int labelIndex = 0;
                for (int i = 0; i < renderer.Points.Count; i++)
                {
                    var point = renderer.Points[i];
                    var wirePoint = renderer.ChartPoints[i];
                    Assert.Equal(i, point.Index);
                    Assert.Equal(i, wirePoint.Index);
                    Assert.Equal(point.Y, wirePoint.Y);
                    Assert.Equal(point.XValue, wirePoint.XValue);
                    if (point.X is DateTime date)
                    {
                        Assert.Equal((double)labelIndex++, point.XValue);
                        Assert.Equal(ChartHelper.GetTime(date).ToString(CultureInfo.InvariantCulture),
                            renderer.XAxisRenderer.Labels[(int)point.XValue]);
                    }
                    else
                    {
                        Assert.True(double.IsNaN(point.XValue));
                        Assert.False(point.Visible);
                    }
                }
            }
        }

        [Theory]
        [InlineData(ListSortDirection.Ascending)]
        [InlineData(ListSortDirection.Descending)]
        public void Sorting_DateCategories_OnlyNullXProducesNoLabels(ListSortDirection direction)
        {
            var chart = new SfChart();
            chart._sorting.SetSortKeyAndDirection("X", direction);
            var renderer = new LineSeriesRenderer
            {
                XAxisRenderer = new DateTimeCategoryAxisRenderer
                {
                    Axis = SetParameters(new ChartPrimaryXAxis(), (nameof(ChartPrimaryXAxis.ValueType), ChartValueType.DateTimeCategory))
                },
                Points = new List<Point> { new() { X = null, XValue = double.NaN, Index = 4 } },
                ChartPoints = new List<IChartPoint> { new() { X = null, XValue = double.NaN, Index = 4 } }
            };
            var container = new ChartSeriesRendererContainer { Owner = chart };
            container.Renderers.Add(renderer);
            container.Sorting();

            Assert.Empty(renderer.XAxisRenderer.Labels);
            var point = Assert.Single(renderer.Points);
            var wirePoint = Assert.Single(renderer.ChartPoints);
            Assert.Equal(0, point.Index);
            Assert.Equal(point.Index, wirePoint.Index);
            Assert.True(double.IsNaN(point.XValue));
            Assert.True(double.IsNaN(wirePoint.XValue));
        }

        [Fact]
        public void BorderOverrides_AcceptNull_WithoutChangingDefaults()
        {
            ChartDefaultBorder lastLabel = new ChartLastDataLabelBorder();
            ChartDefaultBorder crosshair = new ChartCrosshairLine();
            Assert.Equal(string.Empty, lastLabel.Color);
            Assert.Equal(string.Empty, crosshair.Color);
            SetParameters(lastLabel, (nameof(ChartDefaultBorder.Color), null));
            SetParameters(crosshair, (nameof(ChartDefaultBorder.Color), null));
            Assert.Null(lastLabel.Color);
            Assert.Null(crosshair.Color);
        }

        [Fact]
        public void SizeEquals_NullAndSameReference_RetainBehavior()
        {
            var size = new Size(10, 20);
            Assert.False(size.Equals(null));
            Assert.True(size.Equals(size));
        }

        [Fact]
        public void FontStyle_RequiresConfiguredFont_AndExplicitFontIsUnchanged()
        {
            Assert.Throws<ArgumentNullException>(() => ChartHelper.GetFontStyle(null!));
            var font = SetParameters(new ChartDataLabelFont(),
                (nameof(ChartDataLabelFont.Size), "13px"),
                (nameof(ChartDataLabelFont.FontStyle), "italic"),
                (nameof(ChartDataLabelFont.FontWeight), "600"),
                (nameof(ChartDataLabelFont.FontFamily), "serif"),
                (nameof(ChartDataLabelFont.Opacity), 1d),
                (nameof(ChartDataLabelFont.Color), "red"));
            Assert.Equal("font-size:13px; font-style:italic; font-weight:600; font-family:serif;opacity:1; color:red;",
                ChartHelper.GetFontStyle(font));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void DataLabelRendering_RejectsExplicitNullFontBeforeLayout(bool useTemplate)
        {
            var renderer = new ChartDataLabelRenderer { SeriesRenderer = new LineSeriesRenderer() };
            var label = SetParameters(new ChartDataLabel(), (nameof(ChartDataLabel.Font), null));
            if (useTemplate)
            {
                RenderFragment<ChartDataPointInfo> template = _ => builder => builder.AddContent(0, "Template");
                SetParameters(label, (nameof(ChartDataLabel.Template), template));
                Assert.Same(template, label.Template);
            }
            Assert.Null(label.Font);

            // Omitting a font child uses ChartDataLabel's configured default. Explicit null
            // is not a supported missing-font mode for either SVG labels or templates.
            var exception = Assert.Throws<ArgumentNullException>(() =>
                renderer.CalculateRenderTreeBuilderOptions(new ChartSeries(), label));
            Assert.Equal(nameof(ChartDataLabel.Font), exception.ParamName);
        }

        [Theory]
        [InlineData(false, Theme.Fluent)]
        [InlineData(false, Theme.FluentDark)]
        [InlineData(true, Theme.Fluent)]
        [InlineData(true, Theme.FluentDark)]
        public void DataLabels_RealChartLifecycle_PreservesDefaultAndExplicitFonts(bool explicitFont, Theme theme)
        {
            using var context = new BunitTestContext();
            ChartBrowserInterop.Setup(context.JSInterop);
            var cut = context.RenderComponent<SfChart>(parameters => parameters
                .Add(chart => chart.ID, "nullable-font")
                .Add(chart => chart.Width, "600")
                .Add(chart => chart.Height, "450")
                .Add(chart => chart.Theme, theme)
                .AddChildContent<ChartSeries>(series => series
                    .Add(item => item.DataSource, new[] { new NullableDatum { X = 1, Y = 2 }, new NullableDatum { X = 2, Y = 3 } })
                    .Add(item => item.XName, nameof(NullableDatum.X))
                    .Add(item => item.YName, nameof(NullableDatum.Y))
                    .Add(item => item.Type, ChartSeriesType.Line)
                    .AddChildContent<ChartSeriesAnimation>(animation => animation.Add(item => item.Enable, false))
                    .AddChildContent<ChartMarker>(marker => marker.AddChildContent<ChartDataLabel>(label =>
                    {
                        label.Add(item => item.Visible, true)
                            .Add(item => item.Position, ChartLabelPosition.Top)
                            .Add(item => item.LabelIntersectAction, "None");
                        if (explicitFont)
                        {
                            label.AddChildContent<ChartDataLabelFont>(font => font
                                .Add(item => item.Size, "13px")
                                .Add(item => item.FontFamily, "serif")
                                .Add(item => item.FontWeight, "600")
                                .Add(item => item.FontStyle, "italic")
                                .Add(item => item.Color, "red"));
                        }
                    }))));

            cut.WaitForAssertion(() =>
            {
                var label = cut.FindComponent<ChartDataLabel>().Instance;
                var renderer = Assert.Single(cut.FindComponents<ChartDataLabelRenderer>()).Instance;
                Assert.Same(renderer, label.Renderer);
                Assert.NotNull(label.Font);
                var themeStyle = cut.Instance._chartThemeStyle;
                Assert.NotNull(themeStyle);
                var text = cut.Find("svg #nullable-font_Series_0_Point_0_Text_0");
                Assert.Equal("2", text.TextContent.Trim());
                Assert.Equal(2, cut.FindAll("svg #nullable-fontTextGroup0 text").Count);
                Assert.Equal(explicitFont ? "13px" : themeStyle.DataLabelSize, text.GetAttribute("font-size"));
                Assert.Equal(explicitFont ? "serif" : themeStyle.DataLabelFontFamily, text.GetAttribute("font-family"));
                Assert.Equal(explicitFont ? "600" : themeStyle.DataLabelFontWeight, text.GetAttribute("font-weight"));
                if (explicitFont)
                {
                    Assert.Same(cut.FindComponent<ChartDataLabelFont>().Instance, label.Font);
                    Assert.Equal("italic", text.GetAttribute("font-style"));
                    Assert.Equal("red", text.GetAttribute("fill"));
                }
                else
                {
                    // Theme resolution must not overwrite the default font's inheritance markers.
                    Assert.Empty(label.Font.Size);
                    Assert.Empty(label.Font.FontFamily);
                    Assert.Empty(label.Font.FontWeight);
                }
            }, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void DataLabelFont_StillResolvesExistingThemeFallbacks()
        {
            var theme = new ChartThemeStyle { DataLabelSize = "17px", DataLabelFontFamily = "theme-family", DataLabelFontWeight = "700" };
            var font = SetParameters(new ChartDataLabelFont(),
                (nameof(ChartDataLabelFont.Size), ""),
                (nameof(ChartDataLabelFont.FontFamily), ""),
                (nameof(ChartDataLabelFont.FontWeight), ""));
            var resolved = font.GetFontOptions(theme);
            Assert.Equal("17px", resolved.Size);
            Assert.Equal("theme-family", resolved.FontFamily);
            Assert.Equal("700", resolved.FontWeight);
        }

        [Fact]
        public void StriplineTooltip_UsesOwningChartTheme()
        {
            var chart = SetParameters(new SfChart(), (nameof(SfChart.Theme), Theme.FluentDark));
            chart.InitialRect = new Rect(0, 0, 600, 450);
            var tooltip = new ChartStriplineTooltipSettings(chart);
            var method = typeof(ChartStriplineTooltipSettings).GetMethod("BuildSvgTooltip", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            var options = Assert.IsType<SVGTooltip>(method.Invoke(tooltip, new object[]
            {
                new ChartStriplineTooltip(), "header", new ChartStripline(), new TextStyleModel()
            }));
            Assert.Equal("FluentDark", options.Theme);
            SetParameters(chart, (nameof(SfChart.Theme), Theme.HighContrast));
            options = Assert.IsType<SVGTooltip>(method.Invoke(tooltip, new object[]
            {
                new ChartStriplineTooltip(), "header", new ChartStripline(), new TextStyleModel()
            }));
            Assert.Equal("HighContrast", options.Theme);
        }

        [Fact]
        public void ObjectSeries_MissingXMapping_ProducesNoPoints()
        {
            var renderer = CreateLineRenderer();
            InvokeObjectProcessing(renderer, "Missing", new NullableDatum { X = 1, Y = 2 });
            Assert.NotNull(renderer.Points);
            Assert.NotNull(renderer.ChartPoints);
            Assert.Empty(renderer.Points);
            Assert.Empty(renderer.ChartPoints);
            Assert.False(renderer.XAxisRenderer.IsDateOnly);
            Assert.False(renderer.XAxisRenderer.IsTimeOnly);
            Assert.False(renderer.IsDateTimeOffset);
        }

        [Fact]
        public void ObjectSeries_NullMappedLabel_RemainsNull_AndNullYUsesGapSentinel()
        {
            var renderer = CreateLineRenderer();
            InvokeObjectProcessing(renderer, nameof(NullableDatum.X), new NullableDatum { X = 1, Y = 2 });
            Assert.NotNull(renderer.Points);
            Assert.NotNull(renderer.ChartPoints);
            var point = Assert.Single(renderer.Points);
            Assert.Null(point.Text);
            Assert.Null(Assert.Single(renderer.ChartPoints).Text);
            Assert.Equal(2, point.YValue);

            renderer = CreateLineRenderer();
            InvokeObjectProcessing(renderer, nameof(NullableDatum.X), new NullableDatum { X = 1, Y = null });
            Assert.NotNull(renderer.Points);
            point = Assert.Single(renderer.Points);
            Assert.True(point.IsEmpty);
            Assert.False(point.Visible);
            Assert.Equal(0, point.YValue);
        }

        [Fact]
        public void BubbleSeries_NullXAndSize_AreEmptyWithoutInventingCategoryLabel()
        {
            var renderer = new BubbleSeriesRenderer
            {
                Series = new ChartSeries(),
                XAxisRenderer = new CategoryAxisRenderer
                {
                    Axis = SetParameters(new ChartPrimaryXAxis(), (nameof(ChartPrimaryXAxis.ValueType), ChartValueType.Category))
                },
                YAxisRenderer = new ChartAxisRenderer { Axis = new ChartPrimaryYAxis() },
                XData = new List<double> { 0 },
                YData = new List<double>(),
                Points = new List<Point>(),
                ChartPoints = new List<IChartPoint>()
            };
            var point = new BubblePoint { X = null, Y = 2, Size = null };
            var wirePoint = new IBubblePoint { X = null, Y = 2, Size = null };
            renderer.GetSetXValue(point, wirePoint, 0);
            renderer.SetEmptyPoint(point, wirePoint, 0, typeof(NullableDatum));
            Assert.Null(point.X);
            Assert.Null(point.Size);
            Assert.True(point.IsEmpty);
            Assert.False(point.Visible);
            Assert.Empty(renderer.XAxisRenderer.Labels);
        }

        [Fact]
        public void Annotation_NullCoordinates_KeepPixelNaNSentinel_AndTrackNullChange()
        {
            var annotation = SetParameters(new ChartAnnotation(),
                (nameof(ChartAnnotation.X), null), (nameof(ChartAnnotation.Y), null));
            Assert.Null(annotation.X);
            Assert.Null(annotation.Y);
            var lifecycle = typeof(ChartAnnotation).GetMethod("OnParametersSet", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(lifecycle);
            lifecycle.Invoke(annotation, null);
            var cachedX = typeof(ChartAnnotation).GetField("_xCoordinate", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(cachedX);
            Assert.Null(cachedX.GetValue(annotation));
            Assert.True(double.IsNaN(ChartHelper.StringToNumber(annotation.X?.ToString(), 600)));
            Assert.True(double.IsNaN(ChartHelper.StringToNumber(annotation.Y, 450)));
            Assert.Equal(150, ChartHelper.StringToNumber("25%", 600));
        }

        [Fact]
        public void DateTimeDetection_NullFirstX_IsNotUniversal()
        {
            var axis = new ChartAxisRenderer
            {
                Axis = SetParameters(new ChartPrimaryXAxis(), (nameof(ChartPrimaryXAxis.ValueType), ChartValueType.DateTime))
            };
            axis.SeriesRenderer.Add(new LineSeriesRenderer { Points = new List<Point> { new() { X = null } } });
            Assert.False(SfChart.IsUniversalDateTime(axis));
        }

        [Fact]
        public void PointFormatting_PropagatesNullWithoutChangingNumericConversion()
        {
            var renderer = CreateLineRenderer();
            var point = new Point { X = null, Y = null };
            Assert.Null(renderer.GetMarkerY(point));
            Assert.Null(renderer.GetPointXValue(point.X, string.Empty));
            Assert.Null(renderer.XAxisRenderer.GetFormatText(point.X));
            var numericAxis = new NumericAxisRenderer { Axis = new ChartPrimaryYAxis() };
            Assert.Equal(numericAxis.GetFormatText(0), numericAxis.GetFormatText(null));
        }

        [Theory]
        [InlineData(ChartLabelPosition.Top, 90)]
        [InlineData(ChartLabelPosition.Outer, 90)]
        [InlineData(ChartLabelPosition.Bottom, 110)]
        public void PathLabelPosition_PreservesZeroErrorHeightSpacing(ChartLabelPosition position, double expected)
        {
            var renderer = new ChartDataLabelRenderer();
            var margin = typeof(ChartDataLabelRenderer).GetField("_margin", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(margin);
            margin.SetValue(renderer, new ChartEventMargin { Top = 0, Bottom = 0, Left = 0, Right = 0 });
            var method = typeof(ChartDataLabelRenderer).GetMethod("CalculatePathPosition", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            var actual = Assert.IsType<double>(method.Invoke(renderer, new object[]
            {
                100d, new Rect(0, 0, 200, 200), false, position, new ChartSeries(), new Point(), new Size(20, 10), 0
            }));
            Assert.Equal(expected, actual);
        }

        // Invoke the original parameter setters on standalone models without starting a renderer lifecycle.
        private static T SetParameters<T>(T instance, params (string Name, object? Value)[] parameters) where T : IComponent
        {
            ParameterView.FromDictionary(parameters.ToDictionary(parameter => parameter.Name, parameter => parameter.Value))
                .SetParameterProperties(instance);
            return instance;
        }

        private static LineSeriesRenderer CreateLineRenderer()
        {
            var series = new ChartSeries();
            SetParameters(series.Marker.DataLabel, (nameof(ChartDataLabel.Name), nameof(NullableDatum.Text)));
            return new LineSeriesRenderer
            {
                Owner = new SfChart(),
                Series = series,
                XAxisRenderer = new ChartAxisRenderer { Axis = new ChartPrimaryXAxis() },
                YAxisRenderer = new ChartAxisRenderer { Axis = new ChartPrimaryYAxis() },
                Points = new List<Point>(),
                ChartPoints = new List<IChartPoint>(),
                YData = new List<double>()
            };
        }

        private static void InvokeObjectProcessing(ChartSeriesRenderer renderer, string xName, NullableDatum data)
        {
            var method = typeof(ChartSeriesRenderer).GetMethod("ProcessObjectData", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            method.Invoke(renderer, new object[] { typeof(NullableDatum), xName, nameof(NullableDatum.Y), new object[] { data } });
        }

        private sealed class NullableDatum
        {
            public object? X { get; set; }
            public object? Y { get; set; }
            public string? Text { get; set; }
            public NullableDatum? Child { get; set; }
        }
    }
}
