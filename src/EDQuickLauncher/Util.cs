/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace EDQuickLauncher {

  public static class Util {

    /// <summary>
    /// Generates a temporary file name.
    /// </summary>
    /// <returns>A temporary file name that is almost guaranteed to be unique.</returns>
    public static string GetTempFileName() =>
      // https://stackoverflow.com/a/50413126
      Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

    public static void ShowError(string message, string caption, [CallerMemberName] string callerName = "", [CallerLineNumber] int callerLineNumber = 0) {
      MessageBox.Show($"{message}\n\n{callerName} L{callerLineNumber}", caption, MessageBoxButton.OK,
        MessageBoxImage.Error);
    }

    /// <summary>
    ///     Gets the git hash value from the assembly
    ///     or null if it cannot be found.
    /// </summary>
    public static string GetGitHash() {
      Assembly asm = typeof(Util).Assembly;
      IEnumerable<AssemblyMetadataAttribute> attrs = asm.GetCustomAttributes<AssemblyMetadataAttribute>();
      return attrs.FirstOrDefault(a => a.Key == "GitHash")?.Value;
    }

    public static string GetAssemblyVersion() {
      var assembly = Assembly.GetExecutingAssembly();
      var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
      return fvi.FileVersion;
    }

    public static bool IsValidFfxivPath(string path) => !String.IsNullOrEmpty(path) && Directory.Exists(Path.Combine(path, "Products")) && Directory.Exists(Path.Combine(path, "Products", "elite-dangerous-64"));

    public static bool LetChoosePath(string path) {
      if (String.IsNullOrEmpty(path))
        return true;

      var di = new DirectoryInfo(path);

      return di.Name != "Products" && di.Name != "elite-dangerous-64" && di.Name != "elite-dangerous-odyssey-64";
    }

    private static readonly string DefaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "SquareEnix\\FINAL FANTASY XIV - A Realm Reborn");

    private static readonly string[] PathsToTry = SelectEachDrive().Concat(new List<string> {
      Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ProgramFilesX86), "Steam\\steamapps\\common\\Elite Dangerous"),
      DefaultPath
    }).ToArray();

    private static List<string> SelectEachDrive() {
      var eachFolder = new List<string>();

      foreach (DriveInfo driveInfo in DriveInfo.GetDrives()) {
        eachFolder.Add($"{driveInfo.Name}SteamLibrary\\SteamApps\\common\\Elite Dangerous");

        eachFolder.Add($"{driveInfo.Name}Steam Library\\SteamApps\\common\\Elite Dangerous");

        eachFolder.Add($"{driveInfo.Name}Steam\\SteamApps\\common\\Elite Dangerous");
      }

      return eachFolder;
    }

    public static string TryGamePaths() {
      foreach (var path in PathsToTry) {
        if (Directory.Exists(path) && IsValidFfxivPath(path))
          return path;
      }

      return DefaultPath;
    }

    public static int GetUnixMillis() => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;

    public static Color ColorFromArgb(int argb) => Color.FromArgb((byte)(argb >> 24), (byte)(argb >> 16), (byte)(argb >> 8), (byte)argb);

    public static int ColorToArgb(Color color) => (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;

    public static SolidColorBrush SolidColorBrushFromArgb(int argb) => new SolidColorBrush(ColorFromArgb(argb));

    public static void OpenDiscord(object sender, RoutedEventArgs e) => Process.Start("https://discord.me/nekogaming");

    public static void OpenFaq(object sender, RoutedEventArgs e) => Process.Start("https://thakyz.github.io/edquicklauncher-faq/");

    public static bool CheckIsGameOpen() {
      Process[] procs = Process.GetProcesses();

      return procs.Any(x => x.ProcessName == "EDLaunch") || procs.Any(x => x.ProcessName == "EliteDangerous64");
    }

    public static string GetFromResources(string resourceName) {
      Assembly asm = typeof(Util).Assembly;
      using Stream stream = asm.GetManifestResourceStream(resourceName);
      using var reader = new StreamReader(stream);

      return reader.ReadToEnd();
    }

    public static string GenerateAcceptLanguage() {
      CultureInfo system = CultureInfo.InstalledUICulture;
      switch (system.Name) {
        case "en-US":
          return system.Name;

        case "en-GB":
        case "de-DE":
        case "fr-BE":
        case "ja":
        case "fr-FR":
        case "fr-CH":
          return "en-US";
        //return system.Name;
        default:
          return "en-US";
      };
    }

    public static bool HasExpansion => Directory.Exists(Path.Combine(App.Settings.GamePath.FullName, "Products", "elite-dangerous-64"));
  }

  public class Paths {
    public static string RoamingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EDQuickLauncher");

    public static string ResourcesPath = Path.Combine(Path.GetDirectoryName(typeof(Paths).Assembly.Location), "Resources");
  }
}