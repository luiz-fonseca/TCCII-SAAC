﻿using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        const string UserIdKey = "userid";
        static readonly string UserIdDefault = string.Empty;


        public static string IdUser
        {
            get { return AppSettings.GetValueOrDefault(UserIdKey, UserIdDefault); }
            set { AppSettings.AddOrUpdateValue(UserIdKey, value); }
        }

        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(IdUser);

        //publicidade android
        const string AdMobAndroidIdKey = "admobandroidid";
        static readonly string AdMobAndroidIdDefault = "ca-app-pub-3940256099942544/6300978111";


        public static string IdAdMobAndroid
        {
            get { return AppSettings.GetValueOrDefault(AdMobAndroidIdKey, AdMobAndroidIdDefault); }
            set { AppSettings.AddOrUpdateValue(AdMobAndroidIdKey, value); }
        }

    }
}
