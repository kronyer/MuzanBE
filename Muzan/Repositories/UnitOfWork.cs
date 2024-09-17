using Microsoft.EntityFrameworkCore;
using Muzan.Data;
using Muzan.Interfaces;

namespace Muzan.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Posts = new PostRepository(_context);
        }

        public IPostRepository Posts { get; private set; }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
