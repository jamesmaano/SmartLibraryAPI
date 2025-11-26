using System;
using System.Collections.Generic;

namespace SmartLibraryAPI.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int BookId { get; set; }

    public DateTime ReservationDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
