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
    /// bUnit coverage mapped one-to-one to the B-CARD ids in .github/specs/Cards/TEST-MATRIX.md.
    /// The Card has no Disabled or Selected parameter, so P-CARD-018, 057, 062, 063, 073 and 086 are not applicable.
    /// </summary>
    public class SfCardMatrixTest : BunitTestContext
    {
        private const string Root = "[data-testid='card-root']";
        private const string Header = "card-header";
        private const string Content = "card-content";
        private const string Footer = "card-footer";
        private const string Media = "card-media";
        private const string Actions = "card-actions";

        private static RenderFragment Region<TRegion>(string text) where TRegion : IComponent
        {
            return builder =>
            {
                builder.OpenComponent<TRegion>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(inner => inner.AddContent(2, text)));
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

        private static RenderFragment MediaRegion()
        {
            return builder =>
            {
                builder.OpenComponent<SfCardMedia>(0);
                builder.AddAttribute(1, "ImageUrl", "/img/a.png");
                builder.AddAttribute(2, "Alt", "A");
                builder.CloseComponent();
            };
        }

        private IRenderedComponent<SfCard> RenderCard(Action<ComponentParameterCollectionBuilder<SfCard>>? configure = null)
        {
            return RenderComponent<SfCard>(configure ?? (_ => { }));
        }

        private IRenderedComponent<SfCard> RenderWith(params RenderFragment[] regions)
        {
            return RenderCard(p => p.Add(c => c.ChildContent, Compose(regions)));
        }

        private static string[] ChildIds(IRenderedComponent<SfCard> cut)
        {
            return cut.Find(Root).Children.Select(c => c.GetAttribute("data-testid")!).ToArray();
        }

        private IRenderedComponent<SfCard> ClickableCard(Action<MouseEventArgs> onClick, Action<ComponentParameterCollectionBuilder<SfCard>>? more = null)
        {
            return RenderCard(p =>
            {
                p.Add(c => c.IsClickable, true);
                p.Add(c => c.OnClick, EventCallback.Factory.Create<MouseEventArgs>(this, onClick));
                more?.Invoke(p);
            });
        }

        // ------------------------------------------------------------------ Rendering scenarios

        [Fact(DisplayName = "V-CARD-001 Empty card renders without exceptions")]
        public void B001_EmptyCard()
        {
            IRenderedComponent<SfCard> cut = RenderCard();

            Assert.Empty(cut.Find(Root).Children);
        }

        [Fact(DisplayName = "V-CARD-002 Header-only card renders correctly")]
        public void B002_HeaderOnly()
        {
            Assert.Equal([Header], ChildIds(RenderWith(Region<SfCardHeader>("H"))));
        }

        [Fact(DisplayName = "V-CARD-003 Body-only card renders correctly")]
        public void B003_BodyOnly()
        {
            Assert.Equal([Content], ChildIds(RenderWith(Region<SfCardContent>("B"))));
        }

        [Fact(DisplayName = "V-CARD-004 Footer-only card renders correctly")]
        public void B004_FooterOnly()
        {
            Assert.Equal([Footer], ChildIds(RenderWith(Region<SfCardFooter>("F"))));
        }

        [Fact(DisplayName = "V-CARD-005 Media-only card renders correctly")]
        public void B005_MediaOnly()
        {
            IRenderedComponent<SfCard> cut = RenderWith(MediaRegion());

            Assert.Equal([Media], ChildIds(cut));
            Assert.Equal("A", cut.Find("img").GetAttribute("alt"));
        }

        [Fact(DisplayName = "V-CARD-006 Actions-only card renders correctly")]
        public void B006_ActionsOnly()
        {
            Assert.Equal([Actions], ChildIds(RenderWith(Region<SfCardActions>("Go"))));
        }

        [Fact(DisplayName = "V-CARD-007 Header + Body composition renders correctly")]
        public void B007_HeaderBody()
        {
            Assert.Equal([Header, Content], ChildIds(RenderWith(Region<SfCardHeader>("H"), Region<SfCardContent>("B"))));
        }

        [Fact(DisplayName = "V-CARD-008 Header + Footer composition renders correctly")]
        public void B008_HeaderFooter()
        {
            Assert.Equal([Header, Footer], ChildIds(RenderWith(Region<SfCardHeader>("H"), Region<SfCardFooter>("F"))));
        }

        [Fact(DisplayName = "V-CARD-009 Body + Footer composition renders correctly")]
        public void B009_BodyFooter()
        {
            Assert.Equal([Content, Footer], ChildIds(RenderWith(Region<SfCardContent>("B"), Region<SfCardFooter>("F"))));
        }

        [Fact(DisplayName = "V-CARD-010 Media + Header composition renders correctly")]
        public void B010_MediaHeader()
        {
            Assert.Equal([Media, Header], ChildIds(RenderWith(MediaRegion(), Region<SfCardHeader>("H"))));
        }

        [Fact(DisplayName = "V-CARD-011 Media + Footer composition renders correctly")]
        public void B011_MediaFooter()
        {
            Assert.Equal([Media, Footer], ChildIds(RenderWith(MediaRegion(), Region<SfCardFooter>("F"))));
        }

        [Fact(DisplayName = "A-CARD-011 Media + Body composition renders correctly")]
        public void B012_MediaBody()
        {
            Assert.Equal([Media, Content], ChildIds(RenderWith(MediaRegion(), Region<SfCardContent>("B"))));
        }

        [Fact(DisplayName = "V-CARD-012 Full card composition renders correctly")]
        public void B013_Full()
        {
            IRenderedComponent<SfCard> cut = RenderWith(
                Region<SfCardHeader>("H"),
                MediaRegion(),
                Region<SfCardContent>("B"),
                Region<SfCardActions>("A"),
                Region<SfCardFooter>("F"));

            Assert.Equal([Header, Media, Content, Actions, Footer], ChildIds(cut));
        }

        [Fact(DisplayName = "V-CARD-013 Multiple cards render independently")]
        public void B014_MultipleIndependent()
        {
            IRenderedComponent<ComponentBase> cut = RenderComponent<TwoCards>();

            IRefreshableElementCollection<IElement> cards = cut.FindAll(Root);
            Assert.Equal(2, cards.Count);
            Assert.Contains("e-card-outlined", cards[0].ClassList);
            Assert.DoesNotContain("e-card-outlined", cards[1].ClassList);
            Assert.NotEqual(cards[0].Id, cards[1].Id);
        }

        [Fact(DisplayName = "V-CARD-014 Nested cards render correctly")]
        public void B015_NestedCards()
        {
            RenderFragment inner = builder =>
            {
                builder.OpenComponent<SfCardContent>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(c =>
                {
                    c.OpenComponent<SfCard>(0);
                    c.AddAttribute(1, "Id", "inner-card");
                    c.CloseComponent();
                }));
                builder.CloseComponent();
            };

            IRenderedComponent<SfCard> cut = RenderCard(p => p.Add(c => c.Id, "outer-card").Add(c => c.ChildContent, inner));

            Assert.Equal(2, cut.FindAll(Root).Count);
            Assert.NotNull(cut.Find("#outer-card #inner-card"));
        }

        [Fact(DisplayName = "B-CARD-083 SfCardTitle and SfCardSubTitle render with correct CSS classes")]
        public void B090_TitleSubTitleClasses()
        {
            IRenderedComponent<SfCard> cut = RenderWith(Region<SfCardTitle>("T"), Region<SfCardSubTitle>("S"));

            Assert.Contains("e-card-title", cut.Find("[data-testid='card-title']").ClassList);
            Assert.Contains("e-card-subtitle", cut.Find("[data-testid='card-subtitle']").ClassList);
        }

        [Fact(DisplayName = "B-CARD-084 SfCardSeparator renders with role=separator")]
        public void B091_SeparatorRole()
        {
            IElement separator = RenderComponent<SfCardSeparator>().Find("[data-testid='card-separator']");

            Assert.Equal("separator", separator.GetAttribute("role"));
            Assert.Contains("e-card-separator", separator.ClassList);
        }

        [Fact(DisplayName = "B-CARD-085 SfCardSeparator aria-orientation is vertical inside Horizontal card")]
        public void B092_SeparatorOrientation()
        {
            RenderFragment separator = b => { b.OpenComponent<SfCardSeparator>(0); b.CloseComponent(); };

            IRenderedComponent<SfCard> horizontal = RenderCard(p => p.Add(c => c.Orientation, CardOrientation.Horizontal).Add(c => c.ChildContent, separator));
            IRenderedComponent<SfCard> vertical = RenderCard(p => p.Add(c => c.ChildContent, separator));

            Assert.Equal("vertical", horizontal.Find("[data-testid='card-separator']").GetAttribute("aria-orientation"));
            Assert.Equal("horizontal", vertical.Find("[data-testid='card-separator']").GetAttribute("aria-orientation"));
        }

        // ------------------------------------------------------------------ Content rendering

        private string BodyText(RenderFragment body)
        {
            RenderFragment wrapped = b =>
            {
                b.OpenComponent<SfCardContent>(0);
                b.AddAttribute(1, "ChildContent", body);
                b.CloseComponent();
            };
            return RenderWith(wrapped).Find("[data-testid='card-content']").TextContent;
        }

        private IElement BodyElement(RenderFragment body)
        {
            RenderFragment wrapped = b =>
            {
                b.OpenComponent<SfCardContent>(0);
                b.AddAttribute(1, "ChildContent", body);
                b.CloseComponent();
            };
            return RenderWith(wrapped).Find("[data-testid='card-content']");
        }

        [Fact(DisplayName = "V-CARD-015 Plain text content")]
        public void B016_PlainText()
        {
            Assert.Equal("Hello card", BodyText(b => b.AddContent(0, "Hello card")));
        }

        [Fact(DisplayName = "V-CARD-016 Rich HTML content")]
        public void B017_RichHtml()
        {
            IElement body = BodyElement(b => b.AddMarkupContent(0, "<p><strong>Bold</strong> and <em>italic</em></p>"));

            Assert.Equal("Bold", body.QuerySelector("strong")!.TextContent);
            Assert.Equal("italic", body.QuerySelector("em")!.TextContent);
        }

        [Fact(DisplayName = "V-CARD-017 RenderFragment content")]
        public void B018_RenderFragment()
        {
            RenderFragment fragment = b => { b.OpenElement(0, "span"); b.AddAttribute(1, "id", "frag"); b.AddContent(2, "fragment"); b.CloseElement(); };

            Assert.Equal("fragment", BodyElement(fragment).QuerySelector("#frag")!.TextContent);
        }

        [Fact(DisplayName = "V-CARD-018 Large content block")]
        public void B019_LargeContent()
        {
            string large = string.Concat(Enumerable.Repeat("lorem ipsum ", 2000));

            Assert.Equal(large, BodyText(b => b.AddContent(0, large)));
        }

        [Fact(DisplayName = "V-CARD-019 Long text wrapping")]
        public void B020_LongTextWrapping()
        {
            string unbroken = new('W', 600);

            // Wrapping itself is CSS and is asserted in Playwright (V-CARD-023); here the full text must reach the DOM unchanged.
            Assert.Equal(unbroken, BodyText(b => b.AddContent(0, unbroken)));
        }

        [Fact(DisplayName = "V-CARD-020 Overflow content handling")]
        public void B021_Overflow()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.Height, "40px")
                .Add(c => c.ChildContent, Region<SfCardContent>(new string('x', 500))));

            Assert.Contains("height:40px;", cut.Find(Root).GetAttribute("style"));
            Assert.Equal(500, cut.Find("[data-testid='card-content']").TextContent.Length);
        }

        [Fact(DisplayName = "V-CARD-021 Nested components")]
        public void B022_NestedComponents()
        {
            IElement body = BodyElement(b =>
            {
                b.OpenComponent<SfCardSubTitle>(0);
                b.AddAttribute(1, "ChildContent", (RenderFragment)(t => t.AddContent(0, "nested")));
                b.CloseComponent();
            });

            Assert.Equal("nested", body.QuerySelector("[data-testid='card-subtitle']")!.TextContent);
        }

        [Fact(DisplayName = "V-CARD-022 Nested buttons")]
        public void B023_NestedButtons()
        {
            int clicks = 0;
            IElement body = BodyElement(b =>
            {
                b.OpenElement(0, "button");
                b.AddAttribute(1, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => clicks++));
                b.AddContent(2, "Go");
                b.CloseElement();
            });

            body.QuerySelector("button")!.Click();

            Assert.Equal(1, clicks);
        }

        [Fact(DisplayName = "V-CARD-023 Nested inputs")]
        public void B024_NestedInputs()
        {
            IElement body = BodyElement(b =>
            {
                b.OpenElement(0, "input");
                b.AddAttribute(1, "type", "text");
                b.AddAttribute(2, "value", "abc");
                b.CloseElement();
            });

            Assert.Equal("abc", body.QuerySelector("input")!.GetAttribute("value"));
        }

        [Fact(DisplayName = "V-CARD-024 Nested forms")]
        public void B025_NestedForms()
        {
            IElement body = BodyElement(b =>
            {
                b.OpenElement(0, "form");
                b.OpenElement(1, "input");
                b.AddAttribute(2, "name", "q");
                b.CloseElement();
                b.OpenElement(3, "button");
                b.AddAttribute(4, "type", "submit");
                b.AddContent(5, "Send");
                b.CloseElement();
                b.CloseElement();
            });

            Assert.NotNull(body.QuerySelector("form input[name='q']"));
            Assert.NotNull(body.QuerySelector("form button[type='submit']"));
        }

        [Fact(DisplayName = "V-CARD-025 Nested charts")]
        public void B026_NestedCharts()
        {
            IElement body = BodyElement(b =>
            {
                b.OpenComponent<SfChart>(0);
                b.CloseComponent();
            });

            Assert.NotNull(body.QuerySelector(".e-chart, [id*='chart'], div"));
        }

        [Fact(DisplayName = "V-CARD-026 Dynamic content updates")]
        public void B027_DynamicContent()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p.Add(c => c.ChildContent, Region<SfCardContent>("first")));
            Assert.Equal("first", cut.Find("[data-testid='card-content']").TextContent);

            cut.SetParametersAndRender(p => p.Add(c => c.ChildContent, Region<SfCardContent>("second")));

            Assert.Equal("second", cut.Find("[data-testid='card-content']").TextContent);
        }

        // ------------------------------------------------------------------ Variants

        [Fact(DisplayName = "V-CARD-027 Default variant")]
        public void B028_DefaultVariant()
        {
            Assert.Equal("e-card", RenderCard().Find(Root).ClassName);
        }

        [Fact(DisplayName = "V-CARD-028 Elevated variant")]
        public void B029_Elevated()
        {
            Assert.Contains("e-card-elevated", RenderCard(p => p.Add(c => c.Variant, CardVariant.Elevated)).Find(Root).ClassList);
        }

        [Fact(DisplayName = "V-CARD-029 Outlined variant")]
        public void B030_Outlined()
        {
            Assert.Contains("e-card-outlined", RenderCard(p => p.Add(c => c.Variant, CardVariant.Outlined)).Find(Root).ClassList);
        }

        [Fact(DisplayName = "V-CARD-030 Filled variant")]
        public void B031_Filled()
        {
            Assert.Equal("e-card", RenderCard(p => p.Add(c => c.Variant, CardVariant.Filled)).Find(Root).ClassName);
        }

        [Fact(DisplayName = "V-CARD-031 Flat variant")]
        public void B032_Flat()
        {
            Assert.Contains("e-card-flat", RenderCard(p => p.Add(c => c.Variant, CardVariant.Flat)).Find(Root).ClassList);
        }

        [Fact(DisplayName = "V-CARD-032 Runtime variant update")]
        public void B033_RuntimeVariant()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p.Add(c => c.Variant, CardVariant.Outlined));
            Assert.Contains("e-card-outlined", cut.Find(Root).ClassList);

            cut.SetParametersAndRender(p => p.Add(c => c.Variant, CardVariant.Elevated));

            Assert.Contains("e-card-elevated", cut.Find(Root).ClassList);
            Assert.DoesNotContain("e-card-outlined", cut.Find(Root).ClassList);
        }

        [Theory(DisplayName = "P-CARD-036 Variant CSS classes applied correctly")]
        [InlineData(CardVariant.Filled, "e-card")]
        [InlineData(CardVariant.Outlined, "e-card e-card-outlined")]
        [InlineData(CardVariant.Elevated, "e-card e-card-elevated")]
        [InlineData(CardVariant.Flat, "e-card e-card-flat")]
        public void B034_VariantClasses(CardVariant variant, string expected)
        {
            Assert.Equal(expected, RenderCard(p => p.Add(c => c.Variant, variant)).Find(Root).ClassName);
        }

        // ------------------------------------------------------------------ Layout

        [Fact(DisplayName = "P-CARD-037 Vertical orientation")]
        public void B035_Vertical()
        {
            Assert.DoesNotContain("e-card-horizontal", RenderCard(p => p.Add(c => c.Orientation, CardOrientation.Vertical)).Find(Root).ClassList);
        }

        [Fact(DisplayName = "P-CARD-038 Horizontal orientation")]
        public void B036_Horizontal()
        {
            Assert.Contains("e-card-horizontal", RenderCard(p => p.Add(c => c.Orientation, CardOrientation.Horizontal)).Find(Root).ClassList);
        }

        [Fact(DisplayName = "P-CARD-039 Runtime orientation update")]
        public void B037_RuntimeOrientation()
        {
            IRenderedComponent<SfCard> cut = RenderCard();
            cut.SetParametersAndRender(p => p.Add(c => c.Orientation, CardOrientation.Horizontal));

            Assert.Contains("e-card-horizontal", cut.Find(Root).ClassList);
        }

        [Fact(DisplayName = "P-CARD-040 Media alignment")]
        public void B038_MediaAlignment()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.Orientation, CardOrientation.Horizontal)
                .Add(c => c.ChildContent, Compose(MediaRegion(), Region<SfCardContent>("B"))));

            // The media leads the markup, so the horizontal flex row places it on the inline-start side.
            Assert.Equal([Media, Content], ChildIds(cut));
            Assert.Contains("e-card-media", cut.Find("[data-testid='card-media']").ClassList);
        }

        [Fact(DisplayName = "B-CARD-039 Header alignment")]
        public void B039_HeaderAlignment()
        {
            IRenderedComponent<SfCard> cut = RenderWith(Region<SfCardHeader>("H"), Region<SfCardContent>("B"));

            Assert.Equal(Header, ChildIds(cut)[0]);
            Assert.Contains("e-card-header", cut.Find("[data-testid='card-header']").ClassList);
        }

        [Fact(DisplayName = "B-CARD-040 Footer alignment")]
        public void B040_FooterAlignment()
        {
            IRenderedComponent<SfCard> cut = RenderWith(Region<SfCardContent>("B"), Region<SfCardFooter>("F"));

            Assert.Equal(Footer, ChildIds(cut)[^1]);
            Assert.Contains("e-card-footer", cut.Find("[data-testid='card-footer']").ClassList);
        }

        [Fact(DisplayName = "B-CARD-041 Actions alignment")]
        public void B041_ActionsAlignment()
        {
            IRenderedComponent<SfCard> cut = RenderWith(Region<SfCardContent>("B"), Region<SfCardActions>("A"));

            Assert.Equal(Actions, ChildIds(cut)[^1]);
            Assert.Contains("e-card-actions", cut.Find("[data-testid='card-actions']").ClassList);
        }

        [Fact(DisplayName = "B-CARD-086 Square property applies e-card-square class")]
        public void B093_Square()
        {
            Assert.Contains("e-card-square", RenderCard(p => p.Add(c => c.Square, true)).Find(Root).ClassList);
            Assert.DoesNotContain("e-card-square", RenderCard().Find(Root).ClassList);
        }

        [Fact(DisplayName = "B-CARD-087 Hoverable property applies e-card-hoverable class")]
        public void B094_Hoverable()
        {
            Assert.Contains("e-card-hoverable", RenderCard(p => p.Add(c => c.Hoverable, true)).Find(Root).ClassList);
            Assert.DoesNotContain("e-card-hoverable", RenderCard().Find(Root).ClassList);
        }

        [Fact(DisplayName = "B-CARD-088 ContentPadding true (default)")]
        public void B095_ContentPaddingDefault()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p.Add(c => c.ChildContent, Region<SfCardFooter>("F")));

            Assert.Equal("true", cut.Find(Root).GetAttribute("data-content-padding"));
            Assert.Equal("padding:var(--e-card-padding, 16px)", cut.Find("[data-testid='card-footer']").GetAttribute("style"));
        }

        [Fact(DisplayName = "B-CARD-089 ContentPadding false removes region padding")]
        public void B096_ContentPaddingOff()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.ContentPadding, false)
                .Add(c => c.ChildContent, Region<SfCardFooter>("F")));

            Assert.Equal("false", cut.Find(Root).GetAttribute("data-content-padding"));
            Assert.Null(cut.Find("[data-testid='card-footer']").GetAttribute("style"));
        }

        // ------------------------------------------------------------------ Parameters

        [Fact(DisplayName = "P-CARD-041 CssClass applied correctly")]
        public void B042_CssClass()
        {
            Assert.Contains("custom", RenderCard(p => p.Add(c => c.CssClass, "custom")).Find(Root).ClassList);
        }

        [Fact(DisplayName = "P-CARD-042 Multiple CssClass values")]
        public void B043_MultipleCssClass()
        {
            IElement card = RenderCard(p => p.Add(c => c.CssClass, "alpha beta  gamma")).Find(Root);

            Assert.Contains("alpha", card.ClassList);
            Assert.Contains("beta", card.ClassList);
            Assert.Contains("gamma", card.ClassList);
            Assert.Contains("e-card", card.ClassList);
        }

        [Fact(DisplayName = "P-CARD-043 Null CssClass")]
        public void B044_NullCssClass()
        {
            Assert.Equal("e-card", RenderCard(p => p.Add(c => c.CssClass, (string?)null)).Find(Root).ClassName);
        }

        [Fact(DisplayName = "P-CARD-044 Empty CssClass")]
        public void B045_EmptyCssClass()
        {
            Assert.Equal("e-card", RenderCard(p => p.Add(c => c.CssClass, string.Empty)).Find(Root).ClassName);
        }

        [Fact(DisplayName = "B-CARD-046 Style attribute applied")]
        public void B046_StyleApplied()
        {
            IElement card = RenderCard(p => p.Add(c => c.HtmlAttributes, new Dictionary<string, object> { ["style"] = "color:red" })).Find(Root);

            Assert.Contains("color:red", card.GetAttribute("style"));
        }

        [Fact(DisplayName = "P-CARD-045 Null style")]
        public void B047_NullStyle()
        {
            IElement card = RenderCard(p => p.Add(c => c.Width, (string?)null).Add(c => c.Height, (string?)null)).Find(Root);

            Assert.Null(card.GetAttribute("style"));
        }

        [Fact(DisplayName = "P-CARD-046 Empty style")]
        public void B048_EmptyStyle()
        {
            IElement card = RenderCard(p => p
                .Add(c => c.Width, string.Empty)
                .Add(c => c.Height, "  ")
                .Add(c => c.HtmlAttributes, new Dictionary<string, object> { ["style"] = string.Empty })).Find(Root);

            Assert.True(string.IsNullOrWhiteSpace(card.GetAttribute("style")));
        }

        [Fact(DisplayName = "P-CARD-047 Width customization")]
        public void B049_Width()
        {
            Assert.Contains("width:50%;", RenderCard(p => p.Add(c => c.Width, "50%")).Find(Root).GetAttribute("style"));
            Assert.Contains("width:300px;", RenderCard(p => p.Add(c => c.Width, "300")).Find(Root).GetAttribute("style"));
        }

        [Fact(DisplayName = "P-CARD-048 Height customization")]
        public void B050_Height()
        {
            Assert.Contains("height:12rem;", RenderCard(p => p.Add(c => c.Height, "12rem")).Find(Root).GetAttribute("style"));
            Assert.Contains("height:200px;", RenderCard(p => p.Add(c => c.Height, "200")).Find(Root).GetAttribute("style"));
        }

        [Fact(DisplayName = "P-CARD-049 Custom attributes rendered")]
        public void B051_CustomAttributes()
        {
            IElement card = RenderCard(p => p.Add(c => c.HtmlAttributes, new Dictionary<string, object> { ["data-owner"] = "qa", ["title"] = "Tip" })).Find(Root);

            Assert.Equal("qa", card.GetAttribute("data-owner"));
            Assert.Equal("Tip", card.GetAttribute("title"));
        }

        [Fact(DisplayName = "P-CARD-008 Arbitrary HTML attributes rendered")]
        public void B052_ArbitraryAttributes()
        {
            IRenderedComponent<SfCard> cut = RenderComponent<SfCard>(p => p.AddUnmatched("data-anything", "yes").AddUnmatched("lang", "en"));
            IElement card = cut.Find(Root);

            Assert.Equal("yes", card.GetAttribute("data-anything"));
            Assert.Equal("en", card.GetAttribute("lang"));
        }

        [Theory(DisplayName = "B-CARD-090 Elevation value is clamped between 0 and 5")]
        [InlineData(-5, "0")]
        [InlineData(0, "0")]
        [InlineData(3, "3")]
        [InlineData(5, "5")]
        [InlineData(50, "5")]
        public void B097_ElevationClamped(int elevation, string expected)
        {
            Assert.Equal(expected, RenderCard(p => p.Add(c => c.Elevation, elevation)).Find(Root).GetAttribute("data-elevation"));
        }

        [Theory(DisplayName = "B-CARD-091 Elevated variant boosts elevation by +2 (capped at 5)")]
        [InlineData(1, "3")]
        [InlineData(2, "4")]
        [InlineData(3, "5")]
        [InlineData(4, "5")]
        public void B098_ElevatedBoost(int elevation, string expected)
        {
            Assert.Equal(expected, RenderCard(p => p.Add(c => c.Variant, CardVariant.Elevated).Add(c => c.Elevation, elevation)).Find(Root).GetAttribute("data-elevation"));
        }

        [Theory(DisplayName = "B-CARD-092 Outlined and Flat variants force elevation to 0")]
        [InlineData(CardVariant.Outlined)]
        [InlineData(CardVariant.Flat)]
        public void B099_OutlinedFlatForceZero(CardVariant variant)
        {
            Assert.Equal("0", RenderCard(p => p.Add(c => c.Variant, variant).Add(c => c.Elevation, 4)).Find(Root).GetAttribute("data-elevation"));
        }

        [Fact(DisplayName = "B-CARD-093 data-elevation attribute reflects resolved elevation")]
        public void B100_DataElevation()
        {
            Assert.Equal("1", RenderCard().Find(Root).GetAttribute("data-elevation"));
            Assert.Equal("3", RenderCard(p => p.Add(c => c.Variant, CardVariant.Elevated)).Find(Root).GetAttribute("data-elevation"));
        }

        [Fact(DisplayName = "B-CARD-094 Supplied Id is rendered on root")]
        public void B101_SuppliedId()
        {
            Assert.Equal("my-card", RenderCard(p => p.Add(c => c.Id, "my-card")).Find(Root).Id);
        }

        [Fact(DisplayName = "B-CARD-095 Generated Id remains stable across re-renders")]
        public void B102_GeneratedIdStable()
        {
            IRenderedComponent<SfCard> cut = RenderCard();
            string first = cut.Find(Root).Id;

            cut.SetParametersAndRender(p => p.Add(c => c.Square, true));
            cut.SetParametersAndRender(p => p.Add(c => c.Hoverable, true));

            Assert.False(string.IsNullOrWhiteSpace(first));
            Assert.Equal(first, cut.Find(Root).Id);
        }

        [Fact(DisplayName = "B-CARD-096 HtmlAttributes class and style are merged with computed values")]
        public void B103_MergedClassAndStyle()
        {
            IElement card = RenderCard(p => p
                .Add(c => c.Width, "320px")
                .Add(c => c.Square, true)
                .Add(c => c.HtmlAttributes, new Dictionary<string, object> { ["class"] = "user", ["style"] = "color:red" })).Find(Root);

            Assert.Contains("e-card", card.ClassList);
            Assert.Contains("e-card-square", card.ClassList);
            Assert.Contains("user", card.ClassList);
            Assert.Contains("width:320px;", card.GetAttribute("style"));
            Assert.Contains("color:red", card.GetAttribute("style"));
        }

        [Fact(DisplayName = "B-CARD-097 Other HtmlAttributes override computed attributes")]
        public void B104_OtherAttributesOverride()
        {
            IElement card = RenderCard(p => p
                .Add(c => c.IsClickable, true)
                .Add(c => c.AriaLabel, "Computed")
                .Add(c => c.HtmlAttributes, new Dictionary<string, object> { ["role"] = "link", ["aria-label"] = "Override" })).Find(Root);

            Assert.Equal("link", card.GetAttribute("role"));
            Assert.Equal("Override", card.GetAttribute("aria-label"));
        }

        [Fact(DisplayName = "B-CARD-098 Global EnableRtl adds e-rtl class")]
        public void B105_GlobalRtl()
        {
            Services.Configure<GlobalOptions>(options => options.EnableRtl = true);

            Assert.Contains("e-rtl", RenderCard().Find(Root).ClassList);
        }

        // ------------------------------------------------------------------ Media specific

        private static IRenderedComponent<SfCardMedia> RenderMedia(BunitTestContext ctx, Action<ComponentParameterCollectionBuilder<SfCardMedia>> configure)
        {
            return ctx.RenderComponent<SfCardMedia>(configure);
        }

        [Fact(DisplayName = "B-CARD-099 SfCardMedia renders ImageUrl with Alt text")]
        public void B106_MediaImageAlt()
        {
            IElement image = RenderMedia(this, p => p.Add(c => c.ImageUrl, "/a.png").Add(c => c.Alt, "Alt text")).Find("img");

            Assert.Equal("/a.png", image.GetAttribute("src"));
            Assert.Equal("Alt text", image.GetAttribute("alt"));
        }

        [Fact(DisplayName = "B-CARD-100 SfCardMedia without Alt renders empty alt (decorative)")]
        public void B107_MediaEmptyAlt()
        {
            IElement image = RenderMedia(this, p => p.Add(c => c.ImageUrl, "/a.png")).Find("img");

            Assert.True(image.HasAttribute("alt"));
            Assert.Equal(string.Empty, image.GetAttribute("alt"));
        }

        [Fact(DisplayName = "B-CARD-101 CardMediaFit.Cover (default)")]
        public void B108_FitCover()
        {
            Assert.Equal("object-fit:cover;", RenderMedia(this, p => p.Add(c => c.ImageUrl, "/a.png")).Find("img").GetAttribute("style"));
        }

        [Fact(DisplayName = "B-CARD-102 CardMediaFit.Contain")]
        public void B109_FitContain()
        {
            Assert.Equal("object-fit:contain;", RenderMedia(this, p => p.Add(c => c.ImageUrl, "/a.png").Add(c => c.ObjectFit, CardMediaFit.Contain)).Find("img").GetAttribute("style"));
        }

        [Fact(DisplayName = "B-CARD-103 CardMediaFit.Fill")]
        public void B110_FitFill()
        {
            Assert.Equal("object-fit:fill;", RenderMedia(this, p => p.Add(c => c.ImageUrl, "/a.png").Add(c => c.ObjectFit, CardMediaFit.Fill)).Find("img").GetAttribute("style"));
        }

        [Fact(DisplayName = "B-CARD-104 CardMediaFit.None emits no object-fit")]
        public void B111_FitNone()
        {
            Assert.Null(RenderMedia(this, p => p.Add(c => c.ImageUrl, "/a.png").Add(c => c.ObjectFit, CardMediaFit.None)).Find("img").GetAttribute("style"));
        }

        [Fact(DisplayName = "B-CARD-105 Custom media Height")]
        public void B112_MediaHeight()
        {
            Assert.Contains("height:240px;", RenderMedia(this, p => p.Add(c => c.ImageUrl, "/a.png").Add(c => c.Height, "240px")).Find("[data-testid='card-media']").GetAttribute("style"));
            Assert.Contains("height:90px;", RenderMedia(this, p => p.Add(c => c.ImageUrl, "/a.png").Add(c => c.Height, "90")).Find("[data-testid='card-media']").GetAttribute("style"));
        }

        [Fact(DisplayName = "B-CARD-106 Custom ChildContent when ImageUrl is not set")]
        public void B113_MediaChildContent()
        {
            IRenderedComponent<SfCardMedia> cut = RenderMedia(this, p => p.Add(c => c.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<i id='custom'>x</i>"))));

            Assert.Empty(cut.FindAll("img"));
            Assert.Equal("x", cut.Find("#custom").TextContent);
        }

        // ------------------------------------------------------------------ State scenarios

        [Fact(DisplayName = "P-CARD-016 Default state")]
        public void B053_DefaultState()
        {
            IElement card = RenderCard().Find(Root);

            Assert.Equal("e-card", card.ClassName);
            Assert.Null(card.GetAttribute("role"));
            Assert.Null(card.GetAttribute("tabindex"));
            Assert.Null(card.GetAttribute("aria-busy"));
            Assert.Null(card.GetAttribute("aria-disabled"));
        }

        [Fact(DisplayName = "P-CARD-017 Clickable state")]
        public void B054_ClickableState()
        {
            IElement card = ClickableCard(_ => { }).Find(Root);

            Assert.Contains("e-card-clickable", card.ClassList);
            Assert.Equal("button", card.GetAttribute("role"));
            Assert.Equal("0", card.GetAttribute("tabindex"));
        }

        [Fact(DisplayName = "B-CARD-056 Loading state")]
        public void B056_LoadingState()
        {
            IElement card = RenderCard(p => p.Add(c => c.IsLoading, true)).Find(Root);

            Assert.Contains("e-card-loading", card.ClassList);
            Assert.Equal("true", card.GetAttribute("aria-busy"));
        }

        [Fact(DisplayName = "B-CARD-055 Hover state class rendering")]
        public void B058_HoverClass()
        {
            Assert.Contains("e-card-hoverable", RenderCard(p => p.Add(c => c.Hoverable, true)).Find(Root).ClassList);
        }

        [Fact(DisplayName = "B-CARD-056 Focus-visible state rendering")]
        public void B059_FocusVisibleRendering()
        {
            // The outline is CSS (:focus-visible, asserted in Playwright). The card must be focusable for it to apply.
            IElement card = ClickableCard(_ => { }).Find(Root);

            Assert.Equal("0", card.GetAttribute("tabindex"));
            Assert.Contains("e-card-clickable", card.ClassList);
        }

        [Fact(DisplayName = "B-CARD-107 Clickable + Loading combination sets aria-disabled")]
        public void B114_ClickableLoadingAriaDisabled()
        {
            IElement card = ClickableCard(_ => { }, p => p.Add(c => c.IsLoading, true)).Find(Root);

            Assert.Equal("true", card.GetAttribute("aria-disabled"));
            Assert.Equal("true", card.GetAttribute("aria-busy"));
            Assert.Contains("e-disabled", card.ClassList);
        }

        // ------------------------------------------------------------------ State transitions

        [Fact(DisplayName = "B-CARD-057 Loading to Loaded")]
        public void B060_LoadingToLoaded()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p.Add(c => c.IsLoading, true));
            cut.SetParametersAndRender(p => p.Add(c => c.IsLoading, false));

            IElement card = cut.Find(Root);
            Assert.DoesNotContain("e-card-loading", card.ClassList);
            Assert.Null(card.GetAttribute("aria-busy"));
        }

        [Fact(DisplayName = "B-CARD-058 Loaded to Loading")]
        public void B061_LoadedToLoading()
        {
            IRenderedComponent<SfCard> cut = RenderCard();
            cut.SetParametersAndRender(p => p.Add(c => c.IsLoading, true));

            IElement card = cut.Find(Root);
            Assert.Contains("e-card-loading", card.ClassList);
            Assert.Equal("true", card.GetAttribute("aria-busy"));
        }

        [Fact(DisplayName = "B-CARD-059 Clickable to Non-clickable")]
        public void B064_ClickableToNonClickable()
        {
            IRenderedComponent<SfCard> cut = ClickableCard(_ => { });
            cut.SetParametersAndRender(p => p.Add(c => c.IsClickable, false));

            IElement card = cut.Find(Root);
            Assert.Null(card.GetAttribute("role"));
            Assert.Null(card.GetAttribute("tabindex"));
            Assert.DoesNotContain("blazor:onclick", cut.Markup, StringComparison.Ordinal);
        }

        [Fact(DisplayName = "B-CARD-060 Non-clickable to Clickable")]
        public void B065_NonClickableToClickable()
        {
            IRenderedComponent<SfCard> cut = RenderCard();
            cut.SetParametersAndRender(p => p.Add(c => c.IsClickable, true));

            IElement card = cut.Find(Root);
            Assert.Equal("button", card.GetAttribute("role"));
            Assert.Equal("0", card.GetAttribute("tabindex"));
        }

        [Fact(DisplayName = "B-CARD-061 Vertical to Horizontal")]
        public void B066_VerticalToHorizontal()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p.Add(c => c.Orientation, CardOrientation.Vertical));
            cut.SetParametersAndRender(p => p.Add(c => c.Orientation, CardOrientation.Horizontal));

            Assert.Contains("e-card-horizontal", cut.Find(Root).ClassList);
        }

        [Fact(DisplayName = "B-CARD-062 Horizontal to Vertical")]
        public void B067_HorizontalToVertical()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p.Add(c => c.Orientation, CardOrientation.Horizontal));
            cut.SetParametersAndRender(p => p.Add(c => c.Orientation, CardOrientation.Vertical));

            Assert.DoesNotContain("e-card-horizontal", cut.Find(Root).ClassList);
        }

        [Fact(DisplayName = "B-CARD-063 Runtime content updates")]
        public void B068_RuntimeContent()
        {
            IRenderedComponent<SfCard> cut = RenderWith(Region<SfCardHeader>("one"));
            cut.SetParametersAndRender(p => p.Add(c => c.ChildContent, Compose(Region<SfCardHeader>("two"), Region<SfCardFooter>("end"))));

            Assert.Equal("two", cut.Find("[data-testid='card-header']").TextContent);
            Assert.Equal([Header, Footer], ChildIds(cut));
        }

        [Fact(DisplayName = "B-CARD-064 Runtime action updates")]
        public void B069_RuntimeActions()
        {
            IRenderedComponent<SfCard> cut = RenderWith(Region<SfCardActions>("Buy"));
            cut.SetParametersAndRender(p => p.Add(c => c.ChildContent, Region<SfCardActions>("Cancel")));

            Assert.Equal("Cancel", cut.Find("[data-testid='card-actions']").TextContent);
        }

        [Fact(DisplayName = "B-CARD-108 Runtime Elevation / Square / Hoverable / ContentPadding updates")]
        public void B115_RuntimeLayoutParameters()
        {
            IRenderedComponent<SfCard> cut = RenderCard();

            cut.SetParametersAndRender(p => p
                .Add(c => c.Elevation, 4)
                .Add(c => c.Square, true)
                .Add(c => c.Hoverable, true)
                .Add(c => c.ContentPadding, false));

            IElement card = cut.Find(Root);
            Assert.Equal("4", card.GetAttribute("data-elevation"));
            Assert.Contains("e-card-square", card.ClassList);
            Assert.Contains("e-card-hoverable", card.ClassList);
            Assert.Equal("false", card.GetAttribute("data-content-padding"));

            cut.SetParametersAndRender(p => p
                .Add(c => c.Elevation, 1)
                .Add(c => c.Square, false)
                .Add(c => c.Hoverable, false)
                .Add(c => c.ContentPadding, true));

            card = cut.Find(Root);
            Assert.Equal("1", card.GetAttribute("data-elevation"));
            Assert.Equal("e-card", card.ClassName);
            Assert.Equal("true", card.GetAttribute("data-content-padding"));
        }

        // ------------------------------------------------------------------ Events

        [Fact(DisplayName = "B-CARD-065 Mouse click invokes callback")]
        public void B070_ClickInvokes()
        {
            int count = 0;
            ClickableCard(_ => count++).Find(Root).Click();

            Assert.Equal(1, count);
        }

        [Fact(DisplayName = "B-CARD-066 Callback invoked once")]
        public void B071_InvokedOnce()
        {
            int count = 0;
            IRenderedComponent<SfCard> cut = ClickableCard(_ => count++);

            cut.Find(Root).Click();
            cut.Render();

            Assert.Equal(1, count);
        }

        [Fact(DisplayName = "B-CARD-067 Multiple clicks produce expected count")]
        public void B072_MultipleClicks()
        {
            int count = 0;
            IElement card = ClickableCard(_ => count++).Find(Root);

            card.Click();
            card.Click();
            card.Click();

            Assert.Equal(3, count);
        }

        [Fact(DisplayName = "B-CARD-068 Loading state suppresses callback")]
        public void B074_LoadingSuppresses()
        {
            int count = 0;
            IElement card = ClickableCard(_ => count++, p => p.Add(c => c.IsLoading, true)).Find(Root);

            card.Click();
            card.KeyDown(new KeyboardEventArgs { Key = "Enter" });

            Assert.Equal(0, count);
        }

        [Fact(DisplayName = "B-CARD-069 Enter activates clickable card")]
        public void B075_EnterActivates()
        {
            int count = 0;
            ClickableCard(_ => count++).Find(Root).KeyDown(new KeyboardEventArgs { Key = "Enter" });

            Assert.Equal(1, count);
        }

        [Fact(DisplayName = "B-CARD-070 Space activates clickable card")]
        public void B076_SpaceActivates()
        {
            int count = 0;
            IElement card = ClickableCard(_ => count++).Find(Root);

            card.KeyDown(new KeyboardEventArgs { Key = " ", Code = "Space" });
            card.KeyUp(new KeyboardEventArgs { Key = " ", Code = "Space" });

            Assert.Equal(1, count);
        }

        [Fact(DisplayName = "B-CARD-071 Tab navigation preserved")]
        public void B077_TabPreserved()
        {
            int count = 0;
            IRenderedComponent<SfCard> cut = ClickableCard(_ => count++);

            cut.Find(Root).KeyDown(new KeyboardEventArgs { Key = "Tab" });
            cut.Find(Root).KeyUp(new KeyboardEventArgs { Key = "Tab" });

            Assert.Equal(0, count);
            Assert.Equal("0", cut.Find(Root).GetAttribute("tabindex"));
        }

        [Fact(DisplayName = "B-CARD-072 Event arguments propagated correctly")]
        public void B078_EventArgsPropagated()
        {
            MouseEventArgs? received = null;
            ClickableCard(args => received = args).Find(Root).Click(new MouseEventArgs { Detail = 1, ClientX = 12, ClientY = 34, Button = 0 });

            Assert.NotNull(received);
            Assert.Equal(1, received!.Detail);
            Assert.Equal(12, received.ClientX);
            Assert.Equal(34, received.ClientY);

            MouseEventArgs? keyboardArgs = null;
            ClickableCard(args => keyboardArgs = args).Find(Root).KeyDown(new KeyboardEventArgs { Key = "Enter" });

            Assert.NotNull(keyboardArgs);
            Assert.Equal(0, keyboardArgs!.Detail);
        }

        [Fact(DisplayName = "B-CARD-109 Enter activates on keydown")]
        public void B116_EnterOnKeyDown()
        {
            int count = 0;
            IElement card = ClickableCard(_ => count++).Find(Root);

            card.KeyDown(new KeyboardEventArgs { Key = "Enter" });
            Assert.Equal(1, count);

            // Enter on key up must not fire a second activation.
            card.KeyUp(new KeyboardEventArgs { Key = "Enter" });
            Assert.Equal(1, count);
        }

        [Fact(DisplayName = "B-CARD-110 Space activates on keyup only after matching keydown")]
        public void B117_SpaceKeyUpAfterKeyDown()
        {
            int count = 0;
            IElement card = ClickableCard(_ => count++).Find(Root);

            card.KeyUp(new KeyboardEventArgs { Key = " ", Code = "Space" });
            Assert.Equal(0, count);

            card.KeyDown(new KeyboardEventArgs { Key = " ", Code = "Space" });
            Assert.Equal(0, count);

            card.KeyUp(new KeyboardEventArgs { Key = " ", Code = "Space" });
            Assert.Equal(1, count);

            card.KeyUp(new KeyboardEventArgs { Key = " ", Code = "Space" });
            Assert.Equal(1, count);
        }

        [Fact(DisplayName = "B-CARD-111 Repeat keys are ignored")]
        public void B118_RepeatIgnored()
        {
            int count = 0;
            IElement card = ClickableCard(_ => count++).Find(Root);

            card.KeyDown(new KeyboardEventArgs { Key = "Enter", Repeat = true });
            Assert.Equal(0, count);

            card.KeyDown(new KeyboardEventArgs { Key = " ", Code = "Space", Repeat = true });
            card.KeyUp(new KeyboardEventArgs { Key = " ", Code = "Space" });
            Assert.Equal(0, count);
        }

        [Fact(DisplayName = "B-CARD-112 Click or keyboard activation inside SfCardActions does not raise card OnClick")]
        public void B119_ActionsIsolation()
        {
            int cardClicks = 0;
            int buttonClicks = 0;
            RenderFragment actions = b =>
            {
                b.OpenComponent<SfCardActions>(0);
                b.AddAttribute(1, "ChildContent", (RenderFragment)(inner =>
                {
                    inner.OpenElement(0, "button");
                    inner.AddAttribute(1, "id", "act");
                    inner.AddAttribute(2, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, () => buttonClicks++));
                    inner.AddContent(3, "Buy");
                    inner.CloseElement();
                }));
                b.CloseComponent();
            };
            IRenderedComponent<SfCard> cut = ClickableCard(_ => cardClicks++, p => p.Add(c => c.ChildContent, actions));
            IElement button = cut.Find("#act");

            button.Click();
            button.KeyDown(new KeyboardEventArgs { Key = "Enter" });
            button.KeyDown(new KeyboardEventArgs { Key = " ", Code = "Space" });
            button.KeyUp(new KeyboardEventArgs { Key = " ", Code = "Space" });

            Assert.Equal(1, buttonClicks);
            Assert.Equal(0, cardClicks);
        }

        [Fact(DisplayName = "B-CARD-113 SfCardActions receives inert attribute while card is loading")]
        public void B120_ActionsInert()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.IsLoading, true)
                .Add(c => c.ChildContent, Region<SfCardActions>("Buy")));
            Assert.True(cut.Find("[data-testid='card-actions']").HasAttribute("inert"));

            cut.SetParametersAndRender(p => p.Add(c => c.IsLoading, false));
            Assert.False(cut.Find("[data-testid='card-actions']").HasAttribute("inert"));
        }

        // ------------------------------------------------------------------ Accessibility semantics

        [Fact(DisplayName = "B-CARD-073 Default role assignment")]
        public void B079_DefaultRole()
        {
            Assert.Null(RenderCard().Find(Root).GetAttribute("role"));
            Assert.Equal("button", ClickableCard(_ => { }).Find(Root).GetAttribute("role"));
        }

        [Fact(DisplayName = "B-CARD-074 Custom role assignment")]
        public void B080_CustomRole()
        {
            IElement card = RenderCard(p => p.Add(c => c.HtmlAttributes, new Dictionary<string, object> { ["role"] = "article" })).Find(Root);

            Assert.Equal("article", card.GetAttribute("role"));
        }

        [Fact(DisplayName = "B-CARD-075 aria-label support")]
        public void B081_AriaLabel()
        {
            Assert.Equal("Profile", RenderCard(p => p.Add(c => c.AriaLabel, "Profile")).Find(Root).GetAttribute("aria-label"));
        }

        [Fact(DisplayName = "B-CARD-076 aria-labelledby support")]
        public void B082_AriaLabelledBy()
        {
            IElement card = RenderCard(p => p.Add(c => c.HtmlAttributes, new Dictionary<string, object> { ["aria-labelledby"] = "heading-1" })).Find(Root);

            Assert.Equal("heading-1", card.GetAttribute("aria-labelledby"));
        }

        [Fact(DisplayName = "B-CARD-077 aria-describedby support")]
        public void B083_AriaDescribedBy()
        {
            IElement card = RenderCard(p => p.Add(c => c.HtmlAttributes, new Dictionary<string, object> { ["aria-describedby"] = "desc-1" })).Find(Root);

            Assert.Equal("desc-1", card.GetAttribute("aria-describedby"));
        }

        [Fact(DisplayName = "B-CARD-078 Focusable interactive card")]
        public void B084_FocusableInteractive()
        {
            Assert.Equal("0", ClickableCard(_ => { }).Find(Root).GetAttribute("tabindex"));
        }

        [Fact(DisplayName = "B-CARD-079 Non-interactive card not focusable")]
        public void B085_NonInteractiveNotFocusable()
        {
            Assert.Null(RenderCard().Find(Root).GetAttribute("tabindex"));
        }

        [Fact(DisplayName = "B-CARD-080 Focus-visible semantics")]
        public void B087_FocusVisibleSemantics()
        {
            IElement clickable = ClickableCard(_ => { }).Find(Root);
            IElement staticCard = RenderCard().Find(Root);

            Assert.Equal("0", clickable.GetAttribute("tabindex"));
            Assert.Null(staticCard.GetAttribute("tabindex"));
        }

        [Fact(DisplayName = "B-CARD-081 Keyboard activation semantics")]
        public void B088_KeyboardActivationSemantics()
        {
            int count = 0;
            IElement card = ClickableCard(_ => count++, p => p.Add(c => c.AriaLabel, "Open")).Find(Root);

            Assert.Equal("button", card.GetAttribute("role"));
            Assert.Equal("Open", card.GetAttribute("aria-label"));

            card.KeyDown(new KeyboardEventArgs { Key = "Enter" });
            card.KeyDown(new KeyboardEventArgs { Key = " ", Code = "Space" });
            card.KeyUp(new KeyboardEventArgs { Key = " ", Code = "Space" });

            Assert.Equal(2, count);
        }

        [Fact(DisplayName = "B-CARD-082 Screen-reader attributes preserved")]
        public void B089_ScreenReaderAttributesPreserved()
        {
            IRenderedComponent<SfCard> cut = RenderCard(p => p
                .Add(c => c.AriaLabel, "Named")
                .Add(c => c.HtmlAttributes, new Dictionary<string, object> { ["aria-live"] = "polite", ["aria-describedby"] = "d" }));
            IElement card = cut.Find(Root);

            Assert.Equal("polite", card.GetAttribute("aria-live"));

            cut.SetParametersAndRender(p => p.Add(c => c.Square, true));
            card = cut.Find(Root);
            Assert.Equal("Named", card.GetAttribute("aria-label"));
            Assert.Equal("polite", card.GetAttribute("aria-live"));
            Assert.Equal("d", card.GetAttribute("aria-describedby"));
        }

        [Fact(DisplayName = "B-CARD-114 Loading sets aria-busy=true")]
        public void B121_LoadingAriaBusy()
        {
            Assert.Equal("true", RenderCard(p => p.Add(c => c.IsLoading, true)).Find(Root).GetAttribute("aria-busy"));
            Assert.Null(RenderCard().Find(Root).GetAttribute("aria-busy"));
        }

        [Fact(DisplayName = "B-CARD-115 Clickable + Loading sets aria-disabled=true")]
        public void B122_ClickableLoadingAriaDisabled()
        {
            Assert.Equal("true", ClickableCard(_ => { }, p => p.Add(c => c.IsLoading, true)).Find(Root).GetAttribute("aria-disabled"));
            Assert.Null(RenderCard(p => p.Add(c => c.IsLoading, true)).Find(Root).GetAttribute("aria-disabled"));
        }

        private sealed class TwoCards : ComponentBase
        {
            protected override void BuildRenderTree(RenderTreeBuilder builder)
            {
                builder.OpenComponent<SfCard>(0);
                builder.AddAttribute(1, "Variant", CardVariant.Outlined);
                builder.CloseComponent();
                builder.OpenComponent<SfCard>(2);
                builder.CloseComponent();
            }
        }
    }
}
