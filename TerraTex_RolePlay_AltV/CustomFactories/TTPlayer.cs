using AltV.Net;
using AltV.Net.Async.Elements.Entities;
using AltV.Net.Elements.Entities;

namespace TerraTex_RolePlay_AltV_Server.CustomFactories;

public class TTPlayer: AsyncPlayer
{
    public bool LoggedIn { get; set; }

    public TTPlayer(ICore core, IntPtr nativePointer, ushort id) : base(core, nativePointer, id)
    {
        LoggedIn = false;
    }

    public void SendChatMessage(string msg)
    {
        Chat.Chat.Send(this, msg);
    }
}

public class TTPlayerFactory : IEntityFactory<IPlayer>
{
    public IPlayer Create(ICore core, IntPtr playerPointer, ushort id)
    {
        return new TTPlayer(core, playerPointer, id);
    }
}