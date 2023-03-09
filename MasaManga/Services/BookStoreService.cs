using System;
using System.Collections.Concurrent;
using System.IO;
using MasaManga.BookSource;
using MasaManga.Data;
using MasaManga.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace MasaManga.Services
{
    public class BookStoreService
    {
        private readonly BookStoreDbContext _bookStoreDbContext;

        public BookStoreService(BookStoreDbContext bookStoreDbContext)
        {
            _bookStoreDbContext = bookStoreDbContext;
        }

        public List<Book> GetBooks()
        {
            var books = _bookStoreDbContext.Books.AsNoTracking().ToList();
            return books;
        }

        public async Task<(bool ok, string err)> AddBook(string indexUrl)
        {
            try
            {
                var source = SourceStore.SourceSites.FirstOrDefault(x => indexUrl.StartsWith(x.Url));
                if (source == null)
                    return (false, "源不存在");
                if(_bookStoreDbContext.Books.Any(x=>x.IndexUrl == indexUrl))
                    return (false, "书已添加");
                var book = new Book() { IndexUrl = indexUrl, SourceTitle = source.Title };
                source.FulfilBook(book);
                Parallel.ForEach(book.Sections, section =>
                {
                    source.FulfilSection(section);
                });
                book.TotalPage = book.Sections.Sum(x => x.Pics.Count);
                _bookStoreDbContext.Books.Add(book);
                await _bookStoreDbContext.SaveChangesAsync();
                book.Cover = $"wwwroot/store/{book.Title}/cover.jpg";
                var downloader = new FileDownloader();
                await downloader.DownloadAsync(book.CoverUrl, book.Cover);
                return (true, "");
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task BeginDownload(int bookId)
        {
            var book = _bookStoreDbContext.Books
                .Include(x => x.Sections)
                    .ThenInclude(x => x.Pics)
                .FirstOrDefault(x=>x.Id == bookId);
            if (book == null)
                return;
            if (book.IsDownloading)
                return;
            string bookPath = $"wwwroot/store/{book.Title}";
            book.IsDownloading = true;
            book.DownloadPage = book.Sections.Sum(x => x.Pics.Count(p => p.IsDownloaded));
            _bookStoreDbContext.SaveChanges();
            //开启下载
            //todo: 增加多线程下载
            var downloader = new FileDownloader();
            foreach (var section in book.Sections.OrderBy(s=>s.Index))
            {
                var dirPath = Path.Combine(bookPath, section.Title);
                Directory.CreateDirectory(dirPath);
                foreach (var pic in section.Pics)
                {
                    if (pic.IsDownloaded)
                        continue;
                    string filename = Path.Combine(dirPath, pic.FileName);
                    Console.WriteLine($"开始下载：{section.Title}");
                    await downloader.DownloadAsync(pic.Url, filename);
                    pic.IsDownloaded = true;
                    book.DownloadPage++;
                    _bookStoreDbContext.SaveChanges();
                }
            }
            book.DownloadPage = book.Sections.Sum(x => x.Pics.Count(p => p.IsDownloaded));
            book.IsDownloading = false;
            _bookStoreDbContext.SaveChanges();
        }
    }
}
