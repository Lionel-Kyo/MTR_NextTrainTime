using MTR_NextTrain.Api;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace MTR_NextTrain;

public static class MauiProgram
{
	private static async void SetLightRailStationIds()
	{
        using Stream stream = await FileSystem.Current.OpenAppPackageFileAsync("LightRailStationIds.json");
        using var reader = new StreamReader(stream);
        var contents = reader.ReadToEnd();
        Api.LightRail.StationIds = JsonSerializer.Deserialize<Dictionary<int, StationName>>(contents);
    }

    private static async void SetMetroInfo()
    {
        using Stream stream = await FileSystem.Current.OpenAppPackageFileAsync("MetroInfo.json");
        using var reader = new StreamReader(stream);
        var contents = reader.ReadToEnd();
        Api.Metro.MetroInfo = JsonSerializer.Deserialize<MetroInfomation>(contents);
    }

    public static MauiApp CreateMauiApp()
	{
		Task.Run(SetLightRailStationIds).Wait();
		Task.Run(SetMetroInfo).Wait();
        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
}
