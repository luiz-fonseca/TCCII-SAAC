using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using saac.Models;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class AdicionarGrupoPageViewModel : ViewModelBase
    {
        #region Propriedades
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

        private DelegateCommand _salvarGrupoCommand;
        public DelegateCommand SalvarGrupoCommand =>
            _salvarGrupoCommand ?? (_salvarGrupoCommand = new DelegateCommand(SalvarGrupo, CondicaoSalvarGrupo))
            .ObservesProperty(() => Grupos.Nome).ObservesProperty(() => Grupos.Descricao).ObservesProperty(() => Grupos.Categoria);

        #endregion

        #region Construtor
        public AdicionarGrupoPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            IAzureServiceGroup<Grupo> clienteGrupo, IAzureServiceAux<Auxiliar> clienteAuxiliar
            ) : base(navigationService)
        {
            _clienteGrupo = clienteGrupo;
            _clienteAuxiliar = clienteAuxiliar;

            Grupos = new Grupo();
            Aux = new Auxiliar();

            _navigationService = navigationService;
            _dialogService = dialogService;

        }
        #endregion

        #region Métodos
        private bool CondicaoSalvarGrupo()
        {
            return !string.IsNullOrWhiteSpace(Grupos.Nome) &&
                !string.IsNullOrWhiteSpace(Grupos.Descricao) &&
                !string.IsNullOrWhiteSpace(Grupos.Categoria);
        }

        private async void SalvarGrupo()
        {
            Grupos.Id = Guid.NewGuid().ToString("N");
           
            await _clienteGrupo.AdicionarTable(Grupos);

            SalvarAuxiliar(Grupos.Id, true);

            Grupos.Nome = string.Empty;
            Grupos.Descricao = string.Empty;
            Grupos.Categoria = string.Empty;

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

        /*
         private async void SelecionarOpcao(){
            if(condicao == "adicionar")
            {
                Message = "Adicionar Grupo";
                await SalvarGrupo();

            }else if(condicao == "alterar")
            {
                Message = "Alterar Grupo";
                await AlterarGrupo();
            }
         }

        /*private async Task AlterarGrupo()
        {

            await  _clienteGrupo.AlterarTable(Grupos);
            var navigationParams = new NavigationParameters();
            navigationParams.Add("grupo", Grupos);
            navigationParams.Add("userId", UserId);


            await _navigationService.GoBackAsync(parameters);

        }*/

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            UserId = parameters.GetValue<string>("userId");

            /*if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"]; ;
            }
            if (parameters.ContainsKey("condicao"))
            {
                Condicao = (string)parameters["condicao"]; ;
            }
            if (parameters.ContainsKey("grupo"))
            {
                Grupos = (string)parameters["grupo"]; ;
            }
             */

        }
        #endregion
    }
}
