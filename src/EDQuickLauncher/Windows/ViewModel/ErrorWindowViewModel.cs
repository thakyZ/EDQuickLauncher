/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using CheapLoc;

namespace EDQuickLauncher.Windows.ViewModel {
  internal class ErrorWindowViewModel {
    public ErrorWindowViewModel() => SetupLoc();

    private void SetupLoc() {
      ErrorExplanationMsgLoc = Loc.Localize("ErrorExplanation",
        "An error in EDQuickLauncher occurred. Please consult the FAQ. If this issue persists, please report\r\nit on GitHub by clicking the button below, describing the issue and copying the text in the box.");
      JoinDiscordLoc = Loc.Localize("JoinDiscord", "Join Discord");
      OpenFaqLoc = Loc.Localize("OpenFaq", "Open FAQ");
      ReportErrorLoc = Loc.Localize("ReportError", "Report error");
      OkLoc = Loc.Localize("OK", "OK");
    }

    public string ErrorExplanationMsgLoc { get; private set; }

    public string JoinDiscordLoc { get; private set; }

    public string OpenIntegrityReportLoc { get; private set; }

    public string OpenFaqLoc { get; private set; }

    public string ReportErrorLoc { get; private set; }

    public string OkLoc { get; private set; }
  }
}