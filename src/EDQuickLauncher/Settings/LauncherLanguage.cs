/* XIVQuickLauncher - Modified Code
 * Copyright (C) 2021  goatcorp
 */
using System.Collections.Generic;

namespace EDQuickLauncher.Settings {
  public enum LauncherLanguage {
    //Japanese,
    English,
    //German,
    //French,
    //Italian,
    //Spanish,
    //Portuguese,
    //Korean,
    //Norwegian,
    //Russian,
    //SimplifiedChinese
  }

  public static class LauncherLanguageExtensions {

    private static Dictionary<LauncherLanguage, string> GetLangCodes() {
      // MUST match LauncherLanguage enum
      return new Dictionary<LauncherLanguage, string> {
        //{ LauncherLanguage.Japanese, "ja" },
        { LauncherLanguage.English, "en" },
        //{ LauncherLanguage.German, "de" },
        //{ LauncherLanguage.French, "fr" },
        //{ LauncherLanguage.Italian, "it" },
        //{ LauncherLanguage.Spanish, "es" },
        //{ LauncherLanguage.Portuguese, "pt" },
        //{ LauncherLanguage.Korean, "ko" },
        //{ LauncherLanguage.Norwegian, "no" },
        //{ LauncherLanguage.Russian, "ru" },
        //{ LauncherLanguage.SimplifiedChinese, "zh" },
      };
    }

    public static string ToFriendlyString(this LauncherLanguage? language) {
      switch (language) {
        //case LauncherLanguage.Japanese:
        //  return "日本語";
        case LauncherLanguage.English:
          return "English";
        //case LauncherLanguage.German:
        //  return "Deutsch";
        //case LauncherLanguage.French:
        //  return "Français";
        //case LauncherLanguage.Italian:
        //  return "Italiano";
        //case LauncherLanguage.Spanish:
        //  return "Español";
        //case LauncherLanguage.Portuguese:
        //  return "Português";
        //case LauncherLanguage.Korean:
        //  return "한국어";
        //case LauncherLanguage.Norwegian:
        //  return "Norsk";
        //case LauncherLanguage.Russian:
        //  return "русский";
        //case LauncherLanguage.SimplifiedChinese:
        //  return "简体中文";
        default:
          return "Error";
      }
    }

    public static string GetLocalizationCode(this LauncherLanguage? language) => GetLangCodes()[language ?? LauncherLanguage.English]; // Default localization language

    public static bool IsDefault(this LauncherLanguage? language) => language == null || language == LauncherLanguage.English;

    public static LauncherLanguage GetLangFromTwoLetterISO(this LauncherLanguage? language, string code) {
      foreach (KeyValuePair<LauncherLanguage, string> langCode in GetLangCodes()) {
        if (langCode.Value == code) {
          return langCode.Key;
        }
      }
      return LauncherLanguage.English; // Default language
    }
  }
}