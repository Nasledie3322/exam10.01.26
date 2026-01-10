using Microsoft.AspNetCore.Mvc;
using WebApi.Responses;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookLoanController : ControllerBase
{
    private readonly IBookLoanService _bookLoanService;

    public BookLoanController(IBookLoanService bookLoanService)
    {
        _bookLoanService = bookLoanService;
    }

    [HttpGet]
    public async Task<List<BookLoan>> GetBookLoansAsync()
    {
        return await _bookLoanService.GetBookLoanAsync();
    }

    [HttpGet("{bookLoanId}")]
    public async Task<Response<BookLoan?>> GetByIdAsync(int bookLoanId)
    {
        return await _bookLoanService.GetBookLoanByIdAsync(bookLoanId);
    }

    [HttpPost]
    public async Task<Response<string>> AddAsync(BookLoan bookLoan)
    {
        return await _bookLoanService.AddBookLoanAsync(bookLoan);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateAsync(BookLoan bookLoan)
    {
        return await _bookLoanService.UpdateBookLoanAsync(bookLoan);
    }

    [HttpDelete("{bookLoanId}")]
    public async Task<Response<string>> DeleteAsync(int bookLoanId)
    {
        return await _bookLoanService.DeleteBookLoanAsync(bookLoanId);
    }
}
