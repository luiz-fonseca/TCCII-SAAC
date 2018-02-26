using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using saac.Models;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace saac.ViewModels
{
	public class AdicionarGrupoPageViewModel : ViewModelBase
    {
        #region Propriedades
        private readonly IAzureServiceGroup<Grupo> _clienteGrupo;
        private readonly IAzureServiceAux<Auxiliar> _clienteAuxiliar;
        private readonly IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> _clienteConcursoGrupo;

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

        private AuxConcursoGrupo _auxiliar;
        public AuxConcursoGrupo Auxiliar
        {
            get { return _auxiliar; }
            set { SetProperty(ref _auxiliar, value); }
        }


        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        public bool Temporario { get; set; }

        public string ConcursoId { get; set; }

        private DelegateCommand _salvarGrupoCommand;
        public DelegateCommand SalvarGrupoCommand =>
            _salvarGrupoCommand ?? (_salvarGrupoCommand = new DelegateCommand(Salvar, CondicaoSalvarGrupo))
            .ObservesProperty(() => Grupos.Nome).ObservesProperty(() => Grupos.Descricao).ObservesProperty(() => Grupos.Categoria);

        #endregion

        #region Construtor
        public AdicionarGrupoPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            IAzureServiceGroup<Grupo> clienteGrupo, IAzureServiceAux<Auxiliar> clienteAuxiliar, 
            IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> clienteConcursoGrupo) : base(navigationService)
        {
            _clienteGrupo = clienteGrupo;
            _clienteAuxiliar = clienteAuxiliar;
            _clienteConcursoGrupo = clienteConcursoGrupo;

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

        private async void Salvar()
        {
            var codGrupo = await SalvarGrupo();
            await SalvarAuxiliar(codGrupo, true);

            if (Temporario)
            {
                await SalvarConcursoGrupo(codGrupo);

            }

            await _dialogService.DisplayAlertAsync("Grupo Cadastrado", "Parabéns!! O cadastro" +
                " do seu grupo foi realizado.", "OK");

            await _navigationService.GoBackAsync();
        }

        private async Task<string> SalvarGrupo()
        {
            Grupos.Id = Guid.NewGuid().ToString("N");
            Grupos.Temporario = Temporario;
           
            await _clienteGrupo.AdicionarTable(Grupos);

            Grupos.Nome = string.Empty;
            Grupos.Descricao = string.Empty;
            Grupos.Categoria = string.Empty;


            return Grupos.Id;
        }

        private async Task SalvarAuxiliar(string codGrupo, bool adm)
        {
            Aux.Id = Guid.NewGuid().ToString("N");
            Aux.CodGrupo = codGrupo;
            Aux.CodUsuario = UserId;
            Aux.Adiministrador = adm;

            await _clienteAuxiliar.AdicionarTable(Aux);
        }

        private async Task SalvarConcursoGrupo(string codGrupo)
        {
            Auxiliar = new AuxConcursoGrupo();

            Auxiliar.Id = Guid.NewGuid().ToString("N"); ;
            Auxiliar.CodConcurso = ConcursoId;
            Auxiliar.CodGrupo = codGrupo;

            await _clienteConcursoGrupo.AdicionarTable(Auxiliar);

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
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                if (parameters.ContainsKey("temporario"))
                {
                    Temporario = (bool)parameters["temporario"];

                    if (Temporario)
                    {
                        if (parameters.ContainsKey("concursoId"))
                        {
                            ConcursoId = (string)parameters["concursoId"];

                        }

                    }

                }
            }
            
        }
        #endregion
    }
}
