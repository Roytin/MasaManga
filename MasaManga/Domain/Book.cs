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

    public int TotalSection { get; set; }
    public int DownloadSection { get; set; }

    public string Cover { get; set; }
    public bool IsFilled { get; set; }

    [NotMapped]
    public bool IsDownloading { get; set; }
    /// <summary>
    /// 下载进度（0-100）
    /// </summary>
    [NotMapped]
    public int DownloadProgress { get; set; }
    

    public List<BookSection> Sections { get; set; }
}

public class BookSection
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public bool IsFilled { get; set; }
    public bool IsDownloaded { get; set; }
    public int TotalPic { get; set; }
    public int DownloadPic { get; set; }
    public List<BookPic> Pics { get; set; }
}

public class BookPic
{
    public int Id { get; set; }
    public int Index { get; set; }
    public string Url { get; set; }
    public string FileName { get; set; }
    public bool IsDownloaded { get; set; }

    public DateTime DownloadTime { get; set; }
}