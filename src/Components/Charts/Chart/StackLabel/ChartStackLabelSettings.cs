using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Charts.Internal;
using System.ComponentModel;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Configures the options to customize the stack labels in the chart.
    /// Stack labels display the total value for stacked series and provide customization options for appearance and positioning.
    /// </summary>
    /// <remarks>
    /// This sub-component wires to the parent <see cref="SfChart"/> and delegates render changes to a dedicated renderer.
    /// It uses change detection to minimize re-renders.
    /// </remarks>
    public class ChartStackLabelSettings : ChartSubComponent
    {
        #region Fields

        private ChartStackLabelRenderer? _renderer;

        private bool _prevVisible;
        private string _fill = Constants.Transparent;
        double _angle;
        double _stackLabelCornerRadiusX = 5;
        double _stackLabelCornerRadiusY = 5;
        string _format = null!;

        ChartStackLabelBorder _border = new();
        ChartStackLabelFont _font = new();
        ChartStackLabelMargin _margin = new();

        #endregion

        #region Properties

        /// <summary>
        /// A cascading reference to the parent chart.
        /// </summary>
        /// <value>The parent <see cref="SfChart"/> instance when present; otherwise, <c>null</c>.</value>
        [CascadingParameter]
        SfChart? chart { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether the stack labels are visible.
        /// </summary>
        /// <value>
        /// <c>true</c> to make the stack labels visible; otherwise, <c>false</c>.
        /// The default value is <c>false</c>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName=”x” YName=”y” Type=”ChartSeriesType.StackingColumn”>
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true">
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property controls the visibility of the stack labels, allowing them to be toggled on or off.
        /// </remarks>
        [Parameter]
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the background color of the stack labels.
        /// </summary>
        /// <value>
        /// Accepts valid CSS color strings, including hex and rgba values. The default value is <c>transparent</c>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName=”x” YName=”y” Type=”ChartSeriesType.StackingColumn”>
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true" Fill=”white”>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property sets the background color for the stack labels.
        /// </remarks>
        [Parameter]
        public string Fill
        {
            get => _fill;
            set
            {
                if (_fill != value)
                {
                    _fill = value;
                    Renderer?.StackLabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the format used to display the stack labels.
        /// </summary>
        /// <value>
        /// A string that specifies the format used to display the stack labels.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName="x" YName="y" Type="ChartSeriesType.StackingColumn">
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true" Format="N1">
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// Supports placeholders such as <c>{value}</c>, where <c>{value}</c> represents the total stack value.
        /// You can use standard numeric format strings such as "N0", "N1", "C0", "P0", etc., to customize the display format of the stack labels.
        /// </remarks>
        [Parameter]
        public string Format
        {
            get => _format;
            set
            {
                if (_format != value)
                {
                    _format = value;
                    Renderer?.StackLabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the angle for rotating the stack labels.
        /// </summary>
        /// <value>
        /// A double value representing the rotation angle, ranging from 0 to 360. The default value is <c>0</c>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName=”x” YName=”y” Type=”ChartSeriesType.StackingColumn”>
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true" Angle=”90”>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property allows rotating the stack labels by adjusting their angle.
        /// </remarks>
        [Parameter]
        public double Angle
        {
            get => _angle;
            set
            {
                if (_angle != value)
                {
                    _angle = value;
                    Renderer?.StackLabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the rounded corner radius along the X-axis for the stack label background.
        /// </summary>
        /// <value>
        /// A double value representing the corner radius along the X-axis. The default value is <c>5</c>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName=”x” YName=”y” Type=”ChartSeriesType.StackingColumn”>
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true" Rx=”10”>
        ///     </ChartStackLabelSettings>
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property sets the rounded corner radius along the X-axis for the stack label background.
        /// The border must be set for the rounded corners to be visible.
        /// </remarks>
        [Parameter]
        public double Rx
        {
            get => _stackLabelCornerRadiusX;
            set
            {
                if (_stackLabelCornerRadiusX != value)
                {
                    _stackLabelCornerRadiusX = value;
                    Renderer?.StackLabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the rounded corner radius along the Y-axis for the stack label background.
        /// </summary>
        /// <value>
        /// A double value representing the corner radius along the Y-axis. The default value is <c>5</c>.
        /// </value>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <SfChart>
        ///     <ChartSeries XName=”x” YName=”y” Type=”ChartSeriesType.StackingColumn”>
        ///     </ChartSeries>
        ///     <ChartStackLabelSettings Visible="true" Ry=”10”>
        ///     </ChartStackLabelSettings >
        /// </SfChart>
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// This property sets the rounded corner radius along the Y-axis for the stack label background.
        /// The border must be set for the rounded corners to be visible.
        /// </remarks>
        [Parameter]
        public double Ry
        {
            get => _stackLabelCornerRadiusY;
            set
            {
                if (_stackLabelCornerRadiusY != value)
                {
                    _stackLabelCornerRadiusY = value;
                    Renderer?.StackLabelValueChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the internal renderer that is responsible for drawing stack labels.
        /// </summary>
        /// <value>The renderer instance associated with the parent chart.</value>
        internal ChartStackLabelRenderer Renderer
        {
            get => _renderer ?? null!;
            set
            {
                if (_renderer != value)
                {
                    _renderer = value;
                    _renderer?.OnParentParameterSet();
                }
            }
        }

        /// <summary>
        /// Gets or sets the border configuration for the stack labels.
        /// </summary>
        /// <value>The <see cref="ChartStackLabelBorder"/> instance.</value>
        internal ChartStackLabelBorder Border
        {
            get => _border;
            set
            {
                if (_border != value)
                {
                    _border = value;
                    if (_border is not null && _border._isPropertyChanged)
                    {
                        _border._isPropertyChanged = false;
                        Renderer?.StackLabelValueChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the font configuration for the stack labels.
        /// </summary>
        /// <value>The <see cref="ChartStackLabelFont"/> instance.</value>
        internal ChartStackLabelFont Font
        {
            get => _font;
            set
            {
                if (_font != value)
                {
                    _font = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the margin configuration for the stack labels.
        /// </summary>
        /// <value>The <see cref="ChartStackLabelMargin"/> instance.</value>
        internal ChartStackLabelMargin Margin
        {
            get => _margin;
            set
            {
                if (_margin != value)
                {
                    _margin = value;
                }
            }
        }

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component, connects to the parent <see cref="SfChart"/>, and establishes the renderer.
        /// </summary>
        /// <remarks>
        /// Ensures the parent chart references this settings instance and that the renderer is ready to process changes.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (chart is not null)
            {
                chart._stackLabelSettings = this;
            }

            Renderer = chart?._stackLabelRenderer ?? null!;
        }

        /// <exclude />
        /// <summary>
        /// Applies parameter updates and keeps the parent chart's <see cref="SfChart.StackLabelSettings"/> reference synchronized.
        /// </summary>
        /// <remarks>
        /// This method does not trigger rendering directly; instead, property setters request redraws when values change.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (chart is null)
            {
                return;
            }

            if (_prevVisible != Visible)
            {
                _renderer?.ToggleVisibility();
            }

            chart._stackLabelSettings = this;
            _prevVisible = Visible;
        }

        /// <summary>
        /// Releases references and disposes nested subcomponents to avoid memory leaks.
        /// </summary>
        /// <remarks>
        /// Clears references to the parent chart, renderer, and child content, then forwards disposal to nested parts.
        /// </remarks>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override ValueTask DisposeAsyncCore()
        {
            chart = null;
            ChildContent = null!;
            _renderer = null;

            Margin?.ComponentDispose();
            Border?.ComponentDispose();
            Font?.ComponentDispose();
            return base.DisposeAsyncCore();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Updates the nested stack label property instances by key.
        /// </summary>
        /// <param name="key">The property key. Expected values are <c>Border</c>, <c>Font</c>, or <c>Margin</c>.</param>
        /// <param name="keyValue">The property instance.</param>
        internal void UpdateStackLabelProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Border):
                    Border = (ChartStackLabelBorder)keyValue;
                    break;

                case nameof(Font):
                    Font = (ChartStackLabelFont)keyValue;
                    break;

                case nameof(Margin):
                    Margin = (ChartStackLabelMargin)keyValue;
                    break;
            }
        }

        #endregion
    }
}
