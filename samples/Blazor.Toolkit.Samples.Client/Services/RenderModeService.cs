namespace Blazor.Toolkit.Samples.Client.Services
{
    public enum RenderMode
    {
        InteractiveServer,
        InteractiveWebAssembly,
        InteractiveAuto
    }

    public class RenderModeService
    {
        private RenderMode _currentMode = RenderMode.InteractiveAuto;


        public RenderMode CurrentMode => _currentMode;

        public event Action<RenderMode>? RenderModeChanged;

        public void SetMode(RenderMode mode)
        {
            if (_currentMode == mode)
                return;
            
            _currentMode = mode;
            RenderModeChanged?.Invoke(mode);
        }
    }
}
