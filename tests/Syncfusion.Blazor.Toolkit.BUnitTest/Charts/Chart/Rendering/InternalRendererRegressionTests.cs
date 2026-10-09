using Bunit;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Charts.Chart.Rendering
{
    public class InternalRendererRegressionTests : BunitTestContext
    {
        public InternalRendererRegressionTests()
        {
            // Mock only browser boundaries; run the real chart and renderer lifecycles.
            ChartBrowserInterop.Setup(JSInterop);
        }

        [Fact]
        public void DefaultChart_InstantiatesInternalRenderers_AndRendersSvg()
        {
            // Before the fix this render fails in SetCharSizeAsync: the title renderer
            // is an unknown markup tag, so it never registers and the font key is null.
            var cut = RenderComponent<SfChart>(parameters => parameters
                .Add(chart => chart.ID, "renderer-regression")
                .Add(chart => chart.Title, "Renderer regression")
                .Add(chart => chart.Width, "600")
                .Add(chart => chart.Height, "450"));

            cut.WaitForAssertion(() =>
            {
                AssertInternalRenderers(cut);
                Assert.Empty(cut.FindComponents<NoDataTemplateContainer>());
                Assert.Null(cut.Instance._noDataTemplateContainer);
                Assert.Empty(cut.FindAll("#noDataTemplateContainer"));
                AssertNoUnknownTag<NoDataTemplateContainer>(cut);
                Assert.Single(cut.FindAll("svg #renderer-regression_ChartBorder"));
                Assert.Equal("Renderer regression", cut.Find("svg #renderer-regression_ChartTitle").TextContent.Trim());
                Assert.NotEmpty(cut.Instance._requestedFontKeys);
                Assert.All(cut.Instance._requestedFontKeys.Keys, key => Assert.False(string.IsNullOrEmpty(key)));
            }, TimeSpan.FromSeconds(5));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void NoDataTemplate_InstantiatesInternalContainer_AndShowsOnlyForEmptyData(bool hasData)
        {
            var data = hasData
                ? new[] { new DataPoint { X = 1, Y = 2 }, new DataPoint { X = 2, Y = 3 } }
                : Array.Empty<DataPoint>();
            RenderFragment noDataTemplate = builder =>
            {
                builder.OpenElement(0, "p");
                builder.AddAttribute(1, "data-testid", "empty-chart");
                builder.AddContent(2, "No data available.");
                builder.CloseElement();
            };

            var cut = RenderComponent<SfChart>(parameters => parameters
                .Add(chart => chart.ID, "template-regression")
                .Add(chart => chart.Width, "600")
                .Add(chart => chart.Height, "450")
                .Add(chart => chart.NoDataTemplate, noDataTemplate)
                .AddChildContent<ChartSeries>(series => series
                    .Add(item => item.DataSource, data)
                    .Add(item => item.XName, nameof(DataPoint.X))
                    .Add(item => item.YName, nameof(DataPoint.Y))
                    .Add(item => item.Type, ChartSeriesType.Line)
                    .AddChildContent<ChartSeriesAnimation>(animation => animation.Add(item => item.Enable, false))));

            cut.WaitForAssertion(() =>
            {
                AssertInternalRenderers(cut);
                Assert.Same(cut.Instance, AssertRegistered(cut, cut.Instance._noDataTemplateContainer).Owner);
                Assert.Single(cut.FindAll("svg #template-regression_ChartBorder"));
                if (hasData)
                {
                    Assert.Empty(cut.FindAll("#noDataTemplateContainer"));
                    Assert.Empty(cut.FindAll("[data-testid='empty-chart']"));
                    var path = cut.Find("svg #template-regressionSeriesCollection #template-regression_Series_0");
                    Assert.False(string.IsNullOrWhiteSpace(path.GetAttribute("d")));
                }
                else
                {
                    Assert.Equal("No data available.", cut.Find(
                        "#template-regression_Secondary_Element #noDataTemplateContainer [data-testid='empty-chart']").TextContent);
                }
            }, TimeSpan.FromSeconds(5));
        }

        private static void AssertInternalRenderers(IRenderedComponent<SfChart> cut)
        {
            var chart = cut.Instance;
            Assert.Same(chart, AssertRegistered(cut, chart._annotationContainer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._datalabelTemplateContainer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._axisLabelTemplateContainer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._legendItemTemplateContainer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._chartBorderRenderer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._chartAreaRenderer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._chartTitleRenderer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._legendRenderer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._columnContainer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._rowContainer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._axisContainer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._striplineBehindContainer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._seriesContainer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._stackLabelRenderer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._customLegendRenderer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._axisOutSideContainer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._trendlineContainer).Owner);
            Assert.Same(chart, AssertRegistered(cut, chart._striplineOverContainer).Owner);
        }

        private static TComponent AssertRegistered<TComponent>(IRenderedComponent<SfChart> cut, TComponent? registered)
            where TComponent : class, IComponent
        {
            var instance = Assert.Single(cut.FindComponents<TComponent>()).Instance;
            Assert.Same(instance, registered);
            Assert.False(typeof(TComponent).IsVisible);
            AssertNoUnknownTag<TComponent>(cut);
            return instance;
        }

        private static void AssertNoUnknownTag<TComponent>(IRenderedComponent<SfChart> cut)
        {
            Assert.DoesNotContain(cut.FindAll("*"), element =>
                string.Equals(element.LocalName, typeof(TComponent).Name, StringComparison.OrdinalIgnoreCase));
        }

        private sealed class DataPoint
        {
            public double X { get; set; }

            public double Y { get; set; }
        }
    }
}
