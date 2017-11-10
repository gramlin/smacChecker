using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
namespace Scanner
{
    public static class Settings
    {
        private static ISettings AppSettings =>
        CrossSettings.Current;

        public static string ScanUrl
        {
            get => AppSettings.GetValueOrDefault(nameof(ScanUrl), "http://rrr.se/t3?breakout=123&text=");
            set => AppSettings.AddOrUpdateValue(nameof(ScanUrl), value);
        }

      
    }
}
