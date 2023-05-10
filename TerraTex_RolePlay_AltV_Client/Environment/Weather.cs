using AltV.Net.Client;
using AltV.Net.Enums;
using System.Globalization;

namespace TerraTex_RolePlay_AltV_Client.Environment;

public class Weather
{
    public Weather()
    {
        Alt.SetWeatherSyncActive(false);
        Alt.OnServer<string, string>("weather:set", SetWeather);
        Alt.OnServer<string, string, string>("weather:transition", SetWeatherTransition);
    }

    public bool IsWinter { get; set; } = false;

    private void SetWeatherTransition(string weatherStr, string oldWeather, string timeStr)
    {
        bool type = Enum.TryParse<WeatherType>(weatherStr, out var weather);
        if (type)
        {
            DateTime time =
                DateTime.ParseExact(timeStr, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            SetTime(time);
            if (time.Month == 1 || time.Month == 2 || time.Month == 12)
            {
                IsWinter = true;
            }

            Alt.Natives.SetWeatherTypeOvertimePersist(weather.ToString(), 60000);
        }
    }

    private void SetWeather(string weatherStr, string timeStr)
    {
        bool type = Enum.TryParse<WeatherType>(weatherStr, out var weather);
        if (type)
        {
            Alt.LogInfo($"start set Weather {weather} & {timeStr}");
            DateTime time =
                DateTime.ParseExact(timeStr, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

            if (time.Month == 1 || time.Month == 2 || time.Month == 12)
            {
                IsWinter = true;
            }

            Alt.Natives.SetWeatherTypeNowPersist(weather.ToString());
            SetTime(time);
        }
    }

    private void SetTime(DateTime time)
    {
        Alt.LogInfo($"Set Time: {time}");
        int oldHours = Alt.Natives.GetClockHours();
        int oldMinutes = Alt.Natives.GetClockMinutes();
        int oldSeconds = Alt.Natives.GetClockSeconds();

        int oldCalcTime = oldMinutes + oldHours * 60;
        int newCalcTime = time.Hour * 60 + time.Minute;

        if (Alt.MsPerGameMinute != 60000)
        {
            Alt.MsPerGameMinute = 60000;
            Alt.Natives.SetClockDate(time.Day, time.Month, time.Year);
            Alt.Natives.SetClockTime(time.Hour, time.Minute, time.Second);
        }

        // if time diff is bigger as 1 minute
        if (Math.Abs(oldCalcTime - newCalcTime) > 1)
        {
            Alt.MsPerGameMinute = 60000;
            Alt.Natives.SetClockDate(time.Day, time.Month, time.Year);
            Alt.Natives.SetClockTime(time.Hour, time.Minute, time.Second);
        }
    }
}