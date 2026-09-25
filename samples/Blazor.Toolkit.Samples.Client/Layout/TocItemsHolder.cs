namespace Blazor.Toolkit.Samples.Client.Layout;

/// <summary>
/// Mutable holder shared between pages and the right-sidebar slot in
/// <see cref="MainLayout"/>. A page writes its section list into the
/// holder during
/// <see cref="Microsoft.AspNetCore.Components.ComponentBase.OnParametersSet"/>
/// and clears it on disposal; the right sidebar (<see cref="RightToc"/>)
/// subscribes to <see cref="Changed"/> and re-renders immediately on every
/// update so stale entries never linger after navigation.
/// </summary>
public sealed class TocItemsHolder
{
    private IReadOnlyList<RightToc.TocItem>? items;

    /// <summary>
    /// Section entries for the currently-rendered page, or
    /// <see langword="null"/> when no page has published a list yet.
    /// </summary>
    public IReadOnlyList<RightToc.TocItem>? Items
    {
        get => items;
        set
        {
            items = value;
            Changed?.Invoke();
        }
    }

    /// <summary>
    /// Raised after <see cref="Items"/> is assigned. Subscribers should
    /// re-render and re-attach the scroll-spy observer with the new ids.
    /// </summary>
    public event Action? Changed;
}
