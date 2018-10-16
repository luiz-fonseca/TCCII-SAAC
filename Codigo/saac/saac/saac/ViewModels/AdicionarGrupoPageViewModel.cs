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
using Acr.UserDialogs;
using Xamarin.Essentials;

namespace saac.ViewModels
{
	public class AdicionarGrupoPageViewModel : ViewModelBase
    {
        #region Propriedades
        private readonly IAzureServiceGroup<Grupo> _clienteGrupo;
        private readonly IAzureServiceAux<Auxiliar> _clienteAuxiliar;
        private readonly IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> _clienteConcursoGrupo;

        private readonly INavigationService _navigationService;

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

        private string _mensagem;
        public string Mensagem
        {
            get { return _mensagem; }
            set { SetProperty(ref _mensagem, value); }
        }

        private string _opcao;
        public string Opcao
        {
            get { return _opcao; }
            set { SetProperty(ref _opcao, value); }
        }

        private DelegateCommand _salvarGrupoCommand;
        public DelegateCommand SalvarGrupoCommand =>
            _salvarGrupoCommand ?? (_salvarGrupoCommand = new DelegateCommand(SelecionarOpcao, CondicaoSalvarGrupo))
            .ObservesProperty(() => Grupos.Nome).ObservesProperty(() => Grupos.Descricao);

        #endregion

        #region Construtor
        public AdicionarGrupoPageViewModel(INavigationService navigationService, IAzureServiceGroup<Grupo> clienteGrupo, 
            IAzureServiceAux<Auxiliar> clienteAuxiliar, IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> clienteConcursoGrupo) : base(navigationService)
        {
            _navigationService = navigationService;

            _clienteGrupo = clienteGrupo;
            _clienteAuxiliar = clienteAuxiliar;
            _clienteConcursoGrupo = clienteConcursoGrupo;

            Grupos = new Grupo();
            Aux = new Auxiliar();
            
        }
        #endregion

        #region Métodos
        private bool CondicaoSalvarGrupo()
        {
            return !string.IsNullOrWhiteSpace(Grupos.Nome) &&
                !string.IsNullOrWhiteSpace(Grupos.Descricao);
        }

        private async Task Salvar()
        {
            try
            {
                var codGrupo = await SalvarGrupo();
                await SalvarAuxiliar(codGrupo, true);

                if (Temporario)
                {
                    await SalvarConcursoGrupo(codGrupo);

                }

                UserDialogs.Instance.Toast("Parabéns!! O cadastro" +
                    " do seu grupo foi realizado.", TimeSpan.FromSeconds(2));

                await _navigationService.GoBackAsync();

            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }
        }

        private async Task<string> SalvarGrupo()
        {
            Grupos.Id = Guid.NewGuid().ToString("N");
            Grupos.Temporario = Temporario;
           
            await _clienteGrupo.AdicionarTable(Grupos);

            Grupos.Nome = string.Empty;
            Grupos.Descricao = string.Empty;
            
            return Grupos.Id;
        }

        private async Task SalvarAuxiliar(string codGrupo, bool adm)
        {
            Aux.Id = Guid.NewGuid().ToString("N");
            Aux.CodGrupo = codGrupo;
            Aux.CodUsuario = UserId;
            Aux.Adiministrador = adm;
            Aux.DtInscricao = DateTime.Now;
            Aux.DtVisualizacao = DateTime.Now;

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

        
         private async void SelecionarOpcao(){
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                if (Opcao.Contains("adicionar"))
                {
                    await Salvar();

                }
                else if (Opcao.Contains("editar"))
                {
                    await AlterarGrupo();

                }
            }
            else
            {
                UserDialogs.Instance.Toast("Você está sem conexão.", TimeSpan.FromSeconds(2));

            }
         }

        
        private async Task AlterarGrupo()
        {
            try
            {
                await _clienteGrupo.AtualizarTable(Grupos);

                UserDialogs.Instance.Toast("Alteração do grupo foi realizada", TimeSpan.FromSeconds(2));

                await _navigationService.GoBackAsync();

            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("adicionar"))
            {
                Opcao = (string)parameters["adicionar"];

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
                    Mensagem = "Adicionar Grupo";

                }
            }
            else if (parameters.ContainsKey("editar"))
            {
                Opcao = (string)parameters["editar"];

                if (parameters.ContainsKey("grupo"))
                {
                    Grupos = (Grupo)parameters["grupo"];

                    Mensagem = "Editar Grupo";

                }
            }
   
        }
        #endregion
    }
}
