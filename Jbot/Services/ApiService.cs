using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Jbot.Services;

public class ApiService
{
    private readonly string _baseUrl;
    private readonly string _applicationKey;
    private readonly string _login;
    private readonly string _password;

    private readonly HttpClient _http;
    private string? _token;
    public bool IsAuthorized => !string.IsNullOrEmpty(_token);
    public string? Token => _token;

    private static readonly JsonSerializerOptions PrettyJson = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public ApiService()
    {
        EnvConfig.Load();

        _baseUrl = EnvConfig.Get("API_BASE_URL", "https://msapi.top-academy.ru/api/v2/");
        _applicationKey = EnvConfig.Get("APPLICATION_KEY");
        _login = EnvConfig.Get("USERNAME");
        _password = EnvConfig.Get("PASSWORD");

        _http = new HttpClient
        {
            BaseAddress = new Uri(_baseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
        _http.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
        _http.DefaultRequestHeaders.Add("Referer", "https://journal.top-academy.ru/");
        _http.DefaultRequestHeaders.Add("Origin", "https://journal.top-academy.ru");
    }

    // ── Auth ──
    public async Task<JsonElement?> LoginAsync()
    {
        var body = new Dictionary<string, string?>
        {
            ["application_key"] = _applicationKey,
            ["id_city"] = null,
            ["username"] = _login,
            ["password"] = _password
        };
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync("auth/login", content);
        var responseBody = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(responseBody);
        var root = doc.RootElement;

        if (root.TryGetProperty("access_token", out var tokenProp))
        {
            _token = tokenProp.GetString();
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
        return root.Clone();
    }

    // ── Generic GET ──
    public async Task<JsonElement> GetAsync(string endpoint)
    {
        var response = await _http.GetAsync(endpoint);
        var body = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(body);
        return doc.RootElement.Clone();
    }

    public async Task<string> GetRawAsync(string endpoint)
    {
        var response = await _http.GetAsync(endpoint);
        var body = await response.Content.ReadAsStringAsync();
        // Pretty-print if JSON
        try
        {
            var doc = JsonDocument.Parse(body);
            return JsonSerializer.Serialize(doc, PrettyJson);
        }
        catch { return body; }
    }

    // ── Specific endpoints ──
    public Task<JsonElement> GetUserInfoAsync() => GetAsync("settings/user-info");
    public Task<JsonElement> GetAttendanceStatAsync() => GetAsync("dashboard/progress/attendance-statistic");
    public Task<JsonElement> GetLeaderGroupAsync() => GetAsync("dashboard/progress/leader-group");
    public Task<JsonElement> GetActivityAsync() => GetAsync("dashboard/progress/activity");
    public Task<JsonElement> GetScheduleTodayAsync() => GetAsync("schedule/operations/get-by-date");
    public Task<JsonElement> GetScheduleMonthAsync() => GetAsync("schedule/operations/get-month");
    public Task<JsonElement> GetStudentVisitsAsync() => GetAsync("progress/operations/student-visits");
    public Task<JsonElement> GetStudentExamsAsync() => GetAsync("progress/operations/student-exams");
    public Task<JsonElement> GetChartProgressAsync() => GetAsync("dashboard/chart/progress");
    public Task<JsonElement> GetHomeworkCountAsync() => GetAsync("count/homework");
    public Task<JsonElement> GetHomeworkListAsync(int page = 1, string? status = null, int type = 0)
    {
        var url = $"homework/operations/list?page={page}&type={type}";
        if (!string.IsNullOrEmpty(status)) url += $"&status={status}";
        return GetAsync(url);
    }
    public Task<JsonElement> GetLatestNewsAsync() => GetAsync("news/operations/latest-news");
    public Task<JsonElement> GetGroupSpecsAsync() => GetAsync("settings/group-specs");
    public Task<JsonElement> GetPageCountersAsync() => GetAsync("count/page-counters");
    public Task<JsonElement> GetLeaderStreamAsync() => GetAsync("dashboard/progress/leader-stream");
    public Task<JsonElement> GetAchievementsAsync() => GetAsync("profile/statistic/student-achievements");
}
