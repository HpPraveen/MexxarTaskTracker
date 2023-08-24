using AutoMapper;
using MexxarTaskTracker.Api.Services.Interfaces;
using MexxarTaskTracker.Domain;
using MexxarTaskTracker.Infrastructure.UnitOfWork;

namespace MexxarTaskTracker.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericUnitOfWork _genericUnitOfWork;
        private readonly IMapper _mapper;

        public UserService(IGenericUnitOfWork genericUnitOfWork,
            IMapper mapper)
        {
            _genericUnitOfWork = genericUnitOfWork;
            _mapper = mapper;
        }

        public object GetAllUsers(int offset, int limit)
        {
            var allUsers = _genericUnitOfWork.UserRepository.Get().Skip(offset).Take(limit).ToList();
            return _mapper.Map<List<UserDto>>(allUsers);
        }

        public object GetUserById(long userId)
        {
            var userDetails = _genericUnitOfWork.UserRepository.Get(u => u.Id == userId).FirstOrDefault();
            return _mapper.Map<UserDto>(userDetails);
        }

        public object CreateUpdateUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            if (user.Id > 0)
            {
                _genericUnitOfWork.UserRepository.Update(user);
            }
            else
            {
                var existingUser = GetUserById(user.Id);
                if (existingUser == null)
                {
                    _genericUnitOfWork.UserRepository.Insert(user);
                }
            }
            _genericUnitOfWork.Commit();
            return _mapper.Map<UserDto>(user);
        }

        public bool DeleteUser(long userId)
        {
            try
            {
                var existingUser = GetUserById(userId);
                if (existingUser != null)
                {
                    _genericUnitOfWork.UserRepository.Delete(existingUser);
                    _genericUnitOfWork.Commit();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
