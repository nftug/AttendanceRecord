using Photino.NET;

namespace BrowserBridge.Photino;

public class PhotinoWindowInstance
{
    private PhotinoWindow? _window;

    public void Inject(PhotinoWindow window) => _window = window;

    public PhotinoWindow? Value => _window;
}
