using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using saac.Models;
using saac.Services.Interfaces;
using saac.Services;
using Prism.Services;
using Microsoft.WindowsAzure.MobileServices;

namespace saac.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IAzureServiceUser<Usuario> _clienteUser;

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private Usuario _user;
        public Usuario User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public DelegateCommand CriarUsuarioCommand { get; set; }


        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IAzureServiceUser<Usuario> clienteUser) 
            : base (navigationService)
        {
            _clienteUser = clienteUser;
            _dialogService = dialogService;
            _navigationService = navigationService;
            CriarUsuarioCommand = new DelegateCommand(CriarUsuario);

        }

        private async void CriarUsuario()
        {
            User = new Usuario();
            User.Id = "aaaa";
            User.Nome = "Clovis";
            User.Foto = "ok";
            User.DtNasci = new DateTime(24 / 04 / 1984);
            User.Sexo = true;
            User.Endereco = "Areia Branca";

            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", User.Id);


            try
            {
                await _clienteUser.ExisteResgistro(User.Id);
                await _navigationService.NavigateAsync("NavigationPage/PrincipalPage", navigationParams);
      
            }
            catch(MobileServiceInvalidOperationException)
            {
                await _clienteUser.AdicionarTable(User);
                await _dialogService.DisplayAlertAsync("Cadastro Realizado", "Parabéns!! O seu cadastro foi realizado.", "OK");
                await _navigationService.NavigateAsync("NavigationPage/PrincipalPage", navigationParams);

            }

        }

    }
}
