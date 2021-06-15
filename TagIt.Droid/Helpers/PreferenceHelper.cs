﻿using Android.App;
using Android.Content;
using Android.Preferences;
using System;

namespace TagIt.Droid.Helpers
{
    public static class PreferenceHelper
    {
        // Android
        public static ISharedPreferences Preferences => PreferenceManager.GetDefaultSharedPreferences(Application.Context);

        /*// iOS
        public static NSUserDefaults IOSPreferences => null;

        // Windows Phone
        public static IsolatedStorageSettings WindowsPreferences => null;*/

        public static void ResetToDefaults() => Preferences.Edit()
            .Clear()
            .Commit();

        public static int PredictionsThreshold => int.Parse(Preferences.GetString("predictions_timestamp_threshold", "5"));

        public static Themes DisplayTheme => ThemeHelper.ParseTheme(Preferences.GetString("display_theme", "light"));

        public static bool StayActive => Preferences.GetBoolean("stay_active", false);

        public static TimeSpan CrossoverTime => TimeSpan.FromMinutes(Preferences.GetInt("crossover_time", 0));

        public static TimeSpan SleepTime => TimeSpan.FromMinutes(Preferences.GetInt("sleep_time", 0)); //TimeSpan.ParseExact(Preferences.GetString("sleep_time", "00:00"), "hh:mm", CultureInfo.CurrentCulture);

        public static TimeSpan WakeTime => TimeSpan.FromMinutes(Preferences.GetInt("wake_time", 480)); //TimeSpan.ParseExact(Preferences.GetString("wake_time", "08:00"), "hh:mm", CultureInfo.CurrentCulture);
    }
}