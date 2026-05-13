using System.Threading.Tasks;
using Xunit;
using Bunit;
using Syncfusion.Blazor.Toolkit.Inputs;
using Syncfusion.Blazor.Toolkit.Tests;

namespace Syncfusion.Blazor.Toolkit.Tests.Inputs.Uploader
{
    /// <summary>
    /// Accessibility and keyboard interaction tests for <see cref="SfUploader"/>.
    /// </summary>
    /// <remarks>
    /// Feature Group: A11y & Keyboard
    /// </remarks>
    public class SfUploaderKeyboardAndA11y_Tests : BunitTestContext
    {
        [Fact(Timeout = 10000)]
        [Trait("Category", "A11y & Keyboard")]
        public void BrowseButton_IsTabbable_WithCustomTabIndex()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.TabIndex, 7));

            // Assert
            var browseBtn = uploader.Find("button.e-upload-browse-btn");
            Assert.Equal("7", browseBtn.GetAttribute("tabindex"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "A11y & Keyboard")]
        public void AriaAttributes_Toggle_WithEnabledState()
        {
            // Arrange
            var uploader = RenderComponent<SfUploader>(parameters => parameters
                .Add(p => p.Disabled, true));

            // Assert disabled state
            var input = uploader.Find("input");
            Assert.Equal("disabled", input.GetAttribute("disabled"));
            Assert.Equal("true", input.GetAttribute("aria-disabled"));

            // Act - enable and re-check
            uploader.SetParametersAndRender(ps => ps.Add(p => p.Disabled, false));
            input = uploader.Find("input");

            // Assert enabled state
            Assert.Null(input.GetAttribute("disabled"));
            Assert.Null(input.GetAttribute("aria-disabled"));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "A11y & Keyboard")]
        public void BrowseButton_HasTitleAndAriaLabel()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>();

            // Assert
            var browseBtn = uploader.Find("button.e-upload-browse-btn");
            Assert.Equal("Browse for file to upload", browseBtn.GetAttribute("aria-label"));
            Assert.False(string.IsNullOrEmpty(browseBtn.GetAttribute("title")));
        }

        [Fact(Timeout = 10000)]
        [Trait("Category", "A11y & Keyboard")]
        public void Input_IsHiddenFromTabFocus_ByDefault()
        {
            // Arrange & Act
            var uploader = RenderComponent<SfUploader>();

            // Assert input is not tabbable by default (tabindex set to -1)
            var input = uploader.Find("input");
            Assert.Equal("-1", input.GetAttribute("tabindex"));
        }
    }
}
