---
name: syncfusion-blazor-toolkit-notifications
description: Implement loading indicator components in Syncfusion Blazor Toolkit. Covers Spinner (activity indicators) with customization, animation effects, accessibility, and real-world patterns for indicating loading states.
compatibility: .NET 8+, Blazor WebAssembly/Server
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit: Notifications & Loading Indicators

The Notifications components provide visual feedback during asynchronous operations and content loading scenarios. This skill guides you through implementing the **Spinner** component for indicating background processing.

## Component Overview

### SfSpinner Component
The **SfSpinner** component displays a rotating activity indicator to show that a process is ongoing. It's lightweight, customizable, and fully accessible.

- **Use Case:** Indicate background operations, form submissions, API calls
- **Best For:** Quick loading indicators, overlay states, modal dialogs
- **Customization:** Size, color, label, templates

---

## Documentation and Navigation Guide

### Spinner Implementation
📄 **Read:** [references/spinner-implementation.md](references/spinner-implementation.md)
- Basic Spinner setup
- Visibility and binding
- Label text and accessibility
- CSS class customization
- Child content support
- Size and positioning

### Spinner Customization & Events
📄 **Read:** [references/spinner-events-customization.md](references/spinner-events-customization.md)
- Event callbacks: Created, OnOpen, OnClose, Destroyed
- SpinnerEventArgs and Cancel functionality
- Template-based customization
- Custom styling and themes
- Performance considerations

### Accessibility & Best Practices
📄 **Read:** [references/accessibility-best-practices.md](references/accessibility-best-practices.md)
- ARIA labels and screen reader support
- Keyboard navigation
- WCAG 2.1 AA compliance
- Semantic HTML
- Common pitfalls and solutions
- Testing accessibility

---

## Quick Start Examples

### Basic Spinner
```csharp
@if (isLoading)
{
    <SfSpinner @bind-Visible="@isLoading" Label="Loading data..."></SfSpinner>
}

@code {
    private bool isLoading = true;
}
```

### Spinner with Events
```csharp
<SfSpinner @bind-Visible="@showSpinner" 
           Label="Processing..."
           OnOpen="@HandleOpen"
           OnClose="@HandleClose">
</SfSpinner>

@code {
    private bool showSpinner = false;

    private async Task HandleOpen(SpinnerEventArgs args)
    {
        // Execute before spinner opens
    }

    private async Task HandleClose(SpinnerEventArgs args)
    {
        // Execute before spinner closes
    }
}
```

---

## Common Implementation Patterns

### Pattern 1: Form Submission with Spinner Overlay
Show spinner while form is being processed.

```csharp
<div class="form-container">
    <EditForm Model="formData" OnSubmit="@HandleSubmit">
        <InputText @bind-Value="formData.Name" placeholder="Enter name" />
        <button type="submit" disabled="@isSubmitting">Submit</button>
    </EditForm>
    
    @if (isSubmitting)
    {
        <SfSpinner @bind-Visible="@isSubmitting" Label="Submitting form..."></SfSpinner>
    }
</div>

@code {
    private FormModel formData = new();
    private bool isSubmitting = false;

    private async Task HandleSubmit()
    {
        isSubmitting = true;
        try
        {
            // Simulate API call
            await Task.Delay(2000);
        }
        finally
        {
            isSubmitting = false;
        }
    }

    private class FormModel
    {
        public string Name { get; set; }
    }
}
```

---

## Key Properties Summary

### SfSpinner Key Properties
| Property | Type | Default | Purpose |
|----------|------|---------|---------|
| `Visible` | bool | false | Show/hide the spinner |
| `Label` | string? | null | Display text below spinner |
| `CssClass` | string? | null | Custom CSS classes |
| `ChildContent` | RenderFragment? | null | Nested content within spinner |
| `Size` | string? | null | Spinner size (e.g., "24", "36px", "48") |
| `Thickness` | string? | null | Stroke width (e.g., "2", "4", "6px") |
| `ZIndex` | string | "auto" | CSS z-index for stacking order |
| `VisibleChanged` | EventCallback<bool> | - | Two-way binding for Visible |

### SfSpinner Key Events
| Event | Callback Type | Purpose |
|-------|--------------|---------|
| `Created` | EventCallback<object> | Fired after spinner is rendered |
| `OnOpen` | EventCallback<SpinnerEventArgs> | Before spinner shows (can cancel via args.Cancel) |
| `OnClose` | EventCallback<SpinnerEventArgs> | Before spinner hides (can cancel via args.Cancel) |
| `Destroyed` | EventCallback<object> | After spinner is removed |

---

## Common Use Cases

1. **Form Processing**: Indicate ongoing submission with spinner
2. **Async Operations**: Spinner for API calls, calculations
3. **Real-time Updates**: Toggle spinner state during live refresh
4. **Modal/Dialog Loading**: Display spinner during background operations
5. **Overlay Patterns**: Show spinner over content during processing

---

## Next Steps

Start with [Spinner Implementation](references/spinner-implementation.md) to understand basic usage, then explore specific features based on your use case.

**For Spinner configuration:** Focus on [Spinner Customization & Events](references/spinner-events-customization.md)  
**For advanced usage:** See [Accessibility & Best Practices](references/accessibility-best-practices.md)
