﻿/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using EDQuickLauncher.Settings;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace EDQuickLauncher.Addon {
  internal class AddonManager {
    private List<Tuple<IAddon, Thread, CancellationTokenSource>> _runningAddons;

    public void RunAddons(Process gameProcess, LauncherSettingsV2 setting, List<IAddon> addonEntries)
    {
      if (_runningAddons != null)
        throw new Exception("Addons still running?");

      _runningAddons = new List<Tuple<IAddon, Thread, CancellationTokenSource>>();

      foreach (IAddon addonEntry in addonEntries)
      {
        addonEntry.Setup(gameProcess, setting);

        if (addonEntry is IPersistentAddon persistentAddon)
        {
          Log.Information("Starting PersistentAddon {0}", persistentAddon.Name);
          var cancellationTokenSource = new CancellationTokenSource();

          var addonThread = new Thread(persistentAddon.DoWork);
          addonThread.Start(cancellationTokenSource.Token);

          _runningAddons.Add(new Tuple<IAddon, Thread, CancellationTokenSource>(persistentAddon, addonThread, cancellationTokenSource));
        }

        if (addonEntry is IRunnableAddon runnableAddon)
        {
          Log.Information("Starting RunnableAddon {0}", runnableAddon.Name);
          runnableAddon.Run();
        }

        if (addonEntry is INotifyAddonAfterClose notifiedAddon)
          _runningAddons.Add(new Tuple<IAddon, Thread, CancellationTokenSource>(notifiedAddon, null, null));
      }
    }

    public void StopAddons()
    {
      Log.Information("Stopping addons...");

      if (_runningAddons != null)
      {
        foreach (Tuple<IAddon, Thread, CancellationTokenSource> addon in _runningAddons)
        {
          addon.Item3?.Cancel();
          addon.Item2?.Join();

          if (addon.Item1 is INotifyAddonAfterClose notifiedAddon)
            notifiedAddon.GameClosed();
        }

        _runningAddons = null;
      }
    }
  }
}