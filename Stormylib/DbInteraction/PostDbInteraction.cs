using StormyLib.Models;
using StormyLib.Contexts;
using Microsoft.EntityFrameworkCore;

namespace StormyLib.DbInteraction;

public class PostDbInteraction(BloggingContext context) : IDbInteraction<Post>
{
    private readonly BloggingContext _context = context;

    public async Task<Post?> GetAsync(int id) => _context.Posts switch
    {
        DbSet<Post> posts => await posts.FindAsync(id),
        _ => null,
    };

    public async Task<Post?> AddAsync(Post entity)
    {
        switch (_context.Posts)
        {
            case DbSet<Post> posts:
                if (await posts.FindAsync(entity.PostId) != null)
                {
                    return null;
                }
                await posts.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            default:
                return null;
        }
    }

    public async Task<Post?> UpdateAsync(Post entity)
    {
        switch (_context.Posts)
        {
            case DbSet<Post> posts:
                if (await posts.FindAsync(entity.PostId) == null)
                {
                    return null;
                }
                _context.Posts.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            default:
                return null;
        }
    }

    public async Task<Post?> DeleteAsync(int id)
    {
        switch (_context.Posts)
        {
            case DbSet<Post> posts:
                var entity = await posts.FindAsync(id);
                if (entity == null)
                {
                    return null;
                }
                posts.Remove(entity);
                await _context.SaveChangesAsync();
                return entity;
            default:
                return null;
        }
    }
}
