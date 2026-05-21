using System.Text.Json;
using Jbot.Services;

namespace Jbot.Views;

public partial class DashboardPage : ContentPage
{
    private readonly ApiService _api;

    public DashboardPage(ApiService api)
    {
        InitializeComponent();
        _api = api;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!_api.IsAuthorized) await _api.LoginAsync();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            Loader.IsRunning = true;
            Loader.IsVisible = true;

            var userTask = _api.GetUserInfoAsync();
            var attTask = _api.GetAttendanceStatAsync();
            var leadersTask = _api.GetLeaderGroupAsync();
            var activityTask = _api.GetActivityAsync();
            var chartTask = _api.GetChartProgressAsync();

            await Task.WhenAll(userTask, attTask, leadersTask, activityTask, chartTask);

            FillProfile(userTask.Result);
            FillAttendance(attTask.Result);
            FillProgressChart(chartTask.Result);
            FillLeaders(leadersTask.Result);
            FillActivity(activityTask.Result);
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = $"Ошибка: {ex.Message}";
            ErrorLabel.IsVisible = true;
        }
        finally
        {
            Loader.IsRunning = false;
            Loader.IsVisible = false;
        }
    }

    private void FillProfile(JsonElement d)
    {
        NameLabel.Text = Str(d, "full_name", "—");
        GroupLabel.Text = $"{Str(d, "group_name")}  ·  {Str(d, "stream_name")}";
        LevelLabel.Text = $"Уровень {Int(d, "level")}";

        int pts = 0;
        if (d.TryGetProperty("gaming_points", out var gp) && gp.ValueKind == JsonValueKind.Array)
            foreach (var p in gp.EnumerateArray()) pts += Int(p, "points");
        PointsLabel.Text = $"{pts:N0} очков";

        var photo = Str(d, "photo");
        if (!string.IsNullOrEmpty(photo))
            try { AvatarImage.Source = ImageSource.FromUri(new Uri(photo)); } catch { }

        ProfileCard.IsVisible = true;
    }

    private void FillAttendance(JsonElement d)
    {
        AttWeekLabel.Text = $"{Int(d, "statWeek")}%";
        AttMonthLabel.Text = $"{Int(d, "statMonth")}%";
        AttTotalLabel.Text = $"{Int(d, "statTotal")}%";

        var dw = Int(d, "diffWeek"); var dm = Int(d, "diffMonth");
        AttWeekDiffLabel.Text = dw >= 0 ? $"▲ +{dw}%" : $"▼ {dw}%";
        AttWeekDiffLabel.TextColor = Color.FromArgb(dw >= 0 ? "#7EC699" : "#E07A5F");
        AttMonthDiffLabel.Text = dm >= 0 ? $"▲ +{dm}%" : $"▼ {dm}%";
        AttMonthDiffLabel.TextColor = Color.FromArgb(dm >= 0 ? "#7EC699" : "#E07A5F");
        AttendanceCard.IsVisible = true;
    }

    private void FillLeaders(JsonElement d)
    {
        LeadersStack.Children.Clear();
        if (d.ValueKind != JsonValueKind.Array) return;
        int i = 0;
        foreach (var l in d.EnumerateArray())
        {
            if (i >= 5) break;
            var pos = Int(l, "position", i + 1);
            var medal = pos switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => $"  {pos}." };
            var c = pos switch { 1 => "#E8C872", 2 => "#B0B0B0", 3 => "#CD8E54", _ => "#A09B93" };

            var row = new Border
            {
                BackgroundColor = Color.FromArgb("#363636"),
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 10 },
                Stroke = Brush.Transparent, Padding = new Thickness(14, 10)
            };
            var g = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(36), new(GridLength.Star), new(GridLength.Auto) }, ColumnSpacing = 12 };
            g.Add(new Label { Text = medal, FontSize = 16, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }, 0);
            g.Add(new Label { Text = Str(l, "full_name"), FontSize = 14, TextColor = Color.FromArgb("#E8E4DE"), VerticalOptions = LayoutOptions.Center }, 1);
            g.Add(new Label { Text = $"{Int(l, "amount"):N0}", FontSize = 13, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb(c), VerticalOptions = LayoutOptions.Center }, 2);
            row.Content = g;
            LeadersStack.Children.Add(row);
            i++;
        }
        LeadersCard.IsVisible = true;
    }

    private void FillActivity(JsonElement d)
    {
        ActivityStack.Children.Clear();
        if (d.ValueKind != JsonValueKind.Array) return;
        int i = 0;
        foreach (var item in d.EnumerateArray())
        {
            if (i >= 10) break;
            var pn = Str(item, "point_types_name");
            var an = Str(item, "achievements_name");
            var cp = Int(item, "current_point");
            var icon = pn == "DIAMOND" ? "💎" : pn == "COIN" ? "🪙" : "⭐";
            var txt = an switch { "PAIR_VISIT" => "Посещение пары", "HOMEWORK" => "Домашняя работа", "EXAM" => "Экзамен", _ => an?.Replace("_", " ") ?? "" };
            var date = Str(item, "date");
            var dtStr = DateTime.TryParse(date, out var dt) ? dt.ToString("dd.MM HH:mm") : date;

            ActivityStack.Children.Add(new Label { Text = $"{icon} +{cp}  {txt}  ·  {dtStr}", FontSize = 13, TextColor = Color.FromArgb("#A09B93"), Padding = new Thickness(0, 2) });
            i++;
        }
        ActivityCard.IsVisible = true;
    }

    private void FillProgressChart(JsonElement d)
    {
        ProgressChartGrid.Children.Clear();
        ProgressChartGrid.ColumnDefinitions.Clear();
        ProgressChartGrid.RowDefinitions.Clear();
        if (d.ValueKind != JsonValueKind.Array) return;

        JsonElement? models = null;
        foreach (var ch in d.EnumerateArray())
            if (Int(ch, "chart_type") == 3 && ch.TryGetProperty("chart_models", out var m) && m.ValueKind == JsonValueKind.Array && m.GetArrayLength() > 0)
            { models = m; break; }
        if (models == null) return;

        var pts = new List<(string m, int v)>();
        foreach (var md in models.Value.EnumerateArray())
        {
            var dt = Str(md, "date");
            var val = Int(md, "points");
            pts.Add((DateTime.TryParse(dt, out var d2) ? d2.ToString("MMM") : dt, val));
        }
        if (pts.Count == 0) return;
        var max = Math.Max(pts.Max(p => p.v), 1);

        ProgressChartGrid.RowDefinitions.Add(new RowDefinition(new GridLength(80)));
        ProgressChartGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

        for (int i = 0; i < pts.Count; i++)
        {
            ProgressChartGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            var (month, value) = pts[i];
            var h = Math.Max(6, (double)value / max * 70);
            var clr = value >= 4 ? "#7EC699" : value >= 3 ? "#E8C872" : "#E07A5F";

            var bar = new VerticalStackLayout { VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.Center, Spacing = 2 };
            bar.Children.Add(new Label { Text = value.ToString(), FontSize = 10, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb(clr), HorizontalOptions = LayoutOptions.Center });
            bar.Children.Add(new BoxView { Color = Color.FromArgb(clr), CornerRadius = 5, WidthRequest = 28, HeightRequest = h });
            Grid.SetRow(bar, 0); Grid.SetColumn(bar, i);
            ProgressChartGrid.Children.Add(bar);

            var lbl = new Label { Text = month, FontSize = 9, TextColor = Color.FromArgb("#888"), HorizontalOptions = LayoutOptions.Center };
            Grid.SetRow(lbl, 1); Grid.SetColumn(lbl, i);
            ProgressChartGrid.Children.Add(lbl);
        }
        ProgressChartCard.IsVisible = true;
    }

    static string Str(JsonElement e, string p, string def = "") => e.TryGetProperty(p, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? def : def;
    static int Int(JsonElement e, string p, int def = 0) => e.TryGetProperty(p, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetInt32() : def;
}
