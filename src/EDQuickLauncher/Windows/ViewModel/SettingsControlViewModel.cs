/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

using CheapLoc;
using EDQuickLauncher.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace EDQuickLauncher.Windows.ViewModel {

  internal class SettingsControlViewModel : INotifyPropertyChanged {
    private string _gamePath;
    private string _patchPath;

    public SettingsControlViewModel() => SetupLoc();

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>
    /// Gets a value indicating whether the "Run Integrity Checks" button is enabled.
    /// </summary>
    public bool IsRunIntegrityCheckPossible =>
      !System.String.IsNullOrEmpty(GamePath) && Directory.Exists(GamePath);

    /// <summary>
    /// Gets or sets the path to the game folder.
    /// </summary>
    public string GamePath {
      get => _gamePath;

      set {
        _gamePath = value;
        OnPropertyChanged(nameof(GamePath));
        OnPropertyChanged(nameof(IsRunIntegrityCheckPossible));
      }
    }

    /// <summary>
    /// Gets or sets the path to the game folder.
    /// </summary>
    public string PatchPath {
      get => _patchPath;

      set {
        _patchPath = value;
        OnPropertyChanged(nameof(PatchPath));
      }
    }

    private void SetupLoc() {
      SaveSettingsLoc = Loc.Localize("SaveSettings", "Save Settings");

      SettingsGameLoc = Loc.Localize("SettingsGame", "Game");
      GamePathLoc = Loc.Localize("ChooseGamePath",
        "Please select the folder your game is installed in.\r\nIt should contain the folders \"Products\".");
      GamePathSafeguardLoc = Loc.Localize("GamePathSafeguardError",
        "Please do not select the \"Products\", \"elite-dangerous-64\" or \"elite-dangerous-odyssey-64\" folder of your Elite Dangerous installation, and choose the folder that contains these instead.");
      AdditionalArgumentsLoc = Loc.Localize("AdditionalArguments", "Additional launch arguments");
      SettingsGameSettingsLoc = Loc.Localize("SettingsGameSettings", "Game Settings");
      ChooseLauncherLanguageLoc = Loc.Localize("ChooseLauncherLanguage", "Please select the launcher language, requires a restart.");
      LauncherLanguageHelpCtaLoc = Loc.Localize("LauncherLanguageHelpCtaLoc",
        "Notice any mistakes? You can help out translating the launcher! Just click here!");
      LauncherLanguageNoticeLoc = Loc.Localize("LauncherLanguageNotice", "A restart is required to apply the launcher language setting.");

      SettingsAutoLaunchLoc = Loc.Localize("SettingsAutoLaunch", "Auto-Launch");
      AutoLaunchHintLoc = Loc.Localize("AutoLaunchHint",
        "These are applications that are started as soon as the game has started.");
      RemoveLoc = Loc.Localize("Remove", "Remove");
      AddNewLoc = Loc.Localize("AddNew", "Add new");
      AutoLaunchAddNewToolTipLoc =
        Loc.Localize("AutoLaunchAddNewToolTip", "Add a new Auto-Start entry that allows you to launch any program.");

      SettingsAboutLoc = Loc.Localize("SettingsAbout", "About");
      CreditsLoc = Loc.Localize("Credits",
        "Made by thakyZ. Special thanks to the GoatCorp team.\r\n\r\nAny issues or requests? Join the Discord or create an issue on GitHub!");
      LicenseLoc = Loc.Localize("License", "Licensed under MIT. Click here for more.");
      JoinDiscordLoc = Loc.Localize("JoinDiscord", "Join Discord");
      OpenFaqLoc = Loc.Localize("OpenFaq", "Open FAQ");
      var items = new List<ComboBoxItem>();
      foreach (LauncherLanguage language in (LauncherLanguage[])Enum.GetValues(typeof(LauncherLanguage))) {
        items.Add(new ComboBoxItem { Content = LauncherLanguageExtensions.ToFriendlyString(language) });
      }
      LauncherLanguageItems = items;
    }

    public string SaveSettingsLoc { get; private set; }

    public string SettingsGameLoc { get; private set; }

    public string GamePathLoc { get; private set; }

    public string GamePathSafeguardLoc { get; private set; }

    public string AdditionalArgumentsLoc { get; private set; }

    public string SettingsGameSettingsLoc { get; private set; }

    public string ChooseLauncherLanguageLoc { get; private set; }

    public string LauncherLanguageHelpCtaLoc { get; private set; }

    public static List<ComboBoxItem> LauncherLanguageItems { get; private set; }

    public string LauncherLanguageNoticeLoc { get; private set; }

    public string SettingsAutoLaunchLoc { get; private set; }

    public string AutoLaunchHintLoc { get; private set; }

    public string RemoveLoc { get; private set; }

    public string AddNewLoc { get; private set; }

    public string AutoLaunchAddNewToolTipLoc { get; private set; }

    public string SettingsAboutLoc { get; private set; }

    public string CreditsLoc { get; private set; }

    public string LicenseLoc { get; private set; }

    public string JoinDiscordLoc { get; private set; }

    public string OpenFaqLoc { get; private set; }
  }
}