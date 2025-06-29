namespace EdgeBookmarksManager.Models;

public class BookmarksRoot
{
    [JsonPropertyName("roots")]
    public BookmarkRoots Roots { get; set; } = new();
}

public class BookmarkRoots
{
    [JsonPropertyName("bookmark_bar")]
    public BookmarkFolder BookmarkBar { get; set; } = new();
    
    [JsonPropertyName("other")]
    public BookmarkFolder Other { get; set; } = new();
}

public class BookmarkFolder
{
    [JsonPropertyName("children")]
    public List<BookmarkItem> Children { get; set; } = new();
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";
}

public class BookmarkItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";
    
    [JsonPropertyName("children")]
    public List<BookmarkItem>? Children { get; set; }
}