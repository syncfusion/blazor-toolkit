using Bunit;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Tests.Navigations.Toolbar
{
    /// <summary>
    /// Shared helpers for the SfToolbar bUnit tests. Test names carry the TEST-MATRIX id (U-TB-xxx, I-TB-xxx).
    /// </summary>
    public abstract class ToolbarTestBase : BunitTestContext
    {
        protected const string Root = "[data-testid='toolbar-root']";

        /// <summary>Builds a child component fragment from parameter name and value pairs.</summary>
        protected static RenderFragment Comp<TComponent>(params (string Name, object? Value)[] parameters)
            where TComponent : IComponent
        {
            return Comp<TComponent>(null, parameters);
        }

        /// <summary>Builds a child component fragment with child content.</summary>
        protected static RenderFragment Comp<TComponent>(RenderFragment? children, params (string Name, object? Value)[] parameters)
            where TComponent : IComponent
        {
            return builder =>
            {
                int seq = 0;
                builder.OpenComponent<TComponent>(seq++);
                foreach ((string name, object? value) in parameters)
                {
                    builder.AddAttribute(seq++, name, value);
                }
                if (children is not null)
                {
                    builder.AddAttribute(seq++, "ChildContent", children);
                    if (typeof(TComponent) == typeof(ToolbarItem))
                    {
                        builder.AddAttribute(seq++, "Items", children);
                    }
                }
                builder.CloseComponent();
            };
        }

        protected static RenderFragment Compose(params RenderFragment[] fragments)
        {
            return builder =>
            {
                for (int i = 0; i < fragments.Length; i++)
                {
                    builder.AddContent(i, fragments[i]);
                }
            };
        }

        protected static RenderFragment Item(string text, params (string Name, object? Value)[] extra)
        {
            (string, object?)[] all = new (string, object?)[extra.Length + 1];
            all[0] = ("Text", text);
            Array.Copy(extra, 0, all, 1, extra.Length);
            return Comp<ToolbarItem>(all);
        }

        /// <summary>Renders an SfToolbar with the given declarative content and optional typed parameters.</summary>
        protected IRenderedComponent<SfToolbar> RenderToolbar(
            RenderFragment? content = null,
            Action<ComponentParameterCollectionBuilder<SfToolbar>>? configure = null)
        {
            return RenderComponent<SfToolbar>(p =>
            {
                if (content is not null)
                {
                    _ = p.AddChildContent(content);
                }
                configure?.Invoke(p);
            });
        }
    }
}
