using BloggingApp.Data;
using BloggingApp.Models;
using BloggingApp.Pages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BloggingApp.Tests.Pages;

public class IndexModelTests
{
    [Fact]
    public async Task OnGetAsync_LoadsCategoriesWithTheirPostsForSidebar()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<BloggingContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new BloggingContext(options);
        await context.Database.EnsureCreatedAsync();

        var webCategory = new Category
        {
            Name = "Web Development",
            Posts =
            [
                new Post
                {
                    Title = "Razor Pages Project Setup",
                    Summary = "Summary 1",
                    Content = "Content 1",
                    PublishedOn = new DateTime(2026, 3, 10)
                },
                new Post
                {
                    Title = "Styling Layouts with Bootstrap",
                    Summary = "Summary 2",
                    Content = "Content 2",
                    PublishedOn = new DateTime(2026, 3, 12)
                }
            ]
        };

        var dataCategory = new Category
        {
            Name = "Databases",
            Posts =
            [
                new Post
                {
                    Title = "Why SQLite Fits Small Projects",
                    Summary = "Summary 3",
                    Content = "Content 3",
                    PublishedOn = new DateTime(2026, 3, 11)
                }
            ]
        };

        await context.Categories.AddRangeAsync(webCategory, dataCategory);
        await context.SaveChangesAsync();

        var model = new IndexModel(context);

        await model.OnGetAsync();

        Assert.Equal(3, model.Posts.Count);
        Assert.Collection(
            model.SidebarCategories,
            category =>
            {
                Assert.Equal("Databases", category.Name);
                Assert.Single(category.Posts);
                Assert.Equal("Why SQLite Fits Small Projects", category.Posts[0].Title);
            },
            category =>
            {
                Assert.Equal("Web Development", category.Name);
                Assert.Equal(2, category.Posts.Count);
                Assert.Equal(
                    ["Razor Pages Project Setup", "Styling Layouts with Bootstrap"],
                    category.Posts.Select(post => post.Title).ToArray());
            });
    }
}
