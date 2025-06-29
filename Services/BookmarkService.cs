namespace EdgeBookmarksManager.Services;

public static class BookmarkService
{
    public static string GetBookmarksPath()
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(userProfile, @"AppData\Local\Microsoft\Edge\User Data\Default\Bookmarks");
    }
}