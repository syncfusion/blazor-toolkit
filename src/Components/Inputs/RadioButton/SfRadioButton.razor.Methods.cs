using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Syncfusion.Blazor.Toolkit.Inputs
{
    public partial class SfRadioButton<TChecked>
    {
        #region Logging
        /// <summary>
        /// LoggerMessage delegate for error processing click.
        /// </summary>
        private static readonly Action<ILogger, string, Exception> _logHandleClickError =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(1, nameof(HandleClickAsync)),
                "Error processing click in HandleClickAsync: {ExceptionMessage}");
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles user click interactions on the radio input and updates the component state.
        /// </summary>
        /// <param name="args">Mouse event arguments associated with the click.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task HandleClickAsync(MouseEventArgs args)
        {
            try
            {
                TChecked state = Value is NullLocalStorageValue
                    ? (TChecked)(object)null!
                    : Value is null
                        ? (TChecked)(object)false
                        : TryParseValueFromString(Value);

                if (EnablePersistence && Value is not null && Value.Equals(Checked))
                {
                    await UpdateCheckStateAsync((TChecked)(object)null!).ConfigureAwait(true);
                }

                await UpdateCheckStateAsync(state).ConfigureAwait(true);

                if (ValueChange.HasDelegate)
                {
                    await ValueChange.InvokeAsync(new ChangeArgs<TChecked> { Value = Checked, Event = args }).ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    _logHandleClickError(Logger, ex.Message, ex);
                }
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Attempts to parse the provided string value into the generic <typeparamref name="TChecked"/> type.
        /// </summary>
        /// <param name="value">The input string value.</param>
        /// <returns>The parsed value as <typeparamref name="TChecked"/>.</returns>
        private static TChecked TryParseValueFromString(string value)
        {
            bool isBoolType = typeof(TChecked) == typeof(bool) || Nullable.GetUnderlyingType(typeof(TChecked)) == typeof(bool);
            TChecked parsedValue = isBoolType
                ? (TChecked)(object)Convert.ToBoolean(value, CultureInfo.InvariantCulture)
                : (BindConverter.TryConvertTo(value, CultureInfo.CurrentCulture, out TChecked? result) ? result : default)!;

            return parsedValue;
        }
        #endregion
    }
}
