using AttendanceRecord.Composition;

namespace AttendanceRecord;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var container = new AppContainer();
        container.Run<AppService>(app => app.Run(container));
    }
}
