using Microsoft.AspNetCore.Mvc;
using WebApi.Responses;
using WebApi.Services;

namespace WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BookLoanController(IBookLoanService BookLoanService) : ControllerBase
{
    [HttpGet]
    public async Task<List<BookLoan>> GetBookLoansAsync()
    {
     return await BookLoanService.GetBookLoanAsync();
    }
     [HttpGet("{bookLoanId}")]
    public async Task<Response<BookLoan>> GetByIdAsync(int bookLoanId)
    {
        return await BookLoanService.GetBookLoanByIdAsync(bookLoanId);
    }

    [HttpPost]
    public async Task<Response<string>> AddAsync(BookLoan BookLoan)
    {
        return await BookLoanService.AddBookLoanAsync(BookLoan);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateAsync(BookLoan BookLoan)
    {
        return await BookLoanService.UpdateBookLoanAsync(BookLoan);
    }

    [HttpDelete("{attributeId}")]
    public async Task<Response<string>> DeleteAsync(int BookLoanId)
    {
        return await BookLoanService.DeleteBookLoanAsync(BookLoanId);
    }
}

