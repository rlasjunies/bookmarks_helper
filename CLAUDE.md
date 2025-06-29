# Edge Bookmarks Manager

A .NET 9 web application that provides a local web server to manage Microsoft Edge bookmarks.

## Project Structure
- `EdgeBookmarksManager.csproj` - .NET 9 project file with AOT compilation support
- `Program.cs` - Clean entry point and application startup (20 lines)
- `GlobalUsings.cs` - Centralized using statements for all files
- `Models/` - Data models and JSON serialization context
  - `BookmarkModels.cs` - Bookmark data structures
  - `JsonContext.cs` - AOT-compatible JSON serialization context
- `Services/` - Business logic and utility services
  - `BookmarkService.cs` - Bookmark file operations
  - `PortService.cs` - Automatic port detection logic
- `Controllers/` - API endpoint definitions
  - `BookmarkController.cs` - Bookmark API endpoints
- `Web/` - Frontend assets and content
  - `HtmlContent.cs` - Main HTML page template
  - `Themes.cs` - All 20 theme definitions separated
  - `Styles.cs` - Base CSS styles and layout
  - `JavaScript.cs` - Complete client-side functionality

## Key Features
- AOT (Ahead-of-Time) compilation for fast startup
- Single-file deployment with .NET 9 support
- Automatic port detection (starts from 5005, tries up to 1000 ports)
- Reads Edge bookmarks from: `%USERPROFILE%\AppData\Local\Microsoft\Edge\User Data\Default\Bookmarks`
- Modern responsive web interface with 20 beautiful themes
- Advanced multi-word search and filtering with keyboard navigation
- Real-time bookmark statistics in sticky footer
- Theme persistence (saves user preference in localStorage)
- Organized folder-based bookmark display with visual grouping
- Full keyboard navigation support (arrows, enter, tab, escape, F2)
- Smart auto-navigation for single search results
- Visual selection highlighting with smooth auto-scroll
- Intelligent focus management with Modern Navigation API detection
- Bookmarks open in current tab (not new tabs)
- High contrast support for all themes including dark modes
- Sticky header with fixed search bar and theme selector
- Sticky footer with bookmark count statistics
- Scrollable bookmark list with full viewport height utilization
- Custom themed scrollbars that match selected theme colors
- Full-row clickable bookmark cards for easy interaction
- Smart navigation order that follows visual layout with robust fallback support
- Session-based focus restoration after browser back/forward navigation
- F2 keyboard shortcut for quick search access with text selection
- Modern Navigation API integration for precise navigation type detection
- Custom favicon and page title branding

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
- **Bookmark name search**: Searches only in bookmark names/titles (not folder paths or domains)
- **Real-time filtering**: Results update instantly as you type
- **Logical AND matching**: Each word must be found, but can match any part of the text
- **Case-insensitive**: Search works regardless of capitalization
- **Statistics display**: Shows "X of Y bookmarks" count
- **No results message**: Clear feedback when no matches found

## Keyboard Navigation
- **Intelligent Focus Management**: 
  - Fresh page load: Search bar focused, ready for typing
  - Return from navigation: Search bar focused + previous selection restored
- **Smart Enter**: 
  - 1 result: Auto-navigate immediately
  - Multiple results + selection: Navigate to selected bookmark
  - Multiple results + no selection: Select first bookmark
- **Visual Arrow Navigation**: ↑/↓ keys follow visual order through folders
  - Works immediately from restored selections with robust fallback
  - Seamless navigation from any starting point
  - Graceful degradation when visual mapping unavailable
- **Tab Selection**: Press Tab to select first bookmark in results
- **F2 Quick Access**: Press F2 to focus search bar and select all text
- **Visual Selection**: Selected bookmark highlighted with primary theme color
- **Smooth Auto-scroll**: Selected bookmark automatically scrolled into view
- **Escape**: Clear selection and return to search
- **Current Tab Navigation**: All bookmark navigation opens in current tab

## Focus Management & State Restoration
- **Modern Navigation API Integration**: Uses latest web standards for precise navigation detection
  - `navigate`: Fresh page loads, direct URL entry
  - `reload`: Page refresh (F5, Ctrl+R)
  - `traverse`: Back/forward button navigation
- **Dual Focus Strategy**: Different focus behavior based on navigation type
  - Fresh page load/reload: Search bar focused, no selection
  - Back/forward navigation: Search bar focused + bookmark selection restored
- **Session Persistence**: Remembers last selected bookmark and search query
- **Smart Restoration**: Restores exact state when returning from bookmark navigation
- **Robust Arrow Navigation**: Works immediately from restored selections with fallback
- **Time-based Expiry**: Automatically clears old state after 30 seconds
- **URL-based Matching**: Finds exact bookmark even in filtered results
- **Consistent Input**: Search bar always ready for typing
- **Keyboard-Friendly**: Full keyboard navigation available immediately

## API Endpoints
- `GET /` - Modern web interface with themes and search (HTML)
- `GET /api/bookmarks` - Returns bookmarks as JSON with nested folder structure

