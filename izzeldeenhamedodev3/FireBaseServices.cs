using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;

namespace izzeldeenhamedodev3.Models
{
    internal static class FireBaseServices
    {
        private static readonly FirebaseAuthClient authClient;
        private static readonly FirebaseClient dbClient;

        static FireBaseServices()
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyD_ygrfPkA4Q2o_jJVmEXJ9ZcP4qDcYLQo",
                AuthDomain = "izzeldeenhamedodev3.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                },
                // يمكنك تفعيل هذا إن أردت حفظ المستخدم بين الجلسات:
                // UserRepository = new FileUserRepository("MyApp")
            };

            authClient = new FirebaseAuthClient(config);

            dbClient = new FirebaseClient("https://izzeldeenhamedodev3-default-rtdb.firebaseio.com/");
        }

        // تسجيل مستخدم جديد
        public static async Task<UserCredential> Register(string email, string password, string displayName)
        {
            var userCred = await authClient.CreateUserWithEmailAndPasswordAsync(email, password, displayName);

            await dbClient
                .Child("users")
                .Child(userCred.User.Uid)
                .PutAsync(new { name = displayName, email });

            return userCred;
        }

        // تسجيل الدخول
        public static async Task<UserCredential> Login(string email, string password)
        {
            var userCred = await authClient.SignInWithEmailAndPasswordAsync(email, password);
            return userCred;
        }
    }
}
