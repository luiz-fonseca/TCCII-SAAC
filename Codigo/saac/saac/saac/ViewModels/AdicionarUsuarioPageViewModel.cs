using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace saac.ViewModels
{
	public class AdicionarUsuarioPageViewModel : ViewModelBase
	{
        #region Propriedades
        public string Opcao { get; set; }

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
            _salvarUsuarioCommand ?? (_salvarUsuarioCommand = new DelegateCommand(Salvar));
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
        public async void Salvar()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                if (Opcao.Contains("adicionar"))
                {
                    await SalvarUsuario();
                }
                else if (Opcao.Contains("editar"))
                {
                    await AtualizarUsuario();
                }
            }
            else
            {
                UserDialogs.Instance.Toast("Você está sem conexão.", TimeSpan.FromSeconds(2));

            }

        }

        public async Task SalvarUsuario()
        {
            try
            {
                var navigationParams = new NavigationParameters();
                navigationParams.Add("userId", User.Id);
                navigationParams.Add("adicionar", "adicionar");

                await _clienteUsuario.AdicionarTable(User);
                await _navigationService.NavigateAsync("../AdicionarPrefUserPage", navigationParams, useModalNavigation: false);
                UserDialogs.Instance.Toast("Parabéns!! O seu cadastro foi realizado.", TimeSpan.FromSeconds(2));
                
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }
        }

        public async Task AtualizarUsuario()
        {
            try
            {
                User.Foto = Preferences.Get("Picture", "");
                await _clienteUsuario.AtualizarTable(User);
                UserDialogs.Instance.Toast("Os seus dados foram atualizados", TimeSpan.FromSeconds(2));

            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }

        }


        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("usuario"))
            {
                User = (Usuario)parameters["usuario"];

                if (parameters.ContainsKey("adicionar"))
                {
                    Opcao = (string)parameters["adicionar"];
                }
                else if (parameters.ContainsKey("editar"))
                {
                    Opcao = (string)parameters["editar"];
                }

            }

        }
        #endregion
    }
}
