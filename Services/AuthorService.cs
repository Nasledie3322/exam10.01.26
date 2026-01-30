using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.Responses;
using WebApi.Data;

namespace WebApi.Services;

public class AuthorService(ApplicationDbContext dbContext,IMapper mapper) : IAuthorService
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<string>> AddAuthorAsync(AddAuthorDto authorDto)
    {
        var author = _mapper.Map<Author>(authorDto);
        await _dbContext.Authors.AddAsync(author);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "Author added successfully!");
    }

    public async Task<Response<string>> DeleteAuthorAsync(int authorId)
    {
        var author = await _dbContext.Authors.FindAsync(authorId);
        if (author == null)
            return new Response<string>(HttpStatusCode.NotFound, "Author not found!");

        _dbContext.Authors.Remove(author);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "Author deleted successfully!");
    }

    public async Task<List<Author>> GetAuthorAsync()
    {
        return await _dbContext.Authors.ToListAsync();
    }

    public async Task<Response<Author?>> GetAuthorByIdAsync(int authorId)
    {
        var author = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == authorId);
        if (author == null)
            return new Response<Author?>(HttpStatusCode.NotFound, "Author not found!");

        return new Response<Author?>(HttpStatusCode.OK, "Author found!", author);
    }

    public async Task<Response<string>> UpdateAuthorAsync(UpdateAuthorDto authorDto)
    {
        var author = await _dbContext.Authors.FindAsync(authorDto.Id);
        if (author == null)
            return new Response<string>(HttpStatusCode.NotFound, "Author not found!");

        _mapper.Map(authorDto, author);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "Author updated successfully!");
    }
}
