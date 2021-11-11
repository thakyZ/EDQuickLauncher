/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using SteamworksSharp.Native.Libs;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SteamworksSharp.Native.Windows_x64 {
  public class NativeLibrary : INativeLibrary {
    public OSPlatform Platform { get; } = OSPlatform.Windows;

    public Architecture Architecture { get; } = Architecture.X64;

    public Lazy<byte[]> LibraryBytes { get; } = new Lazy<byte[]>(() =>
      LibUtils.ReadResourceBytes(typeof(NativeLibrary).GetTypeInfo().Assembly, "EDQuickLauncher.SteamWorksSharp.Native.Windows_x64.steam_api64.dll"));

    public string Extension { get; } = "dll";
  }
}