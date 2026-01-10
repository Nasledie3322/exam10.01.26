using WebApi.Responses;

namespace WebApi.Services;

public interface IUserService
{
    Task<Response<string>> AddUserAsync(User user);

    Task<Response<string>> DeleteUserAsync(int userId);

    Task<List<User>> GetUserAsync();

    Task<Response<User?>> GetUserByIdAsync(int userId);

    Task<Response<string>> UpdateUserAsync(User user);
}
