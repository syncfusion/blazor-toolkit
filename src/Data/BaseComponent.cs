using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using System.Collections;
using Microsoft.JSInterop;
using System.ComponentModel;
using Syncfusion.Blazor.Toolkit.Data;
using System.Linq.Expressions;
using Syncfusion.Blazor.Toolkit.Internal;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
using System.Security.Cryptography;
using Syncfusion.Blazor.Toolkit.Charts.Internal;

namespace Syncfusion.Blazor.Toolkit
{
    /// <summary>
    /// A Base Component for all the Syncfusion Blazor UI components.
    /// </summary>
    public abstract class BaseComponent : ComponentBase
    {
        #region properties

        /// <summary>
        /// Gets or sets the unique identifier for the instance.
        /// </summary>
        protected int _uniqueId { get; set; }
        private readonly List<string> _directParamKeys = [];

        //This property is used for InvokeMethod json serializer option.
        private readonly JsonSerializerOptions _invokeMethodJsonSettings = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        //This property is used for Trigger method json serializer option.
        private readonly JsonSerializerOptions _triggerJsonSettings = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {
                                new DateTimeZoneHandlingConverter()
                            }
        };

        //This property is used for serialiazeBindableProp method json serializer option.
        private readonly JsonSerializerOptions _serialiazeBindablePropJsonSettings = new()
        {
            WriteIndented = true,
            Converters = {
                    new DateTimeZoneHandlingConverter(),
                    new NonFlagStringEnumConverter()
                }
        };

        //This property is used for Insert method json serializer option
        private readonly JsonSerializerOptions _insert1JsonSettings = new()
        {
            Converters = {
                                new DateTimeZoneHandlingConverter()
                        }
        };

        //This property is used for Insert method json serializer option
        private readonly JsonSerializerOptions _insert2JsonSettings = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        //This property is used for Update method for update record json serializer option
        private readonly JsonSerializerOptions _update1JsonSettings = new()
        {
            Converters = {
                                new DateTimeZoneHandlingConverter()
                            }
        };

        //This property is used for Update method for update result json serializer option
        private readonly JsonSerializerOptions _update2JsonSettings = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        //This property is used for Remove method json serializer option
        private readonly JsonSerializerOptions _removeJsonSettings = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        //This property is used for BatchUpdate method json serializer option
        private readonly JsonSerializerOptions _batchUpdateJsonSettings = new()
        {
            Converters = {
                                new DateTimeZoneHandlingConverter()
                            }
        };

        //This property is used for BatchUpdate method save action json serializer option
        private readonly JsonSerializerOptions _batchUpdateSaveJsonSettings = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        #endregion

        #region protected properties

        /// <inheritdoc/>
        [Inject]
        protected IJSRuntime? JsRuntime { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        [CascadingParameter]
        protected EditContext? EditContext { get; set; }

        /// <inheritdoc/>
        protected virtual string? NameSpace { get; set; }

        /// <inheritdoc/>
        protected virtual string? JsProperty { get; set; }

        /// <inheritdoc/>
        protected virtual BaseComponent? MainParent { get; set; }

        /// <inheritdoc/>
        protected virtual JSInteropAdaptor CreateJsAdaptor()
        {
            return default!;
        }

        /// <inheritdoc/>
        protected DotNetObjectReference<object>? DotNetObjectRef { get; set; }
        #endregion

        #region internal properties
        internal bool IsDataBound { get; set; }

        internal bool IsRerendering { get; set; }

        internal bool IsClientChanges { get; set; }

        internal bool IsServerRendered { get; set; }

        internal bool IsEventTriggered { get; set; }

        internal bool IsPropertyChanged { get; set; }

        internal bool IsAutoInitialized { get; set; }

        internal bool IsObservableCollectionChanged { get; set; }

        internal Dictionary<string, object>? HtmlAttributes { get; set; }

        internal List<object> InvokedEvents { get; set; } = [];

        internal List<string> ObservableChangedList { get; set; } = [];

        internal Dictionary<string, object> _observableData = [];
        internal Dictionary<string, EventData> _delegateList = [];
        internal Dictionary<string, object> _childDotNetObjectRef = [];

        internal Dictionary<string, object> ClientChanges { get; set; } = [];

        internal Dictionary<string, object> DirectParameters { get; set; } = [];

        internal Dictionary<string, object> BindableProperties { get; set; } = [];

        [JsonIgnore]
        internal JSInteropAdaptor? JsAdaptor { get; set; }

        [Inject]
        [JsonIgnore]
        internal SyncfusionBlazorToolkitService? SyncfusionService { get; set; }

        [JsonIgnore]
        internal Dictionary<string, DataManager> DataManagerContainer { get; set; } = [];
        #endregion

        #region public properties

        /// <exclude/>
        public virtual string? ID { get; set; }

        /// <exclude/>
        [JsonIgnore]
        public bool IsRendered { get; set; }

        /// <exclude/>
        [JsonIgnore]
        public virtual Type? ModelType { get; set; }

        /// <exclude/>
        [JsonIgnore]
        public DataManager? DataManager { get; set; }

        /// <exclude/>
        [JsonIgnore]
        public bool TemplateClientChanges { get; set; }

        /// <exclude/>
        [JsonPropertyName("guid")]
        public int UniqueId
        {
            get
            {
                if (_uniqueId == 0)
                {
                    _uniqueId = RandomNumberGenerator.GetInt32(1, 100000);
                    return _uniqueId;
                }
                else
                {
                    return _uniqueId;
                }
            }
        }

        /// <exclude/>
        [JsonIgnore]
        public virtual Dictionary<string, object> DataContainer { get; set; } = [];

        /// <exclude/>
        [JsonIgnore]
        public virtual Dictionary<string, object> DataHashTable { get; set; } = [];
        #endregion

        #region life cycle methods

        /// <inheritdoc/>
        protected override async Task OnInitializedAsync()
        {
            JsAdaptor = CreateJsAdaptor();
            JsAdaptor?.Init();
            await base.OnInitializedAsync().ConfigureAwait(false);
            IsRerendering = false;
        }

        /// <inheritdoc/>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender).ConfigureAwait(false);
            if (firstRender)
            {
                Dictionary<string, object>? tempDictionary = [];
                foreach (string key in _directParamKeys)
                {
                    object? initValue = GetType().GetProperty(key)?.GetValue(this);
                    UpdateDictionary(key, initValue!, tempDictionary);
                }

                DirectParameters = tempDictionary.ToDictionary(prop => prop.Key, prop => prop.Value);
                tempDictionary = null;
            }

            if (NameSpace != null && !IsServerRendered)
            {
                if (firstRender && !IsClientChanges)
                {
                    await InitComponentAsync().ConfigureAwait(false);
                }
                else
                {
                    await DataBindAsync().ConfigureAwait(false);
                }
            }

            IsRerendering = true;
            IsObservableCollectionChanged = false;
            ObservableChangedList = [];
        }
        #endregion

