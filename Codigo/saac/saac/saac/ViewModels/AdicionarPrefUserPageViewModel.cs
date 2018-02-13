using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
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

        private bool _inicial;
        public bool Inicial
        {
            get { return _inicial; }
            set { SetProperty(ref _inicial, value); }
        }

        private readonly INavigationService _navigationService;
        private readonly IAzureServicePrefUser<PreferenciaUser> _clientePreferencia;

        private DelegateCommand _salvarCommand;
        public DelegateCommand SalvarCommand =>
            _salvarCommand ?? (_salvarCommand = new DelegateCommand(Salvar));


        public AdicionarPrefUserPageViewModel(INavigationService navigationService, IAzureServicePrefUser<PreferenciaUser> clientePreferencia) : base (navigationService)
        {
            _navigationService = navigationService;
            _clientePreferencia = clientePreferencia;

            Preferencias = new PreferenciaUser();

        }

        public async void Salvar()
        {
            if (Inicial)
            {
                await SalvarPreferencia();
            }
            else
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

                if (parameters.ContainsKey("inicial"))
                {
                    Inicial = (bool)parameters["inicial"]; ;
                }
                else
                {
                    MinhaPreferencia(UserId);
                }
            }
        }

    }
}
