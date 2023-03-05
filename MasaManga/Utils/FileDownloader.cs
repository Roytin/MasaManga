namespace MasaManga.Utils
{
    public static class FileDownloader
    {
        public class File
        {
            public object Target { get; set; }
            public string Url { get; set; }
            public string Filename { get; set; }
        }

        public static async Task DownloadAsync(int wokers, Func<File> getFile, DownloadEnd onDownloadEnd)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < wokers; i++)
            {
                var t = DownloadAsync(getFile, onDownloadEnd);
                tasks.Add(t);
            }
            await Task.WhenAll(tasks);
        }

        public static async Task DownloadAsync(Func<File> getFile, DownloadEnd onDownloadEnd)
        {
            HttpClient webClient = new HttpClient();
            webClient.Timeout = TimeSpan.FromMinutes(1);
            var f = getFile();
            while (f != null)
            {
                try
                {
                    if (!System.IO.File.Exists(f.Filename))
                    {
                        using var stream = await webClient.GetStreamAsync(new Uri(f.Url));
                        using var fileStream = System.IO.File.Open(f.Filename, FileMode.OpenOrCreate);
                        await stream.CopyToAsync(fileStream);
                        await fileStream.FlushAsync();
                    }
                    onDownloadEnd(f, true);
                    f = getFile();
                }
                catch (Exception ex)
                {
                    onDownloadEnd(f, false);
                }
            }
        }
        public delegate void DownloadEnd(File file, bool isOk);
    }
}
