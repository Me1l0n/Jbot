using System.Text.Json;
using Jbot.Services;

namespace Jbot.Views;

public partial class GradesPage : ContentPage
{
    private readonly ApiService _api;
    private List<GradeItem> _allVisitGrades = new();
    private List<GradeItem> _examGrades = new();
    private bool _loaded;
    private string _currentTab = "all";

    private static readonly (string id, string label, string color)[] Tabs =
    [
        ("all", "Все", "#F5F5F0"),
        ("exams", "Экзамены", "#D97757"),
        ("home_work_mark", "Домашние", "#6BA3D6"),
        ("lab_work_mark", "Лабораторные", "#B07ED6"),
        ("class_work_mark", "Классная", "#E8C872"),
        ("control_work_mark", "Контрольные", "#E07A5F"),
        ("practical_work_mark", "Практические", "#5EBFA0"),
        ("final_work_mark", "Итоговая", "#D4A843"),
    ];

    record GradeItem(string Spec, string Teacher, string Date, int Mark, string? Theme, string MarkType);

    public GradesPage(ApiService api)
    {
        InitializeComponent();
        _api = api;
        BuildTabs();
    }

    private void BuildTabs()
    {
        foreach (var (id, label, color) in Tabs)
        {
            var isActive = id == "all";
            var btn = new Button
            {
                Text = label,
                ClassId = id,
                BackgroundColor = isActive ? Color.FromArgb("#D97757") : Color.FromArgb("#363636"),
                TextColor = isActive ? Colors.White : Color.FromArgb(color),
                FontSize = 12,
                FontFamily = "OpenSansSemibold",
                CornerRadius = 14,
                Padding = new Thickness(14, 6),
                HeightRequest = 34
            };
            btn.Clicked += OnTabClicked;
            TabsContainer.Children.Add(btn);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            if (!_api.IsAuthorized) await _api.LoginAsync();
            if (!_loaded) await LoadAllDataAsync();
            ShowTab(_currentTab);
        }
        catch (Exception ex) { ShowError(ex.Message); }
    }

    private async Task LoadAllDataAsync()
    {
        Loader.IsRunning = true;
        Loader.IsVisible = true;
        try
        {
            var visitsTask = _api.GetStudentVisitsAsync();
            var examsTask = _api.GetStudentExamsAsync();
            await Task.WhenAll(visitsTask, examsTask);

            var visits = visitsTask.Result;
            var exams = examsTask.Result;

            await Task.Run(() =>
            {
                _examGrades.Clear();
                if (exams.ValueKind == JsonValueKind.Array)
                    foreach (var e in exams.EnumerateArray())
                    {
                        var m = Int(e, "mark");
                        if (m >= 1 && m <= 5)
                            _examGrades.Add(new GradeItem(Str(e, "spec"), Str(e, "teacher"), Str(e, "date"), m, null, "exams"));
                    }

                _allVisitGrades.Clear();
                if (visits.ValueKind == JsonValueKind.Array)
                {
                    foreach (var v in visits.EnumerateArray())
                    {
                        var spec = Str(v, "spec_name");
                        var teacher = Str(v, "teacher_name");
                        var date = Str(v, "date_visit");
                        var theme = Str(v, "lesson_theme");

                        foreach (var prop in new[] { "home_work_mark", "lab_work_mark", "class_work_mark", "control_work_mark", "practical_work_mark", "final_work_mark" })
                        {
                            if (v.TryGetProperty(prop, out var mv) && mv.ValueKind == JsonValueKind.Number)
                            {
                                var mark = mv.GetInt32();
                                if (mark >= 1 && mark <= 5)
                                    _allVisitGrades.Add(new GradeItem(spec, teacher, date, mark, theme, prop));
                            }
                        }
                    }
                }
            });

            _loaded = true;
        }
        catch (Exception ex) { ShowError(ex.Message); }
        finally { Loader.IsRunning = false; Loader.IsVisible = false; }
    }

    private async void OnTabClicked(object? sender, EventArgs e)
    {
        if (sender is not Button btn) return;
        _currentTab = btn.ClassId;

        foreach (var child in TabsContainer.Children)
        {
            if (child is Button b)
            {
                var tab = Array.Find(Tabs, t => t.id == b.ClassId);
                b.BackgroundColor = b == btn ? Color.FromArgb(tab.color == "#F5F5F0" ? "#D97757" : tab.color) : Color.FromArgb("#363636");
                b.TextColor = b == btn ? Colors.White : Color.FromArgb(tab.color);
            }
        }

        if (!_loaded) await LoadAllDataAsync();
        ShowTab(btn.ClassId);
    }

