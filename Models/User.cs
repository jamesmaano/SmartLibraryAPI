using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartLibraryAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Student or Faculty identifiers
        public string? StudentId { get; set; }
        public string? EmployeeId { get; set; }

        // NEW — Required by EF
        public string UserType { get; set; } = string.Empty;

        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

        // EF navigation properties
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        // Virtual methods
        public virtual int BorrowLimit => 0;
        public virtual int ReturnDays => 0;

        public User() { }

        public User(string name, string email, string phoneNumber)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
