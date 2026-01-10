using Microsoft.AspNetCore.Mvc;
using WebApi.Responses;
using WebApi.Services;

namespace WebApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService UserService) : ControllerBase
{
    [HttpGet]
    public async Task<List<User>> GetUsersAsync()
    {
     return await UserService.GetUserAsync();
    }
     [HttpGet("{UserId}")]
    public async Task<Response<User>> GetByIdAsync(int UserId)
    {
        return await UserService.GetUserByIdAsync(UserId);
    }

    [HttpPost]
    public async Task<Response<string>> AddAsync(User User)
    {
        return await UserService.AddUserAsync(User);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateAsync(User User)
    {
        return await UserService.UpdateUserAsync(User);
    }

    [HttpDelete("{attributeId}")]
    public async Task<Response<string>> DeleteAsync(int UserId)
    {
        return await UserService.DeleteUserAsync(UserId);
    }
}

