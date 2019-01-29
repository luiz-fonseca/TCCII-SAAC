using Acr.UserDialogs;
using ImTools;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace saac.ViewModels
{
	public class CategoriasConcursoPageViewModel : ViewModelBase
	{
        #region Propriedades
        protected bool HasInitialized { get; set; }

        private bool _verificadorAdm = false;
        public bool VerificadorAdm 
        {
            get { return _verificadorAdm; }
            set
            {
                SetProperty(ref _verificadorAdm, value);
                AdministradorCommand.RaiseCanExecuteChanged();
            }
        }

        public bool VerificacaoRealizada { get; set; }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

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

        private string _pesquisar;
        public string Pesquisar
        {
            get { return _pesquisar; }
            set { SetProperty(ref _pesquisar, value); }
        }

        private string _regiao;
        public string Regiao
        {
            get {return _regiao; }
            set {SetProperty(ref _regiao, value); }
        }

        
        private string _mensagem;
        public string Mensagem
        {
            get { return _mensagem; }
            set { SetProperty(ref _mensagem, value); }
        }

        private int _itemRegioes;
        public int ItemRegioes
        {
            get { return _itemRegioes; }
            set
            {
                SetProperty(ref _itemRegioes, value);
                InicializarEstados(_itemRegioes);

            }
        }

        private NetworkAccess _access;
        public NetworkAccess Access
        {
            get { return _access; }
            set { SetProperty(ref _access, value); }
        }

        private ObservableCollection<Group<string, Concurso>> _concursosAgrupados;
        public ObservableCollection<Group<string, Concurso>> ConcursosAgrupados
        {
            get { return _concursosAgrupados; }
            set { SetProperty(ref _concursosAgrupados, value); }
        }

        private ObservableCollection<object> _categorias;
        public ObservableCollection<object> Categorias
        {
            get { return _categorias; }
            set { SetProperty(ref _categorias, value); }
        }

        private readonly INavigationService _navigationService;
        private readonly IAzureServiceUser<Usuario> _clienteUser;
        private readonly IAzureServiceConcurso<Concurso> _clienteConcurso;

        private DelegateCommand _pesquisarConcursoCommand;
        public DelegateCommand PesquisarConcursoCommand =>
            _pesquisarConcursoCommand ?? (_pesquisarConcursoCommand = new DelegateCommand(ExibirPesquisarConcurso));

        private DelegateCommand _atualizarCommand;
        public DelegateCommand AtualizarCommand =>
            _atualizarCommand ?? (_atualizarCommand = new DelegateCommand(AtualizarVerificador));

        private DelegateCommand _administradorCommand;
        public DelegateCommand AdministradorCommand =>
            _administradorCommand ?? (_administradorCommand = new DelegateCommand(Administrador, CondicaoAdministrador))
            .ObservesProperty(()=> Access);

        /*private DelegateCommand<object> _categoriaSelectedCommand;
        public DelegateCommand<object> CategoriaSelectedCommand =>
            _categoriaSelectedCommand != null ? _categoriaSelectedCommand : (_categoriaSelectedCommand = new DelegateCommand<object>(ItemTapped));
*/
        private DelegateCommand<Concurso> _concursoSelectedCommand;
        public DelegateCommand<Concurso> ConcursoSelectedCommand =>
            _concursoSelectedCommand != null ? _concursoSelectedCommand : (_concursoSelectedCommand = new DelegateCommand<Concurso>(ItemTapped));
        #endregion

        #region Construtor
        public CategoriasConcursoPageViewModel(INavigationService navigationService, IAzureServiceUser<Usuario> clienteUser, IAzureServiceConcurso<Concurso> clienteConcurso) : base(navigationService)
        {
            _navigationService = navigationService;
            _clienteUser = clienteUser;

            _clienteConcurso = clienteConcurso;
            
            ConcursosAgrupados = new ObservableCollection<Group<string, Concurso>>();
            
            Categorias = new ObservableCollection<object>();

            ConnectivityTest();
        }

        #endregion

        #region Métodos
        public void Atualizar()
        {
            Atualizando = true;

            ExibirConcursos(Regiao);
            
            Atualizando = false;
        }

        public void InicializarEstados(int item)
        {
            Atualizando = true;

            var resul = (string)Categorias[item];

            ExibirConcursos(resul);

            Atualizando = false;

        }
        

        public async void ExibirConcursos(string args)
        {
            IsLoading = true;

            await ConcursosDisponiveis(args);

            IsLoading = false;

        }

        public async void ExibirPesquisarConcurso()
        {
            IsLoading = true;

            await PesquisarConcurso();

            IsLoading = false;

        }

        private void ConnectivityTest()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
           
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            //Access = e.NetworkAccess;
            //var profiles = e.ConnectionProfiles;
            
        }

        public void AtualizarVerificador()
        {
            Atualizando = true;

            Verificacao(UserId);

            Atualizando = false;

        }


        public void ExibirCategorias()
        {
            Categorias.Add("Tudo");
            Categorias.Add("Nacional");
            Categorias.Add("Nordeste" );
            Categorias.Add("Norte" );
            Categorias.Add("Sudeste" );
            Categorias.Add("Sul" );
            Categorias.Add("Centro-Oeste");

            Regiao = "Tudo";

        }


        private async void ItemTapped(object obj)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("concurso", obj);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("ConcursoSelecionadoPage", navigationParams, useModalNavigation: false);

        }

        private async void Administrador()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("GerenciarConcursoPage", navigationParams, useModalNavigation: false);
        }

        private bool CondicaoAdministrador()
        {
            return VerificadorAdm || Access == NetworkAccess.Internet;

        }

        public async void Verificacao(string id)
        {
            
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    VerificadorAdm = await _clienteUser.VerificarAdministrador(id);
                    VerificacaoRealizada = true;

                }
                else
                {
                    VerificacaoRealizada = false;

                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }
        }

        public async Task PesquisarConcurso()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    List<Concurso> resultado;
                    if (Regiao.Equals("Tudo"))
                    {
                        resultado = await _clienteConcurso.PesquisarConcursos(Pesquisar);
                    }
                    else
                    {
                        resultado = await _clienteConcurso.PesquisarConcursos(Regiao, Pesquisar);

                    }
                    
                    if (resultado.Count != 0)
                    {
                        Mensagem = string.Empty;

                        var listaAgrupada = Agrupar(resultado);

                        Converter(listaAgrupada);
                    }
                    else
                    {
                        ConcursosAgrupados.Clear();
                        Mensagem = "Nenhum resultado encontrado";
                    }
                }
                else
                {
                    Mensagem = "Você está sem conexão";

                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }
        }

        public async Task ConcursosDisponiveis(string regiao)
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    List<Concurso> resultado;

                    if (regiao.Equals("Tudo"))
                    {
                        resultado = await _clienteConcurso.ConcursosDisponiveis();

                    }
                    else
                    {
                        resultado = await _clienteConcurso.ConcursosDisponiveis(regiao);

                    }
                    

                    if (resultado.Count != 0)
                    {
                        Mensagem = string.Empty;

                        var listaAgrupada = Agrupar(resultado);

                        Converter(listaAgrupada);
                    }
                    else
                    {
                        ConcursosAgrupados.Clear();
                        Mensagem = "Está região ainda não possui nenhum Concurso Disponível";
                    }
                }
                else
                {
                    Mensagem = "Você está sem conexão";

                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }

        }

        public IEnumerable<Group<string, Concurso>> Agrupar(List<Concurso> concursos)
        {

            var resultado = from concurso in concursos
                            orderby concurso.Estado
                            group concurso by concurso.Estado into grupos
                            select new Group<string, Concurso>(grupos.Key, grupos);

            return resultado;


        }

        public void Converter(IEnumerable<Group<string, Concurso>> listaAgrupada)
        {
            ConcursosAgrupados.Clear();
            foreach (var item in listaAgrupada)
            {
                ConcursosAgrupados.Add(item);

            }

        }


        /*public string ConversaoCategoria(object args)
        {
            var aux = Conversao(args, new { Nome = "" });

            var resutado = aux.Nome;

            return resutado;
            
        }

        public T Conversao<T>(object objeto, T tipo)
        {
            return (T)objeto;

        }*/

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (HasInitialized) return;
            HasInitialized = true;

            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];
                
                ExibirCategorias();

                Verificacao(UserId);
                
            }
            
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

        }

        #endregion
    }
}
