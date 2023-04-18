using Microsoft.Maui.Controls;
using MTR_NextTrain.Api;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace MTR_NextTrain;

public partial class MtrPage : ContentPage
{
	public MtrPage()
	{
		InitializeComponent();
        LinePicker.SetAppThemeColor(Picker.BackgroundColorProperty, Colors.White, Colors.Black);
        LinePicker.SetAppThemeColor(Picker.TextColorProperty, Colors.Black, Colors.White);
        LinePicker.SetAppThemeColor(Picker.TitleColorProperty, Colors.Black, Colors.White);
        StationPicker.SetAppThemeColor(Picker.BackgroundColorProperty, Colors.White, Colors.Black);
        StationPicker.SetAppThemeColor(Picker.TextColorProperty, Colors.Black, Colors.White);
        StationPicker.SetAppThemeColor(Picker.TitleColorProperty, Colors.Black, Colors.White);
        LinePicker.ItemsSource = Api.Metro.MetroInfo.Lines.ToList();
        LinePicker.ItemDisplayBinding = new Binding("Value");
        LinePicker.SelectedIndexChanged += LinePicker_SelectedIndexChanged;

        StationPicker.SelectedIndexChanged += StationPicker_SelectedIndexChanged;
        LinePicker.SelectedIndex = 0;
    }

    private void LinePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var itemsSource = (List<KeyValuePair<string, string>>)LinePicker.ItemsSource;
        // Because MAUI have bug that can't update the changed Items
        List<string> newValue = new List<string>();
        foreach (var station in Api.Metro.MetroInfo.LineStations[itemsSource[LinePicker.SelectedIndex].Key]) 
        {
            newValue.Add(Api.Metro.MetroInfo.Stations[station]);
        }
        StationPicker.ItemsSource = newValue;
        StationPicker.ItemsSource = StationPicker.GetItemsAsArray();
        StationPicker.SelectedIndex = 0;
    }

    private void Update()
    {
        if (LinePicker.SelectedIndex < 0 || StationPicker.SelectedIndex < 0)
            return;
        var lineItemsSource = (List<KeyValuePair<string, string>>)LinePicker.ItemsSource;

        string currentLine = lineItemsSource[LinePicker.SelectedIndex].Key;
        string currentStation = Api.Metro.MetroInfo.Stations.FirstOrDefault(kv => kv.Value == StationPicker.Items[StationPicker.SelectedIndex]).Key;

        MetroResult stationInfo = null;
        try
        {
            stationInfo = Task.Run(async () => await Api.Metro.Get(currentLine, currentStation, ApiLanguage.English)).Result;
        }
        catch
        {
            Task.Run(async () => await DisplayAlert("Error", "Unable to get train data.", "OK"));
            return;
        }
        PlatformTable.Root.Clear();
        if (stationInfo.data == null)
            return;

        foreach (var data in stationInfo.data)
        {
            Dictionary<string, List<MetroUpDown>> platformMetros = new Dictionary<string, List<MetroUpDown>>();
            if (data.Value.UP != null)
            {
                foreach (var item in data.Value.UP)
                {
                    if (!platformMetros.ContainsKey(item.plat))
                        platformMetros[item.plat] = new List<MetroUpDown>();
                    platformMetros[item.plat].Add(item);
                }
            }
            if (data.Value.DOWN != null)
            {
                foreach (var item in data.Value.DOWN)
                {
                    if (!platformMetros.ContainsKey(item.plat))
                        platformMetros[item.plat] = new List<MetroUpDown>();
                    platformMetros[item.plat].Add(item);
                }
            }
            foreach (var platform in platformMetros)
            {
                TableSection platformSelection = new TableSection();
                platformSelection.Title = $"Platform {platform.Key}";
                platformSelection.SetAppThemeColor(TableSection.TextColorProperty, Colors.DarkViolet, Colors.PaleVioletRed);
                List<MetroUpDown> routes = platform.Value;
                foreach (MetroUpDown route in routes)
                {
                    string specialRoute = "";
                    if (!string.IsNullOrEmpty(route.route) && route.route.Equals("RAC"))
                        specialRoute = "Racecourse";

                    TextCell routeCell = new TextCell();
                    routeCell.SetAppThemeColor(TextCell.TextColorProperty, Colors.DarkBlue, Colors.Azure);
                    routeCell.SetAppThemeColor(TextCell.DetailColorProperty, Colors.Black, Colors.White);
                    routeCell.Text = $"{Api.Metro.MetroInfo.Stations[route.dest]} {specialRoute}";
                    double remainMins = (route.time - data.Value.curr_time).TotalMinutes;
                    string remain = remainMins.ToString("0.##") + " min" + (remainMins > 1 ? "s" : "");
                    routeCell.Detail = $"Time: {route.time.ToString("yyyy-MM-dd HH:mm:ss")} Remain: {remain}";
                    platformSelection.Add(routeCell);
                }
                PlatformTable.Root.Add(platformSelection);
            }
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

