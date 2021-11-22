using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WinCopies.Util;

namespace EDQuickLauncher.Game {

  /// <summary>
  ///
  /// </summary>
  public partial class Headlines {
    public List<News> News { get; set; }
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
      var html = @"https://community.elitedangerous.com";
      var startDate = DateTime.Today.AddYears(1286);

      var client = new HttpClient();
      var headlines = new Headlines {
        News = new List<News>()
      };
      using (var response = await client.GetAsync(html)) {
        var document = new HtmlDocument();
        document.Load(await response.Content.ReadAsStreamAsync());
        var nodes = document.DocumentNode.SelectNodes("//div[@class=\"article\"]");
        var TempNews = new List<News>();
        List<HtmlNode> nodeList = nodes.ToList(0, 9);
        nodeList.ForEach(delegate (HtmlNode node) {
          TempNews.AddIfNotContains(new News {
            Title = node.ChildNodes[1].ChildNodes[0].InnerText.TrimStart(),
            Date = DateTime.Parse(Converter.FirstCharToUpper(node.ChildNodes[3].ChildNodes[0].InnerText)),
            Url = $"{html}{node.ChildNodes[1].ChildNodes[0].GetAttributeValue("href", "")}",
            Id = node.ChildNodes[1].ChildNodes[0].GetAttributeValue("href", "").Substring(12)
          });
        });
        headlines.News = TempNews;
      }

      return headlines;
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