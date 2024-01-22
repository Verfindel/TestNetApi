using Microsoft.EntityFrameworkCore;
using StormyLib.Models;

namespace StormyLib.Contexts;
public class BloggingContext(DbContextOptions<BloggingContext> options) : DbContext(options)
{
    public DbSet<Blog>? Blogs { get; set; }
    public DbSet<Post>? Posts { get; set; }
}
