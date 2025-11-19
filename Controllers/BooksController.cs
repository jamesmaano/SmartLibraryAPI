using Microsoft.AspNetCore.Mvc;
using MauiApp1.Interfaces;
using MauiApp1.Models;

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
        public async Task<ActionResult<List<Book>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookByIdAsync(id);
            if (book == null) return NotFound(new { message = "Book not found" });
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult> AddBook([FromBody] Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
                return BadRequest(new { message = "Title and Author are required" });

            await _bookRepository.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, [FromBody] Book book)
        {
            var existing = await _bookRepository.GetBookByIdAsync(id);
            if (existing == null) return NotFound(new { message = "Book not found" });

            book.Id = id;
            await _bookRepository.UpdateBookAsync(book);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var existing = await _bookRepository.GetBookByIdAsync(id);
            if (existing == null) return NotFound(new { message = "Book not found" });

            await _bookRepository.DeleteBookAsync(id);
            return Ok(new { message = "Book deleted successfully" });
        }
    }
}
