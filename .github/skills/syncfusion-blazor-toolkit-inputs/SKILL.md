---
name: syncfusion-blazor-toolkit-inputs
description: Implement Syncfusion Blazor Toolkit input components. Comprehensive guide for TextBox, TextArea, NumericTextBox, Uploader, CheckBox, RadioButton, and Switch. Covers component usage, event handling, validation, styling, accessibility, and real-world patterns.
compatibility: .NET 8+, Blazor WebAssembly/Server
metadata:
  author: "Syncfusion Inc"
  version: "1.0.0"
---

# Syncfusion Blazor Toolkit - Input Components

The Syncfusion Blazor Toolkit provides a comprehensive collection of input components for building rich, interactive forms and data entry experiences. From basic text inputs to file uploaders, the Inputs namespace offers solutions for data input scenarios.

## Component Overview

| Component | Purpose | Use Case |
|-----------|---------|----------|
| **SfTextBox** | Single-line text input | General text entry, search fields, usernames |
| **SfTextArea** | Multi-line text input | Comments, descriptions, feedback, long-form content |
| **SfNumericTextBox<T>** | Numeric input with formatting | Prices, quantities, ages, measurements |
| **SfUploader** | File upload with validation | Document upload, image upload, bulk file handling |
| **SfCheckBox<T>** | Boolean selection (multiple) | Agreement checkboxes, multi-select options |
| **SfRadioButton<T>** | Boolean selection (single) | Exclusive choice options, single selection groups |
| **SfSwitch<T>** | Toggle boolean state | Feature toggles, on/off switches, preferences |

## Documentation and Navigation Guide

### Getting Started
📄 **Read:** [references/getting-started-inputs.md](references/getting-started-inputs.md)
- Installation and package setup
- CSS imports and theming
- Basic component initialization
- Common event patterns
- State management basics

### Basic Input Controls
📄 **Read:** [references/textbox-textarea.md](references/textbox-textarea.md)
- SfTextBox component properties and events
- SfTextArea for multi-line input
- Floating labels and placeholders
- Focus and blur event handling
- Input validation and error states

📄 **Read:** [references/numeric-currency.md](references/numeric-currency.md)
- SfNumericTextBox<T> generic implementation
- Currency and percentage formatting
- Min/Max constraints and step values
- Decimal place handling
- Spin button behavior
- Practical examples with real-world scenarios

### Specialized Input Components
📄 **Read:** [references/uploader.md](references/uploader.md)
- SfUploader complete workflow
- Auto upload and async settings
- Multiple file selection and restrictions
- File type and size validation
- Upload events and progress tracking
- Error handling and retry logic
- Drag-and-drop support

### Selection Controls
📄 **Read:** [references/checkbox-radio-switch.md](references/checkbox-radio-switch.md)
- SfCheckBox<T> states and binding
- SfRadioButton<T> grouping and selection
- SfSwitch<T> toggle behavior
- Label positioning and text
- Change event handling
- Grouped and conditional patterns

### Advanced Features & Best Practices
📄 **Read:** [references/input-accessibility.md](references/input-accessibility.md)
- WCAG compliance principles
- Keyboard navigation across all inputs
- ARIA attributes and label associations
- Focus management and indicators
- Error messaging and validation
- Screen reader support
- Component-specific accessibility notes

## Quick Start Example

### Basic Text Input
```razor
@page "/inputs-demo"
@using Syncfusion.Blazor.Toolkit.Inputs

<h3>Basic Inputs Demo</h3>

<div class="input-group">
    <label for="username">Username:</label>
    <SfTextBox @ref="usernameBox" 
               Placeholder="Enter username" 
               ValueChange="@OnUsernameChange">
    </SfTextBox>
</div>

<div class="input-group">
    <label for="email">Email:</label>
    <SfTextBox Type="InputType.Email" 
               Placeholder="Enter email" 
               ValueChange="@OnEmailChange">
    </SfTextBox>
</div>

<div class="input-group">
    <label for="comments">Comments:</label>
    <SfTextArea Placeholder="Enter your feedback" 
                @bind-Value="@comments" 
                RowCount="4">
    </SfTextArea>
</div>

@code {
    private SfTextBox usernameBox;
    private string username = "";
    private string email = "";
    private string comments = "";

    private void OnUsernameChange(ChangedEventArgs args)
    {
        username = args.Value;
        Console.WriteLine($"Username changed: {username}");
    }

    private void OnEmailChange(ChangedEventArgs args)
    {
        email = args.Value;
        Console.WriteLine($"Email changed: {email}");
    }
}
```

### NumericTextBox with Currency
```razor
<div class="input-group">
    <label for="price">Price:</label>
    <SfNumericTextBox TValue="decimal" 
                      Format="C2" 
                      Min="0" 
                      Max="10000" 
                      Step="0.01m"
                      Placeholder="$0.00"
                      @bind-Value="@price">
    </SfNumericTextBox>
</div>

@code {
    private decimal price = 29.99m;
}
```

