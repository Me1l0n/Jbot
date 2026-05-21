using System.Diagnostics;
using Jbot.Services;

namespace Jbot.Views;

public partial class ExplorerPage : ContentPage
{
    private readonly ApiService _api;

    public ExplorerPage(ApiService api)
    {
        InitializeComponent();
        _api = api;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!_api.IsAuthorized) await _api.LoginAsync();
    }

    private async void OnCustomGetClicked(object? sender, EventArgs e)
    {
        var endpoint = CustomEndpoint.Text?.Trim();
        if (string.IsNullOrEmpty(endpoint)) return;
        await SendRequest(endpoint);
    }

    private async void OnQuickClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && !string.IsNullOrEmpty(btn.ClassId))
        {
            CustomEndpoint.Text = btn.ClassId;
            await SendRequest(btn.ClassId);
        }
    }

    private async Task SendRequest(string endpoint)
    {
        EndpointLabel.Text = $"⏳ GET /{endpoint}";
        ResponseEditor.Text = "Загрузка...";
        BtnCopy.IsVisible = false;

        try
        {
            var sw = Stopwatch.StartNew();
            var result = await _api.GetRawAsync(endpoint);
            sw.Stop();

            EndpointLabel.Text = $"✅ GET /{endpoint}  •  {sw.ElapsedMilliseconds}ms";
            ResponseEditor.Text = result;
            BtnCopy.IsVisible = true;
        }
        catch (Exception ex)
        {
            EndpointLabel.Text = $"❌ GET /{endpoint}";
            ResponseEditor.Text = $"Ошибка: {ex.Message}\n\n{ex.StackTrace}";
        }
    }

    private async void OnCopyClicked(object? sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ResponseEditor.Text))
        {
            await Clipboard.Default.SetTextAsync(ResponseEditor.Text);
            BtnCopy.Text = "✅ Скопировано!";
            await Task.Delay(1500);
            BtnCopy.Text = "📋 Копировать";
        }
    }
}
