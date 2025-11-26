using Microsoft.AspNetCore.Mvc;
using SmartLibraryAPI.Interfaces;
using SmartLibraryAPI.Models;
using SmartLibraryAPI.DTOs.Request;
using SmartLibraryAPI.DTOs.Response;

namespace SmartLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Book>>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            return Ok(ApiResponse<List<Book>>.SuccessResponse("Books retrieved successfully", books));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Book>>> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null)
                return NotFound(ApiResponse<Book>.ErrorResponse("Book not found"));

            return Ok(ApiResponse<Book>.SuccessResponse("Book retrieved successfully", book));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Book>>> AddBook([FromBody] AddBookRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<Book>.ErrorResponse("Invalid input data"));

            var book = new Book(request.Title, request.Author, request.ISBN ?? "");
            await _bookRepository.AddBookAsync(book);

            return CreatedAtAction(
                nameof(GetBookById),
                new { id = book.Id },
                ApiResponse<Book>.SuccessResponse("Book added successfully", book));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateBook(int id, [FromBody] AddBookRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input data"));

            var existing = await _bookRepository.GetBookByIdAsync(id);
            if (existing == null)
                return NotFound(ApiResponse<object>.ErrorResponse("Book not found"));

            existing.Title = request.Title;
            existing.Author = request.Author;
            existing.ISBN = request.ISBN ?? "";

            await _bookRepository.UpdateBookAsync(existing);
            return Ok(ApiResponse<object>.SuccessResponse("Book updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteBook(int id)
        {
            var existing = await _bookRepository.GetBookByIdAsync(id);
            if (existing == null)
                return NotFound(ApiResponse<object>.ErrorResponse("Book not found"));

            await _bookRepository.DeleteBookAsync(id);
            return Ok(ApiResponse<object>.SuccessResponse("Book deleted successfully"));
        }
    }
}