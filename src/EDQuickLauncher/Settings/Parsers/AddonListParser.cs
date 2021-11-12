/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

using Config.Net;
using EDQuickLauncher.Addon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EDQuickLauncher.Settings.Parsers {

  internal class AddonListParser : ITypeParser {
    public IEnumerable<Type> SupportedTypes => new[] { typeof(List<AddonEntry>) };

    public string ToRawString(object value) {
      return value is List<AddonEntry> list ? JsonConvert.SerializeObject(list) : null;
    }

    public bool TryParse(string value, Type t, out object result) {
      if (value == null) {
        result = null;
        return false;
      }

      if (t == typeof(List<AddonEntry>)) {
        result = JsonConvert.DeserializeObject<List<AddonEntry>>(value);
        return true;
      }

      result = null;
      return false;
    }
  }
}