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

namespace saac.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Propriedades

        private Facebook _facebookProfile;
        public Facebook FacebookProfile
        {
            get { return _facebookProfile; }
            set
            {
                SetProperty(ref _facebookProfile, value);
            }
        }

        private readonly IAzureServiceUser<Usuario> _clienteUser;
        private readonly IFacebookService _clienteFacebook;

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private Usuario _user;
        public Usuario User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private DelegateCommand _criarUsuarioCommand;
        public DelegateCommand CriarUsuarioCommand =>
            _criarUsuarioCommand ?? (_criarUsuarioCommand = new DelegateCommand(CriarUsuario));

        #endregion

        #region Construtor

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IAzureServiceUser<Usuario> clienteUser,
            IFacebookService clienteFacebook)
            : base(navigationService)
        {
            _clienteUser = clienteUser;
            _clienteFacebook = clienteFacebook;
            _dialogService = dialogService;
            _navigationService = navigationService;

        }
        #endregion

        #region Métodos

        #region facebook
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
                await SetFacebookUserProfileAsync(accessToken);

                //chamar o método aqui

                CriarUsuario();

            }
        }
        #endregion


        private async void CriarUsuario()
        {
            User = new Usuario();
            User.Id = FacebookProfile.Id;
            User.Nome = FacebookProfile.Name;
            User.Foto = FacebookProfile.Picture.Data.Url;
            User.DtNasci = FacebookProfile.DtNascimento;
            User.Sexo = true;
            User.Endereco = "Areia Branca";

            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", User.Id);


            try
            {
                var resultado = await _clienteUser.ExisteUsuario(User.Id);
         
                if (resultado != 0)
                {
                    /*var aleatorio = new Random();
                    var resulAleatorio = aleatorio.Next(0, 1);

                    if (resulAleatorio == 0)
                    {
                        await _clienteUser.AtualizarTable(User);
                    }*/
                    await _navigationService.NavigateAsync("../PrincipalPage", navigationParams);

                }
                else
                {
                    navigationParams.Add("inicial", true);
                    User.Administrador = true;

                    await _clienteUser.AdicionarTable(User);
                    await _dialogService.DisplayAlertAsync("Cadastro Realizado", "Parabéns!! O seu cadastro foi realizado.", "OK");
                    await _navigationService.NavigateAsync("../AdicionarPrefUserPage", navigationParams);
                }



            }
            catch (Exception)
            {
                await _dialogService.DisplayAlertAsync("Ops!", "Ocorreu algum problema.", "OK");

            }

        }

        #endregion
    }
}
