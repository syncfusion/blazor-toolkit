using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Popups
{
    /// <summary>
    /// A class used for configuring the animation properties in the <see cref="SfDialog"/> component.
    /// </summary>
    /// <remarks>
    /// The <see cref="DialogAnimationSettings"/> class allows you to customize the animation behavior when opening and closing dialogs.
    /// You can configure properties such as delay, duration, and animation effects to create smooth transitions and enhance user experience.
    /// The animation settings are applied uniformly to both the show and hide operations of the dialog.
    /// </remarks>
    /// <example>
    /// In the following example, change the animation effect and delay time while opening the dialog.
    /// <code><![CDATA[
    /// @using Syncfusion.Blazor.Toolkit.Popups
    /// <SfDialog Width="500px" @bind-Visible="Visibility">
    ///   <DialogTemplates>
    ///     <Content>
    ///         <p>
    ///            Dialog content
    ///           </p>
    ///       </Content>
    ///   </DialogTemplates>
    ///     <DialogAnimationSettings Delay="400" Effect="DialogEffect.SlideTop">
    ///    </DialogAnimationSettings>
    ///   </SfDialog>
    ///  @code {
    ///   private bool Visibility { get; set; } = true;
    ///  }
    /// ]]></code>
    /// </example>
    public class DialogAnimationSettings : SfBaseComponent
    {
        private const string DATA_ID = "dataId";
        private const string ANIMATION_SETTINGS = "animationSettings";
        private const string UPDATE_ANIMATION = "updateAnimation";

        private double _delay;
        private double _duration;
        private DialogEffect _effect;

        [CascadingParameter]
        internal SfDialog? Parent { get; set; }

        /// <summary>
        /// Gets or sets the delay in milliseconds before the animation begins.
        /// </summary>
        /// <value>
        /// A numeric value representing the delay time in milliseconds before the animation starts. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// The delay setting affects both opening and closing animations of the dialog. A higher delay value will create a longer pause
        /// before the animation begins. This can be useful for creating staggered effects or synchronizing animations with other UI elements.
        /// </remarks>
        [Parameter]
        public double Delay { get; set; }

        /// <summary>
        /// Gets or sets the duration in milliseconds that the animation takes to open or close the <see cref="SfDialog"/>.
        /// </summary>
        /// <value>
        /// A numeric value representing the time in milliseconds for the animation to complete. The default value is <c>400</c>.
        /// </value>
        /// <remarks>
        /// The duration controls how fast or slow the animation plays. A shorter duration creates faster animations, while a longer 
        /// duration creates slower, more gradual transitions. The same duration is used for both opening and closing animations.
        /// Setting this to <c>0</c> will effectively disable the animation transition.
        /// </remarks>
        [Parameter]
        public double Duration { get; set; } = 400;

        /// <summary>
        /// Gets or sets the animation effect that should be used when opening and closing the <see cref="SfDialog"/>.
        /// </summary>
        /// <value>
        /// A <see cref="DialogEffect"/> enumeration value that specifies the animation effect. The default value is <see cref="DialogEffect.Fade"/>.
        /// The available animation effects are:
        /// <list type="bullet">
        /// <item><description><see cref="DialogEffect.Fade"/> - Gradual opacity transition</description></item>
        /// <item><description><see cref="DialogEffect.FadeZoom"/> - Combined fade and zoom effect</description></item>
        /// <item><description><see cref="DialogEffect.FlipLeftDown"/> - Flip animation from left to down</description></item>
        /// <item><description><see cref="DialogEffect.FlipLeftUp"/> - Flip animation from left to up</description></item>
        /// <item><description><see cref="DialogEffect.FlipRightDown"/> - Flip animation from right to down</description></item>
        /// <item><description><see cref="DialogEffect.FlipRightUp"/> - Flip animation from right to up</description></item>
        /// <item><description><see cref="DialogEffect.FlipXDown"/> - Horizontal flip with downward motion</description></item>
        /// <item><description><see cref="DialogEffect.FlipXUp"/> - Horizontal flip with upward motion</description></item>
        /// <item><description><see cref="DialogEffect.FlipYLeft"/> - Vertical flip with leftward motion</description></item>
        /// <item><description><see cref="DialogEffect.FlipYRight"/> - Vertical flip with rightward motion</description></item>
        /// <item><description><see cref="DialogEffect.SlideBottom"/> - Slide in/out from bottom</description></item>
        /// <item><description><see cref="DialogEffect.SlideLeft"/> - Slide in/out from left</description></item>
        /// <item><description><see cref="DialogEffect.SlideRight"/> - Slide in/out from right</description></item>
        /// <item><description><see cref="DialogEffect.SlideTop"/> - Slide in/out from top</description></item>
        /// <item><description><see cref="DialogEffect.Zoom"/> - Scale-based zoom effect</description></item>
        /// <item><description><see cref="DialogEffect.None"/> - No animation effect</description></item>
        /// </list>
        /// </value>        
        /// <remarks>
        /// The animation effect determines the visual transition style when the dialog appears or disappears. Each effect has both an 
        /// opening and closing variation - for example, <see cref="DialogEffect.Fade"/> will use 'FadeIn' when opening and 'FadeOut' when closing.
        /// The slide effects will animate from/to the specified direction. Setting this to <see cref="DialogEffect.None"/> will disable animations entirely.
        /// </remarks>
        [Parameter]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DialogEffect Effect { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start, called once the component has been initialized.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is part of the Blazor component lifecycle and is called after the component has been constructed and its
        /// initial parameters have been set. It registers this animation settings instance with its parent <see cref="SfDialog"/>
        /// component to ensure the animation configuration is properly applied.
        /// </remarks>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            Parent?.UpdateChildProperties(ANIMATION_SETTINGS, this);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent and parameter values have changed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method is called whenever the component's parameters change after initialization. It detects changes in the animation
        /// properties (<see cref="Delay"/>, <see cref="Duration"/>, and <see cref="Effect"/>) and updates the dialog's animation
        /// configuration accordingly. If the parent dialog is already rendered, it will immediately apply the new animation settings
        /// through JavaScript interop to ensure the changes take effect without requiring a full re-render.
        /// </remarks>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            if (Delay != _delay || Duration != _duration || Effect != _effect)
            {
                _delay = Delay;
                _duration = Duration;
                _effect = Effect;
                if (Parent is not null && Parent.IsRendered)
                {
                    await InvokeVoidAsync(Parent._dialogJsModule, Parent._dialogJsInProcessModule, UPDATE_ANIMATION, new Dictionary<string, object>
                    {
                        { DATA_ID, Parent._dataId },
                        { ANIMATION_SETTINGS, Parent.GetAnimationSettings() }
                    }).ConfigureAwait(true);
                }
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="DialogAnimationSettings"/> and optionally releases the managed resources.
        /// </summary>
        /// <remarks>
        /// This method is called by the public <see cref="DisposeAsyncCore()"/> method and the finalizer.
        /// this method releases all resources held by managed objects that this <see cref="DialogAnimationSettings"/> references.
        /// It clears the reference to the parent dialog component to prevent memory leaks.
        /// </remarks>
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            return base.DisposeAsyncCore();
        }
    }
}