using Microsoft.AspNetCore.Components;
namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public partial class SfRadioButton<TChecked>
    {
        #region Members

        /// <summary>
        /// Gets or sets the text label shown next to the radio button.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> that represents the radio button's label text. The default is <c>null</c>.
        /// </value>
        /// <remarks>
        /// <para>
        /// The <see cref="Label"/> property provides simple text content displayed adjacent to the radio input. For accessibility and localization,
        /// prefer providing meaningful labels that describe the option conveyed by the radio button.
        /// </para>
        /// <para>
        /// The label text is rendered as plain text and is automatically HTML-encoded by Blazor, preventing XSS (Cross-Site Scripting) attacks.
        /// </para>
        /// </remarks>
        /// <example>
        /// The following examples show how to set the label for a radio button:
        /// <code><![CDATA[
        /// <!-- Simple label -->
        /// <SfRadioButton Label="Option A" />
        /// 
        /// <!-- Label with data binding -->
        /// <SfRadioButton Label="@optionLabel" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the position of the label relative to the radio input element.
        /// </summary>
        /// <value>
        /// A <see cref="LabelPosition"/> enumeration value indicating whether the label appears before (left) or after (right) the radio input. The default is <see cref="LabelPosition.Before"/>.
        /// </value>
        /// <remarks>
        /// Changing the <see cref="LabelPosition"/> adjusts the visual ordering of the label and input which can improve readability in different locales and when supporting RTL layouts.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <!-- Label shown after the radio input -->
        /// <SfRadioButton Label="Option" LabelPosition="LabelPosition.After" />
        /// ]]></code>
        /// </example>
        [Parameter]
        public LabelPosition LabelPosition { get; set; }

        #endregion
    }
}
