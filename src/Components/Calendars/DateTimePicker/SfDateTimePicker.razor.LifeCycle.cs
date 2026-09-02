using Microsoft.JSInterop;
using System.Globalization;
using System.Reflection;

namespace Syncfusion.Blazor.Toolkit.Calendars
{
    public partial class SfDateTimePicker<TValue> : SfDatePicker<TValue>
    {
        /// <summary>
        /// Performs one-time setup for the <see cref="SfDateTimePicker{TValue}"/> on top of the shared <see cref="SfDatePicker{TValue}"/> initialization.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// Invoked by the framework during the first render. The method assigns the time-icon class, registers the component reference with the parent composite control when the parent is a <c>DateTime</c> type, and adjusts <see cref="SfCalendar{TValue}.Min"/> and <see cref="SfCalendar{TValue}.Max"/> to the supported Islamic (Hijri) range when <see cref="SfCalendar{TValue}.CalendarMode"/> is <see cref="CalendarType.Islamic"/>.
        /// </remarks>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
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
        /// Releases DateTimePicker-specific resources and delegates the remaining disposal to the base <see cref="SfDatePicker{TValue}"/>.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous disposal operation.</returns>
        /// <remarks>
        /// Invoked by the framework when the component is being torn down. The override calls <c>ComponentDisposeAsync</c> to clear the time-icon state, popup event arguments, and time-popup list data, then delegates to <see cref="SfDatePicker{TValue}.DisposeAsyncCore"/> for the rest of the cleanup. <see cref="JSDisconnectedException"/> and <see cref="ObjectDisposedException"/> are caught so that disposal completes safely when the Blazor circuit has already been torn down.
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
            try
            {
                DateIcon = string.Empty;
                TimeIcon = string.Empty;
                PopupEventArgs = default!;
                ListData = null;
            }
            catch (JSDisconnectedException)
            {
                // Ignore: The circuit disconnected (e.g., page reload) before JS disposal could complete.
            }
            catch (ObjectDisposedException)
            {
                // Ignore: Already disposed
            }
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
