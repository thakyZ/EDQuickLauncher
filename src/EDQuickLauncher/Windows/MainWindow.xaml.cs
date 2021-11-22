/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

using CheapLoc;
using EDQuickLauncher.Addon;
using EDQuickLauncher.Game;
using EDQuickLauncher.Windows.ViewModel;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EDQuickLauncher.Windows {

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    private bool _isLaunching;
    private Headlines _headlines = new Headlines();
    public Action ReloadHeadlines;

    public MainWindow() {
      InitializeComponent();

      DataContext = new MainWindowViewModel();
      Closing += MainWindow_OnWindowClosing;

      ReloadHeadlines += () => Task.Run(SetupHeadlines);

#if !XL_NOAUTOUPDATE
      Title += " v" + Util.GetAssemblyVersion();
#else
      Title += " " + Util.GetGitHash();
#endif

#if !XL_NOAUTOUPDATE
      if (EnvironmentSettings.IsDisableUpdates)
#endif
      {
        Title += " - UNSUPPORTED VERSION - NO UPDATES - COULD DO BAD THINGS";
      }

#if DEBUG
      Title += " - Debugging";
#endif

      if (EnvironmentSettings.IsWine)
        Title += " - Wine on Linux";
    }

    public void Initialize() {
#if DEBUG
      var fakeStartMenuItem = new MenuItem {
        Header = "Fake start"
      };
      fakeStartMenuItem.Click += FakeStart_OnClick;
#endif

      ExpansionSwitcher.ItemsSource = MainWindowViewModel.ExpansionSwitcherItems;

      // Clean up invalid addons
      if (App.Settings.AddonList != null)
        App.Settings.AddonList = App.Settings.AddonList.Where(x => !String.IsNullOrEmpty(x.Addon.Path)).ToList();

      var version = Util.GetAssemblyVersion();
      if (App.Settings.LastVersion != version) {
        new ChangelogWindow().ShowDialog();

        App.Settings.LastVersion = version;
      }

      AutoLaunchCheckBox.IsChecked = App.Settings.AutolaunchEnabled;

      if (App.GlobalIsDisableAutolaunch) {
        Log.Information("Autolaunch was disabled globally, saving into settings...");
        App.Settings.AutolaunchEnabled = false;
      }

      if (App.Settings.AutolaunchEnabled && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) {
        Log.Information("Engaging Autolaunch...");

        try {
          Kickoff(true);
          return;
        } catch (Exception ex) {
          new ErrorWindow(ex, Loc.Localize("CheckLoginInfo", "Additionally, please check your login information or try again."), "AutoLogin")
            .ShowDialog();
          App.Settings.AutolaunchEnabled = false;
          _isLaunching = false;
        }
      } else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) || Boolean.Parse(Environment.GetEnvironmentVariable("XL_NOAUTOLOGIN") ?? "false")) {
        App.Settings.AutolaunchEnabled = false;
        AutoLaunchCheckBox.IsChecked = false;
      }

      if (App.Settings.GamePath?.Exists != true) {
        var setup = new FirstTimeSetup();
        setup.ShowDialog();

        // If the user didn't reach the end of the setup, we should quit
        if (!setup.WasCompleted) {
          Environment.Exit(0);
          return;
        }

        SettingsControl.ReloadSettings();
      }

      ReloadHeadlines();

      Log.Information("MainWindow initialized.");

      Show();
      Activate();
    }

    private void LaunchButton_Click(object sender, RoutedEventArgs e) {
      if (AutoLaunchCheckBox.IsChecked == true && App.Settings.HasShownAutoLaunchDisclaimer.GetValueOrDefault(false) == false) {
        CustomMessageBox.Show(Loc.Localize("AutoLaunchIntro", "You are enabling Auto-Launch.\nThis means that EDQuickLauncher will always log you in with the current account and you will not see this window.\n\nTo change settings and accounts, you have to hold the shift button on your keyboard while clicking the EDQuickLauncher icon."), "EDQuickLauncher");
        App.Settings.HasShownAutoLaunchDisclaimer = true;
      }

      Kickoff(false);
    }

    private void Kickoff(bool autoLogin) => PrepareLogin(autoLogin);

    private void PrepareLogin(bool autoLaunch) {
      if (_isLaunching)
        return;

      _isLaunching = true;

      if (!autoLaunch) {
        App.Settings.AutolaunchEnabled = AutoLaunchCheckBox.IsChecked == true;
      }

      StartLogin();
    }

    private async void StartLogin() {
      Log.Information("StartLogin() called");
      try {
        await StartGameAndAddon();
      } catch (Exception e) {
        Log.Debug(e, "Failed to call StartGameAndAddon()");
      }
    }

    private async Task StartGameAndAddon() {
      // We won't do any sanity checks here anymore, since that should be handled in StartLogin

      System.Diagnostics.Process gameProcess = Launcher.LaunchGame(App.Settings.ExpansionVersion, App.Settings.AdditionalLaunchArgs, App.Settings.GamePath);

      if (gameProcess == null) {
        Log.Information("GameProcess was null...");
        Reactivate();
        return;
      }

      Hide();

      var addonMgr = new AddonManager();

      try {
        if (App.Settings.AddonList == null)
          App.Settings.AddonList = new List<AddonEntry>();

        var addons = App.Settings.AddonList.Where(x => x.IsEnabled).Select(x => x.Addon).Cast<IAddon>().ToList();

        await Task.Run(() => addonMgr.RunAddons(gameProcess, App.Settings, addons));
      } catch (Exception ex) {
        new ErrorWindow(ex,
          "This could be caused by your anti-virus, please check its logs and add any needed exclusions.",
          "Add-ons").ShowDialog();
        Reactivate();

        addonMgr.StopAddons();
      }

      var watchThread = new Thread(() => {
        while (!gameProcess.HasExited) {
          gameProcess.Refresh();
          Thread.Sleep(1);
        }

        Log.Information("Game has exited.");
        addonMgr.StopAddons();

        CleanUp(gameProcess);

        Environment.Exit(0);
      });
      watchThread.Start();

      Log.Debug("Started WatchThread");
    }

    private void CleanUp(Process process) {
      process.Dispose();
    }

    private void Card_KeyDown(object sender, KeyEventArgs e) {
      if ((e.Key != Key.Enter && e.Key != Key.Return) || _isLaunching)
        return;

      Kickoff(false);
      _isLaunching = true;
    }

    private void Reactivate() {
      _isLaunching = false;
      ReloadHeadlines();
      Activate();
    }

    private void MainWindow_OnClosed(object sender, EventArgs e) {
      Application.Current.Shutdown();
    }

    public void MainWindow_OnWindowClosing(object sender, CancelEventArgs args) {
      if (_isLaunching)
        args.Cancel = true;
    }

    private void SettingsControl_OnSettingsDismissed(object sender, EventArgs e) {
    }

    private async void FakeStart_OnClick(object sender, RoutedEventArgs e) => await StartGameAndAddon();

    private void ExpansionSwitcher_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      if (EnvironmentSettings.NoDirectLaunch) {
        if (ExpansionSwitcher.SelectedIndex == 0) {
          App.Settings.ExpansionVersion = 0;
        }
      } else {
        if (ExpansionSwitcher.SelectedIndex == 0) {
          App.Settings.ExpansionVersion = 1;
        } else if (ExpansionSwitcher.SelectedIndex == 1) {
          App.Settings.ExpansionVersion = 2;
        }
      }
    }

    private void EnableVrCheckBox_CheckedChanged(object sender, RoutedEventArgs e) {
    }

    private void AutoLaunchCheckBox_CheckedChanged(object sender, RoutedEventArgs e) {
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) {
      switch (App.Settings.ExpansionVersion) {
        case 0:
        case 1:
          ExpansionSwitcher.SelectedIndex = 0;
          break;

        case 2:
          ExpansionSwitcher.SelectedIndex = 1;
          break;

        default:
          ExpansionSwitcher.SelectedIndex = 0;
          break;
      }
    }

    private async Task SetupHeadlines() {
      try {
        _headlines = await Headlines.Get();

        _ = Dispatcher.BeginInvoke(() => NewsListView.ItemsSource = _headlines.News);
      } catch (Exception) {
        _ = Dispatcher.BeginInvoke(() => NewsListView.ItemsSource = new List<News> { new News { Title = Loc.Localize("NewsDlFailed", "Could not download news data.") } });
      }
    }

    private void NewsListView_OnMouseUp(object sender, MouseButtonEventArgs e) {
      if (e.ChangedButton != MouseButton.Left)
        return;

      if (_headlines == null)
        return;

      if (NewsListView.SelectedItem is not News item)
        return;

      if (!string.IsNullOrEmpty(item.Url)) {
        Util.OpenBrowser(item.Url);
      }
    }
  }
}