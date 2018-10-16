using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Xamarin.Auth;
using Xamarin.Essentials;
using Newtonsoft.Json.Linq;
using saac.Views;
using saac.UWP.Services;

[assembly: ExportRenderer(typeof(FacebookAuthenticationPage), typeof(FacebookAuthenticationRenderer))]
namespace saac.UWP.Services
{
    public class FacebookAuthenticationRenderer: PageRenderer
    {
        public FacebookAuthenticationRenderer()
        {
            var oauth = new OAuth2Authenticator
            (
                clientId: "1958735654139902",
                scope: "email",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html")
            );

            oauth.Completed += Oauth_Completed;

            Windows.UI.Xaml.Controls.Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
            rootFrame.Navigate(oauth.GetUI(), oauth);

        }

        private async void Oauth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var token = e.Account.Properties["access_token"].ToString();

                var requisicao = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=name,email,picture"), null, e.Account);
                var resposta = await requisicao.GetResponseAsync();

                var obj = JObject.Parse(resposta.GetResponseText());

                var Id = obj["id"].ToString().Replace("\"", "");
                await SecureStorage.SetAsync("UserId", Id);

                var Nome = obj["name"].ToString().Replace("\"", "");
                Preferences.Set("Nome", Nome);

                var Email = obj["email"].ToString().Replace("\"", "");
                Preferences.Set("Email", Email);

                var Picture = obj["picture"]["data"]["url"].ToString().Replace("\"", "");
                Preferences.Set("Picture", Picture);

                await saac.App.NavegacaoAuthentication();
                //saac.App.HideLoginView();

            }
            else
            {
                // await App.NavegacaoLogin();
            }
        }
    }
}
