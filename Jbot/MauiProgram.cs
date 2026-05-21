using Microsoft.Extensions.Logging;
using Jbot.Services;
using Jbot.Views;

namespace Jbot;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Services
		builder.Services.AddSingleton<ApiService>();

		// Pages (transient so they refresh each time)
		builder.Services.AddTransient<DashboardPage>();
		builder.Services.AddTransient<SchedulePage>();
		builder.Services.AddTransient<GradesPage>();
		builder.Services.AddTransient<HomeworkPage>();
		builder.Services.AddTransient<ExplorerPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
