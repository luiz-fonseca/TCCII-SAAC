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
        
        private string _ex;

        public string Ex
        {
            get { return _ex; }
            set { SetProperty(ref _ex, value); }
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
            _user = new Usuario();
            _user.Id = "aaaa";
            _user.Nome = "Luiz";
            _user.Foto = "ok";
            _user.DtNasci = new DateTime(25 / 11 / 1991);
            _user.Sexo = true;
            _user.Endereco = "Pedra Mole";

            try
            {
                var resultado = await _cliente.ExisteResgistro(_user.Id);
                Ex = resultado.Nome;
                await _navigationService.NavigateAsync("PrincipalPage");

                //var navigationParams = new NavigationParameters();
                //navigationParams.Add("model", new Contact());
                //_navigationService.NavigateAsync("PrincipalPage", navigationParams);
            }
            catch
            {
                 _cliente.AdicionarTable(_user);

                await _dialogService.DisplayAlertAsync("Cadastro", "Parabéns!! O seu cadastro foi realizado.", "OK");

                //var navigationParams = new NavigationParameters();
                //navigationParams.Add("model", _user.Id);
                //_navigationService.NavigateAsync("PrincipalPage", navigationParams);

            }

        }
}
}
