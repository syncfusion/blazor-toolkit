using System;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Specifies the marker configuration of the chart.
    /// </summary>
    /// <remarks>
    /// Use this component to customize markers for data points within a series: shape, size, fill, opacity, image source,
    /// and additional nested configurations like border, offset, and data labels.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// <SfChart>
    ///     <ChartSeriesCollection>
    ///         <ChartSeries DataSource="@Data" XName="XValue" YName="YValue" Name="Series A">
    ///             <ChartMarker Visible="true"
    ///                          Shape="ChartShape.Circle"
    ///                          Height="8"
    ///                          Width="8"
    ///                          Fill="#3B82F6"
    ///                          Opacity="0.9">
    ///                 <ChartMarkerBorder Color="#1E40AF" Width="2"></ChartMarkerBorder>
    ///                 <ChartMarkerOffset X="2" Y="-2" />
    ///                 <ChartDataLabel Visible="true" />
    ///             </ChartMarker>
    ///         </ChartSeries>
    ///     </ChartSeriesCollection>
    /// </SfChart>
    /// ]]>
    /// </code>
    /// </example>
    public class ChartCommonMarker : ChartSubComponent, IChartElement
    {
        #region Fields


        internal Type _rendererType = null!;
        #endregion

        #region Properties

        /// <summary>
        /// Internal renderer key used by the infrastructure to associate with a renderer.
        /// </summary>
        /// <value>The renderer key string.</value>
        string IChartElement.RendererKey { get; set; } = null!;

        /// <summary>
        /// Gets or sets the internal marker renderer.
        /// </summary>
        /// <value>The current <see cref="ChartMarkerRenderer"/> for this component.</value>
        private ChartMarkerRenderer? _renderer;
        internal ChartMarkerRenderer Renderer
        {
            get => _renderer ?? null!;
            set
            {
                if (_renderer == value)
                {
                    return;
                }

                _renderer = value;
                _renderer?.OnParentParameterSet();
            }
        }

        /// <exclude />
        /// <summary>
        /// Gets or sets the concrete renderer type for this component.
        /// </summary>
        /// <value>A <see cref="Type"/> that represents the renderer.</value>
        /// <remarks>
        /// This is used by the rendering pipeline to instantiate and bind the marker renderer.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public Type RendererType
        {
            get => _rendererType;
            set => _rendererType = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the marker is visible for the series.
        /// </summary>
        /// <value>
        /// <c>true</c> to display markers; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When enabled, markers are rendered for each data point in the series.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Visible="true" />
        /// ]]>
        /// </code>
        /// </example>
        private bool _visible;
        [Parameter]
        public bool Visible
        {
            get => _visible;
            set
            {
                if (_visible == value)
                {
                    return;
                }

                _visible = value;
                Renderer?.ToggleVisibility();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the marker is filled for the series.
        /// </summary>
        /// <value>
        /// <c>true</c> to render filled markers; otherwise, <c>false</c>. The default value is <c>false</c>.
        /// </value>
        /// <remarks>
        /// When enabled, markers are filled with color; otherwise, they are outlined.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Visible="true" IsFilled="true" />
        /// ]]>
        /// </code>
        /// </example>
        private bool _isFilled;
        [Parameter]
        public bool IsFilled
        {
            get => _isFilled;
            set
            {
                if (_isFilled == value)
                {
                    return;
                }

                _isFilled = value;
            }
        }

        /// <summary>
        /// Gets or sets the shape of the marker.
        /// </summary>
        /// <value>
        /// A <see cref="ChartShape"/> value. The default is <see cref="ChartShape.Auto"/>.
        /// </value>
        /// <remarks>
        /// Choose a predefined shape or <see cref="ChartShape.Image"/> to supply a custom image.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Shape="ChartShape.Triangle" />
        /// ]]>
        /// </code>
        /// </example>
        private ChartShape _shape = ChartShape.Auto;
        [Parameter]
        public ChartShape Shape
        {
            get => _shape;
            set
            {
                if (_shape == value)
                {
                    return;
                }

                _shape = value;
                Renderer?.UpdateDirection();
            }
        }

        /// <summary>
        /// Gets or sets the URL path for the image.
        /// </summary>
        /// <value>A <see cref="System.Uri"/> for the image used when <see cref="Shape"/> is <see cref="ChartShape.Image"/>.</value>
        /// <remarks>
        /// Use a fully qualified URL or application-relative path to render custom image markers.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Shape="ChartShape.Image"
        ///              ImageUrl="https://ej2.syncfusion.com/demos/src/chart/images/cloud.png"
        ///              Height="30" Width="30" />
        /// ]]>
        /// </code>
        /// </example>
        private Uri? _imageUrl;
        [Parameter]
        public Uri? ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (_imageUrl == value)
                {
                    return;
                }

                _imageUrl = value;
                Renderer?.UpdateDirection();
            }
        }

        /// <summary>
        /// Gets or sets the height of the marker.
        /// </summary>
        /// <value>A <see cref="double"/> representing the marker height. Default is <b>5</b>.</value>
        /// <remarks>Adjusts the vertical size of the marker.</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Shape="ChartShape.Rectangle" Height="30" />
        /// ]]>
        /// </code>
        /// </example>
        private double _height = Constants.MarkerSize;
        [Parameter]
        public double Height
        {
            get => _height;
            set
            {
                if (Math.Abs(_height - value) < double.Epsilon)
                {
                    return;
                }

                _height = value;
                Renderer?.UpdateDirection();
            }
        }

        /// <summary>
        /// Gets or sets the width of the marker.
        /// </summary>
        /// <value>A <see cref="double"/> representing the marker width. Default is <b>5</b>.</value>
        /// <remarks>Adjusts the horizontal size of the marker.</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Shape="ChartShape.Rectangle" Width="30" />
        /// ]]>
        /// </code>
        /// </example>
        private double _widthProperty = Constants.MarkerSize;
        [Parameter]
        public double Width
        {
            get => _widthProperty;
            set
            {
                if (Math.Abs(_widthProperty - value) < double.Epsilon)
                {
                    return;
                }

                _widthProperty = value;
                Renderer?.UpdateDirection();
            }
        }

        /// <summary>
        /// Gets or sets the options to customize the border of the marker.
        /// </summary>
        /// <value>
        /// A <see cref="ChartMarkerBorder"/> instance specifying color and width for the marker border.
        /// </value>
        /// <remarks>
        /// Use to highlight markers or improve contrast against the chart.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Visible="true">
        ///     <ChartMarkerBorder Color="red" Width="2"></ChartMarkerBorder>
        /// </ChartMarker>
        /// ]]>
        /// </code>
        /// </example>
        private ChartMarkerBorder? _border;
        [Parameter]
        public ChartMarkerBorder Border
        {
          
            get => _border ??= new ChartMarkerBorder();
            set
            {
                _border = value;
                if (_border is not null && _border._isPropertyChanged)
                {
                    Renderer?.UpdateMarkerBorderWidth();
                    Renderer?.UpdateCustomization("Color");
                    _border._isPropertyChanged = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the options to customize the offset of the marker.
        /// </summary>
        /// <value>
        /// A <see cref="ChartMarkerOffset"/> instance specifying X and Y offset values.
        /// </value>
        /// <remarks>
        /// Use to fine-tune marker placement relative to its data point.
        /// </remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Visible="true">
        ///     <ChartMarkerOffset X="20" Y="20" />
        /// </ChartMarker>
        /// ]]>
        /// </code>
        /// </example>
        private ChartMarkerOffset? _offset;
        [Parameter]
        public ChartMarkerOffset Offset
        {
          
            get => _offset ??= new ChartMarkerOffset();
            set
            {
                _offset = value;
                if (_offset is not null && _offset._isPropertyChanged)
                {
                    Renderer?.UpdateDirection();
                    _offset._isPropertyChanged = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the fill color of the marker.
        /// </summary>
        /// <value>A string representing the fill color.</value>
        /// <remarks>Accepts CSS color names, HEX, RGB, or HSL values.</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Visible="true" Fill="red" />
        /// ]]>
        /// </code>
        /// </example>
        private string _fill = string.Empty;
        [Parameter]
        public string Fill
        {
            get => _fill;
            set
            {
                if (_fill == value)
                {
                    return;
                }

                _fill = value;
                Renderer?.UpdateCustomization("Fill");
            }
        }

        /// <summary>
        /// Gets or sets the opacity of the marker shape.
        /// </summary>
        /// <value>
        /// A <see cref="double"/> from 0 (transparent) to 1 (opaque). Default is <b>1</b>.
        /// </value>
        /// <remarks>Controls the transparency of the rendered marker.</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Visible="true" Opacity="0.5" />
        /// ]]>
        /// </code>
        /// </example>
        private double _opacity = Constants.DefaultOpacity;
        [Parameter]
        public double Opacity
        {
            get => _opacity;
            set
            {
                if (Math.Abs(_opacity - value) < double.Epsilon)
                {
                    return;
                }

                _opacity = value;
                Renderer?.UpdateCustomization("Opacity");
            }
        }

        /// <summary>
        /// Gets or sets the options to customize the data label for the series.
        /// </summary>
        /// <value>A <see cref="ChartDataLabel"/> instance for data label configuration.</value>
        /// <remarks>Enables annotations of data points directly on the chart.</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Visible="true">
        ///     <ChartDataLabel Visible="true" />
        /// </ChartMarker>
        /// ]]>
        /// </code>
        /// </example>
        private ChartDataLabel? _dataLabel;
        [Parameter]
        public ChartDataLabel DataLabel
        {
            get => _dataLabel ??= new ChartDataLabel();
            set
            {
                if (_dataLabel == value)
                {
                    return;
                }

                _dataLabel = value;
            }
        }

        /// <summary> 
        /// Gets or sets a value that allows or disallows marker highlight for the chart tooltip.
        /// </summary>
        /// <value>
        /// <c>true</c> to permit marker highlight on tooltip; otherwise <c>false</c>. Default is <b>true</b>.
        /// </value>
        /// <remarks>Controls whether the marker receives highlight styling when a tooltip is shown.</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// <ChartMarker Visible="true" AllowHighlight="false" />
        /// ]]>
        /// </code>
        /// </example>
        [Parameter]
        public bool AllowHighlight { get; set; } = true;

        #endregion

        #region Lifecycle Methods

        /// <exclude />
        /// <summary>
        /// Initializes the component and wires the renderer type.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        protected override void OnInitialized()
        {
            base.OnInitialized();
            _rendererType = typeof(ChartMarkerRenderer);
        }

        /// <summary>
        /// Releases nested sub-components and associated resources.
        /// </summary>
        protected override ValueTask DisposeAsyncCore()
        {
            ComponentDispose();
            return base.DisposeAsyncCore();
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Releases nested sub-components and associated resources.
        /// </summary>
        internal void ComponentDispose()
        {
            DataLabel?.ComponentDispose();
            Border?.ComponentDispose();
            Offset?.ComponentDispose();
        }
        /// <summary>
        /// Updates the corresponding nested marker sub-component property using a key.
        /// </summary>
        /// <param name="key">The property key to update (e.g., <c>Border</c>, <c>Offset</c>, <c>DataLabel</c>).</param>
        /// <param name="keyValue">The sub-component instance to set.</param>
        internal void UpdateMarkerProperties(string key, object keyValue)
        {
            switch (key)
            {
                case nameof(Border):
                    Border = (ChartMarkerBorder)keyValue;
                    break;
                case nameof(Offset):
                    Offset = (ChartMarkerOffset)keyValue;
                    break;
                case nameof(DataLabel):
                    DataLabel = (ChartDataLabel)keyValue;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Copies values from another <see cref="ChartCommonMarker"/> into this instance.
        /// </summary>
        /// <param name="marker">The source marker whose values will be copied.</param>
        internal void SetMarkerValues(ChartCommonMarker marker)
        {
            Visible = marker.Visible;
            Shape = marker.Shape;
            ImageUrl = marker.ImageUrl;
            Height = marker.Height;
            Width = marker.Width;

            if (marker.Border != null)
            {
                Border = new ChartMarkerBorder();
                Border.SetBorderValues(marker.Border.Color, marker.Border.Width);
            }

            if (marker.Offset != null)
            {
                Offset = new ChartMarkerOffset();
                Offset.SetOffsetValues(marker.Offset.X, marker.Offset.Y);
            }

            if (marker.DataLabel != null)
            {
                DataLabel = marker.DataLabel;
            }

            // Prefer existing Fill if already set; otherwise, use source marker's Fill.
            Fill = string.IsNullOrEmpty(Fill) ? marker.Fill : Fill;

            // Copy from source marker (fixes previous self-assignment bug).
            Opacity = marker.Opacity;

            IsFilled = marker.IsFilled;
        }

        #endregion
    }
}
