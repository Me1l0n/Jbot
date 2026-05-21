namespace Jbot.Services;

/// <summary>
/// Reads configuration from a .env file.
/// Searches in multiple locations: executable directory → project root → solution root.
/// </summary>
public static class EnvConfig
{
    private static readonly Dictionary<string, string> _values = new(StringComparer.OrdinalIgnoreCase);
    private static bool _loaded;

    public static string Get(string key, string defaultValue = "")
        => _values.TryGetValue(key, out var v) ? v : defaultValue;

    public static void Load()
    {
        if (_loaded) return;
        _loaded = true;

        var envPath = FindEnvFile();
        if (envPath == null) return;

        foreach (var line in File.ReadAllLines(envPath))
        {
            var trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith('#'))
                continue;

            var eqIndex = trimmed.IndexOf('=');
            if (eqIndex <= 0) continue;

            var key = trimmed[..eqIndex].Trim();
            var value = trimmed[(eqIndex + 1)..].Trim();

            // Remove surrounding quotes if present
            if (value.Length >= 2 &&
                ((value.StartsWith('"') && value.EndsWith('"')) ||
                 (value.StartsWith('\'') && value.EndsWith('\''))))
            {
                value = value[1..^1];
            }

            _values[key] = value;
        }
    }

    private static string? FindEnvFile()
    {
        // 1. Next to the executable
        var exeDir = AppContext.BaseDirectory;
        var path = Path.Combine(exeDir, ".env");
        if (File.Exists(path)) return path;

        // 2. Walk up from executable to find .env (covers solution root)
        var dir = new DirectoryInfo(exeDir);
        while (dir?.Parent != null)
        {
            dir = dir.Parent;
            path = Path.Combine(dir.FullName, ".env");
            if (File.Exists(path)) return path;

            // Stop if we hit a directory with .git (repo root)
            if (Directory.Exists(Path.Combine(dir.FullName, ".git")))
                break;
        }

        // 3. Current working directory
        path = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        if (File.Exists(path)) return path;

        return null;
    }
}
