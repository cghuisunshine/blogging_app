using BloggingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BloggingApp.Data;

public static class SeedData
{
    public static async Task EnsureSeededAsync(BloggingContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Categories.AnyAsync() || await context.Posts.AnyAsync())
        {
            return;
        }

        var categories = new List<Category>
        {
            new()
            {
                Name = "Web Development",
                Posts =
                [
                    new Post
                    {
                        Title = "Razor Pages Project Setup",
                        Summary = "A quick look at structuring a clean Razor Pages application.",
                        Content = "Razor Pages works best when each page owns a focused slice of behavior. Start by keeping page models small, views readable, and shared infrastructure in dedicated services.",
                        PublishedOn = new DateTime(2026, 3, 10)
                    },
                    new Post
                    {
                        Title = "Styling Layouts with Bootstrap",
                        Summary = "Using Bootstrap spacing and grid utilities to create clean content and sidebar layouts.",
                        Content = "A strong content layout usually starts with hierarchy. Keep the primary reading column wide enough for body text, then use the sidebar for secondary navigation such as categories or related articles.",
                        PublishedOn = new DateTime(2026, 3, 12)
                    },
                    new Post
                    {
                        Title = "Deploying to Azure App Service",
                        Summary = "What to verify before publishing a small ASP.NET Core application.",
                        Content = "Before deployment, confirm the app starts locally, connection strings are defined clearly, and seed logic is safe to run. Small deployment checks prevent most classroom-demo failures.",
                        PublishedOn = new DateTime(2026, 3, 18)
                    }
                ]
            },
            new()
            {
                Name = "Databases",
                Posts =
                [
                    new Post
                    {
                        Title = "Why SQLite Fits Small Projects",
                        Summary = "SQLite is often enough for demos, assignments, and prototypes.",
                        Content = "SQLite keeps setup costs low. For a small blogging app, it provides persistence without extra infrastructure while still supporting relational queries and EF Core.",
                        PublishedOn = new DateTime(2026, 3, 11)
                    },
                    new Post
                    {
                        Title = "Seeding Sample Data in EF Core",
                        Summary = "A practical first-run seeding approach for local projects.",
                        Content = "For assignment-sized apps, a startup seed method is simpler than a full admin UI. It helps every run begin with useful sample content for navigation and screenshots.",
                        PublishedOn = new DateTime(2026, 3, 16)
                    },
                    new Post
                    {
                        Title = "Querying Related Records",
                        Summary = "How to load records from the same category without overcomplicating the query.",
                        Content = "When showing related posts, start from the current post's category id, exclude the current id, and order results predictably. The goal is relevance and stability, not query cleverness.",
                        PublishedOn = new DateTime(2026, 3, 22)
                    }
                ]
            },
            new()
            {
                Name = "Student Workflow",
                Posts =
                [
                    new Post
                    {
                        Title = "Planning Before Coding",
                        Summary = "A short argument for defining scope before implementation.",
                        Content = "Even small web apps benefit from a written design. It clarifies page responsibilities, data shape, and what success looks like before code starts to lock decisions in place.",
                        PublishedOn = new DateTime(2026, 3, 9)
                    },
                    new Post
                    {
                        Title = "Testing Page Model Behavior",
                        Summary = "Why focused tests against page models are useful in Razor Pages apps.",
                        Content = "Page model tests give confidence in query logic and page behavior without forcing full browser automation. They are especially useful for validating sidebars and filtered lists.",
                        PublishedOn = new DateTime(2026, 3, 20)
                    }
                ]
            }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }
}
