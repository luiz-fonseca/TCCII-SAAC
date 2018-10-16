using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace saac.ViewModels
{
	public class GerenciarUsuarioPageViewModel : ViewModelBase
	{

        private Usuario _user;
        public Usuario User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private readonly IAzureServiceUser<Usuario> _clienteUser;

        private readonly INavigationService _navigationService;

        public GerenciarUsuarioPageViewModel(INavigationService navigationService, IAzureServiceUser<Usuario> clienteUser) :base(navigationService)
        {
            _navigationService = navigationService;

            _clienteUser = clienteUser;

            User = new Usuario();

            GerenciarUsuario();
        }

        private async void GerenciarUsuario()
        {
            using (var Dialog = UserDialogs.Instance.Loading("Carregando...", null, null, true, MaskType.Black))
            {

                User.Nome = Preferences.Get("Nome", "nome");
                //Email = Preferences.Get("Email", "email");
                User.Foto = Preferences.Get("Picture", "");
                User.DtNasci = Preferences.Get("Birthday", DateTime.Now);
                User.Sexo = true;
                User.Administrador = false;
                User.Endereco = "";
                User.Id = await SecureStorage.GetAsync("UserId");
                
                var navigationParams = new NavigationParameters();
                
                var resultado = await _clienteUser.ExisteUsuario(User.Id);

                if (resultado != 0)
                {
                    navigationParams.Add("userId", User.Id);

                    await _navigationService.NavigateAsync("../../PrincipalPage", navigationParams, useModalNavigation: false);
                    
                }
                else
                {
                    navigationParams.Add("usuario", User);
                    navigationParams.Add("adicionar", "adicionar");
                    
                    await _navigationService.NavigateAsync("../../AdicionarUsuarioPage", navigationParams, useModalNavigation: false);

                }
            }
        }
    }
}
