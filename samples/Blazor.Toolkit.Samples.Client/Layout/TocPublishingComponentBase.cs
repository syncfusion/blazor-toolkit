using Microsoft.AspNetCore.Components;

namespace Blazor.Toolkit.Samples.Client.Layout;

/// <summary>
/// Base class for sample pages that publish their Table of Contents to the
/// right-sidebar <see cref="RightToc"/> via the cascading
/// <see cref="TocItemsHolder"/>.
/// </summary>
/// <remarks>
/// Each derived page overrides <see cref="TocItems"/> to declare the section
/// list that should appear in the sidebar. The base class:
/// <list type="bullet">
///   <item>Subscribes to the cascading <see cref="TocItemsHolder"/>.</item>
///   <item>Publishes the derived <see cref="TocItems"/> on every
///     <see cref="ComponentBase.OnParametersSet"/>.</item>
///   <item>Clears the holder on <see cref="Dispose"/> so stale entries never
///     linger after the user navigates away.</item>
/// </list>
/// Pages no longer need to declare their own <c>[CascadingParameter]</c>,
/// <c>OnParametersSet</c>, or <c>Dispose</c> members for the TOC pattern.
/// </remarks>
public abstract class TocPublishingComponentBase : ComponentBase, IDisposable
{
    /// <summary>
    /// Cascading holder shared with the right-sidebar TOC. Resolved by
    /// <see cref="MainLayout"/> on every render and re-published by the
    /// derived class.
    /// </summary>
    [CascadingParameter]
    private TocItemsHolder? TocHolder { get; set; }

    /// <summary>
    /// Section entries to publish to the right sidebar when this page
    /// renders. The reference identity of the returned list is used by
    /// <see cref="Dispose"/> to clear the holder on navigation away.
    /// </summary>
    protected abstract IReadOnlyList<RightToc.TocItem> TocItems { get; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (TocHolder is not null)
        {
            TocHolder.Items = TocItems;
        }

        base.OnParametersSet();
    }

    /// <summary>
    /// Clears the cascading holder so the right sidebar does not linger
    /// with the entries of a page the user has just navigated away from.
    /// </summary>
    public virtual void Dispose()
    {
        if (TocHolder is not null && ReferenceEquals(TocHolder.Items, TocItems))
        {
            TocHolder.Items = null;
        }

        GC.SuppressFinalize(this);
    }
}
