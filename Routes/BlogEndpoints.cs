using BlogMinimalApi.Data;
using BlogMinimalApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BlogMinimalApi.Routes;

public static class BlogEndpoints
{
    public static void MapBlogEndpoints(this WebApplication app)
    {
        app.MapGet("/blogs", [Authorize] async (BlogDbContext context) =>
            await context.Blogs.ToListAsync())
            .WithName("GetAllBlogs")
            .RequireAuthorization(options => options.RequireRole("Admin"));

        app.MapGet("/blogs/{id}", [Authorize] async (int id, BlogDbContext context) =>
            await context.Blogs.FindAsync(id) is Blog blog
                ? Results.Ok(blog)
                : Results.NotFound())
            .WithName("GetBlogById")
            .RequireAuthorization(options => options.RequireRole("Admin"));


        app.MapPost("/blogs", [Authorize] async (BlogDbContext context, Blog blog) =>
        {
            context.Blogs.Add(blog);
            await context.SaveChangesAsync();
            return Results.Created($"/blogs/{blog.Id}", blog);
        }).WithName("CreateBlog")
          .RequireAuthorization(options => options.RequireRole("Admin"));


        // Diğer blog endpoint'leri (güncelleme, silme vb.) buraya eklenebilir
    }
}