        internal async Task InitComponentAsync()
        {
            // The below condition avoid to reinitialize the already rendered component through ResourceManager.
            // Some components may used StateHasChange at OnAfterRenderAsync which cause reinitializing the component again even after rendering firstRender.
            // This is applicable only for dynamic script loading
            await OnInitRenderAsync().ConfigureAwait(false);
        }

        internal virtual async Task InitialRenderedAsync()
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task OnInitRenderAsync()
        {
            if (SyncfusionService!.IsFirstBaseResource)
            {
                SyncfusionService.IsFirstBaseResource = false;
            }

            DotNetObjectRef = DotNetObjectReference.Create<object>(this);
            string bindableProps = SerialiazeBindableProp(BindableProperties);
            string key = NameSpace + ".dataSource";
            if (DataContainer.TryGetValue(key, out object? value) && value != null)
            {
                SetDataHashTable((IEnumerable)DataContainer[key]);
            }

            _ = await SyncfusionInterop.InitAsync<string>(JsRuntime!, ID!, GetUpdateModel(true), GetEventList(), NameSpace!, DotNetObjectRef, bindableProps, HtmlAttributes!, _childDotNetObjectRef, JsAdaptor?.GetRef()!).ConfigureAwait(false);
            IsRendered = true;

            // set initial property changes in component create event
            if (_delegateList.ContainsKey("created") && BindableProperties.Count > 0)
            {
                await DataBindAsync().ConfigureAwait(false);
            }

            BindableProperties.Clear();
            TemplateClientChanges = false;
            IsRerendering = true;
            IsObservableCollectionChanged = false;
            ObservableChangedList = [];
            await InitialRenderedAsync().ConfigureAwait(false);
        }

        /// <exclude/>
        public override Task SetParametersAsync(ParameterView parameters)
        {
            // parameters.SetParameterProperties(this);
            if (DirectParameters.Count == 0)
            {
                foreach (ParameterValue parameter in parameters)
                {
                    if (!parameter.Cascading)
                    {
                        _directParamKeys.Add(parameter.Name);
                    }
                }
            }

            return base.SetParametersAsync(parameters);
        }

        internal virtual void ComponentDispose()
        {
        }

        internal void CommonDispose()
        {
            EditContext = null;
            DataManager?.Dispose();
            MainParent = null;
            BindableProperties.Clear();
            ClientChanges?.Clear();
            InvokedEvents.Clear();
            _directParamKeys.Clear();
            DirectParameters.Clear();
            _observableData.Clear();
            _delegateList.Clear();
            DataContainer.Clear();
            DataManagerContainer.Clear();
            DataHashTable.Clear();
            ObservableChangedList.Clear();
            HtmlAttributes?.Clear();
            _childDotNetObjectRef.Clear();
            UnWireObservableEvents();
            DotNetObjectRef?.Dispose();
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            CommonDispose();
            ComponentDispose();
            if (NameSpace != null && IsRendered)
            {
                ValueTask<object> valueTask = SyncfusionInterop.InvokeMethodAsync<object>(JsRuntime!, ID!, "destroy", null!, null!, NameSpace);
            }

            JsAdaptor?.Dispose();
            JsAdaptor = null!;
        }

        /// <inheritdoc/>
        public async void Refresh()
        {
            if (NameSpace != null && IsRendered)
            {
                _ = await SyncfusionInterop.InvokeMethodAsync<object>(JsRuntime!, ID!, "refresh", null!, null!, NameSpace).ConfigureAwait(false);
            }
        }

        /// <exclude/>
        public async Task DataBindAsync(bool hasStateChanged = false)
        {
            _ = hasStateChanged;
            IsDataBound = false;
            IsEventTriggered = false;
            if (ClientChanges.Count > 0)
            {
                await OnClientChangedAsync(ClientChanges).ConfigureAwait(false);
            }

            ClearClientChanges();
            if (IsRendered && NameSpace != null && BindableProperties.Count > 0)
            {
                string bindableProps = SerialiazeBindableProp(BindableProperties);
                BindableProperties.Clear();
                _ = await SyncfusionInterop.UpdateAsync<object>(JsRuntime!, ID!, bindableProps, NameSpace).ConfigureAwait(false);
            }
            else
            {
                BindableProperties.Clear();
            }

            IsPropertyChanged = false;
            IsDataBound = true;
            InvokedEvents = [];
        }

        /// <inheritdoc/>
        protected void ClearClientChanges(bool clearBindables = false)
        {
            if ((IsClientChanges && !IsDataBound) || clearBindables)
            {
                foreach (KeyValuePair<string, object> property in ClientChanges)
                {
                    _ = BindableProperties.Remove(property.Key);
                }

                if (!clearBindables)
                {
                    IsClientChanges = false;
                    ClientChanges.Clear();
                }
            }
        }

        internal static void UpdateDictionary(string key, object value, Dictionary<string, object> dictionary)
        {
            if (!dictionary.TryAdd(key, value))
            {
                dictionary[key] = value;
            }
        }

        internal void SetDataHashTable(IEnumerable DataSource)
        {
            foreach (object? Data in DataSource)
            {
                Guid guid = Guid.NewGuid();
                DataHashTable.Add("BlazTempId_" + guid.ToString(), Data);
            }
        }

