using AttendanceRecord.Presentation.Composition;

namespace AttendanceRecord.Presentation;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var container = new AppContainer();
        container.Run<AppService>(app => app.Run(container));
    }
}
