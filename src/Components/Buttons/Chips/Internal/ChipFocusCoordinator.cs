namespace Syncfusion.Blazor.Toolkit.Internal
{
    /// <summary>Roving tabindex and removal focus target. Does not touch document.activeElement.</summary>
    internal static class ChipFocusCoordinator
    {
        internal static int Move(int current, int count, int delta, Func<int, bool> canFocus)
        {
            if (count == 0)
            {
                return -1;
            }

            int index = current < 0 ? (delta > 0 ? -1 : count) : current;
            for (int step = 0; step < count; step++)
            {
                index = (index + delta + count) % count;
                if (canFocus(index))
                {
                    return index;
                }
            }

            return -1;
        }

        internal static int Edge(int count, bool end, Func<int, bool> canFocus)
        {
            if (end)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    if (canFocus(i))
                    {
                        return i;
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    if (canFocus(i))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>Next focusable chip, else previous, else -1 for the set root.</summary>
        internal static int Restore(int removed, int count, Func<int, bool> canFocus)
        {
            for (int i = removed; i < count; i++)
            {
                if (canFocus(i))
                {
                    return i;
                }
            }

            for (int i = removed - 1; i >= 0; i--)
            {
                if (canFocus(i))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
