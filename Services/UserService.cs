using System.Net;
using Dapper;
using WebApi.Data;
using WebApi.Responses;
using Microsoft.Extensions.Logging;

namespace WebApi.Services;

public class UserService(ApplicationDbContext applicationDbContext, ILogger<UserService> logger) : IUserService
{
    private readonly ApplicationDbContext _dbContext = applicationDbContext;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<Response<string>> AddUserAsync(AddUserDto userDto)
    {
        _logger.LogInformation("Starting AddUserAsync");

        using var conn = _dbContext.Connection();
        var query = "insert into users(fullName, email, registeredAt) values(@fullName, @email, @registeredAt)";

        var res = await conn.ExecuteAsync(query, new
        {
            fullName = userDto.FullName,
            email = userDto.Email,
            registeredAt = DateTime.Now
        });

        if (res == 0)
        {
            _logger.LogWarning("AddUserAsync failed");
            return new Response<string>(HttpStatusCode.InternalServerError, "Something went wrong!");
        }

        _logger.LogInformation("AddUserAsync succeeded");
        return new Response<string>(HttpStatusCode.OK, "User added successfully!");
    }

    public async Task<Response<string>> DeleteUserAsync(int UserId)
    {
        _logger.LogInformation("Starting DeleteUserAsync");

        using var context = _dbContext.Connection();
        var query = "delete from users where id = @id";

        var result = await context.ExecuteAsync(query, new { id = UserId });

        if (result == 0)
        {
            _logger.LogWarning("DeleteUserAsync failed");
            return new Response<string>(HttpStatusCode.InternalServerError, "User not deleted!");
        }

        _logger.LogInformation("DeleteUserAsync succeeded");
        return new Response<string>(HttpStatusCode.OK, "User successfully deleted!");
    }

    public async Task<List<User>> GetUserAsync()
    {
        _logger.LogInformation("Starting GetUserAsync");

        using var context = _dbContext.Connection();
        var query = "select * from users";
        var users = await context.QueryAsync<User>(query);

        if (!users.Any())
        {
            _logger.LogWarning("GetUserAsync returned empty list");
        }
        else
        {
            _logger.LogInformation("GetUserAsync successfully");
        }

        return users.ToList();
    }

    public async Task<Response<User?>> GetUserByIdAsync(int UserId)
    {
        _logger.LogInformation("Starting GetUserByIdAsync");

        try
        {
            using var context = _dbContext.Connection();
            var query = "select * from users where id = @id";
            var result = await context.QueryFirstOrDefaultAsync<User>(query, new { id = UserId });

            if (result == null)
            {
                _logger.LogWarning("GetUserByIdAsync: User not found");
                return new Response<User?>(HttpStatusCode.InternalServerError, "User not found!");
            }

            _logger.LogInformation("GetUserByIdAsync successfully");
            return new Response<User?>(HttpStatusCode.OK, "User found!", result);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "GetUserByIdAsync error");
            return new Response<User?>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<Response<string>> UpdateUserAsync(UpdateUserDto userDto)
    {
        _logger.LogInformation("Starting UpdateUserAsync");

        try
        {
            using var context = _dbContext.Connection();
            var query = "update users set fullName = @fullName, email = @email where id = @id";

            var result = await context.ExecuteAsync(query, new
            {
                fullName = userDto.FullName,
                email = userDto.Email,
                id = userDto.Id
            });

            if (result == 0)
            {
                _logger.LogWarning("UpdateUserAsync failed");
                return new Response<string>(HttpStatusCode.InternalServerError, "User not updated!");
            }

            _logger.LogInformation("UpdateUserAsync successfully");
            return new Response<string>(HttpStatusCode.OK, "User successfully updated!");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "UpdateUserAsync error");
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}
