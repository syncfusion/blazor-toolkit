# Syncfusion Blazor Charts - Events Reference

Comprehensive reference guide for all chart events organized by category.

## Table of Contents

- [Chart Lifecycle Events](#chart-lifecycle-events)
   - [OnChartLoaded (Loaded)](#onchartloaded-loaded)
   - [SizeChanged](#sizechanged)
- [Mouse and Touch Events](#mouse-and-touch-events)
   - [ChartMouseMove](#chartmousemove)
   - [ChartMouseClick](#chartmouseclick)
   - [ChartMouseDown](#chartmousedown)
   - [ChartMouseUp](#chartmouseup)
   - [ChartMouseLeave](#chartmouseleave)
   - [PointClick (OnPointClick)](#pointclick-onpointclick)
   - [PointMove](#pointmove)
- [Rendering Events](#rendering-events)
   - [OnPointRender](#onpointrender)
   - [OnDataLabelRender](#ondatalabelrender)
   - [OnAxisLabelRender](#onaxislabelrender)
   - [OnLegendItemRender](#onlegenditemrender)
   - [OnSeriesRender](#onseriesrender)
- [Interactive Events](#interactive-events)
   - [OnSelectionChanged](#onselectionchanged)
   - [OnLegendClick](#onlegendclick)
   - [OnAxisLabelClick](#onaxislabelclick)
   - [OnDataEdit](#ondataedit)
   - [OnDataEditCompleted](#ondataeditcompleted)
- [Zoom and Scroll Events](#zoom-and-scroll-events)
   - [OnZoomStart](#onzoomstart)
   - [OnZoomEnd](#onzoomend)
   - [OnScrollChanged](#onscrollchanged)
- [Export and Print Events](#export-and-print-events)
   - [OnExportComplete](#onexportcomplete)
   - [OnPrintComplete](#onprintcomplete)
- [Axis Events](#axis-events)
   - [OnAxisActualRangeCalculated](#onaxisactualrangecalculated)
   - [OnAxisMultiLevelLabelRender](#onaxismultilevellabelrender)
   - [OnMultiLevelLabelClick](#onmultilevellabelclick)
- [Additional Events](#additional-events)
   - [TooltipRender](#tooltiprender)
   - [SharedTooltipRender](#sharedtooltiprender)


## Chart Lifecycle Events

### OnChartLoaded (Loaded)

Triggers after the chart has completed loading.

**EventArgs**: `LoadedEventArgs`

**Example**:

```razor
<SfChart>
    <ChartEvents Loaded="OnChartLoadedHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnChartLoadedHandler(LoadedEventArgs args)
    {
        // Initialize chart-dependent operations
    }
}
```

**Use Cases**:
- Initialize chart-dependent operations
- Trigger data refresh after chart initialization
- Set up external integrations

### SizeChanged

Triggers when the chart is resized.

**EventArgs**: `ResizeEventArgs`
- `CurrentSize` - Current size of the chart
- `PreviousSize` - Previous size of the chart

**Example**:

```razor
<SfChart>
    <ChartEvents SizeChanged="OnSizeChangedHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnSizeChangedHandler(ResizeEventArgs args)
    {
        Console.WriteLine($"Resized from {args.PreviousSize.Width}x{args.PreviousSize.Height} to {args.CurrentSize.Width}x{args.CurrentSize.Height}");
    }
}
```

**Use Cases**:
- Responsive layout adjustments
- Recalculate chart dimensions for adaptive displays
- Log resize events for analytics

---

## Mouse and Touch Events

### ChartMouseMove

Triggers when the mouse moves over the chart area.

**EventArgs**: `ChartMouseEventArgs`
- `MouseX` - Current mouse X coordinate
- `MouseY` - Current mouse Y coordinate

**Example**:

```razor
<SfChart>
    <ChartEvents ChartMouseMove="OnMouseMoveHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnMouseMoveHandler(ChartMouseEventArgs args)
    {
        // Display coordinates in a custom tooltip or status bar
        Console.WriteLine($"Mouse at X: {args.MouseX}, Y: {args.MouseY}");
    }
}
```

**Use Cases**:
- Custom cursor tracking
- Display real-time coordinates
- Implement custom hover effects

### ChartMouseClick

Triggers when the chart is clicked.

**EventArgs**: `ChartMouseEventArgs`
- `MouseX` - Current mouse X coordinate
- `MouseY` - Current mouse Y coordinate

**Example**:

```razor
<SfChart>
    <ChartEvents ChartMouseClick="OnChartClickHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnChartClickHandler(ChartMouseEventArgs args)
    {
        // Handle chart background clicks
        Console.WriteLine("Chart clicked");
    }
}
```

**Use Cases**:
- Clear selections on background click
- Show chart-level context menus
- Reset zoom/pan on click

### ChartMouseDown

Triggers when the mouse button is pressed on the chart.

**EventArgs**: `ChartMouseEventArgs`
- `MouseX` - Current mouse X coordinate
- `MouseY` - Current mouse Y coordinate

**Example**:

```razor
<SfChart>
    <ChartEvents ChartMouseDown="OnMouseDownHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Area">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnMouseDownHandler(ChartMouseEventArgs args)
    {
        // Capture mouse down for custom drag operations
    }
}
```

**Use Cases**:
- Custom drag operations
- Selection box initialization
- Custom annotation placement

### ChartMouseUp

Triggers when the mouse button is released over the chart.

**EventArgs**: `ChartMouseEventArgs`
- `MouseX` - Current mouse X coordinate
- `MouseY` - Current mouse Y coordinate

**Example**:

```razor
<SfChart>
    <ChartEvents ChartMouseUp="OnMouseUpHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Spline">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnMouseUpHandler(ChartMouseEventArgs args)
    {
        // Complete drag operation
    }
}
```

**Use Cases**:
- Complete custom drag operations
- Finalize selection boxes
- Save annotation positions

### ChartMouseLeave

Triggers when the mouse leaves the chart area. This event has no specific arguments.

**Example**:

```razor
<SfChart>
    <ChartEvents ChartMouseLeave="OnMouseLeaveHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Bar">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnMouseLeaveHandler()
    {
        // Hide custom tooltips
    }
}
```

**Use Cases**:
- Hide custom tooltips or overlays
- Reset hover states
- Clean up temporary UI elements

### PointClick (OnPointClick)

Triggers when a data point is clicked.

**EventArgs**: `PointEventArgs`
- `Chart` - Current chart instance
- `Point` - Clicked data point
- `PointIndex` - Index of the clicked point
- `Series` - Series containing the point
- `SeriesIndex` - Index of the series
- `PageX` - Window page X location
- `PageY` - Window page Y location
- `X` - X coordinate of click
- `Y` - Y coordinate of click

**Example**:

```razor
<SfChart>
    <ChartEvents OnPointClick="OnPointClickHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnPointClickHandler(PointEventArgs args)
    {
        Console.WriteLine($"Clicked: {args.Point.X} - {args.Point.Y}");
        // Navigate to detail view or show modal
    }
}
```

**Use Cases**:
- Navigate to detail views
- Display point-specific modals
- Trigger data updates

### PointMove

Triggers when the mouse hovers over a data point. Uses the same `PointEventArgs` as PointClick.

**Example**:

```razor
<SfChart>
    <ChartEvents PointMove="OnPointMoveHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
            <ChartMarker Visible="true"></ChartMarker>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnPointMoveHandler(PointEventArgs args)
    {
        // Highlight related data or show additional info
    }
}
```

**Use Cases**:
- Custom hover effects
- Display additional point information
- Highlight related data points

---

## Rendering Events

### OnPointRender

Triggers before each data point is rendered.

**EventArgs**: `PointRenderEventArgs`
- `Border` - Point border color and width
- `CornerRadius` - Corner radius for rectangular series
- `Fill` - Point fill color
- `Height` - Point height
- `Width` - Point width
- `Shape` - Marker shape
- `Point` - Current data point
- `Series` - Current series

**Example**:

```razor
<SfChart>
    <ChartEvents OnPointRender="OnPointRenderHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnPointRenderHandler(PointRenderEventArgs args)
    {
        // Color points based on value
        if ((double)args.Point.Y < 30)
            args.Fill = "#FF0000";
        else if ((double)args.Point.Y > 50)
            args.Fill = "#00FF00";
    }
}
```

**Use Cases**:
- Conditional point coloring
- Dynamic shape assignment
- Apply custom styling based on data

### OnDataLabelRender

Triggers before data labels are rendered.

**EventArgs**: `TextRenderEventArgs`
- `Border` - Label border settings
- `Color` - Label text color
- `Font` - Font settings
- `Template` - Template data
- `Text` - Label text

**Example**:

```razor
<SfChart>
    <ChartEvents OnDataLabelRender="OnDataLabelRenderHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
            <ChartMarker>
                <ChartDataLabel Visible="true"></ChartDataLabel>
            </ChartMarker>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnDataLabelRenderHandler(TextRenderEventArgs args)
    {
        // Format labels with custom prefix/suffix
        args.Text = "$" + args.Text + "K";
        args.Color = "#333333";
    }
}
```

**Use Cases**:
- Format label text with units or currency
- Apply conditional styling to labels
- Customize label appearance

### OnAxisLabelRender

Triggers before axis labels are rendered.

**EventArgs**: `AxisLabelRenderEventArgs`
- `LabelStyle` - Font settings for the label
- `Text` - Label text
- `Value` - Label value

**Example**:

```razor
<SfChart>
    <ChartEvents OnAxisLabelRender="OnAxisLabelRenderHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnAxisLabelRenderHandler(AxisLabelRenderEventArgs args)
    {
        // Abbreviate month names
        if (args.Text.Length > 3)
            args.Text = args.Text.Substring(0, 3);
    }
}
```

**Use Cases**:
- Format axis labels
- Abbreviate long labels
- Apply custom number formatting

### OnLegendItemRender

Triggers before legend items are rendered.

**EventArgs**: `LegendRenderEventArgs`
- `Fill` - Legend icon fill color
- `MarkerShape` - Marker shape
- `Shape` - Legend icon shape
- `Text` - Legend text

**Example**:

```razor
<SfChart>
    <ChartEvents OnLegendItemRender="OnLegendRenderHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" Name="Q1 Sales" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
        <ChartSeries DataSource="@TargetData" Name="Target" XName="Month" YName="Target" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnLegendRenderHandler(LegendRenderEventArgs args)
    {
        // Customize legend text
        args.Text = args.Text.ToUpper();
    }
}
```

**Use Cases**:
- Customize legend text
- Change legend icon shapes
- Apply custom legend styling

### OnSeriesRender

Triggers before each series is rendered. This event allows customization of series appearance before rendering.

**Example**:

```razor
<SfChart>
    <ChartEvents OnSeriesRender="OnSeriesRenderHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnSeriesRenderHandler()
    {
        // Apply series-level customization
    }
}
```

**Use Cases**:
- Apply conditional series styling
- Modify series properties before render
- Dynamic series configuration

---

## Interactive Events

### OnSelectionChanged

Triggers after point, series, or cluster selection is completed.

**EventArgs**: `SelectionCompleteEventArgs`
- `SelectedDataValues` - Collection of selected data X and Y values

**Example**:

```razor
<SfChart SelectionMode="Syncfusion.Blazor.Charts.SelectionMode.Point">
    <ChartEvents OnSelectionChanged="OnSelectionChangedHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnSelectionChangedHandler(SelectionCompleteEventArgs args)
    {
        Console.WriteLine($"Selected {args.SelectedDataValues.Count} points");
        // Update related components based on selection
    }
}
```

**Use Cases**:
- Filter related data based on selection
- Update dashboards with selected data
- Enable/disable actions based on selection

### OnLegendClick

Triggers when a legend item is clicked.

**EventArgs**: `LegendClickEventArgs`
- `LegendShape` - Shape of the legend item
- `LegendText` - Text of the legend item
- `Series` - Associated series

**Example**:

```razor
<SfChart>
    <ChartEvents OnLegendClick="OnLegendClickHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" Name="Sales" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
        <ChartSeries DataSource="@TargetData" Name="Target" XName="Month" YName="Target" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnLegendClickHandler(LegendClickEventArgs args)
    {
        Console.WriteLine($"Clicked legend: {args.LegendText}");
        // Toggle series visibility or filter data
    }
}
```

**Use Cases**:
- Custom legend toggle behavior
- Filter data by series
- Track legend interactions

### OnAxisLabelClick

Triggers when an axis label is clicked.

**EventArgs**: `AxisLabelClickEventArgs`
- `Axis` - Current axis
- `Chart` - Chart instance
- `Index` - Label index
- `LabelID` - Label element ID
- `Location` - Label location
- `Text` - Label text
- `Value` - Label value

**Example**:

```razor
<SfChart>
    <ChartEvents OnAxisLabelClick="OnAxisLabelClickHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Bar">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnAxisLabelClickHandler(AxisLabelClickEventArgs args)
    {
        Console.WriteLine($"Clicked axis label: {args.Text}");
        // Filter data by category or drill down
    }
}
```

**Use Cases**:
- Drill-down navigation by category
- Filter data by axis label
- Show category-specific details

### OnDataEdit

Triggers while dragging a data point (when data editing is enabled).

**EventArgs**: `DataEditingEventArgs`
- `NewValue` - New value of the point
- `OldValue` - Previous value of the point
- `Point` - Current point being edited
- `PointIndex` - Index of the point
- `Series` - Current series
- `SeriesIndex` - Index of the series

**Example**:

```razor
<SfChart>
    <ChartEvents OnDataEdit="OnDataEditHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
            <ChartDataEditSettings Enable="true"></ChartDataEditSettings>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnDataEditHandler(DataEditingEventArgs args)
    {
        Console.WriteLine($"Editing: {args.OldValue} -> {args.NewValue}");
        // Validate changes during drag
    }
}
```

**Use Cases**:
- Real-time validation during editing
- Display preview of changes
- Constrain edit values

### OnDataEditCompleted

Triggers when data point dragging is completed.

**EventArgs**: `DataEditingEventArgs` (same as OnDataEdit)

**Example**:

```razor
<SfChart>
    <ChartEvents OnDataEditCompleted="OnDataEditCompletedHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
            <ChartDataEditSettings Enable="true"></ChartDataEditSettings>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnDataEditCompletedHandler(DataEditingEventArgs args)
    {
        // Update data source with new value
        SalesData[args.PointIndex].Sales = args.NewValue;
    }
}
```

**Use Cases**:
- Save edited values to database
- Recalculate dependent values
- Log data changes

---

## Zoom and Scroll Events

### OnZoomStart

Triggers when zoom selection starts.

**EventArgs**: `ZoomingEventArgs`
- `AxisCollection` - Collection of axes being zoomed

**Example**:

```razor
<SfChart>
    <ChartEvents OnZoomStart="OnZoomStartHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Area">
        </ChartSeries>
    </ChartSeriesCollection>
    <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
</SfChart>

@code {
    public void OnZoomStartHandler(ZoomingEventArgs args)
    {
        Console.WriteLine("Zoom started");
        // Show zoom indicator
    }
}
```

**Use Cases**:
- Display zoom indicators
- Disable other interactions during zoom
- Log zoom operations

### OnZoomEnd

Triggers when zoom selection is completed.

**EventArgs**: `ZoomingEventArgs`
- `AxisCollection` - Collection of axes that were zoomed

**Example**:

```razor
<SfChart>
    <ChartEvents OnZoomEnd="OnZoomEndHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
    <ChartZoomSettings EnableSelectionZooming="true"></ChartZoomSettings>
</SfChart>

@code {
    public void OnZoomEndHandler(ZoomingEventArgs args)
    {
        // Load detailed data for zoomed range
        // Update related charts
    }
}
```

**Use Cases**:
- Load detailed data for zoomed range
- Synchronize zoom with other charts
- Save zoom state

### OnScrollChanged

Triggers while scrolling the chart.

**EventArgs**: `ScrollEventArgs`
- `Axis` - Current axis being scrolled
- `CurrentRange` - Current range
- `PreviousAxisRange` - Previous axis range
- `PreviousRange` - Previous range
- `PreviousZoomFactor` - Previous zoom factor
- `PreviousZoomPosition` - Previous zoom position
- `Range` - Current range
- `ZoomFactor` - Current zoom factor
- `ZoomPosition` - Current zoom position

**Example**:

```razor
<SfChart>
    <ChartEvents OnScrollChanged="OnScrollChangedHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category" ZoomFactor="0.5" ZoomPosition="0.2">
        <ChartAxisScrollbarSettings Enable="true"></ChartAxisScrollbarSettings>
    </ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnScrollChangedHandler(ScrollEventArgs args)
    {
        // Lazy load data as user scrolls
        Console.WriteLine($"Scrolled to position: {args.ZoomPosition}");
    }
}
```

**Use Cases**:
- Lazy load data during scroll
- Synchronize scroll with other charts
- Update range indicators

---

## Export and Print Events

### OnExportComplete

Triggers after chart export is completed.

**EventArgs**: `ExportEventArgs`
- `DataUrl` - Data URL of the exported file

**Example**:

```razor
<button @onclick="ExportChart">Export</button>
<SfChart @ref="chartRef">
    <ChartEvents OnExportComplete="OnExportCompleteHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    SfChart chartRef;

    public async Task ExportChart()
    {
        await chartRef.ExportAsync(ExportType.PNG, "SalesChart");
    }

    public void OnExportCompleteHandler(ExportEventArgs args)
    {
        Console.WriteLine("Export completed");
        // Show success message
    }
}
```

**Use Cases**:
- Show export success notification
- Log export operations
- Process exported file data

### OnPrintComplete

Triggers after chart print is completed. This event has no specific arguments.

**Example**:

```razor
<button @onclick="PrintChart">Print</button>
<SfChart @ref="chartRef">
    <ChartEvents OnPrintComplete="OnPrintCompleteHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Bar">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    SfChart chartRef;

    public async Task PrintChart()
    {
        await chartRef.PrintAsync();
    }

    public void OnPrintCompleteHandler()
    {
        Console.WriteLine("Print completed");
        // Log print operation
    }
}
```

**Use Cases**:
- Show print confirmation
- Log print operations
- Track usage metrics

---

## Axis Events

### OnAxisActualRangeCalculated

Triggers before each axis range is calculated.

**EventArgs**: `AxisRangeCalculatedEventArgs`
- `Interval` - Current axis interval
- `Maximum` - Current maximum value
- `Minimum` - Current minimum value

**Example**:

```razor
<SfChart>
    <ChartEvents OnAxisActualRangeCalculated="OnAxisRangeHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnAxisRangeHandler(AxisRangeCalculatedEventArgs args)
    {
        // Adjust axis range dynamically
        args.Maximum = args.Maximum * 1.1; // Add 10% padding
    }
}
```

**Use Cases**:
- Adjust axis ranges dynamically
- Add padding to axis limits
- Synchronize axis ranges across multiple charts

### OnAxisMultiLevelLabelRender

Triggers while rendering multi-level axis labels.

**EventArgs**: `AxisMultiLabelRenderEventArgs`
- `Alignment` - Label alignment
- `Text` - Label text
- `TextStyle` - Text style settings

**Example**:

```razor
<SfChart>
    <ChartEvents OnAxisMultiLevelLabelRender="OnMultiLevelLabelRenderHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category">
        <ChartMultiLevelLabels>
            <ChartMultiLevelLabel>
                <ChartCategories>
                    <ChartCategory Start="0" End="3" Text="Q1"></ChartCategory>
                    <ChartCategory Start="3" End="6" Text="Q2"></ChartCategory>
                </ChartCategories>
            </ChartMultiLevelLabel>
        </ChartMultiLevelLabels>
    </ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnMultiLevelLabelRenderHandler(AxisMultiLabelRenderEventArgs args)
    {
        // Customize multi-level label appearance
        args.Text = args.Text.ToUpper();
    }
}
```

**Use Cases**:
- Customize multi-level label text
- Apply conditional styling
- Format hierarchical labels

### OnMultiLevelLabelClick

Triggers when a multi-level axis label is clicked.

**EventArgs**: `MultiLevelLabelClickEventArgs`
- `Axis` - Axis of the clicked label
- `CustomAttributes` - Custom attributes for the label
- `End` - End value of the label range
- `Level` - Current level of the label
- `Start` - Start value of the label range
- `Text` - Label text

**Example**:

```razor
<SfChart>
    <ChartEvents OnMultiLevelLabelClick="OnMultiLevelLabelClickHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category">
        <ChartMultiLevelLabels>
            <ChartMultiLevelLabel>
                <ChartCategories>
                    <ChartCategory Start="0" End="3" Text="Q1 2024"></ChartCategory>
                    <ChartCategory Start="3" End="6" Text="Q2 2024"></ChartCategory>
                </ChartCategories>
            </ChartMultiLevelLabel>
        </ChartMultiLevelLabels>
    </ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public void OnMultiLevelLabelClickHandler(MultiLevelLabelClickEventArgs args)
    {
        Console.WriteLine($"Clicked: {args.Text} (Level {args.Level})");
        // Filter data by quarter or drill down
    }
}
```

**Use Cases**:
- Drill down by time periods
- Filter data by hierarchical categories
- Navigate to period-specific views

---

## Additional Events

### TooltipRender

Triggers before a tooltip is rendered for a single series.

**EventArgs**: `TooltipRenderEventArgs`
- `HeaderText` - Tooltip header text
- `Text` - Tooltip content text

**Example**:

```razor
<SfChart>
    <ChartEvents TooltipRender="OnTooltipRenderHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
        </ChartSeries>
    </ChartSeriesCollection>
    <ChartTooltipSettings Enable="true"></ChartTooltipSettings>
</SfChart>

@code {
    public void OnTooltipRenderHandler(TooltipRenderEventArgs args)
    {
        args.Text = $"Sales: ${args.Text}K";
    }
}
```

**Use Cases**:
- Format tooltip content
- Add custom metrics to tooltips
- Conditional tooltip styling

### SharedTooltipRender

Triggers before a shared tooltip (multiple series) is rendered.

**EventArgs**: `SharedTooltipRenderEventArgs`
- `HeaderText` - Shared tooltip header
- `Text` - Shared tooltip content

**Example**:

```razor
<SfChart>
    <ChartEvents SharedTooltipRender="OnSharedTooltipRenderHandler"></ChartEvents>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"></ChartPrimaryXAxis>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" Name="Sales" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column">
        </ChartSeries>
        <ChartSeries DataSource="@TargetData" Name="Target" XName="Month" YName="Target" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
        </ChartSeries>
    </ChartSeriesCollection>
    <ChartTooltipSettings Enable="true" Shared="true"></ChartTooltipSettings>
</SfChart>

@code {
    public void OnSharedTooltipRenderHandler(SharedTooltipRenderEventArgs args)
    {
        args.HeaderText = $"Period: {args.HeaderText}";
    }
}
```

**Use Cases**:
- Format shared tooltip headers
- Customize multi-series tooltip layout
- Add comparison data to tooltips
