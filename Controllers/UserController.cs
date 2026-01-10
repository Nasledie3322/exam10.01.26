using Microsoft.AspNetCore.Mvc;
using WebApi.Responses;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<List<User>> GetUsersAsync()
    {
        return await _userService.GetUserAsync();
    }

    [HttpGet("{userId}")]
    public async Task<Response<User?>> GetByIdAsync(int userId)
    {
        return await _userService.GetUserByIdAsync(userId);
    }

    [HttpPost]
    public async Task<Response<string>> AddAsync(User user)
    {
        return await _userService.AddUserAsync(user);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateAsync(User user)
    {
        return await _userService.UpdateUserAsync(user);
    }

    [HttpDelete("{userId}")]
    public async Task<Response<string>> DeleteAsync(int userId)
    {
        return await _userService.DeleteUserAsync(userId);
    }
}
