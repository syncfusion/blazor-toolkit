using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Represents a single color stop that defines a color transition for the series gradient fill.
    /// A color stop determines how the current color blends at its position in the gradient.
    /// </summary>
    /// <remarks>
    /// A stop is identified by its <see cref="Offset"/> (0–100). Use <see cref="Color"/> to set the color,
    /// <see cref="Opacity"/> to control transparency, and optionally <see cref="Lighten"/> and
    /// <see cref="Brighten"/> to fine-tune the resulting tone.
    ///
    /// Declare multiple stops within <see cref="ChartGradientColorStops"/> to produce smooth transitions across the gradient span.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// @using Syncfusion.Blazor.Toolkit.Charts
    ///
    /// <SfChart>
    ///     <ChartSeriesCollection>
    ///         <ChartSeries Type="ChartSeriesType.Area"
    ///                      XName="Category"
    ///                      YName="Value">
    ///             <ChartRadialGradient Cx="0.5" Cy="0.5" Fx="0.5" Fy="0.5" R="0.6">
    ///                 <ChartGradientColorStops>
    ///                     <ChartGradientColorStop Offset="0"   Color="#FFE082" Opacity="1" />
    ///                     <ChartGradientColorStop Offset="100" Color="#FFCA28" Opacity="1" />
    ///                 </ChartGradientColorStops>
    ///             </ChartRadialGradient>
    ///         </ChartSeries>
    ///     </ChartSeriesCollection>
    /// </SfChart>
    /// ]]></code>
    /// </example>
    public class ChartGradientColorStop : ChartSubComponent
    {
        #region Fields

        private int _offset;
        private string _color = string.Empty;
        private double _opacity = Constants.DefaultOpacity;
        private double _brighten = Constants.DefaultBrighten;
        private double _lighten;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the position of the gradient stop as a percentage value.
        /// </summary>
        /// <value>
        /// An <see cref="int"/> ranging from <c>0</c> to <c>100</c>, representing the position of the color stop. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// Defines the stop location along the gradient axis; <c>0</c> represents the start and <c>100</c> represents the end.
        /// </remarks>
        [Parameter]
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the color of the gradient stop.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> accepting CSS color formats such as HEX, RGB/RGBA, HSL/HSLA, or named colors. The default value is <c>string.Empty</c>.
        /// </value>
        /// <remarks>
        /// When empty, theme defaults may be applied by the chart.
        /// </remarks>
        [Parameter]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the opacity of the gradient stop.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> in the range <c>0</c> (transparent) to <c>1</c> (opaque). The default value is <c>1</c>.
        /// </value>
        /// <remarks>
        /// Controls the transparency at this stop position.
        /// </remarks>
        [Parameter]
        public double Opacity { get; set; } = Constants.DefaultOpacity;

        /// <summary>
        /// Gets or sets the brightness adjustment for the color at the gradient stop.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> factor where values &gt; 1 brighten and values &lt; 1 darken. The default value is <c>1</c>.
        /// </value>
        /// <remarks>
        /// Applies a brightness multiplier to the base color before rendering the stop.
        /// </remarks>
        [Parameter]
        public double Brighten { get; set; } = Constants.DefaultBrighten;

        /// <summary>
        /// Gets or sets the lightness adjustment for the color at the gradient stop.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> where positive values lighten and negative values darken the color. The default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// Adjusts the perceived lightness of the color at the stop to fine-tune transitions.
        /// </remarks>
        [Parameter]
        public double Lighten { get; set; }

        /// <summary>
        /// Gets the parent gradient color stops container.
        /// </summary>
        /// <value>
        /// The cascading <see cref="ChartGradientColorStops"/> instance that owns this stop, if any.
        /// </value>
        [CascadingParameter]
        internal ChartGradientColorStops? Parent { get; set; }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component, resolves its parent, and registers this stop in the parent collection.
        /// </summary>      
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            ResolveParentFromTracker();
            Parent?.GradientColorStops.Add(this);
            UpdateParentGradientRegistration();
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter changes and notifies the renderer if a color-stop input has changed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            UpdateParentGradientRegistration();
            NotifyGradientValueChangedIfNeeded();
        }

        /// <summary>
        /// Releases references to parent components to allow garbage collection.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            Parent = null;
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Resolve the parent from the <c>Tracker</c> chain if available.
        /// </summary>
        private void ResolveParentFromTracker()
        {
            if (Tracker is ChartGradientColorStops chartGradientColorStops)
            {
                Parent = chartGradientColorStops;
            }
        }

        /// <summary>
        /// Notify parent gradient (linear or radial) that color stops have been updated.
        /// </summary>
        private void UpdateParentGradientRegistration()
        {
            if (Parent is null)
            {
                return;
            }

            if (Parent.LinearGradient is not null)
            {
                Parent.LinearGradient.UpdateGradientProperties(nameof(Parent.GradientColorStops), Parent.GradientColorStops);
            }
            else
            {
                Parent.RadialGradient?.UpdateGradientProperties(nameof(Parent.GradientColorStops), Parent.GradientColorStops);
            }
        }

        /// <summary>
        /// Detects changes to stop parameters and triggers a gradient value update in the renderer.
        /// </summary>
        private void NotifyGradientValueChangedIfNeeded()
        {
            if (_color != Color || _offset != Offset || _opacity != Opacity || _lighten != Lighten || _brighten != Brighten)
            {
                (Parent?.LinearGradient?.Renderer ?? Parent?.RadialGradient?.Renderer)?.GradientValueChanged();

                _color = Color;
                _offset = Offset;
                _opacity = Opacity;
                _lighten = Lighten;
                _brighten = Brighten;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Returns the final color for this stop after applying lightness and brightness adjustments.
        /// </summary>
        /// <remarks>
        /// Values are applied in the following order: <see cref="Lighten"/> then <see cref="Brighten"/>.
        /// If adjustments are default (0 for <see cref="Lighten"/> and 1 for <see cref="Brighten"/>), the original <see cref="Color"/> is returned.
        /// </remarks>
        /// <returns>
        /// A processed color string suitable for painting this gradient stop.
        /// </returns>
        internal string GetProcessedColor()
        {
            string finalColor = Color;

            if (!double.IsNaN(Lighten) && Lighten != 0)
            {
                finalColor = ChartHelper.LightenColor(finalColor, Lighten);
            }

            if (!double.IsNaN(Brighten) && Brighten != 1)
            {
                finalColor = ChartHelper.BrightenColor(finalColor, Brighten);
            }

            return finalColor;
        }

        #endregion
    }
}