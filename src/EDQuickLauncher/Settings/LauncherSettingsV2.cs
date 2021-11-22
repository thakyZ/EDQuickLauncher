/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using EDQuickLauncher.Addon;
using EDQuickLauncher.Settings.Parsers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace EDQuickLauncher.Settings {
  public class LauncherSettingsV2 {
    #region Launcher Setting

    [JsonConverter(typeof(DirectoryInfoConverter))]
    [JsonProperty(Required = Required.AllowNull)]
    public DirectoryInfo GamePath { get; set; }

    public bool AutolaunchEnabled { get; set; }

    [JsonProperty(Required = Required.AllowNull)]
    public List<AddonEntry> AddonList { get; set; }

    public string AdditionalLaunchArgs { get; set; }

    [JsonProperty(Required = Required.AllowNull)]
    public LauncherLanguage? LauncherLanguage { get; set; }

    public bool? HasComplainedAboutAdmin { get; set; }

    public string LastVersion { get; set; }

    public bool? HasShownAutoLaunchDisclaimer { get; set; }

    public string AcceptLanguage { get; set; }

    public bool EnableVR { get; set; }

    public int ExpansionVersion { get; set; }

    #endregion Launcher Setting
  }
}
