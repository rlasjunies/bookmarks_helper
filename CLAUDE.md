# Edge Bookmarks Manager

A .NET 9 web application that provides a local web server to manage Microsoft Edge bookmarks.

## Project Structure
- `EdgeBookmarksManager.csproj` - .NET 9 project file with AOT compilation support
- `Program.cs` - Main application with web server and bookmark management

## Key Features
- AOT (Ahead-of-Time) compilation for fast startup
- Single-file deployment
- Automatic port detection (starts from 5000, tries up to 1000 ports)
- Reads Edge bookmarks from: `%USERPROFILE%\AppData\Local\Microsoft\Edge\User Data\Default\Bookmarks`
- Modern responsive web interface with 20 beautiful themes
- Advanced multi-word search and filtering with keyboard navigation
- Real-time bookmark statistics
- Theme persistence (saves user preference)
- Organized folder-based bookmark display
- Full keyboard navigation support (arrows, enter, tab, escape)
- Smart auto-navigation for single results
- Visual selection highlighting with auto-scroll
- Auto-focus search bar on page load
- Bookmarks open in new tabs
- High contrast support for dark themes

## Build Commands
```bashù
# Run in development
dotnet run

# Build for release
dotnet build -c Release

# Publish as AOT single file
dotnet publish -c Release

# Publish for specific runtime (Windows x64)
dotnet publish -c Release -r win-x64 --self-contained

dotnet publish EdgeBookmarksManager.csproj  -c Release -r win-x64 --self-contained
```

## Project Configuration
- Target Framework: .NET 9
- AOT Compilation: Enabled
- Single File: Enabled
- Self-Contained: Enabled
- Trimmed: Enabled
- Invariant Globalization: Enabled (for AOT compatibility)

## Available Themes (20 total)
1. **Default** - Clean Microsoft-style
2. **Dark Mode** - Easy on the eyes  
3. **Ocean Blue** - Calming blue tones
4. **Nature Green** - Fresh and natural
5. **Royal Purple** - Elegant and rich
6. **Warm Orange** - Energetic and vibrant
7. **Bold Red** - Strong and confident
8. **Cool Teal** - Modern and professional
9. **Deep Indigo** - Sophisticated dark blue
10. **Soft Pink** - Gentle and warm
11. **Bright Cyan** - Fresh and modern
12. **Fresh Lime** - Vibrant and energetic
13. **Golden Amber** - Warm and inviting
14. **Earth Brown** - Natural and grounded
15. **Modern Grey** - Clean and professional
16. **Neon Green** - Futuristic hacker style
17. **Deep Ocean** - Rich aquatic theme
18. **Sunset Gradient** - Warm pink/purple
19. **Forest Green** - Deep nature theme
20. **Minimal Black** - Ultra-clean monochrome

## Search & Filter Features
- **Multi-word search**: Type multiple words, all must match somewhere in the text
- **Full-text search**: Searches bookmark names, folder paths, and website domains
- **Real-time filtering**: Results update instantly as you type
- **Logical AND matching**: Each word must be found, but can match any part of the text
- **Case-insensitive**: Search works regardless of capitalization
- **Statistics display**: Shows "X of Y bookmarks" count
- **No results message**: Clear feedback when no matches found

## Keyboard Navigation
- **Auto-focus**: Search bar automatically focused on page load
- **Smart Enter**: 
  - 1 result: Auto-navigate immediately
  - Multiple results + selection: Navigate to selected bookmark
  - Multiple results + no selection: Select first bookmark
- **Arrow Navigation**: Use ↑/↓ keys to navigate through bookmark list
- **Tab Selection**: Press Tab to select first bookmark in results
- **Visual Selection**: Selected bookmark highlighted with primary theme color
- **Auto-scroll**: Selected bookmark automatically scrolled into view
- **Escape**: Clear selection and return to search
- **New Tab**: All bookmark navigation opens in new tabs

## API Endpoints
- `GET /` - Modern web interface with themes and search (HTML)
- `GET /api/bookmarks` - Returns bookmarks as JSON with nested folder structure

## Technical Features
- **Port auto-detection**: Automatically finds available port starting from 5000
- **CSS Custom Properties**: Theme system using CSS variables for easy customization
- **Local Storage**: Theme preference saved in browser
- **Responsive Design**: Mobile-friendly interface
- **Error Handling**: Graceful handling of missing bookmark files
- **Security**: HTML escaping to prevent XSS attacks
- **Accessibility**: Auto-focus search input, high contrast themes, proper ARIA labels
- **Form Styling**: Proper contrast for input fields and dropdowns in all themes
- **Target Blank**: All bookmark links open in new tabs for better UX

## Dependencies
- Microsoft.NET.Sdk.Web (built-in)
- System.Text.Json with source generation for AOT compatibility
- System.Net.Sockets (for port detection)