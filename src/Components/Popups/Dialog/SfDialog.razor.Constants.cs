namespace Syncfusion.Blazor.Toolkit.Popups
{
    public partial class SfDialog : SfBaseComponent
    {
        #region Common string constants
        private const string X = "X";
        private const string Y = "Y";
        private const string ARIA_LABEL = "aria-label";
        private const string DRAG = "Drag";
        private const string BODY = "body";
        private const string DELAY = "delay";
        private const string CLASS = "class";
        private const string STYLE = "style";
        private const string TITLE = "title";
        private const string MODAL = "modal";
        private const string CLOSE = "Close";
        private const string WIDTH = "Width";
        private const string CENTER = "center";
        private const string DIALOG = "dialog";
        private const string HEIGHT = "Height";
        private const string ZINDEX = "ZIndex";
        private const string TARGET = "Target";
        private const string ESCAPE = "Escape";
        private const string DATA_ID = "dataId";
        private const string Z_INDEX = "z-index";
        private const string ISMODAL = "IsModal";
        private const string ELEMENT = "element";
        private const string DURATION = "duration";
        private const string CSSCLASS = "CssClass";
        private const string POSITION = "position";
        private const string DRAG_STOP = "DragStop";
        private const string IS_INITIAL = "isInitial";
        private const string MIN_HEIGHT = "MinHeight";
        private const string MAX_HEIGHT = "maxHeight";
        private const string DRAG_START = "DragStart";
        private const string ENABLE_RTL = "EnableRtl";
        private const string DOT_NET_REF = "dotNetRef";
        private const string ANIMATE_EFFECT = "effect";
        private const string SOUTH_WEST = "south-west";
        private const string SOUTH_EAST = "south-east";
        private const string NORTH_WEST = "north-west";
        private const string NORTH_EAST = "north-east";
        private const string ALL_DIRECTIONS = "south north east west north-east north-west south-east south-west";
        private const string SPACE = " ";
        private const string CLOSE_ICON = "Close Icon";
        private const string FULL_SCREEN = "fullScreen";
        private const string USER_ACTION = "User Action";
        private const string DIALOG_CLOSE = "Close";
        private const string ENABLE_RESIZE = "EnableResize";
        private const string ALLOW_DRAGGING = "AllowDragging";
        private const string CLOSE_ON_ESCAPE = "CloseOnEscape";
        private const string ALLOWMAXHEIGHT = "allowMaxHeight";
        private const string PREVENT_VISIBILITY = "preventVisibility";
        private const string ANIMATION_SETTINGS = "animationSettings";
        private const string RESIZE_ICON_DIRECTION = "resizeIconDirection";
        private const string OPENED_ENABLED = "openedEnabled";
        private const string CLOSED_ENABLED = "closedEnabled";
        private const string ON_DRAG_ENABLED = "onDragEnabled";
        private const string RESIZING_ENABLED = "resizingEnabled";
        private const string ON_DRAG_STOP_ENABLED = "onDragStopEnabled";
        private const string ON_DRAG_START_ENABLED = "onDragStartEnabled";
        private const string ON_RESIZE_STOP_ENABLED = "onResizeStopEnabled";
        private const string ON_RESIZE_START_ENABLED = "onResizeStartEnabled";
        #endregion

        #region Class constants
        private const string RTL = "e-rtl";
        private const string POPUP = "e-popup";
        private const string HEADER = "e-dlg-header";
        private const string OVERLAY = "e-dlg-overlay";
        private const string CONTENT = "e-dlg-content";
        private const string DIALOG_MODAL = "e-dlg-modal";
        private const string POPUP_CLOSE = "e-popup-close";
        private const string RESIZABLE = "e-dlg-resizable";
        private const string CONTAINER = "e-dlg-container";
        private const string FOOTER_CONTENT = "e-footer-content";
        private const string HEADERCONTENT = "e-dlg-header-content";
        private const string BTNICONCSS = "e-close e-toolkit-icons";
        private const string BTNCSSCLASS = "e-dlg-closeicon-btn e-flat";
        #endregion

        #region Dictionary string constants
        private const string DICTIONARY_TARGET = "target";
        private const string DICTIONARY_WIDTH = "width";
        private const string DICTIONARY_HEIGHT = "height";
        private const string DICTIONARY_ZINDEX = "zIndex";
        private const string DICTIONARY_IS_MODAL = "isModal";
        private const string DICTIONARY_VISIBLE = "visible";
        private const string DICTIONARY_CSSCLASS = "cssClass";
        private const string DICTIONARY_ENABLE_RTL = "enableRtl";
        private const string DICTIONARY_MIN_HEIGHT = "minHeight";
        private const string DICTIONARY_ENABLE_RESIZE = "enableResize";
        private const string DICTIONARY_ENABLE_PERSISTENCE = "enablePersistence";
        private const string DICTIONARY_ALLOW_DRAGGING = "allowDragging";
        private const string DICTIONARY_CLOSE_ON_ESCAPE = "closeOnEscape";
        private const string DICTIONARY_ALLOW_PRERENDER = "allowPrerender";
        #endregion

        #region JS invoke method string constants
        private const string JS_SHOW = "show";
        private const string JS_HIDE = "hide";
        private const string JS_DESTROY = "destroy";
        private const string JS_INITIALIZE = "initialize";
        private const string JS_GET_MAX_HEIGHT = "getMaxHeight";
        private const string JS_FOCUS_CONTENT = "focusContent";
        private const string JS_GET_CLASS_LIST = "getClassList";
        private const string JS_POPUP_CLOSE = "popupCloseHandler";
        private const string JS_PROPERTY_CHANGED = "propertyChanged";
        private const string JS_REFRESH_POSITION = "refreshPosition";
        private const string JS_GET_DIMENSION = "getDimension";
        private const string JS_WINDOW_LOCAL_STORAGE_GET_ITEM = "getLocalStorageItem";
        #endregion

        #region Event name string constants
        private const string OPENED = "Opened";
        private const string CLOSED = "Closed";
        private const string CREATED = "Created";
        private const string DESTROYED = "Destroyed";
        #endregion
    }
}