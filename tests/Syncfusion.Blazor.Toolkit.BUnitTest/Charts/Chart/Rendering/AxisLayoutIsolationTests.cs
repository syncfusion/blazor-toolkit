using System.Reflection;
using Syncfusion.Blazor.Toolkit;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Charts.Chart.Rendering
{
    public class AxisLayoutIsolationTests
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ResettingOverlapState_ClearsOnlyOwningLayout(bool legendToggle)
        {
            var previousAxis = GetInstanceField("_previousAxis");
            var previousAxisEnd = GetInstanceField("_previousAxisEnd");
            var previousStartX = GetInstanceField("_previousStartX");
            var layoutA = new CartesianAxisLayout();
            var layoutB = new CartesianAxisLayout();
            var chartA = CreateChart(layoutA);
            var chartB = CreateChart(layoutB);
            var axisA = new ChartAxis();
            var axisB = new ChartAxis();

            SeedState(layoutA, axisA, 120, 20);
            SeedState(layoutB, axisB, 340, 40);

            // Reproduce the offending interleaving synchronously: B resets while A
            // still needs its previous-axis reference and bounds for overlap checks.
            ResetOverlapState(chartB, legendToggle);
            AssertState(layoutA, axisA, 120, 20);
            // Legend preparation historically preserves the start coordinate; layout resets it.
            AssertState(layoutB, null, 0, legendToggle ? 40 : 0);

            // Reset A as well, proving the reset is neither global nor a no-op.
            SeedState(layoutB, axisB, 560, 60);
            ResetOverlapState(chartA, legendToggle);
            AssertState(layoutB, axisB, 560, 60);
            AssertState(layoutA, null, 0, legendToggle ? 20 : 0);

            void SeedState(CartesianAxisLayout layout, ChartAxis axis, double end, double start)
            {
                previousAxis.SetValue(layout, axis);
                previousAxisEnd.SetValue(layout, end);
                previousStartX.SetValue(layout, start);
            }

            void AssertState(CartesianAxisLayout layout, ChartAxis? axis, double end, double start)
            {
                Assert.Same(axis, previousAxis.GetValue(layout));
                Assert.Equal(end, Assert.IsType<double>(previousAxisEnd.GetValue(layout)));
                Assert.Equal(start, Assert.IsType<double>(previousStartX.GetValue(layout)));
            }
        }

        private static FieldInfo GetInstanceField(string name)
        {
            var field = typeof(CartesianAxisLayout).GetField(name,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            Assert.NotNull(field);
            // SetValue ignores its target for static fields. Fail before any writes
            // so this regression cannot corrupt state used by parallel chart tests.
            Assert.False(field.IsStatic, $"CartesianAxisLayout.{name} must be instance-scoped.");
            return field;
        }

        private static SfChart CreateChart(CartesianAxisLayout layout)
        {
            var chart = new SfChart();
            // Empty renderer collections let the real reset paths run synchronously
            // without browser interop, timing assumptions, or unrelated label layout.
            chart._axisContainer = new ChartAxisRendererContainer { Owner = chart, AxisLayout = layout };
            return chart;
        }

        private static void ResetOverlapState(SfChart chart, bool legendToggle)
        {
            if (legendToggle)
            {
                var series = new ChartSeries { Container = chart };
                var method = typeof(ChartSeries).GetMethod("PrepareForLegendToggle",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.NotNull(method);
                method.Invoke(series, null);
            }
            else
            {
                var method = typeof(ChartAxisRendererContainer).GetMethod("ComputePlotAreaBounds",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.NotNull(method);
                method.Invoke(chart._axisContainer, new object[] { new Rect(0, 0, 600, 400) });
            }
        }
    }
}
