/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

using CheapLoc;
using EDQuickLauncher.Windows;
using Serilog;
using Squirrel;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace EDQuickLauncher {

  internal class Updates {
#if !XL_NOAUTOUPDATE
    public EventHandler OnUpdateCheckFinished;
#endif

    public async Task Run(bool downloadPrerelease = false) {
      // GitHub requires TLS 1.2, we need to hardcode this for Windows 7
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      try {
        ReleaseEntry newRelease = null;

        using (UpdateManager updateManager = await UpdateManager.GitHubUpdateManager(repoUrl: App.RepoUrl, applicationName: "EDQuickLauncher", prerelease: downloadPrerelease)) {
          // TODO: is this allowed?
          SquirrelAwareApp.HandleEvents(
            onInitialInstall: v => updateManager.CreateShortcutForThisExe(),
            onAppUpdate: v => updateManager.CreateShortcutForThisExe(),
            onAppUninstall: v => updateManager.RemoveShortcutForThisExe());

          UpdateInfo a = await updateManager.CheckForUpdate();
          newRelease = await updateManager.UpdateApp();
        }

        if (newRelease != null) {
          UpdateManager.RestartApp();
        }
#if !XL_NOAUTOUPDATE
        else
          OnUpdateCheckFinished?.Invoke(this, null);
#endif
      } catch (Exception ex) {
        Log.Error(ex, "Update failed");
        CustomMessageBox.Show(Loc.Localize("UpdateFailureError", "EDQuickLauncher failed to check for updates. This may be caused by connectivity issues to GitHub. Wait a few minutes and try again.\nDisable your VPN, if you have one.\nIf it continues to fail after several minutes, please join the discord linked on GitHub for support."),
          "EDQuickLauncher",
          MessageBoxButton.OK,
          MessageBoxImage.Error);
        System.Environment.Exit(1);
      }

      // Reset security protocol after updating
      ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
    }
  }
}