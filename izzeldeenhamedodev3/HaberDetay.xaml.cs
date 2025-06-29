using izzeldeenhamedodev3.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using izzeldeenhamedodev3;

namespace izzeldeenhamedodev3;

public partial class HaberDetay : ContentPage
{
    private Item haber; 

    public HaberDetay(Item item)
    {
        InitializeComponent();
        haber = item;
        webView.Source = new Uri(haber.link);
    }

    private async void ShareClicked(object sender, EventArgs e)
    {
        await ShareUri(haber.link, Share.Default);
    }

    public async Task ShareUri(string uri, IShare share)
    {
        await share.RequestAsync(new ShareTextRequest
        {
            Uri = uri,
            Title = haber.title
        });
    }
}

