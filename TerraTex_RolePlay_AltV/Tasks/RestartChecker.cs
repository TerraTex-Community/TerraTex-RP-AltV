using System.Timers;
using AltV.Net;

namespace TerraTex_RolePlay_AltV_Server.Tasks;

public class RestartChecker: IDisposable
{
    private System.Timers.Timer _timer;

    public RestartChecker()
    {
        _timer = new System.Timers.Timer();
        _timer.Interval = 5000;
        _timer.Elapsed += TimerOnElapsed;
        _timer.Start();
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        if (File.Exists("stop.command"))
        {
            Console.WriteLine("Found Stop Command ... Start Server Shutdown");
            Alt.Core.StopServer();
        }
    }

    public void Dispose()
    {
        _timer.Stop();
    }
}