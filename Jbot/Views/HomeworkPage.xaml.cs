using System.Text.Json;
using Jbot.Services;

namespace Jbot.Views;

public partial class HomeworkPage : ContentPage
{
    private readonly ApiService _api;
    private Dictionary<int, int> _counters = new();

    // Финальная привязка ID от API к нужным цветам
    private static readonly Dictionary<string, string> FilterColors = new()
    {
        ["all"] = "#F5F5F0", 
        ["4"] = "#D97757", // Всего
        ["1"] = "#7EC699", // Проверено 
        ["3"] = "#E8C872", // Текущие
        ["2"] = "#6BA3D6", // На проверке
        ["0"] = "#E07A5F", // Просрочено
        ["5"] = "#888"     // Удалено
    };

    public HomeworkPage(ApiService api) { InitializeComponent(); _api = api; }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!_api.IsAuthorized) await _api.LoginAsync();
        await LoadCountersAsync();
        await LoadListAsync("all");
    }

    private async void OnFilterClicked(object? sender, EventArgs e)
    {
        if (sender is not Button btn) return;
        if (btn.Parent is HorizontalStackLayout container)
        {
            foreach (var child in container.Children)
            {
                if (child is Button b)
                {
                    b.BackgroundColor = b == btn ? Color.FromArgb("#D97757") : Color.FromArgb("#363636");
                    b.TextColor = b == btn ? Colors.White : Color.FromArgb(FilterColors.GetValueOrDefault(b.ClassId, "#A09B93"));
                }
            }
        }
        await LoadListAsync(btn.ClassId);
    }

    private async Task LoadCountersAsync()
    {
        try
        {
            var data = await _api.GetHomeworkCountAsync();

            JsonElement countersArray;
            if (data.ValueKind == JsonValueKind.Array)
                countersArray = data;
            else if (data.TryGetProperty("value", out var val) && val.ValueKind == JsonValueKind.Array)
                countersArray = val;
            else
                return;

            _counters.Clear();
            foreach (var c in countersArray.EnumerateArray())
            {
                var type = Int(c, "counter_type", -1);
                if (type == -1) type = Int(c, "type", -1);
                
                _counters[type] = Int(c, "counter");
            }

            // Финальный маппинг
            CountTotal.Text = _counters.GetValueOrDefault(4).ToString();   // ID 4 = Всего
            CountChecked.Text = _counters.GetValueOrDefault(1).ToString(); // ID 1 = Проверено
            CountCurrent.Text = _counters.GetValueOrDefault(3).ToString(); // ID 3 = Текущие
            CountReview.Text = _counters.GetValueOrDefault(2).ToString();  // ID 2 = На проверке
            CountOverdue.Text = _counters.GetValueOrDefault(0).ToString(); // ID 0 = Просрочено
            CountDeleted.Text = _counters.GetValueOrDefault(5).ToString(); // ID 5 = Удалено
            
            BuildChart();
        }
        catch { }
    }

    private void BuildChart()
    {
        ChartStack.Children.Clear();
        
        var items = new (string l, int c, string clr)[]
        {
            ("Текущие", _counters.GetValueOrDefault(3), "#E8C872"), 
            ("На проверке", _counters.GetValueOrDefault(2), "#6BA3D6"),
            ("Просрочено", _counters.GetValueOrDefault(0), "#E07A5F"),
            ("Проверено", _counters.GetValueOrDefault(1), "#7EC699"),
            ("Удалено", _counters.GetValueOrDefault(5), "#888"),
        };
        
        var max = items.Max(i => i.c);
        if (max == 0) return;
        
        foreach (var (label, count, color) in items)
        {
            if (count == 0) continue;
            var row = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(85), new(GridLength.Star), new(36) }, ColumnSpacing = 8, HeightRequest = 18 };
            row.Add(new Label { Text = label, FontSize = 10, TextColor = Color.FromArgb("#888"), VerticalOptions = LayoutOptions.Center }, 0);
            var bar = new Grid { VerticalOptions = LayoutOptions.Center };
            bar.Children.Add(new BoxView { Color = Color.FromArgb("#2d2d2d"), CornerRadius = 3, HeightRequest = 10 });
            bar.Children.Add(new BoxView { Color = Color.FromArgb(color), CornerRadius = 3, HeightRequest = 10, Opacity = 0.7, HorizontalOptions = LayoutOptions.Start, WidthRequest = Math.Max(6, (double)count / max * 180) });
            row.Add(bar, 1);
            row.Add(new Label { Text = count.ToString(), FontSize = 10, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb(color), VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.End }, 2);
            ChartStack.Children.Add(row);
        }
    }

    private async Task LoadListAsync(string filter)
    {
        Loader.IsRunning = true; Loader.IsVisible = true;
        HomeworkStack.Children.Clear(); EmptyLabel.IsVisible = false;
        try
        {
            JsonElement data;
            if (filter == "all")
            {
                data = await _api.GetHomeworkListAsync(page: 1);
                FilterLabel.Text = "Все домашние задания"; FilterLabel.IsVisible = true;
            }
            else
            {
                data = await _api.GetHomeworkListAsync(page: 1, status: filter);
                FilterLabel.Text = StatusLabel(int.Parse(filter)); FilterLabel.IsVisible = true;
            }

            if (data.ValueKind != JsonValueKind.Array || data.GetArrayLength() == 0)
            { EmptyLabel.IsVisible = true; return; }

            foreach (var hw in data.EnumerateArray())
                HomeworkStack.Children.Add(CreateCard(hw));
        }
        catch (Exception ex) { EmptyLabel.Text = $"Ошибка: {ex.Message}"; EmptyLabel.TextColor = Color.FromArgb("#E07A5F"); EmptyLabel.IsVisible = true; }
        finally { Loader.IsRunning = false; Loader.IsVisible = false; }
    }

    private static View CreateCard(JsonElement hw)
    {
        var theme = Str(hw, "theme"); var spec = Str(hw, "name_spec"); var teach = Str(hw, "fio_teach");
        var created = Str(hw, "creation_time"); var deadline = Str(hw, "completion_time");
        
        var status = Int(hw, "status", -1);
        if (status == -1) status = Int(hw, "type", -1);
        if (status == -1) status = Int(hw, "status_id", -1);

        int? mark = null;
        if (hw.TryGetProperty("homework_stud", out var stud) && stud.ValueKind == JsonValueKind.Object)
        {
            if (stud.TryGetProperty("mark", out var m) && m.ValueKind == JsonValueKind.Number)
                mark = m.GetInt32();
        }
        if (!mark.HasValue && hw.TryGetProperty("mark", out var dm) && dm.ValueKind == JsonValueKind.Number)
            mark = dm.GetInt32();

        var (statusText, statusColor) = StatusInfo(status);

        var card = new Border
        {
            BackgroundColor = Color.FromArgb("#2d2d2d"),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 },
            Stroke = new SolidColorBrush(Color.FromArgb("#3a3a3a")), StrokeThickness = 1,
            Padding = new Thickness(16, 14)
        };

        var stack = new VerticalStackLayout { Spacing = 6 };

        var header = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(GridLength.Star), new(GridLength.Auto) }, ColumnSpacing = 8 };
        header.Add(new Label { Text = spec, FontSize = 14, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb("#F5F5F0"), LineBreakMode = LineBreakMode.TailTruncation }, 0);
        
        var badge = new Border
        {
            BackgroundColor = Color.FromArgb(statusColor).WithAlpha(0.15f),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 8 },
            Stroke = Brush.Transparent, Padding = new Thickness(10, 4),
            Content = new Label { Text = statusText, FontSize = 11, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb(statusColor) }
        };
        header.Add(badge, 1);
        stack.Children.Add(header);

        if (!string.IsNullOrWhiteSpace(theme))
            stack.Children.Add(new Label { Text = theme, FontSize = 13, TextColor = Color.FromArgb("#C8C4BC"), LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 2 });

        stack.Children.Add(new Label { Text = teach, FontSize = 12, TextColor = Color.FromArgb("#A09B93") });

        var dates = new HorizontalStackLayout { Spacing = 16 };
        dates.Children.Add(new Label { Text = $"Создано: {FmtDate(created)}", FontSize = 11, TextColor = Color.FromArgb("#777") });
        dates.Children.Add(new Label { Text = $"Срок: {FmtDate(deadline)}", FontSize = 11, TextColor = Color.FromArgb("#777") });
        stack.Children.Add(dates);

        if (mark.HasValue)
        {
            var mc = mark.Value >= 4 ? "#7EC699" : mark.Value == 3 ? "#E8C872" : "#E07A5F";
            stack.Children.Add(new Label { Text = $"Оценка: {mark.Value}", FontSize = 13, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb(mc) });
        }

        card.Content = stack;
        return card;
    }

    // Присваиваем текстам в списке правильные ID
    static (string, string) StatusInfo(int s) => s switch
    {
        4 => ("Всего", "#D97757"), 
        1 => ("Проверено", "#7EC699"),
        3 => ("Текущее", "#E8C872"), 
        2 => ("На проверке", "#6BA3D6"),
        0 => ("Просрочено", "#E07A5F"),
        5 => ("Удалено", "#888"),
        _ => ($"Статус #{s}", "#888")
    };
    
    static string StatusLabel(int s) => StatusInfo(s).Item1;
    static string Str(JsonElement e, string p) => e.TryGetProperty(p, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";
    static int Int(JsonElement e, string p, int d = 0) => e.TryGetProperty(p, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetInt32() : d;
    static string FmtDate(string? s) => DateTime.TryParse(s, out var d) ? d.ToString("dd.MM.yyyy") : s ?? "";
}