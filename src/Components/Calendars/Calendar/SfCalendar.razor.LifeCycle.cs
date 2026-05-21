using Microsoft.JSInterop;
using Syncfusion.Blazor.Toolkit.Internal;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    public partial class SfCalendar<TValue> : CalendarBase<TValue>
    {
        internal const string ROOT = "e-control e-calendar e-lib";

        /// <summary>
        /// Gets or sets the root CSS class string applied to the calendar's top-level element.
        /// </summary>
        /// <exclude />
        protected override string RootClass { get; set; } = string.Empty;

        /// <summary>
        /// Invoked during the initial rendering of the <see cref="SfCalendar{TValue}"/> component, enabling one-time setup and initialization logic.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// This method is called by the Blazor framework when the component is first initialized. It is primarily used to set up component state and apply default or configured values to all internal and dependent properties before the UI is rendered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// protected override async Task OnInitializedAsync()
        /// {
        ///     await base.OnInitializedAsync();
        ///     // Additional setup logic here
        /// }
        /// ]]></code>
        /// </example>
        /// <exclude />
        protected override async Task OnInitializedAsync()
        {
            if (Value is not null)
            {
                PreviousDate = (TValue)SfBaseUtils.ChangeType(Value, typeof(TValue));
            }
            await base.OnInitializedAsync().ConfigureAwait(false);
            Calendar_CssClass = CssClass;
            Calendar_Disabled = Disabled;
            CalendarBase_Value = Value;
            CalendarBase_Depth = Depth;
            CalendarBase_Start = Start;
            Calendar_Values = Values;
            CalendarBase_Min = Min;
            CalendarBase_Max = Max;
            CalendarBase_FirstDayOfWeek = FirstDayOfWeek;
            if (string.IsNullOrEmpty(ID))
            {
                ID = "calendar-" + Guid.NewGuid().ToString();
            }
            RootClass = ROOT;
            SetEnabled();
        }

        /// <summary>
        /// Called by the framework when the set of component parameters has changed, allowing the component to react to dynamic property changes and state updates.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        /// <remarks>
        /// Use this lifecycle method to handle logic that depends on updated parameters, such as refreshing the display or updating dependent property values. This method is executed whenever a parent component re-renders and passes new parameter values to this component.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// protected override async Task OnParametersSetAsync()
        /// {
        ///     await base.OnParametersSetAsync();
        ///     // Recompute dependent properties here
        /// }
        /// ]]></code>
        /// </example>
        /// <exclude />
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            await PropertyParametersSetAsync().ConfigureAwait(false);
            InitRender();
            await ProcessPropertyChangesAsync().ConfigureAwait(false);
            SetCssClass();
        }

        private async Task ProcessPropertyChangesAsync()
        {
            if (PropertyChanges is not null && PropertyChanges.Count == 0)
            {
                return;
            }
            await HandleValuesChangeAsync().ConfigureAwait(false);
            await HandleValueChangeAsync().ConfigureAwait(false);
            HandleCssClassChange();
            HandleEnabledChange();
        }

        private async Task HandleValuesChangeAsync()
        {
            if (PropertyChanges is not null && PropertyChanges.ContainsKey(nameof(Values)) && Values?.Length > 0)
            {
                await UpdateCalendarPropertyAsync(nameof(Value), Values?.LastOrDefault()).ConfigureAwait(false);
            }
        }

        private async Task HandleValueChangeAsync()
        {
            if (PropertyChanges is not null && PropertyChanges.ContainsKey(nameof(Value)))
            {
                await UpdateCalendarPropertyAsync(nameof(Value), Value).ConfigureAwait(false);
            }
        }

        private void HandleCssClassChange()
        {
            if (PropertyChanges is not null && PropertyChanges.ContainsKey(nameof(CssClass)))
            {
                RootClass = string.IsNullOrEmpty(RootClass) ? RootClass : SfBaseUtils.RemoveClass(RootClass, Calendar_CssClass);
                Calendar_CssClass = CssClass;
            }
        }

        private void HandleEnabledChange()
        {
            if (PropertyChanges is not null && PropertyChanges.ContainsKey(nameof(Disabled)))
            {
                Calendar_Disabled = Disabled;
                SetEnabled();
            }
        }

        /// <summary>
        /// Executes logic after the component has been rendered in the UI, handling post-rendering operations such as event registration and persistence.
        /// </summary>
        /// <param name="firstRender">If <c>true</c>, indicates this is the first time the component is rendered; otherwise, <c>false</c>.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation performed after rendering.
        /// </returns>
        /// <remarks>
        /// This method is mainly used to perform additional initialization, interact with JavaScript interop, and handle UI state persistence after the first render. It is also responsible for firing the <c>Created</c> event.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// protected override async Task OnAfterRenderAsync(bool firstRender)
        /// {
        ///     if (firstRender) { /* one-time logic */ }
        /// }
        /// ]]></code>
        /// </example>
        /// <exclude />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            if (firstRender)
            {
                bool stateChanged = await HandlePersistenceAsync().ConfigureAwait(false);
                await InvokeCreatedEventAsync().ConfigureAwait(false);
                if (stateChanged)
                {
                    await InvokeAsync(StateHasChanged).ConfigureAwait(false);
                }
            }
        }

        private async Task<bool> HandlePersistenceAsync()
        {
            if (!EnablePersistence)
            {
                return false;
            }
            string localStorageValue = await InvokeAsync<string>(_baseJsModule!, _baseJsInProcessModule!, LOCALSTORAGE_GET_ITEM, [ID]).ConfigureAwait(true);
            if (string.IsNullOrEmpty(localStorageValue) || localStorageValue == "null" || Value is not null)
            {
                return false;
            }
            string localValue = localStorageValue;
            TValue? persistValue = (TValue)SfBaseUtils.ChangeType(localValue, typeof(TValue));
            if (EqualityComparer<TValue>.Default.Equals(persistValue, CalendarBase_Value))
            {
                return false;
            }
            _ = NotifyPropertyChanges(nameof(Value), persistValue, CalendarBase_Value);
            await UpdateCalendarPropertyAsync(nameof(Value), persistValue).ConfigureAwait(false);
            return true;
        }

        private async Task InvokeCreatedEventAsync()
        {
            if (Created.HasDelegate)
            {
                await InvokeAsync(() => Created.InvokeAsync(null)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Triggers while dynamically changing the properties of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <summary>
        /// Handles property parameter updates and change notifications.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task PropertyParametersSetAsync()
        {
            CalendarBase_Start = NotifyPropertyChanges(nameof(Start), Start, CalendarBase_Start);
            CalendarBase_Depth = NotifyPropertyChanges(nameof(Depth), Depth, CalendarBase_Depth);
            CalendarBase_FirstDayOfWeek = NotifyPropertyChanges(nameof(FirstDayOfWeek), FirstDayOfWeek, CalendarBase_FirstDayOfWeek);
            CalendarBase_Min = NotifyPropertyChanges(nameof(Min), Min, CalendarBase_Min);
            CalendarBase_Max = NotifyPropertyChanges(nameof(Max), Max, CalendarBase_Max);
            _ = NotifyPropertyChanges(nameof(Disabled), Disabled, Calendar_Disabled);
            _ = NotifyPropertyChanges(nameof(CssClass), CssClass, Calendar_CssClass);
            CalendarBase_Value = NotifyPropertyChanges(nameof(Value), Value, CalendarBase_Value);
            Calendar_Values = NotifyPropertyChanges(nameof(Values), Values, Calendar_Values);
            await Task.CompletedTask.ConfigureAwait(false);
        }

        internal override async Task ImportComponentModuleAsync()
        {
            await base.ImportComponentModuleAsync().ConfigureAwait(true);
            // Load animation-related script before calendars scripts
            await LoadAnimationScriptAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Invoke the component dispose.
        /// </summary>
        /// <exclude />
        protected override async ValueTask DisposeAsyncCore()
        {
            try
            {
                await ComponentDisposeAsync().ConfigureAwait(false);
            }
            catch (JSDisconnectedException)
            {
                // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
            }
            catch (ObjectDisposedException)
            {
                // Ignore: Already disposed
            }
            await base.DisposeAsyncCore().ConfigureAwait(true);
        }

        private async Task ComponentDisposeAsync()
        {
            if (!IsRendered)
            {
                return;
            }
            IsDisposed = true;
            try
            {
                if (Destroyed.HasDelegate)
                {
                    await Destroyed.InvokeAsync(null).ConfigureAwait(false);
                }
            }
            catch (JSDisconnectedException)
            {
                // Ignore: The circuit disconnected before event could fire.
            }
            // Clear event callbacks to prevent memory leaks
            ValueChange = default;
            Selected = default;
            DeSelected = default;
            Created = default;
            Destroyed = default;
            Navigated = default;
            DayCellRendering = default;
            ValuesChanged = default;
            ChangedArgs = default!;
            ContainerAttributes = null;
            _containerAttr = null;
            CellDetailsData = null;
            CalendarBase = null;
            ChildContent = default!;
            SyncfusionService = null;
        }
    }
}