using System.Net;
using Dapper;
using WebApi.Data;
using WebApi.Responses;

namespace WebApi.Services;

public class BookLoanService(ApplicationDbContext applicationDbContext) : IBookLoanService
{
    private readonly ApplicationDbContext _dbContext = applicationDbContext;

    public async Task<Response<string>> AddBookLoanAsync(AddBookLoanDto bookLoanDto)
    {
        using var conn = _dbContext.Connection();
        var query = "insert into bookLoans(bookId, userId, loanDate) values(@bookId, @userId, @loanDate)";
        var res = await conn.ExecuteAsync(query, new
        {
            bookId = bookLoanDto.BookId,
            userId = bookLoanDto.UserId,
            loanDate = DateTime.Now 
        });

        return res == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Something went wrong!")
            : new Response<string>(HttpStatusCode.OK, "BookLoan added successfully!");
    }

    public async Task<Response<string>> DeleteBookLoanAsync(int BookLoanId)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = "delete from bookLoans where id = @id";
            var result = await context.ExecuteAsync(query, new { id = BookLoanId });
            return result == 0
                ? new Response<string>(HttpStatusCode.InternalServerError, "BookLoan not deleted!")
                : new Response<string>(HttpStatusCode.OK, "BookLoan successfully deleted!");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<List<BookLoan>> GetBookLoanAsync()
    {
        using var context = _dbContext.Connection();
        var query = "select * from bookLoans";
        var loans = await context.QueryAsync<BookLoan>(query);
        return loans.ToList();
    }

    public async Task<Response<BookLoan?>> GetBookLoanByIdAsync(int BookLoanId)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = "select * from bookLoans where id = @id";
            var result = await context.QueryFirstOrDefaultAsync<BookLoan>(query, new { id = BookLoanId });
            return result == null
                ? new Response<BookLoan?>(HttpStatusCode.InternalServerError, "BookLoan not found!")
                : new Response<BookLoan?>(HttpStatusCode.OK, "BookLoan found!", result);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<BookLoan?>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateBookLoanAsync(UpdateBookLoanDto bookLoanDto)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = @"update bookLoans
                          set bookId = @bookId,
                              userId = @userId,
                              loanDate = @loanDate,
                              returnDate = @returnDate
                          where id = @id";
            var result = await context.ExecuteAsync(query, new
            {
                bookId = bookLoanDto.BookId,
                userId = bookLoanDto.UserId,
                loanDate = bookLoanDto.LoanDate,
                returnDate = bookLoanDto.ReturnDate,
                id = bookLoanDto.Id
            });

            return result == 0
                ? new Response<string>(HttpStatusCode.InternalServerError, "BookLoan not updated!")
                : new Response<string>(HttpStatusCode.OK, "BookLoan successfully updated!");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}
