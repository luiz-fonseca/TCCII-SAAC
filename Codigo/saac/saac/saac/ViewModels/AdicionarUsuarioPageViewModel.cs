using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class AdicionarUsuarioPageViewModel : ViewModelBase
	{
        #region Propriedades
        private Usuario _user;
        public Usuario User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        private readonly INavigationService _navigationService;

        private readonly IAzureServiceUser<Usuario> _clienteUsuario;

        private DelegateCommand _salvarUsuarioCommand;
        public DelegateCommand SalvarUsuarioCommand =>
            _salvarUsuarioCommand ?? (_salvarUsuarioCommand = new DelegateCommand(SalvarUsuario));
        #endregion

        #region Construtor
        public AdicionarUsuarioPageViewModel(INavigationService navigationService, IAzureServiceUser<Usuario> clienteUsuario) : base(navigationService)
        {
            _navigationService = navigationService;

            _clienteUsuario = clienteUsuario;

            User = new Usuario();

        }
        #endregion

        #region Métodos
        public async void SalvarUsuario()
        {
            await _clienteUsuario.AtualizarTable(User);
            UserDialogs.Instance.Toast("Os seus dados foram atualizados", TimeSpan.FromSeconds(2));

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("usuario"))
            {
                User = (Usuario)parameters["usuario"];

            }

        }
        #endregion
    }
}
