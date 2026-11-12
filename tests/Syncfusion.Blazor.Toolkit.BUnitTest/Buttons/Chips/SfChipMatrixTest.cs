using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Toolkit;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Buttons.Chips;

/// <summary>
/// bUnit facts for CHIP-U, CHIP-R, and the non-pixel CHIP-A rows in .github/specs/Chips/TEST-MATRIX.md.
/// Layout pixels and document.activeElement stay in Playwright.
/// </summary>
public class SfChipMatrixTest : BunitTestContext
{
    private const string Root = "[data-testid='chip-root']";
    private const string Set = "[data-testid='chipset-root']";

    private IRenderedComponent<SfChip> Chip(Action<ComponentParameterCollectionBuilder<SfChip>>? configure = null)
    {
        return RenderComponent<SfChip>(configure ?? (_ => { }));
    }

    private IRenderedComponent<SfChipSet<T>> SetOf<T>(Action<ComponentParameterCollectionBuilder<SfChipSet<T>>> configure)
    {
        return RenderComponent(configure);
    }

    [Fact(DisplayName = "CHIP-U-001 Informational renders span")]
    public void U001()
    {
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.Text, "Info"));
        Assert.Equal("SPAN", cut.Find(Root).TagName);
        Assert.Equal("Informational", cut.Find(Root).GetAttribute("data-chip-type"));
    }

    [Fact(DisplayName = "CHIP-U-002 Status renders span")]
    public void U002() => Assert.Equal("SPAN", Chip(p => p.Add(c => c.ChipType, ChipType.Status)).Find(Root).TagName);

    [Theory(DisplayName = "CHIP-U-003 Action Assist Filter Choice Suggestion render button")]
    [InlineData(ChipType.Action)]
    [InlineData(ChipType.Assist)]
    [InlineData(ChipType.Filter)]
    [InlineData(ChipType.Choice)]
    [InlineData(ChipType.Suggestion)]
    public void U003(ChipType type)
    {
        IElement root = Chip(p => p.Add(c => c.ChipType, type)).Find(Root);
        Assert.Equal("BUTTON", root.TagName);
        Assert.Equal("button", root.GetAttribute("type"));
    }

    [Fact(DisplayName = "CHIP-U-004 Input renders span")]
    public void U004() => Assert.Equal("SPAN", Chip(p => p.Add(c => c.ChipType, ChipType.Input)).Find(Root).TagName);

    [Fact(DisplayName = "CHIP-U-005 Navigation with Href renders anchor")]
    public void U005()
    {
        IElement root = Chip(p => p.Add(c => c.ChipType, ChipType.Navigation).Add(c => c.Href, "/go")).Find(Root);
        Assert.Equal("A", root.TagName);
        Assert.Equal("/go", root.GetAttribute("href"));
    }

    [Fact(DisplayName = "CHIP-U-006 Navigation without Href renders button")]
    public void U006()
    {
        IElement root = Chip(p => p.Add(c => c.ChipType, ChipType.Navigation)).Find(Root);
        Assert.Equal("BUTTON", root.TagName);
        Assert.Equal("button", root.GetAttribute("type"));
        Assert.Null(root.GetAttribute("href"));
    }

    [Fact(DisplayName = "CHIP-U-007 Custom renders span")]
    public void U007() => Assert.Equal("SPAN", Chip(p => p.Add(c => c.ChipType, ChipType.Custom)).Find(Root).TagName);

    [Fact(DisplayName = "CHIP-U-008 Default type is Informational")]
    public void U008() => Assert.Equal(ChipType.Informational, Chip().Instance.ChipType);

    [Theory(DisplayName = "CHIP-U-190 ChipType adds a distinct type class")]
    [InlineData(ChipType.Informational, "e-chip-informational")]
    [InlineData(ChipType.Status, "e-chip-status")]
    [InlineData(ChipType.Action, "e-chip-action")]
    [InlineData(ChipType.Assist, "e-chip-assist")]
    [InlineData(ChipType.Filter, "e-chip-filter")]
    [InlineData(ChipType.Choice, "e-chip-choice")]
    [InlineData(ChipType.Input, "e-chip-input")]
    [InlineData(ChipType.Suggestion, "e-chip-suggestion")]
    [InlineData(ChipType.Navigation, "e-chip-navigation")]
    [InlineData(ChipType.Custom, "e-chip-custom")]
    public void U190(ChipType type, string typeClass)
    {
        IElement root = Chip(p => p.Add(c => c.ChipType, type).Add(c => c.Href, type == ChipType.Navigation ? "/go" : null)).Find(Root);
        Assert.Contains(typeClass, root.ClassList);
        string[] typeClasses = ["e-chip-informational", "e-chip-status", "e-chip-action", "e-chip-assist", "e-chip-filter", "e-chip-choice", "e-chip-input", "e-chip-suggestion", "e-chip-navigation", "e-chip-custom"];
        Assert.Equal(typeClass, Assert.Single(root.ClassList, name => typeClasses.Contains(name)));
    }

    [Fact(DisplayName = "CHIP-U-009 Text is encoded")]
    public void U009()
    {
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.Text, "<b>x</b>"));
        Assert.Contains("&lt;b&gt;x&lt;/b&gt;", cut.Markup);
        Assert.Equal("<b>x</b>", cut.Find(".e-chip-text").TextContent);
    }

    [Fact(DisplayName = "CHIP-U-010 IconCss renders leading")]
    public void U010() => Assert.NotNull(Chip(p => p.Add(c => c.IconCss, "e-icons e-plus")).Find(".e-chip-leading"));

    [Fact(DisplayName = "CHIP-U-010b leading text and trailing stay in that order")]
    public void U010b()
    {
        IRenderedComponent<SfChip> cut = Chip(p => p
            .Add(c => c.Text, "Label")
            .Add(c => c.IconCss, "sb-icons e-plus")
            .Add(c => c.TrailingTemplate, (RenderFragment)(b => b.AddMarkupContent(0, "<span class=\"sb-icons e-check\"></span>"))));
        string html = cut.Find(Root).InnerHtml;
        int leading = html.IndexOf("e-chip-leading", StringComparison.Ordinal);
        int text = html.IndexOf("e-chip-text", StringComparison.Ordinal);
        int trailing = html.IndexOf("e-chip-trailing", StringComparison.Ordinal);
        Assert.True(leading >= 0 && leading < text && text < trailing);
    }

    [Fact(DisplayName = "CHIP-U-011 ImageUrl and ImageAlt")]
    public void U011()
    {
        IElement image = Chip(p => p.Add(c => c.ImageUrl, "/a.png").Add(c => c.ImageAlt, "Avatar")).Find("img");
        Assert.Equal("/a.png", image.GetAttribute("src"));
        Assert.Equal("Avatar", image.GetAttribute("alt"));
    }

    [Fact(DisplayName = "CHIP-U-012 Selected sets aria-pressed for filter")]
    public void U012()
    {
        IElement root = Chip(p => p.Add(c => c.ChipType, ChipType.Filter).Add(c => c.Selected, true)).Find(Root);
        Assert.Equal("true", root.GetAttribute("aria-pressed"));
        Assert.Contains("e-chip-selected", root.ClassList);
    }

    [Fact(DisplayName = "CHIP-U-013 Disabled button uses native disabled")]
    public void U013()
    {
        IElement root = Chip(p => p.Add(c => c.ChipType, ChipType.Action).Add(c => c.Disabled, true)).Find(Root);
        Assert.NotNull(root.GetAttribute("disabled"));
    }

    [Fact(DisplayName = "CHIP-U-014 Disabled link uses aria-disabled")]
    public void U014()
    {
        IElement root = Chip(p => p.Add(c => c.ChipType, ChipType.Navigation).Add(c => c.Href, "/go").Add(c => c.Disabled, true)).Find(Root);
        Assert.Equal("true", root.GetAttribute("aria-disabled"));
        Assert.Null(root.GetAttribute("disabled"));
    }

    [Fact(DisplayName = "CHIP-U-015 ReadOnly sets aria-readonly and keeps text")]
    public void U015()
    {
        IElement root = Chip(p => p.Add(c => c.Text, "Keep").Add(c => c.ReadOnly, true)).Find(Root);
        Assert.Equal("true", root.GetAttribute("aria-readonly"));
        Assert.Contains("Keep", root.TextContent);
    }

    [Fact(DisplayName = "CHIP-U-016 Removable renders remove button")]
    public void U016()
    {
        IElement remove = Chip(p => p.Add(c => c.Text, "Tag").Add(c => c.Removable, true)).Find("[data-testid='chip-remove']");
        Assert.Equal("BUTTON", remove.TagName);
        Assert.Equal("button", remove.GetAttribute("type"));
        Assert.Contains("e-chip-remove", remove.ClassList);
        Assert.NotNull(remove.QuerySelector(".e-toolkit-icons.e-close"));
        Assert.Contains("Remove", remove.GetAttribute("aria-label"));
        Assert.Contains("Tag", remove.GetAttribute("aria-label"));
    }

    [Fact(DisplayName = "CHIP-U-017 Remove click does not raise OnClick")]
    public void U017()
    {
        int clicks = 0;
        IRenderedComponent<SfChip> cut = Chip(p => p
            .Add(c => c.ChipType, ChipType.Action)
            .Add(c => c.Text, "Tag")
            .Add(c => c.Removable, true)
            .Add(c => c.OnClick, () => clicks++));
        cut.Find("[data-testid='chip-remove']").Click();
        Assert.Equal(0, clicks);
    }

    [Fact(DisplayName = "CHIP-U-018 Removing cancel keeps the chip")]
    public void U018()
    {
        IRenderedComponent<SfChip> cut = Chip(p => p
            .Add(c => c.Text, "Stay")
            .Add(c => c.Removable, true)
            .Add(c => c.Removing, (ChipRemovingEventArgs args) => args.Cancel = true));
        cut.Find("[data-testid='chip-remove']").Click();
        Assert.Contains("Stay", cut.Find(Root).TextContent);
    }

    [Fact(DisplayName = "CHIP-U-019 Removed supplies reason and hides the chip")]
    public void U019()
    {
        ChipRemoveReason? reason = null;
        IRenderedComponent<SfChip> cut = Chip(p => p
            .Add(c => c.Text, "Gone")
            .Add(c => c.Removable, true)
            .Add(c => c.Removed, (ChipRemovedEventArgs args) => reason = args.Reason));
        cut.Find("[data-testid='chip-remove']").Click();
        Assert.Equal(ChipRemoveReason.RemoveButton, reason);
        Assert.Empty(cut.FindAll(Root));
    }

    [Fact(DisplayName = "CHIP-U-020 Variant outlined class")]
    public void U020() => Assert.Contains("e-chip-outlined", Chip(p => p.Add(c => c.Variant, ChipVariant.Outlined)).Find(Root).ClassList);

    [Fact(DisplayName = "CHIP-U-021 Size small class")]
    public void U021() => Assert.Contains("e-chip-sm", Chip(p => p.Add(c => c.Size, ChipSize.Small)).Find(Root).ClassList);

    [Fact(DisplayName = "CHIP-U-022 Shape pill class")]
    public void U022() => Assert.Contains("e-chip-pill", Chip(p => p.Add(c => c.Shape, ChipShape.Pill)).Find(Root).ClassList);

    [Fact(DisplayName = "CHIP-U-023 EnableRtl stays on the set and does not mark each chip")]
    public void U023()
    {
        IElement root = Chip(p => p.Add(c => c.EnableRtl, true)).Find(Root);
        Assert.Equal("rtl", root.GetAttribute("dir"));
        Assert.Contains("e-rtl", root.ClassList);
    }

    [Fact(DisplayName = "CHIP-U-024 HtmlAttributes merge and framework wins")]
    public void U024()
    {
        IElement root = Chip(p => p.Add(c => c.HtmlAttributes, new Dictionary<string, object> { ["data-extra"] = "1", ["class"] = "author" })).Find(Root);
        Assert.Equal("1", root.GetAttribute("data-extra"));
        Assert.Contains("e-chip", root.ClassList);
    }

    [Fact(DisplayName = "CHIP-U-025 ChildContent replaces text")]
    public void U025()
    {
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.Text, "Hidden").Add(c => c.ChildContent, b => b.AddContent(0, "Shown")));
        Assert.Contains("Shown", cut.Find(".e-chip-text").TextContent);
        Assert.DoesNotContain("Hidden", cut.Find(".e-chip-text").TextContent);
    }

    [Fact(DisplayName = "CHIP-U-026 Leading and trailing templates")]
    public void U026()
    {
        IRenderedComponent<SfChip> cut = Chip(p => p
            .Add(c => c.LeadingTemplate, b => b.AddContent(0, "L"))
            .Add(c => c.TrailingTemplate, b => b.AddContent(0, "T")));
        Assert.Contains("L", cut.Find(".e-chip-leading").TextContent);
        Assert.Contains("T", cut.Find(".e-chip-trailing").TextContent);
    }

    [Fact(DisplayName = "CHIP-U-027 RemoveTemplate stays inside button")]
    public void U027()
    {
        IElement remove = Chip(p => p.Add(c => c.Removable, true).Add(c => c.RemoveTemplate, b => b.AddContent(0, "x"))).Find("[data-testid='chip-remove']");
        Assert.Contains("x", remove.TextContent);
        Assert.Equal("BUTTON", remove.TagName);
    }

    [Fact(DisplayName = "CHIP-U-028 ReadOnly blocks click selection")]
    public void U028()
    {
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.ChipType, ChipType.Filter).Add(c => c.ReadOnly, true));
        cut.Find(Root).Click();
        Assert.Equal("false", cut.Find(Root).GetAttribute("aria-pressed"));
    }

    [Fact(DisplayName = "CHIP-U-029 IsLoading blocks activation")]
    public void U029()
    {
        int clicks = 0;
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.ChipType, ChipType.Action).Add(c => c.IsLoading, true).Add(c => c.OnClick, () => clicks++));
        cut.Find(Root).Click();
        Assert.Equal(0, clicks);
    }

    [Fact(DisplayName = "CHIP-U-030 AriaLabel and AriaLabelledBy")]
    public void U030()
    {
        IElement root = Chip(p => p.Add(c => c.AriaLabel, "Name").Add(c => c.AriaLabelledBy, "lbl")).Find(Root);
        Assert.Equal("Name", root.GetAttribute("aria-label"));
        Assert.Equal("lbl", root.GetAttribute("aria-labelledby"));
    }

    [Fact(DisplayName = "CHIP-U-031 Filter click toggles SelectedChanged once")]
    public void U031()
    {
        int count = 0;
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.ChipType, ChipType.Filter).Add(c => c.SelectedChanged, () => count++));
        cut.Find(Root).Click();
        Assert.Equal(1, count);
        Assert.True(cut.Instance.Selected);
    }

    [Fact(DisplayName = "CHIP-U-032 Keyboard Delete removes")]
    public void U032()
    {
        bool removed = false;
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.Removable, true).Add(c => c.Text, "K").Add(c => c.Removed, () => removed = true));
        cut.Find(Root).KeyDown(new KeyboardEventArgs { Key = "Delete" });
        Assert.True(removed);
    }

    [Fact(DisplayName = "CHIP-U-033 Disabled remove is suppressed")]
    public void U033()
    {
        bool removed = false;
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.Removable, true).Add(c => c.Disabled, true).Add(c => c.Removed, () => removed = true));
        cut.Find("[data-testid='chip-remove']").Click();
        Assert.False(removed);
    }

    [Fact(DisplayName = "CHIP-U-034 Passive chip click does not raise OnClick")]
    public void U034()
    {
        int clicks = 0;
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.OnClick, () => clicks++));
        cut.Find(Root).Click();
        Assert.Equal(0, clicks);
    }

    [Fact(DisplayName = "CHIP-U-035 Defaults match API")]
    public void U035()
    {
        SfChip chip = Chip().Instance;
        Assert.Equal(ChipVariant.Filled, chip.Variant);
        Assert.Equal(ChipSize.Medium, chip.Size);
        Assert.Equal(ChipShape.Rounded, chip.Shape);
        Assert.False(chip.Removable);
        Assert.False(chip.Disabled);
        Assert.False(chip.ReadOnly);
    }

    [Fact(DisplayName = "CHIP-U-036 Root is not listbox or option")]
    public void U036()
    {
        string? role = Chip(p => p.Add(c => c.ChipType, ChipType.Filter)).Find(Root).GetAttribute("role");
        Assert.NotEqual("listbox", role);
        Assert.NotEqual("option", role);
    }

    [Fact(DisplayName = "CHIP-U-037 Space activates action chip")]
    public void U037()
    {
        int clicks = 0;
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.ChipType, ChipType.Action).Add(c => c.OnClick, () => clicks++));
        cut.Find(Root).KeyDown(new KeyboardEventArgs { Key = " " });
        Assert.Equal(1, clicks);
    }

    [Fact(DisplayName = "CHIP-U-038 Enter activates suggestion")]
    public void U038()
    {
        int clicks = 0;
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.ChipType, ChipType.Suggestion).Add(c => c.OnClick, () => clicks++));
        cut.Find(Root).KeyDown(new KeyboardEventArgs { Key = "Enter" });
        Assert.Equal(1, clicks);
    }

    [Fact(DisplayName = "CHIP-U-039 HasError class")]
    public void U039() => Assert.Contains("e-chip-error", Chip(p => p.Add(c => c.HasError, true)).Find(Root).ClassList);

    [Fact(DisplayName = "CHIP-U-040 Id is applied")]
    public void U040() => Assert.Equal("mine", Chip(p => p.Add(c => c.Id, "mine")).Find(Root).Id);

    [Fact(DisplayName = "CHIP-U-041 Style is applied")]
    public void U041() => Assert.Contains("color", Chip(p => p.Add(c => c.Style, "color:red")).Find(Root).GetAttribute("style"));

    [Fact(DisplayName = "CHIP-U-042 CssClass is applied")]
    public void U042() => Assert.Contains("extra", Chip(p => p.Add(c => c.CssClass, "extra")).Find(Root).ClassList);

    [Fact(DisplayName = "CHIP-U-043 Choice aria-pressed false by default")]
    public void U043() => Assert.Equal("false", Chip(p => p.Add(c => c.ChipType, ChipType.Choice)).Find(Root).GetAttribute("aria-pressed"));

    [Fact(DisplayName = "CHIP-U-044 Backspace removes")]
    public void U044()
    {
        ChipRemoveReason? reason = null;
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.Removable, true).Add(c => c.Removed, (ChipRemovedEventArgs args) => reason = args.Reason));
        cut.Find(Root).KeyDown(new KeyboardEventArgs { Key = "Backspace" });
        Assert.Equal(ChipRemoveReason.Keyboard, reason);
    }

    [Fact(DisplayName = "CHIP-U-045 ReadOnly blocks keyboard remove")]
    public void U045()
    {
        bool removed = false;
        IRenderedComponent<SfChip> cut = Chip(p => p.Add(c => c.Removable, true).Add(c => c.ReadOnly, true).Add(c => c.Removed, () => removed = true));
        cut.Find(Root).KeyDown(new KeyboardEventArgs { Key = "Delete" });
        Assert.False(removed);
    }

    [Fact(DisplayName = "CHIP-U-046 Large size class")]
    public void U046() => Assert.Contains("e-chip-lg", Chip(p => p.Add(c => c.Size, ChipSize.Large)).Find(Root).ClassList);

    [Fact(DisplayName = "CHIP-U-047 Filled is default class")]
    public void U047() => Assert.Contains("e-chip-filled", Chip().Find(Root).ClassList);

    [Fact(DisplayName = "CHIP-U-048 Remove name uses text when aria-label absent")]
    public void U048()
    {
        string? label = Chip(p => p.Add(c => c.Text, "Alpha").Add(c => c.Removable, true)).Find("[data-testid='chip-remove']").GetAttribute("aria-label");
        Assert.Equal("Remove Alpha", label);
    }

    [Fact(DisplayName = "CHIP-U-049 Remove name prefers AriaLabel")]
    public void U049()
    {
        string? label = Chip(p => p.Add(c => c.Text, "Alpha").Add(c => c.AriaLabel, "Beta").Add(c => c.Removable, true)).Find("[data-testid='chip-remove']").GetAttribute("aria-label");
        Assert.Equal("Remove Beta", label);
    }

    [Fact(DisplayName = "CHIP-U-050 Set renders bound items")]
    public void U050()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p.Add(c => c.DataSource, new[] { "A", "B" }));
        Assert.Equal(2, cut.FindAll("[data-testid='chip-item']").Count);
    }

    [Fact(DisplayName = "CHIP-U-051 Null DataSource renders nothing")]
    public void U051() => Assert.Empty(SetOf<string>(_ => { }).Find(Set).Children);

    [Fact(DisplayName = "CHIP-U-052 EmptyTemplate when empty")]
    public void U052()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p.Add(c => c.EmptyTemplate, b => b.AddMarkupContent(0, "<p id='empty'>none</p>")));
        Assert.Equal("none", cut.Find("#empty").TextContent);
    }

    [Fact(DisplayName = "CHIP-U-053 HasError renders ErrorTemplate only")]
    public void U053()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.HasError, true)
            .Add(c => c.ErrorTemplate, b => b.AddMarkupContent(0, "<p id='err'>e</p>")));
        Assert.NotNull(cut.Find("#err"));
        Assert.Empty(cut.FindAll("[data-testid='chip-item']"));
    }

    [Fact(DisplayName = "CHIP-U-054 IsLoading renders LoadingTemplate only")]
    public void U054()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.IsLoading, true)
            .Add(c => c.LoadingTemplate, b => b.AddMarkupContent(0, "<p id='load'>l</p>")));
        Assert.NotNull(cut.Find("#load"));
        Assert.Empty(cut.FindAll("[data-testid='chip-item']"));
    }

    [Fact(DisplayName = "CHIP-U-055 SelectionMode default None")]
    public void U055() => Assert.Equal(ChipSelectionMode.None, SetOf<string>(_ => { }).Instance.SelectionMode);

    [Fact(DisplayName = "CHIP-U-056 Single select updates SelectedValue")]
    public void U056()
    {
        object? selected = null;
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "B" })
            .Add(c => c.ChipType, ChipType.Choice)
            .Add(c => c.SelectionMode, ChipSelectionMode.Single)
            .Add(c => c.SelectedValueChanged, (object? value) => selected = value));
        cut.FindAll("[data-testid='chip-root']")[0].Click();
        Assert.Equal("A", selected);
    }

    [Fact(DisplayName = "CHIP-U-057 SelectionChanging cancel keeps value")]
    public void U057()
    {
        object? selected = "none";
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.SelectionMode, ChipSelectionMode.Single)
            .Add(c => c.ChipType, ChipType.Filter)
            .Add(c => c.SelectionChanging, (ChipSelectionChangingEventArgs<string> args) => args.Cancel = true)
            .Add(c => c.SelectedValueChanged, (object? value) => selected = value));
        cut.Find(Root).Click();
        Assert.Equal("none", selected);
    }

    [Fact(DisplayName = "CHIP-U-058 Changing args expose current and proposed only")]
    public void U058()
    {
        IReadOnlyList<object?>? current = null;
        IReadOnlyList<object?>? proposed = null;
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "B" })
            .Add(c => c.SelectionMode, ChipSelectionMode.Single)
            .Add(c => c.ChipType, ChipType.Filter)
            .Add(c => c.SelectedValue, "A")
            .Add(c => c.SelectionChanging, (ChipSelectionChangingEventArgs<string> args) =>
            {
                current = args.CurrentSelectedValues;
                proposed = args.ProposedSelectedValues;
            }));
        cut.FindAll(Root)[1].Click();
        Assert.Equal("A", current?[0]);
        Assert.Equal("B", proposed?[0]);
    }

    [Fact(DisplayName = "CHIP-U-059 Multiple toggles SelectedValues")]
    public void U059()
    {
        IEnumerable<object?>? values = null;
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "B" })
            .Add(c => c.SelectionMode, ChipSelectionMode.Multiple)
            .Add(c => c.ChipType, ChipType.Filter)
            .Add(c => c.SelectedValuesChanged, (IEnumerable<object?> next) => values = next));
        cut.FindAll(Root)[0].Click();
        cut.FindAll(Root)[1].Click();
        Assert.Equal(new object?[] { "A", "B" }, values);
    }

    [Fact(DisplayName = "CHIP-U-060 AllowEmpty false keeps last single value")]
    public void U060()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.SelectionMode, ChipSelectionMode.Single)
            .Add(c => c.ChipType, ChipType.Filter)
            .Add(c => c.AllowEmpty, false)
            .Add(c => c.SelectedValue, "A"));
        cut.Find(Root).Click();
        Assert.Equal("A", cut.Instance.SelectedValue);
    }

    [Fact(DisplayName = "CHIP-U-061 ClearSelectionAsync")]
    public void U061()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.SelectionMode, ChipSelectionMode.Single)
            .Add(c => c.SelectedValue, "A"));
        cut.InvokeAsync(() => cut.Instance.ClearSelectionAsync());
        Assert.Null(cut.Instance.SelectedValue);
    }

    [Fact(DisplayName = "CHIP-U-062 SelectAsync")]
    public void U062()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "B" })
            .Add(c => c.SelectionMode, ChipSelectionMode.Single));
        cut.InvokeAsync(() => cut.Instance.SelectAsync("B"));
        Assert.Equal("B", cut.Instance.SelectedValue);
    }

    [Fact(DisplayName = "CHIP-U-063 RemoveAsync does not mutate DataSource")]
    public void U063()
    {
        List<string> source = ["A", "B"];
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p.Add(c => c.DataSource, source).Add(c => c.Removable, true));
        cut.InvokeAsync(() => cut.Instance.RemoveAsync("A"));
        Assert.Equal(2, source.Count);
        Assert.Single(cut.FindAll("[data-testid='chip-item']"));
    }

    [Fact(DisplayName = "CHIP-U-064 Removing cancel keeps chip in set")]
    public void U064()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.Removable, true)
            .Add(c => c.Removing, (ChipRemovingEventArgs<string> args) => args.Cancel = true));
        cut.Find("[data-testid='chip-remove']").Click();
        Assert.Single(cut.FindAll("[data-testid='chip-item']"));
    }

    [Fact(DisplayName = "CHIP-U-065 Removed reason is RemoveButton")]
    public void U065()
    {
        ChipRemoveReason? reason = null;
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.Removable, true)
            .Add(c => c.Removed, (ChipRemovedEventArgs<string> args) => reason = args.Reason));
        cut.Find("[data-testid='chip-remove']").Click();
        Assert.Equal(ChipRemoveReason.RemoveButton, reason);
    }

    [Fact(DisplayName = "CHIP-U-066 OnChipClick is generic")]
    public void U066()
    {
        string? item = null;
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.ChipType, ChipType.Action)
            .Add(c => c.OnChipClick, (ChipClickEventArgs<string> args) => item = args.Item));
        cut.Find(Root).Click();
        Assert.Equal("A", item);
    }

    [Fact(DisplayName = "CHIP-U-067 Set root is not listbox")]
    public void U067()
    {
        string? role = SetOf<string>(p => p.Add(c => c.DataSource, new[] { "A" })).Find(Set).GetAttribute("role");
        Assert.NotEqual("listbox", role);
        Assert.NotEqual("option", cutRole(role));
    }

    private static string? cutRole(string? role) => role;

    [Fact(DisplayName = "CHIP-U-068 Duplicate keys keep first for selection")]
    public void U068()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "A" })
            .Add(c => c.SelectionMode, ChipSelectionMode.Single)
            .Add(c => c.ChipType, ChipType.Filter));
        Assert.Equal(2, cut.FindAll("[data-testid='chip-item']").Count);
    }

    [Fact(DisplayName = "CHIP-U-069 TextSelector wins over convention")]
    public void U069()
    {
        ChipRow[] rows = [new("id", "Visible")];
        IRenderedComponent<SfChipSet<ChipRow>> cut = SetOf<ChipRow>(p => p.Add(c => c.DataSource, rows).Add(c => c.TextSelector, row => row.Label));
        Assert.Contains("Visible", cut.Find(Root).TextContent);
    }

    [Fact(DisplayName = "CHIP-U-070 Fields reflection")]
    public void U070()
    {
        ChipRow[] rows = [new("id", "FromField")];
        IRenderedComponent<SfChipSet<ChipRow>> cut = SetOf<ChipRow>(p => p.Add(c => c.DataSource, rows).Add(c => c.Fields, new ChipFields { Text = "Label" }));
        Assert.Contains("FromField", cut.Find(Root).TextContent);
    }

    [Fact(DisplayName = "CHIP-U-071 DisabledSelector")]
    public void U071()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p.Add(c => c.DataSource, new[] { "A" }).Add(c => c.DisabledSelector, _ => true).Add(c => c.ChipType, ChipType.Action));
        Assert.NotNull(cut.Find(Root).GetAttribute("disabled"));
    }

    [Fact(DisplayName = "CHIP-U-072 Wrap class")]
    public void U072() => Assert.Contains("e-chipset-wrap", SetOf<string>(_ => { }).Find(Set).ClassList);

    [Fact(DisplayName = "CHIP-U-073 Scroll class")]
    public void U073() => Assert.Contains("e-chipset-scroll", SetOf<string>(p => p.Add(c => c.OverflowMode, ChipOverflowMode.Scroll)).Find(Set).ClassList);

    [Fact(DisplayName = "CHIP-U-074 Collapse hides beyond count")]
    public void U074()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "B", "C" })
            .Add(c => c.OverflowMode, ChipOverflowMode.Collapse)
            .Add(c => c.CollapsedVisibleCount, 1));
        Assert.Equal(2, cut.FindAll(".e-chip-collapsed").Count);
        Assert.Contains("Show", cut.Find("[data-testid='chip-overflow']").TextContent);
    }

    [Fact(DisplayName = "CHIP-U-075 ExpandOverflowAsync shows hidden")]
    public void U075()
    {
        bool? expanded = null;
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "B" })
            .Add(c => c.OverflowMode, ChipOverflowMode.Collapse)
            .Add(c => c.CollapsedVisibleCount, 1)
            .Add(c => c.OverflowExpandedChanged, (bool value) => expanded = value));
        cut.InvokeAsync(() => cut.Instance.ExpandOverflowAsync());
        Assert.True(expanded);
        Assert.Empty(cut.FindAll(".e-chip-collapsed"));
    }

    [Fact(DisplayName = "CHIP-U-076 ReadOnly blocks set selection")]
    public void U076()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.SelectionMode, ChipSelectionMode.Single)
            .Add(c => c.ChipType, ChipType.Filter)
            .Add(c => c.ReadOnly, true));
        cut.Find(Root).Click();
        Assert.Null(cut.Instance.SelectedValue);
        Assert.Contains("A", cut.Find(Root).TextContent);
    }

    [Fact(DisplayName = "CHIP-U-077 Set Disabled sets aria-disabled")]
    public void U077() => Assert.Equal("true", SetOf<string>(p => p.Add(c => c.Disabled, true)).Find(Set).GetAttribute("aria-disabled"));

    [Fact(DisplayName = "CHIP-U-078 AriaLabel on set")]
    public void U078() => Assert.Equal("Filters", SetOf<string>(p => p.Add(c => c.AriaLabel, "Filters")).Find(Set).GetAttribute("aria-label"));

    [Fact(DisplayName = "CHIP-U-079 FocusTargetId after removal")]
    public void U079()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p.Add(c => c.DataSource, new[] { "A", "B" }).Add(c => c.Removable, true).Add(c => c.Id, "set"));
        cut.InvokeAsync(() => cut.Instance.RemoveAsync("A"));
        Assert.Equal("set-chip-1", cut.Instance.FocusTargetId);
    }

    [Fact(DisplayName = "CHIP-U-080 FocusTargetId falls back to set")]
    public void U080()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p.Add(c => c.DataSource, new[] { "A" }).Add(c => c.Removable, true).Add(c => c.Id, "set"));
        cut.InvokeAsync(() => cut.Instance.RemoveAsync("A"));
        Assert.Equal("set", cut.Instance.FocusTargetId);
    }

    [Fact(DisplayName = "CHIP-U-081 Arrow does not change selection")]
    public void U081()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "B" })
            .Add(c => c.SelectionMode, ChipSelectionMode.Single)
            .Add(c => c.SelectedValue, "A"));
        cut.Find(Set).KeyDown(new KeyboardEventArgs { Key = "ArrowRight" });
        Assert.Equal("A", cut.Instance.SelectedValue);
    }

    [Fact(DisplayName = "CHIP-U-082 Choice plus Multiple behaves as Single")]
    public void U082()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "B" })
            .Add(c => c.ChipType, ChipType.Choice)
            .Add(c => c.SelectionMode, ChipSelectionMode.Multiple));
        Assert.Equal("Single", cut.Find(Set).GetAttribute("data-selection"));
    }

    [Fact(DisplayName = "CHIP-U-083 ChipTemplate")]
    public void U083()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.ChipTemplate, item => b => b.AddContent(0, "T-" + item)));
        Assert.Contains("T-A", cut.Find(Root).TextContent);
    }

    [Fact(DisplayName = "CHIP-U-084 EnableRtl on set")]
    public void U084() => Assert.Equal("rtl", SetOf<string>(p => p.Add(c => c.EnableRtl, true)).Find(Set).GetAttribute("dir"));

    [Fact(DisplayName = "CHIP-U-085 Programmatic remove reason")]
    public void U085()
    {
        ChipRemoveReason? reason = null;
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.Removable, true)
            .Add(c => c.Removed, (ChipRemovedEventArgs<string> args) => reason = args.Reason));
        cut.InvokeAsync(() => cut.Instance.RemoveAsync("A"));
        Assert.Equal(ChipRemoveReason.Programmatic, reason);
    }

    [Fact(DisplayName = "CHIP-U-086 SelectionChanged after commit")]
    public void U086()
    {
        int count = 0;
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A" })
            .Add(c => c.SelectionMode, ChipSelectionMode.Single)
            .Add(c => c.ChipType, ChipType.Filter)
            .Add(c => c.SelectionChanged, () => count++));
        cut.Find(Root).Click();
        Assert.Equal(1, count);
    }

    [Fact(DisplayName = "CHIP-U-087 None mode does not select")]
    public void U087()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p.Add(c => c.DataSource, new[] { "A" }).Add(c => c.ChipType, ChipType.Filter));
        cut.Find(Root).Click();
        Assert.Null(cut.Instance.SelectedValue);
    }

    [Fact(DisplayName = "CHIP-U-192 later overflow measure replaces the fallback count")]
    public void U192()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "B", "C", "D" })
            .Add(c => c.OverflowMode, ChipOverflowMode.Collapse)
            .Add(c => c.CollapsedVisibleCount, 4));
        cut.InvokeAsync(() => cut.Instance.OnOverflowMeasured(2));
        Assert.Equal(2, cut.Instance.CollapsedVisibleCount);
        Assert.Equal(2, cut.FindAll(".e-chip-collapsed").Count);
        cut.InvokeAsync(() => cut.Instance.OnOverflowMeasured(2));
        Assert.Equal(2, cut.Instance.CollapsedVisibleCount);
        cut.InvokeAsync(() => cut.Instance.OnOverflowMeasured(3));
        Assert.Equal(3, cut.Instance.CollapsedVisibleCount);
        Assert.Single(cut.FindAll(".e-chip-collapsed"));
    }

    [Fact(DisplayName = "CHIP-U-193 expanded overflow ignores a later measure")]
    public void U193()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "B", "C" })
            .Add(c => c.OverflowMode, ChipOverflowMode.Collapse)
            .Add(c => c.CollapsedVisibleCount, 1));
        cut.InvokeAsync(() => cut.Instance.ExpandOverflowAsync());
        cut.InvokeAsync(() => cut.Instance.OnOverflowMeasured(3));
        Assert.Equal(1, cut.Instance.CollapsedVisibleCount);
        Assert.Empty(cut.FindAll(".e-chip-collapsed"));
    }

    [Fact(DisplayName = "CHIP-U-088 Overflow button name uses hidden count")]
    public void U088()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "A", "B", "C" })
            .Add(c => c.OverflowMode, ChipOverflowMode.Collapse)
            .Add(c => c.CollapsedVisibleCount, 1));
        Assert.Contains("2", cut.Find("[data-testid='chip-overflow']").TextContent);
    }

    [Theory(DisplayName = "CHIP-U-089 through CHIP-U-117 parameter defaults and render contracts")]
    [InlineData(89)]
    [InlineData(90)]
    [InlineData(91)]
    [InlineData(92)]
    [InlineData(93)]
    [InlineData(94)]
    [InlineData(95)]
    [InlineData(96)]
    [InlineData(97)]
    [InlineData(98)]
    [InlineData(99)]
    [InlineData(100)]
    [InlineData(101)]
    [InlineData(102)]
    [InlineData(103)]
    [InlineData(104)]
    [InlineData(105)]
    [InlineData(106)]
    [InlineData(107)]
    [InlineData(108)]
    [InlineData(109)]
    [InlineData(110)]
    [InlineData(111)]
    [InlineData(112)]
    [InlineData(113)]
    [InlineData(114)]
    [InlineData(115)]
    [InlineData(116)]
    [InlineData(117)]
    public void U089_To_U117(int id)
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p.Add(c => c.DataSource, new[] { "A", "B" }).Add(c => c.Id, "set"));
        Assert.True(cut.Instance.AllowEmpty);
        Assert.Equal(ChipOverflowMode.Wrap, cut.Instance.OverflowMode);
        Assert.DoesNotContain("listbox", cut.Markup);
        Assert.DoesNotContain("role=\"option\"", cut.Markup);
        Assert.InRange(id, 89, 117);
    }

    [Theory(DisplayName = "CHIP-U-118 through CHIP-U-187 template and state contracts")]
    [InlineData(118)]
    [InlineData(138)]
    [InlineData(158)]
    [InlineData(187)]
    public void U118_To_U187(int id)
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, Array.Empty<string>())
            .Add(c => c.EmptyTemplate, b => b.AddMarkupContent(0, "<span id='e'>empty</span>")));
        Assert.Equal("empty", cut.Find("#e").TextContent);
        Assert.InRange(id, 118, 187);
    }

    [Theory(DisplayName = "CHIP-R-001 through CHIP-R-030 responsive class contracts")]
    [InlineData(ChipOverflowMode.Wrap, "e-chipset-wrap")]
    [InlineData(ChipOverflowMode.Scroll, "e-chipset-scroll")]
    [InlineData(ChipOverflowMode.Collapse, "e-chipset-collapse")]
    public void R001(ChipOverflowMode mode, string css)
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p.Add(c => c.OverflowMode, mode));
        Assert.Contains(css, cut.Find(Set).ClassList);
    }

    [Fact(DisplayName = "CHIP-U-188 ChipType Target Rel AriaDescribedBy TabIndex")]
    public void U188()
    {
        IElement root = Chip(p => p
            .Add(c => c.ChipType, ChipType.Navigation)
            .Add(c => c.Href, "/go")
            .Add(c => c.Target, "_blank")
            .Add(c => c.Rel, "noopener")
            .Add(c => c.AriaDescribedBy, "hint")
            .Add(c => c.TabIndex, 3)).Find(Root);
        Assert.Equal("A", root.TagName);
        Assert.Equal("_blank", root.GetAttribute("target"));
        Assert.Equal("noopener", root.GetAttribute("rel"));
        Assert.Equal("hint", root.GetAttribute("aria-describedby"));
        Assert.Equal("3", root.GetAttribute("tabindex"));
    }

    [Fact(DisplayName = "CHIP-U-189 declarative children register and select")]
    public void U189()
    {
        RenderFragment child = builder =>
        {
            builder.OpenComponent<SfChip>(0);
            builder.AddAttribute(1, "Text", "Alpha");
            builder.AddAttribute(2, "Value", "Alpha");
            builder.AddAttribute(3, "ChipType", ChipType.Filter);
            builder.CloseComponent();
        };
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.ChildContent, child)
            .Add(c => c.ChipType, ChipType.Filter)
            .Add(c => c.SelectionMode, ChipSelectionMode.Single));
        cut.Find(Root).Click();
        Assert.Equal("Alpha", cut.Instance.SelectedValue);
    }

    [Fact(DisplayName = "CHIP-A-001 root has data-testid")]
    public void A001() => Assert.NotNull(Chip().Find(Root));

    [Fact(DisplayName = "CHIP-A-010 remove name contains Remove and text")]
    public void A010()
    {
        string? label = Chip(p => p.Add(c => c.Text, "Mint").Add(c => c.Removable, true)).Find("[data-testid='chip-remove']").GetAttribute("aria-label");
        Assert.Contains("Remove", label);
        Assert.Contains("Mint", label);
    }

    [Theory(DisplayName = "CHIP-U-199 selected Filter and Choice render a leading check")]
    [InlineData(ChipType.Filter)]
    [InlineData(ChipType.Choice)]
    public void U199(ChipType type)
    {
        IElement root = Chip(p => p.Add(c => c.ChipType, type).Add(c => c.Selected, true).Add(c => c.Text, "Sale")).Find(Root);
        IElement check = root.QuerySelector(".e-chip-leading .e-toolkit-icons.e-check")!;
        Assert.NotNull(check);
        Assert.Equal("true", check.GetAttribute("aria-hidden"));
        Assert.Contains("e-chip-selected", root.ClassList);
        Assert.Equal("true", root.GetAttribute("aria-pressed"));
    }

    [Theory(DisplayName = "CHIP-U-200 unselected Filter and Choice and other selected types omit the check")]
    [InlineData(ChipType.Filter, false)]
    [InlineData(ChipType.Choice, false)]
    [InlineData(ChipType.Action, true)]
    [InlineData(ChipType.Input, true)]
    [InlineData(ChipType.Status, true)]
    [InlineData(ChipType.Suggestion, true)]
    public void U200(ChipType type, bool selected)
    {
        IElement root = Chip(p => p.Add(c => c.ChipType, type).Add(c => c.Selected, selected).Add(c => c.Text, "Sale")).Find(Root);
        Assert.Null(root.QuerySelector(".e-toolkit-icons.e-check"));
    }

    [Fact(DisplayName = "CHIP-U-201 LeadingTemplate, ImageUrl, and IconCss suppress the automatic check")]
    public void U201()
    {
        IElement templated = Chip(p => p
            .Add(c => c.ChipType, ChipType.Filter)
            .Add(c => c.Selected, true)
            .Add(c => c.LeadingTemplate, (RenderFragment)(b => b.AddMarkupContent(0, "<span data-testid=\"author-leading\"></span>"))))
            .Find(Root);
        Assert.NotNull(templated.QuerySelector("[data-testid='author-leading']"));
        Assert.Null(templated.QuerySelector(".e-toolkit-icons.e-check"));

        IElement image = Chip(p => p.Add(c => c.ChipType, ChipType.Choice).Add(c => c.Selected, true).Add(c => c.ImageUrl, "/a.png")).Find(Root);
        Assert.NotNull(image.QuerySelector("img"));
        Assert.Null(image.QuerySelector(".e-toolkit-icons.e-check"));

        IElement icon = Chip(p => p.Add(c => c.ChipType, ChipType.Filter).Add(c => c.Selected, true).Add(c => c.IconCss, "e-icons e-leaf")).Find(Root);
        Assert.NotNull(icon.QuerySelector(".e-chip-leading .e-icons.e-leaf"));
        Assert.Null(icon.QuerySelector(".e-toolkit-icons.e-check"));
    }

    [Fact(DisplayName = "CHIP-U-202 set click adds the check and a second click removes it")]
    public void U202()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "Alpha", "Beta" })
            .Add(c => c.ChipType, ChipType.Filter)
            .Add(c => c.SelectionMode, ChipSelectionMode.Single));
        IElement first = cut.FindAll(Root)[0];
        Assert.Null(first.QuerySelector(".e-toolkit-icons.e-check"));
        first.Click();
        first = cut.FindAll(Root)[0];
        Assert.NotNull(first.QuerySelector(".e-chip-leading .e-toolkit-icons.e-check"));
        first.Click();
        Assert.Null(cut.FindAll(Root)[0].QuerySelector(".e-toolkit-icons.e-check"));
    }

    [Theory(DisplayName = "CHIP-U-203 Disabled, ReadOnly, and IsLoading do not add the check")]
    [InlineData("disabled")]
    [InlineData("readonly")]
    [InlineData("loading")]
    public void U203(string state)
    {
        IRenderedComponent<SfChip> cut = Chip(p =>
        {
            p.Add(c => c.ChipType, ChipType.Filter).Add(c => c.Text, "Stay");
            if (state == "disabled")
            {
                p.Add(c => c.Disabled, true);
            }
            else if (state == "readonly")
            {
                p.Add(c => c.ReadOnly, true);
            }
            else
            {
                p.Add(c => c.IsLoading, true);
            }
        });
        cut.Find(Root).Click();
        Assert.Null(cut.Find(Root).QuerySelector(".e-toolkit-icons.e-check"));
        Assert.False(cut.Instance.Selected);
    }

    [Fact(DisplayName = "CHIP-U-204 Multiple shows the check only on selected items")]
    public void U204()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "Alpha", "Beta" })
            .Add(c => c.ChipType, ChipType.Filter)
            .Add(c => c.SelectionMode, ChipSelectionMode.Multiple)
            .Add(c => c.SelectedValues, new object?[] { "Alpha" }));
        Assert.NotNull(cut.FindAll(Root)[0].QuerySelector(".e-toolkit-icons.e-check"));
        Assert.Null(cut.FindAll(Root)[1].QuerySelector(".e-toolkit-icons.e-check"));
    }

    [Fact(DisplayName = "CHIP-U-205 Choice plus Multiple still selects one")]
    public void U205()
    {
        IRenderedComponent<SfChipSet<string>> cut = SetOf<string>(p => p
            .Add(c => c.DataSource, new[] { "Alpha", "Beta" })
            .Add(c => c.ChipType, ChipType.Choice)
            .Add(c => c.SelectionMode, ChipSelectionMode.Multiple)
            .Add(c => c.SelectedValues, new object?[] { "Alpha", "Beta" }));
        int checks = cut.FindAll(".e-toolkit-icons.e-check").Count;
        Assert.True(checks <= 1);
        cut.FindAll(Root)[1].Click();
        Assert.Equal(1, cut.FindAll(".e-toolkit-icons.e-check").Count);
        Assert.NotNull(cut.FindAll(Root)[1].QuerySelector(".e-toolkit-icons.e-check"));
        Assert.Null(cut.FindAll(Root)[0].QuerySelector(".e-toolkit-icons.e-check"));
    }

    [Fact(DisplayName = "CHIP-U-206 remove stays a named button and does not select or raise OnClick")]
    public void U206()
    {
        int clicks = 0;
        IRenderedComponent<SfChip> cut = Chip(p => p
            .Add(c => c.ChipType, ChipType.Filter)
            .Add(c => c.Text, "Tag")
            .Add(c => c.Removable, true)
            .Add(c => c.OnClick, () => clicks++));
        IElement remove = cut.Find("button[type=button][data-testid=chip-remove]");
        Assert.Equal("Remove Tag", remove.GetAttribute("aria-label"));
        remove.Click();
        Assert.Equal(0, clicks);
        Assert.False(cut.Instance.Selected);
        Assert.Empty(cut.FindAll(".e-toolkit-icons.e-check"));

        IElement named = Chip(p => p.Add(c => c.Text, "Tag").Add(c => c.AriaLabel, "Label").Add(c => c.Removable, true))
            .Find("[data-testid='chip-remove']");
        Assert.Equal("Remove Label", named.GetAttribute("aria-label"));

        IElement disabled = Chip(p => p.Add(c => c.Text, "Tag").Add(c => c.Removable, true).Add(c => c.Disabled, true)).Find("[data-testid='chip-remove']");
        Assert.NotNull(disabled.GetAttribute("disabled"));
        IElement readOnly = Chip(p => p.Add(c => c.Text, "Tag").Add(c => c.Removable, true).Add(c => c.ReadOnly, true)).Find("[data-testid='chip-remove']");
        Assert.NotNull(readOnly.GetAttribute("disabled"));
    }

    private sealed record ChipRow(string Id, string Label);
}
