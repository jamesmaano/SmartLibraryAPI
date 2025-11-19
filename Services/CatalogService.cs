using MauiApp1.Interfaces;
using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly List<Catalog> _catalogs = new();
        private readonly List<BookCatalog> _bookCatalogs = new();
        private readonly IBookRepository _bookRepository;
        private int _nextCatalogId = 1;
        private int _nextBookCatalogId = 1;

        public CatalogService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
            InitializeDefaultCatalogs();
        }

        private void InitializeDefaultCatalogs()
        {
            _catalogs.Add(new Catalog("Fiction", "Fictional books and novels") { Id = _nextCatalogId++ });
            _catalogs.Add(new Catalog("Non-Fiction", "Educational and informative books") { Id = _nextCatalogId++ });
            _catalogs.Add(new Catalog("Science", "Science and technology books") { Id = _nextCatalogId++ });
            _catalogs.Add(new Catalog("History", "Historical books and references") { Id = _nextCatalogId++ });
        }

        public Task<List<Catalog>> GetAllCatalogsAsync()
        {
            return Task.FromResult(_catalogs);
        }

        public Task<Catalog?> GetCatalogByIdAsync(int id)
        {
            var catalog = _catalogs.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(catalog);
        }

        public Task<bool> AddCatalogAsync(Catalog catalog)
        {
            catalog.Id = _nextCatalogId++;
            _catalogs.Add(catalog);
            return Task.FromResult(true);
        }

        public async Task<bool> AddBookToCatalogAsync(int bookId, int catalogId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            var catalog = await GetCatalogByIdAsync(catalogId);

            if (book == null || catalog == null)
                return false;

            var existingEntry = _bookCatalogs
                .FirstOrDefault(bc => bc.BookId == bookId && bc.CatalogId == catalogId);

            if (existingEntry != null)
                return false;

            var bookCatalog = new BookCatalog(bookId, catalogId, book, catalog)
            {
                Id = _nextBookCatalogId++
            };

            _bookCatalogs.Add(bookCatalog);
            return true;
        }

        public async Task<List<Book>> GetBooksByCatalogAsync(int catalogId)
        {
            var bookIds = _bookCatalogs
                .Where(bc => bc.CatalogId == catalogId)
                .Select(bc => bc.BookId)
                .ToList();

            var allBooks = await _bookRepository.GetAllBooksAsync();
            return allBooks.Where(b => bookIds.Contains(b.Id)).ToList();
        }

        public Task<bool> RemoveBookFromCatalogAsync(int bookId, int catalogId)
        {
            var bookCatalog = _bookCatalogs
                .FirstOrDefault(bc => bc.BookId == bookId && bc.CatalogId == catalogId);

            if (bookCatalog == null)
                return Task.FromResult(false);

            _bookCatalogs.Remove(bookCatalog);
            return Task.FromResult(true);
        }
    }
}