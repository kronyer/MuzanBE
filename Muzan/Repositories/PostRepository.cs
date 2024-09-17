using Microsoft.EntityFrameworkCore;
using Muzan.Interfaces;
using Muzan.Models;

namespace Muzan.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly DbContext _context;

        public PostRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Post> GetScheduledPosts()
        {
            return _context.Set<Post>().Where(p => p.Scheduled.HasValue).ToList();
        }
    }
}
