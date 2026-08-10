# Legend Reference

## Table of Contents

- [Basic Legend Implementation](#basic-legend-implementation)
- [Legend Positioning](#legend-positioning)
   - [Standard Positions](#standard-positions)
   - [Custom Positioning](#custom-positioning)
   - [Legend Alignment](#legend-alignment)
- [Legend Order and Reversal](#legend-order-and-reversal)
   - [Reverse Legend Order](#reverse-legend-order)
- [Legend Appearance Customization](#legend-appearance-customization)
   - [Legend Shape](#legend-shape)
   - [Legend Size](#legend-size)
   - [Legend Shape Size](#legend-shape-size)
   - [Legend Padding and Spacing](#legend-padding-and-spacing)
   - [Legend Text Styling](#legend-text-styling)
   - [Legend Border](#legend-border)
- [Legend Paging](#legend-paging)
- [Legend Text Handling](#legend-text-handling)
   - [Text Wrapping](#text-wrapping)
- [Legend Interactivity](#legend-interactivity)
   - [Toggle Series Visibility](#toggle-series-visibility)
   - [Disable Toggle Visibility](#disable-toggle-visibility)
- [Hiding Legend Items](#hiding-legend-items)
   - [Hide Specific Series from Legend](#hide-specific-series-from-legend)
- [Legend Templates](#legend-templates)
- [Practical Legend Patterns](#practical-legend-patterns)
   - [Multi-Series with Custom Shapes](#multi-series-with-custom-shapes)
   - [Styled Legend with Border](#styled-legend-with-border)
   - [Compact Legend](#compact-legend)
- [Legend Best Practices](#legend-best-practices)
   - [Naming Conventions](#naming-conventions)
   - [Visual Design](#visual-design)
   - [Interaction Guidelines](#interaction-guidelines)
   - [Performance Considerations](#performance-considerations)
   - [Accessibility](#accessibility)
- [Legend with Chart Types](#legend-with-chart-types)
   - [Legend for Line Charts](#legend-for-line-charts)
   - [Legend for Stacked Charts](#legend-for-stacked-charts)
- [Troubleshooting Common Issues](#troubleshooting-common-issues)
   - [Legend Not Visible](#legend-not-visible)
   - [Legend Text Truncated](#legend-text-truncated)
   - [Legend Overlaps Chart](#legend-overlaps-chart)
   - [Too Many Legend Items](#too-many-legend-items)
   - [Legend Click Not Working](#legend-click-not-working)

Legend provides information about the series displayed in the chart, helping users identify and differentiate multiple data series.

## Basic Legend Implementation

Enable legend by setting `Visible="true"` in `ChartLegendSettings`:

```razor
<SfChart Title="Olympic Medals">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
    
        <ChartSeries DataSource="@MedalDetails" Name="Gold" XName="Country" YName="Gold" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column"/>
        <ChartSeries DataSource="@MedalDetails" Name="Silver" XName="Country" YName="Silver" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column"/>
        <ChartSeries DataSource="@MedalDetails" Name="Bronze" XName="Country" YName="Bronze" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column"/>
    
    <ChartLegendSettings Visible="true"/>
</SfChart>

@code {
    public class ChartData
    {
        public string Country { get; set; }
        public double Gold { get; set; }
        public double Silver { get; set; }
        public double Bronze { get; set; }
    }
    
    public List<ChartData> MedalDetails = new List<ChartData>
    {
        new ChartData { Country = "USA", Gold = 50, Silver = 70, Bronze = 45 },
        new ChartData { Country = "China", Gold = 40, Silver = 60, Bronze = 55 },
        new ChartData { Country = "Japan", Gold = 70, Silver = 60, Bronze = 50 },
        new ChartData { Country = "Australia", Gold = 60, Silver = 56, Bronze = 40 }
    };
}
```

## Legend Positioning

### Standard Positions

Position legend at different locations using the `Position` property:

```razor
<!-- Top position -->
<ChartLegendSettings Visible="true" Position="LegendPosition.Top"/>

<!-- Bottom position (default) -->
<ChartLegendSettings Visible="true" Position="LegendPosition.Bottom"/>

<!-- Left position -->
<ChartLegendSettings Visible="true" Position="LegendPosition.Left"/>

<!-- Right position -->
<ChartLegendSettings Visible="true" Position="LegendPosition.Right"/>
```

### Custom Positioning

Place legend at specific coordinates:

```razor
<ChartLegendSettings Visible="true" Position="LegendPosition.Custom">
    <ChartLocation X="200" Y="100"/>
</ChartLegendSettings>
```

### Legend Alignment

Control alignment within the legend area:

```razor
<!-- Left/Near alignment -->
<ChartLegendSettings Visible="true" Alignment="Alignment.Near"/>

<!-- Center alignment (default) -->
<ChartLegendSettings Visible="true" Alignment="Alignment.Center"/>

<!-- Right/Far alignment -->
<ChartLegendSettings Visible="true" Alignment="Alignment.Far"/>
```

**Position and Alignment Combined:**

```razor
<ChartLegendSettings Visible="true" 
                     Position="LegendPosition.Top" 
                     Alignment="Alignment.Far"/>
```

## Legend Order and Reversal

### Reverse Legend Order

Reverse the order of legend items:

```razor
<ChartLegendSettings Visible="true" Reverse="true"/>
```

By default, the first series appears first in the legend. When `Reverse="true"`, the last series appears first.

## Legend Appearance Customization

### Legend Shape

Customize legend icon shape per series using `LegendShape`:

```razor
<ChartSeries DataSource="@Data1" Name="Series 1" XName="X" YName="Y" LegendShape="LegendShape.Circle"/>
<ChartSeries DataSource="@Data2" Name="Series 2" XName="X" YName="Y" LegendShape="LegendShape.Diamond"/>
<ChartSeries DataSource="@Data3" Name="Series 3" XName="X" YName="Y" LegendShape="LegendShape.Triangle"/>
```

**Available Legend Shapes:**
- `SeriesType` - Matches the series type (default)
- `Circle` - Circular icon
- `Rectangle` - Rectangular icon
- `Diamond` - Diamond-shaped icon
- `Triangle` - Triangular icon
- `InvertedTriangle` - Inverted triangle
- `Pentagon` - Five-sided icon
- `Cross` - Cross/plus icon
- `HorizontalLine` - Horizontal line
- `VerticalLine` - Vertical line

### Legend Size

Control overall legend dimensions:

```razor
<ChartLegendSettings Visible="true" 
                     Width="300" 
                     Height="80">
    <ChartLegendBorder Width="1" Color="#CCCCCC"/>
</ChartLegendSettings>
```

**Default Sizes:**
- Top/Bottom position: 20-25% of chart height
- Left/Right position: 20-25% of chart width

### Legend Shape Size

Adjust legend icon dimensions:

```razor
<ChartLegendSettings Visible="true" 
                     ShapeHeight="20" 
                     ShapeWidth="20"/>
```

### Legend Padding and Spacing

Control spacing within legend:

```razor
<ChartLegendSettings Visible="true" 
                     ItemPadding="15"
                     ShapePadding="8"
                     Padding="12"/>
```

**Padding Properties:**
- `ItemPadding` - Space between legend items
- `ShapePadding` - Space between shape and text
- `Padding` - Padding around the legend container

### Legend Text Styling

Customize legend text appearance:

```razor
<ChartLegendSettings Visible="true">
    <ChartLegendTextStyle Size="14px" 
                          Color="#333333" 
                          FontWeight="600" 
                          FontFamily="Arial"/>
</ChartLegendSettings>
```

### Legend Border

Add border around legend:

```razor
<ChartLegendSettings Visible="true">
    <ChartLegendBorder Width="2" Color="#2196F3"/>
</ChartLegendSettings>
```

## Legend Paging

When legend items exceed available space, automatic paging enables navigation buttons:

```razor
<ChartLegendSettings Visible="true" 
                     Width="150" 
                     Height="80">
    <ChartLegendBorder Width="1" Color="#E0E0E0"/>
</ChartLegendSettings>
```

Paging is enabled automatically when content overflows. Users can navigate using arrow buttons.

## Legend Text Handling

### Text Wrapping

Wrap legend text when it exceeds the container:

```razor
<ChartLegendSettings Visible="true" 
                     TextWrap="TextWrap.Wrap" 
                     MaximumLabelWidth="80"/>
```

**Text Wrap Options:**
- `Normal` - No wrapping (default)
- `Wrap` - Wrap text to multiple lines
- `AnyWhere` - Wrap anywhere in the text

**With Maximum Width:**

```razor
<ChartLegendSettings Visible="true" 
                     Position="LegendPosition.Right"
                     TextWrap="TextWrap.Wrap" 
                     MaximumLabelWidth="60">
    <ChartLegendBorder Width="1" Color="#BDBDBD"/>
</ChartLegendSettings>
```

## Legend Interactivity

### Toggle Series Visibility

Click legend items to show/hide series (enabled by default):

```razor
<ChartLegendSettings Visible="true" ToggleVisibility="true"/>
```

### Disable Toggle Visibility

Prevent series toggling when legend is clicked:

```razor
<ChartLegendSettings Visible="true" ToggleVisibility="false"/>
```

This is useful when combined with selection modes:

```razor
<SfChart SelectionMode="Syncfusion.Blazor.Toolkit.Charts.SelectionMode.Series">
        <ChartSeries DataSource="@Data1" Name="Q1" XName="X" YName="Y"/>
        <ChartSeries DataSource="@Data2" Name="Q2" XName="X" YName="Y"/>
        <ChartSeries DataSource="@Data3" Name="Q3" XName="X" YName="Y"/>
    
    <ChartLegendSettings Visible="true" ToggleVisibility="false"/>
</SfChart>
```

## Hiding Legend Items

### Hide Specific Series from Legend

Exclude a series from legend by setting its `Name` to empty string:

```razor
    <ChartSeries DataSource="@Data1" Name="Visible Series" XName="X" YName="Y"/>
    <ChartSeries DataSource="@Data2" Name="" XName="X" YName="Y"/>  <!-- Hidden from legend -->
    <ChartSeries DataSource="@Data3" Name="Another Series" XName="X" YName="Y"/>

<ChartLegendSettings Visible="true"/>
```

## Legend Templates

Create fully custom legend items using templates:

```razor
<SfChart Title="Sales Performance">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
    
    <ChartLegendSettings Visible="true"/>
    
        <ChartSeries DataSource="@SalesData" 
                     Name="Q1 Sales" 
                     XName="Month" 
                     YName="Q1" 
                     Fill="#4CAF50"
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
            <LegendItemTemplate>
                <div style="display: flex; align-items: center; gap: 10px; padding: 6px;">
                    <span style="font-size: 20px;">📊</span>
                    <div style="display: flex; flex-direction: column; line-height: 1.2;">
                        <span style="font-weight: bold; color: #4CAF50;">Q1 Sales</span>
                        <span style="font-size: 11px; color: #757575;">Total: $@Q1Total M</span>
                    </div>
                </div>
            </LegendItemTemplate>
        </ChartSeries>
        
        <ChartSeries DataSource="@SalesData" 
                     Name="Q2 Sales" 
                     XName="Month" 
                     YName="Q2" 
                     Fill="#2196F3"
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column">
            <LegendItemTemplate>
                <div style="display: flex; align-items: center; gap: 10px; padding: 6px;">
                    <span style="font-size: 20px;">📈</span>
                    <div style="display: flex; flex-direction: column; line-height: 1.2;">
                        <span style="font-weight: bold; color: #2196F3;">Q2 Sales</span>
                        <span style="font-size: 11px; color: #757575;">Total: $@Q2Total M</span>
                    </div>
                </div>
            </LegendItemTemplate>
        </ChartSeries>
</SfChart>

@code {
    public List<QuarterData> SalesData = new List<QuarterData>
    {
        new QuarterData { Month = "Jan", Q1 = 35, Q2 = 42 },
        new QuarterData { Month = "Feb", Q1 = 28, Q2 = 38 },
        new QuarterData { Month = "Mar", Q1 = 42, Q2 = 45 }
    };
    
    public double Q1Total => SalesData.Sum(x => x.Q1);
    public double Q2Total => SalesData.Sum(x => x.Q2);
}
```

**Template with Badges:**

```razor
<LegendItemTemplate>
    <div style="display: flex; align-items: center; gap: 8px; padding: 4px 8px; background: #F5F5F5; border-radius: 4px;">
        <span style="width: 12px; height: 12px; background: @context.Fill; border-radius: 50%;"></span>
        <span style="font-weight: 600;">@context.Name</span>
        <span style="background: #FFC107; color: white; padding: 2px 6px; border-radius: 3px; font-size: 10px;">NEW</span>
    </div>
</LegendItemTemplate>
```

**Template with Icons and Statistics:**

```razor
<LegendItemTemplate>
    @{
        var series = context as ChartSeries;
        var total = CalculateSeriesTotal(series);
        var avg = CalculateSeriesAverage(series);
    }
    <div style="border: 1px solid #E0E0E0; padding: 8px; border-radius: 4px; min-width: 150px;">
        <div style="display: flex; align-items: center; gap: 8px; margin-bottom: 4px;">
            <div style="width: 16px; height: 16px; background: @series.Fill; border-radius: 2px;"></div>
            <span style="font-weight: bold; font-size: 13px;">@series.Name</span>
        </div>
        <div style="font-size: 11px; color: #757575;">
            <div>Total: @total.ToString("N0")</div>
            <div>Avg: @avg.ToString("N1")</div>
        </div>
    </div>
</LegendItemTemplate>
```

## Practical Legend Patterns

### Multi-Series with Custom Shapes

```razor
<SfChart Title="Regional Performance">
    <ChartPrimaryXAxis ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category"/>
    
        <ChartSeries DataSource="@NorthData" Name="North Region" XName="Month" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" LegendShape="LegendShape.Circle"/>
        <ChartSeries DataSource="@SouthData" Name="South Region" XName="Month" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" LegendShape="LegendShape.Triangle"/>
        <ChartSeries DataSource="@EastData" Name="East Region" XName="Month" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" LegendShape="LegendShape.Diamond"/>
        <ChartSeries DataSource="@WestData" Name="West Region" XName="Month" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" LegendShape="LegendShape.Pentagon"/>
    
    <ChartLegendSettings Visible="true" 
                         Position="LegendPosition.Top"
                         Alignment="Alignment.Center"
                         ShapeHeight="12"
                         ShapeWidth="12"
                         ItemPadding="20"/>
</SfChart>
```

### Styled Legend with Border

```razor
<ChartLegendSettings Visible="true" 
                     Position="LegendPosition.Right"
                     Width="180"
                     Padding="15"
                     ItemPadding="12"
                     ShapePadding="8"
                     Background="#FAFAFA">
    <ChartLegendBorder Width="2" Color="#2196F3"/>
    <ChartLegendTextStyle Size="13px" FontWeight="500" Color="#424242"/>
</ChartLegendSettings>
```

### Compact Legend

```razor
<ChartLegendSettings Visible="true" 
                     Position="LegendPosition.Bottom"
                     Height="40"
                     ShapeHeight="8"
                     ShapeWidth="8"
                     ItemPadding="8"
                     ShapePadding="4">
    <ChartLegendTextStyle Size="11px"/>
</ChartLegendSettings>
```

## Legend Best Practices

### Naming Conventions

1. **Use clear, concise names** - Keep series names under 20 characters
2. **Be descriptive** - "Q1 2024 Revenue" better than "Q1"
3. **Avoid abbreviations** - Unless industry-standard (like "YTD", "QoQ")
4. **Use consistent naming** - Same format across all series

### Visual Design

1. **Position appropriately** - Bottom/top for horizontal charts, right/left for vertical
2. **Use custom shapes** - Differentiate series with distinct legend shapes
3. **Maintain readability** - Ensure sufficient padding and font size
4. **Consider chart size** - Adjust legend size relative to chart dimensions

### Interaction Guidelines

1. **Enable toggle by default** - Allow users to show/hide series
2. **Disable toggle for critical data** - When all series must remain visible
3. **Use paging wisely** - For charts with many series (>6)
4. **Test click targets** - Ensure legend items are easy to click/tap

### Performance Considerations

1. **Limit series count** - More than 10 series can overwhelm users
2. **Use templates sparingly** - Complex templates can impact rendering performance
3. **Optimize template content** - Keep template HTML minimal
4. **Consider legend size** - Large legends reduce chart display area

### Accessibility

1. **Provide text alternatives** - Use clear series names
2. **Ensure color contrast** - Legend text must be readable
3. **Support keyboard navigation** - Legend items should be keyboard accessible
4. **Test with screen readers** - Verify legend is announced correctly

## Legend with Chart Types

### Legend for Line Charts

```razor
<ChartSeries Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Line" LegendShape="LegendShape.SeriesType">
    <ChartMarker Visible="true" Shape="ChartShape.Circle"/>
</ChartSeries>
```

### Legend for Stacked Charts

```razor
<ChartLegendSettings Visible="true" Position="LegendPosition.Top"/>

    <ChartSeries DataSource="@Data1" Name="Product A" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingColumn"/>
    <ChartSeries DataSource="@Data2" Name="Product B" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingColumn"/>
    <ChartSeries DataSource="@Data3" Name="Product C" XName="Month" YName="Sales" Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.StackingColumn"/>
```

## Troubleshooting Common Issues

### Legend Not Visible

**Issue:** Legend doesn't appear
**Solution:** 
- Ensure `Visible="true"` in `ChartLegendSettings`
- Verify series have `Name` property set
- Check if legend is positioned outside chart bounds

### Legend Text Truncated

**Issue:** Legend text cut off
**Solution:**
```razor
<ChartLegendSettings Visible="true" 
                     TextWrap="TextWrap.Wrap" 
                     MaximumLabelWidth="100"/>
```

### Legend Overlaps Chart

**Issue:** Legend covers chart data
**Solution:**
- Adjust legend position: `Position="LegendPosition.Bottom"`
- Use custom positioning with appropriate coordinates
- Reduce legend size: `Width="200"` or `Height="60"`

### Too Many Legend Items

**Issue:** Legend is overcrowded
**Solution:**
- Enable paging by constraining legend size
- Group related series
- Hide non-essential series from legend
- Use hierarchical legend templates

### Legend Click Not Working

**Issue:** Clicking legend doesn't toggle series
**Solution:**
```razor
<ChartLegendSettings Visible="true" ToggleVisibility="true"/>
```

Ensure `ToggleVisibility` is not set to `false`.
