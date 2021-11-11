/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

using EDQuickLauncher.Addon;
using System.Collections.Generic;
using System.IO;

namespace EDQuickLauncher.Settings {

  public interface ILauncherSettingsV1 {

    #region Launcher Setting

    DirectoryInfo GamePath { get; set; }
    bool AutolaunchEnabled { get; set; }
    List<AddonEntry> AddonList { get; set; }
    string AdditionalLaunchArgs { get; set; }
    LauncherLanguage? LauncherLanguage { get; set; }
    bool? HasComplainedAboutAdmin { get; set; }
    string LastVersion { get; set; }
    bool? HasShownAutoLaunchDisclaimer { get; set; }
    string AcceptLanguage { get; set; }
    bool EnableVR { get; set; }
    int ExpansionVersion { get; set; }

    #endregion Launcher Setting
  }
}