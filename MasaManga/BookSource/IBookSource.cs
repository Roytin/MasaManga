using HtmlAgilityPack;
using MasaManga.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MasaManga.BookSource
{
    public interface IBookSource
    {
        string Title { get; }
        string Url { get; }

        void FulfilBook(Book book);

        void FulfilSection(BookSection section);
    }
}
