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
        private readonly IPageDialogService _dialogService;

        private readonly IAzureServicePrefUser<PreferenciaUser> _clientePreferencia;

        private DelegateCommand _salvarCommand;
        public DelegateCommand SalvarCommand =>
            _salvarCommand ?? (_salvarCommand = new DelegateCommand(Salvar));
        #endregion

        #region Construtor
        public AdicionarPrefUserPageViewModel(INavigationService navigationService, IAzureServicePrefUser<PreferenciaUser> clientePreferencia,
            IPageDialogService dialogService) : base (navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            _clientePreferencia = clientePreferencia;

            Preferencias = new PreferenciaUser();

        }
        #endregion

        #region Métodos
        public async void Salvar()
        {
            if (Opcao.Contains("adicionar"))
            {
                await SalvarPreferencia();
            }
            else if(Opcao.Contains("editar"))
            {
                await AtualizarPreferencia();
            }

        }

        public async Task SalvarPreferencia()
        {
            Preferencias.Id = Guid.NewGuid().ToString("N");
            Preferencias.CodUsuario = UserId;

            await _clientePreferencia.AdicionarTable(Preferencias);

            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("../PrincipalPage", navigationParams);

        }

        public async Task AtualizarPreferencia()
        {
            await _clientePreferencia.AtualizarTable(Preferencias);

            await _dialogService.DisplayAlertAsync("Preferência do usuário", "As suas preferências de concursos foram atualizadas", "Ok");
            await _navigationService.GoBackAsync();

        }

        public async void MinhaPreferencia(string codUsuario)
        {
            Preferencias = await _clientePreferencia.MinhasPreferencias(codUsuario);

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
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
