using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    public partial class SfChart
    {
        // Razor discovers public component tags only. Render internal components directly
        // so their lifecycle and cascading parameters run without exposing new public API.
        private static RenderFragment RenderInternalComponent<
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>()
            where TComponent : IComponent
        {
            return static builder =>
            {
                builder.OpenComponent<TComponent>(0);
                builder.CloseComponent();
            };
        }
    }
}
