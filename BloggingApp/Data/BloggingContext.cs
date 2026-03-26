using BloggingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BloggingApp.Data;

public class BloggingContext(DbContextOptions<BloggingContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasMany(category => category.Posts)
            .WithOne(post => post.Category)
            .HasForeignKey(post => post.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
