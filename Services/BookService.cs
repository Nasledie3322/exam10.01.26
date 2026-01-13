using System.Net;
using Dapper;
using WebApi.Data;
using WebApi.Responses;
using Microsoft.Extensions.Logging;

namespace WebApi.Services;

public class BookService(ApplicationDbContext applicationDbContext, ILogger<BookService> logger) : IBookService
{
    private readonly ApplicationDbContext _dbContext = applicationDbContext;
    private readonly ILogger<BookService> _logger = logger;

    public async Task<Response<string>> AddBookAsync(AddBookDto bookDto)
    {
        _logger.LogInformation("Starting AddBookAsync");

        using var conn = _dbContext.Connection();
        var query = "insert into books(title, publishedyear, genre, authorid) values(@title, @publishedYear, @genre, @AuthorId)";

        var res = await conn.ExecuteAsync(query, bookDto);

        if (res == 0)
        {
            _logger.LogWarning("AddBookAsync failed");
            return new Response<string>(HttpStatusCode.InternalServerError, "Something went wrong!");
        }

        _logger.LogInformation("AddBookAsync succeeded");
        return new Response<string>(HttpStatusCode.OK, "Book added successfully!");
    }

    public async Task<Response<string>> DeleteBookAsync(int bookId)
    {
        _logger.LogInformation("Starting DeleteBookAsync");

        using var context = _dbContext.Connection();
        var query = "delete from books where id = @id";

        var result = await context.ExecuteAsync(query, new { id = bookId });

        if (result == 0)
        {
            _logger.LogWarning("DeleteBookAsync failed");
            return new Response<string>(HttpStatusCode.InternalServerError, "Book not deleted!");
        }

        _logger.LogInformation("DeleteBookAsync succeeded");
        return new Response<string>(HttpStatusCode.OK, "Book successfully deleted!");
    }

    public async Task<List<Book>> GetBookAsync()
    {
        _logger.LogInformation("Starting GetBookAsync");

        using var context = _dbContext.Connection();
        var query = "select * from books";
        var books = await context.QueryAsync<Book>(query);

        if (!books.Any())
        {
            _logger.LogWarning("GetBookAsync returned empty list");
        }
        else
        {
            _logger.LogInformation("GetBookAsync successfully");
        }

        return books.ToList();
    }

    public async Task<Response<Book?>> GetBookByIdAsync(int bookId)
    {
        _logger.LogInformation("Starting GetBookByIdAsync");

        try
        {
            using var context = _dbContext.Connection();
            var query = "select * from books where id = @id";
            var result = await context.QueryFirstOrDefaultAsync<Book>(query, new { id = bookId });

            if (result == null)
            {
                _logger.LogWarning("GetBookByIdAsync: Book not found");
                return new Response<Book?>(HttpStatusCode.InternalServerError, "Book not found!");
            }

            _logger.LogInformation("GetBookByIdAsync succeeded");
            return new Response<Book?>(HttpStatusCode.OK, "Book found!", result);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "GetBookByIdAsync error");
            return new Response<Book?>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateBookAsync(UpdateBookDto bookDto)
    {
        _logger.LogInformation("Starting UpdateBookAsync");

        try
        {
            using var context = _dbContext.Connection();
            var query = "update books set title = @title, publishedYear = @publishedYear, genre = @genre, authorId = @authorId where id = @id";

            var result = await context.ExecuteAsync(query, new
            {
                title = bookDto.Title,
                id = bookDto.Id,
                publishedYear = bookDto.PublishedYear,
                genre = bookDto.Genre,
                authorId = bookDto.AuthorId
            });

            if (result == 0)
            {
                _logger.LogWarning("UpdateBookAsync failed");
                return new Response<string>(HttpStatusCode.InternalServerError, "Book not updated!");
            }

            _logger.LogInformation("UpdateBookAsync successfully");
            return new Response<string>(HttpStatusCode.OK, "Book successfully updated!");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "UpdateBookAsync error");
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}
