using Config.Net;
using EDQuickLauncher.Config;
using EDQuickLauncher.Settings.Parsers;
using System;
using System.Collections.Generic;

namespace EDQuickLauncher.Settings {
  internal static class MigrateConfigHandler {
    public static int CurrentVersion {
      get {
        return 2;
      }
    }

    public static Type CurrentType {
      get {
        switch (CurrentVersion) {
          case 1:
            return typeof(ILauncherSettingsV1);
          case 2:
            return typeof(LauncherSettingsV2);
        }
        return typeof(LauncherSettingsV2);
      }
    }

    public static object MigrateConfig(object oldConfig) {
      if (oldConfig.GetType().ToString().Contains("Castle.Proxies.ILauncherSettingsV1Proxy")) {
        if (CurrentVersion >= 2) {
          object _newConfig = HandleMigrateConfig(oldConfig, typeof(ILauncherSettingsV1));
          ConfigTools.Save(_newConfig, CurrentType);
          return _newConfig;
        }
      }
      /*if (oldConfig == typeof(LauncherSettingsV2)) {
        if (currentVersion >= 3) {
          HandleMigrateConfig(oldConfig, typeof(LauncherSettingsV2), out newConfig);
          return true;
        }
      }*/
      object newConfig = CreateDefaultConfig();
      ConfigTools.Save(newConfig, CurrentType);
      return newConfig;
    }

    public static object CreateDefaultConfig() {
      switch (CurrentVersion) {
        case 1:
          var newConfig = new ConfigurationBuilder<ILauncherSettingsV1>()
             .UseCommandLineArgs()
             .UseJsonFile(App.GetConfigPath("launcher"))
             .UseTypeParser(new DirectoryInfoParser())
             .UseTypeParser(new AddonListParser())
             .Build();

          if (String.IsNullOrEmpty(newConfig.AcceptLanguage)) {
            newConfig.AcceptLanguage = Util.GenerateAcceptLanguage();
          }
          return newConfig;
        case 2:
          var _newConfig = new LauncherSettingsV2();
          if (String.IsNullOrEmpty(_newConfig.AcceptLanguage)) {
            _newConfig.AcceptLanguage = Util.GenerateAcceptLanguage();
          }
          return _newConfig;
      }
      return null;
    }

    public static object HandleMigrateConfig(object config, Type configType) {
      // Pulled from https://stackoverflow.com/a/4478535/1112800
      var @switch = new Dictionary<Type, Func<object>> {
        { typeof(ILauncherSettingsV1), () => ConvertFromV1((ILauncherSettingsV1)config)},
        { typeof(LauncherSettingsV2), () => throw new NotImplementedException() }
      };
      return @switch[configType]();
    }

    public static LauncherSettingsV2 ConvertFromV1(ILauncherSettingsV1 oldConfig) {
      var _tempLauncherSettings = new LauncherSettingsV2 {
        GamePath = oldConfig.GamePath,
        AutolaunchEnabled = oldConfig.AutolaunchEnabled,
        AddonList = oldConfig.AddonList,
        AdditionalLaunchArgs = oldConfig.AdditionalLaunchArgs,
        LauncherLanguage = oldConfig.LauncherLanguage,
        HasComplainedAboutAdmin = oldConfig.HasComplainedAboutAdmin,
        LastVersion = oldConfig.LastVersion,
        HasShownAutoLaunchDisclaimer = oldConfig.HasShownAutoLaunchDisclaimer,
        AcceptLanguage = oldConfig.AcceptLanguage,
        EnableVR = oldConfig.EnableVR,
        ExpansionVersion = oldConfig.ExpansionVersion
      };
      return _tempLauncherSettings;
    }
  }

  internal interface ILauncherSettingsV1Proxy {
  }
}