## Technical Features
- **Port Auto-detection**: Automatically finds available port starting from 5005 (up to 1000 trials)
- **Modular Architecture**: Clean separation of concerns with organized file structure
  - Models for data structures and JSON serialization
  - Services for business logic and utilities
  - Controllers for API endpoint management
  - Web layer for frontend assets (HTML, CSS, JS, themes)
- **Global Using Statements**: Centralized dependency management across all files
- **Modern Web APIs**: Integration with latest browser standards
  - Modern Navigation API for precise navigation type detection
  - Graceful fallback for unsupported browsers
- **CSS Custom Properties**: Complete theme system using CSS variables for consistency
- **Dual Storage Systems**: 
  - localStorage for theme preferences (persistent)
  - sessionStorage for navigation state (temporary)
- **Responsive Design**: Mobile-friendly interface with adaptive layouts and touch optimization
- **Error Handling**: Graceful handling of missing bookmark files and parsing errors
- **Security**: HTML escaping to prevent XSS attacks on bookmark names and URLs
- **Advanced Accessibility**: 
  - Smart focus management with state restoration
  - F2 keyboard shortcut for quick search access
  - High contrast themes for visually impaired users
  - Full keyboard navigation support with robust fallbacks
  - Semantic HTML structure
- **Form Styling**: Proper contrast for input fields and dropdowns across all 20 themes
- **Current Tab Navigation**: All bookmark links navigate in same tab (no tab clutter)
- **Fixed Layout System**: 
  - Sticky header with search and theme controls
  - Sticky footer with real-time statistics
  - Scrollable content area with full viewport utilization
- **Custom UI Components**:
  - Themed scrollbars that match selected theme colors
  - Cross-browser scrollbar support (WebKit + Firefox)
  - Full-row clickable bookmark cards
  - Smooth hover and selection animations
- **Smart Navigation Logic**:
  - Visual order navigation (follows displayed folder structure)
  - Bidirectional index mapping for keyboard navigation
  - Robust fallback when mapping unavailable
  - Visual selection highlighting with auto-scroll
- **State Management**:
  - Navigation type-aware focus restoration
  - Session-based bookmark selection persistence
  - Search query persistence during navigation
  - Time-based state expiry (30 seconds)
- **Performance Optimizations**:
  - Efficient DOM updates with minimal reflows
  - Optimized search filtering algorithms
  - Smooth animations using CSS transitions
- **Custom Branding**: Emoji favicon (📚) and clean page title
- **Maintainable Codebase**: 
  - Separated themes for easy customization
  - Clean file organization for team collaboration
  - DRY principle with global using statements

## User Experience Highlights
- **Seamless Workflow**: Search → Navigate → Return to exact same state with perfect focus
- **No Context Loss**: Maintains search filters and selections across navigation
- **Intelligent Focus**: Search bar always ready + selections preserved when returning
- **Immediate Navigation**: Arrow keys work instantly from any restored selection with fallbacks
- **Quick Access**: F2 shortcut for instant search focus with text selection
- **Intuitive Behavior**: Visual arrow key movement that matches what users see
- **Efficient Interaction**: Full bookmark cards clickable, not just tiny text links
- **Navigation Type Aware**: Uses Modern Navigation API for precise state management
- **Professional Polish**: Themed scrollbars, smooth animations, consistent styling
- **Accessibility First**: Works great with keyboard-only navigation and robust fallbacks
- **Performance**: Fast filtering, smooth scrolling, responsive interface

## Dependencies
- Microsoft.NET.Sdk.Web (built-in)
- System.Text.Json with source generation for AOT compatibility
- System.Net.Sockets (for port detection)

## File Structure
```
/mnt/c/Users/lasjunie/Documents/GitHub/bookmarks_helper/
├── EdgeBookmarksManager.csproj    # .NET 9 project configuration with AOT support
├── Program.cs                     # Clean entry point (20 lines)
├── GlobalUsings.cs                # Centralized using statements
├── CLAUDE.md                      # This documentation file
├── .gitignore                     # Git ignore patterns for .NET projects
├── Models/
│   ├── BookmarkModels.cs         # Bookmark data structures
│   └── JsonContext.cs            # AOT-compatible JSON serialization
├── Services/
│   ├── BookmarkService.cs        # Bookmark file operations
│   └── PortService.cs            # Port detection logic
├── Controllers/
│   └── BookmarkController.cs     # API endpoint handlers
└── Web/
    ├── HtmlContent.cs            # Main HTML page template
    ├── Themes.cs                 # All 20 theme definitions
    ├── Styles.cs                 # Base CSS styles and layout
    └── JavaScript.cs             # Complete client-side functionality
```

## Code Organization Benefits
- **Separation of Concerns**: Each layer has a distinct responsibility
- **Maintainability**: Easy to locate and modify specific functionality
- **Team Collaboration**: Multiple developers can work on different areas
- **Theme Management**: All 20 themes isolated for easy customization
- **DRY Principle**: Global usings eliminate duplicate imports
- **AOT Compatibility**: Modular structure fully supports AOT compilation
- **Scalability**: Easy to add new features, themes, or services