namespace Syncfusion.Blazor.Toolkit
{
    /// <exclude />
    /// <summary>
    /// One item as the overflow algorithm sees it: an identifier, a measured size along the main axis, and its overflow preferences.
    /// </summary>
    /// <param name="Id">The identifier of the item.</param>
    /// <param name="Size">The measured size along the main axis, in CSS pixels.</param>
    /// <param name="Option">How strongly the item prefers to stay in the main bar.</param>
    /// <param name="Priority">The secondary sort key. A lower value stays visible longer.</param>
    internal readonly record struct OverflowCandidate(string Id, double Size, ToolbarOverflowOption Option, int Priority);

    /// <exclude />
    /// <summary>
    /// The outcome of one overflow calculation.
    /// </summary>
    internal sealed class OverflowResult
    {
        /// <exclude />
        internal OverflowResult(IReadOnlyList<OverflowCandidate> visible, IReadOnlyList<OverflowCandidate> overflowed)
        {
            Visible = visible;
            Overflowed = overflowed;
        }

        /// <exclude />
        /// <summary>
        /// Gets the items that stay in the main bar, in declaration order.
        /// </summary>
        internal IReadOnlyList<OverflowCandidate> Visible { get; }

        /// <exclude />
        /// <summary>
        /// Gets the items that move to the overflow popup, in sort order.
        /// </summary>
        internal IReadOnlyList<OverflowCandidate> Overflowed { get; }

        /// <exclude />
        /// <summary>
        /// Gets whether at least one item overflowed, which is when the overflow button is needed.
        /// </summary>
        internal bool HasOverflow => Overflowed.Count > 0;
    }

    /// <exclude />
    /// <summary>
    /// The overflow algorithm of the Toolbar (Spec section 6). It is pure: it reads no component state and renders nothing.
    /// </summary>
    /// <remarks>
    /// <list type="number">
    /// <item><description>Stable-sort the items by overflow option (Show, None, Hide), then Priority ascending, then declaration order.</description></item>
    /// <item><description>Place items greedily while the remaining space is at least the item size plus the gap.</description></item>
    /// <item><description>Mark every remaining item as overflowed. Placement stops at the first item that does not fit.</description></item>
    /// </list>
    /// The space for the overflow button is reserved only when something overflows. Visible items are returned in declaration order.
    /// </remarks>
    internal static class OverflowCalculator
    {
        /// <exclude />
        /// <summary>
        /// Sorts the candidates by overflow option, then Priority, then declaration order. The input is not changed.
        /// </summary>
        /// <param name="items">The candidates in declaration order.</param>
        /// <returns>A new array in placement order.</returns>
        internal static OverflowCandidate[] Sort(IReadOnlyList<OverflowCandidate> items)
        {
            ArgumentNullException.ThrowIfNull(items);
            return
            [
                .. items
                    .Select(static (item, index) => (item, index))
                    .OrderBy(static x => OptionRank(x.item.Option))
                    .ThenBy(static x => x.item.Priority)
                    .ThenBy(static x => x.index)
                    .Select(static x => x.item)
            ];
        }

        /// <exclude />
        /// <summary>
        /// Decides which items stay in the bar and which overflow.
        /// </summary>
        /// <param name="items">The candidates in declaration order.</param>
        /// <param name="available">The available size along the main axis. A negative value is treated as zero.</param>
        /// <param name="gap">The space between two items.</param>
        /// <param name="overflowButtonSize">The size of the overflow button, reserved only when something overflows.</param>
        /// <returns>The visible and overflowed items.</returns>
        internal static OverflowResult Calculate(IReadOnlyList<OverflowCandidate> items, double available, double gap, double overflowButtonSize)
        {
            ArgumentNullException.ThrowIfNull(items);
            if (double.IsNaN(available) || double.IsInfinity(available))
            {
                available = 0;
            }
            double budget = Math.Max(0, available);
            double safeGap = Math.Max(0, gap);
            OverflowCandidate[] sorted = Sort(items);

            // First pass: does everything fit without an overflow button?
            int fitWithoutButton = CountThatFit(sorted, budget, safeGap);
            int fitCount = fitWithoutButton;
            if (fitWithoutButton < sorted.Length)
            {
                // Something overflows, so the button needs room too.
                fitCount = CountThatFit(sorted, budget - ButtonSpace(overflowButtonSize, safeGap), safeGap);
            }

            HashSet<OverflowCandidate> placed = [.. sorted.Take(fitCount)];
            List<OverflowCandidate> visible = [.. items.Where(placed.Contains)];
            List<OverflowCandidate> overflowed = [.. sorted.Skip(fitCount)];
            return new OverflowResult(visible, overflowed);
        }

        private static int CountThatFit(OverflowCandidate[] sorted, double budget, double gap)
        {
            double remaining = budget;
            int count = 0;
            foreach (OverflowCandidate item in sorted)
            {
                double needed = item.Size + (count > 0 ? gap : 0);
                if (remaining < needed)
                {
                    break;
                }
                remaining -= needed;
                count++;
            }
            return count;
        }

        private static double ButtonSpace(double buttonSize, double gap)
        {
            return Math.Max(0, buttonSize) + gap;
        }

        private static int OptionRank(ToolbarOverflowOption option)
        {
            return option switch
            {
                ToolbarOverflowOption.Show => 0,
                ToolbarOverflowOption.None => 1,
                ToolbarOverflowOption.Hide => 2,
                _ => 1
            };
        }
    }
}
