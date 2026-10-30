using System.Reflection;
using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor.Toolkit;
using Xunit;

namespace Syncfusion.Blazor.Toolkit.Tests.Cards
{
    /// <summary>
    /// bUnit coverage for the SfCard family. Test names carry the TEST-MATRIX id (U-CARD-xxx, I-CARD-xxx).
    /// </summary>
    public class SfCardTest : BunitTestContext
    {
        private const string Root = "[data-testid='card-root']";

        private static RenderFragment Region<TRegion>(string text, int seq = 0) where TRegion : IComponent
        {
            return builder =>
            {
                builder.OpenComponent<TRegion>(seq);
                builder.AddAttribute(seq + 1, "ChildContent", (RenderFragment)(inner => inner.AddContent(seq + 2, text)));
                builder.CloseComponent();
            };
        }

        private static RenderFragment Compose(params RenderFragment[] fragments)
        {
            return builder =>
            {
                foreach (RenderFragment fragment in fragments)
                {
                    builder.AddContent(0, fragment);
                }
            };
        }

        private IRenderedComponent<SfCard> RenderCard(Action<ComponentParameterCollectionBuilder<SfCard>>? configure = null)
        {
            return RenderComponent<SfCard>(configure ?? (_ => { }));
        }

        private static Func<KeyboardEventArgs, bool> GetPreventDefaultPredicate(SfCard component, string attributeName)
        {
            // bUnit serializes these callbacks as markers, so invoke the registered predicate to verify key-specific behavior.
            FieldInfo? attributesField = typeof(SfCard).GetField("_attributes", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(attributesField);

            Dictionary<string, object> attributes = Assert.IsType<Dictionary<string, object>>(attributesField.GetValue(component));
            Assert.True(attributes.TryGetValue(attributeName, out object? predicate), $"The '{attributeName}' prevent-default predicate was not rendered.");
            return Assert.IsType<Func<KeyboardEventArgs, bool>>(predicate);
        }

        // ---------------------------------------------------------------- U-CARD-001 / 019 default render

        [Fact(DisplayName = "U-CARD-001 Default card renders a plain root with the base class only")]
        public void DefaultCard_RendersPlainRoot()
        {
            IElement card = RenderCard().Find(Root);

            Assert.Equal("div", card.TagName.ToLowerInvariant());
            Assert.Equal("e-card", card.ClassName);
            Assert.False(string.IsNullOrWhiteSpace(card.Id));
            Assert.Null(card.GetAttribute("role"));
            Assert.Null(card.GetAttribute("tabindex"));
            Assert.Null(card.GetAttribute("aria-busy"));
            Assert.Null(card.GetAttribute("aria-disabled"));
            Assert.Equal("1", card.GetAttribute("data-elevation"));
            Assert.Equal("true", card.GetAttribute("data-content-padding"));
        }

        [Fact(DisplayName = "U-CARD-019 Filled variant adds no variant class")]
        public void FilledVariant_AddsNoVariantClass()
        {
            IElement card = RenderCard(p => p.Add(c => c.Variant, CardVariant.Filled)).Find(Root);

            Assert.DoesNotContain("e-card-outlined", card.ClassList);
            Assert.DoesNotContain("e-card-elevated", card.ClassList);
            Assert.DoesNotContain("e-card-flat", card.ClassList);
        }

        [Fact(DisplayName = "Card generated id is stable across re-renders")]
        public void GeneratedId_IsStableAcrossRenders()
        {
            IRenderedComponent<SfCard> cut = RenderCard();
            string first = cut.Find(Root).Id;

            cut.SetParametersAndRender(p => p.Add(c => c.Hoverable, true));

            Assert.Equal(first, cut.Find(Root).Id);
        }

        [Fact(DisplayName = "Card uses the supplied Id")]
        public void SuppliedId_IsRendered()
        {
            Assert.Equal("my-card", RenderCard(p => p.Add(c => c.Id, "my-card")).Find(Root).Id);
        }

        // ---------------------------------------------------------------- U-CARD-002 / 017 / 018 regions

        [Fact(DisplayName = "U-CARD-002 / 017 / 018 All regions render in markup order with title, subtitle and separator")]
        public void AllRegions_RenderInOrder()
        {
            RenderFragment content = builder =>
            {
                builder.OpenComponent<SfCardHeader>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(header =>
                {
                    header.OpenComponent<SfCardTitle>(0);
                    header.AddAttribute(1, "ChildContent", (RenderFragment)(t => t.AddContent(2, "Title")));
                    header.CloseComponent();
                    header.OpenComponent<SfCardSubTitle>(3);
                    header.AddAttribute(4, "ChildContent", (RenderFragment)(t => t.AddContent(5, "Subtitle")));
                    header.CloseComponent();
                }));
                builder.CloseComponent();
                builder.OpenComponent<SfCardMedia>(2);
                builder.AddAttribute(3, "ImageUrl", "img.png");
                builder.AddAttribute(4, "Alt", "Cover");
                builder.CloseComponent();
                builder.OpenComponent<SfCardContent>(5);
                builder.AddAttribute(6, "ChildContent", (RenderFragment)(t => t.AddContent(7, "Body")));
                builder.CloseComponent();
                builder.OpenComponent<SfCardSeparator>(8);
                builder.CloseComponent();
                builder.OpenComponent<SfCardActions>(9);
                builder.AddAttribute(10, "ChildContent", (RenderFragment)(t => t.AddContent(11, "Actions")));
                builder.CloseComponent();
                builder.OpenComponent<SfCardFooter>(12);
                builder.AddAttribute(13, "ChildContent", (RenderFragment)(t => t.AddContent(14, "Footer")));
                builder.CloseComponent();
            };

            IRenderedComponent<SfCard> cut = RenderCard(p => p.Add(c => c.ChildContent, content));

            string[] expected =
            [
                "card-header", "card-media", "card-content", "card-separator", "card-actions", "card-footer"
            ];
            string[] actual = cut.Find(Root).Children.Select(c => c.GetAttribute("data-testid")!).ToArray();
            Assert.Equal(expected, actual);
            Assert.Equal("Title", cut.Find("[data-testid='card-title']").TextContent.Trim());
            Assert.Equal("Subtitle", cut.Find("[data-testid='card-subtitle']").TextContent.Trim());
            Assert.Equal("separator", cut.Find("[data-testid='card-separator']").GetAttribute("role"));
            Assert.Contains("e-card-title", cut.Find("[data-testid='card-title']").ClassList);
            Assert.Contains("e-card-subtitle", cut.Find("[data-testid='card-subtitle']").ClassList);
        }

