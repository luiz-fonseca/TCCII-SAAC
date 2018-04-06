using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Helpers;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class PrincipalPageViewModel : ViewModelBase
    {
        #region Propriedades
        private readonly IFacebookLogout _facebookLogout;
        private readonly INavigationService _navigationService;

        private DelegateCommand _logoutCommand;
        public DelegateCommand LogoutCommand =>
            _logoutCommand ?? (_logoutCommand = new DelegateCommand(Logout));

        private DelegateCommand _sobreCommand;
        public DelegateCommand SobreCommand =>
            _sobreCommand ?? (_sobreCommand = new DelegateCommand(Sobre));

        #endregion

        #region Construtor
        public PrincipalPageViewModel(INavigationService navigationService, IFacebookLogout facebookLogout) : base(navigationService)
        {
            _navigationService = navigationService;
            _facebookLogout = facebookLogout;

        }
        #endregion

        #region Métodos
        public async void Logout()
        {
            _facebookLogout.Logout();
            Settings.IdUser = string.Empty;
            await _navigationService.NavigateAsync("../MainPage", useModalNavigation: false);

        }

        public async void Sobre()
        {
            await _navigationService.NavigateAsync("SobrePage", useModalNavigation: false);

        }
        
        #endregion
    }
}
