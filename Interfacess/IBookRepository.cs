namespace MauiApp1.Interfaces
{
    using MauiApp1.Models;

    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book?> GetBookByTitleAsync(string title);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
    }
}