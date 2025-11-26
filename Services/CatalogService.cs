using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;

namespace SmartLibraryAPI.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly LibraryDbContext _context;
        private readonly IBookRepository _bookRepository;

        public CatalogService(LibraryDbContext context, IBookRepository bookRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
        }

        public async Task<List<Catalog>> GetAllCatalogsAsync()
        {
            return await _context.Catalogs.ToListAsync();
        }

        public async Task<Catalog?> GetCatalogByIdAsync(int id)
        {
            return await _context.Catalogs.FindAsync(id);
        }

        public async Task<bool> AddCatalogAsync(Catalog catalog)
        {
            _context.Catalogs.Add(catalog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddBookToCatalogAsync(int bookId, int catalogId)
        {
            // Check if book exists
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null)
                return false;

            // Check if catalog exists
            var catalog = await GetCatalogByIdAsync(catalogId);
            if (catalog == null)
                return false;

            // Check if relationship already exists
            var exists = await _context.BookCatalogs
                .AnyAsync(bc => bc.BookId == bookId && bc.CatalogId == catalogId);

            if (exists)
                return false;

            // Add to junction table
            var bookCatalog = new BookCatalog(bookId, catalogId, book, catalog);
            _context.BookCatalogs.Add(bookCatalog);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Book>> GetBooksByCatalogAsync(int catalogId)
        {
            var bookIds = await _context.BookCatalogs
                .Where(bc => bc.CatalogId == catalogId)
                .Select(bc => bc.BookId)
                .ToListAsync();

            var books = await _context.Books
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();

            return books;
        }

        public async Task<bool> RemoveBookFromCatalogAsync(int bookId, int catalogId)
        {
            var bookCatalog = await _context.BookCatalogs
                .FirstOrDefaultAsync(bc => bc.BookId == bookId && bc.CatalogId == catalogId);

            if (bookCatalog == null)
                return false;

            _context.BookCatalogs.Remove(bookCatalog);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}