using System.Net;
using Dapper;
using WebApi.Data;
using WebApi.Responses;

namespace WebApi.Services;
using WebApi.Data;

public class AuthorService(ApplicationDbContext applicationDbContext):IAuthorService
{
    private readonly ApplicationDbContext _dbContext = applicationDbContext;

 public async Task<Response<string>> AddAuthorAsync(Author author)
    {
        using var conn = _dbContext.Connection();
        var query = "insert into authors(fullname, birthDate, country) values(@fullname, @birthDate, @country)";
        var res = await conn.ExecuteAsync(query, new
        {
            fullname = author.Fullname,
            birthDate = author.BirthDate,
            country = author.Country
        });

        return res == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Something went wrong!")
            : new Response<string>(HttpStatusCode.OK, "Author added successfully!");
    }

public async Task<Response<string>> DeleteAuthorAsync(int authorId)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = "delete from authors where id = @id";
            var result = await context.ExecuteAsync(query, new{id = authorId});
            return result==0
                ?new Response<string>(HttpStatusCode.InternalServerError, "Author not deleted!")
                :new Response<string>(HttpStatusCode.OK, "Author successfully deleted!");
        }
        catch(Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<List<Author>> GetAuthorAsync()
    {
        using var context = _dbContext.Connection();
        var query = "select * from auhtors";
        var companies = await context.QueryAsync<Author>(query);
        return companies.ToList();
    }

public async Task<Response<Author?>> GetAuthorByIdAsync(int authorId)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = "select * from authors where id = @id";
            var result = await context.QueryFirstOrDefaultAsync<Author>(query,new{id = authorId});
            return result==null
                ?new Response<Author?>(HttpStatusCode.InternalServerError, "Auhtor not found!")
                :new Response<Author?>(HttpStatusCode.OK, "Auhtor found!", result);
        }
        catch(Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<Author?>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
    
public async Task<Response<string>> UpdateAuthorAsync(Author author)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = "update authors set fullname = @fullname,country = @country, birthDate= @birthDate where id = @id";
            var result = await context.ExecuteAsync(query, new{fullname = author.Fullname,country = author.Country,birthDate=author.BirthDate,id = author.Id});
            return result==0
                ?new Response<string>(HttpStatusCode.InternalServerError, "Author not updated!")
                :new Response<string>(HttpStatusCode.OK, "Author successfully updated!");
        }
        catch(Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}