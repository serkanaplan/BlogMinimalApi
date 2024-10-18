using BlogMinimalApi.Data;
using BlogMinimalApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BlogMinimalApi.Routes;

public static class PostEndpoints
{
    public static void MapPostEndpoints(this WebApplication app)
    {
        app.MapGet("/posts", [Authorize] async (BlogDbContext context) =>
            await context.Posts.ToListAsync())
            .WithName("GetAllPosts")
            .RequireAuthorization(options => options.RequireRole("User"));

        app.MapGet("/posts/{id}", [Authorize] async (int id, BlogDbContext context) =>
            await context.Posts.FindAsync(id) is Post post
                ? Results.Ok(post)
                : Results.NotFound())
            .WithName("GetPostById")
            .RequireAuthorization(options => options.RequireRole("User"));

        app.MapPost("/posts", [Authorize] async (BlogDbContext context, Post post) =>
        {
            context.Posts.Add(post);
            await context.SaveChangesAsync();
            return Results.Created($"/posts/{post.Id}", post);
        }).WithName("CreatePost")
          .RequireAuthorization(options => options.RequireRole("User"));

        // Diğer post endpoint'leri (güncelleme, silme vb.) buraya eklenebilir
    }
}