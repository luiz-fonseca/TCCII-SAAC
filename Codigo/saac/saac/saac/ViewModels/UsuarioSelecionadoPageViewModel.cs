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
    public class UsuarioSelecionadoPageViewModel : ViewModelBase
    {
        #region Propriedades
        public string UserId { get; set; }

        private Usuario _user;
        public Usuario User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private readonly INavigationService _navigationService;

        private readonly IAzureServiceUser<Usuario> _clienteUsuario;

        private DelegateCommand _editarUsuarioCommand;
        public DelegateCommand EditarUsuarioCommand =>
            _editarUsuarioCommand ?? (_editarUsuarioCommand = new DelegateCommand(EditarUsuario, CondicaoEditarUsuario));
        #endregion

        #region Construtor
        public UsuarioSelecionadoPageViewModel(INavigationService navigationService, IAzureServiceUser<Usuario> clienteUsuario) : base(navigationService)
        {
            _navigationService = navigationService;

            _clienteUsuario = clienteUsuario;

            User = new Usuario();

        }
        #endregion

        #region Métodos
        public async void UsuarioSelecionado(string codUsuario)
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    User = await _clienteUsuario.UsuarioSelecionado(codUsuario);

                }
                else
                {
                    UserDialogs.Instance.Toast("Você está sem conexão", TimeSpan.FromSeconds(2));

                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }  
        }

        public async void EditarUsuario()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("usuario", User);
            navigationParams.Add("editar", "editar");

            await _navigationService.NavigateAsync("AdicionarUsuarioPage", navigationParams, useModalNavigation: false);


        }

        public bool CondicaoEditarUsuario()
        {
            return true;
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                UsuarioSelecionado(UserId);

            }
        }
        #endregion
    }
}