using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Calendars.Interfaces;
using Syncfusion.Blazor.Toolkit.Calendars.Internal;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// The DatePicker is a graphical user interface component that allows the user to select or enter a date value.
    /// </summary>
    public partial class SfDatePicker<TValue> : CalendarBase<TValue>, IMaskPlaceholder
    {
        /// <summary>
        /// Gets or sets the selected date value with mask format.
        /// </summary>
        /// <exclude/>
        internal string CurrentMaskValue { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the value changed by keyboard.
        /// </summary>
        /// <exclude/>
        internal bool IsChangeValue { get; set; } = true;

        /// <summary>
        /// Gets or sets the value selected by popup selection.
        /// </summary>
        /// <exclude/>
        internal bool IsKeySelect { get; set; }

        /// <summary>
        /// Gets or sets the masked format rendered by the component.
        /// </summary>
        /// <exclude/>
        internal string CurrentMaskFormat { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether isDevice.
        /// </summary>
        /// <exclude/>
        protected bool IsDevice { get; set; }

        /// <summary>
        /// Gets or sets the dateIcon.
        /// </summary>
        /// <exclude/>
        protected string DateIcon { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the calendarClass.
        /// </summary>
        /// <exclude/>
        protected string CalendarClass { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the popupElement.
        /// </summary>
        /// <exclude/>
        protected ElementReference PopupElement { get; set; }

        /// <summary>
        /// Gets or sets the popupHolderEle.
        /// </summary>
        /// <exclude/>
        protected ElementReference PopupHolderEle { get; set; }

        /// <summary>
        /// Gets or sets the modelYear.
        /// </summary>
        /// <exclude/>
        protected string? ModelYear { get; set; }

        /// <summary>
        /// Gets or sets the modelDay.
        /// </summary>
        /// <exclude/>
        protected string? ModelDay { get; set; }

        internal TValue IslamicInputTextValue
        {
            get => Value!; set => Value = InternalValue = value;
        }

        /// <summary>
        /// Gets or sets the modelMonth.
        /// </summary>
        /// <exclude/>
        protected string? ModelMonth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether calendar is rendered.
        /// </summary>
        /// <exclude/>
        protected bool IsCalendarRendered { get; set; }

        /// <summary>
        /// Gets or sets the popupEventArgs.
        /// </summary>
        /// <exclude/>
        protected DatePickerPopupArgs PopupEventArgs { get; set; } = new();

        /// <summary>
        /// Gets or sets the changedEventArgs.
        /// </summary>
        /// <exclude/>
        protected ChangedEventArgs<TValue>? ChangedEventArgs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ShowPopupCalendar.
        /// </summary>
        /// <exclude/>
        protected bool ShowPopupCalendar { get; set; }

        /// <summary>
        /// Gets or sets the previousElementValue.
        /// </summary>
        /// <exclude/>
        protected string? PreviousElementValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isCalendarRender.
        /// </summary>
        /// <exclude/>
        protected bool IsCalendarRender { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isListRender.
        /// </summary>
        /// <exclude/>
        protected bool IsListRender { get; set; }

        /// <summary>
        /// Gets or sets the popupContainer.
        /// </summary>
        /// <exclude/>
        protected string PopupContainer { get; set; } = default!;

        /// <summary>
        /// Gets or sets the CalendarBaseInstance.
        /// </summary>
        /// <exclude/>
        protected CalendarBaseRender<TValue>? CalendarBaseInstance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isDatePickerPopup.
        /// </summary>
        /// <exclude/>
        protected bool IsDatePickerPopup { get; set; } = true;

        /// <summary>
        /// Gets or sets css class name to the close icon. 
        /// </summary>
        /// <exclude/>
        protected string? CloseIconClass { get; set; }

        /// <summary>
        /// Gets or sets the time portion used when the DatePicker composes full date/time values.
        /// </summary>
        /// <exclude/>
        protected DateTime TimePart { get; set; }

        internal string? IslamicValueAsString { get; set; }
        internal bool ClearBtnStopPropagation { get; set; }

        private string? ValidClass { get; set; }
        private bool IsFormValidation { get; set; }
        private bool IsCleared { get; set; }
        private string? StrictValue { get; set; }
        private bool IsValideValue { get; set; }
        private bool IsKeyBoardAction { get; set; }
        private string? CurrentInputValue { get; set; }
        private bool IsTyped { get; set; }
        private bool IsBlurred { get; set; }
        private bool IsMinMax { get; set; }
        private DatePickerMaskPlaceholder? MaskPlaceholder { get; set; }
        private bool IsDateIconClicked { get; set; }
        private static readonly char[] _separator = ['_'];

        // Hoisted target classes to avoid allocating a new array on each Enter key handling
        private static readonly string[] _calendarTargetClasses = [
            "e-calendar-content-table",
            "e-calendar e-lib e-keyboard",
            "e-calendar e-lib e-week-number e-keyboard",
            "e-calendar e-lib e-rtl e-keyboard"
        ];

        // Generated regex for better AOT/trimming compatibility and performance
        [GeneratedRegex(@"^[F/U/u/O/o/d/D/f/g/G/m/M/R/r/s/t/T/y/Y]*$")]
        private static partial Regex ValidFormatsRegex();

        [GeneratedRegex(@"\s+")]
        private static partial Regex WhitespaceRegex();

        private ClientMaskValues? ClientMaskValue { get; set; } = new ClientMaskValues();

        internal IJSObjectReference? _datePickerJsModule;
        internal IJSInProcessObjectReference? _datePickerJsInProcessModule;

        internal IJSObjectReference? _maskJsProcessModule;
        internal IJSInProcessObjectReference? _maskJsInProcessModule;

        /// <summary>
        /// Gets or sets the default <see cref="MaskPlaceholder"/> values based on culture's value.
        /// </summary>        
        /// <exclude/>
        protected Dictionary<string, string> MaskPlaceholderDictionary { get; set; } = [];

        private void PropertyInit()
        {
            RootClass = ROOT;
            ContainerClass = CONTAINERCLASS;
            DateIcon = DATEICONCLASS;
            PopupContainer = POPUP_CONTAINER;
            // Unique class added for dynamically rendered Inplace-editor components
            if (DatePickerParent is not null)
            {
                ContainerClass = SfBaseUtils.AddClass(ContainerClass, "e-editable-elements");
                PopupContainer = SfBaseUtils.AddClass(PopupContainer, "e-editable-elements");
            }
            CalendarClass = CALENDAR_ROOT;
            CurrentCulture = GetDefaultCulture();
            ChangedEventArgs = new ChangedEventArgs<TValue>();
            IsDateIconClicked = false;
        }

        private void SetDayHeaderFormat()
        {
            CalendarClass = (DayHeaderFormat == DayHeaderFormats.Wide) ? SfBaseUtils.AddClass(CalendarClass, DAY_HEADER_WIDE) : SfBaseUtils.RemoveClass(CalendarClass, DAY_HEADER_WIDE);
        }

        private void SetRTL()
        {
            CalendarClass = SyncfusionService!._options.EnableRtl ? SfBaseUtils.AddClass(CalendarClass, RTL) : SfBaseUtils.RemoveClass(CalendarClass, RTL);
            PopupContainer = SyncfusionService!._options.EnableRtl ? SfBaseUtils.AddClass(PopupContainer, RTL) : SfBaseUtils.RemoveClass(PopupContainer, RTL);
        }

        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                ContainerClass = (!string.IsNullOrEmpty(ContainerClass) && !ContainerClass.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(ContainerClass, CssClass) : ContainerClass;
                PopupContainer = (!string.IsNullOrEmpty(PopupContainer) && !PopupContainer.Contains(CssClass, StringComparison.Ordinal)) ? SfBaseUtils.AddClass(PopupContainer, CssClass) : PopupContainer;
            }
        }

        /// <summary>
        /// Method which updates the client properties.
        /// </summary>
        /// <returns>The <see cref="DatePickerClientProps{TValue}"/>.</returns>
        /// <exclude/>
        internal DatePickerClientProps<TValue> GetClientProperties()
        {
            return new DatePickerClientProps<TValue>
            {
                Readonly = Readonly,
                Disabled = Disabled,
                ZIndex = ZIndex,
                EnableRtl = SyncfusionService!._options.EnableRtl,
                KeyConfigs = KeyConfigs,
                ShowClearButton = ShowClearButton,
                Value = Value!,
                Width = Width is not null ? SfBaseUtils.FormatUnit(Width) : default!,
                IsDatePopup = IsDatePickerPopup,
                AllowEdit = AllowEdit,
                Depth = Depth.ToString(),
                EnableMask = EnableMask,
                Format = GetStandardFormatString(Format),
                IsRendered = IsRendered,
                FloatLabelType = FloatLabelType.ToString(),
                IsFocused = IsFocused,
                DayAbbreviatedName = CurrentCulture.DateTimeFormat.AbbreviatedDayNames,
                DayName = CurrentCulture.DateTimeFormat.DayNames,
                MonthName = CurrentCulture.DateTimeFormat.MonthNames,
                MonthAbbreviatedName = CurrentCulture.DateTimeFormat.AbbreviatedMonthNames,
                DayPeriod = [CurrentCulture.DateTimeFormat.AMDesignator, CurrentCulture.DateTimeFormat.PMDesignator],
                MaskPlaceholderDictionary = MaskPlaceholderDictionary,
                Placeholder = Placeholder,
                Offset = Intl.GetDateFormat(default(DateTime), "zzzz").ToString(),
                ValueString = ValueString(),
                IsBlurred = IsBlurred,
                MinMax = IsMinMax
            };
        }

        /// <summary>
        /// Specifies the Child content properties.
        /// </summary> 
        /// <param name="maskPlaceholderValue">Maskplaceholder's field values</param>
        /// <exclude/>
        public void UpdateChildProperties(object maskPlaceholderValue)
        {
            MaskPlaceholder = maskPlaceholderValue is null ? new DatePickerMaskPlaceholder() : (DatePickerMaskPlaceholder)maskPlaceholderValue;
        }

        private string ValueString()
        {
            if (Value is null)
            {
                return default!;
            }

            // Guard against null CurrentCulture which is required for date formatting
            if (CurrentCulture is null)
            {
                return default!;
            }

            string format = GetStandardFormatString(GetDefaultFormat());
            string inputValue = PrepareInputValue();
            DateTime dateValue = ParseDateValue(inputValue, format);
            return FormatDateValue(dateValue);
        }

        private string PrepareInputValue()
        {
            string? inputValue = CurrentValueAsString?.Trim();
            bool isArabicCulture = CultureInfo.CurrentCulture.Name.StartsWith(ARABIC, StringComparison.Ordinal);
            return inputValue is null ? string.Empty : isArabicCulture && !string.IsNullOrEmpty(inputValue) ? RemoveCultureDigits(isArabicCulture, inputValue) : inputValue;
        }

        private DateTime ParseDateValue(string inputValue, string format)
        {
            bool isArabicCulture = CultureInfo.CurrentCulture.Name.StartsWith(ARABIC, StringComparison.Ordinal);
            if (IsDateTimeType())
            {
                return isArabicCulture ? ParseDateWithCultureAndFormat(inputValue, format, CultureInfo.CurrentCulture)
                    : DateTime.Parse(Value?.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
            }
            else if (IsDateOnlyType())
            {
                return DateTime.Parse(Value?.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
            }
            else
            {
                DateTimeOffset dateOffset = isArabicCulture ? ParseDateTimeOffsetWithCultureAndFormat(inputValue, format, CultureInfo.CurrentCulture)
                    : DateTime.Parse(Value?.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                return dateOffset.DateTime;
            }
        }

        private string FormatDateValue(DateTime dateValue)
        {
            bool isThailandCulture = CurrentCulture.Name.StartsWith(THAILAND, StringComparison.Ordinal);
            CultureInfo culture = isThailandCulture ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
            return dateValue.ToString("MM/dd/yyyy HH:mm:ss", culture);
        }

        internal void SetPopupVisibility(bool args)
        {
            ShowPopupCalendar = args;
        }

        internal override async Task UpdateCalendarPropertyAsync(string key, object? dateTimeValue)
        {
            if (key == VALUE)
            {
                object? dateValue = null;
                if (IsDateTimeOffsetType())
                {
                    if (dateTimeValue is not null)
                    {
                        int year = ((DateTimeOffset)dateTimeValue).Year;
                        int month = ((DateTimeOffset)dateTimeValue).Month;
                        int day = ((DateTimeOffset)dateTimeValue).Day;
                        TimeSpan offset = ((DateTimeOffset)dateTimeValue).Offset;
                        dateValue = new DateTimeOffset(year, month, day, TimePart.Hour, TimePart.Minute, TimePart.Second, TimePart.Millisecond, offset);
                    }
                }
                else if (IsDateOnlyType())
                {
                    if (dateTimeValue is not null)
                    {
                        int year = ((DateOnly)dateTimeValue).Year;
                        int month = ((DateOnly)dateTimeValue).Month;
                        int day = ((DateOnly)dateTimeValue).Day;
                        dateValue = DateOnly.FromDateTime(new DateTime(year, month, day));
                    }
                }
                else
                {
                    if (dateTimeValue is not null)
                    {
                        dateValue = (DateTime)dateTimeValue;
                    }
                }
                await UpdateValueAsync(dateValue).ConfigureAwait(false);
                if (IsCalendarRender)
                {
                    await HidePopupAsync().ConfigureAwait(false);
                    await FocusAsync().ConfigureAwait(false);
                }
            }
        }

        private void PropertyInitialized()
        {
            InternalCssClass = CssClass;
            CalendarBase_Max = Max;
            CalendarBase_Min = Min;
            CalendarBase_Depth = Depth;
            CalendarBase_Value = Value;
            if (string.IsNullOrEmpty(Format) && EnableMask)
            {
                Format = GetDefaultFormat();
            }
            InternalFormat = Format;
            InternalInputFormats = InputFormats;
            DateStrictMode = StrictMode;
            CloseIconClass = CLOSEICON;
            InternalReadOnly = Readonly;
        }

        private async Task PropertyParametersSetAsync()
        {
            InternalFormat = NotifyPropertyChanges(FORMAT, Format, InternalFormat);
            InternalInputFormats = NotifyPropertyChanges(INPUTFORMATS, InputFormats, InternalInputFormats);
            NotifyPropertyChanges(nameof(CssClass), CssClass, InternalCssClass);
            InternalValue = NotifyPropertyChanges(nameof(Value), Value, InternalValue);
            CalendarBase_Max = NotifyPropertyChanges(MAX, Max, CalendarBase_Max);
            CalendarBase_Min = NotifyPropertyChanges(MIN, Min, CalendarBase_Min);
            CalendarBase_Depth = NotifyPropertyChanges(DEPTH, Depth, CalendarBase_Depth);
            DateStrictMode = NotifyPropertyChanges(nameof(StrictMode), StrictMode, DateStrictMode);
            InternalReadOnly = NotifyPropertyChanges(nameof(READONLYATTR), Readonly, InternalReadOnly);
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Task used to update the value of the component.
        /// </summary>
        /// <param name="dateValue">Specifies the date value.<see cref="object"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task UpdateValueAsync(object? dateValue)
        {
            if (!(Nullable.GetUnderlyingType(typeof(TValue)) is null && dateValue is null))
            {
                TValue? tempValue = dateValue is null ? default : (TValue)SfBaseUtils.ChangeType(dateValue!, typeof(TValue));
                await InvokeAsync(() =>
                {
                    InputTextValue = tempValue;
                    if (CalendarMode == CalendarType.Islamic)
                    {
                        IslamicInputTextValue = tempValue!;
                        IslamicValueAsString = tempValue is not null ? ConvertToHijri(tempValue, GetDefaultFormat()) : null;
                    }
                    return Task.CompletedTask;
                }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Determines the default date/time format string used by the DatePicker when no explicit <see cref="Format"/> is provided.
        /// </summary>
        /// <returns>
        /// A format string to be used for parsing and rendering values. If the component's <see cref="Format"/> property is set,
        /// that value is returned. Otherwise the method returns either a date-only pattern (for date-only mode) or a combined
        /// date and time pattern (for time-capable pickers) derived from the current culture's short date/time patterns.
        /// </returns>
        /// <exclude />
        protected string GetDefaultFormat()
        {
            string? datePattern = CurrentCulture?.DateTimeFormat.ShortDatePattern;
            string timePattern = datePattern + SPACE + CurrentCulture?.DateTimeFormat.ShortTimePattern;
            bool isDatePick = RootClass.Contains(DATEPICKER, StringComparison.Ordinal);
            datePattern = datePattern is not null ? datePattern : string.Empty;
            return !string.IsNullOrEmpty(Format) ? Format : isDatePick ? datePattern : timePattern;
        }

        private string GetStandardFormatString(string format)
        {
            if (!string.IsNullOrEmpty(format) && format.Length < 2 && ValidFormatsRegex().IsMatch(format))
            {
                string[]? cultureformat = CurrentCulture?.DateTimeFormat.GetAllDateTimePatterns(char.Parse(format));
                return cultureformat is not null && cultureformat.Length > 0 ? cultureformat[0] : format;
            }
            return format;
        }

        /// <summary>
        /// Method to get the default cultureinfo.
        /// </summary>
        /// <returns>Cultureinfo.</returns>
        /// <exclude/>
        protected CultureInfo GetDefaultCulture()
        {
            return Intl.GetCulture();
        }
        /// <summary>
        /// Formats the provided generic value (<typeparamref name="TValue"/>) to a string suitable for display in the input element.
        /// </summary>
        /// <param name="formatValue">The value to format. Typical runtime types are <see cref="DateTime"/>, <see cref="DateOnly"/>, or <see cref="DateTimeOffset"/>, depending on <typeparamref name="TValue"/>.</param>
        /// <returns>
        /// A formatted string representation of <paramref name="formatValue"/> according to the active format rules. If <paramref name="formatValue"/> is <c>null</c>
        /// the method returns the current <see cref="StrictValue"/> (may be <c>null</c> or an empty string) which represents an in-progress user input used
        /// by strict mode validation.
        /// </returns>
        /// <exclude/>
        protected override string FormatValueAsString(TValue? formatValue)
        {
            if (formatValue is not null)
            {
                string formatString = GetDefaultFormat();
                formatString = GetStandardFormatString(formatString);
                string dateFormatValue = Intl.GetDateFormat(formatValue, formatString);
                dateFormatValue = StrictMode && !(IsFocused && ValidateOnInput) && StrictValue is not null ? StrictValue : dateFormatValue;
                StrictValue = null;
                return dateFormatValue;
            }
            else
            {
                return StrictValue!;
            }
        }

        /// <summary>
        /// Parses the supplied input string into the component's generic value type (<typeparamref name="TValue"/>).
        /// </summary>
        /// <param name="genericValue">The raw input string provided by the user or binding source. May be <c>null</c> or empty.</param>
        /// <returns>
        /// A value of type <typeparamref name="TValue"/> parsed according to the active format rules, <c>default</c> when parsing fails
        /// or when <paramref name="genericValue"/> is null/empty.
        /// </returns>
        /// <exclude/>
        protected override TValue FormatValue(string? genericValue)
        {
            if (string.IsNullOrEmpty(genericValue))
            {
                StrictValue = null;
                return default!;
            }

            // Guard against null CurrentCulture which is required for parsing
            if (CurrentCulture is null)
            {
                return default!;
            }

            string format = GetStandardFormatString(GetDefaultFormat());
            string inputValue = PrepareInputForParsing(genericValue);
            return TryParseWithFormats(inputValue, format);
        }

        private string PrepareInputForParsing(string value)
        {
            string inputValue = value.Trim();
            bool isArabicCulture = CurrentCulture.Name.StartsWith(ARABIC, StringComparison.Ordinal);
            bool isThailandCulture = CurrentCulture.Name.StartsWith(THAILAND, StringComparison.Ordinal);
            if (isArabicCulture || isThailandCulture)
            {
                inputValue = !string.IsNullOrEmpty(inputValue) ? RemoveCultureDigits(isArabicCulture, inputValue) : inputValue;
            }
            return inputValue;
        }

        private TValue TryParseWithFormats(string inputValue, string format)
        {
            TValue date = default!;
            bool isTryParse = false;
            if (IsTryParse(inputValue, format))
            {
                date = ParseDate(inputValue, format);
                isTryParse = true;
            }
            else if (InputFormats is not null)
            {
                date = TryParseInputFormats(inputValue, out isTryParse);
            }
            return !isTryParse ? HandleParseFailure(inputValue, date) : date;
        }

        private TValue TryParseInputFormats(string inputValue, out bool success)
        {
            foreach (string inputFormat in InputFormats)
            {
                if (IsTryParse(inputValue, inputFormat))
                {
                    success = true;
                    return ParseDate(inputValue, inputFormat);
                }
            }
            success = false;
            return default!;
        }

        private TValue HandleParseFailure(string inputValue, TValue date)
        {
            if (IsFocused && ValidateOnInput)
            {
                StrictValue = inputValue;
                // Notify form validation about the parse failure so validation messages appear
                if (InputEditContext is not null && ValueExpression is not null)
                {
                    SfBaseUtils.ValidateExpression(InputEditContext, ValueExpression);
                }
                return date;
            }
            return HandleStrictModeFailure(inputValue, date);
        }

        private TValue HandleStrictModeFailure(string inputValue, TValue date)
        {
            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) is not null;
            if (Value is not null && (!isNullable || IsValideValue))
            {
                date = Value;
            }
            if (StrictMode && !SfBaseUtils.Equals(date, default))
            {
                StrictValue = inputValue;
            }
            else if (!StrictMode)
            {
                date = HandleNonStrictMode(inputValue, date);
            }
            return StrictMode && SfBaseUtils.Equals(date, default) ? HandleDefaultValue(inputValue) : date;
        }

        private TValue HandleNonStrictMode(string inputValue, TValue date)
        {
            if ((PreviousElementValue != inputValue && !IsValideValue) || EnableMask)
            {
                date = default!;
                StrictValue = inputValue;
            }
            return date;
        }

        private TValue HandleDefaultValue(string inputValue)
        {
            StrictValue = inputValue;
            TValue dateValue = (Value is null || string.IsNullOrEmpty(inputValue)) ? default! : Value;
            return dateValue;
        }
        /// <summary>
        /// Task which updates the strict mode.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task StrictModeUpdateAsync(bool isInit = false)
        {
            // Guard against null CurrentCulture required for date formatting
            if (CurrentCulture is null)
            {
                return;
            }

            // Guard against disposed state during async operations
            if (IsDisposed)
            {
                return;
            }

            string format = GetStandardFormatString(GetDefaultFormat());
            string? inputString = PrepareStrictModeInput();
            string inputValue = inputString ?? string.Empty;
            TValue? date = await ParseStrictModeDateAsync(inputValue, format, isInit).ConfigureAwait(false);
            await ApplyStrictModeLogicAsync(date, inputValue, format, isInit).ConfigureAwait(false);
            if (StrictMode && EnableMask && IsRendered)
            {
                await CreateMaskAsync().ConfigureAwait(false);
            }
        }

        private string? PrepareStrictModeInput()
        {
            string? inputValue = CalendarMode == CalendarType.Islamic ? IslamicValueAsString : CurrentValueAsString?.Trim();
            if (CurrentCulture is null)
            {
                return inputValue;
            }
            bool isArabicCulture = CurrentCulture.Name.StartsWith(ARABIC, StringComparison.Ordinal);
            bool isThailandCulture = CurrentCulture.Name.StartsWith(THAILAND, StringComparison.Ordinal);
            if (isArabicCulture || isThailandCulture)
            {
                inputValue = !string.IsNullOrEmpty(inputValue) ? RemoveCultureDigits(isArabicCulture, inputValue) : inputValue;
            }
            return inputValue;
        }

        private async Task<TValue> ParseStrictModeDateAsync(string inputValue, string format, bool isInit)
        {
            TValue? date = default!;
            if (IsTryParse(inputValue, format) && CalendarMode != CalendarType.Islamic)
            {
                date = ParseDate(inputValue, format);
            }
            if (CalendarMode == CalendarType.Islamic && !string.IsNullOrEmpty(inputValue))
            {
                date = ConvertToGregorian(inputValue, format);
            }
            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) is not null;
            if (Value is not null && (!isNullable || isInit))
            {
                date = Value;
                if (!isInit)
                {
                    await UpdateDateValueAsync(date, format).ConfigureAwait(false);
                }
            }
            return date;
        }

        private async Task UpdateDateValueAsync(TValue date, string format)
        {
            if (ValueChanged.HasDelegate)
            {
                await UpdateInputValueAsync(Intl.GetDateFormat(date, format)).ConfigureAwait(false);
            }
            else
            {
                InputTextValue = date;
            }
        }

        private async Task ApplyStrictModeLogicAsync(TValue date, string inputValue, string format, bool isInit)
        {
            if (StrictMode && !(IsFocused && ValidateOnInput) && date is not null)
            {
                await UpdateInputValueAsync(Intl.GetDateFormat(date, format)).ConfigureAwait(false);
                if (PreviousElementValue != inputValue)
                {
                    await UpdateValueAsync(date).ConfigureAwait(false);
                }
            }
            else if (!StrictMode || (IsFocused && ValidateOnInput))
            {
                if (PreviousElementValue != inputValue)
                {
                    await UpdateValueAsync(date).ConfigureAwait(false);
                }
            }
            if (StrictMode && !(IsFocused && ValidateOnInput) && date is null)
            {
                TValue? dateValue = (Value is null || string.IsNullOrEmpty(inputValue)) ? default : Value;
                await UpdateValueAsync(dateValue).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Update the AriaActiveDescendant attribute on the input element.
        /// </summary>
        /// <exclude/>
        internal override async Task UpdateAriaActiveDescendantAsync(string? cellId)
        {
            if (ShowPopupCalendar)
            {
                cellId = cellId is null ? string.Empty : cellId;
                SfBaseUtils.UpdateDictionary(ARIAACTIVEDESCENDANT, cellId, InputHtmlAttributes);
                await InvokeVoidAsync(_datePickerJsModule, _datePickerJsInProcessModule, "updateAriaActiveDescendant", [DataId, cellId]).ConfigureAwait(true);
            }
            await Task.CompletedTask.ConfigureAwait(false);
        }

        private bool IsTryParse(string dateValue, string format)
        {
            return IsDateTimeType() ? DateTime.TryParseExact(dateValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out _)
                : IsDateOnlyType() ? DateOnly.TryParseExact(dateValue, format, out _)
                : DateTimeOffset.TryParseExact(dateValue, format, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out _);
        }
        private static DateTime ParseDateWithCultureAndFormat(string dateValue, string format, CultureInfo cultureInfo)
        {
            // Only clone when the provided CultureInfo is read-only to avoid mutating shared instances
            CultureInfo effectiveCulture = cultureInfo.IsReadOnly ? (CultureInfo)cultureInfo.Clone() : cultureInfo;
            effectiveCulture.DateTimeFormat.Calendar = new GregorianCalendar();
            DateTime date = DateTime.ParseExact(dateValue, format, effectiveCulture);
            return date;
        }
        private static DateTimeOffset ParseDateTimeOffsetWithCultureAndFormat(string dateValue, string format, CultureInfo cultureInfo)
        {
            // Only clone when the provided CultureInfo is read-only to avoid mutating shared instances
            CultureInfo effectiveCulture = cultureInfo.IsReadOnly ? (CultureInfo)cultureInfo.Clone() : cultureInfo;
            effectiveCulture.DateTimeFormat.Calendar = new GregorianCalendar();
            DateTimeOffset date = DateTimeOffset.ParseExact(dateValue, format, effectiveCulture);
            return date;
        }
        private TValue ParseDate(string dateValue, string format)
        {
            Type type = typeof(TValue);
            CultureInfo cultureInfo = new(CultureInfo.CurrentCulture.Name);
            bool isArabicCulture = CultureInfo.CurrentCulture.Name.StartsWith(ARABIC, StringComparison.Ordinal);
            if (IsDateTimeType())
            {
                if (Value is not null)
                {
                    DateTime date = isArabicCulture ? ParseDateWithCultureAndFormat(dateValue, format, cultureInfo) : DateTime.Parse(Value.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                    if (date.Year > 2029)
                    {
                        cultureInfo.Calendar.TwoDigitYearMax = 2099;
                    }
                    else if (date.Year < 1930)
                    {
                        cultureInfo.Calendar.TwoDigitYearMax = 1999;
                    }
                }
                return (TValue)SfBaseUtils.ChangeType(DateTime.ParseExact(dateValue, format, cultureInfo, DateTimeStyles.AssumeLocal), type);
            }
            else if (IsDateOnlyType())
            {
                if (Value is not null)
                {
                    DateTime date = DateTime.Parse(Value.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                    if (date.Year > 2029)
                    {
                        cultureInfo.Calendar.TwoDigitYearMax = 2099;
                    }
                    else if (date.Year < 1930)
                    {
                        cultureInfo.Calendar.TwoDigitYearMax = 1999;
                    }
                }
                return (TValue)SfBaseUtils.ChangeType(DateOnly.ParseExact(dateValue, format, cultureInfo), type);
            }
            else
            {
                if (Value is not null)
                {
                    DateTimeOffset date = isArabicCulture ? ParseDateTimeOffsetWithCultureAndFormat(dateValue, format, cultureInfo) : DateTimeOffset.Parse(Value.ToString() ?? string.Empty, CultureInfo.CurrentCulture);
                    if (date.Year > 2029)
                    {
                        cultureInfo.Calendar.TwoDigitYearMax = 2099;
                    }
                    else if (date.Year < 1930)
                    {
                        cultureInfo.Calendar.TwoDigitYearMax = 1999;
                    }
                }
                //The below fix included for BLAZ-20489
                return (TValue)SfBaseUtils.ChangeType(DateTimeOffset.ParseExact(dateValue, format, cultureInfo, DateTimeStyles.AssumeLocal), type);
            }
        }

        /// <summary>
        /// Task used to update the popup state.
        /// </summary>
        /// <param name="isOpen">true if the popup is in opened state, otherwise false.</param>
        /// <exclude />
        protected virtual void UpdateDateTimePopupState(bool isOpen)
        {
        }

        internal async Task ClosePopupElementAsync()
        {
            if (IsDateIconClicked)
            {
                await FocusAsync().ConfigureAwait(false);
            }
            if (IsDevice && AllowEdit)
            {
                InputHtmlAttributes = RemoveAttr(READONLYATTR, InputHtmlAttributes);
            }
            IsCalendarRender = false;
            SetPopupVisibility(false);
            UpdateDateTimePopupState(false);
            SfBaseUtils.UpdateDictionary(ARIAEXPANDED, FALSE, InputHtmlAttributes);
            InputHtmlAttributes.Remove(ARIAACTIVEDESCENDANT);
            InputHtmlAttributes.Remove(ARIA_OWN);
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Method to trigger the client-side actions once the popup is displayed when date icon is clicked.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude />
        protected virtual async Task ClientPopupRenderAsync()
        {
            if (ShowPopupCalendar && IsCalendarRendered)
            {
                IsCalendarRendered = false;
                DatePickerClientProps<TValue> options = GetClientProperties();
                await InvokeVoidAsync(_datePickerJsModule, _datePickerJsInProcessModule, "renderPopup", [DataId, PopupElement, PopupHolderEle, PopupEventArgs, options]).ConfigureAwait(true);
                IsCalendarRender = true;
            }
        }

        private void UpdateAriaAttributes()
        {
            SfBaseUtils.UpdateDictionary(ARIA_LIVE, ASSERTIVE, InputHtmlAttributes);
            SfBaseUtils.UpdateDictionary(ARIA_AUTOMIC, TRUE, InputHtmlAttributes);
            SfBaseUtils.UpdateDictionary(ARIA_HAS_POPUP, GRID, InputHtmlAttributes);
            SfBaseUtils.UpdateDictionary(ROLE, COMBOBOX, InputHtmlAttributes);
            SfBaseUtils.UpdateDictionary(AUTO_CORRECT, OFF, InputHtmlAttributes);
            SfBaseUtils.UpdateDictionary(SPELL_CHECK, FALSE, InputHtmlAttributes);
            SfBaseUtils.UpdateDictionary(ARIAINVALID, FALSE, InputHtmlAttributes);
            SfBaseUtils.UpdateDictionary(ARIA_CONTROLS, ID, InputHtmlAttributes);
        }
        private static string RemoveCultureDigits(bool isArabic, string dateValue)
        {
            string outDate = string.Empty;
            if (!string.IsNullOrEmpty(dateValue))
            {
                foreach (char item in dateValue)
                {
                    char startVal = isArabic ? ARABIC_START_DIGIT : THAILAND_START_DIGIT;
                    char endVal = isArabic ? ARABIC_END_DIGIT : THAILAND_END_DIGIT;
                    outDate += (item >= startVal && item <= endVal) ? char.GetNumericValue(item).ToString(CultureInfo.CurrentCulture) : item.ToString();
                }
            }
            return outDate;
        }

        /// <summary>
        /// Checks whether the value type is DateTime.
        /// </summary>
        /// <returns>True or false based on the Type.</returns>
        /// <exclude/>
        protected bool IsDateTimeType()
        {
            Type type = typeof(TValue);
            bool isNullable = Nullable.GetUnderlyingType(type) is not null;
            return type == typeof(DateTime) || (isNullable && typeof(DateTime) == Nullable.GetUnderlyingType(type));
        }

        /// <summary>
        /// Triggers when the value of the component get changed.
        /// </summary>
        /// <param name="args">The <see cref="ChangeEventArgs"/> arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task ChangeHandlerAsync(ChangeEventArgs? args)
        {
            IsChangeValue = true;
            await KeyHandlerAsync((string?)args?.Value).ConfigureAwait(false);
            await Task.CompletedTask.ConfigureAwait(false);
        }

        internal async Task KeyHandlerAsync(string? value)
        {
            StrictValue = string.IsNullOrEmpty(value) ? null : StrictValue;
            CurrentValueAsString = value;
            if (CalendarMode == CalendarType.Islamic)
            {
                IslamicValueAsString = value;
            }
            if (EnableMask && (CurrentMaskFormat == value) && StrictMode)
            {
                CurrentValueAsString = null;
            }
            IsCleared = false;
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Handles raw input events raised by the input element when the user types or edits the value.
        /// </summary>
        /// <param name="args">The <see cref="ChangeEventArgs"/> containing the new input value.</param>
        /// <returns>A <see cref="Task"/> that completes when internal state updates are finished.</returns>
        /// <exclude />
        protected override async Task InputHandlerAsync(ChangeEventArgs? args)
        {
            IsTyped = true;
            CurrentInputValue = (string?)(args?.Value);
            IsKeyBoardAction = true;
            IsValideValue = false;
            if (OnInput.HasDelegate)
            {
                await OnInput.InvokeAsync(args).ConfigureAwait(true);
            }
            await Task.CompletedTask.ConfigureAwait(false);
        }
        /// <summary>
        /// Triggers when the component get focused.
        /// </summary>
        /// <param name="args">The <see cref="Microsoft.AspNetCore.Components.Web.FocusEventArgs"/> arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected virtual async Task InvokeFocusEventAsync(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (OnFocus.HasDelegate)
            {
                FocusEventArgs FocusArgs = new() { Model = new object[] { args } };
                await OnFocus.InvokeAsync(FocusArgs).ConfigureAwait(false);
            }
            if (OpenOnFocus)
            {
                await OpenPopupAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Triggers when the component get focused out.
        /// </summary>
        /// <param name="args">The <see cref="Microsoft.AspNetCore.Components.Web.FocusEventArgs"/> arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected virtual async Task InvokeBlurEventAsync(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (OnBlur.HasDelegate)
            {
                BlurEventArgs BlurArgs = new() { Model = new object[] { args } };
                await OnBlur.InvokeAsync(BlurArgs).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Triggers when the component get focused.
        /// </summary>
        /// <param name="args">The <see cref="Microsoft.AspNetCore.Components.Web.FocusEventArgs"/> arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task FocusHandlerAsync(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            if (ClientMaskValue is not null && (CurrentMaskFormat == ClientMaskValue.InputElementValue) && EnableMask)
            {
                if (Value is not null && string.IsNullOrEmpty(Value.ToString()) && (FloatLabelType == FloatLabelType.Auto || FloatLabelType == FloatLabelType.Never) && !string.IsNullOrEmpty(Placeholder))
                {
                    CurrentValueAsString = ClientMaskValue.InputElementValue;
                }
            }
            await InvokeFocusEventAsync(args).ConfigureAwait(false);
            if (FloatLabelType == FloatLabelType.Auto)
            {
                await InvokeVoidAsync(_datePickerJsModule, _datePickerJsInProcessModule, "removeFloatLabelSize", [DataId]).ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Triggers when the component get focused out.
        /// </summary>
        /// <param name="args">The <see cref="Microsoft.AspNetCore.Components.Web.FocusEventArgs"/> arguments.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected override async Task FocusOutHandlerAsync(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            // Guard against disposed state during async event handling
            if (IsDisposed)
            {
                return;
            }

            IsBlurred = true;
            await HandleMaskClearAsync().ConfigureAwait(false);
            await HandleMaskChangeAsync().ConfigureAwait(false);
            ResetFocusStates();
            await StrictModeUpdateAsync().ConfigureAwait(false);
            if (ShouldClearInput())
            {
                await UpdateInputValueAsync(null).ConfigureAwait(false);
            }
            await FinalizeBlurAsync(args).ConfigureAwait(false);
            await UpdateFloatLabelSizeAsync().ConfigureAwait(false);
            IsBlurred = false;
        }

        private async Task HandleMaskClearAsync()
        {
            if (Value is not null && string.IsNullOrEmpty(Value.ToString()) && !string.IsNullOrEmpty(Placeholder) && (FloatLabelType == FloatLabelType.Auto || FloatLabelType == FloatLabelType.Never) && EnableMask)
            {
                if (!(!StrictMode && CurrentValueAsString != CurrentMaskFormat))
                {
                    CurrentValueAsString = null;
                }
                if (!StrictMode && CurrentValueAsString is null)
                {
                    await CreateMaskAsync().ConfigureAwait(false);
                    await UpdateInputValueAsync(null).ConfigureAwait(false);
                }
            }
        }

        private async Task HandleMaskChangeAsync()
        {
            if (EnableMask && !IsChangeValue && !string.IsNullOrEmpty(CurrentMaskValue))
            {
                await CreateMaskAsync().ConfigureAwait(false);
                await KeyHandlerAsync(CurrentMaskValue).ConfigureAwait(false);
                if (Value is not null)
                {
                    await CreateMaskAsync().ConfigureAwait(false);
                }
                IsChangeValue = true;
            }
        }

        private void ResetFocusStates()
        {
            DateIcon = SfBaseUtils.RemoveClass(DateIcon, ACTIVE);
            IsDateIconClicked = false;
            IsKeySelect = false;
            IsTyped = false;
        }

        private bool ShouldClearInput()
        {
            dynamic? disbleCellDetail = GetDisabledCellDetail();
            return (Value is null && StrictMode && (FloatLabelType != FloatLabelType.Always) && !string.IsNullOrEmpty(Placeholder) && EnableMask) ||
                   (StrictMode && IsKeyBoardAction && disbleCellDetail is not null) || (!EnableMask && Value is null && StrictMode);
        }

        private dynamic? GetDisabledCellDetail()
        {
            if (Value is null || DisabledDayCellData is null || DisabledDayCellData.Count == 0)
            {
                return null;
            }
            long targetTicks = ConvertDateValue(Value).Ticks;
            foreach (CellDetails i in DisabledDayCellData)
            {
                if (string.IsNullOrEmpty(i.CellID))
                {
                    continue;
                }
                string cid = i.CellID;
                int sepIndex = cid.IndexOf('_', StringComparison.Ordinal);
                string tickString = sepIndex >= 0 ? cid[..sepIndex] : cid;
                if (long.TryParse(tickString, NumberStyles.Integer, CultureInfo.CurrentCulture, out long ticks) && ticks == targetTicks)
                {
                    return i;
                }
            }
            return null;
        }

        private async Task FinalizeBlurAsync(Microsoft.AspNetCore.Components.Web.FocusEventArgs args)
        {
            await UpdateInputAsync().ConfigureAwait(false);
            await ChangeTriggerAsync(args).ConfigureAwait(false);
            UpdateValidateClass();
            UpdateErrorClass();
            await InvokeBlurEventAsync(args).ConfigureAwait(false);
        }

        private async Task UpdateFloatLabelSizeAsync()
        {
            if (FloatLabelType is FloatLabelType.Auto or FloatLabelType.Never)
            {
                await InvokeVoidAsync(_datePickerJsModule, _datePickerJsInProcessModule, "updateFloatLabelSize", [DataId]).ConfigureAwait(true);
            }
        }
        /// <summary>
        /// Handles the clear-button activation sequence invoked by the user.
        /// </summary>
        /// <param name="args">The event arguments associated with the clear action. Typically an <see cref="EventArgs"/> placeholder.</param>
        /// <returns>A <see cref="Task"/> that completes after the clear operation and related events have been processed.</returns>
        /// <exclude/>
        protected async Task InvokeClearBtnEventAsync(EventArgs args)
        {
            // Guard against disposed state during async event handling
            if (IsDisposed)
            {
                return;
            }

            PrepareClearOperation();
            await ClearInputValueAsync().ConfigureAwait(false);
            await NotifyClearEventsAsync(args).ConfigureAwait(false);
            await FinalizeClearOperationAsync(args).ConfigureAwait(false);
        }

        private void PrepareClearOperation()
        {
            if (!IsDevice)
            {
                ClearBtnStopPropagation = true;
            }
            IsCleared = true;
        }

        private async Task ClearInputValueAsync()
        {
            if (!EnableMask)
            {
                // Delay to allow cleared value to propagate to input element before mask updates
                await Task.Delay(INPUT_UPDATE_DELAY_MS).ConfigureAwait(false);
            }
            // Ensure updates that trigger rendering occur on the Blazor Dispatcher
            await InvokeAsync(async () =>
            {
                await UpdateValueAsync(null).ConfigureAwait(false);
                await UpdateInputValueAsync(null).ConfigureAwait(false);
            }).ConfigureAwait(false);

            CurrentInputValue = null;
            if (EnableMask)
            {
                await CreateMaskAsync().ConfigureAwait(false);
            }
        }

        private async Task NotifyClearEventsAsync(EventArgs args)
        {
            if (Cleared.HasDelegate)
            {
                await InvokeAsync(() => Cleared.InvokeAsync(new ClearedEventArgs() { Event = args })).ConfigureAwait(false);
            }
            await UpdateInputAsync().ConfigureAwait(false);
            UpdateValidateClass();
            await FocusAsync().ConfigureAwait(false);
        }

        private async Task FinalizeClearOperationAsync(EventArgs args)
        {
            if (OnChange.HasDelegate)
            {
                await InvokeAsync(() => OnChange.InvokeAsync(new ChangeEventArgs() { Value = string.Empty })).ConfigureAwait(false);
            }
            await ChangeEventAsync(args).ConfigureAwait(false);
            if (IsCalendarRender)
            {
                await HidePopupAsync(args).ConfigureAwait(false);
            }
            if (EnableMask)
            {
                IsCleared = false;
            }
        }

        /// <summary>
        /// Method which closes the popup when close icon in full screen mobile calendar popup gets clicked
        /// </summary>
        /// <exclude/>
        protected async Task PopupCloseHandlerAsync()
        {
            await HidePopupAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Method which checks whether the CssClass contains expandable mobile popup class.
        /// </summary>
        /// <exclude/>
        protected bool IsExtendedDevicePopup()
        {
            return IsDevice && !string.IsNullOrEmpty(PopupContainer) && PopupContainer.Contains(POPUPEXPAND, StringComparison.Ordinal);
        }

        /// <summary>
        /// Method which updates the valid class based on the value .
        /// </summary>
        /// <exclude />
        protected void UpdateValidateClass()
        {
            if (ValueExpression is not null && InputEditContext is not null)
            {
                IsFormValidation = true;
                FieldIdentifier fieldIdentifier = FieldIdentifier.Create(ValueExpression);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, " " + ValidClass) : ContainerClass;
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.RemoveClass(ContainerClass, ValidClass + " ") : ContainerClass;
                ValidClass = InputEditContext.FieldCssClass(fieldIdentifier);
                ContainerClass = !string.IsNullOrEmpty(ValidClass) ? SfBaseUtils.AddClass(ContainerClass, ValidClass) : ContainerClass;
                ContainerClass = WhitespaceRegex().Replace(ContainerClass, " ");
                if (ValidClass is INVALID or MODIFIED_INVALID)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERRORCLASS);
                }
                else if (ValidClass == MODIFIED_VALID)
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERRORCLASS);
                    ContainerClass = SfBaseUtils.AddClass(ContainerClass, SUCCESS_CLASS);
                }
                else if (ValidClass == "valid" && !(!string.IsNullOrEmpty(CssClass) && (CssClass.Contains(ERRORCLASS, StringComparison.Ordinal) || CssClass.Contains(SUCCESS_CLASS, StringComparison.Ordinal))))
                {
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERRORCLASS);
                    ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
                }
            }
        }

        /// <summary>
        /// Triggers while mouse icon performs an action.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="iconName"></param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task MouseIconHandlerAsync(EventArgs args, string iconName)
        {
            IsKeyBoardAction = false;
            if (args is not null && iconName is not null)
            {
                if (iconName.Contains(DATEICONCLASS, StringComparison.Ordinal))
                {
                    IsDatePickerPopup = true;
                    await DateIconHandlerAsync().ConfigureAwait(false);
                }
                else
                {
                    IsDatePickerPopup = false;
                    await TimeIconHandlerAsync(args).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Handles the time icon process.
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected virtual async Task TimeIconHandlerAsync(EventArgs eventArgs)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Triggers while popup get opened.
        /// </summary>
        /// <param name="isOpen">The isOpen<see cref="bool"/>.</param>
        /// <param name="args">The args<see cref="EventArgs"/>.</param>
        /// <returns>The <see cref="Task{PopupObjectArgs}"/>.</returns>
        /// <exclude/>
        protected virtual async Task<PopupObjectArgs> InvokeOpenEventAsync(bool isOpen, EventArgs? args = null)
        {
            PopupObjectArgs openEventArgs = new()
            {
                Cancel = false,
                Event = args is null ? new() : args,
                PreventDefault = false
            };
            PopupEventArgs = new DatePickerPopupArgs { AppendTo = IsDevice ? MODEL : BODY, Cancel = false, Event = openEventArgs.Event, PreventDefault = false };
            await InvokeAsync(() => SfBaseUtils.InvokeEventAsync(isOpen ? OnOpen : OnClose, openEventArgs)).ConfigureAwait(false);
            return openEventArgs;
        }

        /// <summary>
        /// Handles the date icon process.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task DateIconHandlerAsync()
        {
            if (!Disabled)
            {
                bool isDisabled = (InputHtmlAttributes is not null && InputHtmlAttributes.ContainsKey("disabled")) || (HtmlAttributes is not null && HtmlAttributes.ContainsKey("disabled"));
                if (isDisabled)
                {
                    return;
                }
                if (IsDevice)
                {
                    // Delay for read only attributes update in device mode before FocusOut
                    await Task.Delay(DEVICE_READONLY_DELAY_MS).ConfigureAwait(false);
                    SfBaseUtils.UpdateDictionary(READONLYATTR, true, InputHtmlAttributes);
                    await FocusOutAsync().ConfigureAwait(false);
                }
                IsDateIconClicked = true;
                if (!Readonly)
                {
                    if (IsCalendarRender)
                    {
                        await HidePopupAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        if (IsListRender)
                        {
                            await HideTimePopupAsync().ConfigureAwait(false);
                            // Delay for CSS transition on time popup hide to complete before opening date popup
                            await Task.Delay(POPUP_HIDE_DELAY_MS).ConfigureAwait(false);
                        }
                        await SetReadOnlyFocusAsync().ConfigureAwait(false);
                        // Delay to allow focus transition animations to complete before opening popup
                        await Task.Delay(FOCUS_TRANSITION_DELAY_MS).ConfigureAwait(false);
                        await OpenPopupAsync().ConfigureAwait(false);
                        ContainerClass = SfBaseUtils.AddClass(ContainerClass.Trim(), INPUTFOCUS);
                        if (DateIcon is not null)
                        {
                            DateIcon = SfBaseUtils.AddClass(DateIcon, ACTIVE);
                        }
                    }
                }
            }
        }

        internal async Task SetReadOnlyFocusAsync()
        {
            await InvokeVoidAsync(_datePickerJsModule, _datePickerJsInProcessModule, "focusIn", [DataId, IsDevice]).ConfigureAwait(true);
        }

        /// <summary>
        /// Method used to hide the time popup.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected virtual async Task HideTimePopupAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Method used to update the value in the input element.
        /// </summary>
        /// <param name="dateValue">The dateValue<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        private async Task UpdateInputValueAsync(string? dateValue)
        {
            await SetValueAsync(dateValue, FloatLabelType, ShowClearButton).ConfigureAwait(false);
        }

        /// <summary>
        /// Triggers when the value of the component get changed.
        /// </summary>
        /// <param name="args">The args<see cref="EventArgs"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected virtual async Task InvokeChangeEventAsync(EventArgs? args = null)
        {
            if (ValueChange.HasDelegate && !(Disabled || Readonly))
            {
                ChangedEventArgs = new ChangedEventArgs<TValue>()
                {
                    Value = Value!,
                    Event = args is null ? new EventArgs() : args,
                    IsInteracted = args is not null
                };
                await ValueChange.InvokeAsync(ChangedEventArgs).ConfigureAwait(false);
            }
        }

        private async Task ChangeTriggerAsync(EventArgs? args = null)
        {
            if (CurrentValueAsString != PreviousElementValue)
            {
                if (PreviousDate is not null && CompareValue(PreviousDate, Value!) != 0)
                {
                    ChangedArgs.Value = Value!;
                    await InvokeChangeEventAsync(args).ConfigureAwait(false);
                    if (EnablePersistence)
                    {
                        await SetLocalStorageAsync(ID, Value!).ConfigureAwait(false);
                    }
                    PreviousElementValue = CurrentValueAsString;
                    PreviousDate = Value!;
                }
            }
        }

        /// <summary>
        /// Triggers when the value of the component get changed.
        /// </summary>
        /// <param name="args">The args<see cref="EventArgs"/>.</param>
        /// <param name="isSelection">Determines whether selection is made using the mouse or keyboard</param>
        /// <exclude/>
        protected override async Task ChangeEventAsync(EventArgs? args, bool isSelection = false)
        {
            // Guard against disposed state during event handling
            if (IsDisposed)
            {
                return;
            }

            if (!SfBaseUtils.Equals(Value, PreviousDate))
            {
                await SelectCalendarAsync(isSelection).ConfigureAwait(false);

                if (EnablePersistence)
                {
                    await SetLocalStorageAsync(ID, Value!).ConfigureAwait(false);
                }
                if (ValueChange.HasDelegate && !(Disabled || Readonly))
                {
                    ChangedEventArgs = new ChangedEventArgs<TValue>()
                    {
                        Value = ChangedArgs.Value,
                        Event = args is null ? new ChangeEventArgs() : args,
                        IsInteracted = args is not null
                    };
                    await InvokeAsync(() => ValueChange.InvokeAsync(ChangedEventArgs)).ConfigureAwait(false);
                }
                PreviousDate = Value;
                PreviousElementValue = CurrentValueAsString;
                if (CalendarMode == CalendarType.Islamic)
                {
                    PreviousElementValue = IslamicValueAsString;
                }
                IsChangeValue = true;
            }
        }

        internal override async Task InvokeSelectEventAsync(SelectedEventArgs<TValue> args)
        {
            if (Selected.HasDelegate)
            {
                await Selected.InvokeAsync(args).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Triggers when change event is triggered to update the selected value in input element .
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task SelectCalendarAsync(bool isSelection = false)
        {
            // Guard against disposed state during async operations
            if (IsDisposed)
            {
                return;
            }

            string date = string.Empty;
            IsValideValue = true;
            if (Value is not null && CurrentCulture is not null)
            {
                string formatString = GetDefaultFormat();
                formatString = GetStandardFormatString(formatString);
                if (ChangedArgs is not null)
                {
                    date = Intl.GetDateFormat(ChangedArgs.Value, formatString);
                }
            }
            if (!string.IsNullOrEmpty(date))
            {
                await UpdateInputValueAsync(date).ConfigureAwait(false);
                if (EnableMask)
                {
                    await CreateMaskAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Method used to update the value in input element.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        protected async Task UpdateInputAsync()
        {
            // Guard against disposed state during async operations
            if (IsDisposed)
            {
                return;
            }

            if (Value is null)
            {
                await UpdateValueAsync(null).ConfigureAwait(false);
            }
            else
            {
                await ProcessValueUpdateAsync().ConfigureAwait(false);
            }
            await FinalizeInputUpdateAsync().ConfigureAwait(false);
        }

        private async Task ProcessValueUpdateAsync()
        {
            if (StrictMode && !(IsFocused && ValidateOnInput))
            {
                await MinMaxUpdatesAsync().ConfigureAwait(false);
            }
            TValue? dateValue = Value;
            string formatString = GetStandardFormatString(GetDefaultFormat());
            string dateString = Intl.GetDateFormat(Value!, formatString);
            bool checkValue = (ConvertDateValue(dateValue!) >= Max) || (ConvertDateValue(dateValue!) <= Min);
            if (IsDateInRange(dateValue!) || (!StrictMode && checkValue))
            {
                if (dateString != CurrentValueAsString)
                {
                    await UpdateInputValueAsync(dateString).ConfigureAwait(false);
                }
            }
        }

        private bool IsDateInRange(TValue dateValue)
        {
            return Max.Ticks >= ConvertDateValue(dateValue).Ticks && Min.Ticks <= ConvertDateValue(dateValue).Ticks;
        }

        private async Task FinalizeInputUpdateAsync()
        {
            await UpdateStrictModeValueAsync().ConfigureAwait(false);
            ChangedArgs.Value = Value!;
            UpdateErrorClass();
            UpdateIconState();
        }

        private async Task UpdateStrictModeValueAsync()
        {
            if (Value is null)
            {
                if (StrictMode && !(IsFocused && ValidateOnInput) && !EnableMask)
                {
                    await UpdateInputValueAsync(null).ConfigureAwait(false);
                }
                else if (!string.IsNullOrEmpty(CurrentValueAsString))
                {
                    await UpdateInputValueAsync(CurrentValueAsString).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Method used to update the error class to the component.
        /// </summary>
        /// <exclude/>
        protected void UpdateErrorClass()
        {
            if (HasValidationError())
            {
                ApplyErrorState();
            }
            else if (HasFormValidationError())
            {
                ApplyFormErrorState();
            }
            else
            {
                ClearErrorState();
            }
        }

        private bool HasValidationError()
        {
            dynamic? disbleCellDetail = GetDisabledCellDetail();
            return (Value is not null && !(ConvertDateValue(Value) >= Min && ConvertDateValue(Value) <= Max)) || ((!StrictMode || (IsFocused && ValidateOnInput))
                && !string.IsNullOrEmpty(CurrentValueAsString) && Value is null && (CurrentMaskFormat != CurrentValueAsString)) || (IsKeyBoardAction && disbleCellDetail is not null);
        }

        private bool HasFormValidationError()
        {
            return IsFormValidation && Value is null && ValidClass == INVALID;
        }

        private void ApplyErrorState()
        {
            ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERRORCLASS);
            InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIAINVALID, TRUE, InputHtmlAttributes);
        }

        private void ApplyFormErrorState()
        {
            ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, SUCCESS_CLASS);
            ContainerClass = SfBaseUtils.AddClass(ContainerClass, ERRORCLASS);
            InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIAINVALID, TRUE, InputHtmlAttributes);
        }

        private void ClearErrorState()
        {
            ContainerClass = SfBaseUtils.RemoveClass(ContainerClass, ERRORCLASS);
            InputHtmlAttributes = SfBaseUtils.UpdateDictionary(ARIAINVALID, FALSE, InputHtmlAttributes);
        }

        /// <summary>
        /// Method update the properties Min and Max .
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exclude/>
        private async Task MinMaxUpdatesAsync()
        {
            // Guard against disposed state during async operations
            if (IsDisposed)
            {
                return;
            }

            await ApplyMinConstraintAsync().ConfigureAwait(false);
            await ApplyMaxConstraintAsync().ConfigureAwait(false);
            await FinalizeMaskUpdateAsync().ConfigureAwait(false);
        }

        private async Task ApplyMinConstraintAsync()
        {
            if (CompareValue(ConvertGeneric(Min), Value!) == 1 && Min <= Max && StrictMode && !(IsFocused && ValidateOnInput))
            {
                await UpdateValueAsync(ConvertGeneric(Min)).ConfigureAwait(false);
                ChangedArgs.Value = Value!;
                IsMinMax = true;
            }
        }

        private async Task ApplyMaxConstraintAsync()
        {
            if (CompareValue(Value!, ConvertGeneric(Max)) == 1 && Min <= Max && StrictMode && !(IsFocused && ValidateOnInput))
            {
                await UpdateValueAsync(ConvertGeneric(Max)).ConfigureAwait(false);
                ChangedArgs.Value = Value!;
                IsMinMax = true;
            }
        }

        private async Task FinalizeMaskUpdateAsync()
        {
            if (EnableMask && IsRendered)
            {
                await CreateMaskAsync().ConfigureAwait(false);
                IsMinMax = false;
            }
        }

        private void SetAllowEdit()
        {
            InputHtmlAttributes = AllowEdit ? !Readonly ? RemoveAttr(READONLYATTR, InputHtmlAttributes) :
                InputHtmlAttributes : SfBaseUtils.UpdateDictionary(READONLYATTR, true, InputHtmlAttributes);
        }

        internal override async Task BindNavigateEventAsync(NavigatedEventArgs eventArgs)
        {
            if (Navigated.HasDelegate)
            {
                await InvokeAsync(() => Navigated.InvokeAsync(eventArgs)).ConfigureAwait(false);
            }
        }

        internal override async Task BindRenderDayEventAsync(RenderDayCellEventArgs eventArgs)
        {
            eventArgs.CurrentView = CurrentView();
            await SfBaseUtils.InvokeEventAsync(DayCellRendering, eventArgs).ConfigureAwait(false);
            if (eventArgs.IsDisabled && eventArgs.Date.Date == DateTime.Now.Date && CalendarBaseInstance is not null && CalendarBaseInstance.TodayEleClass is not null)
            {
                CalendarBaseInstance.TodayEleClass = SfBaseUtils.AddClass(CalendarBaseInstance.TodayEleClass, DISABLE);
            }
        }
        private static Dictionary<string, object> RemoveAttr(string removeClass, Dictionary<string, object> attr)
        {
            attr.Remove(removeClass);
            return attr;
        }

        private void UpdateIconState()
        {
            ContainerClass = (!AllowEdit && !Readonly) ? string.IsNullOrEmpty(CurrentValueAsString) ?
                    SfBaseUtils.RemoveClass(ContainerClass, NOEDIT) : SfBaseUtils.AddClass(ContainerClass, NOEDIT) : SfBaseUtils.RemoveClass(ContainerClass, NOEDIT);
        }
        private static int CompareValue(TValue value1, TValue value2)
        {
            return Comparer<TValue>.Default.Compare(value1, value2);
        }

        internal async Task InputKeyActionHandlerAsync(KeyActions args, bool IsInput = false, string? inputvalue = null)
        {
            // Guard against disposed state during async keyboard handling
            if (IsDisposed)
            {
                return;
            }

            // Guard against null args which could cause NullReferenceException
            if (args is null || args.Action is null)
            {
                return;
            }

            switch (args.Action)
            {
                case MOVEDOWN:
                case MOVEUP:
                    await HandleMoveActionAsync(IsInput, inputvalue is null ? string.Empty : inputvalue, args).ConfigureAwait(false);
                    break;
                case ALT_UP_ARROW:
                    await HandleAltUpArrowAsync(args).ConfigureAwait(false);
                    break;
                case ALT_DOWN_ARROW:
                    await HandleAltDownArrowAsync(args).ConfigureAwait(false);
                    break;
                case ESCAPE:
                    await HidePopupAsync(args.Events).ConfigureAwait(false);
                    break;
                case ENTER:
                case "select":
                    await HandleEnterActionAsync(args).ConfigureAwait(false);
                    break;
                case TAB:
                case SHIFT_TAB:
                    await HandleTabActionAsync(args, IsInput).ConfigureAwait(false);
                    break;
                default:
                    await HandleDefaultActionAsync(args).ConfigureAwait(false);
                    break;
            }
        }

        private async Task HandleMoveActionAsync(bool isInput, string inputvalue, KeyActions args)
        {
            if (EnableMask && isInput && inputvalue is not null)
            {
                await KeyHandlerAsync(inputvalue).ConfigureAwait(false);
                IsChangeValue = false;
            }
            else
            {
                if (CalendarBaseInstance is not null)
                {
                    await CalendarBaseInstance.KeyActionHandlerAsync(args).ConfigureAwait(false);
                }
            }
        }

        private async Task HandleAltUpArrowAsync(KeyActions args)
        {
            await HidePopupAsync(args.Events).ConfigureAwait(false);
            await FocusAsync().ConfigureAwait(false);
        }

        private async Task HandleAltDownArrowAsync(KeyActions args)
        {
            if (IsListRender)
            {
                await HidePopupAsync().ConfigureAwait(false);
            }
            if (!IsCalendarRender)
            {
                IsDatePickerPopup = true;
                await StrictModeUpdateAsync().ConfigureAwait(false);
                await UpdateInputAsync().ConfigureAwait(false);
                await ChangeTriggerAsync(args.Events).ConfigureAwait(false);
                await OpenPopupAsync(args.Events).ConfigureAwait(false);
            }
            else
            {
                await KeyboardTimePopupActionAsync().ConfigureAwait(false);
            }
        }

        private async Task HandleEnterActionAsync(KeyActions args)
        {
            static string[] GetTargetClasses()
            {
                return ["e-calendar-content-table", "e-calendar e-lib e-keyboard", "e-calendar e-lib e-week-number e-keyboard", "e-calendar e-lib e-rtl e-keyboard"];
            }
            if (EnableMask && !IsKeySelect)
            {
                await KeyHandlerAsync(CurrentMaskValue).ConfigureAwait(false);
            }
            await ProcessEnterKeyAsync(args).ConfigureAwait(false);
            if (args.TargetClassList == "e-day e-title")
            {
                await HandleTitleNavigationAsync().ConfigureAwait(false);
            }
            else
            {
                await HandleDateSelectionAsync(args, GetTargetClasses()).ConfigureAwait(false);
            }
        }

        private async Task ProcessEnterKeyAsync(KeyActions args)
        {
            IsChangeValue = false;
            IsKeySelect = false;
            bool isTyped = IsTyped;
            IsTyped = false;
            await StrictModeUpdateAsync().ConfigureAwait(false);
            // Delay to ensure input synchronization after value change event
            await Task.Delay(VALUE_SYNC_DELAY_MS).ConfigureAwait(false);
            await UpdateInputAsync().ConfigureAwait(false);
            await ChangeTriggerAsync(args.Events).ConfigureAwait(false);
            UpdateErrorClass();
            if (!IsCalendarRender || isTyped)
            {
                await HidePopupAsync(args.Events).ConfigureAwait(false);
            }
        }

        private async Task HandleTitleNavigationAsync()
        {
            if (CalendarBaseInstance is not null)
            {
                await CalendarBaseInstance.NavigateTitleAsync().ConfigureAwait(false);
            }
        }

        private async Task HandleDateSelectionAsync(KeyActions args, string[] targetClasses)
        {
            bool isTyped = IsTyped;
            bool hasFocusedOrSelected = args.ClassList is not null && (args.ClassList.Contains("e-focused-date", StringComparison.OrdinalIgnoreCase) ||
                 args.ClassList.Contains("e-selected", StringComparison.OrdinalIgnoreCase));
            bool hasValidTarget = args.TargetClassList is not null && targetClasses.Any(targetClass => args.TargetClassList.Contains(targetClass, StringComparison.OrdinalIgnoreCase));
            if (!isTyped && hasFocusedOrSelected && hasValidTarget)
            {
                args.Action = "select";
                if (CalendarBaseInstance is not null)
                {
                    await CalendarBaseInstance.KeyActionHandlerAsync(args).ConfigureAwait(false);
                }
            }
        }

        private async Task HandleTabActionAsync(KeyActions args, bool isInput)
        {
            await StrictModeUpdateAsync().ConfigureAwait(false);
            await UpdateInputAsync().ConfigureAwait(false);
            await ChangeTriggerAsync(args.Events).ConfigureAwait(false);
            UpdateErrorClass();
            IsChangeValue = true;
            if (IsCalendarRender && args.Action == TAB)
            {
                await ProcessTabNavigationAsync(args, isInput).ConfigureAwait(false);
            }
        }

        private async Task ProcessTabNavigationAsync(KeyActions args, bool IsInput)
        {
            bool shouldMoveToPopup = args.Action == TAB && IsInput && IsCalendarRender;
            if (shouldMoveToPopup)
            {
                await MoveFocusToPopupAsync().ConfigureAwait(false);
            }
            else if ((args.Action == SHIFT_TAB || args.Action == TAB) && IsInput)
            {
                await FocusOutAsync().ConfigureAwait(false);
            }
        }

        private async Task HandleDefaultActionAsync(KeyActions args)
        {
            if (CalendarBaseInstance is not null)
            {
                await CalendarBaseInstance.KeyActionHandlerAsync(args).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Method which opens the time popup.
        /// </summary>
        /// <exclude/>
        protected virtual async Task KeyboardTimePopupActionAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <summary>
        /// Method allows to convert the value to generic type.
        /// </summary>
        /// <param name="dateValue">The dateValue<see cref="DateTime"/>.</param>
        /// <exclude/>
        protected virtual TValue ConvertGeneric(DateTime dateValue)
        {
            if (IsDateTimeOffsetType())
            {
                DateTimeOffset dynamicDateValue = dateValue != default ? dateValue : default(DateTimeOffset);
                int year = dynamicDateValue.Year;
                int month = dynamicDateValue.Month;
                int day = dynamicDateValue.Day;
                TimeSpan offset = dynamicDateValue.Offset;
                DateTimeOffset offsetValue = new(year, month, day, dateValue.Hour, dateValue.Minute, dateValue.Second, dateValue.Millisecond, offset);
                return (TValue)SfBaseUtils.ChangeType(offsetValue, typeof(TValue));
            }
            else if (IsDateOnlyType())
            {
                int year = DateOnly.FromDateTime(dateValue).Year;
                int month = DateOnly.FromDateTime(dateValue).Month;
                int day = DateOnly.FromDateTime(dateValue).Day;
                DateOnly dateOnlyValue = new(year, month, day);
                return (TValue)SfBaseUtils.ChangeType(dateOnlyValue, typeof(TValue));
            }
            else
            {
                return (TValue)SfBaseUtils.ChangeType(dateValue, typeof(TValue));
            }
        }

        /// <summary>
        /// Opens the popup asynchronously, handling the current input state before displaying it.
        /// </summary>
        /// <param name="args">
        /// Optional event arguments that may be passed when triggering the popup.
        /// </param>
        /// <exclude/>
        protected async Task OpenPopupAsync(EventArgs? args = null)
        {
            if (string.IsNullOrEmpty(CurrentInputValue) && IsTyped)
            {
                await UpdateValueAsync(null).ConfigureAwait(false);
                await UpdateInputValueAsync(null).ConfigureAwait(false);
            }
            else if (!string.IsNullOrEmpty(CurrentInputValue) && IsTyped)
            {
                CurrentValueAsString = CurrentInputValue;
            }
            await ShowPopupAsync(args).ConfigureAwait(false);
            IsTyped = false;
            CurrentInputValue = null;
        }

        /// <summary>
        /// Creates or refreshes the input mask for the DatePicker when mask support is enabled.
        /// </summary>
        /// <returns>A <see cref="Task"/> that completes when the mask has been created and the component state updated.</returns>
        /// <exclude/>
        protected async Task CreateMaskAsync()
        {
            DatePickerClientProps<TValue> options = GetClientProperties();
            ClientMaskValue = await InvokeAsync<ClientMaskValues>(_datePickerJsModule!, _datePickerJsInProcessModule!, "createMask", [DataId, options]).ConfigureAwait(false);
            if (ClientMaskValue is not null && !(Value is null && FloatLabelType == FloatLabelType.Auto))
            {
                CurrentValueAsString = ClientMaskValue.InputElementValue;
                CurrentMaskFormat = ClientMaskValue.CurrentMaskFormat;
            }
        }

        private void MaskPlaceholderContent()
        {
            if (EnableMask && Localizer is not null)
            {
                // Use indexed assignment to avoid throwing on duplicate keys and avoid repeated Add allocations
                MaskPlaceholderDictionary ??= [];
                MaskPlaceholderDictionary["Day"] = MaskPlaceholder is null || string.IsNullOrEmpty(MaskPlaceholder.Day) ? Localizer[DAYLOCALEKEY] : MaskPlaceholder.Day;
                MaskPlaceholderDictionary["Month"] = MaskPlaceholder is null || string.IsNullOrEmpty(MaskPlaceholder.Month) ? Localizer[MONTHLOCALEKEY] : MaskPlaceholder.Month;
                MaskPlaceholderDictionary["Year"] = MaskPlaceholder is null || string.IsNullOrEmpty(MaskPlaceholder.Year) ? Localizer[YEARLOCALEKEY] : MaskPlaceholder.Year;
                MaskPlaceholderDictionary["Hour"] = MaskPlaceholder is null || string.IsNullOrEmpty(MaskPlaceholder.Hour) ? Localizer[HOURLOCALEKEY] : MaskPlaceholder.Hour;
                MaskPlaceholderDictionary["Minute"] = MaskPlaceholder is null || string.IsNullOrEmpty(MaskPlaceholder.Minute) ? Localizer[MINUTELOCALEKEY] : MaskPlaceholder.Minute;
                MaskPlaceholderDictionary["Second"] = MaskPlaceholder is null || string.IsNullOrEmpty(MaskPlaceholder.Second) ? Localizer[SECONDLOCALEKEY] : MaskPlaceholder.Second;
                MaskPlaceholderDictionary["DayOfWeek"] = MaskPlaceholder is null || string.IsNullOrEmpty(MaskPlaceholder.DayOfWeek) ? Localizer[DAYOFWEEKLOCALEKEY] : MaskPlaceholder.DayOfWeek;
            }
        }
    }
}
