---
name: syncfusion-blazor-toolkit-buttons
description: Implement interactive Blazor button components with Syncfusion Toolkit. Covers SfButton, SfButtonGroup with styling and accessibility.
compatibility: .NET Core 3.1+, .NET 5+, .NET 6+, .NET 7+, .NET 8+
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit Buttons

## Component Overview

The Syncfusion Blazor Toolkit provides a comprehensive suite of button components for building interactive user interfaces. From basic buttons to button groups, these components support rich content, events, accessibility, and extensive customization.

**Components Included:**
- **SfButton** — Core interactive button component
- **SfButtonGroup** — Group multiple buttons with selection modes

When you need interactive buttons, use the appropriate component from this skill based on your UI requirements.

---

## Documentation and Navigation Guide

### Getting Started
📄 **Read:** [references/getting-started.md](references/getting-started.md)
- Installation and package setup
- Adding button component to your project
- First button implementation
- Basic click event handling
- Content property usage
- Testing in samples

**When to read:** Start here for your first button component or to understand project setup.

### Button Fundamentals
📄 **Read:** [references/button-fundamentals.md](references/button-fundamentals.md)
- Button states (enabled, disabled)
- Disabled state implementation
- Primary vs standard button styling
- CSS class combinations
- HTML attributes capture
- Styling with CssClass property
- Common styling patterns

**When to read:** Need to style buttons, set disabled state, or apply custom CSS classes.

### Icons and Content
📄 **Read:** [references/icons-and-content.md](references/icons-and-content.md)
- Icon CSS classes and icon library
- Icon positioning (left, right, top, bottom)
- Content vs ChildContent properties
- Complex content patterns
- SVG and custom icons
- Icon-only buttons
- Practical icon examples

**When to read:** Want to add icons, position them, or create complex button content.

### Events and Callbacks
📄 **Read:** [references/events-and-callbacks.md](references/events-and-callbacks.md)
- Click event handling
- Created lifecycle event
- EventCallback usage
- Async event handling patterns
- State management with events
- Two-way binding
- Debugging event issues

**When to read:** Need to handle button clicks, lifecycle events, or implement event-driven logic.

### Button Groups
📄 **Read:** [references/button-group.md](references/button-group.md)
- SfButtonGroup component
- Selection modes (single, multiple)
- SelectedChanged event
- Button child component
- Default selection
- Real-world patterns

**When to read:** Need to group buttons or implement selection patterns (radio group, multi-select toolbar).

---

## Quick Start Example

### Basic Button
```razor
<SfButton Content="Click Me" />
```

### Button with Click Handler
```razor
<SfButton Content="Submit" OnClick="OnClickHandler" />

@code {
    private void OnClickHandler(MouseEventArgs args)
    {
        Console.WriteLine("Button clicked!");
    }
}
```

### Button with Icon
```razor
<SfButton Content="Add" IconCss="e-icons e-add" IconPosition="IconPosition.Left" />
```

### Button with Form Integration
```razor
<SfButton Type="ButtonType.Submit" Content="Submit Form" IsPrimary="true" OnClick="SubmitForm" />
<SfButton Type="ButtonType.Reset" Content="Clear Form" />

@code {
    private async Task SubmitForm(MouseEventArgs args)
    {
        // Handle form submission logic
        Console.WriteLine("Form submitted!");
    }
}
```

### Primary Button with Event
```razor
<SfButton Content="Save" IsPrimary="true" OnClick="SaveData" />

@code {
    private async Task SaveData(MouseEventArgs args)
    {
        // Handle save logic
    }
}
```

---

## Common Patterns

### Pattern 1: Disabled State Management
```razor
<SfButton Content="Submit" Disabled="@isProcessing" OnClick="Submit" />

@code {
    private bool isProcessing = false;
    
    private async Task Submit(MouseEventArgs args)
    {
        isProcessing = true;
        await Task.Delay(2000); // Simulate work
        isProcessing = false;
    }
}
```

### Pattern 2: Icon Positioning
```razor
<!-- Icon on the left -->
<SfButton IconCss="e-icons e-edit" IconPosition="IconPosition.Left" Content="Edit" />

<!-- Icon on top -->
<SfButton IconCss="e-icons e-delete" IconPosition="IconPosition.Top" Content="Delete" />

<!-- Icon only -->
<SfButton IconCss="e-icons e-search" />
```

### Pattern 3: Button Group Selection
```razor
<SfButtonGroup Mode="@SelectionMode.Multiple">
    <Button>Bold</Button>
    <Button>Italic</Button>
    <Button>Underline</Button>
</SfButtonGroup>
```

---

## Key Properties Summary

| Component | Key Property | Purpose | Type | Default |
|-----------|--------------|---------|------|---------|
| SfButton | Content | Button text | string | "" |
| SfButton | Disabled | Enable/disable | bool | false |
| SfButton | IsPrimary | Primary styling | bool | false |
| SfButton | IconCss | Icon classes | string | "" |
| SfButton | IconPosition | Icon placement | IconPosition | Left |
| SfButton | CssClass | Custom CSS | string | "" |
| SfButton | Type | Form button type | ButtonType | Button |
| SfButton | HtmlAttributes | Capture HTML attributes | Dictionary<string, object> | {} |
| SfButtonGroup | Mode | Selection type | SelectionMode | None |
| SfButtonGroup | IsVertical | Vertical layout | bool | false |

---

## Button Component Decision Tree

```
Need a button?
├─ Simple clickable button?
│  └─ Use SfButton with Content and OnClick
└─ Multiple related buttons?
   └─ Use SfButtonGroup with Mode
```

---

## Accessibility Considerations

All button components support accessibility best practices:

- **Keyboard Navigation:** All buttons are keyboard accessible via Tab, Enter, and Space
- **ARIA Support:** Proper ARIA attributes are applied automatically
- **Screen Reader Support:** Buttons announce their content and state to screen readers
- **Focus Indicators:** Clear focus indicators for keyboard navigation
- **Disabled State:** Disabled buttons are properly announced and not focusable

When implementing custom HTML attributes or complex content, ensure you maintain these accessibility standards.

---

## Next Steps

1. **Start Simple:** Read [getting-started.md](references/getting-started.md) to create your first button
2. **Style and Configure:** Explore [button-fundamentals.md](references/button-fundamentals.md) for styling options
3. **Add Interactivity:** Learn event handling in [events-and-callbacks.md](references/events-and-callbacks.md)
4. **Expand:** Explore [button-group.md](references/button-group.md) for grouping buttons

**Documentation:** Syncfusion Blazor Toolkit official documentation at https://www.syncfusion.com/blazor-components/
