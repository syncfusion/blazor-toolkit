using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit.Charts
{
    /// <summary>
    /// Partial class containing pointer and mouse event handlers for the SfChart component.
    /// </summary>
    public partial class SfChart
    {
        #region Fields

        private DateTime _lastClickTime = DateTime.MinValue;
        private int _rapidClickCount;
        /*To store the given DateTime value and used for time delay.*/
        private DateTime _threshold;
        private bool _isLegendClick = true;

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether the current interaction is from a touch-enabled device.
        /// </summary>
        /// <param name="args">The chart internal mouse event arguments containing pointer type information</param>
        private void IsTouchEnabled(ChartInternalMouseEventArgs args)
        {
            _isTouch = args.Type.Contains("touch", StringComparison.InvariantCulture) || args.PointerType == "touch" || args.PointerType == "2";
        }

        /// <summary>
        /// Processes mouse move events over the chart and triggers associated handlers.
        /// </summary>
        /// <param name="args">The chart internal mouse event arguments containing position and target information</param>
        private void ProcessChartMouseMove(ChartInternalMouseEventArgs args)
        {
            args.ID = ID;
            _mouseX = args.MouseX;
            _mouseY = args.MouseY;
            MouseMove?.Invoke(this, args);
            if (ChartMouseMove is not null)
            {
                ChartMouseEventArgs mouseMoveArgs = new("onmousemove", args.MouseX, args.MouseY, args.ID);
                FindXYPointValue(mouseMoveArgs);
                if (ChartMouseMove is not null)
                {
                    DataVizCommonHelper.InvokeEvent(ChartMouseMove, mouseMoveArgs);
                }
            }
            if (!_isTouch)
            {
                TitleTooltip(args);
                AxisTooltip(args);
            }
            if (_isChartFirstRender)
            {
                _dataEditingModule?.PointMouseMove();
            }
            if (_striplineTooltipModule is not null)
            {
                _ = _striplineTooltipModule.MouseMoveHandlerAsync(args.Target);
            }
        }

        /// <summary>
        /// Handles mouse click events on the chart and triggers associated event callbacks.
        /// </summary>
        /// <param name="args">The chart internal mouse event arguments containing click position and target information</param>
        private void ChartMouseClickHandler(ChartInternalMouseEventArgs args)
        {
            args.ID = ID;
            _mouseX = args.MouseX;
            _mouseY = args.MouseY;
            MouseClick?.Invoke(this, args);
            if (ChartMouseClick.HasDelegate)
            {
                ChartMouseEventArgs mouseClickArgs = new("onmouseclick", args.MouseX, args.MouseY, args.ID);
                FindXYPointValue(mouseClickArgs);
                _ = InvokeDelegateAsync(ChartMouseClick, mouseClickArgs);
            }
            if (!_isTouch)
            {
                TitleTooltip(args);
                AxisTooltip(args);
            }
            if (OnPointClick.HasDelegate)
            {
                TriggerPointEvent(Constants.PointerClick, OnPointClick, args);
            }
            if (OnMultiLevelLabelClick is not null)
            {
                TriggerMultilevelLabelClick(args);
            }

            if (OnAxisLabelClick is not null)
            {
                TriggerAxisLabelClickEvent(args);
            }
        }

        /// <summary>
        /// Handles mouse down events on the chart and initiates drag operations.
        /// </summary>
        /// <param name="args">The chart internal mouse event arguments containing pointer down position and target information</param>
        private async Task OnMouseDownAsync(ChartInternalMouseEventArgs args)
        {
            args.ID = ID;
            _mouseX = args.MouseX;
            _mouseY = args.MouseY;
            _mouseDownX = _previousMouseMoveX = args.MouseX;
            _mouseDownY = _previousMouseMoveY = args.MouseY;
            IsTouchEnabled(args);
            double offset = _browser is not null && _browser.IsDevice ? Constants.TouchOffset : Constants.MouseOffset;
            if (_isTouch)
            {
                _isDoubleTap = DateTime.Now < _threshold && !args.Target.Contains(ID + "_Zooming_", StringComparison.InvariantCulture) &&
                    (_mouseDownX - offset >= _mouseX || _mouseDownX + offset >= _mouseX) &&
                    (_mouseDownY - offset >= _mouseY || _mouseDownY + offset >= _mouseY) &&
                    (_mouseX - offset >= _mouseDownX || _mouseX + offset >= _mouseDownX) &&
                    (_mouseY - offset >= _mouseDownY || _mouseY + offset >= _mouseDownY);
            }
            MouseDown?.Invoke(this, args);
            if (ChartMouseDown is not null)
            {
                ChartMouseEventArgs mouseDownArgs = new("onmousedown", args.MouseX, args.MouseY, args.ID);
                FindXYPointValue(mouseDownArgs);
                if (ChartMouseDown is not null)
                {
                    DataVizCommonHelper.InvokeEvent(ChartMouseDown, mouseDownArgs);
                }
            }
            _dataEditingModule?.PointMouseDown();
            if (_isTouch)
            {
                TitleTooltip(args);
                AxisTooltip(args);
                _threshold = DateTime.Now.AddMilliseconds(Constants.PointerThersholdMs);
            }
            if (_isTouch)
            {
                if (args.Target.Contains("legend", StringComparison.OrdinalIgnoreCase))
                {
                    _isLegendClick = false;
                    await CalculateSecondaryElementPositionAsync().ConfigureAwait(true);
                    await HandleLegendClickAsync(args).ConfigureAwait(true);
                }
            }
            if (!_isTouch && _isLegendClick)
            {
                await HandleLegendClickAsync(args).ConfigureAwait(true);
            }

            if (!_isTouch && !_isLegendClick && (_interaction.PreviousMouseMoveReqTime == DateTime.MinValue || (DateTime.Now - _interaction.PreviousMouseMoveReqTime).TotalMilliseconds >= Constants.UpdateThersholdMs))
            {
                _interaction.PreviousMouseMoveReqTime = DateTime.Now;
                _isLegendClick = true;
            }
        }

        /// <summary>
        /// Handles mouse up events on the chart and completes drag operations.
        /// </summary>
        /// <param name="args">The chart internal mouse event arguments containing pointer up position information</param>
        private void OnMouseUp(ChartInternalMouseEventArgs args)
        {
            args.ID = ID;
            IsTouchEnabled(args);
            _isChartDrag = false;
            MouseUp?.Invoke(this, args);
            if (ChartMouseUp is not null)
            {
                ChartMouseEventArgs mouseUpArgs = new("onmouseup", args.MouseX, args.MouseY, args.ID);
                FindXYPointValue(mouseUpArgs);
                if (ChartMouseUp is not null)
                {
                    DataVizCommonHelper.InvokeEvent(ChartMouseUp, mouseUpArgs);
                }
            }
            _dataEditingModule?.PointMouseUp();
        }

        /// <summary>
        /// Handles legend click events with debouncing to prevent rapid successive clicks.
        /// </summary>
        /// <param name="arguments">The chart internal mouse event arguments containing the legend click information</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        private async Task HandleLegendClickAsync(ChartInternalMouseEventArgs arguments)
        {
            DateTime currentClickTime = DateTime.Now;
            if (_lastClickTime != DateTime.MinValue)
            {
                TimeSpan timeDifference = currentClickTime - _lastClickTime;
                if (timeDifference.TotalMilliseconds < Constants.ClickThersholdMs)
                {
                    _rapidClickCount++;
                }
                else
                {
                    // If the click is not rapid
                    _rapidClickCount = 1;
                }
            }
            else
            {
                // If the click is first one.
                _rapidClickCount = 1;
            }

            _lastClickTime = currentClickTime;

            //Introduce a delay and check if another click occurs within the _threshold
            await Task.Delay(Constants.ClickDelayMs).ConfigureAwait(true);

            //Check if the current click is the last one in the sequence
            if ((DateTime.Now - _lastClickTime).TotalMilliseconds >= Constants.ClickThersholdMs)
            {
                ExecuteLegendClick(arguments);
            }
        }

        /// <summary>
        /// Executes the legend click action after debouncing validation.
        /// </summary>
        /// <param name="arguments">The chart internal mouse event arguments containing the legend click information</param>
        private void ExecuteLegendClick(ChartInternalMouseEventArgs arguments)
        {
            // This method is called on the last click of the rapid sequence
            if (_rapidClickCount > 0)
            {
                _rapidClickCount = 0;
                _lastClickTime = DateTime.MinValue;
                _legendRenderer?.Click(arguments);
            }
        }
        #endregion
    }
}
