using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using HtmlAgilityPack;
using Castle.DynamicProxy.Generators.Emitters;
using System.Text.Json.Nodes;
using System.Windows.Automation;
using System.Collections.Generic;
using WinCopies.Util;
using WinCopies.Linq;
using static SharedMemory.CircularBuffer;

namespace EDQuickLauncher.Game {

  /// <summary>
  ///
  /// </summary>
  public partial class Headlines {
    [JsonProperty("news")] public List<News> News { get; set; }
  }

  public class News {
    public string Title { get; set; }

    private DateTime date;

    public DateTime Date {
      get {
        return date;
      }

      set {
        date = value.AddYears(-1286);
      }
    }

    public string Url { get; set; }
    public string Id { get; set; }
  }

  public partial class Headlines {

    public static async Task<Headlines> Get() {
      var tcs = new TaskCompletionSource<Headlines>();
      var html = @"https://community.elitedangerous.com/";
      var startDate = DateTime.Today.AddYears(1286);

      var web = new HtmlWeb().Load(html);
      var headlines = new Headlines {
        News = new List<News>()
      };
      var nodes = web.DocumentNode.SelectNodes("//div[@class=\"article\"]");
      var TempNews = new List<News>();
      List<HtmlNode> nodeList = nodes.ToList(0, 9);
      nodeList.ForEach(delegate (HtmlNode node) {
        TempNews.AddIfNotContains(new News {
          Title = node.ChildNodes[1].ChildNodes[0].InnerText.TrimStart(),
          Date = DateTime.Parse(Converter.FirstCharToUpper(node.ChildNodes[3].ChildNodes[0].InnerText)),
          Url = node.ChildNodes[1].ChildNodes[0].GetAttributeValue("href", ""),
          Id = node.ChildNodes[1].ChildNodes[0].GetAttributeValue("href", "").Substring(12)
        });
      });
      headlines.News = TempNews;
      var test = headlines;

      return test;
    }
  }

  internal static class Converter {

    public static string FirstCharToUpper(this string input) =>
        input switch {
          null => throw new ArgumentNullException(nameof(input)),
          "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
          _ => string.Concat(input.Substring(0, 4), string.Concat(input[4].ToString().ToLower(), string.Concat(input[5].ToString().ToLower(), input.AsSpan(6))))
        };
  }
}