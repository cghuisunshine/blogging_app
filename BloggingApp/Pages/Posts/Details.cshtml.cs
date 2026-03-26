using BloggingApp.Data;
using BloggingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BloggingApp.Pages.Posts;

public class DetailsModel(BloggingContext context) : PageModel
{
    public Post? Post { get; private set; }

    public List<RelatedPostLink> RelatedPosts { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Post = await context.Posts
            .Include(post => post.Category)
            .FirstOrDefaultAsync(post => post.Id == id);

        if (Post is null)
        {
            return NotFound();
        }

        RelatedPosts = await context.Posts
            .Where(post => post.CategoryId == Post.CategoryId && post.Id != Post.Id)
            .OrderBy(post => post.Title)
            .Select(post => new RelatedPostLink
            {
                Id = post.Id,
                Title = post.Title
            })
            .ToListAsync();

        return Page();
    }

    public class RelatedPostLink
    {
        public int Id { get; init; }

        public string Title { get; init; } = string.Empty;
    }
}
