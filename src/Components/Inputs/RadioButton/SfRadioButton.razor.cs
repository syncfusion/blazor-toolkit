using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// Represents a RadioButton component, which is a graphical interface element that allows users to select a single option from a group. It supports checked and unchecked states for user interaction and custom logic.
    /// </summary>
    /// <remarks>
    /// This generic component allows flexible data binding to various types for checked state tracking. The RadioButton supports RTL, persistence, label positioning,
    /// and can be easily customized via its component parameters. This class should be used when you want to offer a mutually exclusive choice selection within a UI form or group.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to initialize a basic radio button using the <c>Checked</c> property.
    /// <code><![CDATA[
    /// <SfRadioButton Checked="true">
    /// </SfRadioButton>
    /// ]]></code>
    /// </example>
    public partial class SfRadioButton<TChecked> : SfSelectionBase<TChecked>
    {
        #region Constants
        /// <summary>A single space character used for CSS class concatenation.</summary>
        private const string Space = " ";

        /// <summary>String literal representing a null-valued radio selection from localStorage.</summary>
        private const string NullLocalStorageValue = "null";

        /// <summary>CSS class name for RTL layout support.</summary>
        private const string Rtl = "e-rtl";

        /// <summary>CSS class name for right alignment.</summary>
        private const string Right = "e-right";

        /// <summary>CSS class name for root radio button wrapper element.</summary>
        private const string RootClass = "e-radio-wrapper e-wrapper";

        /// <summary>CSS class name for radio button control element.</summary>
        private const string RadioButtonClass = "e-control e-radio e-lib";

        /// <summary>HTML input type attribute value.</summary>
        private const string InputType = "radio";

        /// <summary>CSS class name for label element.</summary>
        private const string LabelClass = "e-label";
        #endregion

        #region Fields
        /// <summary>The computed root CSS class for the radio button element.</summary>
        private string _rootClass = RootClass;

        /// <summary>The computed label CSS class for label positioning and RTL support.</summary>
        private string? _labelClass;

        /// <summary>Tracks whether the radio button is currently checked.</summary>
        private bool _isChecked;

        /// <summary>Tracks whether this is the first load to handle persistence restoration.</summary>
        private bool _isFirstLoad;
        #endregion

        #region Injected Services
        /// <summary>
        /// Gets the logger instance used for recording component errors.
        /// </summary>
        [Inject]
        public ILogger<SfRadioButton<TChecked>>? Logger { get; set; }
        #endregion

        #region Protected Methods
        /// <inheritdoc />
        protected override void InitRender(bool isDynamic = false)
        {
            _rootClass = RootClass;

            _isChecked = Checked is not null && Value is not null && Value is not NullLocalStorageValue && !EnablePersistence && Checked.Equals(TryParseValueFromString(Value));

            if (!string.IsNullOrEmpty(CssClass))
            {
                _rootClass += Space + CssClass;
            }

            bool shouldApplyRtl = SyncfusionService?._options?.EnableRtl ?? false;
            _labelClass = LabelPosition == LabelPosition.Before
                ? Right + Space + Rtl
                : shouldApplyRtl
                    ? Rtl
                    : LabelPosition == LabelPosition.Before
                        ? Right
                        : null;
        }
        #endregion
    }
}
