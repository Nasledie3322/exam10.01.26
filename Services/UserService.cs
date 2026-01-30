using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApi.Responses;
using WebApi.Data;

namespace WebApi.Services;

public class UserService(ApplicationDbContext applicationDbContext, IMapper mapper) : IUserService
{
    private readonly ApplicationDbContext _dbContext = applicationDbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<string>> AddUserAsync(AddUserDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        user.RegisteredAt = DateTime.Now;

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "User added successfully!");
    }

    public async Task<Response<string>> DeleteUserAsync(int userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if (user == null)
            return new Response<string>(HttpStatusCode.NotFound, "User not found!");

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "User deleted successfully!");
    }

    public async Task<List<User>> GetUserAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<Response<User?>> GetUserByIdAsync(int userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return new Response<User?>(HttpStatusCode.NotFound, "User not found!");

        return new Response<User?>(HttpStatusCode.OK, "User found!", user);
    }

    public async Task<Response<string>> UpdateUserAsync(UpdateUserDto userDto)
    {
        var user = await _dbContext.Users.FindAsync(userDto.Id);
        if (user == null)
            return new Response<string>(HttpStatusCode.NotFound, "User not found!");

        _mapper.Map(userDto, user);
        await _dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, "User updated successfully!");
    }
}
