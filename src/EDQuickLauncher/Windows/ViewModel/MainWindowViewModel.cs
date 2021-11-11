/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

using CheapLoc;

namespace EDQuickLauncher.Windows.ViewModel {

  internal class MainWindowViewModel {

    public MainWindowViewModel() => SetupLoc();

    private void SetupLoc() {
      AutoLaunchLoc = Loc.Localize("ExpansionBoxAutoLaunch", "Launch automatically");
      ExpansionLoc = Loc.Localize("ExpansionBoxAutoExpansion", "Use One-Time-Passwords");
      CancelLoc = Loc.Localize("Cancel", "Cancel");
      SettingsLoc = Loc.Localize("Settings", "Settings");
      EliteDangerousLauncherLoc = Loc.Localize("EliteDangerousLauncher", "Elite: Dangerous Launcher");
      EliteDangerousLoc = Loc.Localize("EliteDangerous", "Elite: Dangerous");
      EliteDangerousExpansionLoc = Loc.Localize("EliteDangerousExpansion", "Elite: Dangerous Expansion");
    }

    public string AutoLaunchLoc { get; private set; }

    public string ExpansionLoc { get; private set; }

    public string CancelLoc { get; private set; }

    public string SettingsLoc { get; private set; }

    public string EliteDangerousLoc { get; private set; }

    public string EliteDangerousLauncherLoc { get; private set; }

    public string EliteDangerousExpansionLoc { get; private set; }
  }
}