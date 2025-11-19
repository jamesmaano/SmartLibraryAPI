namespace MauiApp1.Models
{
    public class BookCatalog
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int CatalogId { get; set; }
        public Book Book { get; set; }
        public Catalog Catalog { get; set; }

        public BookCatalog(int bookId, int catalogId, Book book, Catalog catalog)
        {
            BookId = bookId;
            CatalogId = catalogId;
            Book = book;
            Catalog = catalog;
        }
    }
}