using System.ComponentModel.DataAnnotations.Schema;

namespace MasaManga.Data;

public class Book
{
    public int Id { get; set; }
    public string SourceTitle { get; set; }
    public string Title { get; set; }
    /// <summary>
    /// 索引页地址
    /// </summary>
    public string IndexUrl { get; set; }
    public string CoverUrl { get; set; }
    public string Cover { get; set; }

    public bool IsDownloading { get; set; }
    /// <summary>
    /// 下载进度（0-100）
    /// </summary>
    public int TotalPage { get; set; }
    public int DownloadPage { get; set; }


    public List<BookSection> Sections { get; set; } = new List<BookSection>();
}

public class BookSection
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public List<BookPic> Pics { get; set; } = new List<BookPic>();
}

public class BookPic
{
    public int Id { get; set; }

    public int SectionIndex { get; set; }
    public int Index { get; set; }
    public string Url { get; set; }
    public string FileName { get; set; }
    public bool IsDownloaded { get; set; }

    public DateTime DownloadTime { get; set; }
}