### CheckBox and Switch
```razor
<div>
    <SfCheckBox TChecked="bool" @bind-Checked="@rememberMe" Label="Remember me"></SfCheckBox>
</div>

<div style="display: flex; align-items: center; gap: 10px;">
    <label style="margin: 0;">Enable Notifications:</label>
    <SfSwitch @bind-Checked="@notificationsEnabled" OnLabel="ON" OffLabel="OFF" />
</div>

@code {
    private bool rememberMe = false;
    private bool notificationsEnabled = true;
}
```

## Common Patterns Across All Inputs

### 1. Two-Way Data Binding
All input components support two-way binding with `@bind-Value`:
```razor
<SfTextBox @bind-Value="@myText"></SfTextBox>
<SfNumericTextBox TValue="int" @bind-Value="@myNumber"></SfNumericTextBox>
<SfCheckBox TChecked="bool" @bind-Checked="@myBool"></SfCheckBox>

@code {
    private string myText = "";
    private int myNumber = 0;
    private bool myBool = false;
}
```

### 2. Event Handling Pattern
Components emit change events for tracking modifications:
```razor
<SfTextBox ValueChange="@OnValueChanged"></SfTextBox>

@code {
    private void OnValueChanged(ChangedEventArgs args)
    {
        Console.WriteLine($"New value: {args.Value}");
    }
}
```

### 3. Validation and Error States
Use CascadingParameter with EditContext for form validation:
```razor
@using Syncfusion.Blazor.Toolkit.Inputs
@using System.ComponentModel.DataAnnotations

<EditForm Model="@formModel" OnValidSubmit="@HandleSubmit">
    <DataAnnotationsValidator />
    
    <div class="form-group">
        <label>Email:</label>
        <SfTextBox 
            @bind-Value="@formModel.Email" 
            Placeholder="Email">
        </SfTextBox>
        <ValidationMessage For="@(() => formModel.Email)" />
    </div>
    
    <button type="submit">Submit</button>
</EditForm>

@code {
    private LoginModel formModel = new();

    private void HandleSubmit()
    {
        Console.WriteLine($"Email: {formModel.Email}");
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "";
    }
}
```

### 4. Disabled and ReadOnly States
All inputs support disabled and read-only configurations:
```razor
<SfTextBox Disabled="true"></SfTextBox>
<SfNumericTextBox TValue="int?" Readonly="true" Value="42"></SfNumericTextBox>
<SfCheckBox TChecked="bool" Disabled="true" Checked="true"></SfCheckBox>
```

### 5. Event Binding for Multiple Inputs
Handle focus, blur, and input events:
```razor
<SfTextBox OnFocus="@OnFocus" OnBlur="@OnBlur" OnInput="@OnInput"></SfTextBox>

@code {
    private void OnFocus(FocusInEventArgs args)
    {
        Console.WriteLine("Focused");
    }

    private void OnBlur(FocusOutEventArgs args)
    {
        Console.WriteLine("Blurred");
    }

    private void OnInput(InputEventArgs args)
    {
        Console.WriteLine($"Input: {args.Value}");
    }
}
```

## Key Props & Events Summary

### Universal Input Props
- `Disabled`: Disable user input (bool)
- `ReadOnly`: Prevent modification (bool)
- `Placeholder`: Hint text (string)
- `CssClass`: Custom CSS classes (string)
- `HtmlAttributes`: Additional HTML attributes (Dictionary)

### Common Value Props
- `Value`: Current component value (varies by type)
- `ValueChange`: Event fired when value changes (callback)
- `@bind-Value`: Two-way binding helper

### Focus & Blur Events
- `OnFocus`: Fired when component gains focus
- `OnBlur`: Fired when component loses focus

### Input-Specific Props (See references for details)
- **TextBox**: `Type`, `FloatLabelType`
- **TextArea**:  `RowCount`, `ColumnCount`, `MaxLength`
- **NumericTextBox**: `Format`, `Min`, `Max`, `Step`, `Decimals`
- **Uploader**: `AllowedExtensions`, `MaxFileSize`, `AutoUpload`

## Component Selection Guide

**Use SfTextBox when:**
- Need simple text input
- Collecting usernames, passwords, search queries
- Want email/URL/number type validation

**Use SfTextArea when:**
- Accepting multi-line content
- Building comment sections, feedback forms
- Need customizable rows/columns

**Use SfNumericTextBox when:**
- Input must be numeric
- Need currency/percentage formatting
- Require min/max constraints and step values

**Use SfUploader when:**
- Need file upload functionality
- Require file validation (type, size)
- Want progress tracking and error handling

**Use SfCheckBox when:**
- Need multiple independent selections
- Building agreement/terms checkboxes
- Want multiple boolean flags

**Use SfRadioButton when:**
- Need mutually exclusive selection
- Building single-choice option groups
- Want dependent form fields

**Use SfSwitch when:**
- Need simple on/off toggle
- Building preference/feature switches
- Want visual toggle indicator

---

**👉 Start with [references/getting-started-inputs.md](references/getting-started-inputs.md) for installation and basic setup, then navigate to your specific component reference.**
