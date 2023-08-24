using MexxarTaskTracker.Domain;

namespace MexxarTaskTracker.Api.Services.Interfaces
{
    public interface IUserService
    {
        object GetAllUsers(int offset, int limit);
        object CreateUpdateUser(UserDto userDto);
        object GetUserById(long userId);
        bool DeleteUser(long userId);
    }
}
