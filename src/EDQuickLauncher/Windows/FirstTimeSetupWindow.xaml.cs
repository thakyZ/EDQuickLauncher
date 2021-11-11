/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using CheapLoc;
using EDQuickLauncher.Windows.ViewModel;
using IWshRuntimeLibrary;
using System;
using System.IO;
using System.Windows;

namespace EDQuickLauncher.Windows {
  /// <summary>
  ///     Interaction logic for FirstTimeSetup.xaml
  /// </summary>
  public partial class FirstTimeSetup : Window {
    public bool WasCompleted { get; private set; } = false;

    public FirstTimeSetup() {
      InitializeComponent();

      DataContext = new FirstTimeSetupViewModel();

      var detectedPath = Util.TryGamePaths();

      if (detectedPath != null)
        GamePathEntry.Text = detectedPath;

#if XL_NOAUTOUPDATE
      CustomMessageBox.Show(
        $"You're running an unsupported version of XIVLauncher.\n\nThis can be unsafe and a danger to your SE account. If you have not gotten this unsupported version on purpose, please reinstall a clean version from {App.RepoUrl}/releases.",
        "XIVLauncher Problem", MessageBoxButton.OK, MessageBoxImage.Exclamation);
#endif
    }

    public static string GetShortcutTargetFile(string path) {
      var shell = new WshShell();
      var shortcut = (IWshShortcut)shell.CreateShortcut(path);

      return shortcut.TargetPath;
    }

    private void NextButton_Click(object sender, RoutedEventArgs e) {
      if (SetupTabControl.SelectedIndex == 0) {
        if (String.IsNullOrEmpty(GamePathEntry.Text)) {
          CustomMessageBox.Show(Loc.Localize("GamePathEmptyError", "Please select a game path."), "Error",
            MessageBoxButton.OK, MessageBoxImage.Error, false);
          return;
        }

        if (!Util.LetChoosePath(GamePathEntry.Text)) {
          CustomMessageBox.Show(Loc.Localize("GamePathSafeguardError", "Please do not select the \"game\" or \"boot\" folder of your FFXIV installation, and choose the folder that contains these instead."), "Error",
            MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }

        if (!Util.IsValidFfxivPath(GamePathEntry.Text)) {
          CustomMessageBox.Show(Loc.Localize("GamePathInvalidError", "The folder you selected has no FFXIV installation.\nXIVLauncher will install FFXIV the first time you log in."), "XIVLauncher",
            MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }

      if (SetupTabControl.SelectedIndex >= SetupTabControl.Items.Count - 1) {
        WasCompleted = true;
        Close();
      }

      SetupTabControl.SelectedIndex++;
    }
  }
}