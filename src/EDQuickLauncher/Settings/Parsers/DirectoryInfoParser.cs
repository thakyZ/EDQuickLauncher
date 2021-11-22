/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */

using Config.Net;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace EDQuickLauncher.Settings.Parsers {

  internal class DirectoryInfoConverter : JsonConverter {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer jsonSerializer) {
      writer.WriteValue(value is DirectoryInfo di ? di.FullName : null);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      if (reader.Value == null) {
        return null;
      }

      if (objectType == typeof(DirectoryInfo)) {
        return new DirectoryInfo((string)reader.Value);
      }

      return null;
    }

    public override bool CanConvert(Type objectType) {
      return objectType == typeof(DirectoryInfo);
    }
  }

  internal class DirectoryInfoParser : ITypeParser {
    public IEnumerable<Type> SupportedTypes => new[] { typeof(DirectoryInfo) };

    public string ToRawString(object value) {
      return value is DirectoryInfo di ? di.FullName : null;
    }

    public bool TryParse(string value, Type t, out object result) {
      if (value == null) {
        result = null;
        return false;
      }

      if (t == typeof(DirectoryInfo)) {
        var di = new DirectoryInfo(value);
        result = di;
        return true;
      }

      result = null;
      return false;
    }
  }
}