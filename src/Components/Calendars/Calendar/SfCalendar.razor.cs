using Syncfusion.Blazor.Toolkit.Calendars.Internal;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    /// <summary>
    /// Represents the Syncfusion Blazor Calendar component, which provides a user-friendly interface for displaying and selecting dates on a Gregorian calendar.
    /// </summary>
    /// <remarks>
    /// <para>The <see cref="SfCalendar{TValue}"/> component allows users to navigate through months and years, select single or multiple dates, and customize the display with templates and styles. It supports localization, RTL, and various accessibility features. For advanced scenarios, the component raises events to handle value changes, cell rendering, and navigation actions.</para>
    /// <para>This component is commonly used for inputting or displaying dates inline, either standalone or as part of a larger form.</para>
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to declare and use the <see cref="SfCalendar{TValue}"/> component in a Razor file:
    /// <code><![CDATA[
    /// <SfCalendar TValue="DateTime" @bind-Value="SelectedDate"></SfCalendar>
    /// @code {
    ///     private DateTime? SelectedDate = DateTime.Today;
    /// }
    /// ]]></code>
    /// </example>
    public partial class SfCalendar<TValue> : CalendarBase<TValue>
    {
        internal const string VALUECHANGE_EVENT = "ValueChange";
        internal const string WEEK_NUMBER = "e-week-number";
        internal const string RTL = "e-rtl";
        internal const string DAYHEADERLONG = "e-calendar-day-header-lg";
        internal const string OVERLAY = "e-overlay";
        internal const string CLASS = "class";
        internal const string LOCALSTORAGE_GET_ITEM = "getLocalStorageItem";
        internal const string LOCALSTORAGE_SET_ITEM = "setLocalStorageItem";
        private Dictionary<string, object>? _containerAttr = [];
        internal CalendarBaseRender<TValue>? CalendarBase { get; set; }
        private bool IsMultipleDatesProgrammaticallySet { get; set; }

        private void InitRender()
        {
            ValidateMinMax();
            ApplyMultiSelectionDefaults();
            ApplyDayHeaderFormat();
            ApplyRtlSettings();
            ProcessHtmlAttributes();
            ApplyWeekNumberSetting();
        }

        private void ValidateMinMax()
        {
            RootClass = !(Min <= Max) ? SfBaseUtils.AddClass(RootClass, OVERLAY) : SfBaseUtils.RemoveClass(RootClass, OVERLAY);
        }

        private void ApplyMultiSelectionDefaults()
        {
            if (IsMultiSelection && Values is not null && Values.Length > 0)
            {
                TValue? tempValue = GenericValue(Values[^1]);
                if (tempValue is not null)
                {
                    Value = tempValue;
                    PreviousValues = Values.Length;
                }
            }
        }

        private void ApplyDayHeaderFormat()
        {
            RootClass = (DayHeaderFormat == DayHeaderFormats.Wide) ? SfBaseUtils.AddClass(RootClass, DAYHEADERLONG) : SfBaseUtils.RemoveClass(RootClass, DAYHEADERLONG);
        }

        private void ApplyRtlSettings()
        {
            RootClass = SyncfusionService is not null && SyncfusionService._options.EnableRtl ? SfBaseUtils.AddClass(RootClass, RTL) : SfBaseUtils.RemoveClass(RootClass, RTL);
        }

        private void ProcessHtmlAttributes()
        {
            if (HtmlAttributes is null)
            {
                return;
            }
            foreach (KeyValuePair<string, object> item in HtmlAttributes)
            {
                if (ContainerAttributes is not null && _containerAttr is not null && !ContainerAttributes.Contains(item.Key))
                {
                    _containerAttr = SfBaseUtils.UpdateDictionary(item.Key, item.Value, _containerAttr);
                }
                else
                {
                    ApplyContainerAttribute(item);
                }
            }
        }

        private void ApplyContainerAttribute(KeyValuePair<string, object> item)
        {
            string itemClass = item.Value is not null ? item.Key.ToString() : string.Empty;
            if (item.Key == CLASS)
            {
                RootClass = SfBaseUtils.AddClass(RootClass, itemClass);
            }
            else if (_containerAttr is not null)
            {
                _containerAttr = SfBaseUtils.UpdateDictionary(item.Key, itemClass, _containerAttr);
            }
        }

        private void ApplyWeekNumberSetting()
        {
            RootClass = WeekNumber ? SfBaseUtils.AddClass(RootClass, WEEK_NUMBER) : SfBaseUtils.RemoveClass(RootClass, WEEK_NUMBER);
        }

        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                RootClass = SfBaseUtils.AddClass(RootClass, CssClass);
            }
        }

        private new void SetEnabled()
        {
            RootClass = Disabled ? SfBaseUtils.AddClass(RootClass, DISABLE) : SfBaseUtils.RemoveClass(RootClass, DISABLE);
        }

        internal override async Task UpdateCalendarPropertyAsync(string key, object? dateValue)
        {
            if (!Disabled)
            {
                if (key == nameof(Value))
                {
                    TValue tempValue = (TValue)SfBaseUtils.ChangeType(dateValue!, typeof(TValue));
                    Value = CalendarBase_Value = await SfBaseUtils.UpdatePropertyAsync(tempValue!, CalendarBase_Value!, ValueChanged, CalendarEditContext!, ValueExpression);
                }
                else
                {
                    Values = Calendar_Values = await SfBaseUtils.UpdatePropertyAsync((DateTime[])dateValue!, Calendar_Values!, ValuesChanged, CalendarEditContext!, ValuesExpression);
                }
            }
        }

        internal override void BindNavigateEvent(NavigatedEventArgs eventArgs)
        {
            if (Navigated.HasDelegate)
            {
                _ = InvokeAsync(() => Navigated.InvokeAsync(eventArgs));
            }
        }

        internal override async Task BindRenderDayEventAsync(RenderDayCellEventArgs eventArgs)
        {
            eventArgs.CurrentView = CurrentView();
            if (DayCellRendering.HasDelegate)
            {
                await DayCellRendering.InvokeAsync(eventArgs).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Invoked when the calendar value changes, either through user selection or programmatic update.
        /// </summary>
        /// <param name="args">Specifies the event arguments associated with the value change, usually from a user interaction or input.</param>
        /// <param name="isSelection">Determines whether the selection was made using the mouse or keyboard. <c>true</c> if selected by the user; otherwise, <c>false</c>.</param>
        /// <remarks>
        /// This method notifies subscribers of the <c>ValueChange</c> event and persists the new value if <c>EnablePersistence</c> is enabled. It updates the <c>PreviousDate</c> property to the newly selected value after change notification.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// protected override void ChangeEvent(EventArgs? args, bool isSelection = false)
        /// {
        ///     // Custom logic on value change
        /// }
        /// ]]></code>
        /// </example>
        /// <exclude />
        protected override void ChangeEvent(EventArgs? args, bool isSelection = false)
        {
            if (Disabled)
            {
                return;
            }
            if (EnablePersistence)
            {
                _ = SetLocalStorageAsync(ID, Value!);
            }
            if (ValueChange.HasDelegate)
            {
                ChangedArgs.Name = VALUECHANGE_EVENT;
                _ = ValueChange.InvokeAsync(ChangedArgs);
            }
            PreviousDate = (TValue)SfBaseUtils.ChangeType(Value!, typeof(TValue));
        }

        /// <summary>
        /// Checks whether the Values property changes dynamically in multiselection.
        /// </summary>
        /// <returns>Returns bool value.</returns>
        /// <exclude />
        internal override bool IsMultipleDatesSetProgrammatically()
        {
            bool temp = IsMultipleDatesProgrammaticallySet;
            if (IsMultipleDatesProgrammaticallySet)
            {
                IsMultipleDatesProgrammaticallySet = false;
            }
            return temp;
        }

        /// <summary>
        /// Invokes the deselect event when a date is deselected.
        /// </summary>
        /// <param name="args">The deselection event arguments.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal override async Task InvokeDeSelectEventAsync(DeSelectedEventArgs<TValue> args)
        {
            if (DeSelected.HasDelegate)
            {
                await InvokeAsync(() => DeSelected.InvokeAsync(args)).ConfigureAwait(true);
            }
        }

        internal override async Task InvokeSelectEventAsync(SelectedEventArgs<TValue> args)
        {
            if (Selected.HasDelegate)
            {
                await InvokeAsync(() => Selected.InvokeAsync(args)).ConfigureAwait(true);
            }
        }
    }
}