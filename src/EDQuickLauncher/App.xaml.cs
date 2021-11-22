/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using CheapLoc;
using Config.Net;
using Newtonsoft.Json;
using Serilog;
using EDQuickLauncher.Game;
using EDQuickLauncher.Settings;
using EDQuickLauncher.Settings.Parsers;
using EDQuickLauncher.Windows;
using EDQuickLauncher.Config;

namespace EDQuickLauncher {

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    public const string RepoUrl = "https://github.com/thakyZ/EDQuickLauncher";

    public static LauncherSettingsV2 Settings;

#if !XL_NOAUTOUPDATE
    private UpdateLoadingDialog _updateWindow;
#endif

    private MainWindow _mainWindow;

    public static bool GlobalIsDisableAutolaunch { get; private set; }

    public App() {
      RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;

      foreach (var arg in Environment.GetCommandLineArgs()) {
        if (!arg.StartsWith("--roamingPath="))
          continue;
        Paths.RoamingPath = arg.Substring(14);
        break;
      }

      var release = $"edquicklauncher-{Util.GetAssemblyVersion()}-{Util.GetGitHash()}";

      Log.Logger = new LoggerConfiguration()
        .WriteTo.Async(a =>
         a.File(Path.Combine(Paths.RoamingPath, "output.log")))
#if DEBUG
        .WriteTo.Debug()
        .MinimumLevel.Verbose()
#else
        .MinimumLevel.Information()
#endif
        .CreateLogger();

#if !DEBUG
      AppDomain.CurrentDomain.UnhandledException += EarlyInitExceptionHandler;
      TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
#endif

      CustomMessageBox.InvokableApp = this;

      try {
        SetupSettings();
      } catch (Exception e) {
        Log.Error(e, "Settings were corrupted, resetting");
        File.Delete(GetConfigPath("launcher"));
        SetupSettings();
      }
#if !XL_LOC_FORCEFALLBACKS
      try {
        if (App.Settings.LauncherLanguage is null) {
          var currentUiLang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
          App.Settings.LauncherLanguage = App.Settings.LauncherLanguage.GetLangFromTwoLetterISO(currentUiLang);
        }

        Log.Information("Trying to set up Loc for language code {0}", App.Settings.LauncherLanguage.GetLocalizationCode());
        if (!App.Settings.LauncherLanguage.IsDefault()) {
          Loc.Setup(Util.GetFromResources(Util.GetFromResources($"EDQuickLauncher.Resources.Loc.ed.ed_{App.Settings.LauncherLanguage.GetLocalizationCode()}.json")));
        } else {
          try {
            Loc.Setup(Util.GetFromResources($"EDQuickLauncher.Resources.Loc.EDQuickLauncher_Localizable.json"));
          } catch {
            Loc.SetupWithFallbacks();
          }
        }
      } catch (Exception ex) {
        Log.Error(ex, "Could not get language information. Setting up fallbacks.");
        Loc.Setup("{}");
      }
#else
      // Force all fallbacks
      Loc.Setup ("{}");
#endif

      Log.Information(
        $"EDQuickLauncher started as {release}");

#if !XL_NOAUTOUPDATE

      if (!EnvironmentSettings.IsDisableUpdates) {
        try {
          Log.Information("Starting update check...");

          _updateWindow = new UpdateLoadingDialog();
          _updateWindow.Show();

          var updateMgr = new Updates();
          updateMgr.OnUpdateCheckFinished += OnUpdateCheckFinished;

          var result = updateMgr.Run(EnvironmentSettings.IsPreRelease);
        } catch (Exception ex) {
          MessageBox.Show(
            "EDQuickLauncher could not contact the update server. Please check your internet connection or try again.\n\n" + ex,
            "EDQuickLauncher Error", MessageBoxButton.OK, MessageBoxImage.Error);
          Environment.Exit(0);
        }
      }
#endif
    }

    private static void SetupSettings() {
      Settings = ConfigTools.Load();
    }

    private void OnUpdateCheckFinished(object sender, EventArgs e) {
      Dispatcher.Invoke(() => {
        _useFullExceptionHandler = true;

#if !XL_NOAUTOUPDATE
        if (_updateWindow != null)
          _updateWindow.Hide();
#endif

        _mainWindow = new MainWindow();
        _mainWindow.Initialize();
      });
    }

    private bool _useFullExceptionHandler = false;

    private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e) {
      if (!e.Observed)
        EarlyInitExceptionHandler(sender, new UnhandledExceptionEventArgs(e.Exception, true));
    }

    private void EarlyInitExceptionHandler(object sender, UnhandledExceptionEventArgs e) {
      Dispatcher.Invoke(() => {
        Log.Error((Exception)e.ExceptionObject, "Unhandled exception.");

        if (_useFullExceptionHandler) {
          new ErrorWindow((Exception)e.ExceptionObject, "An unhandled exception occurred.", "Unhandled")
            .ShowDialog();
        } else {
          MessageBox.Show(
            "Error during early initialization. Please report this error.\n\n" + e.ExceptionObject,
            "EDQuickLauncher Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        Log.CloseAndFlush();
        Environment.Exit(0);
      });
    }

    internal static string GetConfigPath(string prefix) => Path.Combine(Paths.RoamingPath, $"{prefix}ConfigV2.json");

    private void App_OnStartup(object sender, StartupEventArgs e) {
      var accountName = String.Empty;

      if (e.Args.Length > 0) {
        foreach (var arg in e.Args) {
          if (arg == "--noautologin") {
            GlobalIsDisableAutolaunch = true;
          }

          if (arg == "--genLocalizable") {
            try {
              Loc.ExportLocalizable();
            } catch (Exception ex) {
              MessageBox.Show(ex.ToString());
            }
            Environment.Exit(0);
            return;
          }
        }
      }

      Log.Verbose("Loading MainWindow for account '{0}'", accountName);

      if (EnvironmentSettings.IsDisableUpdates) {
        OnUpdateCheckFinished(null, null);
      }

#if XL_NOAUTOUPDATE
      OnUpdateCheckFinished(null, null);
#endif
    }
  }
}