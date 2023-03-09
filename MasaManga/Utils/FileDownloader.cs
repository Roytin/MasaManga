namespace MasaManga.Utils
{
    public class FileDownloader
    {
        HttpClient _webClient;
        public FileDownloader()
        {
            _webClient = new HttpClient();
            _webClient.Timeout = TimeSpan.FromMinutes(1);
        }

        public async Task DownloadAsync(string url, string fileName)
        {
            int retry = 3;
            bool done = File.Exists(fileName);
            Exception ex = null;
            while(!done && retry > 0)
            {
                try
                {
                    retry --;
                    using var stream = await _webClient.GetStreamAsync(new Uri(url));
                    using var fileStream = File.Open(fileName, FileMode.OpenOrCreate);
                    await stream.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                    done = true;
                    ex = null;
                }
                catch (Exception exception)
                {
                    ex = exception;
                }
            }
            if (ex != null)
                throw ex;
        }
    }
}
