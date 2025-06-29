using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace izzeldeenhamedodev3.Models
{
    internal class Servisler
    {
        public static async Task<string> GetAltinDovizGuncelKurlar()
    {
        HttpClient client = new HttpClient();
        string url = "https://finans.truncgil.com/today.json";
        using HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string jsondata = await response.Content.ReadAsStringAsync();
        return jsondata;
    }


        public static async Task<string> HaberleriGetir(Kategori ctg)
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = $"https://api.rss2json.com/v1/api.json?rss_url={ctg.Link}";
                using HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsondata = await response.Content.ReadAsStringAsync();
                return jsondata;
            }
            catch
            {
                return null;
            }
        }
    }
}




