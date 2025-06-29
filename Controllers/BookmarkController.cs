namespace EdgeBookmarksManager.Controllers;

public static class BookmarkController
{
    public static void MapBookmarkEndpoints(this WebApplication app)
    {
        app.MapGet("/api/bookmarks", GetBookmarks);
    }

    private static IResult GetBookmarks()
    {
        var bookmarksPath = BookmarkService.GetBookmarksPath();
        if (!File.Exists(bookmarksPath))
            return Results.NotFound("Bookmarks file not found");
        
        var json = File.ReadAllText(bookmarksPath);
        var bookmarks = JsonSerializer.Deserialize<BookmarksRoot>(json, AppJsonSerializerContext.Default.BookmarksRoot);
        return Results.Ok(bookmarks);
    }
}