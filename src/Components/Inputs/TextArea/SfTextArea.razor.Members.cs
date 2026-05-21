using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    /// <summary>
    /// The TextArea is an textarea element that allows to get input from the user. It allows the user to edit or display the text value.
    /// </summary>
    public partial class SfTextArea : SfInputBase<string>
    {
        private string _cssClass = string.Empty;

        /// <summary> 
        /// Gets or sets the floating label behavior of the <see cref="SfTextArea"/>. The <see cref="Placeholder"/> text acts as a label. 
        /// </summary> 
        /// <value> 
        /// One of the <see cref="FloatLabelType"/> enumeration. The default value is <see cref="FloatLabelType.Never"/>. 
        /// </value> 
        /// <remarks>  
        /// <list type="bullet"> 
        /// <item><description>If the <see cref="FloatLabelType"/> is <see cref="FloatLabelType.Never"/>, the placeholder text does not float as a label.</description></item> 
        /// <item><description>If the <see cref="FloatLabelType"/> is <see cref="FloatLabelType.Auto"/>, the placeholder text will float above the <see cref="SfTextArea"/> component as a label after focusing on it.</description></item> 
        /// <item><description>If the <see cref="FloatLabelType"/> is <see cref="FloatLabelType.Always"/>, the placeholder text is displayed as a label above the  <see cref="SfTextArea"/> component.</description></item> 
        /// </list>  
        /// </remarks> 
        /// <example> 
        /// In the following code example, set the float label as <see cref="FloatLabelType.Auto"/>. 
        /// <code><![CDATA[ 
        ///   <SfTextArea FloatLabelType="FloatLabelType.Auto" Placeholder="Enter a value"></SfTextArea> 
        /// ]]></code> 
        /// </example>
        [Parameter]
        public FloatLabelType FloatLabelType
        {
            get => BaseFloatLabelType;
            set => BaseFloatLabelType = value;
        }
        /// <inheritdoc/>
        /// <exclude/>
        protected override FloatLabelType BaseFloatLabelType { get; set; }

        /// <summary> 
        /// Gets or sets the text that is shown as a hint or placeholder until the user focuses on the <see cref="SfTextArea"/>. 
        /// </summary> 
        /// <value> 
        /// A string representing the placeholder text. The default value is <c>null</c>. 
        /// </value> 
        /// <remarks> 
        /// The Placeholder property specifies the text that appears in the <see cref="SfTextArea"/> when it is empty, providing a hint to the user about the expected input. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfTextArea Placeholder="Enter text" RowCount="4"></SfTextArea> 
        /// ]]></code> 
        /// </example>
        [Parameter]
        public string Placeholder
        {
            get => BasePlaceholder;
            set => BasePlaceholder = value;
        }

        /// <inheritdoc/>
        /// <exclude/>
        protected override string BasePlaceholder { get; set; } = default!;

        /// <summary> 
        /// Gets or sets a value indicating whether the <see cref="SfTextArea"/> is read-only. 
        /// </summary> 
        /// <value> 
        /// A boolean indicating whether the <see cref="SfTextArea"/> is read-only. The default value is <c>false</c>. 
        /// </value> 
        /// <remarks> 
        /// Set this property to <c>true</c> to make the <see cref="SfTextArea"/> read-only, preventing user input. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfTextArea ReadOnly="true" Placeholder="Read-only text" RowCount="4"></SfTextArea> 
        /// ]]></code> 
        /// </example>
        [Parameter]
        public bool ReadOnly
        {
            get => BaseReadOnly;
            set => BaseReadOnly = value;
        }

        /// <inheritdoc/>
        /// <exclude/>
        protected override bool BaseReadOnly { get; set; }

        /// <summary> 
        /// Gets or sets a value indicating whether the clear button is displayed in the <see cref="SfTextArea"/>. 
        /// </summary> 
        /// <value> 
        /// A boolean indicating whether the clear button is displayed. The default value is <c>false</c>. 
        /// </value> 
        /// <remarks> 
        /// Set this property to <c>true</c> to show a clear button in the <see cref="SfTextArea"/>, allowing users to quickly remove all text content. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfTextArea ShowClearButton="true" Placeholder="Enter text" RowCount="4"></SfTextArea> 
        /// ]]></code> 
        /// </example>
        [Parameter]
        public bool ShowClearButton
        {
            get => BaseShowClearButton;
            set => BaseShowClearButton = value;
        }

        /// <inheritdoc/>
        /// <exclude/>
        protected override bool BaseShowClearButton { get; set; }

        /// <summary> 
        /// Gets or sets the width of the <see cref="SfTextArea"/> component. 
        /// </summary> 
        /// <value> 
        /// A string representing the width of the <see cref="SfTextArea"/>. The default value is <c>null</c>. 
        /// </value> 
        /// <remarks> 
        /// Use this property to specify the width of the <see cref="SfTextArea"/>, supporting various units such as auto, cm, mm, in, px, pt, pc, %, em, ex, ch, rem, vw, vh, vmin, vmax. 
        /// If the <see cref="Width"/> property is not set, the width of the <see cref="SfTextArea"/> is determined by the <see cref="ColumnCount"/> property. 
        /// The <see cref="ColumnCount"/> property specifies the visible width of the <see cref="SfTextArea"/>, measured in average character widths, with a default value of 20. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfTextArea Width="300px" Placeholder="Enter text" RowCount="4"></SfTextArea> 
        /// ]]></code> 
        /// </example>
        [Parameter]
        public string? Width { get; set; } = null;

        private string? _width;

        /// <summary>
        /// Gets or sets the tab order of the <see cref="SfTextArea"/> component.
        /// </summary>
        /// <value>
        /// An integer value representing the tab index of the <see cref="SfTextArea"/> component. The default value is 0.
        /// </value>
        /// <remarks>
        /// The TabIndex property determines the order in which the <see cref="SfTextArea"/> component receives focus when users navigate through the page using the Tab key.
        /// A higher TabIndex value means the element will be focused later in the tab sequence.
        /// Setting TabIndex to -1 removes the element from the tab sequence, making it non-focusable via keyboard navigation.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        ///   <SfTextArea TabIndex="1" Placeholder="Enter text"></SfTextArea>
        /// ]]></code>
        /// </example>
        [Parameter]
        public int TabIndex
        {
            get => BaseTabIndex;
            set => BaseTabIndex = value;
        }
        /// <inheritdoc/>
        /// <exclude/>
        protected override int BaseTabIndex { get; set; }

        /// <summary> 
        /// Gets or sets a collection of additional attributes such as styles, class, and more that will be applied to the <see cref="SfTextArea"/> component. 
        /// </summary> 
        /// <value> 
        /// The value as a dictionary collection. The default value is <c>null</c>. 
        /// </value> 
        /// <remarks> 
        /// If you configure both the property and equivalent HTML attributes, the component considers the property value. 
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfTextArea HtmlAttributes="@CustomAttribute"> 
        /// </SfTextArea> 
        /// @code{ 
        ///     Dictionary<string, object> CustomAttribute = new Dictionary<string, object>() 
        ///     { 
        ///         { "title", "Enter your text" } 
        ///     }; 
        /// } 
        /// ]]></code> 
        /// </example>       
        [Parameter]
        public Dictionary<string, object>? HtmlAttributes
        {
            get => BaseHtmlAttributes;
            set => BaseHtmlAttributes = value;
        }
        /// <inheritdoc/>
        /// <exclude/>
        protected override Dictionary<string, object>? BaseHtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets a collection of additional input attributes such as disabled, value, and more that will be applied to the <see cref="SfTextArea"/> component.
        /// </summary>
        /// <value>
        /// A dictionary collection containing additional input attributes. The default value is an empty dictionary.
        /// </value>
        /// <remarks>
        /// Additional attributes can be added by specifying them as inline attributes or by using the <c>@attributes</c> directive.
        /// If you configure both a property and its equivalent HTML attribute, the component considers the property value over the HTML attribute.
        /// This property captures unmatched HTML attributes that are not explicitly defined as component parameters.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextArea Placeholder="Enter your Name" @attributes="@CustomAttribute">
        /// </SfTextArea>
        /// @code{
        ///     Dictionary<string, object> CustomAttribute = new Dictionary<string, object>()
        ///     {
        ///         { "maxlength", "4" }
        ///     };
        /// }
        /// ]]></code>
        /// </example>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes
        {
            get => BaseInputAttributes ?? [];
            set => BaseInputAttributes = value;
        }

        /// <inheritdoc/>
        /// <exclude/>
        protected override Dictionary<string, object>? BaseInputAttributes { get; set; } = [];

        /// <summary> 
        /// Gets or sets the maximum number of characters allowed in the <see cref="SfTextArea"/>. 
        /// </summary>
        /// <value> 
        /// An integer representing the maximum number of characters. The default value is -1, indicating no maximum limit. 
        /// </value> 
        /// <remarks> 
        /// Set this property to impose a maximum limit on the number of characters that can be entered into the <see cref="SfTextArea"/>. 
        /// A value of -1 indicates no maximum limit, allowing unlimited text input.
        /// When the maximum length is reached, users will not be able to type additional characters.
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfTextArea MaxLength="100" Placeholder="Enter text"></SfTextArea> 
        /// ]]></code> 
        /// </example>

        [Parameter]
        public int MaxLength { get; set; } = -1;

        private int _maxLength;

        /// <summary>
        /// Gets or sets the resize behavior of the <see cref="SfTextArea"/>.
        /// </summary>
        /// <value>
        /// One of the <see cref="Resize"/> enumeration. The default value is <see cref="Resize.Both"/>.
        /// </value>
        /// <remarks>
        /// <list type="bullet"> 
        /// <item><description>If the <see cref="ResizeMode"/> is <see cref="Resize.None"/>, the <see cref="SfTextArea"/> component should not be resizable ever.</description></item>
        /// <item><description>If the <see cref="ResizeMode"/> is <see cref="Resize.Horizontal"/>, the <see cref="SfTextArea"/> component resizable horizontally always.</description></item>
        /// <item><description>If the <see cref="ResizeMode"/> is <see cref="Resize.Vertical"/>, the <see cref="SfTextArea"/> component resizable vertically always.</description></item>
        /// <item><description>If the <see cref="ResizeMode"/> is <see cref="Resize.Both"/>, the <see cref="SfTextArea"/> component resizable both vertically and horizontally always.</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// In the following code example, set the resize mode as <see cref="Resize.Horizontal"/>.
        /// <code><![CDATA[
        ///   <SfTextArea ResizeMode="Resize.Horizontal" Placeholder="Enter a value"></SfTextArea>
        /// ]]></code>
        /// </example>

        [Parameter]
        public Resize ResizeMode { get; set; } = Resize.Both;

        private Resize _resizeMode;

        /// <summary> 
        /// Gets or sets the number of lines that are visible in the <see cref="SfTextArea"/>. 
        /// </summary> 
        /// <value> 
        /// An integer representing the number of lines. The default value is 2. 
        /// </value> 
        /// <remarks> 
        /// The RowCount property specifies the visible height of the <see cref="SfTextArea"/>, measured in lines of text.
        /// This determines how many lines of text are visible without scrolling.
        /// A higher RowCount value increases the initial height of the <see cref="SfTextArea"/>.
        /// </remarks> 
        /// <example> 
        /// <code><![CDATA[ 
        /// <SfTextArea RowCount="4" Placeholder="Enter text"></SfTextArea> 
        /// ]]></code> 
        /// </example>

        [Parameter]
        public int RowCount { get; set; } = 2;

        private int _rowCount;

        /// <summary> 
        /// Gets or sets the number of columns for the <see cref="SfTextArea"/> component. 
        /// </summary> 
        /// <value> 
        /// An integer representing the number of columns. The default value is 20. 
        /// </value>
        /// <remarks> 
        /// The ColumnCount property specifies the visible width of the <see cref="SfTextArea"/>, measured in average character widths.
        /// This property is only effective when the <see cref="Width"/> property is not set.
        /// If both ColumnCount and <see cref="Width"/> are specified, the <see cref="Width"/> property takes precedence.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfTextArea ColumnCount="30" RowCount="4" Placeholder="Enter text"></SfTextArea>
        /// ]]></code>
        /// </example>

        [Parameter]
        public int ColumnCount { get; set; } = 20;
        private int _columnCount;

        private bool _disabled;
    }
}
