using Microsoft.WindowsAzure.MobileServices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using saac.Models;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Acr.UserDialogs;
using saac.Helpers;
using Plugin.Connectivity;

namespace saac.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Propriedades

        private Facebook _facebookProfile;
        public Facebook FacebookProfile
        {
            get { return _facebookProfile; }
            set { SetProperty(ref _facebookProfile, value); }
        }

        private Usuario _user;
        public Usuario User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private readonly IAzureServiceUser<Usuario> _clienteUser;
        private readonly IFacebookService _clienteFacebook;

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;
        
        #endregion

        #region Construtor

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IAzureServiceUser<Usuario> clienteUser,
            IFacebookService clienteFacebook)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            _clienteUser = clienteUser;
            _clienteFacebook = clienteFacebook;
            
            User = new Usuario();

        }
        #endregion

        #region Métodos

        #region Facebook
        public async Task SetFacebookUserProfileAsync(string accessToken)
        {
            FacebookProfile = await _clienteFacebook.GetFacebookProfileAsync(accessToken);

        }

        public string ExtractAccessTokenFromUrl(string url)
        {

            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {

                var at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");

                var accessToken = at.Remove(at.IndexOf("&expires_in="));

                return accessToken;

            }

            return string.Empty;

        }

        public WebView Initialize()
        {
            string ClientId = "1958735654139902";

            var apiRequest =

                "https://www.facebook.com/dialog/oauth?client_id="
                + ClientId
                + "&display=popup&response_type=token&redirect_uri=http://www.facebook.com/connect/login_success.html";

            var webView = new WebView
            {
                Source = apiRequest,
                HeightRequest = 1

            };

            webView.Navigated += WebViewOnNavigated;

            return webView;
        }


        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {
            var accessToken = ExtractAccessTokenFromUrl(e.Url);

            if (accessToken != "")
            {
                using (var Dialog = UserDialogs.Instance.Loading("Carregando...", null, null, true, MaskType.Black))
                {
                    try
                    {
                        if (CrossConnectivity.Current.IsConnected) {
                            await SetFacebookUserProfileAsync(accessToken);

                            await CriarUsuario();

                        }
                        else
                        {
                            UserDialogs.Instance.Toast("Você está sem conexão.", TimeSpan.FromSeconds(2));

                        }
                    }
                    catch (Exception)
                    {
                        UserDialogs.Instance.Toast("Ops! Ocorreu algum problema.", TimeSpan.FromSeconds(2));

                    }

                }
            }
        }
        #endregion


        private async Task CriarUsuario()
        {
            User.Id = Settings.IdUser = FacebookProfile.Id;
            User.Nome = FacebookProfile.Name;
            User.Foto = FacebookProfile.Picture.Data.Url;
            User.DtNasci = FacebookProfile.DtNascimento;
            User.Sexo = true;
            User.Endereco = "Areia Branca";

            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", User.Id);


           
            var resultado = await _clienteUser.ExisteUsuario(User.Id);
         
            if (resultado != 0)
            {
                    /*var aleatorio = new Random();
                    var resulAleatorio = aleatorio.Next(0, 1);

                    if (resulAleatorio == 0)
                    {
                        await _clienteUser.AtualizarTable(User);
                    }*/
                await _navigationService.NavigateAsync("../PrincipalPage", navigationParams, useModalNavigation: false);

            }
            else
            {
                navigationParams.Add("adicionar", "adicionar");
                User.Administrador = false;

                await _clienteUser.AdicionarTable(User);
                UserDialogs.Instance.Toast("Parabéns!! O seu cadastro foi realizado.", TimeSpan.FromSeconds(2));
                await _navigationService.NavigateAsync("../AdicionarPrefUserPage", navigationParams, useModalNavigation: false);

            }
        }

        public async void Logado()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", Settings.IdUser);

            await _navigationService.NavigateAsync("../PrincipalPage", navigationParams, useModalNavigation: false);

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (Settings.IsLoggedIn)
            {
                Logado();

            }
        }

        #endregion
    }
}
