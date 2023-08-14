using System.Numerics;
using AltV.Net.Data;

namespace TerraTex_RolePlay_AltV_Server.Lib.Helper;

public static class PositionHelper
{
    public static Vector3 ToVector3(this Position pos)
    {
        return new Vector3(pos.X, pos.Y, pos.Z);
    }

    public static Position ToPosition(this Vector3 pos)
    {
        return new Position(pos.X, pos.Y, pos.Z);
    }
}