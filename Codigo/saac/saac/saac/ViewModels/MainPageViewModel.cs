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
using Xamarin.Essentials;

namespace saac.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Propriedades
        
        private readonly INavigationService _navigationService;
       
        private DelegateCommand _facebookCommand;
        public DelegateCommand FacebookCommand =>
            _facebookCommand ?? (_facebookCommand = new DelegateCommand(Facebook));

        #endregion

        #region Construtor

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IAzureServiceUser<Usuario> clienteUser) : base(navigationService)
        {
            _navigationService = navigationService;
            
        }
        #endregion

        #region Métodos
        private async void Facebook()
        {
            await _navigationService.NavigateAsync("../FacebookAuthenticationPage");
        }


      
       /*
        private async Task CriarUsuario()
        {
            User.Id = FacebookProfile.Id;
            User.Nome = FacebookProfile.Name;
            User.Foto = FacebookProfile.Picture.Data.Url;
            User.DtNasci = FacebookProfile.DtNascimento;
            User.Sexo = true;
            User.Endereco = "Areia Branca";

            await SecureStorage.SetAsync("UserId", User.Id);

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
                    }
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
*/

        public async void Logado()
        {
            var oauthToken = await SecureStorage.GetAsync("UserId");
            if (oauthToken != null)
            {
                var navigationParams = new NavigationParameters();
                navigationParams.Add("userId", oauthToken);

                await _navigationService.NavigateAsync("../PrincipalPage", navigationParams, useModalNavigation: false);
            }
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            
            var myValue = Preferences.Get("publicidade", null);
            if (myValue == null)
            {
                Preferences.Set("publicidade", "ca - app - pub - 3940256099942544 / 6300978111");
            }

            Logado();

            
        }

        #endregion
    }
}
