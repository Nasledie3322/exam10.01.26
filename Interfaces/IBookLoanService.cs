using WebApi.Responses;

namespace WebApi.Services;

public interface IBookLoanService
{
    Task<Response<string>> AddBookLoanAsync(BookLoan bookLoan);

    Task<Response<string>> DeleteBookLoanAsync(int bookLoanId);

    Task<List<BookLoan>> GetBookLoanAsync();

    Task<Response<BookLoan?>> GetBookLoanByIdAsync(int bookLoanId);

    Task<Response<string>> UpdateBookLoanAsync(BookLoan bookLoan);
}
