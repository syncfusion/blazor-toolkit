using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// The overflow algorithm (Spec section 6): stable sort, greedy placement, overflow marking.
    /// The calculator is pure, so these tests need no renderer.
    /// </summary>
    public class ToolbarOverflowCalculatorTests
    {
        private static OverflowCandidate C(string id, double size, ToolbarOverflowOption option = ToolbarOverflowOption.None, int priority = 0)
        {
            return new OverflowCandidate(id, size, option, priority);
        }

        private static string[] Ids(IEnumerable<OverflowCandidate> items) => items.Select(i => i.Id).ToArray();

        // ------------------------------------------------------------------ sorting

        [Fact(DisplayName = "U-TB-023 Sort order is Show, then None, then Hide")]
        public void Sort_ByOption()
        {
            OverflowCandidate[] sorted = OverflowCalculator.Sort(
            [
                C("hide", 10, ToolbarOverflowOption.Hide),
                C("none", 10, ToolbarOverflowOption.None),
                C("show", 10, ToolbarOverflowOption.Show)
            ]);

            Assert.Equal(new[] { "show", "none", "hide" }, Ids(sorted));
        }

        [Fact(DisplayName = "U-TB-023 Within one option, a lower Priority sorts first")]
        public void Sort_ByPriority()
        {
            OverflowCandidate[] sorted = OverflowCalculator.Sort(
            [
                C("p5", 10, priority: 5),
                C("p1", 10, priority: 1),
                C("p3", 10, priority: 3)
            ]);

            Assert.Equal(new[] { "p1", "p3", "p5" }, Ids(sorted));
        }

        [Fact(DisplayName = "U-TB-023 Equal option and Priority keep declaration order (stable sort)")]
        public void Sort_IsStable()
        {
            OverflowCandidate[] sorted = OverflowCalculator.Sort(
            [
                C("a", 10), C("b", 10), C("c", 10), C("d", 10)
            ]);

            Assert.Equal(new[] { "a", "b", "c", "d" }, Ids(sorted));
        }

        [Fact(DisplayName = "U-TB-023 A negative Priority sorts before zero, so the item stays visible longer")]
        public void Sort_NegativePriorityIsLower()
        {
            OverflowCandidate[] sorted = OverflowCalculator.Sort([C("zero", 10), C("neg", 10, priority: -3)]);
            Assert.Equal(new[] { "neg", "zero" }, Ids(sorted));
        }

        [Fact(DisplayName = "U-TB-023 Option outranks Priority")]
        public void Sort_OptionBeatsPriority()
        {
            OverflowCandidate[] sorted = OverflowCalculator.Sort(
            [
                C("hideLow", 10, ToolbarOverflowOption.Hide, priority: -100),
                C("showHigh", 10, ToolbarOverflowOption.Show, priority: 100)
            ]);

            Assert.Equal(new[] { "showHigh", "hideLow" }, Ids(sorted));
        }

        // ------------------------------------------------------------------ placement

        [Fact(DisplayName = "U-TB-024 All items fit: nothing overflows")]
        public void Place_AllFit()
        {
            OverflowResult result = OverflowCalculator.Calculate([C("a", 40), C("b", 40), C("c", 40)], available: 200, gap: 4, overflowButtonSize: 32);

            Assert.Equal(new[] { "a", "b", "c" }, Ids(result.Visible));
            Assert.Empty(result.Overflowed);
            Assert.False(result.HasOverflow);
        }

        [Fact(DisplayName = "U-TB-025 Partial overflow: the items that do not fit move to the overflow list")]
        public void Place_Partial()
        {
            // Overflow button reserved (32 + 4 gap). Available for items: 200 - 36 = 164. a=40, b=40 (+4 gap), c=40 (+4 gap), d=40 would need 172.
            OverflowResult result = OverflowCalculator.Calculate([C("a", 40), C("b", 40), C("c", 40), C("d", 40), C("e", 40)], available: 200, gap: 4, overflowButtonSize: 32);

            Assert.Equal(new[] { "a", "b", "c" }, Ids(result.Visible).Order().ToArray());
            Assert.Equal(new[] { "d", "e" }, Ids(result.Overflowed));
            Assert.True(result.HasOverflow);
        }

        [Fact(DisplayName = "U-TB-026 Nothing fits: every item overflows")]
        public void Place_AllOverflow()
        {
            OverflowResult result = OverflowCalculator.Calculate([C("a", 80), C("b", 80)], available: 50, gap: 4, overflowButtonSize: 32);

            Assert.Empty(result.Visible);
            Assert.Equal(new[] { "a", "b" }, Ids(result.Overflowed));
            Assert.True(result.HasOverflow);
        }

        [Fact(DisplayName = "U-TB-025 A smaller item after a large one that overflowed does not jump the queue")]
        public void Place_StopsAtFirstMisfit()
        {
            // Greedy and ordered: once an item does not fit, later items overflow too, even if they are small.
            OverflowResult result = OverflowCalculator.Calculate([C("big", 150), C("huge", 400), C("tiny", 5)], available: 200, gap: 4, overflowButtonSize: 32);

            Assert.Equal(new[] { "big" }, Ids(result.Visible));
            Assert.Equal(new[] { "huge", "tiny" }, Ids(result.Overflowed));
        }

        [Fact(DisplayName = "U-TB-025 The overflow button's space is only reserved when something overflows")]
        public void Place_ReservesButtonOnlyWhenNeeded()
        {
            // 3 x 60 + 2 gaps of 4 = 188 fits in 190 without the button, so the button must not shrink the budget.
            OverflowResult fits = OverflowCalculator.Calculate([C("a", 60), C("b", 60), C("c", 60)], available: 190, gap: 4, overflowButtonSize: 32);
            Assert.False(fits.HasOverflow);
            Assert.Equal(3, fits.Visible.Count);

            // One pixel less and the third no longer fits once the button is reserved.
            OverflowResult tight = OverflowCalculator.Calculate([C("a", 60), C("b", 60), C("c", 60)], available: 187, gap: 4, overflowButtonSize: 32);
            Assert.True(tight.HasOverflow);
            Assert.Equal(new[] { "c" }, Ids(tight.Overflowed));
        }

        [Fact(DisplayName = "U-TB-023 Visible items keep declaration order, not sort order")]
        public void Place_VisibleKeepsDeclarationOrder()
        {
            // 'late' has the best priority so it is placed first, but it is declared last and must render last.
            // Budget: 150 - (32 button + 4 gap) = 114. late 50, then first 54 = 104 fits, second 54 would be 158.
            OverflowResult result = OverflowCalculator.Calculate(
                [C("first", 50), C("second", 50), C("late", 50, priority: -1)], available: 150, gap: 4, overflowButtonSize: 32);

            Assert.Equal(new[] { "first", "late" }, Ids(result.Visible));
            Assert.Equal(new[] { "second" }, Ids(result.Overflowed));
        }

        [Fact(DisplayName = "U-TB-023 Overflow=Show keeps an item visible ahead of earlier items")]
        public void Place_ShowBeatsDeclarationOrder()
        {
            OverflowResult result = OverflowCalculator.Calculate(
                [C("a", 80), C("b", 80), C("pinned", 80, ToolbarOverflowOption.Show)], available: 130, gap: 4, overflowButtonSize: 32);

            Assert.Contains("pinned", Ids(result.Visible));
            Assert.DoesNotContain("pinned", Ids(result.Overflowed));
        }

        [Fact(DisplayName = "U-TB-023 Overflow=Hide items overflow first and are not shown in the bar while others overflow")]
        public void Place_HideGoesFirst()
        {
            OverflowResult result = OverflowCalculator.Calculate(
                [C("hidden", 40, ToolbarOverflowOption.Hide), C("a", 40), C("b", 40)], available: 100, gap: 4, overflowButtonSize: 32);

            Assert.Contains("hidden", Ids(result.Overflowed));
        }

        // ------------------------------------------------------------------ degenerate input

        [Fact(DisplayName = "U-TB-066 A zero-width container overflows everything and does not throw")]
        public void Edge_ZeroWidth()
        {
            OverflowResult result = OverflowCalculator.Calculate([C("a", 40), C("b", 40)], available: 0, gap: 4, overflowButtonSize: 32);

            Assert.Empty(result.Visible);
            Assert.Equal(2, result.Overflowed.Count);
        }

        [Fact(DisplayName = "U-TB-066 A negative available size is treated as zero")]
        public void Edge_NegativeAvailable()
        {
            OverflowResult result = OverflowCalculator.Calculate([C("a", 40)], available: -50, gap: 4, overflowButtonSize: 32);

            Assert.Empty(result.Visible);
            Assert.Single(result.Overflowed);
        }

        [Fact(DisplayName = "U-TB-066 An empty item list yields an empty result")]
        public void Edge_NoItems()
        {
            OverflowResult result = OverflowCalculator.Calculate([], available: 100, gap: 4, overflowButtonSize: 32);

            Assert.Empty(result.Visible);
            Assert.Empty(result.Overflowed);
            Assert.False(result.HasOverflow);
        }

        [Fact(DisplayName = "U-TB-066 A zero-size item fits anywhere and does not break the budget")]
        public void Edge_ZeroSizeItem()
        {
            OverflowResult result = OverflowCalculator.Calculate([C("z", 0), C("a", 40)], available: 50, gap: 4, overflowButtonSize: 32);

            Assert.Contains("z", Ids(result.Visible));
        }

        [Fact(DisplayName = "U-TB-066 Calculation does not mutate its input")]
        public void Edge_DoesNotMutateInput()
        {
            OverflowCandidate[] input = [C("b", 10, priority: 2), C("a", 10, priority: 1)];
            string[] before = Ids(input);

            _ = OverflowCalculator.Calculate(input, available: 100, gap: 4, overflowButtonSize: 32);

            Assert.Equal(before, Ids(input));
        }

        // ------------------------------------------------------------------ combinatorial

        public static IEnumerable<object[]> Combinations()
        {
            foreach (ToolbarOverflowOption option in Enum.GetValues<ToolbarOverflowOption>())
            {
                foreach (int priority in new[] { -1, 0, 3 })
                {
                    foreach (double available in new[] { 0d, 60d, 140d, 400d })
                    {
                        yield return new object[] { option, priority, available };
                    }
                }
            }
        }

        [Theory(DisplayName = "U-TB-025 Every item lands in exactly one list for any option, priority and width")]
        [MemberData(nameof(Combinations))]
        public void Combinatorial_PartitionIsExact(ToolbarOverflowOption option, int priority, double available)
        {
            OverflowCandidate[] items =
            [
                C("a", 50), C("b", 50, option, priority), C("c", 50, ToolbarOverflowOption.Show), C("d", 50, ToolbarOverflowOption.Hide, priority)
            ];

            OverflowResult result = OverflowCalculator.Calculate(items, available, gap: 4, overflowButtonSize: 32);

            string[] all = Ids(result.Visible).Concat(Ids(result.Overflowed)).Order().ToArray();
            Assert.Equal(new[] { "a", "b", "c", "d" }, all);
            Assert.Equal(result.Overflowed.Count > 0, result.HasOverflow);
        }

        [Theory(DisplayName = "U-TB-025 The placed items never exceed the available size")]
        [MemberData(nameof(Combinations))]
        public void Combinatorial_NeverExceedsBudget(ToolbarOverflowOption option, int priority, double available)
        {
            OverflowCandidate[] items = [C("a", 50), C("b", 50, option, priority), C("c", 50, ToolbarOverflowOption.Show), C("d", 50, ToolbarOverflowOption.Hide, priority)];

            OverflowResult result = OverflowCalculator.Calculate(items, available, gap: 4, overflowButtonSize: 32);

            // When every item overflows the button must still be present (U-TB-026), even in a container too small
            // to hold it, so the budget only binds once at least one item is visible.
            if (result.Visible.Count > 0)
            {
                double used = result.Visible.Sum(v => v.Size) + (result.Visible.Count - 1) * 4 + (result.HasOverflow ? 32 + 4 : 0);
                Assert.True(used <= Math.Max(0, available) + 0.0001, $"Used {used} exceeds available {available}.");
            }
            else
            {
                Assert.True(result.HasOverflow, "With no visible item the overflow button must be present.");
            }
        }
    }
}
