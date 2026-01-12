using System.Net;
using Dapper;
using WebApi.Data;
using WebApi.Responses;

namespace WebApi.Services;
using WebApi.Data;

public class BookService(ApplicationDbContext applicationDbContext) : IBookService
{
    private readonly ApplicationDbContext _dbContext = applicationDbContext;


public async Task<Response<string>> AddBookAsync(AddBookDto bookDto)
{
    using var conn = _dbContext.Connection();
    var query = "insert into books(title, publishedyear, genre, authorid) values(@title, @publishedYear, @genre, @AuthorId)";
    var res = await conn.ExecuteAsync(query, bookDto);

    return res == 0
        ? new Response<string>(HttpStatusCode.InternalServerError, "Something went wrong!")
        : new Response<string>(HttpStatusCode.OK, "Book added successfully!");
}



public async Task<Response<string>> DeleteBookAsync(int bookId)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = "delete from books where id = @id";
            var result = await context.ExecuteAsync(query, new{id = bookId});
            return result==0
                ?new Response<string>(HttpStatusCode.InternalServerError, "Book not deleted!")
                :new Response<string>(HttpStatusCode.OK, "Book successfully deleted!");
        }
        catch(Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<List<Book>> GetBookAsync()
    {
        using var context = _dbContext.Connection();
        var query = "select * from books";
        var companies = await context.QueryAsync<Book>(query);
        return companies.ToList();
    }

public async Task<Response<Book?>> GetBookByIdAsync(int bookId)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = "select * from books where id = @id";
            var result = await context.QueryFirstOrDefaultAsync<Book>(query,new{id = bookId});
            return result==null
                ?new Response<Book?>(HttpStatusCode.InternalServerError, "Book not found!")
                :new Response<Book?>(HttpStatusCode.OK, "Book found!", result);
        }
        catch(Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<Book?>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
    
public async Task<Response<string>> UpdateBookAsync(UpdateBookDto bookDto)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = "update books set title = @title, publishedYear= @publishedYear, genre = @genre, authorId=@authorId where id = @id";
            var result = await context.ExecuteAsync(query, new{title = bookDto.Title ,id = bookDto.Id, publishedYear = bookDto.PublishedYear, genre = bookDto.Genre,authorId= bookDto.AuthorId});
            return result==0
                ?new Response<string>(HttpStatusCode.InternalServerError, "Book not updated!")
                :new Response<string>(HttpStatusCode.OK, "Book successfully updated!");
        }
        catch(Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}