using System.Text.Json;
using Jbot.Services;

namespace Jbot.Views;

public partial class SchedulePage : ContentPage
{
    private readonly ApiService _api;

    public SchedulePage(ApiService api) { InitializeComponent(); _api = api; }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!_api.IsAuthorized) await _api.LoginAsync();
        await LoadTodayAsync();
    }

    private async void OnTodayClicked(object? s, EventArgs e)
    {
        BtnToday.BackgroundColor = Color.FromArgb("#D97757"); BtnToday.TextColor = Colors.White;
        BtnMonth.BackgroundColor = Color.FromArgb("#363636"); BtnMonth.TextColor = Color.FromArgb("#A09B93");
        await LoadTodayAsync();
    }

    private async void OnMonthClicked(object? s, EventArgs e)
    {
        BtnMonth.BackgroundColor = Color.FromArgb("#D97757"); BtnMonth.TextColor = Colors.White;
        BtnToday.BackgroundColor = Color.FromArgb("#363636"); BtnToday.TextColor = Color.FromArgb("#A09B93");
        await LoadMonthAsync();
    }

    private async Task LoadTodayAsync()
    {
        ShowLoading();
        try
        {
            var data = await _api.GetScheduleTodayAsync();
            DateHeader.Text = $"Расписание на {DateTime.Now:dd MMMM yyyy}";
            DateHeader.IsVisible = true;
            RenderLessons(data, false);
        }
        catch (Exception ex) { ShowErr(ex.Message); }
        finally { HideLoading(); }
    }

    private async Task LoadMonthAsync()
    {
        ShowLoading();
        try
        {
            var data = await _api.GetScheduleMonthAsync();
            DateHeader.Text = $"Расписание — {DateTime.Now:MMMM yyyy}";
            DateHeader.IsVisible = true;
            RenderLessons(data, true);
        }
        catch (Exception ex) { ShowErr(ex.Message); }
        finally { HideLoading(); }
    }

    private void RenderLessons(JsonElement data, bool groupByDate)
    {
        if (data.ValueKind != JsonValueKind.Array || data.GetArrayLength() == 0)
        { EmptyLabel.IsVisible = true; return; }

        string? lastDate = null;
        foreach (var lesson in data.EnumerateArray())
        {
            var date = Str(lesson, "date");
            if (groupByDate && date != lastDate)
            {
                lastDate = date;
                ScheduleStack.Children.Add(new Label
                {
                    Text = DateTime.TryParse(date, out var dt)
                        ? $"{dt:dd.MM} — {char.ToUpper(dt.ToString("dddd", new System.Globalization.CultureInfo("ru-RU"))[0])}{dt.ToString("dddd", new System.Globalization.CultureInfo("ru-RU"))[1..]}"
                        : date,
                    FontSize = 14, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb("#D97757"),
                    Margin = new Thickness(0, groupByDate ? 10 : 0, 0, 0)
                });
            }

            var num = Int(lesson, "lesson"); var start = Str(lesson, "started_at"); var end = Str(lesson, "finished_at");
            var subj = Str(lesson, "subject_name"); var teach = Str(lesson, "teacher_name"); var room = Str(lesson, "room_name");

            var card = new Border
            {
                BackgroundColor = Color.FromArgb("#2d2d2d"),
                StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 },
                Stroke = new SolidColorBrush(Color.FromArgb("#3a3a3a")), StrokeThickness = 1,
                Padding = new Thickness(16, 12)
            };

            var grid = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new(52), new(GridLength.Star), new(GridLength.Auto) }, ColumnSpacing = 14 };

            var timeCol = new VerticalStackLayout { VerticalOptions = LayoutOptions.Center };
            timeCol.Children.Add(new Label { Text = $"#{num}", FontSize = 11, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb("#D97757"), HorizontalOptions = LayoutOptions.Center });
            timeCol.Children.Add(new Label { Text = start, FontSize = 15, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb("#F5F5F0"), HorizontalOptions = LayoutOptions.Center });
            timeCol.Children.Add(new Label { Text = end, FontSize = 11, TextColor = Color.FromArgb("#888"), HorizontalOptions = LayoutOptions.Center });
            grid.Add(timeCol, 0);

            var infoCol = new VerticalStackLayout { VerticalOptions = LayoutOptions.Center, Spacing = 3 };
            infoCol.Children.Add(new Label { Text = subj, FontSize = 14, FontFamily = "OpenSansSemibold", TextColor = Color.FromArgb("#F5F5F0"), LineBreakMode = LineBreakMode.TailTruncation });
            infoCol.Children.Add(new Label { Text = teach, FontSize = 12, TextColor = Color.FromArgb("#A09B93") });
            grid.Add(infoCol, 1);

            var roomBadge = new Border { BackgroundColor = Color.FromArgb("#363636"), StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 8 }, Stroke = Brush.Transparent, Padding = new Thickness(10, 6), VerticalOptions = LayoutOptions.Center };
            roomBadge.Content = new Label { Text = room, FontSize = 11, TextColor = Color.FromArgb("#D97757") };
            grid.Add(roomBadge, 2);

            card.Content = grid;
            ScheduleStack.Children.Add(card);
        }
    }

    private void ShowLoading() { Loader.IsRunning = true; Loader.IsVisible = true; ScheduleStack.Children.Clear(); EmptyLabel.IsVisible = false; }
    private void HideLoading() { Loader.IsRunning = false; Loader.IsVisible = false; }
    private void ShowErr(string m) { EmptyLabel.Text = $"Ошибка: {m}"; EmptyLabel.TextColor = Color.FromArgb("#E07A5F"); EmptyLabel.IsVisible = true; }
    static string Str(JsonElement e, string p) => e.TryGetProperty(p, out var v) && v.ValueKind == JsonValueKind.String ? v.GetString() ?? "" : "";
    static int Int(JsonElement e, string p) => e.TryGetProperty(p, out var v) && v.ValueKind == JsonValueKind.Number ? v.GetInt32() : 0;
}
