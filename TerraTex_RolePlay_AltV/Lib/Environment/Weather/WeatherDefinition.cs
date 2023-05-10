using AltV.Net.Enums;

namespace TerraTex_RolePlay_AltV_Server.Lib.Environment.Weather;

public class WeatherDefinition
{
    public WeatherType Id { get; }
    public int Factor { get; }
    public int FactorWinter { get; }
    public string Description { get; }
    public string WinterDescription { get; }
    public WeatherType WinterId { get; }

    public WeatherDefinition(WeatherType id, WeatherType winterId, int factor, int factorInWinter,
        string description, string winterDescription)
    {
        Id = id;
        WinterId = winterId;
        FactorWinter = factorInWinter;
        Factor = factor;
        Description = description;
        WinterDescription = winterDescription;
    }
}