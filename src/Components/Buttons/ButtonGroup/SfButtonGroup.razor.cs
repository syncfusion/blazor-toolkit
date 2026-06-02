using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Buttons
{
    /// <summary>
    /// The ButtonGroup component is a container that groups a series of buttons on a single line and supports different selection modes, such as single and multiple selections.
    /// </summary>
    /// <remarks>
    /// The ButtonGroup can contain <see cref="Button"/> components, as well as other button components like <c>DropDownButton</c> or <c>SplitButton</c>. To learn more about the ButtonGroup and its features, you can refer to the <a href="https://blazor.syncfusion.com/documentation/button-group/getting-started">Syncfusion Blazor ButtonGroup</a> documentation.
    /// </remarks>
    /// <example>
    /// The following example demonstrates a basic ButtonGroup with several <see cref="Button"/> instances.
    /// <code><![CDATA[ 
    /// <SfButtonGroup> 
    /// <Button>Left</Button> 
    /// <Button>Center</Button> 
    /// <Button>Right</Button> f
    /// </SfButtonGroup> 
    /// ]]></code> 
    /// </example>
    public partial class SfButtonGroup
    {
        #region Fields

        /// <exclude />
        /// <summary>
        /// Unique name used for input elements when none is provided by the consumer.
        /// </summary>
        internal readonly string _inputName = SfBaseUtils.GenerateID("SfButtonGroup");

        /// <exclude />
        /// <summary>
        /// Registered child buttons in the group.
        /// </summary>
        internal List<Button>? _buttonItems = [];

        /// <exclude />
        /// <summary>
        /// Indicates that a click originated from a child and is being processed.
        /// </summary>
        internal bool _isClicked;

        #endregion

        #region Helper Methods

        /// <exclude />
        /// <summary>
        /// Registers a child <see cref="Button"/> with this group.
        /// </summary>
        /// <param name="Button">The button instance to register.</param>
        /// <example>
        /// Child components register automatically during initialization. For testing or advanced scenarios you can register
        /// a button instance manually:
        /// <code><![CDATA[
        /// var group = new SfButtonGroup();
        /// var button = new Button();
        /// group.UpdateChildProperty(button);
        /// ]]></code>
        /// </example>
        internal void UpdateChildProperty(Button Button)
        {
            if (Button is null || _buttonItems is null)
            {
                return;
            }

            if (!_buttonItems.Contains(Button))
            {
                _buttonItems.Add(Button);
            }
        }

        /// <exclude />
        /// <summary>
        /// Unregisters a child <see cref="Button"/> from this group.
        /// </summary>
        /// <param name="button">The button instance to remove.</param>
        /// <example>
        /// Remove a previously registered child button from the group.
        /// <code><![CDATA[
        /// group.RemoveChildProperty(button);
        /// ]]></code>
        /// </example>
        internal void RemoveChildProperty(Button? button)
        {
            if (button is null || _buttonItems is null)
            {
                return;
            }

            if (_buttonItems.Contains(button))
            {
                _ = _buttonItems.Remove(button);
            }
        }

        /// <exclude />
        /// <summary>
        /// Builds the CSS class string for the ButtonGroup based on configuration.
        /// </summary>
        /// <returns>A string containing all applicable CSS classes.</returns>
        private string BuildCssClass()
        {
            List<string> classes = ["e-btn-group"];

            // Add RTL support
            if (SyncfusionService?._options?.EnableRtl ?? false)
            {
                classes.Add("e-rtl");
            }

            // Add vertical layout class if IsVertical is true
            if (IsVertical)
            {
                classes.Add("e-vertical");
            }

            // Add custom CSS classes
            if (!string.IsNullOrEmpty(CssClass))
            {
                classes.Add(CssClass);
            }

            return string.Join(" ", classes);
        }

        /// <exclude />
        /// <summary>
        /// Merges HTML attributes with default attributes for the root element.
        /// </summary>
        /// <returns>A dictionary containing merged attributes.</returns>
        private Dictionary<string, object> GetMergedAttributes()
        {
            Dictionary<string, object> mergedAttrs = new(HtmlAttributes ?? []);

            if (Mode == SelectionMode.Single)
            {
                mergedAttrs["aria-label"] = "Button group with single selection mode";
            }

            return mergedAttrs;
        }

        #endregion
    }
}
