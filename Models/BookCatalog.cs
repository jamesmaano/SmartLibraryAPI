namespace SmartLibraryAPI.Models
{
    public partial class BookCatalog
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int CatalogId { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Catalog Catalog { get; set; } = null!;

        public BookCatalog() { }

        // Constructor matching your CatalogService calls
        public BookCatalog(int id, int bookId, int catalogId, Catalog catalog)
        {
            Id = id;
            BookId = bookId;
            CatalogId = catalogId;
            Catalog = catalog;
        }
    }
}
