﻿/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using Config.Net;
using System;
using System.Collections.Generic;
using System.IO;

namespace EDQuickLauncher.Settings.Parsers {
  internal class DirectoryInfoParser : ITypeParser {
    public IEnumerable<Type> SupportedTypes => new[] { typeof(DirectoryInfo) };

    public string ToRawString(object value)
    {
      if (value is DirectoryInfo di)
        return di.FullName;

      return null;
    }

    public bool TryParse(string value, Type t, out object result)
    {
      if (value == null)
      {
        result = null;
        return false;
      }

      if (t == typeof(DirectoryInfo))
      {
        var di = new DirectoryInfo(value);
        result = di;
        return true;
      }

      result = null;
      return false;
    }
  }
}