using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Shared.Enums;

namespace TerraTex_RolePlay_AltV_Server.MicroScripts;

public class MicroTeleports: IScript
{
    public List<TeleportConfig> TeleportConfigs = new ()
    {
        new (
            new Position(-1066.6813f, -2799.6133f, 27.695923f), 0,
            new Position(-1043.0513916016f, -2746.7419433594f, 20.35883140564f), 0)
    };
    
    
    public MicroTeleports()
    {
        foreach (var teleportConfig in TeleportConfigs)
        {
            // IMarker marker = Alt.CreateMarker(MarkerType.MarkerCylinder, teleportConfig.TeleportPos1, new Rgba(36, 237, 157, 255));
            // IMarker marker2 = Alt.CreateMarker(MarkerType.MarkerCylinder, teleportConfig.TeleportPos2, new Rgba(36, 237, 157, 255));
            IColShape shape1 = Alt.CreateColShapeCylinder(teleportConfig.TeleportPos1, 2f, 3f);
            IColShape shape2 = Alt.CreateColShapeCylinder(teleportConfig.TeleportPos2, 2f, 3f);
            
            Console.WriteLine($"Created Marker with Data {teleportConfig}");

            shape1.IsPlayersOnly = true;
            shape2.IsPlayersOnly = true;
            
            shape1.SetData("MicroTeleportTo", shape2);
            shape1.SetData("MicroTeleportHeading", teleportConfig.Heading1);
            shape2.SetData("MicroTeleportHeading", teleportConfig.Heading2);
            shape2.SetData("MicroTeleportTo", shape1);
        }
    }

    [ScriptEvent(ScriptEventType.ColShape)]
    public async void OnMarkerEnter(IColShape shape, IEntity wObj, bool state)
    {
        if (shape.HasData("MicroTeleportTo") && !wObj.HasData("getTeleported") && state)
        {
            IPlayer player = (IPlayer)wObj;
            shape.GetData<IColShape>("MicroTeleportTo", out var target);
            shape.GetData<float>("MicroTeleportHeading", out var heading);
            player.SetData("getTeleported", true);
            player.Position = target.Position;
            player.Rotation = new Rotation(heading,heading, heading);

            await Task.Delay(1000);
            player.DeleteData("getTeleported");
        }
    }
}

public class TeleportConfig
{
    public Position TeleportPos1 { get; }
    public float Heading1 { get; }
    public Position TeleportPos2 { get; }
    public float Heading2 { get; }

    public TeleportConfig(Position teleportPos1, float heading1 ,Position teleportPos2, float heading2)
    {
        TeleportPos1 = teleportPos1;
        Heading1 = heading1;
        TeleportPos2 = teleportPos2;
        Heading2 = heading2;
    }
}