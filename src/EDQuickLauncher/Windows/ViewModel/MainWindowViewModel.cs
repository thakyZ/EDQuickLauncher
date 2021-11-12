/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

using CheapLoc;
using System.Collections.Generic;
using System.Windows.Controls;

namespace EDQuickLauncher.Windows.ViewModel {

  public class MainWindowViewModel {

    public MainWindowViewModel() => SetupLoc();

    private void SetupLoc() {
      AutoLaunchLoc = Loc.Localize("ExpansionBoxAutoLaunch", "Launch automatically");
      ExpansionLoc = Loc.Localize("ExpansionBoxAutoExpansion", "Use One-Time-Passwords");
      CancelLoc = Loc.Localize("Cancel", "Cancel");
      SettingsLoc = Loc.Localize("Settings", "Settings");
      EnableVrLoc = Loc.Localize("EnableVr", "Enable VR");
      LaunchLoc = Loc.Localize("ExpansionBoxLogin", "Launch");
      LaunchTooltipLoc = Loc.Localize("ExpansionBoxLoginTooltip", "Launch Game");
      EliteDangerousLauncherLoc = Loc.Localize("EliteDangerousLauncher", "Elite: Dangerous Launcher");
      EliteDangerousLoc = Loc.Localize("EliteDangerous", "Elite: Dangerous");
      EliteDangerousExpansionLoc = Loc.Localize("EliteDangerousExpansion", "Elite: Dangerous Expansion");
      var items = new List<ComboBoxItem>();
      if (EnvironmentSettings.NoDirectLaunch) {
        items.Add(new ComboBoxItem { Content = EliteDangerousLauncherLoc, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left });
      } else {
        items.Add(new ComboBoxItem { Content = EliteDangerousLoc, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left });
        if (Util.HasExpansion) {
          items.Add(new ComboBoxItem { Content = EliteDangerousExpansionLoc, VerticalContentAlignment = System.Windows.VerticalAlignment.Center, HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left });
        }
      }
      ExpansionSwitcherItems = items;
    }

    public string AutoLaunchLoc { get; private set; }

    public string ExpansionLoc { get; private set; }

    public string CancelLoc { get; private set; }

    public string SettingsLoc { get; private set; }

    public string EnableVrLoc { get; private set; }

    public string LaunchLoc { get; private set; }

    public string LaunchTooltipLoc { get; private set; }

    public string EliteDangerousLoc { get; private set; }

    public string EliteDangerousLauncherLoc { get; private set; }

    public string EliteDangerousExpansionLoc { get; private set; }

    public static List<ComboBoxItem> ExpansionSwitcherItems { get; private set; }
  }
}