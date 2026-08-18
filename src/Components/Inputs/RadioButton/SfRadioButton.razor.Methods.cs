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
            if (Disabled)
            {
                return;
            }

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
            catch (Exception ex) when (Logger is not null)
            {
                // Logger is wired up: capture the exception for diagnostics and prevent the failure
                // from surfacing to Blazor's error boundary (which would tear down the circuit).
                _logHandleClickError(Logger, ex.Message, ex);
            }
            catch (Exception)
            {
                // No logger is configured: rethrow so the developer is alerted to the failure.
                throw;
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
            return isBoolType
                ? (TChecked)(object)Convert.ToBoolean(value, CultureInfo.InvariantCulture)
                : (TChecked)Convert.ChangeType(value, typeof(TChecked), CultureInfo.CurrentCulture);
        }
        #endregion
    }
}
