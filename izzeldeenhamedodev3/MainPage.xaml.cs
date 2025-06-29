using System;
using Microsoft.Maui.Controls;
using izzeldeenhamedodev3.Models;    // تأكد من إضافة هذا الـ using
using Firebase.Auth;            // إذا أردت استخدام UserCredential

namespace izzeldeenhamedodev3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // للتبديل بين شاشتي الدخول والتسجيل
        private void KayitLoginEkraniGoster(object sender, EventArgs e)
        {
            if (kayitEkran.IsVisible)
            {
                kayitEkran.IsVisible = false;
                loginEkran.IsVisible = true;
            }
            else
            {
                loginEkran.IsVisible = false;
                kayitEkran.IsVisible = true;
            }
        }

        // عند الضغط على "Kaydol" → تسجيل حساب جديد
        private async void RegisterClicked(object sender, EventArgs e)
        {
            try
            {
                // ملاحظة: الترتيب (email, password, displayName)
                var result = await FireBaseServices
                    .Register(
                        txtREmail.Text.Trim(),     // Email
                        txtRPassword.Text.Trim(),  // Password
                        txtName.Text.Trim()        // Display Name
                    );

                // إذا لم يُرمَ استثناء، نعتبر التسجيل ناجحاً
                await DisplayAlert("Başarılı", "Kayıt başarılı!", "OK");

                // نرجع لشاشة الدخول
                kayitEkran.IsVisible = false;
                loginEkran.IsVisible = true;
            }
            catch (Exception ex)
            {
                // هنا يمكنك تخصيص الرسالة بناءً على ex.Message
                await DisplayAlert("Hata", "Kayıt başarısız:\n" + ex.Message, "OK");
            }
        }

        // عند الضغط على "Oturum Aç" → تسجيل الدخول
        private async void LoginInClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FireBaseServices
                    .Login(
                        txtEmail.Text.Trim(),     // Email
                        txtPassword.Text.Trim()   // Password
                    );

                // نجاح الدخول
                string name = result.User.Info.DisplayName;
                await DisplayAlert("Hoşgeldin", $"Merhaba {name}!", "OK");

                // مثال للتنقل بعد الدخول
                await Shell.Current.GoToAsync("//NotesPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Giriş Hatası",
                    "Email veya parola hatalı.\n" + ex.Message,
                    "OK");
            }
        }
    }
}
