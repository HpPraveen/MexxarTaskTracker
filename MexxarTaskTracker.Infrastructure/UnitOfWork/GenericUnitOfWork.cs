using static System.GC;

namespace MexxarTaskTracker.Infrastructure.UnitOfWork
{
    public class GenericUnitOfWork : IGenericUnitOfWork
    {
        private readonly DbContext _context;
        private bool _disposed;

        public GenericUnitOfWork(DbContext context)
        {
            _context = context;
        }

        public Task CommitAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            SuppressFinalize(this);
        }

        //public GenericRepository<Invoice> InvoiceRepository => new(_context);
    }
}