using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class AdicionarGrupoPageViewModel : ViewModelBase
	{
        private readonly IAzureServiceGroup<Grupo> _clienteGrupo;
        private readonly IAzureServiceAux<Auxiliar> _clienteAuxiliar;

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private Grupo _grupos;
       
        public Grupo Grupos
        {
            get { return _grupos; }
            set { SetProperty(ref _grupos, value); }
        }

        public Auxiliar _aux;

        public Auxiliar Aux
        {
            get { return _aux; }
            set { SetProperty(ref _aux, value); }
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }


        public DelegateCommand SalvarGrupoCommand { get; set; }

        public AdicionarGrupoPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            IAzureServiceGroup<Grupo> clienteGrupo, IAzureServiceAux<Auxiliar> clienteAuxiliar
            ) :base(navigationService)
        {
            _clienteGrupo = clienteGrupo;
            _clienteAuxiliar = clienteAuxiliar;

            Grupos = new Grupo();
            Aux = new Auxiliar();

            _navigationService = navigationService;
            _dialogService = dialogService;

            SalvarGrupoCommand = new DelegateCommand(SalvarGrupo);
        }

        private async void SalvarGrupo()
        {
            Grupos.Id = Guid.NewGuid().ToString("N");

            await _clienteGrupo.AdicionarTable(Grupos);

            SalvarAuxiliar(Grupos.Id, true);

            await _dialogService.DisplayAlertAsync("Grupo Cadastrado", "Parabéns!! O cadastro" +
                " do seu grupo foi realizado.", "OK");
            await _navigationService.GoBackAsync();
        }

        private async void SalvarAuxiliar(string IdGrupo, bool adm)
        { 
            Aux.CodGrupo = IdGrupo;
            Aux.CodUsuario = UserId;
            Aux.Adiministrador = adm;

            await _clienteAuxiliar.AdicionarTable(Aux);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            UserId = parameters.GetValue<string>("userId");

        }

    }
}
