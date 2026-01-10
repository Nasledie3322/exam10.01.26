using Microsoft.AspNetCore.Mvc;
using WebApi.Responses;
using WebApi.Services;

namespace WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthorController(IAuthorService authorService) : ControllerBase
{
    [HttpGet]
    public async Task<List<Author>> GetAuthorsAsync()
    {
     return await authorService.GetAuthorAsync();
    }
     [HttpGet("{authorId}")]
    public async Task<Response<Author>> GetByIdAsync(int authorId)
    {
        return await authorService.GetAuthorByIdAsync(authorId);
    }

    [HttpPost]
    public async Task<Response<string>> AddAsync(Author author)
    {
        return await authorService.AddAuthorAsync(author);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateAsync(Author author)
    {
        return await authorService.UpdateAuthorAsync(author);
    }

    [HttpDelete("{attributeId}")]
    public async Task<Response<string>> DeleteAsync(int authorId)
    {
        return await authorService.DeleteAuthorAsync(authorId);
    }
}

