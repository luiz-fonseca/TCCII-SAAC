using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace saac.ViewModels
{
	public class AdicionarPrefUserPageViewModel : ViewModelBase
	{
        #region Propriedades
        private PreferenciaUser _preferencias;
        public PreferenciaUser Preferencias
        {
            get { return _preferencias; }
            set { SetProperty(ref _preferencias, value); }
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        private string _opcao;
        public string Opcao
        {
            get { return _opcao; }
            set { SetProperty(ref _opcao, value); }
        }

        private readonly INavigationService _navigationService;

        private readonly IAzureServicePrefUser<PreferenciaUser> _clientePreferencia;

        private DelegateCommand _salvarCommand;
        public DelegateCommand SalvarCommand =>
            _salvarCommand ?? (_salvarCommand = new DelegateCommand(Salvar, CondicaoSalvar))
            .ObservesProperty(() => Preferencias.Administracao).ObservesProperty(() => Preferencias.Financas)
            .ObservesProperty(() => Preferencias.Licenciatura).ObservesProperty(() => Preferencias.Militar)
            .ObservesProperty(() => Preferencias.Saude).ObservesProperty(() => Preferencias.Ti)
            .ObservesProperty(() => Preferencias.Outra);
        #endregion

        #region Construtor
        public AdicionarPrefUserPageViewModel(INavigationService navigationService, 
            IAzureServicePrefUser<PreferenciaUser> clientePreferencia) : base (navigationService)
        {
            _navigationService = navigationService;

            _clientePreferencia = clientePreferencia;

            Preferencias = new PreferenciaUser();

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
                    await SalvarPreferencia();
                }
                else if (Opcao.Contains("editar"))
                {
                    await AtualizarPreferencia();
                }
            }
            else
            {
                UserDialogs.Instance.Toast("Você está sem conexão.", TimeSpan.FromSeconds(2));

            }

        }

        private bool CondicaoSalvar()
        {
            return Preferencias.Administracao || Preferencias.Financas
                || Preferencias.Licenciatura || Preferencias.Militar
                || Preferencias.Saude || Preferencias.Ti || Preferencias.Outra;
        }

        public async Task SalvarPreferencia()
        {
            try
            {
                Preferencias.Id = Guid.NewGuid().ToString("N");
                Preferencias.CodUsuario = UserId;

                await _clientePreferencia.AdicionarTable(Preferencias);

                UserDialogs.Instance.Toast("As suas preferências foram salvas", TimeSpan.FromSeconds(2));

                var navigationParams = new NavigationParameters();
                navigationParams.Add("userId", UserId);

                await _navigationService.NavigateAsync("../PrincipalPage", navigationParams, useModalNavigation: false);

            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }

        }

        public async Task AtualizarPreferencia()
        {
            try
            {
                await _clientePreferencia.AtualizarTable(Preferencias);

                UserDialogs.Instance.Toast("As suas preferências de concursos foram atualizadas", TimeSpan.FromSeconds(2));
                await _navigationService.GoBackAsync();

            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }

        }

        public async void MinhaPreferencia(string codUsuario)
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    Preferencias = await _clientePreferencia.MinhasPreferencias(codUsuario);

                }
                else
                {
                    UserDialogs.Instance.Toast("Você está sem conexão.", TimeSpan.FromSeconds(2));

                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                if (parameters.ContainsKey("adicionar"))
                {
                    Opcao = (string)parameters["adicionar"];

                }
                else if (parameters.ContainsKey("editar"))
                {
                    Opcao = (string)parameters["editar"];

                    MinhaPreferencia(UserId);

                }
            }
        }
        #endregion

    }
}
