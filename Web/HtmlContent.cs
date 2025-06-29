namespace EdgeBookmarksManager.Web;

public static class HtmlContent
{
    public static string GetMainPage() => $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Edge Bookmarks Manager</title>
            <link rel="icon" href="data:image/svg+xml,<svg xmlns=%22http://www.w3.org/2000/svg%22 viewBox=%220 0 100 100%22><text y=%22.9em%22 font-size=%2290%22>📚</text></svg>">
            <style>
                {Themes.GetThemeStyles()}
                {Styles.GetBaseStyles()}
            </style>
        </head>
        <body class="theme-default">
            <div class="container">
                <div class="header">
                    <h1>📚 Edge Bookmarks Manager</h1>
                    <div class="controls">
                        <div class="search-container">
                            <input type="text" id="searchInput" placeholder="Search bookmarks..." autocomplete="off">
                            <span class="search-icon">🔍</span>
                        </div>
                        <select id="themeSelector">
                            {Themes.GetThemeOptions()}
                        </select>
                    </div>
                </div>
                
                <div class="content">
                    <div class="bookmarks-container">
                        <div id="bookmarks">Loading...</div>
                    </div>
                </div>
                
                <div class="footer">
                    <div class="stats" id="stats">Loading bookmarks...</div>
                </div>
            </div>

            <script>
                {JavaScript.GetClientScript()}
            </script>
        </body>
        </html>
        """;
}