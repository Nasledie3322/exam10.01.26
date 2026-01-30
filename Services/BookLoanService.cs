using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.Responses;
using WebApi.Data;

namespace WebApi.Services;

public class BookLoanService(ApplicationDbContext dbContext,IMapper mapper) : IBookLoanService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<string>> AddBookLoanAsync(AddBookLoanDto bookLoanDto)
    {
        var bookLoan = _mapper.Map<BookLoan>(bookLoanDto);
        bookLoan.LoanDate = DateTime.Now;

        await _dbContext.BookLoans.AddAsync(bookLoan);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "BookLoan added successfully!");
    }

    public async Task<Response<string>> DeleteBookLoanAsync(int bookLoanId)
    {
        var bookLoan = await _dbContext.BookLoans.FindAsync(bookLoanId);
        if (bookLoan == null)
            return new Response<string>(HttpStatusCode.NotFound, "BookLoan not found!");

        _dbContext.BookLoans.Remove(bookLoan);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "BookLoan deleted successfully!");
    }

    public async Task<List<BookLoan>> GetBookLoanAsync()
    {
        return await _dbContext.BookLoans.ToListAsync();
    }

    public async Task<Response<BookLoan?>> GetBookLoanByIdAsync(int bookLoanId)
    {
        var bookLoan = await _dbContext.BookLoans.FirstOrDefaultAsync(b => b.Id == bookLoanId);
        if (bookLoan == null)
            return new Response<BookLoan?>(HttpStatusCode.NotFound, "BookLoan not found!");

        return new Response<BookLoan?>(HttpStatusCode.OK, "BookLoan found!", bookLoan);
    }

    public async Task<Response<string>> UpdateBookLoanAsync(UpdateBookLoanDto bookLoanDto)
    {
        var bookLoan = await _dbContext.BookLoans.FindAsync(bookLoanDto.Id);
        if (bookLoan == null)
            return new Response<string>(HttpStatusCode.NotFound, "BookLoan not found!");

        _mapper.Map(bookLoanDto, bookLoan);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "BookLoan updated successfully!");
    }
}
