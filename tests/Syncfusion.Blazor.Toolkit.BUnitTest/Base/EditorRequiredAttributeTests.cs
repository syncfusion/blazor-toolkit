// Copyright © 2001-2026 Syncfusion Inc. All rights reserved.
// Licensed under the MIT license. See LICENSE in the repository root for full license information.

using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Toolkit.Buttons;
using Syncfusion.Blazor.Toolkit.Calendars;
using Syncfusion.Blazor.Toolkit.Charts;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Popups;
using Syncfusion.Blazor.Toolkit.Spinner;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.BUnitTest.Base;

/// <summary>
/// D4 / BEQ-10 enforcement: every public <see cref="ParameterAttribute"/>
/// declared by an <c>Sf*</c> component whose XML documentation marks the
/// parameter as required (note contains the word "required") must also
/// carry <see cref="EditorRequiredAttribute"/>.
/// </summary>
public sealed class EditorRequiredAttributeTests
{
    [Fact]
    public void ChartSeries_RequiredAxisFiltersUseEditorRequired()
    {
        AssertMarkedWithEditorRequired(typeof(ChartSeries));
        AssertMarkedWithEditorRequired(typeof(Sorting));
    }

    [Fact]
    public void Buttons_ComponentParametersHaveValidAttribute()
    {
        // Regression guard: a future author may not quietly drop [EditorRequired]
        // from a `[Parameter]` that is documented as required.
        foreach (var type in new[] { typeof(SfButton), typeof(SfButtonGroup) })
        {
            EnsureNoMissingEditorRequired(type);
        }
    }

    [Fact]
    public void Calendars_ComponentParametersHaveValidAttribute()
    {
        foreach (var type in new[]
                 {
                     typeof(SfCalendar<>),
                     typeof(SfDatePicker<>),
                     typeof(SfDateTimePicker<>),
                     typeof(SfTimePicker<>)
                 })
        {
            EnsureNoMissingEditorRequired(type);
        }
    }

    [Fact]
    public void Inputs_ComponentParametersHaveValidAttribute()
    {
        foreach (var type in new[]
                 {
                     typeof(SfCheckBox),
                     typeof(SfRadioButton),
                     typeof(SfSwitch),
                     typeof(SfTextBox),
                     typeof(SfTextArea),
                     typeof(SfNumericTextBox<>),
                     typeof(SfUploader)
                 })
        {
            EnsureNoMissingEditorRequired(type);
        }
    }

    [Fact]
    public void Popups_ComponentParametersHaveValidAttribute()
    {
        foreach (var type in new[] { typeof(SfDialog), typeof(SfTooltip) })
        {
            EnsureNoMissingEditorRequired(type);
        }
    }

    [Fact]
    public void Spinner_ComponentParametersHaveValidAttribute()
    {
        EnsureNoMissingEditorRequired(typeof(SfSpinner));
    }

    private static void AssertMarkedWithEditorRequired(Type type)
    {
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var parameter = prop.GetCustomAttribute<ParameterAttribute>();
            if (parameter is null)
            {
                continue;
            }

            var doc = prop.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty;
            if (!doc.Contains("required", System.StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            Assert.True(
                prop.IsDefined(typeof(EditorRequiredAttribute), inherit: false),
                $"{type.FullName}.{prop.Name} lacks [EditorRequired] although its docs mark the parameter as required.");
        }
    }

    private static void EnsureNoMissingEditorRequired(Type type) => AssertMarkedWithEditorRequired(type);
}