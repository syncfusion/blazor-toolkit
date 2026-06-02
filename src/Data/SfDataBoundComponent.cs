using System.Collections;
using System.Linq.Expressions;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Toolkit.Data
{
    /// <summary>
    /// Provides the base class for components that work with data managers.
    /// </summary>
    public abstract class SfDataBoundComponent : SfBaseComponent
    {
        /// <summary>
        /// Gets or sets the data manager used by the component.
        /// </summary>
        [JsonIgnore]
        public DataManager DataManager { get; set; }

        /// <summary>
        /// Gets or sets the main parent component when the data-bound component is nested.
        /// </summary>
        protected virtual SfBaseComponent? MainParent { get; set; }

        /// <summary>
        /// Tracks parameter keys passed directly to the component for change detection.
        /// </summary>
        /// <exclude />
        internal List<string> DirectParamKeys { get; set; } = [];

        /// <summary>
        /// Stores parameter values passed directly to the component.
        /// </summary>
        /// <exclude />
        internal Dictionary<string, object> DirectParameters { get; set; } = [];

        /// <summary>
        /// Indicates whether the component is in a re-rendering cycle.
        /// </summary>
        /// <exclude />
        internal bool IsRerendering { get; set; }

        /// <exclude />
        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            if (DirectParamKeys.Count == 0)
            {
                foreach (ParameterValue parameter in parameters)
                {
                    if (!parameter.Cascading)
                    {
                        DirectParamKeys.Add(parameter.Name);
                    }
                }
            }

            return base.SetParametersAsync(parameters);
        }

        /// <summary>
        /// Creates or assigns a data manager for the provided data source.
        /// </summary>
        /// <typeparam name="T">The type used when creating an empty local data source.</typeparam>
        /// <param name="dataSource">The data source to adapt.</param>
        /// <returns>The assigned data manager or original data source when already a data manager instance.</returns>
        protected object SetDataManager<T>(object dataSource)
        {
            if (dataSource is Data.SfDataManager || dataSource is DataManager)
            {
                return dataSource;
            }

            // For Blazor Adaptor
            if (dataSource != null)
            {
                Type type = dataSource.GetType();
                if (typeof(IEnumerable).IsAssignableFrom(type) ^ typeof(IEnumerable<object>).IsAssignableFrom(type))
                {
                    DataManager = new DataManager() { Json = ((IEnumerable)dataSource).Cast<object>() };
                }
                else
                {
                    DataManager = dataSource is IQueryable
                        ? new DataManager() { Json = [.. (IEnumerable<object>)dataSource] }
                        : new DataManager() { Json = (IEnumerable<object>)dataSource };
                }
            }
            else
            {
                DataManager ??= new DataManager() { Json = [.. Enumerable.Empty<T>().Cast<object>()] };
            }

            return DataManager;
        }

        /// <summary>
        /// Initializes the component and prepares direct parameter tracking.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);
            DirectParameters = [];
        }

        /// <summary>
        /// Handles parameter updates and marks the component for rerender processing.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(false);
            IsRerendering = true;
        }

        /// <summary>
        /// Captures direct parameter values after the first render.
        /// </summary>
        /// <param name="firstRender">A value indicating whether this is the first render.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            if (firstRender)
            {
                // Added direct parameters from DirectParamKeys
                foreach (string key in DirectParamKeys)
                {
                    DirectParameters ??= [];
                    object? initValue = GetType().GetProperty(key)?.GetValue(this);
                    _ = SfBaseUtils.UpdateDictionary(key, initValue!, DirectParameters);
                }
            }
        }

        /// <exclude />
        internal async Task OnPropertyChangedAsync()
        {
            await OnParametersSetAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Processes property value changes and resolves the final value to apply.
        /// </summary>
        /// <remarks>
        /// This method coordinates direct parameter tracking, two-way binding notifications,
        /// and rendered-state checks to determine whether the public or private value should be used.
        /// </remarks>
        internal virtual async Task<T> UpdatePropertyAsync<T>(string propertyName, T publicValue, T privateValue, object? eventCallback = null, Expression<Func<T>>? expression = null)
        {
            T finalResult = publicValue;
            if (!EqualityComparer<T>.Default.Equals(publicValue, privateValue))
            {
                // Get the direct parameter value
                T directParam = DirectParameters.TryGetValue(propertyName, out object? value) ? (T)value : publicValue;
                bool hasEventDelegate = eventCallback != null && ((EventCallback<T>)eventCallback).HasDelegate;
                bool isPropertyBinding = !SfBaseUtils.Equals(publicValue, directParam) && IsRerendering;
                SfBaseComponent baseComponent = MainParent ?? this;
                bool isTwoWayBinding = IsRerendering && hasEventDelegate;

                // Validate and assign public or private values to the property based on changes
                finalResult = (isTwoWayBinding || isPropertyBinding || !baseComponent.IsRendered) ? publicValue : privateValue;

                // Checking eventcallback for two-way notification
                if (hasEventDelegate)
                {
                    EventCallback<T> eventMethod = (EventCallback<T>)eventCallback!;
                    await eventMethod.InvokeAsync(finalResult).ConfigureAwait(false);
                }

                if (isPropertyBinding)
                {
                    DirectParameters[propertyName] = finalResult!;
                    _ = SfBaseUtils.UpdateDictionary(propertyName, finalResult!, PropertyChanges);
                }
            }

            return finalResult;
        }

        /// <summary>
        /// Releases component resources and clears tracked data.
        /// </summary>
        /// <returns>A task that represents the asynchronous dispose operation.</returns>
        protected override ValueTask DisposeAsyncCore()
        {
            DirectParameters?.Clear();
            DataManager?.Dispose();
            return base.DisposeAsyncCore();
        }
    }
}
