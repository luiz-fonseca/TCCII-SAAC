using Microsoft.WindowsAzure.MobileServices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Models;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Plugin.Connectivity;
using Acr.UserDialogs;
using System.Threading.Tasks;

namespace saac.ViewModels
{
	public class MeusGruposPageViewModel : ViewModelBase
    {
        #region Propriedades

        public struct GrupoAux
        {
            public string Id { get; set; }
            public string Nome { get; set; }
            public string Descricao { get; set; }
            public bool Temporario { get; set; }
            public int QtdPubPendente { get; set; }

        }

        public GrupoAux _grupoAux;

        protected bool HasInitialized { get; set; }

        private bool _atualizando = false;
        public bool Atualizando
        {
            get { return _atualizando; }
            set { SetProperty(ref _atualizando, value); }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private ObservableCollection<Group<string, GrupoAux>> _meusGroups;
        public ObservableCollection<Group<string, GrupoAux>> MeusGroups
        {
            get { return _meusGroups; }
            set { SetProperty(ref _meusGroups, value); }
        }

        private readonly IAzureServiceAux<Auxiliar> _clienteAux;
        private readonly IAzureServiceGroup<Grupo> _clienteGroup;
        private readonly IAzureServicePublication<Publicacao> _clientePublication;

        private readonly INavigationService _navigationService;

        private DelegateCommand _adicionarGrupoCommand;
        public DelegateCommand AdicionarGrupoCommand =>
            _adicionarGrupoCommand ?? (_adicionarGrupoCommand = new DelegateCommand(AdicionarGrupo));

        private DelegateCommand _pesquisarGrupoCommand;
        public DelegateCommand PesquisarGrupoCommand =>
            _pesquisarGrupoCommand ?? (_pesquisarGrupoCommand = new DelegateCommand(PesquisarGrupo));

        private DelegateCommand _atualizarCommand;
        public DelegateCommand AtualizarCommand =>
            _atualizarCommand ?? (_atualizarCommand = new DelegateCommand(AtualizarGrupos));

        private DelegateCommand<object> _grupoSelectedCommand;
        public DelegateCommand<object> GrupoSelectedCommand =>
            _grupoSelectedCommand != null ? _grupoSelectedCommand : (_grupoSelectedCommand = new DelegateCommand<object>(ItemTapped));
        #endregion

        #region Construtor
        public MeusGruposPageViewModel(INavigationService navigationService, IAzureServiceAux<Auxiliar> clienteAux,
            IAzureServiceGroup<Grupo> clienteGroup, IAzureServicePublication<Publicacao> clientePublication) : base(navigationService)
        {
            _navigationService = navigationService;
            _clienteAux = clienteAux;
            _clienteGroup = clienteGroup;
            _clientePublication = clientePublication;

            MeusGroups = new ObservableCollection<Group<string, GrupoAux>>();

        }
        #endregion

        #region Métodos
        public void AtualizarGrupos()
        {
            Atualizando = true;

            ExibirMeusGrupos(UserId);

            Atualizando = false;

        }

        public async void ExibirMeusGrupos(string id)
        {
            IsLoading = true;

            await MeusGruposDisponiveis(id);

            IsLoading = false;

        }

        public async Task MeusGruposDisponiveis(string id)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var aux = await _clienteAux.MeusGrupos(id);
                    if (aux.Count == 0)
                    {
                        MeusGroups.Clear();
                        Message = "Você ainda não possui nenhum Grupo. Crie um novo ou Entre em algum.";

                    }
                    else
                    {
                        Message = string.Empty;

                        var resultado = await _clienteGroup.MeusGrupos(aux);
                        var resulPublicacao = await PublicacoesPendentes(resultado);
                        var resultadoAgrupar = Agrupar(resulPublicacao);
                        Converter(resultadoAgrupar);
                    }
                }
                else
                {
                    MeusGroups.Clear();
                    Message = "Você está sem conexão.";

                }
            }
            catch(Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }
            
        }


        public async Task<List<GrupoAux>> PublicacoesPendentes(List<Grupo> meusGrupos)
        {
            var lista = new List<GrupoAux>();

            foreach (var item in meusGrupos)
            {
                var resultadoAux = await _clienteAux.GetAuxiliar(item.Id, UserId);
                var qtdPublicacao = await _clientePublication.QtdPublicacoesPendentes(item.Id, UserId, resultadoAux.DtVisualizacao);

                _grupoAux.Id = item.Id;
                _grupoAux.Nome = item.Nome;
                _grupoAux.Descricao = item.Descricao;
                _grupoAux.Temporario = item.Temporario;
                _grupoAux.QtdPubPendente = qtdPublicacao;

                lista.Add(_grupoAux);

            }
            return lista;

        }

        public IEnumerable<Group<string, GrupoAux>> Agrupar(List<GrupoAux> Grupos)
        {

           var resultado = from grupos in Grupos
                           orderby grupos.Nome
                           group grupos by grupos.Nome[0].ToString().ToUpper() into grupos
                           select new Group<string, GrupoAux>(grupos.Key, grupos);

           return resultado;


        }

       public void Converter(IEnumerable<Group<string, GrupoAux>> listaAgrupada)
       {
           MeusGroups.Clear();
           foreach (var item in listaAgrupada)
           {
               MeusGroups.Add(item);

           }

       }

        public Grupo ConverterGrupo(object args)
        {
            var aux = (GrupoAux)args;

            var GrupoSelecionado = new Grupo();

            GrupoSelecionado.Id = aux.Id;
            GrupoSelecionado.Nome = aux.Nome;
            GrupoSelecionado.Descricao = aux.Descricao;
            GrupoSelecionado.Temporario = aux.Temporario;
            
            return GrupoSelecionado;

        }

        public async void ItemTapped(object args)
        {
            var navigationParams = new NavigationParameters();

           var resultado = ConverterGrupo(args);

            navigationParams.Add("grupo", resultado);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("GrupoSelecionadoPage", navigationParams, useModalNavigation: false);

        }

        public async void AdicionarGrupo()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", UserId);
            navigationParams.Add("adicionar", "adicionar");
            navigationParams.Add("temporario", false);

            await _navigationService.NavigateAsync("AdicionarGrupoPage", navigationParams, useModalNavigation: false);

        }

        public async void PesquisarGrupo()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("PesquisarGrupoPage", navigationParams, useModalNavigation: false);

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (HasInitialized) return;
            HasInitialized = true;

            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                ExibirMeusGrupos(UserId);

            }
            

        }
        #endregion
    }
}
