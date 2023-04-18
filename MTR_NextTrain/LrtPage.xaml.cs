using Microsoft.Maui.Controls;
using MTR_NextTrain.Api;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MTR_NextTrain;

public partial class LrtPage : ContentPage
{
	public LrtPage()
	{
		InitializeComponent();
        StationPicker.SetAppThemeColor(Picker.BackgroundColorProperty, Colors.White, Colors.Black);
        StationPicker.SetAppThemeColor(Picker.TextColorProperty, Colors.Black, Colors.White);
        StationPicker.SetAppThemeColor(Picker.TitleColorProperty, Colors.Black, Colors.White);
        StationPicker.ItemsSource = new List<KeyValuePair<int, StationName>>();
        foreach (KeyValuePair<int, StationName> stationPair in Api.LightRail.StationIds)
        {
            StationPicker.ItemsSource.Add(stationPair);
        }
        StationPicker.ItemDisplayBinding = new Binding("Value.Cn");
        StationPicker.SelectedIndexChanged += StationPicker_SelectedIndexChanged;
        StationPicker.SelectedIndex = 0;
    }

    private void Update()
    {
        if (StationPicker.SelectedIndex < 0)
            return;

        KeyValuePair<int, StationName> stationPair = ((List<KeyValuePair<int, StationName>>)StationPicker.ItemsSource)[StationPicker.SelectedIndex];
        LightRailResult stationInfo = null;
        try
        {
            stationInfo = Task.Run(async () => await Api.LightRail.Get(stationPair.Key)).Result;
        }
        catch
        {
            Task.Run(async () => await DisplayAlert("Error", "Unable to get train data.", "OK"));
            return;
        }
        List<LightRailPlatform> platforms = stationInfo.platform_list;
        PlatformTable.Root.Clear();
        if (platforms == null)
            return;
        foreach (LightRailPlatform platform in platforms)
        {
            TableSection platformSelection = new TableSection();
            platformSelection.Title = $"Platform {platform.platform_id}";
            platformSelection.SetAppThemeColor(TableSection.TextColorProperty, Colors.DarkViolet, Colors.PaleVioletRed);
            List<LightRailRoute> routes = platform.route_list;
            if (routes == null)
                continue;
            foreach (LightRailRoute route in routes)
            {
                TextCell routeCell = new TextCell();
                routeCell.SetAppThemeColor(TextCell.TextColorProperty, Colors.DarkBlue, Colors.Azure);
                routeCell.SetAppThemeColor(TextCell.DetailColorProperty, Colors.Black, Colors.White);
                string carsText = route.train_length.ToString() + " car" + (route.train_length > 1 ? "s" : "");
                routeCell.Text = $"{route.route_no} {route.dest_ch} ({carsText})";
                string timeEn = route.time_en.Trim().ToLower();
                double departMins = 0;
                if (timeEn.Equals("departing") || timeEn.Equals("arriving") || timeEn.Equals("-"))
                {

                }
                else
                {
                    string strMins = timeEn.Split(' ')[0];
                    if (int.TryParse(strMins, out int mins))
                    {
                        departMins = mins;
                    }
                }
                DateTime time = stationInfo.system_time.AddMinutes(departMins).AddSeconds(-30);
                double remainMins = (time - stationInfo.system_time).TotalMinutes;
                string remain = remainMins.ToString("0.##") + " min" + (remainMins > 1 ? "s" : "");
                routeCell.Detail = $"Time: {time.ToString("yyyy-MM-dd HH:mm:ss")} Remain: {remain}";
                platformSelection.Add(routeCell);
            }
            PlatformTable.Root.Add(platformSelection);
        }
    }

    private void StationPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        Update();
    }

    private void OnRefreshBtnClicked(object sender, EventArgs e)
    {
        Update();
    }

}

