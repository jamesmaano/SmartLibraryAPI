using MauiApp1.Services;
using MauiApp1.Interfaces;
using MauiApp1.Interfaces;
using MauiApp1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IReservationRepository, ReservationRepository>();

builder.Services.AddSingleton<ILoanService, LoanService>();
builder.Services.AddSingleton<IFineService, FineService>();
builder.Services.AddSingleton<IReservationService, ReservationService>();
builder.Services.AddSingleton<ICatalogService, CatalogService>();
builder.Services.AddSingleton<IReportService, ReportService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<IAuthService, AuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();