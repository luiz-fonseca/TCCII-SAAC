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
	public class PrincipalPageViewModel : ViewModelBase
    {
        #region Propriedades
        public string UserId { get; set; }

        private Usuario _user;
        public Usuario User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private readonly IFacebookLogout _facebookLogout;
        private readonly INavigationService _navigationService;

        private DelegateCommand _logoutCommand;
        public DelegateCommand LogoutCommand =>
            _logoutCommand ?? (_logoutCommand = new DelegateCommand(Logout));

        private DelegateCommand _sobreCommand;
        public DelegateCommand SobreCommand =>
            _sobreCommand ?? (_sobreCommand = new DelegateCommand(Sobre));

        private DelegateCommand _perfilCommand;
        public DelegateCommand PerfilCommand =>
            _perfilCommand ?? (_perfilCommand = new DelegateCommand(Perfil));

        #endregion

        #region Construtor
        public PrincipalPageViewModel(INavigationService navigationService, IFacebookLogout facebookLogout) : base(navigationService)
        {
            _navigationService = navigationService;
            _facebookLogout = facebookLogout;

        }
        #endregion

        #region Métodos
        public async void Perfil()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("UsuarioSelecionadoPage", navigationParams, useModalNavigation: false);

        }


        public void UsuarioSelecionado()
        {
            User = new Usuario
            {
                Nome = Preferences.Get("Nome", "nome"),
                Foto = Preferences.Get("Picture", "")
            };
        }


        public async void Logout()
        {
            _facebookLogout.Logout();
            SecureStorage.RemoveAll();
            Preferences.Clear();

            await _navigationService.NavigateAsync("../MainPage", useModalNavigation: false);

        }

        public async void Sobre()
        {
            await _navigationService.NavigateAsync("SobrePage", useModalNavigation: false);

        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];
                
                UsuarioSelecionado();

            }
        }
        #endregion
    }
}
