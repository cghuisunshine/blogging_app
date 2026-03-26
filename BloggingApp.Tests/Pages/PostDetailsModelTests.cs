using BloggingApp.Data;
using BloggingApp.Models;
using BloggingApp.Pages.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BloggingApp.Tests.Pages;

public class PostDetailsModelTests
{
    [Fact]
    public async Task OnGetAsync_LoadsCurrentPostAndSameCategoryRelatedPosts()
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
                    Title = "Current Post",
                    Summary = "Summary 1",
                    Content = "Content 1",
                    PublishedOn = new DateTime(2026, 3, 10)
                },
                new Post
                {
                    Title = "Related Post A",
                    Summary = "Summary 2",
                    Content = "Content 2",
                    PublishedOn = new DateTime(2026, 3, 12)
                },
                new Post
                {
                    Title = "Related Post B",
                    Summary = "Summary 3",
                    Content = "Content 3",
                    PublishedOn = new DateTime(2026, 3, 14)
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
                    Title = "Different Category Post",
                    Summary = "Summary 4",
                    Content = "Content 4",
                    PublishedOn = new DateTime(2026, 3, 16)
                }
            ]
        };

        await context.Categories.AddRangeAsync(webCategory, dataCategory);
        await context.SaveChangesAsync();

        var currentPostId = webCategory.Posts.Single(post => post.Title == "Current Post").Id;

        var model = new DetailsModel(context);

        var result = await model.OnGetAsync(currentPostId);

        Assert.IsType<PageResult>(result);
        Assert.NotNull(model.Post);
        Assert.Equal("Current Post", model.Post!.Title);
        Assert.Equal(
            ["Related Post A", "Related Post B"],
            model.RelatedPosts.Select(post => post.Title).ToArray());
    }

    [Fact]
    public async Task OnGetAsync_ExcludesCurrentPostFromRelatedPosts()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<BloggingContext>()
            .UseSqlite(connection)
            .Options;

        await using var context = new BloggingContext(options);
        await context.Database.EnsureCreatedAsync();

        var category = new Category
        {
            Name = "Databases",
            Posts =
            [
                new Post
                {
                    Title = "SQLite Basics",
                    Summary = "Summary 1",
                    Content = "Content 1",
                    PublishedOn = new DateTime(2026, 3, 11)
                },
                new Post
                {
                    Title = "EF Core Setup",
                    Summary = "Summary 2",
                    Content = "Content 2",
                    PublishedOn = new DateTime(2026, 3, 12)
                }
            ]
        };

        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var currentPostId = category.Posts.Single(post => post.Title == "SQLite Basics").Id;

        var model = new DetailsModel(context);

        await model.OnGetAsync(currentPostId);

        Assert.DoesNotContain(model.RelatedPosts, post => post.Id == currentPostId);
        Assert.Single(model.RelatedPosts);
        Assert.Equal("EF Core Setup", model.RelatedPosts[0].Title);
    }
}
