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

namespace saac.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IAzureServiceBase<Usuario> _cliente;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        public Usuario _user;

        public Usuario User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public DelegateCommand CriarUsuarioCommand { get; set; }


        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IAzureServiceBase<Usuario> cliente) 
            : base (navigationService)
        {
            _cliente = cliente;
            _dialogService = dialogService;
            _navigationService = navigationService;
            CriarUsuarioCommand = new DelegateCommand(CriarUsuario);

        }

        private async void CriarUsuario()
        {
            User = new Usuario();
            User.Id = "abcdefghi";
            User.Nome = "Clovis";
            User.Foto = "ok";
            User.DtNasci = new DateTime(24 / 04 / 1984);
            User.Sexo = true;
            User.Endereco = "Areia Branca";

            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", User.Id);


            try
            {
                var resultado = await _cliente.ExisteResgistro(User.Id);
                await _navigationService.NavigateAsync("NavigationPage/PrincipalPage", navigationParams);
      
            }
            catch
            {
                await _cliente.AdicionarTable(_user);
                await _dialogService.DisplayAlertAsync("Cadastro Realizado", "Parabéns!! O seu cadastro foi realizado.", "OK");
                await _navigationService.NavigateAsync("NavigationPage/PrincipalPage?message=3");

            }

        }

    }
}
