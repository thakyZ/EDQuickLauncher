/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

using CheapLoc;
using EDQuickLauncher.Windows;
using Serilog;
using SteamworksSharp;
using SteamworksSharp.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace EDQuickLauncher.Game {

  public class Launcher {
    private const int STEAM_APP_ID = 359320;

    public static Process LaunchGame(int expansionLevel, string additionalArguments, DirectoryInfo gamePath) {
      Log.Information($"EliteDangerousGame::LaunchGame(expansionLevel:{expansionLevel}, steamIntegration:{true}, args:{additionalArguments})");

      try {
        try {
          SteamNative.Initialize();
          if (SteamApi.IsSteamRunning() && SteamApi.Initialize(STEAM_APP_ID)) {
            Log.Information("Steam initialized.");
          }
        } catch (Exception ex) {
          Log.Error(ex, "Could not initialize Steam.");
        }

        var exePath = gamePath.FullName;

        var workingDir = gamePath.FullName;
        switch (expansionLevel) {
          case 1:
            workingDir = Path.Combine(workingDir, "Products", "elite-dangerous-64");
            exePath = Path.Combine(workingDir, "EliteDangerous64.exe");
            break;

          case 2:
            workingDir = Path.Combine(workingDir, "Products", "elite-dangerous-odyssey-64");
            exePath = Path.Combine(workingDir, "EliteDangerous64.exe");
            break;

          default:
            exePath = Path.Combine(gamePath.FullName, "EDLaunch.exe");
            break;
        }

        var enviroment = new Dictionary<string, string>();

        ArgumentBuilder argumentBuilder = new ArgumentBuilder()
          .Append("/Steam", "");

        if (App.Settings.EnableVR) {
          argumentBuilder.Append("/VR", "");
        } else {
          argumentBuilder.Append("/novr", "");
        }

        // Unneeded?
        // environment.Add("IS_ED_LAUNCH_FROM_STEAM", "1");

        // This is a bit of a hack; ideally additionalArguments would be a dictionary or some KeyValue structure
        if (!String.IsNullOrEmpty(additionalArguments)) {
          var regex = new Regex(@"\s*(?<key>[^=]+)\s*=\s*(?<value>[^\s]+)\s*", RegexOptions.Compiled);
          foreach (Match match in regex.Matches(additionalArguments))
            argumentBuilder.Append(match.Groups["key"].Value, match.Groups["value"].Value);
        }

        if (!File.Exists(exePath)) {
          CustomMessageBox.Show(
            Loc.Localize("BinaryNotPresentError", "Could not find the game executable.\nThis might be caused by your antivirus. You may have to reinstall the game."), "EDQuickLauncher Error", MessageBoxButton.OK, MessageBoxImage.Error);

          Log.Error("Game binary at {0} wasn't present.", exePath);

          return null;
        }

        Task<Process> game;
        try {
          var arguments = argumentBuilder.Build();
          //game = NativeAclFix.LaunchGame(workingDir, exePath, arguments, environment);
          game = RunProcessAsync(new ProcessStartInfo {
            WorkingDirectory = workingDir,
            FileName = exePath,
            Arguments = arguments
          }, enviroment);
        } catch (Win32Exception ex) {
          CustomMessageBox.Show(String.Format(Loc.Localize("NativeLauncherError", "Could not start the game correctly. Please report this error.\n\nHRESULT: 0x{0}"), ex.HResult.ToString("X")), "EDQuickLauncher Error", MessageBoxButton.OK, MessageBoxImage.Error);
          Log.Error(ex, $"Launcher error; {ex.HResult}: {ex.Message}");
          return null;
        }

        try {
          SteamApi.Uninitialize();
          SteamNative.Uninitialize();
        } catch (Exception ex) {
          Log.Error(ex, "Could not uninitialize Steam.");
        }

        return game.Result;
      } catch (Exception ex) {
        new ErrorWindow(ex, "Your game path might not be correct. Please check in the settings.", "XG LaunchGame").ShowDialog();
      }

      return null;
    }

    private static Task<Process> RunProcessAsync(ProcessStartInfo startInfo, Dictionary<string, string> enviroment) {
      var tcs = new TaskCompletionSource<Process>();

      var process = new Process {
        StartInfo = startInfo,
        EnableRaisingEvents = true
      };
      foreach (KeyValuePair<string, string> entry in enviroment) {
        process.StartInfo.EnvironmentVariables.Add(entry.Key, entry.Value);
      }

      process.Exited += (sender, args) => CatchExit(process);

      process.Start();

      tcs.SetResult(process);

      return tcs.Task;
    }

    private static void CatchExit(Process game) {
      game.Refresh();

      // Something went wrong here, why even bother
      if (game.HasExited) {
        if (Process.GetProcessesByName("EDLaunch").Length +
          Process.GetProcessesByName("EliteDangerous64").Length >= 2) {
          CustomMessageBox.Show(
            Loc.Localize("MultiboxDeniedWarning",
              "You can't launch more than two instances of the game by default.\n\nPlease check if there is an instance of the game that did not close correctly."),
            "EDQuickLauncher Error", image: MessageBoxImage.Error);
        } else {
          if (game.ExitCode != 0) {
            throw new Exception("Game exited prematurely");
          }
        }
      }
    }
  }

  public sealed class ArgumentBuilder {
    private readonly List<KeyValuePair<string, string>> m_arguments;

    public ArgumentBuilder() => m_arguments = new List<KeyValuePair<string, string>>();

    public ArgumentBuilder(IEnumerable<KeyValuePair<string, string>> items) => m_arguments = new List<KeyValuePair<string, string>>(items);

    public ArgumentBuilder Append(string key, string value) => Append(new KeyValuePair<string, string>(key, value));

    public ArgumentBuilder Append(KeyValuePair<string, string> item) {
      m_arguments.Add(item);

      return this;
    }

    public ArgumentBuilder Append(IEnumerable<KeyValuePair<string, string>> items) {
      m_arguments.AddRange(items);

      return this;
    }

    public string Build() {
      return m_arguments.Aggregate(new StringBuilder(),
      (whole, part) => part.Value != "" ? whole.Append($" {part.Key}={part.Value}") : whole.Append($" {part.Key}")).ToString();
    }
  }
}