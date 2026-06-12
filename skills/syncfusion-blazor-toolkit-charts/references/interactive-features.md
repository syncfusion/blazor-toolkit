# Interactive Features Reference

## Table of Contents

- [Tooltips](#tooltips)
   - [Basic Tooltip](#basic-tooltip)
   - [Tooltip Formatting](#tooltip-formatting)
   - [Custom Tooltip Templates](#custom-tooltip-templates)
   - [Shared Tooltips](#shared-tooltips)
- [Crosshair](#crosshair)
   - [Enabling Crosshair](#enabling-crosshair)
   - [Snap to Data](#snap-to-data)
   - [Crosshair Customization](#crosshair-customization)
   - [Crosshair Tooltips](#crosshair-tooltips)
- [Trackball](#trackball)
   - [Enabling Trackball](#enabling-trackball)
   - [Trackball Customization](#trackball-customization)
- [Selection](#selection)
   - [Point Selection](#point-selection)
   - [Series Selection](#series-selection)
   - [Cluster Selection](#cluster-selection)
   - [Drag Selection](#drag-selection)
   - [Selection Modes](#selection-modes)
- [Zooming and Panning](#zooming-and-panning)
   - [Selection Zooming](#selection-zooming)
   - [Mouse Wheel Zooming](#mouse-wheel-zooming)
   - [Pinch Zooming](#pinch-zooming)
   - [Zoom Modes](#zoom-modes)
   - [Zoom Toolbar](#zoom-toolbar)
   - [Panning](#panning)
- [Interactive Best Practices](#interactive-best-practices)
   - [Tooltip Guidelines](#tooltip-guidelines)
   - [Crosshair Best Practices](#crosshair-best-practices)
   - [Selection Recommendations](#selection-recommendations)
   - [Zooming Best Practices](#zooming-best-practices)
   - [Performance Tips](#performance-tips)
   - [Accessibility Considerations](#accessibility-considerations)

## Tooltips

Tooltips display information about data points on mouse hover or touch.

### Basic Tooltip

Enable tooltips using `ChartTooltipSettings`:

```razor
<SfChart Title="Product Sales">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"/>
    <ChartPrimaryYAxis LabelFormat="{value}M"/>
    
    <ChartTooltipSettings Enable="true"/>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" 
                     Name="Sales" 
                     XName="Month" 
                     YName="Revenue" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"/>
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<SalesInfo> SalesData = new List<SalesInfo>
    {
        new SalesInfo { Month = "Jan", Revenue = 35 },
        new SalesInfo { Month = "Feb", Revenue = 28 },
        new SalesInfo { Month = "Mar", Revenue = 42 },
        new SalesInfo { Month = "Apr", Revenue = 38 }
    };
}
```

### Tooltip Formatting

Customize tooltip content using the `Format` property:

```razor
<!-- Basic format -->
<ChartTooltipSettings Enable="true" Format="${point.x} : ${point.y}"/>

<!-- With series name -->
<ChartTooltipSettings Enable="true" 
                      Header="Sales Information"
                      Format="<b>${series.name}</b><br/>Month: ${point.x}<br/>Revenue: $${point.y}M"/>

<!-- With custom header -->
<ChartTooltipSettings Enable="true" 
                      Header="${point.x}"
                      Format="<b>Revenue: $${point.y}M</b>"/>
```

**Format Placeholders:**
- `${point.x}` - X-axis value
- `${point.y}` - Y-axis value
- `${series.name}` - Series name
- `${point.index}` - Point index
- HTML tags supported for formatting

### Custom Tooltip Templates

Create fully custom tooltips with Razor templates:

```razor
<ChartTooltipSettings Enable="true">
    <Template>
        @{
            var data = context as ChartTooltipInfo;
            <div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); 
                        padding: 10px; 
                        border-radius: 6px; 
                        color: white; 
                        box-shadow: 0 4px 6px rgba(0,0,0,0.3);">
                <div style="font-weight: bold; margin-bottom: 5px;">@data.Series.Name</div>
                <div>Month: @data.X</div>
                <div>Revenue: $@($"{data.Y:N2}")M</div>
            </div>
        }
    </Template>
</ChartTooltipSettings>
```

**Template with Conditional Styling:**

```razor
<ChartTooltipSettings Enable="true">
    <Template>
        @{
            var data = context as ChartTooltipInfo;
            var bgColor = data.Y > 40 ? "#4CAF50" : "#F44336";
            var status = data.Y > 40 ? "Above Target" : "Below Target";
            
            <div style="background-color: @bgColor; color: white; padding: 8px; border-radius: 4px;">
                <b>@data.X</b><br/>
                Revenue: $@data.Y M<br/>
                <span style="font-size: 0.9em;">@status</span>
            </div>
        }
    </Template>
</ChartTooltipSettings>
```

**Multi-Series Tooltip Template:**

```razor
<ChartTooltipSettings Enable="true" Shared="true">
    <Template>
        @{
            var data = context as List<ChartTooltipInfo>;
            <div style="background: white; border: 2px solid #2196F3; padding: 10px; border-radius: 4px;">
                <div style="font-weight: bold; border-bottom: 1px solid #ccc; margin-bottom: 5px;">
                    @data[0].X
                </div>
                @foreach (var item in data)
                {
                    <div style="margin: 3px 0;">
                        <span style="color: @item.Series.Fill;">●</span>
                        <b>@item.Series.Name:</b> @item.Y
                    </div>
                }
            </div>
        }
    </Template>
</ChartTooltipSettings>
```

### Shared Tooltips

Display all series data points in a single tooltip:

```razor
<SfChart Title="Quarterly Comparison">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"/>
    
    <ChartTooltipSettings Enable="true" 
                          Shared="true"
                          Format="<b>${point.x}</b><br/>${series.name}: ${point.y}"/>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@Q1Data" Name="Q1" XName="Month" YName="Value" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
            <ChartMarker Visible="true"/>
        </ChartSeries>
        <ChartSeries DataSource="@Q2Data" Name="Q2" XName="Month" YName="Value" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
            <ChartMarker Visible="true"/>
        </ChartSeries>
        <ChartSeries DataSource="@Q3Data" Name="Q3" XName="Month" YName="Value" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
            <ChartMarker Visible="true"/>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>
```

**Tooltip Appearance Customization:**

```razor
<ChartTooltipSettings Enable="true"
                      Fill="#37474F"
                      Opacity="0.95"
                      EnableMarker="true"
                      TextStyle="@tooltipStyle"/>

@code {
    ChartCommonFont tooltipStyle = new ChartCommonFont
    {
        Size = "13px",
        Color = "#FFFFFF",
        FontWeight = "500",
        FontFamily = "Segoe UI"
    };
}
```

---

## Crosshair

Crosshair displays vertical and horizontal lines to precisely identify data point coordinates.

### Enabling Crosshair

Enable crosshair using `ChartCrosshairSettings`:

```razor
<SfChart>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.DateTime">
        <ChartAxisCrosshairTooltip Enable="true"/>
    </ChartPrimaryXAxis>
    
    <ChartPrimaryYAxis>
        <ChartAxisCrosshairTooltip Enable="true"/>
    </ChartPrimaryYAxis>
    
    <ChartCrosshairSettings Enable="true"/>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@TimeSeriesData" XName="Date" YName="Value" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line"/>
    </ChartSeriesCollection>
</SfChart>
```

### Snap to Data

Make crosshair snap to nearest data point instead of exact mouse position:

```razor
<ChartCrosshairSettings Enable="true" SnapToData="true" DashArray="5,5"/>

<ChartTooltipSettings Enable="true" 
                      Shared="true" 
                      Format="<b>${point.x}</b> : ${point.y}"/>
```

### Crosshair Customization

Customize crosshair line appearance and behavior:

```razor
<ChartCrosshairSettings Enable="true" SnapToData="true">
    <ChartCrosshairLine Width="2" Color="#2196F3" DashArray="5,5"/>
</ChartCrosshairSettings>

<ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category">
    <ChartAxisCrosshairTooltip Enable="true" Fill="#FF5722">
        <ChartCrosshairTextStyle Size="13px" Color="white" FontWeight="600"/>
    </ChartAxisCrosshairTooltip>
</ChartPrimaryXAxis>

<ChartPrimaryYAxis>
    <ChartAxisCrosshairTooltip Enable="true" Fill="#4CAF50">
        <ChartCrosshairTextStyle Size="13px" Color="white" FontWeight="600"/>
    </ChartAxisCrosshairTooltip>
</ChartPrimaryYAxis>
```

### Crosshair Tooltips

Configure axis-specific crosshair tooltips:

```razor
<ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.DateTime">
    <ChartAxisCrosshairTooltip Enable="true" 
                                Fill="#37474F"
                                Format="MM/dd/yyyy">
        <ChartCrosshairTextStyle Color="#FFFFFF" Size="12px"/>
    </ChartAxisCrosshairTooltip>
</ChartPrimaryXAxis>

<ChartPrimaryYAxis LabelFormat="${value}">
    <ChartAxisCrosshairTooltip Enable="true" 
                                Fill="#37474F">
        <ChartCrosshairTextStyle Color="#FFFFFF" Size="12px"/>
    </ChartAxisCrosshairTooltip>
</ChartPrimaryYAxis>
```

---

## Trackball

Trackball is similar to crosshair but displays tooltips for all series at the crosshair position.

### Enabling Trackball

Enable trackball mode by combining crosshair with shared tooltips:

```razor
<SfChart>
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.DateTime"/>
    
    <ChartCrosshairSettings Enable="true" SnapToData="true" LineType="LineType.Vertical"/>
    
    <ChartTooltipSettings Enable="true" 
                          Shared="true" 
                          Format="${series.name} : <b>${point.y}</b>"/>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@Sales2023" Name="2023 Sales" XName="Date" YName="Amount" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
            <ChartMarker Visible="true" Height="8" Width="8"/>
        </ChartSeries>
        <ChartSeries DataSource="@Sales2024" Name="2024 Sales" XName="Date" YName="Amount" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line">
            <ChartMarker Visible="true" Height="8" Width="8"/>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>
```

### Trackball Customization

Customize trackball appearance and behavior:

```razor
<ChartCrosshairSettings Enable="true" SnapToData="true">
    <ChartCrosshairLine Width="1" Color="#757575" DashArray="3,3"/>
</ChartCrosshairSettings>

<ChartTooltipSettings Enable="true" 
                      Shared="true"
                      EnableMarker="true"
                      Fill="#263238"
                      Opacity="0.9">
    <Template>
        @{
            var data = context as List<ChartTooltipInfo>;
            <div style="padding: 10px; background: #263238; color: white; border-radius: 4px;">
                <div style="font-weight: bold; margin-bottom: 8px; border-bottom: 1px solid #546E7A; padding-bottom: 4px;">
                    @data[0].X.ToString("MMM dd, yyyy")
                </div>
                @foreach (var item in data)
                {
                    <div style="margin: 4px 0; display: flex; justify-content: space-between; gap: 15px;">
                        <span>
                            <span style="color: @item.Series.Fill; font-size: 16px;">●</span>
                            @item.Series.Name
                        </span>
                        <span style="font-weight: bold;">$@($"{item.Y:N2}")</span>
                    </div>
                }
            </div>
        }
    </Template>
</ChartTooltipSettings>
```

---

## Selection

Allow users to select data points, series, or regions for highlighting or further analysis.

### Point Selection

Select individual data points:

```razor
<SfChart Title="Product Performance" SelectionMode="Syncfusion.Blazor.Charts.SelectionMode.Point">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"/>
    
    <ChartSelectionSettings Enable="true" 
                             Mode="Syncfusion.Blazor.Charts.SelectionMode.Point"
                             Type="SelectionType.Highlight"
                             Pattern="SelectionPattern.Dots"/>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@ProductData" 
                     XName="Product" 
                     YName="Sales" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"
                     SelectionStyle="@selectionStyle"/>
    </ChartSeriesCollection>
</SfChart>

@code {
    string selectionStyle = "fill: #FF5722; opacity: 1;";
}
```

### Series Selection

Select entire series:

```razor
<SfChart Title="Quarterly Sales" SelectionMode="Syncfusion.Blazor.Charts.SelectionMode.Series">
    <ChartSeriesCollection>
        <ChartSeries DataSource="@Q1Data" Name="Q1" XName="Month" YName="Revenue" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"/>
        <ChartSeries DataSource="@Q2Data" Name="Q2" XName="Month" YName="Revenue" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"/>
        <ChartSeries DataSource="@Q3Data" Name="Q3" XName="Month" YName="Revenue" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"/>
    </ChartSeriesCollection>
</SfChart>
```

### Cluster Selection

Select all points at the same index across series:

```razor
<SfChart Title="Regional Comparison" SelectionMode="Syncfusion.Blazor.Charts.SelectionMode.Cluster">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"/>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@NorthRegion" Name="North" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"/>
        <ChartSeries DataSource="@SouthRegion" Name="South" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"/>
        <ChartSeries DataSource="@EastRegion" Name="East" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"/>
    </ChartSeriesCollection>
</SfChart>
```

### Drag Selection

Enable rectangular drag selection:

```razor
<SfChart SelectionMode="Syncfusion.Blazor.Charts.SelectionMode.DragXY">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.DateTime"/>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@ScatterData" 
                     XName="Date" 
                     YName="Value" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Scatter">
            <ChartMarker Height="10" Width="10"/>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>
```

**Drag Selection Modes:**
- `DragXY` - Rectangle selection in both directions
- `DragX` - Horizontal selection only
- `DragY` - Vertical selection only

### Selection Modes

Configure selection behavior and appearance:

```razor
<SfChart SelectionMode="Syncfusion.Blazor.Charts.SelectionMode.Point">
    <ChartSeriesCollection>
        <ChartSeries DataSource="@ChartData" 
                     XName="X" 
                     YName="Y" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"
                     SelectionStyle="fill: #4CAF50; stroke: #2E7D32; stroke-width: 3"/>
    </ChartSeriesCollection>
</SfChart>
```

**Multi-Select with Ctrl Key:**

```razor
<SfChart Title="Multi-Select Chart" 
         SelectionMode="Syncfusion.Blazor.Charts.SelectionMode.Point" 
         IsMultiSelect="true">
    <ChartSeriesCollection>
        <ChartSeries DataSource="@Data" XName="X" YName="Y" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"/>
    </ChartSeriesCollection>
</SfChart>
```

**Selection Events:**

```razor
<SfChart SelectionMode="Syncfusion.Blazor.Charts.SelectionMode.Point" 
         OnSelectionComplete="HandleSelectionComplete">
    <ChartSeriesCollection>
        <ChartSeries DataSource="@ChartData" XName="X" YName="Y" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"/>
    </ChartSeriesCollection>
</SfChart>

@code {
    private void HandleSelectionComplete(SelectionCompleteEventArgs args)
    {
        var selectedPoints = args.SelectedDataValues;
        Console.WriteLine($"Selected {selectedPoints.Count} points");
        
        foreach (var point in selectedPoints)
        {
            Console.WriteLine($"X: {point.X}, Y: {point.Y}");
        }
    }
}
```

---

## Zooming and Panning

Enable users to zoom into chart regions and pan across data.

### Selection Zooming

Enable selection-based zooming (drag to select region):

```razor
<SfChart Title="Sales History">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"/>
    
    <ChartZoomSettings EnableSelectionZooming="true"/>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column"/>
    </ChartSeriesCollection>
</SfChart>
```

### Mouse Wheel Zooming

Enable zoom with mouse wheel:

```razor
<ChartZoomSettings EnableMouseWheelZooming="true"/>
```

### Pinch Zooming

Enable pinch-to-zoom for touch devices:

```razor
<ChartZoomSettings EnablePinchZooming="true"/>
```

**All Zoom Modes Combined:**

```razor
<SfChart Title="Comprehensive Zoom Example">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Charts.ValueType.Category"/>
    
    <ChartZoomSettings EnableSelectionZooming="true"
                       EnableMouseWheelZooming="true"
                       EnablePinchZooming="true"
                       EnableScrollbar="true"/>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@LargeDataset" XName="X" YName="Y" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line"/>
    </ChartSeriesCollection>
</SfChart>
```

### Zoom Modes

Control zoom direction:

```razor
<!-- Horizontal zoom only -->
<ChartZoomSettings EnableSelectionZooming="true" Mode="ZoomMode.X"/>

<!-- Vertical zoom only -->
<ChartZoomSettings EnableSelectionZooming="true" Mode="ZoomMode.Y"/>

<!-- Both directions (default) -->
<ChartZoomSettings EnableSelectionZooming="true" Mode="ZoomMode.XY"/>
```

### Zoom Toolbar

Customize the zoom toolbar that appears after zooming:

```razor
<ChartZoomSettings EnableSelectionZooming="true"
                   EnableMouseWheelZooming="true"
                   ToolbarItems="@zoomToolbarItems"/>

@code {
    private List<ToolbarItems> zoomToolbarItems = new List<ToolbarItems>
    {
        ToolbarItems.Zoom,
        ToolbarItems.ZoomIn,
        ToolbarItems.ZoomOut,
        ToolbarItems.Pan,
        ToolbarItems.Reset
    };
}
```

**Hide Specific Toolbar Items:**

```razor
@code {
    private List<ToolbarItems> customToolbar = new List<ToolbarItems>
    {
        ToolbarItems.Reset,
        ToolbarItems.Pan
    };
}

<ChartZoomSettings EnableSelectionZooming="true" ToolbarItems="@customToolbar"/>
```

### Panning

Enable panning after zooming:

```razor
<ChartZoomSettings EnableSelectionZooming="true"
                   EnablePan="true"/>
```

**Programmatic Zoom:**

```razor
<SfChart @ref="chartObj">
    <ChartZoomSettings EnableSelectionZooming="true"/>
    <ChartSeriesCollection>
        <ChartSeries DataSource="@Data" XName="X" YName="Y" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line"/>
    </ChartSeriesCollection>
</SfChart>

<button @onclick="ZoomIn">Zoom In</button>
<button @onclick="ZoomOut">Zoom Out</button>
<button @onclick="ResetZoom">Reset</button>

@code {
    private SfChart chartObj;
    
    private void ZoomIn()
    {
        chartObj.ZoomByFactor(1.5);
    }
    
    private void ZoomOut()
    {
        chartObj.ZoomByFactor(0.5);
    }
    
    private void ResetZoom()
    {
        chartObj.ZoomByRange(new DateTime(2020, 1, 1), new DateTime(2024, 12, 31));
    }
}
```

---

## Interactive Best Practices

### Tooltip Guidelines

1. **Keep tooltips concise** - Display only essential information
2. **Use formatting** - Format numbers consistently (currency, decimals)
3. **Enable shared tooltips** for multi-series comparisons
4. **Add visual cues** - Use colors, icons, or markers in templates
5. **Test on mobile** - Ensure tooltips work well with touch interactions

### Crosshair Best Practices

1. **Use with line charts** - Most effective for time-series or continuous data
2. **Enable snap-to-data** - Provides precise data point information
3. **Customize colors** - Match crosshair to chart theme
4. **Combine with tooltips** - Provide complete data context

### Selection Recommendations

1. **Choose appropriate mode** - Point for detail, Series for comparison, Cluster for correlation
2. **Provide visual feedback** - Use distinct selection colors
3. **Enable multi-select** when users need to compare multiple points
4. **Handle selection events** - Implement actions based on user selections

### Zooming Best Practices

1. **Enable multiple zoom methods** - Selection, mouse wheel, and pinch for accessibility
2. **Set appropriate zoom modes** - X for time-series, XY for scatter plots
3. **Always include reset** - Allow users to return to original view
4. **Consider data volume** - Enable scrollbar for large datasets
5. **Test zoom limits** - Prevent excessive zoom that obscures data

### Performance Tips

1. **Limit tooltip complexity** - Avoid heavy computations in tooltip templates
2. **Throttle crosshair updates** on large datasets
3. **Use selection judiciously** - Avoid enabling all selection modes simultaneously
4. **Optimize zoom rendering** - Consider virtualization for very large datasets
5. **Test on target devices** - Ensure smooth interactions on mobile and desktop

### Accessibility Considerations

1. **Provide keyboard navigation** for selection and zooming
2. **Ensure tooltip contrast** - Maintain WCAG AA compliance
3. **Support screen readers** - Use ARIA labels where appropriate
4. **Test with assistive technologies** - Verify all interactive features are accessible
5. **Offer alternative data access** - Provide data tables or exports for users who cannot use interactive features
