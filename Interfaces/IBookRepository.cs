using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
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