// ReSharper disable once CheckNamespace

namespace Snoop.Core;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class SettingsHelper
{
    public const string SNOOPINSTALLPATHENVVAR = "SNOOP_INSTALL_PATH";
    public const string SNOOPSETTINGSPATHENVVAR = "SNOOP_SETTINGS_PATH";
    public const string SNOOPSETTINGSDIRECTORYNAME = ".Snoop";
    public const string DEFAULTAPPLICATIONSETTINGSFILENAME = "DefaultSettings.xml";

    public static string GetSettingsRootPath()
    {
        return GetPotentialSettingsPaths().First();
    }

    public static string GetSettingsFileForSnoop()
    {
        const string file = "SnoopSettings.xml";
        return FindExistingSettingsFile(file)
               ?? Path.Combine(GetPotentialSettingsPaths().First(), file);
    }

    public static string GetDefaultApplicationSettingsFile()
    {
        return FindExistingSettingsFile(DEFAULTAPPLICATIONSETTINGSFILENAME)
               ?? Path.Combine(GetPotentialSettingsPaths().First(), DEFAULTAPPLICATIONSETTINGSFILENAME);
    }

    public static string GetApplicationSpecificSettingsFile()
    {
        var file = $"AppSettings\\{EnvironmentEx.CurrentProcessName}.xml";
        return FindExistingSettingsFile(file)
               ?? Path.Combine(GetPotentialSettingsPaths().First(), file);
    }

    public static string GetSettingsFileForCurrentApplication()
    {
        // Try to find application specific settings
        var settingsFile = GetApplicationSpecificSettingsFile();

        if (!string.IsNullOrEmpty(settingsFile) && File.Exists(settingsFile))
        {
            return settingsFile;
        }

        // Use default settings
        return GetDefaultApplicationSettingsFile();
    }

    public static string? FindExistingSettingsFile(string file)
    {
        foreach (var path in GetPotentialSettingsPaths())
        {
            var filePath = Path.Combine(path, file);

            if (File.Exists(filePath))
            {
                return filePath;
            }
        }

        return null;
    }

    public static IEnumerable<string> GetPotentialSettingsPaths()
    {
        // 1. from environment variable
        var path = Environment.GetEnvironmentVariable(SNOOPSETTINGSPATHENVVAR);

        if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
        {
            yield return path;
        }

        // 2. from ".Snoop" folders
        var startPath = EnvironmentEx.CurrentProcessPath;
        var currentPath = startPath;

        if (!string.IsNullOrEmpty(currentPath))
        {
            while (true)
            {
                path = Path.Combine(currentPath, SNOOPSETTINGSDIRECTORYNAME);

                if (Directory.Exists(path))
                {
                    yield return path;
                }

                currentPath = Path.GetDirectoryName(currentPath);

                if (string.IsNullOrEmpty(currentPath))
                {
                    break;
                }
            }
        }

        // 3. Snoop install directory
        var snoopInstallPath = Environment.GetEnvironmentVariable(SNOOPINSTALLPATHENVVAR);
        if (!string.IsNullOrEmpty(snoopInstallPath))
        {
            path = Path.Combine(snoopInstallPath, SNOOPSETTINGSDIRECTORYNAME);

            if (Directory.Exists(path))
            {
                yield return path;
            }
        }

        // 4. AppData
        path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snoop");

        // Don't check for existence here
        yield return path;
    }
}