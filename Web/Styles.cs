namespace EdgeBookmarksManager.Web;

public static class Styles
{
    public static string GetBaseStyles() => """
        * { box-sizing: border-box; }
        
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            margin: 0;
            padding: 0;
            background-color: var(--background-color);
            color: var(--text-color);
            line-height: 1.5;
            height: 100vh;
            overflow: hidden;
        }

        .container {
            max-width: 1200px;
            margin: 0 auto;
            height: 100vh;
            display: flex;
            flex-direction: column;
        }

        .header {
            position: sticky;
            top: 0;
            z-index: 100;
            background-color: var(--background-color);
            border-bottom: 2px solid var(--border-color);
            padding: 20px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            gap: 20px;
            box-shadow: var(--shadow);
        }

        .content {
            flex: 1;
            overflow-y: auto;
            padding: 20px 20px 0 20px;
        }

        .footer {
            position: sticky;
            bottom: 0;
            z-index: 100;
            background-color: var(--background-color);
            border-top: 2px solid var(--border-color);
            padding: 15px 20px;
            box-shadow: 0 -2px 4px rgba(0,0,0,0.1);
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
            margin-bottom: 20px;
        }

        /* Custom Scrollbar Styling */
        .content::-webkit-scrollbar {
            width: 12px;
        }

        .content::-webkit-scrollbar-track {
            background: var(--surface-color);
            border-radius: 6px;
        }

        .content::-webkit-scrollbar-thumb {
            background: var(--primary-color);
            border-radius: 6px;
            border: 2px solid var(--surface-color);
        }

        .content::-webkit-scrollbar-thumb:hover {
            background: var(--secondary-color);
        }

        /* Firefox scrollbar */
        .content {
            scrollbar-width: thin;
            scrollbar-color: var(--primary-color) var(--surface-color);
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
            cursor: pointer;
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
            pointer-events: none;
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

        @media (max-width: 768px) {
            .header {
                flex-direction: column;
                align-items: stretch;
                padding: 15px;
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

            .content {
                padding: 15px 15px 0 15px;
            }

            .footer {
                padding: 12px 15px;
            }
        }
        """;
}