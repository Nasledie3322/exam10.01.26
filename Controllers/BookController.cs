using Microsoft.AspNetCore.Mvc;
using WebApi.Responses;
using WebApi.Services;

namespace WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BookController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<List<Book>> GetBooksAsync()
    {
     return await bookService.GetBookAsync();
    }
     [HttpGet("{bookId}")]
    public async Task<Response<Book>> GetByIdAsync(int bookId)
    {
        return await bookService.GetBookByIdAsync(bookId);
    }

    [HttpPost]
    public async Task<Response<string>> AddAsync(Book book)
    {
        return await bookService.AddBookAsync(book);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateAsync(Book book)
    {
        return await bookService.UpdateBookAsync(book);
    }

    [HttpDelete("{attributeId}")]
    public async Task<Response<string>> DeleteAsync(int bookId)
    {
        return await bookService.DeleteBookAsync(bookId);
    }
}

