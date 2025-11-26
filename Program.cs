using Microsoft.EntityFrameworkCore;
using SmartLibraryAPI.Data;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext for PostgreSQL
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Register repositories and services
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IFineService, FineService>();
builder.Services.AddScoped<IReservationService, ReservationService>(); // Keep only this
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
