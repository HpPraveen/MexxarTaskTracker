using AutoMapper;
using MexxarTaskTracker.Api.Services.Interfaces;
using MexxarTaskTracker.Domain;
using MexxarTaskTracker.Infrastructure.UnitOfWork;

namespace MexxarTaskTracker.Api.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly IGenericUnitOfWork _genericUnitOfWork;
        private readonly IMapper _mapper;

        public ToDoListService(IGenericUnitOfWork genericUnitOfWork,
            IMapper mapper)
        {
            _genericUnitOfWork = genericUnitOfWork;
            _mapper = mapper;
        }

        public object GetAllToDoLists(int offset, int limit)
        {
            var userToDoLists = _genericUnitOfWork.ToDoListepository.Get(includeProperties: "Tasks")
                .Skip(offset).Take(limit).ToList();
            return _mapper.Map<List<ToDoListDto>>(userToDoLists);
        }

        public object GetToDoListById(long toDoListId)
        {
            var toDoListDetails = _genericUnitOfWork.ToDoListepository.Get(t => t.Id == toDoListId, includeProperties: "Tasks")
                .FirstOrDefault();
            return _mapper.Map<ToDoListDto>(toDoListDetails);
        }

        public object GetToDoListByUser(long userId)
        {
            var userToDoListDetails = _genericUnitOfWork.ToDoListepository.Get(t => t.UserId == userId, includeProperties: "Tasks")
                .FirstOrDefault();
            return _mapper.Map<ToDoListDto>(userToDoListDetails);
        }

        public object CreateUpdateToDoList(ToDoListDto toDoListDto)
        {
            var toDoList = _mapper.Map<ToDoList>(toDoListDto);

            if (toDoList.Id > 0)
            {
                _genericUnitOfWork.ToDoListepository.Update(toDoList);
            }
            else
            {
                var existingUser = GetToDoListById(toDoList.Id);
                if (existingUser == null)
                {
                    _genericUnitOfWork.ToDoListepository.Insert(toDoList);
                }
            }
            _genericUnitOfWork.Commit();
            return _mapper.Map<ToDoListDto>(toDoList);
        }

        public bool DeleteToDoList(long toDoListId)
        {
            try
            {
                var existingToDoList = GetToDoListById(toDoListId);
                if (existingToDoList != null)
                {
                    _genericUnitOfWork.ToDoListepository.Delete(existingToDoList);
                    _genericUnitOfWork.Commit();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

    }
}
