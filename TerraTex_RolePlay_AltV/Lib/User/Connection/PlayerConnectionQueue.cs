using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Types;
using System;
using System.Timers;
using AltV.Net.Elements.Entities;

namespace TerraTex_RolePlay_AltV_Server.Lib.User.Connection;

public class PlayerConnectionQueue: IScript
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    Queue<IConnectionInfo> _connectionQueue = new Queue<IConnectionInfo>();

    public PlayerConnectionQueue()
    {
       TerraTexRolePlayResource.OnStartUpFinishedEvent += OnStartUpFinishedEvent;
    }


    public void OnStartUpFinishedEvent()
    {
        var connectTimer = new System.Timers.Timer();
        connectTimer.Interval = 200;
        connectTimer.AutoReset = true;
        connectTimer.Elapsed += ConnectTimerOnElapsed;
        connectTimer.Enabled = true;
        connectTimer.Start();

        // Let's do it more dynamic in future by preloading stuff -> Next User should be loaded as soon previous is done
        
        Logger.Info("Start User Connection Queue Interval");
    }

    private void ConnectTimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        if (_connectionQueue.Count > 0)
        {
            _connectionQueue.Dequeue().Accept();
            Logger.Debug("Connect Next User");

        }
    }

    [ScriptEvent(ScriptEventType.ConnectionQueueAdd)]
    public void OnConnectionQueueAdd(IConnectionInfo connectionInfo)
    {
        Logger.Debug($"Add User to Queue: {connectionInfo.Name}");
        _connectionQueue.Enqueue(connectionInfo);
    }

    [ScriptEvent(ScriptEventType.ConnectionQueueRemove)]
    public void OnConnectionQueueRemove(IConnectionInfo connectionInfo)
    {
        Logger.Debug($"Remove User from Queue, As User canceled Queue: {connectionInfo.Name}");
        _connectionQueue = new Queue<IConnectionInfo>(_connectionQueue.Where(s => s != connectionInfo));
    }
}