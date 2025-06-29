using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

string GetBookmarksPath()
{
    var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    return Path.Combine(userProfile, @"AppData\Local\Microsoft\Edge\User Data\Default\Bookmarks");
}

int FindAvailablePort(int startPort = 5005, int maxTrials = 1000)
{
    for (int port = startPort; port < startPort + maxTrials; port++)
    {
        try
        {
            using var listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            listener.Stop();
            return port;
        }
        catch (SocketException)
        {
            // Port is in use, try next one
            continue;
        }
    }
    throw new InvalidOperationException($"No available port found after {maxTrials} trials starting from {startPort}");
}

app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Edge Bookmarks Manager</title>
    <style>
        :root {
            --primary-color: #0078d4;
            --secondary-color: #106ebe;
            --background-color: #ffffff;
            --surface-color: #f5f5f5;
            --text-color: #323130;
            --text-secondary: #605e5c;
            --border-color: #d1d1d1;
            --hover-color: #f3f2f1;
            --shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        * { box-sizing: border-box; }
        
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: var(--background-color);
            color: var(--text-color);
            line-height: 1.5;
        }

        .container {
            max-width: 1200px;
            margin: 0 auto;
        }

        .header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 30px;
            flex-wrap: wrap;
            gap: 20px;
        }

        h1 {
            margin: 0;
            color: var(--primary-color);
            font-size: 2rem;
            font-weight: 600;
        }

        .controls {
            display: flex;
            gap: 15px;
            align-items: center;
            flex-wrap: wrap;
        }

        .search-container {
            position: relative;
            min-width: 300px;
        }

        #searchInput {
            width: 100%;
            padding: 12px 40px 12px 16px;
            border: 2px solid var(--border-color);
            border-radius: 8px;
            font-size: 16px;
            background-color: var(--background-color);
            color: var(--text-color);
            transition: border-color 0.2s, box-shadow 0.2s;
        }

        #searchInput::placeholder {
            color: var(--text-secondary);
            opacity: 0.7;
        }

        #searchInput:focus {
            outline: none;
            border-color: var(--primary-color);
            box-shadow: 0 0 0 3px rgba(0, 120, 212, 0.1);
        }

        .search-icon {
            position: absolute;
            right: 12px;
            top: 50%;
            transform: translateY(-50%);
            color: var(--text-secondary);
        }

        #themeSelector {
            padding: 10px 16px;
            border: 2px solid var(--border-color);
            border-radius: 8px;
            background-color: var(--background-color);
            color: var(--text-color);
            font-size: 14px;
            cursor: pointer;
            min-width: 180px;
        }

        #themeSelector option {
            background-color: var(--background-color);
            color: var(--text-color);
        }

        .stats {
            color: var(--text-secondary);
            font-size: 14px;
            margin-bottom: 20px;
        }

        .bookmarks-container {
            background: var(--surface-color);
            border-radius: 12px;
            padding: 20px;
            box-shadow: var(--shadow);
        }

        .folder {
            font-weight: 600;
            color: var(--primary-color);
            margin: 20px 0 10px 0;
            padding: 10px;
            background: linear-gradient(90deg, var(--primary-color)10, transparent);
            border-radius: 6px;
            border-left: 4px solid var(--primary-color);
            font-size: 1.1em;
        }

        .bookmark {
            margin: 8px 0;
            padding: 12px;
            background: var(--background-color);
            border: 1px solid var(--border-color);
            border-radius: 8px;
            transition: all 0.2s;
            display: flex;
            align-items: center;
            gap: 12px;
        }

        .bookmark:hover, .bookmark.selected {
            background: var(--hover-color);
            border-color: var(--primary-color);
            transform: translateY(-1px);
            box-shadow: var(--shadow);
        }

        .bookmark.selected {
            background: var(--primary-color);
            color: var(--background-color);
        }

        .bookmark.selected .url {
            color: var(--background-color);
        }

        .bookmark.selected .bookmark-url {
            color: var(--background-color);
            opacity: 0.8;
        }

        .bookmark-icon {
            width: 16px;
            height: 16px;
            background: var(--primary-color);
            border-radius: 3px;
            flex-shrink: 0;
        }

        .url {
            color: var(--text-color);
            text-decoration: none;
            font-weight: 500;
            flex: 1;
            word-break: break-word;
        }

        .url:hover {
            color: var(--primary-color);
        }

        .bookmark-url {
            font-size: 12px;
            color: var(--text-secondary);
            margin-top: 4px;
            word-break: break-all;
        }

        .no-results {
            text-align: center;
            padding: 40px;
            color: var(--text-secondary);
            font-style: italic;
        }

        /* Theme Styles */
        .theme-default { /* Already defined in :root */ }
        
        .theme-dark {
            --primary-color: #4fc3f7;
            --secondary-color: #29b6f6;
            --background-color: #121212;
            --surface-color: #1e1e1e;
            --text-color: #ffffff;
            --text-secondary: #b0b0b0;
            --border-color: #333333;
            --hover-color: #2a2a2a;
            --shadow: 0 2px 4px rgba(0,0,0,0.3);
        }

        .theme-blue {
            --primary-color: #2196f3;
            --secondary-color: #1976d2;
            --background-color: #e3f2fd;
            --surface-color: #bbdefb;
            --text-color: #0d47a1;
            --text-secondary: #1565c0;
            --border-color: #90caf9;
            --hover-color: #e1f5fe;
        }

        .theme-green {
            --primary-color: #4caf50;
            --secondary-color: #388e3c;
            --background-color: #e8f5e8;
            --surface-color: #c8e6c9;
            --text-color: #1b5e20;
            --text-secondary: #2e7d32;
            --border-color: #a5d6a7;
            --hover-color: #f1f8e9;
        }

        .theme-purple {
            --primary-color: #9c27b0;
            --secondary-color: #7b1fa2;
            --background-color: #f3e5f5;
            --surface-color: #e1bee7;
            --text-color: #4a148c;
            --text-secondary: #6a1b9a;
            --border-color: #ce93d8;
            --hover-color: #fce4ec;
        }

        .theme-orange {
            --primary-color: #ff9800;
            --secondary-color: #f57c00;
            --background-color: #fff3e0;
            --surface-color: #ffe0b2;
            --text-color: #e65100;
            --text-secondary: #ef6c00;
            --border-color: #ffcc02;
            --hover-color: #fff8e1;
        }

        .theme-red {
            --primary-color: #f44336;
            --secondary-color: #d32f2f;
            --background-color: #ffebee;
            --surface-color: #ffcdd2;
            --text-color: #b71c1c;
            --text-secondary: #c62828;
            --border-color: #ef9a9a;
            --hover-color: #fce4ec;
        }

        .theme-teal {
            --primary-color: #009688;
            --secondary-color: #00695c;
            --background-color: #e0f2f1;
            --surface-color: #b2dfdb;
            --text-color: #004d40;
            --text-secondary: #00695c;
            --border-color: #80cbc4;
            --hover-color: #e8f5e8;
        }

        .theme-indigo {
            --primary-color: #3f51b5;
            --secondary-color: #303f9f;
            --background-color: #e8eaf6;
            --surface-color: #c5cae9;
            --text-color: #1a237e;
            --text-secondary: #283593;
            --border-color: #9fa8da;
            --hover-color: #f3f4f9;
        }

        .theme-pink {
            --primary-color: #e91e63;
            --secondary-color: #c2185b;
            --background-color: #fce4ec;
            --surface-color: #f8bbd9;
            --text-color: #880e4f;
            --text-secondary: #ad1457;
            --border-color: #f48fb1;
            --hover-color: #fff0f3;
        }

        .theme-cyan {
            --primary-color: #00bcd4;
            --secondary-color: #0097a7;
            --background-color: #e0f7fa;
            --surface-color: #b2ebf2;
            --text-color: #006064;
            --text-secondary: #00838f;
            --border-color: #80deea;
            --hover-color: #e8f8f8;
        }

        .theme-lime {
            --primary-color: #cddc39;
            --secondary-color: #afb42b;
            --background-color: #f9fbe7;
            --surface-color: #f0f4c3;
            --text-color: #33691e;
            --text-secondary: #689f38;
            --border-color: #dce775;
            --hover-color: #fcfdf4;
        }

        .theme-amber {
            --primary-color: #ffc107;
            --secondary-color: #ffa000;
            --background-color: #fffbf0;
            --surface-color: #ffecb3;
            --text-color: #ff6f00;
            --text-secondary: #ff8f00;
            --border-color: #ffd54f;
            --hover-color: #fffdf7;
        }

        .theme-brown {
            --primary-color: #795548;
            --secondary-color: #5d4037;
            --background-color: #efebe9;
            --surface-color: #d7ccc8;
            --text-color: #3e2723;
            --text-secondary: #4e342e;
            --border-color: #bcaaa4;
            --hover-color: #f5f5f5;
        }

        .theme-grey {
            --primary-color: #607d8b;
            --secondary-color: #455a64;
            --background-color: #eceff1;
            --surface-color: #cfd8dc;
            --text-color: #263238;
            --text-secondary: #37474f;
            --border-color: #b0bec5;
            --hover-color: #f8f9fa;
        }

        .theme-neon {
            --primary-color: #00ff41;
            --secondary-color: #00cc33;
            --background-color: #0a0a0a;
            --surface-color: #1a1a1a;
            --text-color: #e0ffe0;
            --text-secondary: #a0ffa0;
            --border-color: #003d0f;
            --hover-color: #001a06;
            --shadow: 0 0 10px rgba(0, 255, 65, 0.3);
        }

        .theme-ocean {
            --primary-color: #006494;
            --secondary-color: #003d5b;
            --background-color: #caf0f8;
            --surface-color: #90e0ef;
            --text-color: #003049;
            --text-secondary: #0077b6;
            --border-color: #48cae4;
            --hover-color: #e0fbfc;
        }

        .theme-sunset {
            --primary-color: #f72585;
            --secondary-color: #b5179e;
            --background-color: #2a0a1a;
            --surface-color: #3d1526;
            --text-color: #ffd6e8;
            --text-secondary: #ffb3d9;
            --border-color: #7d1f4a;
            --hover-color: #4a1a2f;
        }

        .theme-forest {
            --primary-color: #7cb342;
            --secondary-color: #8bc34a;
            --background-color: #0f1a0a;
            --surface-color: #1a2d12;
            --text-color: #e8f5e8;
            --text-secondary: #c8e6c9;
            --border-color: #2e5d16;
            --hover-color: #243d1a;
        }

        .theme-minimal {
            --primary-color: #000000;
            --secondary-color: #333333;
            --background-color: #ffffff;
            --surface-color: #fafafa;
            --text-color: #000000;
            --text-secondary: #666666;
            --border-color: #e0e0e0;
            --hover-color: #f5f5f5;
            --shadow: 0 1px 3px rgba(0,0,0,0.1);
        }

        @media (max-width: 768px) {
            .header {
                flex-direction: column;
                align-items: stretch;
            }
            
            .controls {
                justify-content: space-between;
            }
            
            .search-container {
                min-width: 200px;
                flex: 1;
            }
            
            #themeSelector {
                min-width: 120px;
            }
        }
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
                    <option value="theme-default">Default</option>
                    <option value="theme-dark">Dark Mode</option>
                    <option value="theme-blue">Ocean Blue</option>
                    <option value="theme-green">Nature Green</option>
                    <option value="theme-purple">Royal Purple</option>
                    <option value="theme-orange">Warm Orange</option>
                    <option value="theme-red">Bold Red</option>
                    <option value="theme-teal">Cool Teal</option>
                    <option value="theme-indigo">Deep Indigo</option>
                    <option value="theme-pink">Soft Pink</option>
                    <option value="theme-cyan">Bright Cyan</option>
                    <option value="theme-lime">Fresh Lime</option>
                    <option value="theme-amber">Golden Amber</option>
                    <option value="theme-brown">Earth Brown</option>
                    <option value="theme-grey">Modern Grey</option>
                    <option value="theme-neon">Neon Green</option>
                    <option value="theme-ocean">Deep Ocean</option>
                    <option value="theme-sunset">Sunset Gradient</option>
                    <option value="theme-forest">Forest Green</option>
                    <option value="theme-minimal">Minimal Black</option>
                </select>
            </div>
        </div>
        
        <div class="stats" id="stats">Loading bookmarks...</div>
        
        <div class="bookmarks-container">
            <div id="bookmarks">Loading...</div>
        </div>
    </div>

    <script>
        let allBookmarks = [];
        let filteredBookmarks = [];
        let selectedIndex = -1;

        // Load bookmarks
        fetch('/api/bookmarks')
            .then(r => r.json())
            .then(data => {
                allBookmarks = flattenBookmarks(data.roots.bookmark_bar.children);
                filteredBookmarks = [...allBookmarks];
                updateDisplay();
            })
            .catch(error => {
                document.getElementById('bookmarks').innerHTML = '<div class="no-results">Error loading bookmarks: ' + error.message + '</div>';
            });

        function flattenBookmarks(items, path = '') {
            let result = [];
            items.forEach(item => {
                if (item.type === 'folder') {
                    const folderPath = path ? `${path} > ${item.name}` : item.name;
                    if (item.children) {
                        result.push(...flattenBookmarks(item.children, folderPath));
                    }
                } else if (item.url) {
                    result.push({
                        name: item.name,
                        url: item.url,
                        path: path
                    });
                }
            });
            return result;
        }

        function updateDisplay() {
            const container = document.getElementById('bookmarks');
            const stats = document.getElementById('stats');
            
            stats.textContent = `Showing ${filteredBookmarks.length} of ${allBookmarks.length} bookmarks`;
            
            if (filteredBookmarks.length === 0) {
                container.innerHTML = '<div class="no-results">No bookmarks found matching your search.</div>';
                selectedIndex = -1;
                return;
            }
            
            // Reset selection if it's out of bounds
            if (selectedIndex >= filteredBookmarks.length) {
                selectedIndex = -1;
            }
            
            // Group by folder path
            const grouped = {};
            filteredBookmarks.forEach((bookmark, index) => {
                const folder = bookmark.path || 'Bookmarks Bar';
                if (!grouped[folder]) {
                    grouped[folder] = [];
                }
                grouped[folder].push({...bookmark, globalIndex: index});
            });
            
            let html = '';
            Object.keys(grouped).sort().forEach(folder => {
                html += `<div class="folder">📁 ${folder}</div>`;
                grouped[folder].forEach(bookmark => {
                    const domain = extractDomain(bookmark.url);
                    const isSelected = bookmark.globalIndex === selectedIndex ? 'selected' : '';
                    html += `
                        <div class="bookmark ${isSelected}" data-index="${bookmark.globalIndex}">
                            <div class="bookmark-icon"></div>
                            <div style="flex: 1;">
                                <a href="${bookmark.url}" class="url" target="_blank">${escapeHtml(bookmark.name)}</a>
                                <div class="bookmark-url">${escapeHtml(domain)}</div>
                            </div>
                        </div>
                    `;
                });
            });
            
            container.innerHTML = html;
            
            // Scroll selected item into view
            if (selectedIndex >= 0) {
                const selectedElement = container.querySelector(`[data-index="${selectedIndex}"]`);
                if (selectedElement) {
                    selectedElement.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
                }
            }
        }

        function extractDomain(url) {
            try {
                return new URL(url).hostname;
            } catch {
                return url;
            }
        }

        function escapeHtml(text) {
            const div = document.createElement('div');
            div.textContent = text;
            return div.innerHTML;
        }

        // Search functionality
        document.getElementById('searchInput').addEventListener('input', function(e) {
            const query = e.target.value.toLowerCase().trim();
            
            if (!query) {
                filteredBookmarks = [...allBookmarks];
            } else {
                const words = query.split(/\s+/).filter(word => word.length > 0);
                
                filteredBookmarks = allBookmarks.filter(bookmark => {
                    const searchText = `${bookmark.name} ${bookmark.path} ${extractDomain(bookmark.url)}`.toLowerCase();
                    return words.every(word => searchText.includes(word));
                });
            }
            
            selectedIndex = -1; // Reset selection when search changes
            updateDisplay();
        });

        // Keyboard navigation
        document.getElementById('searchInput').addEventListener('keydown', function(e) {
            if (filteredBookmarks.length === 0) return;
            
            switch(e.key) {
                case 'Enter':
                    e.preventDefault();
                    if (filteredBookmarks.length === 1) {
                        // Auto-navigate when only one result
                        window.open(filteredBookmarks[0].url, '_blank');
                        this.value = '';
                        filteredBookmarks = [...allBookmarks];
                        selectedIndex = -1;
                        updateDisplay();
                    } else if (selectedIndex >= 0) {
                        // Navigate to selected bookmark
                        window.open(filteredBookmarks[selectedIndex].url, '_blank');
                    } else if (filteredBookmarks.length > 0) {
                        // Select first item if none selected
                        selectedIndex = 0;
                        updateDisplay();
                    }
                    break;
                    
                case 'Tab':
                    e.preventDefault();
                    if (selectedIndex === -1 && filteredBookmarks.length > 0) {
                        selectedIndex = 0;
                        updateDisplay();
                    }
                    break;
                    
                case 'ArrowDown':
                    e.preventDefault();
                    if (filteredBookmarks.length > 0) {
                        selectedIndex = selectedIndex === -1 ? 0 : Math.min(selectedIndex + 1, filteredBookmarks.length - 1);
                        updateDisplay();
                    }
                    break;
                    
                case 'ArrowUp':
                    e.preventDefault();
                    if (filteredBookmarks.length > 0 && selectedIndex > 0) {
                        selectedIndex = Math.max(selectedIndex - 1, 0);
                        updateDisplay();
                    } else if (selectedIndex === 0) {
                        selectedIndex = -1;
                        updateDisplay();
                    }
                    break;
                    
                case 'Escape':
                    selectedIndex = -1;
                    updateDisplay();
                    break;
            }
        });

        // Theme selector
        document.getElementById('themeSelector').addEventListener('change', function(e) {
            document.body.className = e.target.value;
            localStorage.setItem('selectedTheme', e.target.value);
        });

        // Load saved theme
        const savedTheme = localStorage.getItem('selectedTheme');
        if (savedTheme) {
            document.body.className = savedTheme;
            document.getElementById('themeSelector').value = savedTheme;
        }

        // Auto-focus search input when page loads
        document.getElementById('searchInput').focus();
    </script>
</body>
</html>
""", "text/html"));

app.MapGet("/api/bookmarks", () =>
{
    var bookmarksPath = GetBookmarksPath();
    if (!File.Exists(bookmarksPath))
        return Results.NotFound("Bookmarks file not found");
    
    var json = File.ReadAllText(bookmarksPath);
    var bookmarks = JsonSerializer.Deserialize<BookmarksRoot>(json, AppJsonSerializerContext.Default.BookmarksRoot);
    return Results.Ok(bookmarks);
});

var port = FindAvailablePort();
Console.WriteLine($"Starting server on http://localhost:{port}");
app.Run($"http://localhost:{port}");

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

[JsonSerializable(typeof(BookmarksRoot))]
[JsonSerializable(typeof(BookmarkRoots))]
[JsonSerializable(typeof(BookmarkFolder))]
[JsonSerializable(typeof(BookmarkItem))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}