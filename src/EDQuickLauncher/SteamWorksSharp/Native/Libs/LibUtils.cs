/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using System.IO;
using System.Reflection;

namespace SteamworksSharp.Native.Libs {
  internal static class LibUtils {
    public static byte[] ReadResourceBytes(Assembly assembly, string resourceName) {
      using (Stream stream = assembly.GetManifestResourceStream(resourceName))
      using (var reader = new BinaryReader(stream)) {
        return reader.ReadBytes((int)stream.Length);
      }
    }
  }
}