using Muzan.Models;

namespace Muzan.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        IEnumerable<Post> GetScheduledPosts();
    }
}
