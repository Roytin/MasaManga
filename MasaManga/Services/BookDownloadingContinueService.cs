using MasaManga.Data;
using MasaManga.Utils;
using Microsoft.EntityFrameworkCore;

namespace MasaManga.Services
{
    public class BookDownloadingContinueService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public BookDownloadingContinueService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private Task task;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            List<int> bookIds;
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContextA = scope.ServiceProvider.GetService<BookStoreDbContext>();
                bookIds = dbContextA.Books
                    .Where(x => x.IsDownloading)
                    .Select(x=>x.Id)
                    .ToList();
            }
            task = Task.Run(() =>
            {
                Parallel.ForEach(bookIds, async (bookId) =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetService<BookStoreDbContext>();
                    var book = dbContext.Books
                        .Include(x => x.Sections)
                            .ThenInclude(x => x.Pics)
                        .First(x=>x.Id == bookId);
                    var downloaded = book.Sections.Sum(x => x.Pics.Count(p => p.IsDownloaded));
                    if (downloaded != book.DownloadPage)
                    {
                        book.DownloadPage = downloaded;
                        dbContext.SaveChanges();
                    }
                    string bookPath = $"wwwroot/store/{book.Title}";
                    var downloader = new FileDownloader();
                    foreach (var section in book.Sections.OrderBy(s => s.Index))
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
                            dbContext.SaveChanges();
                        }
                    }
                });
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if(task != null)
            {
                task.Dispose();
            }
            return Task.CompletedTask;
        }
    }
}
