/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

namespace EDQuickLauncher {

  internal static class EnvironmentSettings {
    public static bool IsWine => CheckEnvBool("ED_WINEONLINUX");
    public static bool IsDisableUpdates => CheckEnvBool("ED_NOAUTOUPDATE");
    public static bool IsPreRelease => CheckEnvBool("ED_PRERELEASE");
    public static bool IsNoRunas => CheckEnvBool("ED_NO_RUNAS");

    public static bool NoDirectLaunch => !CheckEnvBool("ED_NO_DIRECT_LAUNCH");

    private static bool CheckEnvBool(string var) => System.Boolean.Parse(System.Environment.GetEnvironmentVariable(var) ?? "false");
  }
}