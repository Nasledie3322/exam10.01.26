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
    public async Task<Response<string>> AddAsync(AddBookDto bookDto)
    {
        return await _bookService.AddBookAsync(bookDto);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateAsync(UpdateBookDto bookDto)
    {
        return await _bookService.UpdateBookAsync(bookDto);
    }

    [HttpDelete("{bookId}")]
    public async Task<Response<string>> DeleteAsync(int bookId)
    {
        return await _bookService.DeleteBookAsync(bookId);
    }
}
