using WebApi.Responses;

namespace WebApi.Services;

public interface IAuthorService
{
    Task<Response<string>> AddAuthorAsync(Author author);

    Task<Response<string>> DeleteAuthorAsync(int authorId);

    Task<List<Author>> GetAuthorAsync();

    Task<Response<Author?>> GetAuthorByIdAsync(int authorId);

    Task<Response<string>> UpdateAuthorAsync(Author author);
}
