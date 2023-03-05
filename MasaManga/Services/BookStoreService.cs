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
            return _bookStoreDbContext.Books
                //.Include(x=>x.Sections)
                //    .ThenInclude(s=>s.Pics)
                .ToList();
        }

        public async Task BeginDownload(int bookId, Action<int> onProgress)
        {
            var book = _bookStoreDbContext.Books
                .Include(x => x.Sections)
                    .ThenInclude(x => x.Pics)
                .FirstOrDefault(x=>x.Id == bookId);
            string bookPath = $"wwwroot/store/{book.Title}";
            book.IsDownloading = true;
            var source = SourceStore.SourceSites.FirstOrDefault(x=>x.Title == book.SourceTitle);
            if (!book.IsFilled)
            {
                source.FulfilBook(book);
                book.IsFilled = true;
                _bookStoreDbContext.SaveChanges();
            }
            double progress = 0.0d;
            foreach(var section in book.Sections.OrderBy(s=>s.Index))
            {
                if(!section.IsDownloaded)
                {
                    if (!section.IsFilled)
                    {
                        source.FulfilSection(section);
                        section.IsFilled = true;
                        _bookStoreDbContext.SaveChanges();
                    }

                    var dirPath = Path.Combine(bookPath, section.Title);
                    Directory.CreateDirectory(dirPath);
                    Console.WriteLine($"开始下载：{section.Title}");
                    var picsQueue = new ConcurrentQueue<BookPic>(section.Pics);
                    await FileDownloader.DownloadAsync( 
                        () =>
                        {
                            if(picsQueue.TryDequeue(out var pic))
                            {
                                string filename = Path.Combine(dirPath, pic.FileName);
                                return new FileDownloader.File { Filename = filename, Url = pic.Url, Target = pic };
                            }
                            return null;
                        },
                        (b, isOk) =>
                        {
                            if(isOk)
                            {
                                if(b.Target is BookPic pic)
                                {
                                    pic.IsDownloaded = true;
                                    section.DownloadPic += 1;
                                    _bookStoreDbContext.SaveChanges();
                                    
                                    progress += 100.0d / book.TotalSection / section.TotalPic * section.Index;
                                    onProgress((int)progress);
                                }
                            }
                        });
                    if(section.DownloadPic == section.TotalPic)
                    {
                        section.IsDownloaded = true;
                        _bookStoreDbContext.SaveChanges();
                    }
                }
                progress = (int)(100.0d / book.TotalSection * section.Index);
                onProgress((int)progress);
            }
            book.IsDownloading = false;
        }
    }
}
