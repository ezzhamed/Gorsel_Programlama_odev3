using izzeldeenhamedodev3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace izzeldeenhamedodev3;

public partial class Haberler : ContentPage
{
	
    public List<Kategori> Kategoriler { get; set; }

    public List<Item> HaberListesi { get; set; }

    public Haberler()
    {
        InitializeComponent();
        Kategoriler = Kategori.liste;
        lstKategoriler.ItemsSource = Kategoriler;

        LoadHaberler();
    }
    private async void LoadHaberler()
    {
        if (lstKategoriler.SelectedItem != null)
        {
            await LoadData((Kategori)lstKategoriler.SelectedItem);
        }
        else
        {
            await LoadData(Kategoriler.FirstOrDefault());
        }
    }

    async Task LoadData(Kategori seciliKategori)
    {
        string haberJson = await Servisler.HaberleriGetir(seciliKategori);

        if (!string.IsNullOrEmpty(haberJson))
        {
            Root haberRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(haberJson);
            HaberListesi = haberRoot.items;

            lstHaberler.ItemsSource = HaberListesi;
        }
    }

    private async void LstHaberler_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lstHaberler.SelectedItem is Item seciliHaber)
        {
            await Navigation.PushAsync(new HaberDetay(seciliHaber));

        }
    }

    private void lstKategoriler_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        LoadHaberler();
    }

    private async void LoadHaberler(object sender, EventArgs e)
    {
        // Your code here
    }


}