using WebApi.Responses;

namespace WebApi.Services;

public interface IBookService
{
    Task<Response<string>> AddBookAsync(Book book);

    Task<Response<string>> DeleteBookAsync(int bookId);

    Task<List<Book>> GetBookAsync();

    Task<Response<Book?>> GetBookByIdAsync(int bookId);

    Task<Response<string>> UpdateBookAsync(Book book);
}