        [Theory(DisplayName = "Footer template keeps padding when ContentPadding is true")]
        [InlineData(true, "padding:var(--e-card-padding, 16px)")]
        [InlineData(false, null)]
        public void Footer_ContentPadding_SetsInlinePadding(bool contentPadding, string? expectedStyle)
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.ContentPadding, contentPadding)
                .Add(c => c.ChildContent, Region<SfCardFooter>("Footer")));

            Assert.Equal(expectedStyle, cut.Find("[data-testid='card-footer']").GetAttribute("style"));
        }

        // ---------------------------------------------------------------- U-CARD-003..008 variants

        [Theory(DisplayName = "U-CARD-003 / 004 / 005 Variant maps to its class")]
        [InlineData(CardVariant.Outlined, "e-card-outlined")]
        [InlineData(CardVariant.Flat, "e-card-flat")]
        [InlineData(CardVariant.Elevated, "e-card-elevated")]
        public void Variant_AddsMatchingClass(CardVariant variant, string expectedClass)
        {
            IElement card = RenderCard(p => p.Add(c => c.Variant, variant)).Find(Root);

            Assert.Contains(expectedClass, card.ClassList);
        }

        [Fact(DisplayName = "U-CARD-006 Horizontal orientation adds e-card-horizontal")]
        public void HorizontalOrientation_AddsClass()
        {
            Assert.Contains("e-card-horizontal", RenderCard(p => p.Add(c => c.Orientation, CardOrientation.Horizontal)).Find(Root).ClassList);
        }

        [Fact(DisplayName = "U-CARD-007 Square adds e-card-square")]
        public void Square_AddsClass()
        {
            Assert.Contains("e-card-square", RenderCard(p => p.Add(c => c.Square, true)).Find(Root).ClassList);
        }

        [Fact(DisplayName = "U-CARD-008 Hoverable adds e-card-hoverable")]
        public void Hoverable_AddsClass()
        {
            Assert.Contains("e-card-hoverable", RenderCard(p => p.Add(c => c.Hoverable, true)).Find(Root).ClassList);
        }

        // ---------------------------------------------------------------- U-CARD-009 loading

        [Fact(DisplayName = "U-CARD-009 Loading sets class and aria-busy; aria-disabled only when clickable")]
        public void Loading_SetsBusyState()
        {
            IElement staticCard = RenderCard(p => p.Add(c => c.IsLoading, true)).Find(Root);
            Assert.Contains("e-card-loading", staticCard.ClassList);
            Assert.Equal("true", staticCard.GetAttribute("aria-busy"));
            Assert.Null(staticCard.GetAttribute("aria-disabled"));

            IElement clickable = RenderCard(p => p
                .Add(c => c.IsLoading, true)
                .Add(c => c.IsClickable, true)).Find(Root);
            Assert.Equal("true", clickable.GetAttribute("aria-disabled"));
            Assert.Equal("0", clickable.GetAttribute("tabindex"));
        }

        // ---------------------------------------------------------------- U-CARD-010..012 geometry

        [Fact(DisplayName = "U-CARD-010 Width and Height are applied; bare numbers become pixels")]
        public void WidthAndHeight_AreApplied()
        {
            string style = RenderCard(p => p
                .Add(c => c.Width, "320px")
                .Add(c => c.Height, "200")).Find(Root).GetAttribute("style")!;

            Assert.Contains("width:320px;", style);
            Assert.Contains("height:200px;", style);
        }

        [Theory(DisplayName = "U-CARD-011 Elevation maps to a clamped data-elevation level")]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(5, "5")]
        [InlineData(9, "5")]
        [InlineData(-3, "0")]
        public void Elevation_IsClamped(int elevation, string expected)
        {
            Assert.Equal(expected, RenderCard(p => p.Add(c => c.Elevation, elevation)).Find(Root).GetAttribute("data-elevation"));
        }

        [Fact(DisplayName = "Elevated variant draws two levels deeper and outlined or flat draw no shadow")]
        public void VariantAdjustsEffectiveElevation()
        {
            Assert.Equal("3", RenderCard(p => p.Add(c => c.Variant, CardVariant.Elevated).Add(c => c.Elevation, 1)).Find(Root).GetAttribute("data-elevation"));
            Assert.Equal("5", RenderCard(p => p.Add(c => c.Variant, CardVariant.Elevated).Add(c => c.Elevation, 4)).Find(Root).GetAttribute("data-elevation"));
            Assert.Equal("0", RenderCard(p => p.Add(c => c.Variant, CardVariant.Elevated).Add(c => c.Elevation, 0)).Find(Root).GetAttribute("data-elevation"));
            Assert.Equal("0", RenderCard(p => p.Add(c => c.Variant, CardVariant.Outlined).Add(c => c.Elevation, 3)).Find(Root).GetAttribute("data-elevation"));
            Assert.Equal("0", RenderCard(p => p.Add(c => c.Variant, CardVariant.Flat).Add(c => c.Elevation, 3)).Find(Root).GetAttribute("data-elevation"));
        }

        [Fact(DisplayName = "U-CARD-012 ContentPadding false turns padding off")]
        public void ContentPaddingFalse_IsReflected()
        {
            Assert.Equal("false", RenderCard(p => p.Add(c => c.ContentPadding, false)).Find(Root).GetAttribute("data-content-padding"));
        }

        // ---------------------------------------------------------------- U-CARD-013 / 020 media

        [Fact(DisplayName = "U-CARD-013 Media renders the image with its alt text, default height and cover fit")]
        public void Media_RendersImageWithAlt()
        {
            IRenderedComponent<SfCardMedia> cut = RenderComponent<SfCardMedia>(p => p
                .Add(c => c.ImageUrl, "/img/shoe.png")
                .Add(c => c.Alt, "Trail Runner"));

            IElement image = cut.Find("img");
            Assert.Equal("/img/shoe.png", image.GetAttribute("src"));
            Assert.Equal("Trail Runner", image.GetAttribute("alt"));
            Assert.Contains("object-fit:cover;", image.GetAttribute("style"));
            Assert.Contains("height:180px;", cut.Find("[data-testid='card-media']").GetAttribute("style"));
        }

        [Fact(DisplayName = "Media without Alt renders an empty alt so the image is decorative, not unnamed")]
        public void Media_WithoutAlt_RendersEmptyAlt()
        {
            IElement image = RenderComponent<SfCardMedia>(p => p.Add(c => c.ImageUrl, "x.png")).Find("img");

            Assert.True(image.HasAttribute("alt"));
            Assert.Equal(string.Empty, image.GetAttribute("alt"));
        }

        [Theory(DisplayName = "Media ObjectFit maps to CSS and None applies no override")]
        [InlineData(CardMediaFit.Cover, "object-fit:cover;")]
        [InlineData(CardMediaFit.Contain, "object-fit:contain;")]
        [InlineData(CardMediaFit.Fill, "object-fit:fill;")]
        public void Media_ObjectFit_MapsToCss(CardMediaFit fit, string expected)
        {
            IElement image = RenderComponent<SfCardMedia>(p => p
                .Add(c => c.ImageUrl, "x.png")
                .Add(c => c.ObjectFit, fit)).Find("img");

            Assert.Equal(expected, image.GetAttribute("style"));
        }

        [Fact(DisplayName = "Media with CardMediaFit.None emits no object-fit")]
        public void Media_FitNone_EmitsNoObjectFit()
        {
            IElement image = RenderComponent<SfCardMedia>(p => p
                .Add(c => c.ImageUrl, "x.png")
                .Add(c => c.ObjectFit, CardMediaFit.None)).Find("img");

            Assert.Null(image.GetAttribute("style"));
        }

        [Fact(DisplayName = "U-CARD-020 Media renders custom ChildContent when no ImageUrl is given")]
        public void Media_CustomChildContent_Renders()
        {
            IRenderedComponent<SfCardMedia> cut = RenderComponent<SfCardMedia>(p => p
                .Add(c => c.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<span id='cover'>cover</span>"))));

            Assert.Empty(cut.FindAll("img"));
            Assert.Equal("cover", cut.Find("#cover").TextContent);
        }

        // ---------------------------------------------------------------- U-CARD-014 / 015 click and keyboard

        [Fact(DisplayName = "U-CARD-014 Clickable card exposes button semantics and raises OnClick on click")]
        public void Clickable_ExposesSemanticsAndRaisesOnClick()
        {
            int count = 0;
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.IsClickable, true)
                .Add(c => c.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => count++)));
            IElement card = cut.Find(Root);

            Assert.Equal("button", card.GetAttribute("role"));
            Assert.Equal("0", card.GetAttribute("tabindex"));
            Assert.Contains("e-card-clickable", card.ClassList);

            card.Click();

            Assert.Equal(1, count);
        }

        [Fact(DisplayName = "Enter activates once and Tab is not prevented")]
        public void Enter_ActivatesOnce_TabIsNotPrevented()
        {
            int count = 0;
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.IsClickable, true)
                .Add(c => c.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => count++)));
            IElement card = cut.Find(Root);

            card.KeyDown(new KeyboardEventArgs { Key = "Enter" });
            Assert.Equal(1, count);

            card.KeyDown(new KeyboardEventArgs { Key = "Enter", Repeat = true });
            card.KeyDown(new KeyboardEventArgs { Key = "Tab" });
            card.KeyDown(new KeyboardEventArgs { Key = "a" });
            Assert.Equal(1, count);

            Assert.Contains("blazor:onkeydown:preventDefault", cut.Markup, StringComparison.Ordinal);
            Assert.Contains("blazor:onkeyup:preventDefault", cut.Markup, StringComparison.Ordinal);

            Func<KeyboardEventArgs, bool> keyDownPreventDefault = GetPreventDefaultPredicate(cut.Instance, "onkeydown:preventDefault");
            Assert.True(keyDownPreventDefault(new KeyboardEventArgs { Key = "Enter" }));
            Assert.True(keyDownPreventDefault(new KeyboardEventArgs { Key = " ", Code = "Space" }));
            Assert.False(keyDownPreventDefault(new KeyboardEventArgs { Key = "Tab" }));
            Assert.False(keyDownPreventDefault(new KeyboardEventArgs { Key = "a" }));

            Func<KeyboardEventArgs, bool> keyUpPreventDefault = GetPreventDefaultPredicate(cut.Instance, "onkeyup:preventDefault");
            Assert.True(keyUpPreventDefault(new KeyboardEventArgs { Key = " ", Code = "Space" }));
            Assert.False(keyUpPreventDefault(new KeyboardEventArgs { Key = "Enter" }));
            Assert.False(keyUpPreventDefault(new KeyboardEventArgs { Key = "Tab" }));
        }

        [Fact(DisplayName = "Space activates on key up only after a matching key down")]
        public void Space_ActivatesOnKeyUp()
        {
            int count = 0;
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.IsClickable, true)
                .Add(c => c.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => count++)));
            IElement card = cut.Find(Root);

            card.KeyUp(new KeyboardEventArgs { Key = " ", Code = "Space" });
            Assert.Equal(0, count);

            card.KeyDown(new KeyboardEventArgs { Key = " ", Code = "Space" });
            Assert.Equal(0, count);

            card.KeyUp(new KeyboardEventArgs { Key = " ", Code = "Space" });
            Assert.Equal(1, count);
        }

        [Fact(DisplayName = "Loading card ignores click, Enter and Space")]
        public void Loading_SuppressesActivation()
        {
            int count = 0;
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.IsClickable, true)
                .Add(c => c.IsLoading, true)
                .Add(c => c.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => count++)));
            IElement card = cut.Find(Root);

            card.Click();
            card.KeyDown(new KeyboardEventArgs { Key = "Enter" });
            card.KeyDown(new KeyboardEventArgs { Key = " ", Code = "Space" });
            card.KeyUp(new KeyboardEventArgs { Key = " ", Code = "Space" });

            Assert.Equal(0, count);
        }

        [Fact(DisplayName = "Non-clickable card raises no OnClick and binds no event handlers")]
        public void NonClickable_BindsNoHandlers()
        {
            int count = 0;
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => count++)));

            Assert.DoesNotContain("blazor:onclick", cut.Markup, StringComparison.Ordinal);
            Assert.DoesNotContain("blazor:onkeydown", cut.Markup, StringComparison.Ordinal);
            Assert.Equal(0, count);
        }

        [Fact(DisplayName = "U-CARD-015 A click inside SfCardActions does not activate a clickable card")]
        public void ActionsClick_DoesNotActivateCard()
        {
            int cardClicks = 0;
            int buttonClicks = 0;
            RenderFragment actions = builder =>
            {
                builder.OpenComponent<SfCardActions>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(inner =>
                {
                    inner.OpenElement(0, "button");
                    inner.AddAttribute(1, "data-testid", "card-action-btn");
                    inner.AddAttribute(2, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => buttonClicks++));
                    inner.AddContent(3, "Buy");
                    inner.CloseElement();
                }));
                builder.CloseComponent();
            };
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.IsClickable, true)
                .Add(c => c.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => cardClicks++))
                .Add(c => c.ChildContent, actions));

            cut.Find("[data-testid='card-action-btn']").Click();

            Assert.Equal(1, buttonClicks);
            Assert.Equal(0, cardClicks);
            Assert.Contains("stoppropagation", cut.Find("[data-testid='card-actions']").OuterHtml.Replace("\"", string.Empty), StringComparison.OrdinalIgnoreCase);
        }

        [Fact(DisplayName = "Actions are inert only while the card loads")]
        public void Actions_AreInertWhileLoading()
        {
            RenderFragment actions = Region<SfCardActions>("Buy");

            IRenderedComponent<SfCard> idle = RenderCard(p => p.Add(c => c.ChildContent, actions));
            Assert.False(idle.Find("[data-testid='card-actions']").HasAttribute("inert"));

            IRenderedComponent<SfCard> loading = RenderCard(p => p
                .Add(c => c.IsLoading, true)
                .Add(c => c.ChildContent, actions));
            Assert.True(loading.Find("[data-testid='card-actions']").HasAttribute("inert"));
        }

        // ---------------------------------------------------------------- U-CARD-016 attributes

        [Fact(DisplayName = "U-CARD-016 CssClass and HtmlAttributes are applied; user class and style are merged")]
        public void CssClassAndHtmlAttributes_AreApplied()
        {
            IElement card = RenderCard(p => p
                .Add(c => c.CssClass, "custom-card")
                .Add(c => c.Width, "320px")
                .Add(c => c.HtmlAttributes, new Dictionary<string, object>
                {
                    ["title"] = "Profile card",
                    ["data-owner"] = "qa",
                    ["class"] = "from-attributes",
                    ["style"] = "color:red"
                })).Find(Root);

            Assert.Contains("custom-card", card.ClassList);
            Assert.Contains("from-attributes", card.ClassList);
            Assert.Contains("e-card", card.ClassList);
            Assert.Equal("Profile card", card.GetAttribute("title"));
            Assert.Equal("qa", card.GetAttribute("data-owner"));
            Assert.Contains("width:320px;", card.GetAttribute("style"));
            Assert.Contains("color:red", card.GetAttribute("style"));
        }

        [Fact(DisplayName = "A user supplied aria-label and role win over computed values")]
        public void UserAttributes_WinOverComputed()
        {
            IElement card = RenderCard(p => p
                .Add(c => c.IsClickable, true)
                .Add(c => c.HtmlAttributes, new Dictionary<string, object>
                {
                    ["role"] = "link",
                    ["aria-label"] = "Open profile"
                })).Find(Root);

            Assert.Equal("link", card.GetAttribute("role"));
            Assert.Equal("Open profile", card.GetAttribute("aria-label"));
        }

        [Fact(DisplayName = "AriaLabel renders aria-label")]
        public void AriaLabel_IsRendered()
        {
            Assert.Equal("Profile", RenderCard(p => p.Add(c => c.AriaLabel, "Profile")).Find(Root).GetAttribute("aria-label"));
        }

        [Fact(DisplayName = "Region components keep a user class and merge HtmlAttributes")]
        public void Regions_MergeUserClassAndAttributes()
        {
            IElement content = RenderComponent<SfCardContent>(p => p
                .Add(c => c.CssClass, "extra")
                .Add(c => c.HtmlAttributes, new Dictionary<string, object> { ["class"] = "user", ["data-x"] = "1" }))
                .Find("[data-testid='card-content']");

            Assert.Contains("e-card-content", content.ClassList);
            Assert.Contains("extra", content.ClassList);
            Assert.Contains("user", content.ClassList);
            Assert.Equal("1", content.GetAttribute("data-x"));
        }

        [Fact(DisplayName = "Separator is drawn vertically inside a horizontal card")]
        public void Separator_IsVerticalInHorizontalCard()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.Orientation, CardOrientation.Horizontal)
                .Add(c => c.ChildContent, (RenderFragment)(b =>
                {
                    b.OpenComponent<SfCardSeparator>(0);
                    b.CloseComponent();
                })));

            Assert.Equal("vertical", cut.Find("[data-testid='card-separator']").GetAttribute("aria-orientation"));
        }

        // ---------------------------------------------------------------- RTL

        [Fact(DisplayName = "Global EnableRtl adds e-rtl")]
        public void GlobalRtl_AddsRtlClass()
        {
            Services.Configure<GlobalOptions>(options => options.EnableRtl = true);

            Assert.Contains("e-rtl", RenderCard().Find(Root).ClassList);
        }

        // ---------------------------------------------------------------- I-CARD integration

        [Fact(DisplayName = "I-CARD-001 Multiple cards in a list keep unique ids")]
        public void MultipleCards_HaveUniqueIds()
        {
            IRenderedComponent<ComponentBase> cut = RenderComponent<CardList>();

            string[] ids = cut.FindAll(Root).Select(c => c.Id).ToArray();
            Assert.Equal(5, ids.Length);
            Assert.Equal(ids.Length, ids.Distinct(StringComparer.Ordinal).Count());
        }

        [Fact(DisplayName = "I-CARD-002 Null ChildContent and empty regions are safe")]
        public void NullContent_IsSafe()
        {
            IRenderedComponent<SfCard> cut = RenderCard();
            Assert.Empty(cut.Find(Root).Children);

            Assert.Empty(RenderComponent<SfCardHeader>().Find("[data-testid='card-header']").Children);
            Assert.Empty(RenderComponent<SfCardMedia>().Find("[data-testid='card-media']").Children);
        }

        [Fact(DisplayName = "I-CARD-003 Parameter churn re-renders without stale classes")]
        public void ParameterChurn_LeavesNoStaleClasses()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.Variant, CardVariant.Outlined)
                .Add(c => c.Square, true)
                .Add(c => c.Hoverable, true));

            cut.SetParametersAndRender(p => p
                .Add(c => c.Variant, CardVariant.Flat)
                .Add(c => c.Square, false)
                .Add(c => c.Hoverable, false));

            Assert.Equal("e-card e-card-flat", cut.Find(Root).ClassName);
        }

        [Fact(DisplayName = "I-CARD-004 Loading toggle updates state and re-enables activation")]
        public void LoadingToggle_UpdatesState()
        {
            int count = 0;
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.IsClickable, true)
                .Add(c => c.IsLoading, true)
                .Add(c => c.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, () => count++)));

            cut.Find(Root).Click();
            Assert.Equal(0, count);

            cut.SetParametersAndRender(p => p.Add(c => c.IsLoading, false));
            IElement card = cut.Find(Root);
            Assert.Null(card.GetAttribute("aria-busy"));
            Assert.DoesNotContain("e-card-loading", card.ClassList);

            card.Click();
            Assert.Equal(1, count);
        }

        [Fact(DisplayName = "Turning IsClickable off removes role, tabindex and handlers")]
        public void ClickableToggle_RemovesSemantics()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p.Add(c => c.IsClickable, true));
            Assert.Equal("button", cut.Find(Root).GetAttribute("role"));

            cut.SetParametersAndRender(p => p.Add(c => c.IsClickable, false));

            IElement card = cut.Find(Root);
            Assert.Null(card.GetAttribute("role"));
            Assert.Null(card.GetAttribute("tabindex"));
            Assert.DoesNotContain("e-card-clickable", card.ClassList);
        }

        private sealed class CardList : ComponentBase
        {
            protected override void BuildRenderTree(RenderTreeBuilder builder)
            {
                for (int i = 0; i < 5; i++)
                {
                    builder.OpenComponent<SfCard>(i);
                    builder.CloseComponent();
                }
            }
        }
    }
}
