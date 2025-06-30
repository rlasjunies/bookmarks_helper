namespace EdgeBookmarksManager.Web;

public static class JavaScript
{
    public static string GetClientScript() => """
        console.log('=== SCRIPT START ===', new Date().toISOString());
        console.log('SCRIPT START - Current URL:', window.location.href);
        console.log('SCRIPT START - Document ready state:', document.readyState);
        console.log('SCRIPT START - Performance navigation type:', performance.navigation ? performance.navigation.type : 'N/A');
        
        let allBookmarks = [];
        let filteredBookmarks = [];
        let selectedIndex = -1;
        let lastNavigatedUrl = null;

        // Load bookmarks
        console.log('SCRIPT - About to fetch bookmarks');
        fetch('/api/bookmarks')
            .then(r => {
                console.log('SCRIPT - Bookmarks fetch response received');
                return r.json();
            })
            .then(data => {
                console.log('SCRIPT - Bookmarks data parsed, calling restoreFocusState');
                allBookmarks = flattenBookmarks(data.roots.bookmark_bar.children);
                filteredBookmarks = [...allBookmarks];
                restoreFocusState();
                updateDisplay();
                // Additional focus management after display is ready
                handlePostDisplayFocus();
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
            
            // Group by folder path but maintain visual order
            const grouped = {};
            filteredBookmarks.forEach((bookmark, index) => {
                const folder = bookmark.path || 'Bookmarks Bar';
                if (!grouped[folder]) {
                    grouped[folder] = [];
                }
                grouped[folder].push({...bookmark, originalIndex: index});
            });
            
            let html = '';
            let visualIndex = 0;
            const visualIndexMap = new Map(); // Maps visual position to original array index
            
            Object.keys(grouped).sort().forEach(folder => {
                html += `<div class="folder">📁 ${folder}</div>`;
                grouped[folder].forEach(bookmark => {
                    const domain = extractDomain(bookmark.url);
                    const isSelected = bookmark.originalIndex === selectedIndex ? 'selected' : '';
                    visualIndexMap.set(visualIndex, bookmark.originalIndex);
                    
                    html += `
                        <div class="bookmark ${isSelected}" data-visual-index="${visualIndex}" data-original-index="${bookmark.originalIndex}" data-url="${escapeHtml(bookmark.url)}" tabindex="-1">
                            <div class="bookmark-icon"></div>
                            <div style="flex: 1;">
                                <div class="url">${escapeHtml(bookmark.name)}</div>
                                <div class="bookmark-url">${escapeHtml(domain)}</div>
                            </div>
                        </div>
                    `;
                    visualIndex++;
                });
            });
            
            container.innerHTML = html;
            
            // Store the visual index map for navigation
            container.visualIndexMap = visualIndexMap;
            container.reverseVisualIndexMap = new Map();
            visualIndexMap.forEach((originalIndex, visualIndex) => {
                container.reverseVisualIndexMap.set(originalIndex, visualIndex);
            });
            
            // Add click handlers to bookmark rows
            container.querySelectorAll('.bookmark').forEach(bookmarkElement => {
                bookmarkElement.addEventListener('click', function() {
                    const url = this.getAttribute('data-url');
                    const originalIndex = parseInt(this.getAttribute('data-original-index'));
                    console.log('CLICK EVENT - Before click: selectedIndex =', selectedIndex, 'Clicked originalIndex =', originalIndex);
                    if (url) {
                        selectedIndex = originalIndex;
                        console.log('CLICK EVENT - After setting: selectedIndex =', selectedIndex, 'URL =', url);
                        updateDisplay(); // Show selection before navigation
                        saveFocusState();
                        window.location.href = url;
                    }
                });
                
                // Add keyboard navigation support for focused bookmarks
                bookmarkElement.addEventListener('keydown', function(e) {
                    const originalIndex = parseInt(this.getAttribute('data-original-index'));
                    selectedIndex = originalIndex; // Ensure selectedIndex is in sync
                    
                    switch(e.key) {
                        case 'Enter':
                        case ' ': // Space bar
                            e.preventDefault();
                            const url = this.getAttribute('data-url');
                            if (url) {
                                saveFocusState();
                                window.location.href = url;
                            }
                            break;
                            
                        case 'ArrowDown':
                        case 'ArrowUp':
                            // Let the existing arrow navigation handle this
                            e.preventDefault();
                            // Trigger the same logic as search input arrow navigation
                            const searchInput = document.getElementById('searchInput');
                            const event = new KeyboardEvent('keydown', { key: e.key, bubbles: true });
                            searchInput.dispatchEvent(event);
                            break;
                            
                        case 'Escape':
                            e.preventDefault();
                            selectedIndex = -1;
                            updateDisplay();
                            document.getElementById('searchInput').focus();
                            break;
                    }
                });
            });
            
            // Scroll selected item into view
            if (selectedIndex >= 0) {
                const selectedElement = container.querySelector(`[data-original-index="${selectedIndex}"]`);
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

        function handlePostDisplayFocus() {
            // Handle focus after the display has been updated
            setTimeout(() => {
                const navigationType = getNavigationType();
                const isBackNavigation = navigationType === 'traverse';
                console.log('XXXXXX FOCUS RESTORATION - selectedIndex =', selectedIndex, 'isBackNavigation =', isBackNavigation);
                
                if (selectedIndex >= 0 && isBackNavigation) {
                    // Back/forward navigation with restored selection - focus the selected bookmark
                    console.log('FOCUS RESTORATION - selectedIndex =', selectedIndex, 'isBackNavigation =', isBackNavigation);
                    const selectedElement = document.querySelector(`[data-original-index="${selectedIndex}"]`);
                    if (selectedElement) {
                        selectedElement.focus();
                        console.log('FOCUS RESTORATION - Focus set to selected bookmark with originalIndex =', selectedIndex, 'Element found:', !!selectedElement);
                    } else {
                        document.getElementById('searchInput').focus();
                        console.log('FOCUS RESTORATION - Fallback to search input - selected element not found for selectedIndex =', selectedIndex);
                    }
                } else {
                    // Fresh load, reload, or push/replace navigation - focus search bar
                    document.getElementById('searchInput').focus();
                    console.log('FOCUS RESTORATION - Fresh navigation, selectedIndex =', selectedIndex, 'navigationType =', navigationType);
                }
            }, 150);
        }

        function saveFocusState() {
            console.log('SAVE FOCUS STATE - selectedIndex =', selectedIndex, 'filteredBookmarks.length =', filteredBookmarks.length);
            if (selectedIndex >= 0 && filteredBookmarks[selectedIndex]) {
                const selectedBookmark = filteredBookmarks[selectedIndex];
                const searchQuery = document.getElementById('searchInput').value;
                console.log('SAVE FOCUS STATE - Saving bookmark URL:', selectedBookmark.url, 'searchQuery:', searchQuery);
                sessionStorage.setItem('bookmarkManagerState', JSON.stringify({
                    selectedUrl: selectedBookmark.url,
                    searchQuery: searchQuery,
                    timestamp: Date.now()
                }));
            } else {
                console.log('SAVE FOCUS STATE - Not saving, invalid selectedIndex or bookmark not found');
            }
        }

        function getNavigationType() {
            // Use Modern Navigation API if available
            console.log('GET NAVIGATION TYPE - window.navigation exists:', 'navigation' in window);
            if ('navigation' in window && window.navigation.currentEntry) {
                const navType = window.navigation.currentEntry.navigationType || 'navigate';
                console.log('GET NAVIGATION TYPE - Modern API result:', navType, 'currentEntry:', window.navigation.currentEntry);
                return navType;
            }
            
            // Fallback: Check if we have saved state in sessionStorage - if so, assume it's back navigation
            const saved = sessionStorage.getItem('bookmarkManagerState');
            if (saved) {
                try {
                    const state = JSON.parse(saved);
                    if (Date.now() - state.timestamp < 30000) {
                        console.log('GET NAVIGATION TYPE - Fallback: Found recent state, assuming traverse');
                        return 'traverse';
                    }
                } catch (e) {
                    // Ignore parsing errors
                }
            }
            
            // Default to navigate if API not available and no saved state
            console.log('GET NAVIGATION TYPE - Using fallback: navigate');
            return 'navigate';
        }

        function restoreFocusState() {
            const navigationType = getNavigationType();
            const isBackNavigation = navigationType === 'traverse';
            console.log('RESTORE FOCUS STATE - Start: navigationType =', navigationType, 'isBackNavigation =', isBackNavigation);
            
            // Only attempt restoration on back/forward navigation
            if (isBackNavigation) {
                const saved = sessionStorage.getItem('bookmarkManagerState');
                console.log('RESTORE FOCUS STATE - SessionStorage state:', saved);
                if (saved) {
                    try {
                        const state = JSON.parse(saved);
                        // Only restore if it's recent (within 30 seconds of page load)
                        if (Date.now() - state.timestamp < 30000) {
                            if (state.searchQuery) {
                                document.getElementById('searchInput').value = state.searchQuery;
                                // Trigger search
                                const query = state.searchQuery.toLowerCase().trim();
                                const words = query.split(/\s+/).filter(word => word.length > 0);
                                filteredBookmarks = allBookmarks.filter(bookmark => {
                                    const searchText = bookmark.name.toLowerCase();
                                    return words.every(word => searchText.includes(word));
                                });
                            }
                            
                            if (state.selectedUrl) {
                                // Find the bookmark with matching URL
                                console.log('STATE RESTORATION - Searching for URL:', state.selectedUrl);
                                const bookmarkIndex = filteredBookmarks.findIndex(b => b.url === state.selectedUrl);
                                console.log('STATE RESTORATION - Found bookmark at index:', bookmarkIndex, 'in filteredBookmarks.length:', filteredBookmarks.length);
                                if (bookmarkIndex >= 0) {
                                    selectedIndex = bookmarkIndex;
                                    console.log('STATE RESTORATION - selectedIndex set to:', selectedIndex);
                                    // Clear the state after successful restoration
                                    sessionStorage.removeItem('bookmarkManagerState');
                                    return; // Selection restored, focus will be handled in handlePostDisplayFocus
                                }
                            }
                            // Clear the state after use
                            sessionStorage.removeItem('bookmarkManagerState');
                        }
                    } catch (e) {
                        console.log('RESTORE FOCUS STATE - Error parsing saved state:', e);
                    }
                } else {
                    console.log('RESTORE FOCUS STATE - No saved state found in sessionStorage');
                }
            } else {
                console.log('RESTORE FOCUS STATE - Not back navigation, skipping restore');
            }
            
            // Focus will be handled in handlePostDisplayFocus after display is ready
        }

        // Search functionality
        document.getElementById('searchInput').addEventListener('input', function(e) {
            const query = e.target.value.toLowerCase().trim();
            
            if (!query) {
                filteredBookmarks = [...allBookmarks];
            } else {
                const words = query.split(/\s+/).filter(word => word.length > 0);
                
                filteredBookmarks = allBookmarks.filter(bookmark => {
                    const searchText = bookmark.name.toLowerCase();
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
                        saveFocusState();
                        window.location.href = filteredBookmarks[0].url;
                    } else if (selectedIndex >= 0) {
                        // Navigate to selected bookmark
                        saveFocusState();
                        window.location.href = filteredBookmarks[selectedIndex].url;
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
                        const container = document.getElementById('bookmarks');
                        // Ensure we have visual index mapping, fallback to simple increment
                        if (container.visualIndexMap && container.reverseVisualIndexMap) {
                            if (selectedIndex === -1) {
                                // Start from first item
                                selectedIndex = container.visualIndexMap.get(0) || 0;
                            } else {
                                // Move to next item in visual order
                                const currentVisualIndex = container.reverseVisualIndexMap.get(selectedIndex);
                                console.log('ArrowDown - selectedIndex:', selectedIndex, 'currentVisualIndex:', currentVisualIndex);
                                if (currentVisualIndex !== undefined) {
                                    const maxVisualIndex = container.visualIndexMap.size - 1;
                                    const nextVisualIndex = Math.min(currentVisualIndex + 1, maxVisualIndex);
                                    const newSelectedIndex = container.visualIndexMap.get(nextVisualIndex);
                                    console.log('Moving from visual', currentVisualIndex, 'to', nextVisualIndex, 'selectedIndex', selectedIndex, '->', newSelectedIndex);
                                    selectedIndex = newSelectedIndex || selectedIndex;
                                } else {
                                    // Fallback: simple increment
                                    console.log('Using fallback increment');
                                    selectedIndex = Math.min(selectedIndex + 1, filteredBookmarks.length - 1);
                                }
                            }
                        } else {
                            // Fallback when no mapping available
                            if (selectedIndex === -1) {
                                selectedIndex = 0;
                            } else {
                                selectedIndex = Math.min(selectedIndex + 1, filteredBookmarks.length - 1);
                            }
                        }
                        updateDisplay();
                        // Focus the newly selected bookmark if it exists
                        setTimeout(() => {
                            const newSelectedElement = document.querySelector(`[data-original-index="${selectedIndex}"]`);
                            if (newSelectedElement && document.activeElement !== document.getElementById('searchInput')) {
                                newSelectedElement.focus();
                            }
                        }, 50);
                    }
                    break;
                    
                case 'ArrowUp':
                    e.preventDefault();
                    if (filteredBookmarks.length > 0) {
                        const container = document.getElementById('bookmarks');
                        if (selectedIndex === -1) {
                            // Do nothing when no selection and pressing up
                            return;
                        } else {
                            // Move to previous item in visual order
                            if (container.visualIndexMap && container.reverseVisualIndexMap) {
                                const currentVisualIndex = container.reverseVisualIndexMap.get(selectedIndex);
                                console.log('ArrowUp - selectedIndex:', selectedIndex, 'currentVisualIndex:', currentVisualIndex);
                                if (currentVisualIndex !== undefined && currentVisualIndex > 0) {
                                    const prevVisualIndex = currentVisualIndex - 1;
                                    const newSelectedIndex = container.visualIndexMap.get(prevVisualIndex);
                                    console.log('Moving from visual', currentVisualIndex, 'to', prevVisualIndex, 'selectedIndex', selectedIndex, '->', newSelectedIndex);
                                    selectedIndex = newSelectedIndex || selectedIndex;
                                    updateDisplay();
                                    // Focus the newly selected bookmark if it exists
                                    setTimeout(() => {
                                        const newSelectedElement = document.querySelector(`[data-original-index="${selectedIndex}"]`);
                                        if (newSelectedElement && document.activeElement !== document.getElementById('searchInput')) {
                                            newSelectedElement.focus();
                                        }
                                    }, 50);
                                } else if (currentVisualIndex === 0) {
                                    // At first item, clear selection
                                    selectedIndex = -1;
                                    updateDisplay();
                                } else {
                                    // Fallback: simple decrement
                                    if (selectedIndex > 0) {
                                        selectedIndex = selectedIndex - 1;
                                        updateDisplay();
                                        // Focus the newly selected bookmark if it exists
                                        setTimeout(() => {
                                            const newSelectedElement = document.querySelector(`[data-original-index="${selectedIndex}"]`);
                                            if (newSelectedElement && document.activeElement !== document.getElementById('searchInput')) {
                                                newSelectedElement.focus();
                                            }
                                        }, 50);
                                    } else {
                                        selectedIndex = -1;
                                        updateDisplay();
                                    }
                                }
                            } else {
                                // Fallback when no mapping available
                                if (selectedIndex > 0) {
                                    selectedIndex = selectedIndex - 1;
                                    updateDisplay();
                                    // Focus the newly selected bookmark if it exists
                                    setTimeout(() => {
                                        const newSelectedElement = document.querySelector(`[data-original-index="${selectedIndex}"]`);
                                        if (newSelectedElement && document.activeElement !== document.getElementById('searchInput')) {
                                            newSelectedElement.focus();
                                        }
                                    }, 50);
                                } else {
                                    selectedIndex = -1;
                                    updateDisplay();
                                }
                            }
                        }
                    }
                    break;
                    
                case 'Escape':
                    selectedIndex = -1;
                    updateDisplay();
                    break;
            }
        });

        // Global keyboard shortcuts
        document.addEventListener('keydown', function(e) {
            if (e.key === 'F2') {
                e.preventDefault();
                const searchInput = document.getElementById('searchInput');
                searchInput.focus();
                searchInput.select();
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

        // Focus handling is now done in restoreFocusState()
        
        // Additional debugging for page load events
        document.addEventListener('DOMContentLoaded', function() {
            console.log('EVENT - DOMContentLoaded fired');
        });
        
        window.addEventListener('load', function() {
            console.log('EVENT - window.load fired');
        });
        
        window.addEventListener('pageshow', function(event) {
            console.log('EVENT - pageshow fired, persisted:', event.persisted);
            if (event.persisted) {
                // Page was restored from cache (back/forward navigation)
                console.log('PAGESHOW - Page restored from cache, calling restore functions');
                if (allBookmarks.length > 0) {
                    // Data already loaded, just restore state
                    restoreFocusState();
                    updateDisplay();
                    handlePostDisplayFocus();
                } else {
                    console.log('PAGESHOW - No bookmark data, will be handled by fetch');
                }
            }
        });
        
        window.addEventListener('pagehide', function(event) {
            console.log('EVENT - pagehide fired, persisted:', event.persisted);
        });
        """;
}