using System.Collections.ObjectModel;
using System.Text.Json;
using izzeldeenhamedodev3.Models;
using System.Diagnostics;
using Microsoft.Maui.Hosting;

namespace izzeldeenhamedodev3;



public partial class HavaDurumu : ContentPage
{
    string fileName = Path.Combine(FileSystem.Current.AppDataDirectory, "hdata.json");
    public ObservableCollection<SehirHavaDurumu> Sehirler { get; set; } = new ObservableCollection<SehirHavaDurumu>();

    public HavaDurumu()
    {
        InitializeComponent();
        listCity.ItemsSource = Sehirler;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadData();
    }

    private void LoadData()
    {
        Debug.WriteLine("LoadData called");
        if (File.Exists(fileName))
        {
            Debug.WriteLine("File exists");
            string data = File.ReadAllText(fileName);
            Debug.WriteLine($"Data read from file: {data}");
            var sehirler = JsonSerializer.Deserialize<ObservableCollection<SehirHavaDurumu>>(data);
            if (sehirler != null)
            {
                Sehirler.Clear();
                foreach (var sehir in sehirler)
                {
                    Debug.WriteLine($"Adding city: {sehir.Name}");
                    Sehirler.Add(sehir);
                }
            }
        }
        listCity.ItemsSource = Sehirler; // Ensure the CollectionView is bound
    }

    private async void EkleClicked(object sender, EventArgs e)
    {
        string sehir = await DisplayPromptAsync("Þehir:", "Þehir ismi", "OK", "Cancel");
        if (string.IsNullOrEmpty(sehir)) return;

        sehir = sehir.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
        sehir = sehir.Replace('Ç', 'C').Replace('Ð', 'G').Replace('Ý', 'I').Replace('Ö', 'O').Replace('Ü', 'U').Replace('Þ', 'S');

        Sehirler.Add(new SehirHavaDurumu() { Name = sehir });

        SaveData();
    }

    private void YukleClicked(object sender, EventArgs e)
    {
        refreshView.IsRefreshing = false;
        LoadData();
    }

    private async void SilClicked(object sender, EventArgs e)
    {
        var b = sender as ImageButton;
        if (b != null)
        {
            var t = Sehirler.First(o => o.Name == b.CommandParameter.ToString());
            var yes = await DisplayAlert("Silinsin mi?", "Silmeyi onayla", "OK", "CANCEL");
            if (yes)
            {
                Sehirler.Remove(t);
                SaveData();
            }
        }
    }

    private void SaveData()
    {
        string data = JsonSerializer.Serialize(Sehirler);
        File.WriteAllText(fileName, data);
    }

    private void ContentPage_Unloaded(object sender, EventArgs e)
    {
        SaveData();
    }
}
