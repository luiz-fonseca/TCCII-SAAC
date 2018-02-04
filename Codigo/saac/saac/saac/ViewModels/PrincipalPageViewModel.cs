using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class PrincipalPageViewModel : ViewModelBase
    {
        private readonly IFacebookLogout _facebookLogout;
        private readonly INavigationService _navigationService;

        private DelegateCommand _logoutCommand;
        public DelegateCommand LogoutCommand =>
            _logoutCommand ?? (_logoutCommand = new DelegateCommand(Logout));

        private DelegateCommand _sobreCommand;
        public DelegateCommand SobreCommand =>
            _sobreCommand ?? (_sobreCommand = new DelegateCommand(Sobre));

        public PrincipalPageViewModel(INavigationService navigationService, IFacebookLogout facebookLogout) : base(navigationService)
        {
            _navigationService = navigationService;
            _facebookLogout = facebookLogout;

        }

        public async void Logout()
        {
            _facebookLogout.Logout();
            await _navigationService.NavigateAsync("../MainPage");

        }

        public async void Sobre()
        {
            await _navigationService.NavigateAsync("SobrePage", useModalNavigation: false);

        }
    }
}
