using AltV.Net;
using AltV.Net.Enums;
using Quartz;
using TerraTex_RolePlay_AltV_Server.CustomFactories;
using TerraTex_RolePlay_AltV_Server.Lib.Helper;
using TerraTex_RolePlay_AltV_Server.Tasks;

namespace TerraTex_RolePlay_AltV_Server.Lib.Environment.Weather;

public class Weather : IScript
{
    private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    private WeatherDefinition _currentWeather;
    private WeatherDefinition _nextWeather;

    public bool IsWinter { get; private set; } = false;

    private readonly List<WeatherDefinition> _weatherList = new List<WeatherDefinition>();
    
 
    private readonly List<WeatherDefinition> _weatherDefinitions = new List<WeatherDefinition>
    {
        // id, idWinter, Factor, FactorWinter, Name, NameWinter
        new WeatherDefinition(WeatherType.ExtraSunny, WeatherType.ExtraSunny, 10, 5, "Sonnig", "Sonnig"),
        new WeatherDefinition(WeatherType.Clear, WeatherType.Clear, 10, 5, "Klarer Himmel", "Klarer Himmel"),
        new WeatherDefinition(WeatherType.Clouds, WeatherType.Clouds, 10, 5, "Bewölkt", "Bewölkt"),
        new WeatherDefinition(WeatherType.Smog, WeatherType.Smog, 5, 7, "Dunst", "Dunst"),
        new WeatherDefinition(WeatherType.Foggy, WeatherType.Foggy, 5, 7, "Nebel", "Nebel"),
        new WeatherDefinition(WeatherType.Overcast, WeatherType.Overcast, 5, 8, "Stark Bewölkt", "Stark Bewölkt"),
        new WeatherDefinition(WeatherType.Rain,WeatherType.Snowlight, 2, 10, "Regen", "Schneefall"),
        new WeatherDefinition(WeatherType.Thunder, WeatherType.Blizzard, 2, 10, "Gewitter", "Blizzard"),
        new WeatherDefinition(WeatherType.Clearing, WeatherType.Snow, 2, 8, "Nieselregen", "leichter Schneefall")
        // @Info what is with xmas? & Halloween?
        // @todo: as bloom is way too high in winter and evening -> have to disable at nighttime?
    };

    public Weather()
    {
        if (DateTime.Now.Month == 1 || DateTime.Now.Month == 2 || DateTime.Now.Month == 12)
        {
            IsWinter = true;
        }

        foreach (WeatherDefinition weather in _weatherDefinitions)
        {
            int factor = IsWinter ? weather.FactorWinter : weather.Factor;

            for (int i = 0; i < factor; i++)
            {
                _weatherList.Add(weather);
            }
        }
        
        _weatherList.Shuffle(10);

        _currentWeather = _weatherDefinitions[0];
        _nextWeather = _weatherDefinitions[0];
    }

    public static async void Init()
    {
        IJobDetail job = JobBuilder.Create<RestartChecker>()
            .WithIdentity("weather_update", "weather_time")
            .Build();

        // Trigger the job to run now, and then repeat every 10 seconds
        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("weather_update_trigger", "weather_time")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(20)
                .RepeatForever())
            .Build();

        await Globals.Scheduler!.ScheduleJob(job, trigger);
    }

    public Task Execute(IJobExecutionContext context)
    {
        Alt.EmitAllClients("weather:transition",
            (IsWinter ? _nextWeather.WinterId : _nextWeather.Id).ToString(), 
            (IsWinter ? _currentWeather.WinterId : _currentWeather.Id).ToString(), 
            DateTime.Now.ToString("O")
        );
        _currentWeather = _nextWeather;

        // Calculate Next Weather
        Random rn = new Random();
        int wrn = rn.Next(0, _weatherList.Count - 1);
        _weatherList.Shuffle();
        _nextWeather = _weatherList[wrn];

        Logger.Info($"Set Current Weather {_currentWeather} - Next Weather will be {_nextWeather}");

        return Task.CompletedTask;
    }

    [ScriptEvent(ScriptEventType.PlayerConnect)]
    public void LoadTimeAndWeatherOnPlayerConnect(TTPlayer player, string reason)
    {
        player.Emit("weather:set", (IsWinter ? _currentWeather.WinterId : _currentWeather.Id).ToString(), DateTime.Now.ToString("O"));
    }
}