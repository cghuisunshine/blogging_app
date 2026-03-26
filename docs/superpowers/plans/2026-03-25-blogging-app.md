# Blogging App Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Scaffold a Razor Pages blogging app with EF Core and SQLite, seed it with sample data, and implement the bonus home-page category sidebar and post-details related-posts sidebar.

**Architecture:** Create a standard ASP.NET Core Razor Pages app, add EF Core models and a `DbContext`, seed SQLite data at startup, then implement page-model queries and Razor markup for the required sidebars. Keep the query logic in page models and keep the views focused on rendering.

**Tech Stack:** ASP.NET Core Razor Pages, C#, EF Core, SQLite, xUnit

---

### Task 1: Scaffold the application

**Files:**
- Create: `BloggingApp.sln`
- Create: `BloggingApp/BloggingApp.csproj`
- Create: `BloggingApp/Program.cs`
- Create: `BloggingApp/appsettings.json`
- Create: `BloggingApp/Pages/**/*`
- Create: `BloggingApp/wwwroot/**/*`

- [ ] **Step 1: Scaffold the base Razor Pages app**

Run: `dotnet new sln -n BloggingApp`
Run: `dotnet new webapp -n BloggingApp`
Run: `dotnet sln BloggingApp.sln add BloggingApp/BloggingApp.csproj`

- [ ] **Step 2: Run the app once to verify the baseline scaffold**

Run: `dotnet run --project BloggingApp/BloggingApp.csproj`
Expected: app starts successfully and serves the default Razor Pages site

- [ ] **Step 3: Commit scaffold**

```bash
git add .
git commit -m "chore: scaffold razor pages app"
```

### Task 2: Add the database dependencies and data model

**Files:**
- Modify: `BloggingApp/BloggingApp.csproj`
- Create: `BloggingApp/Models/Category.cs`
- Create: `BloggingApp/Models/Post.cs`
- Create: `BloggingApp/Data/BloggingContext.cs`
- Create: `BloggingApp/Data/SeedData.cs`
- Modify: `BloggingApp/Program.cs`

- [ ] **Step 1: Add EF Core package references**

Add package references for:
- `Microsoft.EntityFrameworkCore.Sqlite`
- `Microsoft.EntityFrameworkCore.Design`

- [ ] **Step 2: Write the minimal model classes**

```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Post> Posts { get; set; } = new();
}
```

```csharp
public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedOn { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
```

- [ ] **Step 3: Add `BloggingContext` and SQLite registration**

Configure a `DbSet<Category>` and `DbSet<Post>` and register the context in `Program.cs` using a SQLite connection string.

- [ ] **Step 4: Add startup seed logic**

Seed several categories with multiple posts each.

- [ ] **Step 5: Run the app to verify DB initialization**

Run: `dotnet run --project BloggingApp/BloggingApp.csproj`
Expected: SQLite file is created and app starts without exceptions

- [ ] **Step 6: Commit data layer**

```bash
git add .
git commit -m "feat: add blog data model and sqlite persistence"
```

### Task 3: Add failing tests for home page sidebar behavior

**Files:**
- Create: `BloggingApp.Tests/BloggingApp.Tests.csproj`
- Create: `BloggingApp.Tests/Pages/IndexModelTests.cs`
- Modify: `BloggingApp.sln`

- [ ] **Step 1: Create the test project**

Run: `dotnet new xunit -n BloggingApp.Tests`
Run: `dotnet sln BloggingApp.sln add BloggingApp.Tests/BloggingApp.Tests.csproj`
Run: `dotnet add BloggingApp.Tests/BloggingApp.Tests.csproj reference BloggingApp/BloggingApp.csproj`

- [ ] **Step 2: Write the failing test for grouped sidebar data**

```csharp
[Fact]
public async Task OnGetAsync_LoadsCategoriesWithTheirPostsForSidebar()
{
    // Arrange test db with categories and posts
    // Act call OnGetAsync
    // Assert categories are present and include linked posts
}
```

