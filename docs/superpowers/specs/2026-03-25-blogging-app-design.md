# Razor Pages Blogging App Design

**Date:** 2026-03-25

**Goal**

Build a small ASP.NET Core Razor Pages blogging app in this workspace using EF Core with SQLite, then implement the assignment's bonus requirements:

- On the home page, show a right sidebar listing all blog post links grouped by category.
- On the post details page, show a right sidebar listing related posts from the same category as the current post.

**Project Context**

The workspace initially contained only the assignment PDF and no existing ASP.NET project. The app will therefore be scaffolded from scratch and kept assignment-sized.

**Architecture**

The app will use the default Razor Pages structure with EF Core and SQLite. A `BloggingContext` will manage `Category` and `Post` entities. The application will initialize the database and seed a small sample dataset on startup. Razor Page models will query EF Core directly for page-specific view data.

**Data Model**

- `Category`
  - `Id`
  - `Name`
  - `Posts`
- `Post`
  - `Id`
  - `Title`
  - `Summary`
  - `Content`
  - `PublishedOn`
  - `CategoryId`
  - `Category`

Relationship:

- One `Category` has many `Post` records.
- Each `Post` belongs to one `Category`.

**Pages**

- `Index`
  - Main content: a list of post summaries.
  - Right sidebar: all categories, each with links to posts in that category.
- `Posts/Details`
  - Main content: selected post title, metadata, and full content.
  - Right sidebar: related posts from the same category, excluding the current post.

**UI Layout**

Use a simple two-column layout within the existing Razor Pages shared layout:

- Left/main column for primary page content
- Right/sidebar column for grouped navigation widgets

Styling will remain lightweight and assignment-focused.

**Behavior**

- Home page loads all posts for the main list and all categories with their posts for the sidebar.
- Details page loads the selected post and its category.
- Details sidebar shows posts in the same category as the current post, excluding the current post.
- If a category has no related posts beyond the current post, the page renders an empty-state message or an empty list without failing.

**Error Handling**

- Invalid details page id returns `NotFound()`.
- Empty database state does not crash page rendering.
- Sidebar queries always return empty collections rather than nulls where possible.

**Testing Strategy**

Use TDD at the page-model behavior level:

- Verify the home page model loads grouped category/post sidebar data.
- Verify the details page model loads the requested post.
- Verify the details page model returns only same-category related posts.
- Verify the current post is excluded from related posts.

The Razor markup will then bind to these tested page-model properties.

**Constraints**

- Build in the current workspace.
- Use EF Core with SQLite, not in-memory-only persistence.
- Keep the app small, readable, and suitable for local execution and later deployment.
