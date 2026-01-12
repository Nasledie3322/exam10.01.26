using WebApi.Responses;

namespace WebApi.Services;

public interface IUserService
{
    Task<Response<string>> AddUserAsync(AddUserDto userDto);

    Task<Response<string>> DeleteUserAsync(int userId);

    Task<List<User>> GetUserAsync();

    Task<Response<User?>> GetUserByIdAsync(int userId);

    Task<Response<string>> UpdateUserAsync(UpdateUserDto userDto);
}
