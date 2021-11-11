/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using EDQuickLauncher.Addon;
using EDQuickLauncher.Settings;
using EDQuickLauncher.Windows.ViewModel;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EDQuickLauncher.Windows {
  /// <summary>
  ///     Interaction logic for SettingsControl.xaml
  /// </summary>
  public partial class SettingsControl {
    public event EventHandler SettingsDismissed;

    private SettingsControlViewModel ViewModel => DataContext as SettingsControlViewModel;

    public SettingsControl() {
      InitializeComponent();

      DiscordButton.Click += Util.OpenDiscord;
      FaqButton.Click += Util.OpenFaq;
      DataContext = new SettingsControlViewModel();

      ReloadSettings();
    }

    public void ReloadSettings() {
      if (App.Settings.GamePath != null) {
        ViewModel.GamePath = App.Settings.GamePath.FullName;
      }

      LauncherLanguageComboBox.SelectedIndex = (int)App.Settings.LauncherLanguage.GetValueOrDefault(LauncherLanguage.English);
      LauncherLanguageNoticeTextBlock.Visibility = Visibility.Hidden;
      AddonListView.ItemsSource = App.Settings.AddonList ??= new List<AddonEntry>();

      LaunchArgsTextBox.Text = App.Settings.AdditionalLaunchArgs;

      VersionLabel.Text += " - v" + Util.GetAssemblyVersion() + " - " + Util.GetGitHash() + " - " + Environment.Version;
    }

    private void AcceptButton_Click(object sender, RoutedEventArgs e) {
      App.Settings.GamePath = !String.IsNullOrEmpty(ViewModel.GamePath) ? new DirectoryInfo(ViewModel.GamePath) : null;

      // Keep the notice visible if LauncherLanguage has changed
      if (App.Settings.LauncherLanguage == (LauncherLanguage)LauncherLanguageComboBox.SelectedIndex)
        LauncherLanguageNoticeTextBlock.Visibility = Visibility.Hidden;
      App.Settings.LauncherLanguage = (LauncherLanguage)LauncherLanguageComboBox.SelectedIndex;

      App.Settings.AddonList = (List<AddonEntry>)AddonListView.ItemsSource;

      App.Settings.AdditionalLaunchArgs = LaunchArgsTextBox.Text;

      SettingsDismissed?.Invoke(this, null);
    }

    private void GitHubButton_OnClick(object sender, RoutedEventArgs e) => Process.Start("https://github.com/goaaats/FFXIVQuickLauncher");

    private void BackupToolButton_OnClick(object sender, RoutedEventArgs e) => Process.Start(Path.Combine(ViewModel.GamePath, "boot", "ffxivconfig.exe"));

    // All of the list handling is very dirty - but i guess it works

    private void AddAddon_OnClick(object sender, RoutedEventArgs e) {
      var addonSetup = new GenericAddonSetupWindow();
      addonSetup.ShowDialog();

      if (addonSetup.Result != null && !String.IsNullOrEmpty(addonSetup.Result.Path)) {
        List<AddonEntry> addonList = App.Settings.AddonList;

        addonList.Add(new AddonEntry {
          IsEnabled = true,
          Addon = addonSetup.Result
        });

        App.Settings.AddonList = addonList;

        AddonListView.ItemsSource = App.Settings.AddonList;
      }
    }

    private void AddonListView_OnMouseUp(object sender, MouseButtonEventArgs e) {
      if (e.ChangedButton != MouseButton.Left)
        return;

      if (AddonListView.SelectedItem is not AddonEntry entry)
        return;

      if (entry.Addon is GenericAddon genericAddon) {
        var addonSetup = new GenericAddonSetupWindow(genericAddon);
        addonSetup.ShowDialog();

        if (addonSetup.Result != null) {
          App.Settings.AddonList = App.Settings.AddonList.Where(x => x.Addon is GenericAddon thisGenericAddon && thisGenericAddon.Path != genericAddon.Path).ToList();

          List<AddonEntry> addonList = App.Settings.AddonList;

          addonList.Add(new AddonEntry {
            IsEnabled = entry.IsEnabled,
            Addon = addonSetup.Result
          });

          App.Settings.AddonList = addonList;

          AddonListView.ItemsSource = App.Settings.AddonList;
        }
      }
    }

    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e) => App.Settings.AddonList = (List<AddonEntry>)AddonListView.ItemsSource;

    private void RemoveAddonEntry_OnClick(object sender, RoutedEventArgs e) {
      if (AddonListView.SelectedItem is AddonEntry entry && entry.Addon is GenericAddon genericAddon) {
        App.Settings.AddonList = App.Settings.AddonList.Where(x => x.Addon is GenericAddon thisGenericAddon && thisGenericAddon.Path != genericAddon.Path).ToList();

        AddonListView.ItemsSource = App.Settings.AddonList;
      }
    }

    private void LauncherLanguageCombo_SelectionChanged(object sender, RoutedEventArgs e) {
      if (LauncherLanguageNoticeTextBlock != null) {
        LauncherLanguageNoticeTextBlock.Visibility = Visibility.Visible;
      }
    }

    private void OpenI18nLabel_OnClick(object sender, MouseButtonEventArgs e) => Process.Start("https://crowdin.com/project/ffxivquicklauncher");

    private void GamePathEntry_OnTextChanged(object sender, TextChangedEventArgs e) => GamePathSafeguardText.Visibility = !Util.LetChoosePath(ViewModel.GamePath) ? Visibility.Visible : Visibility.Collapsed;

    private void LicenseText_OnMouseUp(object sender, MouseButtonEventArgs e) => Process.Start(Path.Combine(Paths.ResourcesPath, "LICENSE.txt"));

    private void Logo_OnMouseUp(object sender, MouseButtonEventArgs e) {
#if DEBUG
      var fts = new FirstTimeSetup();
      fts.ShowDialog();

      Log.Debug($"WasCompleted: {fts.WasCompleted}");

      ReloadSettings();
#endif
    }

    private void VersionLabel_OnMouseUp(object sender, MouseButtonEventArgs e) => new ChangelogWindow().ShowDialog();
  }
}