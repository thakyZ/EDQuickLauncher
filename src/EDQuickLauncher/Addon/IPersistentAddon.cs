/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
namespace EDQuickLauncher.Addon {
  internal interface IPersistentAddon : IAddon {
    void DoWork(object state);
  }
}