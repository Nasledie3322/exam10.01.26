using System.Net;
using Dapper;
using WebApi.Data;
using WebApi.Responses;
using Microsoft.Extensions.Logging;

namespace WebApi.Services;

public class BookLoanService(ApplicationDbContext applicationDbContext, ILogger<BookLoanService> logger) : IBookLoanService
{
    private readonly ApplicationDbContext _dbContext = applicationDbContext;
    private readonly ILogger<BookLoanService> _logger = logger;

    public async Task<Response<string>> AddBookLoanAsync(AddBookLoanDto bookLoanDto)
    {
        _logger.LogInformation("Starting AddBookLoanAsync");

        using var conn = _dbContext.Connection();
        var query = "insert into bookLoans(bookId, userId, loanDate) values(@bookId, @userId, @loanDate)";

        var res = await conn.ExecuteAsync(query, new
        {
            bookId = bookLoanDto.BookId,
            userId = bookLoanDto.UserId,
            loanDate = DateTime.Now 
        });

        if (res == 0)
        {
            _logger.LogWarning("AddBookLoanAsync failed: BookLoan was not added");
            return new Response<string>(HttpStatusCode.InternalServerError, "Something went wrong!");
        }

        _logger.LogInformation("AddBookLoanAsync succeeded: BookLoan added successfully");
        return new Response<string>(HttpStatusCode.OK, "BookLoan added successfully!");
    }

    public async Task<Response<string>> DeleteBookLoanAsync(int BookLoanId)
    {
        _logger.LogInformation("Starting DeleteBookLoanAsync");

        using var context = _dbContext.Connection();
        var query = "delete from bookLoans where id = @id";

        var result = await context.ExecuteAsync(query, new { id = BookLoanId });

        if (result == 0)
        {
            _logger.LogWarning("DeleteBookLoanAsync warning: BookLoan not found");
            return new Response<string>(HttpStatusCode.InternalServerError, "BookLoan not deleted!");
        }

        _logger.LogInformation("DeleteBookLoanAsync succeeded: BookLoan deleted", BookLoanId);
        return new Response<string>(HttpStatusCode.OK, "BookLoan successfully deleted!");
    }

    public async Task<List<BookLoan>> GetBookLoanAsync()
    {
        _logger.LogInformation("Starting GetBookLoanAsync");

        using var context = _dbContext.Connection();
        var query = "select * from bookLoans";
        var loans = await context.QueryAsync<BookLoan>(query);

        if (!loans.Any())
        {
            _logger.LogWarning("GetBookLoanAsync returned empty list");
        }
        else
        {
            _logger.LogInformation("GetBookLoanAsync returned {Count} BookLoans", loans.Count());
        }

        return loans.ToList();
    }

    public async Task<Response<BookLoan?>> GetBookLoanByIdAsync(int BookLoanId)
    {
        _logger.LogInformation("Starting GetBookLoanByIdAsync");

        try
        {
            using var context = _dbContext.Connection();
            var query = "select * from bookLoans where id = @id";
            var result = await context.QueryFirstOrDefaultAsync<BookLoan>(query, new { id = BookLoanId });

            if (result == null)
            {
                _logger.LogWarning("GetBookLoanByIdAsync warning: BookLoan not found");
                return new Response<BookLoan?>(HttpStatusCode.InternalServerError, "BookLoan not found!");
            }

            _logger.LogInformation("GetBookLoanByIdAsync succeeded: BookLoan found");
            return new Response<BookLoan?>(HttpStatusCode.OK, "BookLoan found!", result);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error in GetBookLoanByIdAsync for BookLoanId = {BookLoanId}");
            return new Response<BookLoan?>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateBookLoanAsync(UpdateBookLoanDto bookLoanDto)
    {
        _logger.LogInformation("Starting UpdateBookLoanAsync");

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

            if (result == 0)
            {
                _logger.LogWarning("UpdateBookLoanAsync warning: BookLoan not updated");
                return new Response<string>(HttpStatusCode.InternalServerError, "BookLoan not updated!");
            }

            _logger.LogInformation("UpdateBookLoanAsync succeeded: BookLoan updated");
            return new Response<string>(HttpStatusCode.OK, "BookLoan successfully updated!");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateBookLoanAsync for BookLoanId");
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}
