using System;
using System.Collections.Generic;

namespace SmartLibraryAPI.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string StudentId { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool IsActive { get; set; }

    // Parameterless ctor (keeps EF happy)
    public Account()
    {
    }

    // New: a 6-argument ctor to match the usage that caused CS1729
    // Signature: (username, passwordHash, role, email, fullName, studentId)
    public Account(
        string username,
        string passwordHash,
        string role,
        string email,
        string fullName,
        string studentId)
    {
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
        Email = email;
        FullName = fullName;
        StudentId = studentId;

        // sensible defaults
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsActive = true;
    }
}
