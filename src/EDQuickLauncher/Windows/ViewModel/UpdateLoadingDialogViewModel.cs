/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using CheapLoc;

namespace EDQuickLauncher.Windows.ViewModel {
  internal class UpdateLoadingDialogViewModel {
    public UpdateLoadingDialogViewModel() => SetupLoc();

    public void SetupLoc() {
      UpdateCheckLoc = Loc.Localize("UpdateCheckMsg", "Checking for updates...");
      AutoLaunchHintLoc = Loc.Localize("AutoLaunchHint", "Hold the shift key to change settings!");
      ResetUidCacheHintLoc = Loc.Localize("ResetUidCacheHint", "Hold the control key to reset UID cache!");
    }

    public string UpdateCheckLoc { get; private set; }

    public string AutoLaunchHintLoc { get; private set; }

    public string ResetUidCacheHintLoc { get; private set; }
  }
}