namespace EdgeBookmarksManager.Models;

[JsonSerializable(typeof(BookmarksRoot))]
[JsonSerializable(typeof(BookmarkRoots))]
[JsonSerializable(typeof(BookmarkFolder))]
[JsonSerializable(typeof(BookmarkItem))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}