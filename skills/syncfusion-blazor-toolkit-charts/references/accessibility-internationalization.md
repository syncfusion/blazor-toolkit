# Accessibility and Internationalization Reference

## Table of Contents

- [Accessibility Features](#accessibility-features)
   - [ARIA Labels and Roles](#aria-labels-and-roles)
   - [Keyboard Navigation](#keyboard-navigation)
   - [Screen Reader Support](#screen-reader-support)
   - [High Contrast Modes](#high-contrast-modes)
   - [Focus Management](#focus-management)
- [Internationalization (i18n)](#internationalization-i18n)
   - [Locale Configuration](#locale-configuration)
   - [Number Formatting](#number-formatting)
   - [Date Formatting](#date-formatting)
   - [Currency Symbols](#currency-symbols)
- [Localization (l10n)](#localization-l10n)
   - [Text Translation](#text-translation)
   - [RTL Support](#rtl-support)
   - [Custom Locales](#custom-locales)
   - [Loading Locale Data](#loading-locale-data)
- [Color Accessibility](#color-accessibility)
   - [WCAG Compliance](#wcag-compliance)
   - [Color-Blind Friendly Palettes](#color-blind-friendly-palettes)
   - [Contrast Ratios](#contrast-ratios)
   - [Pattern Alternatives](#pattern-alternatives)
- [Responsive Accessibility](#responsive-accessibility)
   - [Touch Target Sizing](#touch-target-sizing)
   - [Mobile Accessibility](#mobile-accessibility)
   - [Adaptive Features](#adaptive-features)
- [Testing and Validation](#testing-and-validation)
   - [Accessibility Testing Tools](#accessibility-testing-tools)
   - [Compliance Checklists](#compliance-checklists)
   - [Common Issues and Fixes](#common-issues-and-fixes)


## Accessibility Features

### ARIA Labels and Roles

Syncfusion Blazor Charts follow WAI-ARIA standards to provide semantic information for assistive technologies.

**Default ARIA Attributes**

```razor
@using Syncfusion.Blazor.Toolkit

<SfChart Title="Sales Analysis">
    <ChartPrimaryXAxis Title="Months" />
    <ChartPrimaryYAxis Title="Revenue" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@ChartData" XName="Month" YName="Sales" 
                     Type="Syncfusion.Blazor.Toolkit.ChartSeriesType.Column" Name="Revenue">
            <ChartMarker>
                <ChartDataLabel Visible="true" />
            </ChartMarker>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<SalesData> ChartData = new List<SalesData>
    {
        new SalesData { Month = "Jan", Sales = 35 },
        new SalesData { Month = "Feb", Sales = 28 },
        new SalesData { Month = "Mar", Sales = 42 }
    };
    
    public class SalesData
    {
        public string Month { get; set; }
        public double Sales { get; set; }
    }
}
```

**ARIA Role Mappings**

| Element | ARIA Role | Description |
|---------|-----------|-------------|
| Chart Container | `region` | Main chart area |
| Legend | `button` | Toggleable series visibility |
| Data Point | `img` | Individual data representations |
| Tooltip | `tooltip` | Contextual information |
| Axis Labels | `text` | Axis value descriptions |

### Keyboard Navigation

Complete keyboard support following WCAG 2.2 guidelines.

**Basic Navigation**

```razor
@using Syncfusion.Blazor.Toolkit

<SfChart Title="Keyboard Accessible Chart">
    <ChartPrimaryXAxis Title="Years" />
    <ChartPrimaryYAxis Title="Growth %" />
    
    <ChartLegendSettings Visible="true" Position="LegendPosition.Top" />
    
    <ChartZoomSettings EnableSelectionZooming="true" 
                       EnableMouseWheelZooming="true">
        <ChartZoomToolbarButtons>
            <ChartZoomToolbarButton>Zoom</ChartZoomToolbarButton>
            <ChartZoomToolbarButton>ZoomIn</ChartZoomToolbarButton>
            <ChartZoomToolbarButton>ZoomOut</ChartZoomToolbarButton>
            <ChartZoomToolbarButton>Pan</ChartZoomToolbarButton>
            <ChartZoomToolbarButton>Reset</ChartZoomToolbarButton>
        </ChartZoomToolbarButtons>
    </ChartZoomSettings>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@GrowthData" XName="Year" YName="Rate" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line" Name="Growth Rate" />
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<GrowthInfo> GrowthData = new List<GrowthInfo>
    {
        new GrowthInfo { Year = 2020, Rate = 5.2 },
        new GrowthInfo { Year = 2021, Rate = 6.8 },
        new GrowthInfo { Year = 2022, Rate = 7.5 }
    };
    
    public class GrowthInfo
    {
        public int Year { get; set; }
        public double Rate { get; set; }
    }
}
```

**Keyboard Shortcuts Reference**

| Key Combination | Action |
|----------------|--------|
| `Alt + J` | Focus chart element |
| `Tab` / `Shift + Tab` | Navigate elements |
| `Arrow Keys` | Navigate data points/series |
| `Enter` / `Space` | Select/toggle element |
| `Ctrl + +` / `Ctrl + -` | Zoom in/out |
| `Arrow Keys` (during pan) | Pan chart |
| `R` | Reset zoom |
| `Ctrl + P` | Print chart |

### Screen Reader Support

Optimize chart content for screen readers with descriptive labels.

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="Quarterly Revenue Report" 
         Description="Bar chart showing quarterly revenue from Q1 to Q4">
    <ChartPrimaryXAxis Title="Quarter" />
    <ChartPrimaryYAxis Title="Revenue in Millions" LabelFormat="c0" />
    
    <ChartTooltipSettings Enable="true" 
                          Format="${point.x}: ${point.y}" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@RevenueData" XName="Quarter" YName="Amount" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Bar" Name="Revenue">
            <ChartMarker>
                <ChartDataLabel Visible="true" Position="LabelPosition.Top">
                    <ChartDataLabelFont FontWeight="600" />
                </ChartDataLabel>
            </ChartMarker>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<QuarterData> RevenueData = new List<QuarterData>
    {
        new QuarterData { Quarter = "Q1", Amount = 25.5 },
        new QuarterData { Quarter = "Q2", Amount = 32.8 },
        new QuarterData { Quarter = "Q3", Amount = 28.3 },
        new QuarterData { Quarter = "Q4", Amount = 41.2 }
    };
    
    public class QuarterData
    {
        public string Quarter { get; set; }
        public double Amount { get; set; }
    }
}
```

### High Contrast Modes

Support system high contrast themes for improved visibility.

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="High Contrast Chart" Theme="Theme.HighContrast">
    <ChartPrimaryXAxis Title="Categories" />
    <ChartPrimaryYAxis Title="Values" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@ContrastData" XName="Category" YName="Value" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="Series 1">
            <ChartSeriesBorder Width="2" Color="#FFFFFF" />
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<CategoryData> ContrastData = new List<CategoryData>
    {
        new CategoryData { Category = "A", Value = 45 },
        new CategoryData { Category = "B", Value = 62 },
        new CategoryData { Category = "C", Value = 38 }
    };
    
    public class CategoryData
    {
        public string Category { get; set; }
        public double Value { get; set; }
    }
}
```

### Focus Management

Implement proper focus indicators and management.

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="Focus Managed Chart">
    <ChartPrimaryXAxis Title="Products" />
    <ChartPrimaryYAxis Title="Units Sold" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@ProductData" XName="Product" YName="Units" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="Sales">
            <ChartSeriesAnimation Enable="true" Duration="1000" />
            <ChartEmptyPointSettings Mode="EmptyPointMode.Gap" />
        </ChartSeries>
    </ChartSeriesCollection>
    
    <ChartSelectionSettings Enable="true" Mode="Syncfusion.Blazor.Charts.SelectionMode.Point">
        <ChartSelectionPattern>
            <ChartSelectionPattern Type="SelectionPattern.Dots" />
        </ChartSelectionPattern>
    </ChartSelectionSettings>
</SfChart>

@code {
    public List<ProductInfo> ProductData = new List<ProductInfo>
    {
        new ProductInfo { Product = "Laptop", Units = 120 },
        new ProductInfo { Product = "Phone", Units = 250 },
        new ProductInfo { Product = "Tablet", Units = 95 }
    };
    
    public class ProductInfo
    {
        public string Product { get; set; }
        public double Units { get; set; }
    }
}
```

---

## Internationalization (i18n)

### Locale Configuration

Configure chart to use specific locales for formatting.

```razor
@using Syncfusion.Blazor
@using Syncfusion.Blazor.Charts
@using System.Globalization

<SfChart Title="International Sales" Locale="de-DE">
    <ChartPrimaryXAxis Title="Monat" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    <ChartPrimaryYAxis Title="Verkäufe" LabelFormat="c2" />
    
    <ChartTooltipSettings Enable="true" Format="${point.x}: ${point.y}" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SalesData" XName="Month" YName="Sales" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line" Name="Verkäufe" />
    </ChartSeriesCollection>
</SfChart>

@code {
    protected override void OnInitialized()
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("de-DE");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("de-DE");
    }
    
    public List<MonthlySales> SalesData = new List<MonthlySales>
    {
        new MonthlySales { Month = "Januar", Sales = 12500.50 },
        new MonthlySales { Month = "Februar", Sales = 15300.75 },
        new MonthlySales { Month = "März", Sales = 18200.25 }
    };
    
    public class MonthlySales
    {
        public string Month { get; set; }
        public double Sales { get; set; }
    }
}
```

### Number Formatting

Apply culture-specific number formatting.

```razor
@using Syncfusion.Blazor.Charts
@using System.Globalization

<SfChart Title="Number Formatting Examples">
    <ChartPrimaryXAxis Title="Region" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    <ChartPrimaryYAxis Title="Population" LabelFormat="n0" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@PopulationData" XName="Region" YName="Population" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="Population">
            <ChartMarker>
                <ChartDataLabel Visible="true" Position="LabelPosition.Top">
                    <ChartDataLabelFont Size="10px" />
                </ChartDataLabel>
            </ChartMarker>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<RegionData> PopulationData = new List<RegionData>
    {
        new RegionData { Region = "Asia", Population = 4641054775 },
        new RegionData { Region = "Africa", Population = 1340598147 },
        new RegionData { Region = "Europe", Population = 747636026 }
    };
    
    public class RegionData
    {
        public string Region { get; set; }
        public double Population { get; set; }
    }
}
```

### Date Formatting

Format dates according to locale conventions.

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="Date Time Formatting">
    <ChartPrimaryXAxis Title="Timeline" ValueType="Syncfusion.Blazor.Toolkit.ValueType.DateTime" 
                       LabelFormat="MMM dd, yyyy" IntervalType="IntervalType.Months" />
    <ChartPrimaryYAxis Title="Temperature (°C)" LabelFormat="n1" />
    
    <ChartTooltipSettings Enable="true" Format="${point.x}: ${point.y}°C" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@TemperatureData" XName="Date" YName="Temp" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Spline" Name="Temperature">
            <ChartMarker Visible="true" Height="7" Width="7" />
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<TempData> TemperatureData = new List<TempData>
    {
        new TempData { Date = new DateTime(2024, 1, 1), Temp = 15.5 },
        new TempData { Date = new DateTime(2024, 2, 1), Temp = 18.2 },
        new TempData { Date = new DateTime(2024, 3, 1), Temp = 22.8 },
        new TempData { Date = new DateTime(2024, 4, 1), Temp = 25.3 }
    };
    
    public class TempData
    {
        public DateTime Date { get; set; }
        public double Temp { get; set; }
    }
}
```

### Currency Symbols

Display currency using locale-specific symbols.

```razor
@using Syncfusion.Blazor.Charts
@using System.Globalization

<SfChart Title="Multi-Currency Revenue">
    <ChartPrimaryXAxis Title="Quarter" ValueType="Syncfusion.Blazor.Toolkit.ValueType.Category" />
    <ChartPrimaryYAxis Title="Revenue" LabelFormat="c0" />
    
    <ChartTooltipSettings Enable="true" Format="${series.name}<br/>${point.x}: ${point.y}" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@UsdRevenue" XName="Quarter" YName="Amount" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="USD Revenue" />
        <ChartSeries DataSource="@EurRevenue" XName="Quarter" YName="Amount" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="EUR Revenue" />
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<CurrencyData> UsdRevenue = new List<CurrencyData>
    {
        new CurrencyData { Quarter = "Q1", Amount = 125000 },
        new CurrencyData { Quarter = "Q2", Amount = 138000 }
    };
    
    public List<CurrencyData> EurRevenue = new List<CurrencyData>
    {
        new CurrencyData { Quarter = "Q1", Amount = 110000 },
        new CurrencyData { Quarter = "Q2", Amount = 125000 }
    };
    
    public class CurrencyData
    {
        public string Quarter { get; set; }
        public double Amount { get; set; }
    }
}
```

---

## Localization (l10n)

### Text Translation

Translate chart text elements for different languages.

```razor
@using Syncfusion.Blazor
@using Syncfusion.Blazor.Charts

<SfChart Title="@GetLocalizedText("ChartTitle")">
    <ChartPrimaryXAxis Title="@GetLocalizedText("XAxisTitle")" />
    <ChartPrimaryYAxis Title="@GetLocalizedText("YAxisTitle")" />
    
    <ChartLegendSettings Visible="true" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@LocalizedData" XName="Category" YName="Value" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="@GetLocalizedText("SeriesName")" />
    </ChartSeriesCollection>
</SfChart>

@code {
    private string CurrentLocale = "en-US";
    
    private Dictionary<string, Dictionary<string, string>> Translations = new()
    {
        ["en-US"] = new Dictionary<string, string>
        {
            ["ChartTitle"] = "Sales Overview",
            ["XAxisTitle"] = "Products",
            ["YAxisTitle"] = "Sales Amount",
            ["SeriesName"] = "Current Year"
        },
        ["es-ES"] = new Dictionary<string, string>
        {
            ["ChartTitle"] = "Resumen de Ventas",
            ["XAxisTitle"] = "Productos",
            ["YAxisTitle"] = "Monto de Ventas",
            ["SeriesName"] = "Año Actual"
        },
        ["fr-FR"] = new Dictionary<string, string>
        {
            ["ChartTitle"] = "Aperçu des Ventes",
            ["XAxisTitle"] = "Produits",
            ["YAxisTitle"] = "Montant des Ventes",
            ["SeriesName"] = "Année en Cours"
        }
    };
    
    private string GetLocalizedText(string key)
    {
        return Translations[CurrentLocale].ContainsKey(key) 
            ? Translations[CurrentLocale][key] 
            : key;
    }
    
    public List<LocaleData> LocalizedData = new List<LocaleData>
    {
        new LocaleData { Category = "A", Value = 120 },
        new LocaleData { Category = "B", Value = 95 },
        new LocaleData { Category = "C", Value = 150 }
    };
    
    public class LocaleData
    {
        public string Category { get; set; }
        public double Value { get; set; }
    }
}
```

### RTL Support

Enable right-to-left layout for RTL languages.

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="مخطط المبيعات" EnableRtl="true">
    <ChartPrimaryXAxis Title="المنتجات" />
    <ChartPrimaryYAxis Title="المبيعات" LabelFormat="n0" />
    
    <ChartLegendSettings Visible="true" Position="LegendPosition.Top" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@RtlData" XName="Product" YName="Sales" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Bar" Name="المبيعات الحالية">
            <ChartMarker>
                <ChartDataLabel Visible="true" Position="LabelPosition.Outer" />
            </ChartMarker>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<ProductSales> RtlData = new List<ProductSales>
    {
        new ProductSales { Product = "محمول", Sales = 85000 },
        new ProductSales { Product = "هاتف", Sales = 125000 },
        new ProductSales { Product = "جهاز لوحي", Sales = 65000 }
    };
    
    public class ProductSales
    {
        public string Product { get; set; }
        public double Sales { get; set; }
    }
}
```

### Custom Locales

Define custom locale settings for specialized requirements.

```razor
@using Syncfusion.Blazor.Charts
@using System.Globalization

<SfChart Title="Custom Locale Chart">
    <ChartPrimaryXAxis Title="Period" />
    <ChartPrimaryYAxis Title="Metric" LabelFormat="{value}K" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@MetricData" XName="Period" YName="Value" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Area" Name="Performance" Opacity="0.6" />
    </ChartSeriesCollection>
</SfChart>

@code {
    protected override void OnInitialized()
    {
        var customCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ",";
        customCulture.NumberFormat.NumberGroupSeparator = ".";
        CultureInfo.DefaultThreadCurrentCulture = customCulture;
    }
    
    public List<MetricInfo> MetricData = new List<MetricInfo>
    {
        new MetricInfo { Period = "Jan", Value = 25.5 },
        new MetricInfo { Period = "Feb", Value = 32.8 },
        new MetricInfo { Period = "Mar", Value = 28.3 }
    };
    
    public class MetricInfo
    {
        public string Period { get; set; }
        public double Value { get; set; }
    }
}
```

### Loading Locale Data

Load and apply locale data dynamically.

```razor
@using Syncfusion.Blazor
@using Syncfusion.Blazor.Charts

<div>
    <label>Select Language: </label>
    <select @onchange="ChangeLocale">
        <option value="en-US">English</option>
        <option value="de-DE">Deutsch</option>
        <option value="ja-JP">日本語</option>
    </select>
</div>

<SfChart Title="@ChartTitle" Locale="@SelectedLocale">
    <ChartPrimaryXAxis Title="@XAxisTitle" />
    <ChartPrimaryYAxis Title="@YAxisTitle" LabelFormat="c0" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@ChartData" XName="Month" YName="Sales" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="@SeriesName" />
    </ChartSeriesCollection>
</SfChart>

@code {
    private string SelectedLocale = "en-US";
    private string ChartTitle = "Monthly Sales";
    private string XAxisTitle = "Month";
    private string YAxisTitle = "Sales";
    private string SeriesName = "Revenue";
    
    private void ChangeLocale(ChangeEventArgs e)
    {
        SelectedLocale = e.Value.ToString();
        LoadLocaleStrings(SelectedLocale);
    }
    
    private void LoadLocaleStrings(string locale)
    {
        switch (locale)
        {
            case "de-DE":
                ChartTitle = "Monatliche Verkäufe";
                XAxisTitle = "Monat";
                YAxisTitle = "Verkäufe";
                SeriesName = "Umsatz";
                break;
            case "ja-JP":
                ChartTitle = "月次売上";
                XAxisTitle = "月";
                YAxisTitle = "売上";
                SeriesName = "収益";
                break;
            default:
                ChartTitle = "Monthly Sales";
                XAxisTitle = "Month";
                YAxisTitle = "Sales";
                SeriesName = "Revenue";
                break;
        }
    }
    
    public List<SalesInfo> ChartData = new List<SalesInfo>
    {
        new SalesInfo { Month = "Jan", Sales = 45000 },
        new SalesInfo { Month = "Feb", Sales = 52000 },
        new SalesInfo { Month = "Mar", Sales = 48000 }
    };
    
    public class SalesInfo
    {
        public string Month { get; set; }
        public double Sales { get; set; }
    }
}
```

---

## Color Accessibility

### WCAG Compliance

Ensure color choices meet WCAG 2.2 Level AA standards.

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="WCAG Compliant Chart">
    <ChartPrimaryXAxis Title="Categories" />
    <ChartPrimaryYAxis Title="Values" />
    
    <ChartPalettes>
        <ChartPalette>#0066CC</ChartPalette>
        <ChartPalette>#E67300</ChartPalette>
        <ChartPalette>#00994D</ChartPalette>
        <ChartPalette>#CC0000</ChartPalette>
    </ChartPalettes>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@ComplianceData" XName="Category" YName="Value" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="Accessible Colors">
            <ChartSeriesBorder Width="2" Color="#FFFFFF" />
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<DataPoint> ComplianceData = new List<DataPoint>
    {
        new DataPoint { Category = "A", Value = 45 },
        new DataPoint { Category = "B", Value = 62 },
        new DataPoint { Category = "C", Value = 38 },
        new DataPoint { Category = "D", Value = 71 }
    };
    
    public class DataPoint
    {
        public string Category { get; set; }
        public double Value { get; set; }
    }
}
```

### Color-Blind Friendly Palettes

Use palettes designed for various types of color blindness.

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="Color-Blind Friendly Chart">
    <ChartPrimaryXAxis Title="Years" />
    <ChartPrimaryYAxis Title="Growth Rate %" />
    
    <ChartPalettes>
        <ChartPalette>#0173B2</ChartPalette>
        <ChartPalette>#DE8F05</ChartPalette>
        <ChartPalette>#029E73</ChartPalette>
        <ChartPalette>#CC78BC</ChartPalette>
        <ChartPalette>#CA9161</ChartPalette>
    </ChartPalettes>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@SeriesA" XName="Year" YName="Rate" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line" Name="Product A">
            <ChartMarker Visible="true" Height="8" Width="8" Shape="ChartShape.Circle" />
        </ChartSeries>
        <ChartSeries DataSource="@SeriesB" XName="Year" YName="Rate" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line" Name="Product B">
            <ChartMarker Visible="true" Height="8" Width="8" Shape="ChartShape.Triangle" />
        </ChartSeries>
        <ChartSeries DataSource="@SeriesC" XName="Year" YName="Rate" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Line" Name="Product C">
            <ChartMarker Visible="true" Height="8" Width="8" Shape="ChartShape.Diamond" />
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<RateData> SeriesA = new List<RateData>
    {
        new RateData { Year = 2021, Rate = 5.2 },
        new RateData { Year = 2022, Rate = 6.5 },
        new RateData { Year = 2023, Rate = 7.8 }
    };
    
    public List<RateData> SeriesB = new List<RateData>
    {
        new RateData { Year = 2021, Rate = 4.8 },
        new RateData { Year = 2022, Rate = 5.9 },
        new RateData { Year = 2023, Rate = 6.2 }
    };
    
    public List<RateData> SeriesC = new List<RateData>
    {
        new RateData { Year = 2021, Rate = 6.1 },
        new RateData { Year = 2022, Rate = 7.3 },
        new RateData { Year = 2023, Rate = 8.5 }
    };
    
    public class RateData
    {
        public int Year { get; set; }
        public double Rate { get; set; }
    }
}
```

### Contrast Ratios

Maintain proper contrast ratios between chart elements.

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="High Contrast Chart" Background="#FFFFFF">
    <ChartPrimaryXAxis Title="Months">
        <ChartAxisMajorGridLines Color="#CCCCCC" />
        <ChartAxisMinorGridLines Color="#EEEEEE" />
    </ChartPrimaryXAxis>
    
    <ChartPrimaryYAxis Title="Revenue">
        <ChartAxisMajorGridLines Color="#CCCCCC" />
    </ChartPrimaryYAxis>
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@ContrastData" XName="Month" YName="Revenue" 
                     Fill="#0066CC" Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="Revenue">
            <ChartSeriesBorder Width="2" Color="#003366" />
            <ChartMarker>
                <ChartDataLabel Visible="true" Position="LabelPosition.Top">
                    <ChartDataLabelFont Color="#000000" FontWeight="600" />
                </ChartDataLabel>
            </ChartMarker>
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<MonthlyRevenue> ContrastData = new List<MonthlyRevenue>
    {
        new MonthlyRevenue { Month = "Jan", Revenue = 32000 },
        new MonthlyRevenue { Month = "Feb", Revenue = 28000 },
        new MonthlyRevenue { Month = "Mar", Revenue = 35000 }
    };
    
    public class MonthlyRevenue
    {
        public string Month { get; set; }
        public double Revenue { get; set; }
    }
}
```

### Pattern Alternatives

Provide pattern fills as alternatives to color coding.

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="Pattern-Based Chart">
    <ChartPrimaryXAxis Title="Segments" />
    <ChartPrimaryYAxis Title="Market Share %" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@PatternData" XName="Segment" YName="Share" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="Market Share">
            <ChartSeriesBorder Width="2" Color="#000000" />
        </ChartSeries>
    </ChartSeriesCollection>
    
    <ChartSelectionSettings Enable="true">
        <ChartSelectionPattern>
            <ChartSelectionPattern Type="SelectionPattern.Dots" />
            <ChartSelectionPattern Type="SelectionPattern.DiagonalForward" />
            <ChartSelectionPattern Type="SelectionPattern.Crosshatch" />
        </ChartSelectionPattern>
    </ChartSelectionSettings>
</SfChart>

@code {
    public List<MarketData> PatternData = new List<MarketData>
    {
        new MarketData { Segment = "Segment A", Share = 35.5 },
        new MarketData { Segment = "Segment B", Share = 28.2 },
        new MarketData { Segment = "Segment C", Share = 36.3 }
    };
    
    public class MarketData
    {
        public string Segment { get; set; }
        public double Share { get; set; }
    }
}
```

---

## Responsive Accessibility

### Touch Target Sizing

Ensure touch targets meet minimum size requirements (44x44 pixels).

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="Touch-Optimized Chart">
    <ChartPrimaryXAxis Title="Products" />
    <ChartPrimaryYAxis Title="Sales" />
    
    <ChartLegendSettings Visible="true" Height="50" Width="150" 
                         Padding="10" ShapeHeight="15" ShapeWidth="15" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@TouchData" XName="Product" YName="Sales" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="Q1 Sales">
            <ChartMarker Visible="true" Height="12" Width="12" 
                         Shape="ChartShape.Circle" />
        </ChartSeries>
    </ChartSeriesCollection>
    
    <ChartTooltipSettings Enable="true" />
</SfChart>

@code {
    public List<TouchDataPoint> TouchData = new List<TouchDataPoint>
    {
        new TouchDataPoint { Product = "A", Sales = 120 },
        new TouchDataPoint { Product = "B", Sales = 95 },
        new TouchDataPoint { Product = "C", Sales = 145 }
    };
    
    public class TouchDataPoint
    {
        public string Product { get; set; }
        public double Sales { get; set; }
    }
}
```

### Mobile Accessibility

Optimize charts for mobile devices and touch interactions.

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="Mobile Accessible Chart" Height="400px">
    <ChartPrimaryXAxis Title="Months" />
    <ChartPrimaryYAxis Title="Revenue" />
    
    <ChartZoomSettings EnablePinchZooming="true" 
                       EnableSelectionZooming="false">
    </ChartZoomSettings>
    
    <ChartTooltipSettings Enable="true" EnableAnimation="true" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@MobileData" XName="Month" YName="Revenue" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Area" Opacity="0.6" Name="Revenue">
            <ChartMarker Visible="true" Height="10" Width="10" />
        </ChartSeries>
    </ChartSeriesCollection>
</SfChart>

@code {
    public List<MobileDataPoint> MobileData = new List<MobileDataPoint>
    {
        new MobileDataPoint { Month = "Jan", Revenue = 28 },
        new MobileDataPoint { Month = "Feb", Revenue = 35 },
        new MobileDataPoint { Month = "Mar", Revenue = 42 }
    };
    
    public class MobileDataPoint
    {
        public string Month { get; set; }
        public double Revenue { get; set; }
    }
}
```

### Adaptive Features

Implement features that adapt to different screen sizes and orientations.

```razor
@using Syncfusion.Blazor.Charts

<SfChart Title="Adaptive Layout Chart" Height="@ChartHeight">
    <ChartPrimaryXAxis Title="Categories" />
    <ChartPrimaryYAxis Title="Values" />
    
    <ChartLegendSettings Visible="true" 
                         Position="@(IsSmallScreen ? LegendPosition.Bottom : LegendPosition.Right)" />
    
    <ChartSeriesCollection>
        <ChartSeries DataSource="@AdaptiveData" XName="Category" YName="Value" 
                     Type="Syncfusion.Blazor.Charts.ChartSeriesType.Column" Name="Data Series" />
    </ChartSeriesCollection>
</SfChart>

@code {
    private bool IsSmallScreen = false;
    private string ChartHeight = "400px";
    
    protected override void OnInitialized()
    {
        // Simulate responsive behavior
        IsSmallScreen = true; // Set based on actual screen detection
        ChartHeight = IsSmallScreen ? "300px" : "500px";
    }
    
    public List<AdaptiveDataPoint> AdaptiveData = new List<AdaptiveDataPoint>
    {
        new AdaptiveDataPoint { Category = "A", Value = 65 },
        new AdaptiveDataPoint { Category = "B", Value = 78 },
        new AdaptiveDataPoint { Category = "C", Value = 55 }
    };
    
    public class AdaptiveDataPoint
    {
        public string Category { get; set; }
        public double Value { get; set; }
    }
}
```

---

## Testing and Validation

### Accessibility Testing Tools

Recommended tools for validating chart accessibility:

- **axe DevTools**: Browser extension for WCAG compliance checking
- **WAVE**: Web accessibility evaluation tool
- **NVDA/JAWS**: Screen reader testing
- **Lighthouse**: Chrome DevTools accessibility audit
- **Pa11y**: Automated accessibility testing

### Compliance Checklists

**WCAG 2.2 Level AA Checklist**

- [ ] All images have alt text
- [ ] Color contrast ratio ≥ 4.5:1 for normal text
- [ ] Color contrast ratio ≥ 3:1 for large text
- [ ] Information not conveyed by color alone
- [ ] Keyboard navigation fully functional
- [ ] Focus indicators visible
- [ ] Touch targets ≥ 44x44 pixels
- [ ] Content readable at 200% zoom
- [ ] Screen reader announces all elements
- [ ] RTL support for applicable languages

### Common Issues and Fixes

**Issue 1: Low Contrast Data Labels**

```razor
<!-- Fix: Use high contrast colors for data labels -->
<ChartMarker>
    <ChartDataLabel Visible="true">
        <ChartDataLabelFont Color="#000000" FontWeight="600" />
        <ChartDataLabelBorder Width="1" Color="#FFFFFF" />
    </ChartDataLabel>
</ChartMarker>
```

**Issue 2: Missing ARIA Labels**

```razor
<!-- Fix: Add descriptive titles and labels -->
<SfChart Title="Quarterly Sales Report" 
         Description="Column chart showing sales data for each quarter">
    <ChartPrimaryXAxis Title="Quarter" />
    <ChartPrimaryYAxis Title="Sales in USD" />
</SfChart>
```

**Issue 3: Small Touch Targets**

```razor
<!-- Fix: Increase marker and legend sizes -->
<ChartLegendSettings Visible="true" ShapeHeight="15" ShapeWidth="15" />
<ChartMarker Visible="true" Height="12" Width="12" />
```

**Issue 4: Poor Keyboard Navigation**

```razor
<!-- Fix: Enable proper selection and zoom features -->
<ChartSelectionSettings Enable="true" Mode="Syncfusion.Blazor.Charts.SelectionMode.Point" />
<ChartZoomSettings EnableSelectionZooming="true" />
```

**Best Practices Summary**

1. Always provide text alternatives for visual information
2. Use sufficient color contrast (minimum 4.5:1)
3. Support keyboard navigation for all interactive elements
4. Test with screen readers regularly
5. Implement RTL support for applicable locales
6. Ensure touch targets meet minimum size requirements
7. Provide pattern alternatives to color coding
8. Use semantic HTML and proper ARIA attributes
9. Test across multiple devices and browsers
10. Validate with automated accessibility tools

---

**Compliance Standards Met:**
- WCAG 2.2 Level AA
- Section 508
- ADA (Americans with Disabilities Act)
- EN 301 549 (European Standard)
- ARIA 1.2 Specification
