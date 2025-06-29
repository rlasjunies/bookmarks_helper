namespace EdgeBookmarksManager.Web;

public static class Themes
{
    public static string GetThemeStyles() => """
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
        """;

    public static string GetThemeOptions() => """
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
        """;
}