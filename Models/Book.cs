using System;
using System.Collections.Generic;

namespace SmartLibraryAPI.Models
{
    public partial class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<BookCatalog> BookCatalogs { get; set; } = new List<BookCatalog>();
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public Book()
        {
            IsAvailable = true;
            CreatedDate = DateTime.Now;
        }

        public Book(string title, string author, string isbn)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            IsAvailable = true;
            CreatedDate = DateTime.Now;
        }
    }
}
