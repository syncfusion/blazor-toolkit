namespace Blazor.Toolkit.Samples.Client.Services
{
    // Minimal sample helper service used by samples to indicate device mode.
    // This can be expanded to perform JS-based detection if needed.
    public class SampleService
    {
        // When true, samples may use mobile-friendly values/layouts.
        public bool IsDevice { get; set; }

        public SampleService()
        {
            IsDevice = false; // default to desktop; update if you add detection logic
        }
    }
}