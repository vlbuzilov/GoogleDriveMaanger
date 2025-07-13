using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GoogleDriveManager.Utils
{
public static class AppSettingsHelper
{
    private static IConfigurationRoot _configuration;

    static AppSettingsHelper()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

        public static string GetAppSetting(string key) => _configuration[key];

        public static string GetAppSetting(string section, string key) => _configuration.GetSection(section)[key];
    }

}