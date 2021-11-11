/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using CheapLoc;

namespace EDQuickLauncher.Windows.ViewModel {
  internal class ChangeLogWindowViewModel {
    public ChangeLogWindowViewModel() => SetupLoc();

    public void SetupLoc() {
      UpdateNoticeLoc = System.String.Format(Loc.Localize("UpdateNotice", "EDQuickLauncher was updated to version {0}."), Util.GetAssemblyVersion());
      JoinDiscordLoc = Loc.Localize("JoinDiscord", "Join Discord");
      SendEmailLoc = Loc.Localize("SendEmail", "Send Email");
      EmailInfoLoc = Loc.Localize("EmailInfo", "EDQuickLauncher is free, open-source software - it doesn't use any telemetry or analysis tools to collect your data, but it would help a lot if you could send me a short email with your operating system, why you use XIVLauncher and, if needed, any criticism or things we can do better. Your email will be deleted immediately after evaluation and I won't ever contact you.\n\nThank you very much for using EDQuickLauncher!");
      ChangelogThanksLoc = Loc.Localize("ChangelogThanks", "EDQuickLauncher is free, open-source software supported by a variety of people from all over the world.\nThank you for sticking around!");
      OkLoc = Loc.Localize("OK", "OK");
    }

    public string UpdateNoticeLoc { get; private set; }

    public string JoinDiscordLoc { get; private set; }

    public string SendEmailLoc { get; private set; }

    public string EmailInfoLoc { get; private set; }

    public string ChangelogThanksLoc { get; private set; }

    public string OkLoc { get; private set; }
  }
}