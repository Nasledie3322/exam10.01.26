using Microsoft.AspNetCore.Mvc;
using WebApi.Responses;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<List<Author>> GetAuthorsAsync()
    {
        return await _authorService.GetAuthorAsync();
    }

    [HttpGet("{authorId}")]
    public async Task<Response<Author?>> GetByIdAsync(int authorId)
    {
        return await _authorService.GetAuthorByIdAsync(authorId);
    }

    [HttpPost]
    public async Task<Response<string>> AddAsync(Author author)
    {
        return await _authorService.AddAuthorAsync(author);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateAsync(Author author)
    {
        return await _authorService.UpdateAuthorAsync(author);
    }

    [HttpDelete("{authorId}")]
    public async Task<Response<string>> DeleteAsync(int authorId)
    {
        return await _authorService.DeleteAuthorAsync(authorId);
    }
}
