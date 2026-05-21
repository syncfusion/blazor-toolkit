using Microsoft.JSInterop;
using System.Globalization;
using System.Reflection;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    public partial class SfDateTimePicker<TValue> : SfDatePicker<TValue>
    {
        /// <summary>
        /// Triggers during the initial rendering of the component and sets up default configurations.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method initializes the component by setting up the component ID, configuring Islamic calendar settings if needed,
        /// updating input values, and establishing parent-child relationships for nested DateTimePicker scenarios.
        /// It also handles initial value formatting and calendar mode-specific configurations.
        /// </remarks>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            if (string.IsNullOrEmpty(ID))
            {
                ID = "datetimepicker-" + Guid.NewGuid().ToString();
            }
            DataId = ID;
            if (Value != null)
            {
                if (CalendarMode == CalendarType.Islamic)
                {
                    IslamicValueAsString = ConvertToHijri(Value, GetDefaultFormat());
                }
                await UpdateInputAsync().ConfigureAwait(false);
            }

            TimeIcon = TIME_ICON;
            if (DateTimePickerParent != null && Convert.ToString(DateTimePickerParent?.Type, CultureInfo.CurrentCulture) == "DateTime")
            {
                PropertyInfo? componentRefProperty = DateTimePickerParent?.GetType().GetProperty("ComponentRef", BindingFlags.NonPublic | BindingFlags.Instance);
                componentRefProperty?.SetValue(DateTimePickerParent, this);
            }
            if (CalendarMode == CalendarType.Islamic)
            {
                if (Min == new DateTime(1900, 1, 1))
                {
                    Min = new DateTime(1944, 2, 18);
                }
                if (Max == new DateTime(2099, 12, 31))
                {
                    Max = new DateTime(2069, 10, 16);
                }
            }
        }

        /// <summary>
        /// Performs cleanup operations when the component is being disposed.
        /// </summary>
        /// <remarks>
        /// This method handles the disposal of client-side resources, invokes the Destroyed event callback,
        /// and cleans up internal references to prevent memory leaks. It ensures proper cleanup of
        /// JavaScript interop resources and event handlers.
        /// </remarks>
        /// <exclude/>
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
            if (IsRendered)
            {
                try
                {
                    object[] destroyArgs = [DataId, PopupElement, PopupHolderEle, new PopupEventArgs() { Cancel = false }, GetClientProperties()];
                    await InvokeVoidAsync(_datePickerJsModule!, _datePickerJsInProcessModule!, "destroy", destroyArgs).ConfigureAwait(false);
                }
                catch (JSDisconnectedException)
                {
                    // Ignore: The circuit disconnected before JS disposal could complete.
                }
                catch (ObjectDisposedException)
                {
                    // Ignore: Module already disposed
                }
                try
                {
                    if (Destroyed.HasDelegate)
                    {
                        await InvokeAsync(() => Destroyed.InvokeAsync(null)).ConfigureAwait(false);
                    }
                }
                catch (JSDisconnectedException)
                {
                    // Ignore: The circuit disconnected before event could fire.
                }
            }
            try
            {
                if (_datePickerJsModule is not null)
                {
                    await _datePickerJsModule.DisposeAsync().ConfigureAwait(false);
                    _datePickerJsModule = null;
                }
                _datePickerJsInProcessModule?.Dispose();
                _datePickerJsInProcessModule = null;
            }
            catch (JSDisconnectedException)
            {
                // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
            }
            catch (ObjectDisposedException)
            {
                // Ignore: Already disposed
            }
            DateIcon = string.Empty;
            TimeIcon = string.Empty;
            PopupEventArgs = default!;
            ListData = null;
        }
    }
}