        internal static string ConvertJsonString(object jsonElement)
        {
            string? jsonString = jsonElement.ToString();
            if (!jsonString!.Contains(',', StringComparison.Ordinal))
            {
                jsonString = jsonString.Replace("\\", string.Empty, StringComparison.Ordinal);
            }

            if (jsonString.IndexOf("\"[", StringComparison.Ordinal) == jsonString.IndexOf("\"[\\", StringComparison.Ordinal) && jsonString.IndexOf("\"{", StringComparison.Ordinal) == jsonString.IndexOf("\"{\\", StringComparison.Ordinal))
            {
                return jsonString;
            }
            if ((jsonString.IndexOf("\"[", StringComparison.Ordinal) != jsonString.IndexOf("\"[\\", StringComparison.Ordinal)) || (jsonString.IndexOf("\"{", StringComparison.Ordinal) != jsonString.IndexOf("\"{\\", StringComparison.Ordinal)))
            {
                if ((jsonString.LastIndexOf("\"{", StringComparison.Ordinal) > 0) && (!jsonString.Contains(',', StringComparison.Ordinal)))
                {
                    jsonString = jsonString.Replace("\"[", "[", StringComparison.Ordinal).Replace("]\"", "]", StringComparison.Ordinal)
                                .Replace("\"{", "{", StringComparison.Ordinal).Replace("}\"", "}", StringComparison.Ordinal);
                }
            }

            return jsonString;
        }

        // Invoke void return type methods
        internal async Task InvokeMethod(string methodName, string? moduleName = null, params object[]? methodParams)
        {
            _ = await IsScriptRenderedAsync().ConfigureAwait(false);
            methodParams = (methodParams != null && methodParams.Length > 0) ? methodParams : null;
            _ = await SyncfusionInterop.InvokeMethodAsync<object>(JsRuntime!, ID!, methodName, moduleName!, methodParams!, NameSpace!).ConfigureAwait(false);
        }

        // Invoke object return type methods
        internal virtual async Task<T> InvokeMethod<T>(string methodName, bool isObjectReturnType, string? moduleName = null, params object[]? methodParams)
        {
            _ = await IsScriptRenderedAsync().ConfigureAwait(false);
            methodParams = (methodParams != null && methodParams.Length > 0) ? methodParams : null;
            if (!isObjectReturnType)
            {
                return await SyncfusionInterop.InvokeMethodAsync<T>(JsRuntime!, ID!, methodName, moduleName!, methodParams!, NameSpace!).ConfigureAwait(false);
            }
            else
            {
                string ReturnValue = await SyncfusionInterop.InvokeMethodAsync<string>(JsRuntime!, ID!, methodName, moduleName!, methodParams!, NameSpace!).ConfigureAwait(false);
                return ReturnValue == null ? default! : JsonSerializer.Deserialize<T>(ReturnValue, _invokeMethodJsonSettings)!;
            }
        }

        private void ObservableCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            IsObservableCollectionChanged = true;
            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                foreach (object? item in e.OldItems)
                {
                    if (item is INotifyPropertyChanged changed)
                    {
                        changed.PropertyChanged -= ObservablePropertyChanged;
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (object? item in e.NewItems)
                {
                    if (item is INotifyPropertyChanged changed)
                    {
                        changed.PropertyChanged += ObservablePropertyChanged;
                    }
                }
            }
        }

