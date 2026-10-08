using System.Reflection;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.NumericTextBox
{
    public class NullableNumericContractTests : BunitTestContext
    {
        [Fact]
        public void NullPlaceholder_RendersWithoutAnAttributeAndKeepsNullStorage()
        {
            var cut = RenderComponent<SfNumericTextBox<decimal?>>(parameters => parameters
                .Add(component => component.Placeholder, (string?)null)
                .Add(component => component.InputAttributes, (Dictionary<string, object>?)null));

            Assert.Null(cut.Find("input").GetAttribute("placeholder"));
            Assert.Null(cut.Instance.Placeholder);
        }

        [Fact]
        public async Task NullPlaceholderAndAttributes_ArePreservedThroughDisposal()
        {
            var numeric = new NumericDisposalProbe();
            Assert.Null(numeric.Placeholder);
            Assert.Null(numeric.InputAttributes);

            numeric.PrepareForDisposal(new Dictionary<string, object> { ["data-test"] = "numeric" });
            await numeric.DisposeAsync();

            Assert.Null(numeric.InputAttributes);
            Assert.Null(numeric.Placeholder);
        }

        [Fact]
        public void RoundNumber_PreservesNullableNumericValues()
        {
            Assert.Equal(1.24m, Round<decimal?>(1.235m, 2));
            Assert.Equal(1.25, Round<double?>(1.25, 2));
            // The existing Convert.ToDecimal/ToDouble path rounds a null input as zero.
            Assert.Equal(0m, Round<decimal?>(null, 2));
            Assert.Equal(0d, Round<double?>(null, 2));
        }

        // Own the disposal state without rendering, which can normalize the nullable attributes.
        private sealed class NumericDisposalProbe : SfNumericTextBox<decimal?>
        {
            public NumericDisposalProbe()
            {
                Placeholder = null;
                InputAttributes = null;
            }

            public void PrepareForDisposal(Dictionary<string, object> attributes)
            {
                InputAttributes = attributes;
                IsRendered = true;
            }
        }

        private static T? Round<T>(T value, int precision)
        {
            MethodInfo method = typeof(SfNumericTextBox<T>).GetMethod("RoundNumber", BindingFlags.Static | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException("Numeric rounding method was not found.");
            return (T?)method.Invoke(null, new object?[] { value, precision });
        }
    }
}
