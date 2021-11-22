using EDQuickLauncher.Settings;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using Serilog;
using Newtonsoft.Json.Linq;
using Config.Net;
using EDQuickLauncher.Settings.Parsers;
using System.Windows.Shell;

namespace EDQuickLauncher.Config {
  public static class ConfigTools {
    public static string ConfigPath => App.GetConfigPath("launcher");

    public static bool Save(object config, Type configType) {
      try {
        if (configType != typeof(LauncherSettingsV2) || config.GetType() != configType) {
          return false;
        }

        string json = JsonConvert.SerializeObject(config, Formatting.Indented);

        var fileInfo = new FileInfo(ConfigPath);

        if (!fileInfo.Directory.Exists) fileInfo.Create();

        string _tempPath = Path.GetTempFileName();
        byte[] data = Encoding.UTF8.GetBytes(json);
        using (FileStream tempFile = File.Create(_tempPath, 4096, FileOptions.WriteThrough)) {
          tempFile.Write(data, 0, data.Length);
        }

        string backupPath = $"{ConfigPath}.backup";
        if (File.Exists(ConfigPath)) {
          File.Replace(_tempPath, ConfigPath, backupPath);
        } else {
          File.Move(_tempPath, ConfigPath);
        }

        if (File.Exists(ConfigPath)) {
          File.Delete(backupPath);
        } else {
          File.Move(backupPath, ConfigPath);
        }
      } catch (Exception e) {
        Log.Error(e, $"Could not save settings file to {ConfigPath}. {e.Message}");
        var unused = new Windows.ErrorWindow(e, $"Failed to save settings to file.", "SettingsSaveError");
        return false;
      }
      return true;
    }

    public static LauncherSettingsV2 Load() {
      if (File.Exists(ConfigPath)) {
        string json = File.ReadAllText(ConfigPath);
        return (LauncherSettingsV2)JsonConvert.DeserializeObject(json, typeof(LauncherSettingsV2));
      }
      if (!File.Exists(ConfigPath) && File.Exists($"{ConfigPath.Substring(0, ConfigPath.Length - 6)}1.json")) {
         var _Settings = new ConfigurationBuilder<ILauncherSettingsV1>()
        .UseJsonFile($"{ConfigPath.Substring(0, ConfigPath.Length - 6)}1.json")
        .UseCommandLineArgs()
        .UseTypeParser(new DirectoryInfoParser())
        .UseTypeParser(new AddonListParser())
        .Build();

        if (String.IsNullOrEmpty(_Settings.AcceptLanguage)) {
          _Settings.AcceptLanguage = Util.GenerateAcceptLanguage();
        }

        return (LauncherSettingsV2)MigrateConfigHandler.MigrateConfig(_Settings);
      }
      return (LauncherSettingsV2)MigrateConfigHandler.CreateDefaultConfig();
    }

    public static ILauncherSettingsV1 LoadV1() {
      var _Settings = new ConfigurationBuilder<ILauncherSettingsV1>()
      .UseCommandLineArgs()
      .UseJsonFile(ConfigPath)
      .UseTypeParser(new DirectoryInfoParser())
      .UseTypeParser(new AddonListParser())
      .Build();

      if (String.IsNullOrEmpty(_Settings.AcceptLanguage)) {
        _Settings.AcceptLanguage = Util.GenerateAcceptLanguage();
      }

      return _Settings;
    }
  }
}
