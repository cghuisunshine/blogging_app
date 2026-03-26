using BloggingApp.Data;
using BloggingApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BloggingApp.Pages;

public class IndexModel(BloggingContext context) : PageModel
{
    public List<Post> Posts { get; private set; } = [];

    public List<SidebarCategory> SidebarCategories { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Posts = await context.Posts
            .Include(post => post.Category)
            .OrderByDescending(post => post.PublishedOn)
            .ToListAsync();
        
        var categories = await context.Categories
            .Include(category => category.Posts)
            .OrderBy(category => category.Name)
            .ToListAsync();

        SidebarCategories = categories
            .Select(category => new SidebarCategory
            {
                Name = category.Name,
                Posts = category.Posts
                    .OrderBy(post => post.Title)
                    .Select(post => new SidebarPostLink
                    {
                        Id = post.Id,
                        Title = post.Title
                    })
                    .ToList()
            })
            .ToList();
    }

    public class SidebarCategory
    {
        public string Name { get; init; } = string.Empty;

        public List<SidebarPostLink> Posts { get; init; } = [];
    }

    public class SidebarPostLink
    {
        public int Id { get; init; }

        public string Title { get; init; } = string.Empty;
    }
}
