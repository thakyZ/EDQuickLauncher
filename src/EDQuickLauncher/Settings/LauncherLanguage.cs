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
    private static Dictionary<LauncherLanguage, string> GetLangCodes()
    {
      // MUST match LauncherLanguage enum
      return new Dictionary<LauncherLanguage, string> { //{ LauncherLanguage.Japanese, "ja" },
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

    public static string GetLocalizationCode(this LauncherLanguage? language) => GetLangCodes()[language ?? LauncherLanguage.English]; // Default localization language

    public static bool IsDefault(this LauncherLanguage? language) => language == null || language == LauncherLanguage.English;

    public static LauncherLanguage GetLangFromTwoLetterISO(this LauncherLanguage? language, string code)
    {
      foreach (KeyValuePair<LauncherLanguage, string> langCode in GetLangCodes())
      {
        if (langCode.Value == code)
        {
          return langCode.Key;
        }
      }
      return LauncherLanguage.English; // Default language
    }
  }
}