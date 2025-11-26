using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartLibraryAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    StudentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Author = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ISBN = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "catalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LoanId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    PaidDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RegisteredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserType = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    EmployeeId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StudentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "book_catalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    CatalogId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book_catalogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_book_catalogs_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_book_catalogs_catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsReturned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_loans_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_loans_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reservations_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservations_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_Email",
                table: "accounts",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_Username",
                table: "accounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_book_catalogs_BookId_CatalogId",
                table: "book_catalogs",
                columns: new[] { "BookId", "CatalogId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_book_catalogs_CatalogId",
                table: "book_catalogs",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_books_ISBN",
                table: "books",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_books_Title",
                table: "books",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_fines_LoanId",
                table: "fines",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_fines_UserId",
                table: "fines",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_loans_BookId",
                table: "loans",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_loans_UserId",
                table: "loans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_BookId",
                table: "reservations",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_UserId",
                table: "reservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_users_EmployeeId",
                table: "users",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_users_StudentId",
                table: "users",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "book_catalogs");

            migrationBuilder.DropTable(
                name: "fines");

            migrationBuilder.DropTable(
                name: "loans");

            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "catalogs");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
