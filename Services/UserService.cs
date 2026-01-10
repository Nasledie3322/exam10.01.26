using System.Net;
using Dapper;
using WebApi.Data;
using WebApi.Responses;

namespace WebApi.Services;
using WebApi.Data;

public class UserService(ApplicationDbContext applicationDbContext) : IUserService
{
    private readonly ApplicationDbContext _dbContext = applicationDbContext;

 public async Task<Response<string>> AddUserAsync(User User)
    {
        using var conn = _dbContext.Connection();
        var query = "insert into users(fullName, email, registeredAt) values(@fullName, @email, @registeredAt)";
        var res = await conn.ExecuteAsync(query, new
        {
            fullName = User.FullName,
            email = User.Email,
            registeredAt = User.RegisteredAt
        });

        return res == 0
            ? new Response<string>(HttpStatusCode.InternalServerError, "Something went wrong!")
            : new Response<string>(HttpStatusCode.OK, "User added successfully!");
    }

public async Task<Response<string>> DeleteUserAsync(int UserId)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = "delete from users where id = @id";
            var result = await context.ExecuteAsync(query, new{id = UserId});
            return result==0
                ?new Response<string>(HttpStatusCode.InternalServerError, "User not deleted!")
                :new Response<string>(HttpStatusCode.OK, "User successfully deleted!");
        }
        catch(Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }

    public async Task<List<User>> GetUserAsync()
    {
        using var context = _dbContext.Connection();
        var query = "select * from users";
        var companies = await context.QueryAsync<User>(query);
        return companies.ToList();
    }

public async Task<Response<User?>> GetUserByIdAsync(int UserId)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = "select * from users where id = @id";
            var result = await context.QueryFirstOrDefaultAsync<User>(query,new{id = UserId});
            return result==null
                ?new Response<User?>(HttpStatusCode.InternalServerError, "User not found!")
                :new Response<User?>(HttpStatusCode.OK, "User found!", result);
        }
        catch(Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<User?>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
    
public async Task<Response<string>> UpdateUserAsync(User User)
    {
        try
        {
            using var context = _dbContext.Connection();
            var query = "update users set fullName = @fullName, email = @email where id = @id";
            var result = await context.ExecuteAsync(query, new{fullName = User.FullName,email = User.Email ,id = User.Id});
            return result==0
                ?new Response<string>(HttpStatusCode.InternalServerError, "User not updated!")
                :new Response<string>(HttpStatusCode.OK, "User successfully updated!");
        }
        catch(Exception ex)
        {
            System.Console.WriteLine(ex);
            return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}