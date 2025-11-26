using System;
using System.Collections.Generic;

namespace SmartLibraryAPI.Models;

public partial class Catalog
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual ICollection<BookCatalog> BookCatalogs { get; set; } = new List<BookCatalog>();
}
