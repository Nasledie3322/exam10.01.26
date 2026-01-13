using System.Net;
using Dapper;
using WebApi.Responses;

namespace WebApi.Services;
using WebApi.Data;

public class AuthorService(ApplicationDbContext applicationDbContext, ILogger<AuthorService> logger): IAuthorService
{
    private readonly ApplicationDbContext _dbContext = applicationDbContext;
    private readonly ILogger<AuthorService> _logger = logger;

    public async Task<Response<string>> AddAuthorAsync(AddAuthorDto authorDto)
    {
        _logger.LogInformation("Starting AddAuthorAsync");

        using var conn = _dbContext.Connection();
        var query = "insert into authors(fullname, birthDate, country) values(@fullname, @birthDate, @country)";

        var res = await conn.ExecuteAsync(query, new
        {
            fullname = authorDto.Fullname,
            birthDate = authorDto.BirthDate,
            country = authorDto.Country
        });

        if(res == 0)
        {
            _logger.LogWarning("AddAuthorAsync failed: Author was not added");
            return new Response<string>(HttpStatusCode.InternalServerError, "Something went wrong!");
        }

        _logger.LogInformation("AddAuthorAsync succeeded: Author added successfully");
        return new Response<string>(HttpStatusCode.OK, "Author added successfully!");
    }

    public async Task<Response<string>> DeleteAuthorAsync(int authorId)
    {
        _logger.LogInformation("Starting DeleteAuthorAsync. AuthorId = {AuthorId}", authorId);

        using var context = _dbContext.Connection();
        var query = "delete from authors where id = @id";

        var result = await context.ExecuteAsync(query, new { id = authorId });

        if (result == 0)
        {
            _logger.LogWarning("DeleteAuthorAsync warning: Author with id {AuthorId} not found", authorId);
            return new Response<string>(HttpStatusCode.InternalServerError, "Author not deleted!");
        }

        _logger.LogInformation("DeleteAuthorAsync succeeded: Author with id {AuthorId} deleted", authorId);
        return new Response<string>(HttpStatusCode.OK, "Author successfully deleted!");
    }

    public async Task<List<Author>> GetAuthorAsync()
    {
        _logger.LogInformation("Starting GetAuthorAsync");

        using var context = _dbContext.Connection();
        var query = "select * from authors";
        var authors = await context.QueryAsync<Author>(query);

        if(!authors.Any())
        {
            _logger.LogWarning("GetAuthorAsync returned empty list");
        }
        else
        {
            _logger.LogInformation("GetAuthorAsync returned {Count} authors", authors.Count());
        }

        return authors.ToList();
    }

    public async Task<Response<Author?>> GetAuthorByIdAsync(int authorId)
    {
        _logger.LogInformation("Starting GetAuthorByIdAsync. AuthorId = {AuthorId}", authorId);

        try
        {
            using var context = _dbContext.Connection();
            var query = "select * from authors where id = @id";
            var result = await context.QueryFirstOrDefaultAsync<Author>(query, new { id = authorId });

            if (result == null)
            {
                _logger.LogWarning("GetAuthorByIdAsync warning: Author with id {AuthorId} not found", authorId);
                return new Response<Author?>(HttpStatusCode.InternalServerError, "Author not found!");
            }

            _logger.LogInformation("GetAuthorByIdAsync succeeded: Author with id {AuthorId} found", authorId);
            return new Response<Author?>(HttpStatusCode.OK, "Author found!", result);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error in GetAuthorByIdAsync for AuthorId = {AuthorId}", authorId);
            return new Response<Author?>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateAuthorAsync(UpdateAuthorDto authorDto)
    {
        _logger.LogInformation("Starting UpdateAuthorAsync. AuthorId = {AuthorId}", authorDto.Id);

        try
        {
            using var context = _dbContext.Connection();
            var query = "update authors set fullname = @fullname, country = @country, birthDate= @birthDate where id = @id";
            var result = await context.ExecuteAsync(query, new
            {
                fullname = authorDto.Fullname,
                country = authorDto.Country,
                birthDate = authorDto.BirthDate,
                id = authorDto.Id
            });

            if (result == 0)
            {
                _logger.LogWarning("UpdateAuthorAsync warning: Author with id {AuthorId} not updated", authorDto.Id);
                return new Response<string>(HttpStatusCode.InternalServerError, "Author not updated!");
            }

            _logger.LogInformation("UpdateAuthorAsync succeeded: Author with id {AuthorId} updated", authorDto.Id);
            return new Response<string>(HttpStatusCode.OK, "Author updated successfully!");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateAuthorAsync for AuthorId = {AuthorId}", authorDto.Id);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}
