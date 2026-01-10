using Microsoft.AspNetCore.Mvc;
using WebApi.Responses;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<List<Book>> GetBooksAsync()
    {
        return await _bookService.GetBookAsync();
    }

    [HttpGet("{bookId}")]
    public async Task<Response<Book?>> GetByIdAsync(int bookId)
    {
        return await _bookService.GetBookByIdAsync(bookId);
    }

    [HttpPost]
    public async Task<Response<string>> AddAsync(Book book)
    {
        return await _bookService.AddBookAsync(book);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateAsync(Book book)
    {
        return await _bookService.UpdateBookAsync(book);
    }

    [HttpDelete("{bookId}")]
    public async Task<Response<string>> DeleteAsync(int bookId)
    {
        return await _bookService.DeleteBookAsync(bookId);
    }
}
