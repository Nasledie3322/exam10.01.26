using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.Responses;
using WebApi.Data;

namespace WebApi.Services;

public class BookService(ApplicationDbContext applicationDbContext, IMapper mapper) : IBookService
{
    private readonly ApplicationDbContext _dbContext = applicationDbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<string>> AddBookAsync(AddBookDto bookDto)
    {
        var book = _mapper.Map<Book>(bookDto);
        await _dbContext.Books.AddAsync(book);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "Book added successfully!");
    }

    public async Task<Response<string>> DeleteBookAsync(int bookId)
    {
        var book = await _dbContext.Books.FindAsync(bookId);
        if (book == null)
            return new Response<string>(HttpStatusCode.NotFound, "Book not found!");

        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "Book deleted successfully!");
    }

    public async Task<List<Book>> GetBookAsync()
    {
        return await _dbContext.Books.ToListAsync();
    }

    public async Task<Response<Book?>> GetBookByIdAsync(int bookId)
    {
        var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == bookId);
        if (book == null)
            return new Response<Book?>(HttpStatusCode.NotFound, "Book not found!");

        return new Response<Book?>(HttpStatusCode.OK, "Book found!", book);
    }

    public async Task<Response<string>> UpdateBookAsync(UpdateBookDto bookDto)
    {
        var book = await _dbContext.Books.FindAsync(bookDto.Id);
        if (book == null)
            return new Response<string>(HttpStatusCode.NotFound, "Book not found!");

        _mapper.Map(bookDto, book);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "Book updated successfully!");
    }
}
