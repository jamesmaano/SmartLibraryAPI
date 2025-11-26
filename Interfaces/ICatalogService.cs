using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Interfaces
{
    public interface ICatalogService
    {
        Task<List<Catalog>> GetAllCatalogsAsync();
        Task<Catalog?> GetCatalogByIdAsync(int id);
        Task<bool> AddCatalogAsync(Catalog catalog);
        Task<bool> AddBookToCatalogAsync(int bookId, int catalogId);
        Task<List<Book>> GetBooksByCatalogAsync(int catalogId);
        Task<bool> RemoveBookFromCatalogAsync(int bookId, int catalogId);
    }
}