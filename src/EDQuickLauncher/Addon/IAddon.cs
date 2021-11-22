/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using EDQuickLauncher.Settings;
using System.Diagnostics;

namespace EDQuickLauncher.Addon {
  public interface IAddon {
    string Name { get; }
    void Setup(Process gameProcess, LauncherSettingsV2 setting);
  }
}