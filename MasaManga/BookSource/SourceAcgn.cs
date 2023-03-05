using HtmlAgilityPack;
using MasaManga.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasaManga.BookSource
{
    public class SourceAcgn: IBookSource
    {
        public string Title => "动漫戏说";

        public string Url => "https://comic.acgn.cc/";

        public void FulfilBook(Book book)
        {
            List<BookSection> sections = new List<BookSection>();
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(book.IndexUrl);
            var coverImgNode = htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[1]/div[3]/div[4]/dl/dd[1]/a/img");
            book.CoverUrl = coverImgNode.GetAttributeValue<string>("src", "");
            var liNodes = htmlDoc.DocumentNode.SelectNodes("/html/body/div[2]/div/div[3]/div[1]/div[3]/div[7]/ul/li/a");
            foreach (var pageNode in liNodes)
            {
                var title = pageNode.GetAttributeValue<string>("title", "");
                var href = pageNode.GetAttributeValue<string>("href", "");
                if (!href.StartsWith("http"))
                {
                    Uri u1 = new Uri(book.IndexUrl, UriKind.Absolute);
                    Uri u2 = new Uri(href, UriKind.Relative);
                    var uri = new Uri(u1, u2);
                    href = uri.ToString();
                }
                sections.Add(new BookSection { Url = href, Title = title });
            }
            sections.Reverse();
            for(int i = 0; i< sections.Count; i++)
            {
                sections[i].Index = i + 1;
            }
            book.Sections = sections;
            book.TotalSection = sections.Count;
            book.IsFilled = true;
        }

        public void FulfilSection(BookSection section)
        {
            List<BookPic> pics = new List<BookPic>();
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(section.Url);
            var liNodes = htmlDoc.DocumentNode.SelectNodes("/html/body/div[2]/div/div[2]/div/table/tbody/tr/td/div[2]/div/div").Where(x => x.HasClass("pic"));
            int i = 0;
            foreach (var pageNode in liNodes)
            {
                var src = pageNode.GetAttributeValue<string>("_src", "");
                ++i;
                int lastPointIndex = src.LastIndexOf('.');
                string suffix = src.Substring(lastPointIndex);
                pics.Add(new BookPic { Index = i, Url = src, FileName = i + suffix });
            }
            section.TotalPic = pics.Count;
            section.Pics = pics;
            section.IsFilled = true;
            section.IsDownloaded = false;
        }
    }
}