        private void ObservablePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            IsObservableCollectionChanged = true;
        }

        /// <inheritdoc/>
        protected virtual void WireObservableEvents(object collection)
        {
            if (collection != null && collection.GetType().IsGenericType)
            {
                if (collection is INotifyCollectionChanged changed)
                {
                    changed.CollectionChanged += new NotifyCollectionChangedEventHandler(ObservableCollectionChanged);
                }

                if (collection is INotifyPropertyChanged)
                {
                    List<object> enumerableCollection = [.. (IEnumerable<object>)collection];
                    object? firstItem = enumerableCollection.FirstOrDefault();
                    if (firstItem is INotifyPropertyChanged)
                    {
                        foreach (object item in enumerableCollection)
                        {
                            ((INotifyPropertyChanged)item).PropertyChanged += new PropertyChangedEventHandler(ObservablePropertyChanged);
                        }
                    }
                }
            }
        }

        private void UnWireObservableEvents()
        {
            if (_observableData.Count > 0)
            {
                foreach (KeyValuePair<string, object> collection in _observableData)
                {
                    if (collection.Value is INotifyCollectionChanged changed)
                    {
                        changed.CollectionChanged -= ObservableCollectionChanged;
                    }

                    if (collection.Value is INotifyPropertyChanged)
                    {
                        List<object> enumerableCollection = [.. (IEnumerable<object>)collection.Value];
                        object? firstItem = enumerableCollection.FirstOrDefault();
                        if (firstItem is INotifyPropertyChanged)
                        {
                            foreach (object item in enumerableCollection)
                            {
                                ((INotifyPropertyChanged)item).PropertyChanged -= ObservablePropertyChanged;
                            }
                        }
                    }
                }
            }
        }

        internal virtual async Task<T> UpdatePropertyAsync<T>(string key, T publicValue, T privateValue, object eventCallback = null!, Expression<Func<T>> expression = null!, bool isDataSource = false, bool isObservable = false)
        {
            string? propertyKey = !JsProperty!.StartsWith("sf.", StringComparison.Ordinal) ? $"{JsProperty}.{key}" : key;
            string? dataKey = !string.IsNullOrEmpty(JsProperty) ? $"{JsProperty}.{key}" : key;
            BaseComponent baseComponent = MainParent ?? this;
            string? subString = key[1..];
            string publicKey = char.ToUpper(key[0], CultureInfo.CurrentCulture) + subString;

            T finalResult = publicValue;
            if (isDataSource || isObservable)
            {
                T directParam = GetDirectParam<T>(publicKey);
                object? dataSource = isObservable ? publicValue : GetDataManager(publicValue!, dataKey);
                if (baseComponent.DataContainer.ContainsKey(dataKey))
                {
                    if (!EqualityComparer<T>.Default.Equals(publicValue, directParam) && !IsClientChanges)
                    {
                        UpdateDictionary(propertyKey, dataSource!, baseComponent.BindableProperties);
                        HashSet<string> typeValue = new([typeof(int[,]).Name, typeof(double[,]).Name, typeof(int?[,]).Name]);
                        if (publicValue is not DefaultAdaptor && !typeValue.Contains(publicValue!.GetType().Name))
                        {
                            DataHashTable.Clear();
                            SetDataHashTable((IEnumerable<object>)publicValue);
                            baseComponent.DataContainer[dataKey] = publicValue;
                            DirectParameters[publicKey] = publicValue;
                            baseComponent.IsPropertyChanged = IsRerendering;
                        }
                    }
                    else if (privateValue != null && IsClientChanges && ClientChanges.ContainsKey(key))
                    {
                        finalResult = (T)SfBaseUtils.ChangeType(privateValue, publicValue?.GetType()!)!;
                        baseComponent.DataContainer[dataKey] = publicValue!;
                        DataHashTable.Clear();
                        SetDataHashTable((IEnumerable<object>)publicValue!);
                        baseComponent.IsPropertyChanged = IsRerendering;
                    }

                    if (IsObservableCollectionChanged && !ObservableChangedList.Contains(key))
                    {
                        ObservableChangedList.Add(key);
                        if ((publicValue as IEnumerable<object>)?.Count() != DataHashTable.Count)
                        {
                            DataHashTable.Clear();
                            SetDataHashTable((IEnumerable<object>)publicValue!);
                            baseComponent.IsPropertyChanged = IsRerendering;
                        }

                        UpdateDictionary(propertyKey, dataSource!, baseComponent.BindableProperties);
                    }
                }
                else
                {
                    baseComponent.DataContainer.Add(dataKey, publicValue!);
                    UpdateDictionary(propertyKey, dataSource!, baseComponent.BindableProperties);
                    WireObservableEvents(publicValue!);
                    if (!_observableData.ContainsKey(dataKey))
                    {
                        _observableData.Add(dataKey, publicValue!);
                    }

                    // this.ObservableData.Add(dataKey, publicValue);
                }
            }

            // check and public property value changes for setting proper value
            // !!! Don't change this logic without proper testing, this may cause problems in below scenarios,
            // data-binding using button click
            // direct property value changes from client side user interaction and then component re-rendering from eventcallback
            // one-way or two-way data-binding from c# and then component re-rendering from eventcallback
            else if (CompareValues(publicValue, privateValue))
            {
                bool forceUpdate = false;
                EventCallback<T> eventMethod;
                T directParam = GetDirectParam<T>(publicKey);

                bool isClientChanges = baseComponent.IsClientChanges;
                if (isClientChanges)
                {
                    if (IsEventTriggered)
                    {
                        // To handle clientside changes with event
                        isClientChanges = (ClientChanges.ContainsKey(dataKey) && CompareValues(publicValue, privateValue)) || !CompareValues(directParam, publicValue);
                    }
                }

                // To handle property binding using external button click
                if ((CompareValues(directParam, publicValue) || !baseComponent.IsRendered) && !isClientChanges)
                {
                    forceUpdate = true;
                    DirectParameters[publicKey] = publicValue!;
                    baseComponent.IsPropertyChanged = true;
                }
                else
                {
                    finalResult = publicValue = privateValue;
                }
                if (eventCallback != null)
                {
                    eventMethod = (EventCallback<T>)eventCallback;
                    if (eventMethod.HasDelegate && !InvokedEvents.Contains(eventMethod))
                    {
                        DirectParameters[publicKey] = publicValue!;
                        if (publicValue is not null && !publicValue.GetType().IsArray)
                        {
                            InvokedEvents.Add(eventMethod);
                        }

                        await eventMethod.InvokeAsync(publicValue).ConfigureAwait(false);
                    }
                }

                if (expression != null)
                {
                    EditContext?.NotifyFieldChanged(FieldIdentifier.Create(expression));
                }

                // Update bindable properties for c# side changes alone, since the client changes already reflected in the UI.
                if (forceUpdate && !isDataSource)
                {
                    UpdateDictionary(propertyKey, publicValue!, baseComponent.BindableProperties);
                }
            }
            return finalResult;
        }

        internal T GetDirectParam<T>(string publicKey)
        {
            // Get the previous state from direct parameters for testing with current public property
            T? directParam = DirectParameters.TryGetValue(publicKey, out object? value) ? (T)value : default;
            return directParam!;
        }

        internal static bool CompareValues<T>(T oldValue, T newValue)
        {
            Type? valueType = oldValue?.GetType();
            bool isValueCollection = valueType != null && ((valueType.Namespace != null && valueType.Namespace.Contains("Collections", StringComparison.Ordinal)) || valueType.IsArray);

            if (isValueCollection)
            {
                string oldString = JsonSerializer.Serialize(oldValue);
                string newString = JsonSerializer.Serialize(newValue);
                return !string.Equals(oldString, newString, StringComparison.Ordinal);
            }
            else
            {
                return !EqualityComparer<T>.Default.Equals(oldValue, newValue);
            }
        }

        // The below scenario is used to add a new child component, when its parent component is rerender in the page.
        // i.e. The parent is already rendered in the page and a new child will be added dynamically at the time of rerendering.
        internal void RenderNewChild()
        {
            string childString = GetSerializedModel();
            Dictionary<string, object>? childData = JsonSerializer.Deserialize<Dictionary<string, object>>(childString);
            childData?.Add("isNewComponent", true);
            foreach (KeyValuePair<string, object> property in childData!)
            {
                string jsKey = JsProperty + "." + property.Key;
                DirectParameters[property.Key] = property.Value;
                UpdateDictionary(jsKey, property.Value, MainParent?.BindableProperties!);
            }
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual async Task<object> Trigger(string eventName, string arg)
        {
            EventData data = _delegateList[eventName];

            // Deserialize the event arguments with generic type T
            object? eventArgs = JsonSerializer.Deserialize(arg, data.ArgumentType!, _triggerJsonSettings);

            // Set jsRuntime to event arguments
            if (eventArgs != null && data.ArgumentType?.Namespace != "System")
            {
                PropertyInfo? JSRunTimeProperty = data.ArgumentType?.GetProperty("JsRuntime", BindingFlags.NonPublic | BindingFlags.Instance);
                JSRunTimeProperty?.SetValue(eventArgs, JsRuntime);
            }

            IsDataBound = false;
            IsEventTriggered = true;

            // clear bindable properties from client changes
            ClearClientChanges(true);

            // Invoke the event handler method
            dynamic argumentData = eventArgs!;
            dynamic fn = data.Handler!;
            await fn.InvokeAsync(argumentData);

            // Return the event argument changes to client side
            return JsonSerializer.Serialize(argumentData);
        }

        internal static string ConvertToProperCase(string text)
        {
            string property = char.ToUpper(text[0], CultureInfo.CurrentCulture) + text[1..];
            return property;
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task UpdateModel(Dictionary<string, object> properties)
        {
            IsClientChanges = true;
            UpdateComponentModel(properties, this);
            await OnParametersSetAsync().ConfigureAwait(false);
            StateHasChanged();
        }

        internal void UpdateComponentModel(Dictionary<string, object> properties, BaseComponent parentObject)
        {
            foreach (string key in properties.Keys)
            {
                string propKey = key;
                string actualKey = key;
                int? sfIndex = null;
                if (key.Contains('-', StringComparison.Ordinal))
                {
                    string[] keyIndex = key.Split('-');
                    propKey = keyIndex[0];
                    sfIndex = Convert.ToInt32(keyIndex[1], CultureInfo.CurrentCulture);
                }

                Type parentType = parentObject.GetType();
                PropertyInfo? publicProperty = parentType.GetProperty(ConvertToProperCase(propKey));
                PropertyInfo? property = parentType.GetProperty("_" + propKey, BindingFlags.Instance | BindingFlags.NonPublic);
                if (property == null && parentType.BaseType != null && parentType.BaseType.Namespace?.Contains("Syncfusion", StringComparison.Ordinal) == true)
                {
                    property = parentType.BaseType.GetProperty("_" + propKey, BindingFlags.Instance | BindingFlags.NonPublic);
                    if (property == null && parentType.BaseType.BaseType != null && parentType.BaseType.BaseType.Namespace?.Contains("Syncfusion", StringComparison.Ordinal) == true)
                    {
                        property = parentType.BaseType.BaseType.GetProperty("_" + propKey, BindingFlags.Instance | BindingFlags.NonPublic);
                    }
                }

                Type? propertyType = property?.PropertyType;
                if (property != null)
                {
                    object? propertyValue = publicProperty?.GetValue(parentObject);
                    propertyType = propertyValue != null && Nullable.GetUnderlyingType(propertyType!) == null ? propertyValue.GetType() : propertyType;

                    // Update complex value changes
                    if (propertyValue is BaseComponent component && properties[propKey] != null && propertyType != null && !propertyType.IsPrimitive && !propertyType.IsValueType && !propertyType.IsEnum && propertyType != typeof(string))
                    {
                        UpdateComponentModel(JsonSerializer.Deserialize<Dictionary<string, object>>(properties[propKey].ToString()!)!, component);
                        // property is known to be non-null here (validated earlier)
                        property!.SetValue(parentObject, publicProperty?.GetValue(parentObject), null);
                    }

                    // Update collection value changes
                    else if (((propertyType?.Namespace != null && propertyType.Namespace.Contains("Collection", StringComparison.Ordinal)) || propertyType!.IsArray) && properties[actualKey] != null)
                    {
                        object value = propertyType.IsArray ? UpdateArrayValue(propertyType, properties[actualKey]) :
                            UpdateCollectionValue(propertyValue!, propertyType, sfIndex, properties[actualKey]);
                        property!.SetValue(parentObject, value, null);
                        if (parentObject.IsAutoInitialized)
                        {
                            publicProperty?.SetValue(parentObject, value, null);
                        }

                        BaseComponent parentComponent = parentObject.MainParent ?? parentObject;
                        UpdateDictionary(parentObject.JsProperty + "." + propKey, value!, parentComponent.ClientChanges);
                    }

                    // Update property value changes to its parent object
                    else
                    {
                        object value = SfBaseUtils.ChangeType(properties[propKey], propertyType, true)!;
                        property!.SetValue(parentObject, value, null);
                        if (parentObject.IsAutoInitialized)
                        {
                            publicProperty?.SetValue(parentObject, value, null);
                        }

                        BaseComponent parentComponent = parentObject.MainParent ?? parentObject;
                        UpdateDictionary(parentObject.JsProperty + "." + propKey, value!, parentComponent.ClientChanges);
                    }
                }
            }
        }

        internal object UpdateCollectionValue(object propertyValue, Type propertyType, int? sfIndex, object model)
        {
            object value;
            if (sfIndex == null)
            {
                value = model;
                return SfBaseUtils.ChangeType(value, propertyType, true)!;
            }

            IList list = (IList)propertyValue;
            list ??= (IList)Activator.CreateInstance(propertyType)!;
            Dictionary<string, object>? collectionValue = JsonSerializer.Deserialize<Dictionary<string, object>>(model?.ToString()!);

            // Update null value collection
            if (propertyValue == null || list.Count == 0)
            {
                value = SfBaseUtils.ChangeType("[" + model + "]", propertyType, true)!;
            }

            // Update index based collection values
            else
            {
                if (sfIndex <= list.Count - 1)
                {
                    if (collectionValue != null && collectionValue.TryGetValue("sfAction", out object? Value) && (string)Value == "pop")
                    {
                        list.RemoveAt((int)sfIndex);
                    }
                    else
                    {
                        UpdateComponentModel(collectionValue!, (BaseComponent)list[(int)sfIndex]!);
                    }

                    value = list;
                }
                else
                {
                    _ = list.Add(SfBaseUtils.ChangeType(model!, list[0]?.GetType()!, true));
                    value = list;
                }
            }

            return SfBaseUtils.ChangeType(value!, propertyType, true)!;
        }

        internal static object UpdateArrayValue(Type propertyType, object model)
        {
            return JsonSerializer.Deserialize(model?.ToString()!, propertyType)!;
        }

        internal virtual async Task OnClientChangedAsync(IDictionary<string, object> properties)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        internal string[] GetEventList()
        {
            string[] list = new string[_delegateList.Count];
            int len = 0;
            foreach (string key in _delegateList.Keys)
            {
                list.SetValue(key, len);
                len++;
            }

            return list;
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            JsonSerializerOptions settings = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = {
                    new DateTimeZoneHandlingConverter(),
                    new NonFlagStringEnumConverter()
                }
            };
            return settings;
        }

        internal string SerialiazeBindableProp(Dictionary<string, object> bindableProp)
        {
            return JsonSerializer.Serialize(bindableProp, _serialiazeBindablePropJsonSettings);
        }

        /// <inheritdoc/>
        protected virtual string GetSerializedModel()
        {
            return JsonSerializer.Serialize(this, GetType(), GetJsonSerializerOptions());
        }

        /// <inheritdoc/>
        protected virtual string GetUpdateModel(bool isInit = false)
        {
            if (isInit)
            {
                string defaultSerializedModel = GetSerializedModel();
                if (BindableProperties.Count > 0)
                {
                    Dictionary<string, object> defaultProps = JsonSerializer.Deserialize<Dictionary<string, object>>(defaultSerializedModel)!;
                    foreach (KeyValuePair<string, object> property in BindableProperties)
                    {
                        if (defaultProps.ContainsKey(property.Key))
                        {
                            defaultProps[property.Key] = property.Value;
                        }
                        else
                        {
                            defaultProps.Add(property.Key, property.Value);
                        }
                    }

                    return JsonSerializer.Serialize(defaultProps, GetJsonSerializerOptions());
                }

                return defaultSerializedModel;
            }
            else
            {
                return SerialiazeBindableProp(BindableProperties);
            }
        }

        /// <exclude/>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ErrorHandling(string message, string stack)
        {
            Console.Error.WriteLine(message + "\n" + stack);
        }
                
        /// <inheritdoc/>
        protected object GetDataManager(object dataSource, string? key = null)
        {
            if (dataSource is SfDataManager or Toolkit.DataManager)
            {
                return dataSource;
            }

            // For Blazor Adaptor
            if (dataSource != null)
            {
                Type type = dataSource.GetType();
                DataManager = typeof(IEnumerable).IsAssignableFrom(type) ^ typeof(IEnumerable<object>).IsAssignableFrom(type)
                    ? new DataManager() { Json = ((IEnumerable)dataSource).Cast<object>() }
                    : new DataManager() { Json = (IEnumerable<object>)dataSource };
            }
            else
            {
                DataManager ??= new DataManager() { Json = [] };
            }

            if (MainParent != null && dataSource != null)
            {
                if (MainParent.DataManagerContainer.ContainsKey(key!))
                {
                    MainParent.DataManagerContainer[key!] = DataManager;
                }
                else
                {
                    MainParent.DataManagerContainer.Add(key!, DataManager);
                }
            }
            DefaultAdaptor dataAdaptor = new(key!);
            return dataAdaptor;
        }

        internal static object GetObject(Dictionary<string, object> Data, Type ModelType)
        {
            // Handling Parameterless Constructor
            object? ModelInstance = Activator.CreateInstance(ModelType);

            foreach (KeyValuePair<string, object> KeyValue in Data)
            {
                bool isBlazorId = KeyValue.Key.Split('_').Length > 0 && KeyValue.Key.Split('_')[0] == "BlazTempId";
                string pascalCase = ConvertToProperCase(KeyValue.Key);
                PropertyInfo? Property = ModelType.GetProperty(KeyValue.Key);
                Property ??= ModelType.GetProperty(pascalCase);
                if (Property != null && isBlazorId.ToString() == "False")
                {
                    if (IsCollection(Property.PropertyType, KeyValue.Value))
                    {
                        string CollectionValue = JsonSerializer.Serialize(KeyValue.Value);
                        Type dataType = Property.PropertyType.GetTypeInfo();
                        if (!Property.PropertyType.IsGenericType)
                        {
                            dataType = typeof(IEnumerable<object>);
                        }

                        object? Value = JsonSerializer.Deserialize(CollectionValue, dataType);
                        Property.SetValue(ModelInstance, Value);
                    }
                    else if (IsComplexObject(Property.PropertyType, KeyValue.Value))
                    {
                        string ComplexValue = JsonSerializer.Serialize(KeyValue.Value);
                        Dictionary<string, object> Value = JsonSerializer.Deserialize<Dictionary<string, object>>(ComplexValue)!;
                        object Info = GetObject(Value, Property.PropertyType);
                        Property.SetValue(ModelInstance, Info);
                    }
                    else if (Property.CanWrite || Property.SetMethod != null)
                    {
                        Property.SetValue(ModelInstance, SfBaseUtils.ChangeType(KeyValue.Value, Property.PropertyType));
                    }
                }
                else if (isBlazorId.ToString() == "True")
                {
                    return KeyValue.Value;
                }
            }

            return ModelInstance!;
        }

        internal static bool IsCollection(Type type, object propertyValue)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(List<>)) || type.GetGenericTypeDefinition().Equals(typeof(ICollection<>)) || type.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)) || type.IsAssignableFrom(typeof(IEnumerable))) && propertyValue is JsonElement.ArrayEnumerator;
        }

        internal static bool IsComplexObject(Type type, object propertyValue)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? IsComplexObject(type.GetGenericArguments()[0].GetTypeInfo(), propertyValue)
                : propertyValue is JsonElement || (propertyValue != null && propertyValue is not string && !type.GetTypeInfo().IsPrimitive && propertyValue is not long && propertyValue is not double && propertyValue is not decimal && propertyValue is not DateTimeOffset && propertyValue is not DateTime && !type.GetTypeInfo().IsEnum && !typeof(IEnumerable).IsAssignableFrom(type));
        }

        internal async Task<bool> IsScriptRenderedAsync()
        {
            if (SyncfusionService!.IsScriptRendered)
            {
                return true;
            }
            else
            {
                await Task.Delay(10).ConfigureAwait(false);
                return await IsScriptRenderedAsync().ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Represents the event argument data.
    /// </summary>
    internal class EventData
    {
        // EventCallback handler method
        public object? Handler { get; set; }

        // Event argument type
        public Type? ArgumentType { get; set; }

        // Update event data in the instance.
        public EventData Set<T>(EventCallback<T> action, Type type)
        {
            Handler = action;
            ArgumentType = type;
            return this;
        }
    }

    /// <summary>
    /// Generates globalization details for the specific culture in JSON format.
    /// </summary>
    internal static class GlobalizeJsonGenerator
    {
        private static readonly Dictionary<int, string> _positiveCurrencyMapper = new()
        {
            { 0, "$n" },
            { 1, "n$" },
            { 2, "$ n" },
            { 3, "n $" }
        };

        private static readonly Dictionary<int, string> _negativeCurrencyMapper = new()
        {
            { 0, "($n)" },
            { 1, "-$n" },
            { 2, "$-n" },
            { 3, "$n-" },
            { 4, "(n$)" },
            { 5, "-n$" },
            { 6, "n-$" },
            { 7, "n$-" },
            { 8, "-n $" },
            { 9, "-$ n" },
            { 10, "n $-" },
            { 11, "$ n-" },
            { 12, "$ -n" },
            { 13, "n- $" },
            { 14, "($ n)" },
            { 15, "(n $)" }
        };

        private static readonly Dictionary<int, string> _positivePercentMapper = new()
        {
            { 0, "n %" },
            { 1, "n%" },
            { 2, "%n" },
            { 3, "% n" }
        };

        private static readonly Dictionary<int, string> _numberNegativePattern = new()
        {
            { 0, "(n)" },
            { 1, "-n" },
            { 2, "-n" },
            { 3, "n-" },
            { 4, "n-" }
        };

        private static readonly Dictionary<int, string> _negativePercentMapper = new()
        {
            { 0, "-n %" },
            { 1, "-n%" },
            { 2, "-%n" },
            { 3, " %-n" },
            { 4, "%n-" },
            { 5, "n-%" },
            { 6, "n%-" },
            { 7, "-% n " },
            { 8, "n %-" },
            { 9, "% n-" },
            { 10, "% -n" },
            { 11, "n- %" }
        };

        /// <summary>
        /// Returns the globalized JSON string.
        /// </summary>
        /// <param name="cultureData">Specific culture information.</param>
        /// <returns>Json serialized globalize string.</returns>
        public static string GetGlobalizeJsonString(CultureInfo cultureData)
        {
            return JsonSerializer.Serialize(GetGlobalizeContent(cultureData));
        }

        /// <summary>
        /// Returns current culture information in the Dictionary format.
        /// </summary>
        /// <param name="cultureData">Specific culture information.</param>
        /// <returns>Localized contents.</returns>
        internal static Dictionary<string, object> GetGlobalizeContent(CultureInfo cultureData)
        {
            NumberFormatInfo number = cultureData.NumberFormat;
            DateTimeFormatInfo date = cultureData.DateTimeFormat;
            string timeSeparator = date.TimeSeparator;
            Dictionary<string, object> numberJson = new()
            {
                { "mapper", ConvertStringArrayToString(cultureData.NumberFormat.NativeDigits, false, false) },
                { "mapperDigits", string.Join(string.Empty, cultureData.NumberFormat.NativeDigits) },
                { "numberSymbols", GenerateNumberSymbols(cultureData.NumberFormat, timeSeparator) },
                { "timeSeparator", timeSeparator }
            };
            string currencySymbol = number.CurrencySymbol;
            numberJson.Add("currencySymbol", currencySymbol);
            numberJson.Add("currencypData", GetPositiveCurrencyPercentData(number, false));
            numberJson.Add("percentpData", GetPositiveCurrencyPercentData(number, true));
            numberJson.Add("percentnData", GetNegativePercentData(number));
            numberJson.Add("currencynData", GetNegativeCurrencyData(number));
            numberJson.Add("decimalnData", DefaultPositiveNegativeData(number, true));
            numberJson.Add("decimalpData", DefaultPositiveNegativeData(number, false));
            Dictionary<string, object> cultureObject = new()
            {
                {
                    cultureData.Name, new Dictionary<string, object>()
                    {
                        { "numbers", numberJson },
                        { "dates", GetDateFormatOptions(date) }
                    }
                }
            };
            return cultureObject;
        }

        private static Dictionary<string, object> GetDateFormatOptions(DateTimeFormatInfo date)
        {
            Dictionary<string, object> dictionary = new()
            {
                {
                    "dayPeriods",
                    new Dictionary<string, string>()
            {
                { "am", date.AMDesignator },
                { "pm", date.PMDesignator }
            }
                },
                { "dateSeperator", date.DateSeparator },
                {
                    "days",
                    new Dictionary<string, object>()
            {
                { "abbreviated", ConvertStringArrayToString(date.AbbreviatedDayNames, true, false) },
                { "short", ConvertStringArrayToString(date.ShortestDayNames, true, false) },
                { "wide", ConvertStringArrayToString(date.DayNames, true, false) }
            }
                },
                {
                    "months",
                    new Dictionary<string, object>()
                {
                    { "abbreviated", ConvertStringArrayToString(date.AbbreviatedMonthNames, false, true) },
                    { "wide", ConvertStringArrayToString(date.MonthNames, false, true) }
                }
                },
                { "eras", EraData(date) }
            };
            return dictionary;
        }

        private static Dictionary<string, object> EraData(DateTimeFormatInfo date)
        {
            int[] eraCount = date.Calendar.Eras;
            Dictionary<string, object> dictionary = [];
            foreach (int era in eraCount)
            {
                dictionary.Add(era.ToString(CultureInfo.CurrentCulture), date.GetAbbreviatedEraName(era));
            }

            return dictionary;
        }

        private static Dictionary<string, object> DefaultPositiveNegativeData(NumberFormatInfo numberFormat, bool isNegative)
        {
            string nlead = string.Empty;
            string nend = string.Empty;
            if (isNegative)
            {
                string curMapper = _numberNegativePattern[numberFormat.NumberNegativePattern];
                string[] splitString = curMapper.Split("n");
                nlead = splitString[0];
                nend = splitString[1];
            }

            Dictionary<string, object> numberData = new()
            {
                { "nlead", nlead },
                { "nend", nend },
                { "groupData", new Dictionary<string, int>() { { "primary", numberFormat.NumberGroupSizes[0] } } },
                { "maximumFraction", numberFormat.NumberDecimalDigits },
                { "minimumFraction", numberFormat.NumberDecimalDigits }
            };
            return numberData;
        }

        private static Dictionary<string, object> GetPositiveCurrencyPercentData(NumberFormatInfo numberFormat, bool isPercent)
        {
            Dictionary<int, string> typeMapper = isPercent ? _positivePercentMapper : _positiveCurrencyMapper;
            string curMapper = typeMapper[isPercent ? numberFormat.PercentPositivePattern : numberFormat.CurrencyPositivePattern];
            string currencyString = curMapper.Replace("n", string.Empty, StringComparison.Ordinal);

            string nlead = string.Empty;
            string nend = string.Empty;
            char curSymbol = isPercent ? '%' : '$';
            if (curMapper[0].Equals(curSymbol))
            {
                nlead = currencyString.Replace(isPercent ? "%" : "$", numberFormat.CurrencySymbol, StringComparison.Ordinal);
            }
            else
            {
                nend = currencyString.Replace(isPercent ? "%" : "$", isPercent ? numberFormat.PercentSymbol : numberFormat.CurrencySymbol, StringComparison.Ordinal);
            }

            Dictionary<string, object> percentData = new() { { "nlead", nlead }, { "nend", nend } };
            if (isPercent)
            {
                AddGroupandFractionPercentData(percentData, numberFormat);
            }
            else
            {
                AddGroupandFractionCurrencyData(percentData, numberFormat);
            }

            return percentData;
        }

        private static Dictionary<string, object> GetNegativeCurrencyData(NumberFormatInfo numberFormat)
        {
            string currencyMapper = _negativeCurrencyMapper[numberFormat.CurrencyNegativePattern];
            Dictionary<string, object> currencyData = NegativePatternProcessor(currencyMapper, "$", numberFormat.CurrencySymbol);
            AddGroupandFractionCurrencyData(currencyData, numberFormat);
            return currencyData;
        }

        private static Dictionary<string, object> NegativePatternProcessor(string mapper, string currencySymbol, string replacer)
        {
            string[] splitString = mapper.Split("n");
            string nlead = splitString[0].Replace(currencySymbol, replacer, StringComparison.Ordinal);
            string nend = splitString[1].Replace(currencySymbol, replacer, StringComparison.Ordinal);
            return new Dictionary<string, object>() { { "nlead", nlead }, { "nend", nend } };
        }

        private static Dictionary<string, object> GetNegativePercentData(NumberFormatInfo numberFormat)
        {
            string currencyMapper = _negativePercentMapper[numberFormat.PercentNegativePattern];
            Dictionary<string, object> percentData = NegativePatternProcessor(currencyMapper, "%", numberFormat.PercentSymbol);
            AddGroupandFractionPercentData(percentData, numberFormat);
            return percentData;
        }

        private static void AddGroupandFractionCurrencyData(Dictionary<string, object> numberData, NumberFormatInfo numberFormat)
        {
            numberData.Add("groupSeparator", numberFormat.NumberGroupSeparator);
            numberData.Add("groupData", new Dictionary<string, int>() { { "primary", numberFormat.CurrencyGroupSizes[0] } });
            numberData.Add("maximumFraction", numberFormat.CurrencyDecimalDigits);
            numberData.Add("minimumFraction", numberFormat.CurrencyDecimalDigits);
        }

        private static void AddGroupandFractionPercentData(Dictionary<string, object> numberData, NumberFormatInfo numberFormat)
        {
            numberData.Add("groupSeparator", numberFormat.PercentGroupSeparator);
            numberData.Add("groupData", new Dictionary<string, int>() { { "primary", numberFormat.PercentGroupSizes[0] } });
            numberData.Add("maximumFraction", numberFormat.PercentDecimalDigits);
            numberData.Add("minimumFraction", numberFormat.PercentDecimalDigits);
        }

        private static Dictionary<string, string> GenerateNumberSymbols(NumberFormatInfo numberFormat, string timeSep)
        {
            Dictionary<string, string> numberSymbols = new()
            {
                { "decimal", numberFormat.NumberDecimalSeparator },
                { "group", numberFormat.NumberGroupSeparator },
                { "plusSign", numberFormat.PositiveSign },
                { "minusSign", numberFormat.NegativeSign },
                { "percentSign", numberFormat.PercentSymbol },
                { "nan", numberFormat.NaNSymbol },
                { "timeSeparator", timeSep },
                { "infinity", numberFormat.PositiveInfinitySymbol }
            };
            return numberSymbols;
        }

        // Method used to convert string[] to Dictionary.
        private static Dictionary<string, string> ConvertStringArrayToString(string[] array, bool weekMode, bool? monthMode)
        {
            // Concatenate all the elements into a StringBuilder.
            string[] calendarWeekStrings = ["sun", "mon", "tue", "wed", "thu", "fri", "sat"];
            Dictionary<string, string> digits = [];
            int i = 0;
            if (monthMode == true)
            {
                i = 1;
            }

            foreach (string item in array)
            {
                if (item.Length != 0)
                {
                    digits.Add(weekMode ? calendarWeekStrings[i] : i.ToString(CultureInfo.CurrentCulture), item);
                }

                i++;
            }

            return digits;
        }
    }

    internal static class Internationalization
    {
        /// <summary>
        /// Gets or sets current culture.
        /// </summary>
        internal static CultureInfo? CurrentCulture { get; set; }

        // Cache for storing CultureInfo instances to avoid repeated instantiation and improve performance
        private static readonly Dictionary<string, CultureInfo> _cultureCache = [];

        /// <summary>
        /// Returns the required patterns from the current culture.
        /// </summary>
        /// <param name="cultureCode">Culture code to be processed for the required patterns.</param>
        /// <returns>Returns culture patterns.</returns>
        internal static object GetCultureFormats(string cultureCode)
        {
            CultureInfo culture = GetCulture(cultureCode);
            Dictionary<string, object> cultureFormats = [];

            // Get standard date formats
            Dictionary<string, object> dateFormats = new()
            {
                ["d"] = culture.DateTimeFormat.ShortDatePattern,
                ["D"] = culture.DateTimeFormat.LongDatePattern,
                ["f"] = culture.DateTimeFormat.LongDatePattern + " " + culture.DateTimeFormat.ShortTimePattern,
                ["F"] = culture.DateTimeFormat.FullDateTimePattern,
                ["g"] = culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.ShortTimePattern,
                ["G"] = culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.LongTimePattern,
                ["m"] = culture.DateTimeFormat.MonthDayPattern,
                ["M"] = culture.DateTimeFormat.MonthDayPattern,
                ["r"] = culture.DateTimeFormat.RFC1123Pattern,
                ["R"] = culture.DateTimeFormat.RFC1123Pattern,
                ["s"] = culture.DateTimeFormat.SortableDateTimePattern,
                ["t"] = culture.DateTimeFormat.ShortTimePattern,
                ["T"] = culture.DateTimeFormat.LongTimePattern,
                ["u"] = culture.DateTimeFormat.UniversalSortableDateTimePattern,
                ["U"] = culture.DateTimeFormat.FullDateTimePattern,
                ["y"] = culture.DateTimeFormat.YearMonthPattern,
                ["Y"] = culture.DateTimeFormat.YearMonthPattern
            };
            cultureFormats[cultureCode] = dateFormats;
            return cultureFormats;
        }

        /// <summary>
        /// Returns the current culture information.
        /// </summary>
        /// <param name="culture">Optional parameter to override the current culture.</param>
        /// <returns>Returns the current culture.</returns>
        private static CultureInfo GetCulture(string? culture = null)
        {
            if (string.IsNullOrEmpty(culture))
            {
                return CultureInfo.CurrentCulture ?? new CultureInfo("en-US");
            }

            if (_cultureCache.TryGetValue(culture, out CultureInfo? cachedCulture))
            {
                return cachedCulture;
            }

            CultureInfo newCulture = new(culture);
            _cultureCache[culture] = newCulture;
            return newCulture;
        }
    }
}
