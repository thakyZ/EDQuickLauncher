﻿/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using CheapLoc;

namespace EDQuickLauncher.Windows.ViewModel {
  internal class FirstTimeSetupViewModel {
    public FirstTimeSetupViewModel() => SetupLoc();

    public void SetupLoc() {
      FirstTimeGamePathLoc = Loc.Localize("ChooseGamePathFTS",
        "Please select the folder your game is installed in.\r\nIt should contain the folder \"Products\".");
      NextLoc = Loc.Localize("Next", "Next");
    }

    public string FirstTimeGamePathLoc { get; private set; }
    public string NextLoc { get; private set; }
  }
}