- [ ] **Step 3: Run the test to verify it fails for the expected reason**

Run: `dotnet test BloggingApp.Tests/BloggingApp.Tests.csproj --filter IndexModelTests`
Expected: FAIL because the page model does not yet expose the required sidebar data

- [ ] **Step 4: Commit failing home-page test**

```bash
git add .
git commit -m "test: add failing home page sidebar test"
```

### Task 4: Implement the home page query and markup

**Files:**
- Modify: `BloggingApp/Pages/Index.cshtml.cs`
- Modify: `BloggingApp/Pages/Index.cshtml`
- Modify: `BloggingApp/wwwroot/css/site.css`

- [ ] **Step 1: Write the minimal page-model implementation**

Load:
- the main post list
- the category list with posts for the sidebar

- [ ] **Step 2: Run the test to verify it passes**

Run: `dotnet test BloggingApp.Tests/BloggingApp.Tests.csproj --filter IndexModelTests`
Expected: PASS

- [ ] **Step 3: Render the sidebar markup on the home page**

Render each category header and its post links in the right sidebar.

- [ ] **Step 4: Start the app and verify the home page manually**

Run: `dotnet run --project BloggingApp/BloggingApp.csproj`
Expected: right sidebar displays categories and post links

- [ ] **Step 5: Commit home page feature**

```bash
git add .
git commit -m "feat: add home page category sidebar"
```

### Task 5: Add failing tests for details page related posts

**Files:**
- Create: `BloggingApp.Tests/Pages/PostDetailsModelTests.cs`

- [ ] **Step 1: Write the failing tests**

```csharp
[Fact]
public async Task OnGetAsync_LoadsCurrentPostAndSameCategoryRelatedPosts()
{
}

[Fact]
public async Task OnGetAsync_ExcludesCurrentPostFromRelatedPosts()
{
}
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test BloggingApp.Tests/BloggingApp.Tests.csproj --filter PostDetailsModelTests`
Expected: FAIL because details page behavior is not yet implemented

- [ ] **Step 3: Commit failing details tests**

```bash
git add .
git commit -m "test: add failing related posts tests"
```

### Task 6: Implement the post details page and related-posts sidebar

**Files:**
- Create: `BloggingApp/Pages/Posts/Details.cshtml`
- Create: `BloggingApp/Pages/Posts/Details.cshtml.cs`
- Modify: `BloggingApp/Pages/Index.cshtml`
- Modify: `BloggingApp/wwwroot/css/site.css`

- [ ] **Step 1: Add the minimal details page-model implementation**

Load the selected post by id, include its category, and query related posts from the same category excluding the current post.

- [ ] **Step 2: Run the related-post tests**

Run: `dotnet test BloggingApp.Tests/BloggingApp.Tests.csproj --filter PostDetailsModelTests`
Expected: PASS

- [ ] **Step 3: Add details-page markup**

Render the selected post and the related-posts sidebar.

- [ ] **Step 4: Link the home page posts to the details page**

Use `asp-page="/Posts/Details"` and the post id route value.

- [ ] **Step 5: Manually verify details-page behavior**

Run: `dotnet run --project BloggingApp/BloggingApp.csproj`
Expected: details page shows only same-category related posts, not the current post

- [ ] **Step 6: Commit details page feature**

```bash
git add .
git commit -m "feat: add related posts sidebar"
```

### Task 7: Final verification

**Files:**
- Verify only

- [ ] **Step 1: Run the full test suite**

Run: `dotnet test BloggingApp.sln`
Expected: PASS

- [ ] **Step 2: Run the app for final smoke test**

Run: `dotnet run --project BloggingApp/BloggingApp.csproj`
Expected: home page and details page both render correctly

- [ ] **Step 3: Record deployment follow-up**

Prepare the project for GitHub push and Azure App Service deployment after local verification.
