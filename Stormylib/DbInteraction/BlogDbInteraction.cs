using Microsoft.EntityFrameworkCore;
using StormyLib.Contexts;
using StormyLib.Models;

namespace StormyLib.DbInteraction;

public class BlogDbInteraction(BloggingContext context) : IDbInteraction<Blog>
{
	private readonly BloggingContext _context = context;

    public async Task<Blog?> GetAsync(int id) => _context.Blogs switch
	{
		DbSet<Blog> blogs => await blogs
            .Include(b => b.Posts)
            .FirstOrDefaultAsync(b => b.BlogId == id),
		_ => null,
	};

	public async Task<Blog?> AddAsync(Blog entity)
	{
        switch (_context.Blogs)
        {
            case DbSet<Blog> blogs:
                if(await blogs.FindAsync(entity.BlogId) != null)
                {
                    return null;
                }
                await blogs.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            default:
                return null;
        }
	}

	public async Task<Blog?> UpdateAsync(Blog entity)
	{
        switch (_context.Blogs)
        {
            case DbSet<Blog> blogs:
                _context.Blogs.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            default:
                return null;
        }
	}

	public async Task<Blog?> DeleteAsync(int id)
	{
        switch (_context.Blogs)
        {
            case DbSet<Blog> blogs:
                var entity = await blogs.FindAsync(id);
                if(entity == null)
                {
                    return null;
                }
                blogs.Remove(entity);
                await _context.SaveChangesAsync();
                return entity;
            default:
                return null;
        }
	}
}
