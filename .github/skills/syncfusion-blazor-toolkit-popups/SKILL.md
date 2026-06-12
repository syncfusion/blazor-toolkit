---
name: syncfusion-blazor-toolkit-popups
description: Implement interactive popup components with Syncfusion Blazor Toolkit. Covers SfDialog (modal windows) and SfTooltip (hover-based help text). Includes positioning, animations, event handling, accessibility, and real-world integration patterns. Use this skill when building modals, confirmation dialogs, contextual menus, informational overlays, or help tooltips in Blazor applications.
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit: Popups

## Component Ecosystem Overview

The Syncfusion Blazor Toolkit provides a comprehensive set of popup components for creating interactive, user-friendly interfaces. These components handle common UI patterns like modal dialogs, context menus, and help tooltips.

### Components in This Skill

| Component | Purpose | Key Use Cases |
|-----------|---------|---|
| **SfDialog** | Modal windows with configurable buttons, drag, resize, and animations | Confirmation dialogs, forms, alerts, full-screen modals |
| **SfTooltip** | Hover-based informational text with positioning and animations | Help text, keyboard shortcut hints, status indicators |

---

## When to Use Each Component

### Dialog (SfDialog)
**Use SfDialog when you need:**
- Modal or modeless dialog windows
- User confirmations ("Are you sure?")
- Form input collection
- Full-screen overlays with controlled interactions
- Drag, resize, and positioning capabilities
- Button actions with event handling
- Custom animation effects

**Example Scenarios:**
- "Save changes?" confirmation before navigation
- Registration or login form popup
- Settings configuration modal
- Alert notification with user acknowledgment

### Tooltip (SfTooltip)
**Use SfTooltip when you need:**
- Hover-based help text
- Brief explanatory messages
- Keyboard shortcut hints
- Icon descriptions
- Status or error messages on hover
- Automatic positioning and arrow indicators

**Example Scenarios:**
- Help icon with explanation on hover
- "Ctrl+S" keyboard shortcut hint
- "Invalid email format" tooltip on input field
- Icon button description tooltip

---

## Documentation and Navigation Guide

### Getting Started with Dialogs
📄 **Read:** [references/dialog-basics.md](references/dialog-basics.md)
- Installation and initial setup
- Creating your first dialog
- Basic dialog configuration
- Dialog types (modal, modeless)
- Animation and effects
- Button configuration
- Basic event handling
- content allowprerender and closeonescape apis
- programatic methods in dialog

### Advanced Dialog Features
📄 **Read:** [references/dialog-advanced.md](references/dialog-advanced.md)
- Dialog positioning and collision handling
- Drag and resize functionality
- Custom templates and content rendering
- Dialog service and provider pattern
- Complex event handling
- Dynamic dialog creation
- State management patterns

### Tooltip Implementation
📄 **Read:** [references/tooltip-implementation.md](references/tooltip-implementation.md)
- Tooltip content configuration
- Position and arrow indicators
- Animation settings
- open modes (hover, focus, click)
- Event handling and lifecycle
- tooltip programmatic methods
- keyboard accessibility
- real world examples

---

## Quick Start Examples

### Quick Dialog Example
```razor
<SfButton @onclick="@OpenDialog">Open Dialog</SfButton>
<SfDialog @bind-Visible="isVisible" 
          Width="500px" 
          Height="auto"
          Header="Confirm Action">
    <DialogTemplates>
        <Content>
            <p>Are you sure you want to proceed?</p>
        </Content>
    </DialogTemplates>
    <DialogButtons>
        <DialogButton Content="OK" IsPrimary="true" OnClick="@OnOkClick"/>
        <DialogButton Content="Cancel" OnClick="@OnCancelClick"/>
    </DialogButtons>
</SfDialog>

@code {
    private bool isVisible = false;
    private void OpenDialog()
    {
        isVisible = true;
    }
    
    private async Task OnOkClick()
    {
        // Handle OK action
        isVisible = false;
    }
    
    private async Task OnCancelClick()
    {
        isVisible = false;
    }
}
```

### Quick Tooltip Example
```razor
<SfTooltip Content="Click to refresh" Position="Position.TopCenter">
    <SfButton Content="Refresh"></SfButton>
</SfTooltip>
```

---

## Common Patterns

### Modal Confirmation Dialog
A reusable pattern for asking user confirmation:
```razor
<SfButton @onclick="@OpenDialog">Open Dialog</SfButton>
<SfDialog @ref="confirmDialog" 
          @bind-Visible="showConfirm"
          IsModal="true"
          Width="400px"
          Header="Confirmation">
    <DialogTemplates>
        <Content>
            <p>@confirmMessage</p>
        </Content>
    </DialogTemplates>
    <DialogButtons>
        <DialogButton Content="Yes" IsPrimary="true" OnClick="@OnConfirmYes"/>
        <DialogButton Content="No" OnClick="@OnConfirmNo"/>
    </DialogButtons>
</SfDialog>

@code {
    private SfDialog confirmDialog;
    private bool showConfirm = false;
    private string confirmMessage = "";
    private Func<Task> onConfirmCallback;
    private void OpenDialog()
    {
        showConfirm = true;
    }
    
    public async Task ShowConfirmation(string message, Func<Task> callback)
    {
        confirmMessage = message;
        onConfirmCallback = callback;
        showConfirm = true;
    }
    
    private async Task OnConfirmYes()
    {
        showConfirm = false;
    }
    
    private void OnConfirmNo()
    {
        showConfirm = false;
    }
}
```

---

## Key Properties and Methods

### SfDialog Key Properties
- `Visible` - Show/hide the dialog
- `Header` - Dialog header text
- `Width` / `Height` - Dialog dimensions
- `IsModal` - Modal vs modeless behavior
- `AllowDragging` - Enable/disable dragging
- `EnableResize` - Enable/disable resizing
- `DialogAnimationSettings` - Animation configuration

### SfTooltip Key Properties
- `Content` - Tooltip text or template
- `Position` - Tooltip position
- `OpensOn` - Trigger mode (Auto, Hover, Click, Focus, Custom)
- `TooltipAnimationSettings` - Animation configuration
- `TipPointerPosition` - Arrow position

---

## Accessibility and Best Practices

✅ **DO:**
- Use semantic HTML within dialogs and popups
- Provide proper ARIA labels for interactive elements
- Ensure keyboard navigation support
- Use sufficient color contrast in tooltips
- Include descriptive headers and footers
- Test with screen readers
- Provide alternative text for images in popups

❌ **DON'T:**
- Overuse dialogs for simple messages (use tooltips)
- Hide critical information in tooltips only
- Create dialogs without close buttons
- Ignore keyboard accessibility
- Create modals that trap focus improperly

---

## Next Steps

1. **Start with basics:** Read [dialog-basics.md](references/dialog-basics.md) to understand core dialog usage
2. **Explore advanced features:** Check [dialog-advanced.md](references/dialog-advanced.md) for complex scenarios
3. **Implement tooltips:** Follow [tooltip-implementation.md](references/tooltip-implementation.md) for help text

For additional help, refer to official Syncfusion documentation or check the referenced files for specific implementation details.
