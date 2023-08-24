using Invoicing.Infrastructure.Repository;
using MexxarTaskTracker.Domain;

namespace MexxarTaskTracker.Infrastructure.UnitOfWork
{
    public interface IGenericUnitOfWork
    {
        public Task CommitAsync();

        public void Commit();

        public void Dispose();

        public GenericRepository<User> UserRepository { get; }
        public GenericRepository<UserTask> TaskRepository { get; }
        public GenericRepository<ToDoList> ToDoListepository { get; }

    }
}