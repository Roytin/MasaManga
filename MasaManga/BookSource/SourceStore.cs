namespace MasaManga.BookSource
{
    public static class SourceStore
    {

        public readonly static List<IBookSource> SourceSites = new List<IBookSource>()
        {
            new SourceAcgn(),
            new SourceMhg(),
        };
    }
}