    private void ShowTab(string tabId)
    {
        try
        {
            ContentStack.Children.Clear();
            EmptyLabel.IsVisible = false;

            if (tabId == "all")
            {
                ShowAllOverview();
                return;
            }

            List<GradeItem> items = tabId == "exams" ? _examGrades : _allVisitGrades.Where(g => g.MarkType == tabId).ToList();
            var marks = items.Select(i => i.Mark).ToList();

            ShowStats(marks, tabId);
            BuildChart(marks);

            if (items.Count == 0)
            {
                EmptyLabel.Text = "Нет оценок в этой категории";
                EmptyLabel.IsVisible = true;
                return;
            }

            var shown = items.Take(50).ToList();
            foreach (var item in shown)
                ContentStack.Children.Add(CreateCard(item));

            if (items.Count > 50)
                ContentStack.Children.Add(new Label { Text = $"Показано 50 из {items.Count}", FontSize = 12, TextColor = Color.FromArgb("#888"), HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 8) });
        }
        catch (Exception ex) { ShowError(ex.Message); }
    }

    private void ShowAllOverview()
    {
        // Combine all grades
        var all = new List<GradeItem>();
        all.AddRange(_examGrades);
        all.AddRange(_allVisitGrades);

        var allMarks = all.Select(i => i.Mark).ToList();

        // Global stats
        ShowStats(allMarks, "all");
        BuildChart(allMarks);

        // Per-category breakdown
        var categories = new (string id, string label, string color)[]
        {
            ("exams", "Экзамены", "#D97757"),
            ("home_work_mark", "Домашние задания", "#6BA3D6"),
            ("lab_work_mark", "Лабораторные работы", "#B07ED6"),
            ("class_work_mark", "Классная работа", "#E8C872"),
            ("control_work_mark", "Контрольные работы", "#E07A5F"),
            ("practical_work_mark", "Практические работы", "#5EBFA0"),
            ("final_work_mark", "Итоговая контрольная", "#D4A843"),
        };

        var breakdownCard = new Border
        {
            BackgroundColor = Color.FromArgb("#2d2d2d"),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 14 },
            Stroke = new SolidColorBrush(Color.FromArgb("#3a3a3a")),
            StrokeThickness = 1,
            Padding = new Thickness(20)
        };
        var breakdownStack = new VerticalStackLayout { Spacing = 12 };
        breakdownStack.Children.Add(new Label { Text = "Средний балл по категориям", FontSize = 15, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb("#F5F5F0") });

        foreach (var (id, label, color) in categories)
        {
            var catItems = id == "exams" ? _examGrades : _allVisitGrades.Where(g => g.MarkType == id).ToList();
            if (catItems.Count == 0) continue;

            var catMarks = catItems.Select(i => i.Mark).ToList();
            var avg = catMarks.Average();
            var avgColor = avg >= 4 ? "#7EC699" : avg >= 3 ? "#E8C872" : "#E07A5F";

            var row = new Border
            {
                BackgroundColor = Color.FromArgb("#363636"),
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 10 },
                Stroke = Brush.Transparent,
                Padding = new Thickness(14, 10)
            };
            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection { new(GridLength.Star), new(60), new(40), new(44) },
                ColumnSpacing = 10
            };
            grid.Add(new Label { Text = label, FontSize = 13, TextColor = Color.FromArgb(color), VerticalOptions = LayoutOptions.Center }, 0);
            grid.Add(new Label { Text = $"{catMarks.Count} оц.", FontSize = 11, TextColor = Color.FromArgb("#888"), VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.End }, 1);

            // Progress bar showing average
            var barGrid = new Grid { VerticalOptions = LayoutOptions.Center };
            barGrid.Children.Add(new BoxView { Color = Color.FromArgb("#2d2d2d"), CornerRadius = 3, HeightRequest = 6 });
            barGrid.Children.Add(new BoxView { Color = Color.FromArgb(avgColor), CornerRadius = 3, HeightRequest = 6, HorizontalOptions = LayoutOptions.Start, WidthRequest = avg / 5.0 * 40 });
            grid.Add(barGrid, 2);

            // Average circle
            var avgBorder = new Border
            {
                BackgroundColor = Color.FromArgb(avgColor).WithAlpha(0.15f),
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 16 },
                Stroke = new SolidColorBrush(Color.FromArgb(avgColor)),
                StrokeThickness = 1.5,
                WidthRequest = 40, HeightRequest = 32,
                VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center,
                Content = new Label { Text = avg.ToString("F1"), FontSize = 13, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb(avgColor), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }
            };
            grid.Add(avgBorder, 3);

            row.Content = grid;
            breakdownStack.Children.Add(row);
        }

        breakdownCard.Content = breakdownStack;
        ContentStack.Children.Add(breakdownCard);
    }

    private static View CreateCard(GradeItem item)
    {
        var mc = item.Mark switch { 5 => "#7EC699", 4 => "#A3C67E", 3 => "#E8C872", 2 => "#E07A5F", _ => "#C75050" };

        var card = new Border
        {
            BackgroundColor = Color.FromArgb("#2d2d2d"),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 },
            Stroke = new SolidColorBrush(Color.FromArgb("#3a3a3a")),
            StrokeThickness = 1,
            Padding = new Thickness(16, 12)
        };

        var grid = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(GridLength.Star), new(48) }, ColumnSpacing = 12 };
        var info = new VerticalStackLayout { Spacing = 3, VerticalOptions = LayoutOptions.Center };
        info.Children.Add(new Label { Text = item.Spec, FontSize = 13, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb("#F5F5F0"), LineBreakMode = LineBreakMode.TailTruncation });
        if (!string.IsNullOrWhiteSpace(item.Theme))
            info.Children.Add(new Label { Text = item.Theme, FontSize = 11, TextColor = Color.FromArgb("#A09B93"), LineBreakMode = LineBreakMode.TailTruncation });

        var meta = new HorizontalStackLayout { Spacing = 12 };
        if (!string.IsNullOrWhiteSpace(item.Teacher)) meta.Children.Add(new Label { Text = item.Teacher, FontSize = 11, TextColor = Color.FromArgb("#777") });
        if (!string.IsNullOrWhiteSpace(item.Date)) meta.Children.Add(new Label { Text = FormatDate(item.Date), FontSize = 11, TextColor = Color.FromArgb("#666") });
        info.Children.Add(meta);
        grid.Add(info, 0);

        var markCircle = new Border
        {
            BackgroundColor = Color.FromArgb(mc).WithAlpha(0.15f),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 22 },
            Stroke = new SolidColorBrush(Color.FromArgb(mc)),
            StrokeThickness = 2,
            WidthRequest = 44, HeightRequest = 44,
            VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center,
            Content = new Label { Text = item.Mark.ToString(), FontSize = 20, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb(mc), HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }
        };
        grid.Add(markCircle, 1);

        card.Content = grid;
        return card;
    }

    private void ShowStats(List<int> marks, string tabId)
    {
        if (marks.Count == 0) { StatsCard.IsVisible = false; return; }

        StatsCount.Text = marks.Count.ToString();
        var avg = marks.Average();
        StatsAvg.Text = avg.ToString("F1");
        StatsAvg.TextColor = Color.FromArgb(avg >= 4 ? "#7EC699" : avg >= 3 ? "#E8C872" : "#E07A5F");
        StatsMax.Text = marks.Max().ToString();

        // Bottom-right: category average label
        var tabLabel = Tabs.FirstOrDefault(t => t.id == tabId).label ?? "Все";
        StatsCategoryLabel.Text = $"Ср. {tabLabel}";
        StatsCategoryAvg.Text = avg.ToString("F1");
        StatsCategoryAvg.TextColor = Color.FromArgb(avg >= 4 ? "#7EC699" : avg >= 3 ? "#E8C872" : "#E07A5F");

        StatsCard.IsVisible = true;
    }

    private void BuildChart(List<int> marks)
    {
        ChartStack.Children.Clear();
        if (marks.Count == 0) { ChartCard.IsVisible = false; return; }

        var dist = new int[6];
        foreach (var m in marks) if (m >= 1 && m <= 5) dist[m]++;
        int maxD = 0;
        for (int i = 1; i <= 5; i++) maxD = Math.Max(maxD, dist[i]);
        if (maxD == 0) { ChartCard.IsVisible = false; return; }

        var colors = new[] { "", "#C75050", "#E07A5F", "#E8C872", "#A3C67E", "#7EC699" };
        for (int i = 5; i >= 1; i--)
        {
            var row = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(24), new(GridLength.Star), new(30) }, ColumnSpacing = 10, HeightRequest = 22 };
            row.Add(new Label { Text = i.ToString(), FontSize = 12, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb(colors[i]), VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }, 0);
            var barBg = new Grid { VerticalOptions = LayoutOptions.Center };
            barBg.Children.Add(new BoxView { Color = Color.FromArgb("#363636"), CornerRadius = 4, HeightRequest = 14 });
            if (dist[i] > 0)
                barBg.Children.Add(new BoxView { Color = Color.FromArgb(colors[i]), CornerRadius = 4, HeightRequest = 14, HorizontalOptions = LayoutOptions.Start, Opacity = 0.7, WidthRequest = Math.Max(10, (double)dist[i] / maxD * 220) });
            row.Add(barBg, 1);
            row.Add(new Label { Text = dist[i].ToString(), FontSize = 11, TextColor = Color.FromArgb(dist[i] > 0 ? colors[i] : "#555"), VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center }, 2);
            ChartStack.Children.Add(row);
        }
        ChartCard.IsVisible = true;
    }

    private void ShowError(string msg)
    {
        Loader.IsRunning = false; Loader.IsVisible = false;
        EmptyLabel.Text = $"Ошибка: {msg}"; EmptyLabel.TextColor = Color.FromArgb("#E07A5F"); EmptyLabel.IsVisible = true;
    }

    static string Str(JsonElement e, string p) => e.TryGetProperty(p, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";
    static int Int(JsonElement e, string p) => e.TryGetProperty(p, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetInt32() : 0;
    static string FormatDate(string? s) => DateTime.TryParse(s, out var d) ? d.ToString("dd.MM.yyyy") : s ?? "";
}
