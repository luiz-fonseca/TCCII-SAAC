using Microsoft.WindowsAzure.MobileServices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using saac.Models;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Connectivity;

namespace saac.ViewModels
{
	public class GrupoSelecionadoPageViewModel : ViewModelBase
    {
        #region Propriedades
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

        private Publicacao _publication;
        public Publicacao Publication
        {
            get { return _publication; }
            set { SetProperty(ref _publication, value); }
        }

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

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }
        
        private string _texto;
        public string Texto
        {
            get { return _texto; }
            set { SetProperty(ref _texto, value); }
        }

        private bool _verificar = false;
        public bool Verificar
        {
            get { return _verificar; }
            set { SetProperty(ref _verificar, value); }
        }

        private bool _administrador = false;
        public bool Administrador
        {
            get { return _administrador; }
            set { SetProperty(ref _administrador, value); }
        }

        public bool VerificacaoRealizada { get; set; }

        private ObservableCollection<object> _publicacoesGrupo;
        public ObservableCollection<object> PublicacoesGrupo
        {
            get { return _publicacoesGrupo; }
            set { SetProperty(ref _publicacoesGrupo, value); }

        }

        private readonly IAzureServiceUser<Usuario> _clienteUser;
        private readonly IAzureServicePublication<Publicacao> _clientePublication;
        private readonly IAzureServiceAux<Auxiliar> _clienteAuxiliar;
        private readonly IAzureServiceGroup<Grupo> _clienteGroup;
        private readonly IAzureServiceComment<Comentario> _clienteComment;

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private DelegateCommand _excluirGrupoCommand;
        public DelegateCommand ExcluirGrupoCommand =>
            _excluirGrupoCommand ?? (_excluirGrupoCommand = new DelegateCommand(ExcluirGrupo, CondicaoExcluirGrupo))
            .ObservesProperty(() => Administrador);

        private DelegateCommand _editarGrupoCommand;
        public DelegateCommand EditarGrupoCommand =>
            _editarGrupoCommand ?? (_editarGrupoCommand = new DelegateCommand(EditarGrupo, CondicaoEditarGrupo))
            .ObservesProperty(() => Administrador);

        private DelegateCommand _salvarPublicacaoCommand;
        public DelegateCommand SalvarPublicacaoCommand =>
            _salvarPublicacaoCommand ?? (_salvarPublicacaoCommand = new DelegateCommand(AdicionarPublicacao, CondicaoAdicionarPublicacao))
            .ObservesProperty(() => Texto);

        private DelegateCommand _seguirGrupoCommand;
        public DelegateCommand SeguirGrupoCommand =>
            _seguirGrupoCommand ?? (_seguirGrupoCommand = new DelegateCommand(Seguir));


        private DelegateCommand _atualizarCommand;
        public DelegateCommand AtualizarCommand =>
            _atualizarCommand ?? (_atualizarCommand = new DelegateCommand(AtualizarPublicacoes));


        private DelegateCommand<object> _publicacaoSelectedCommand;
        public DelegateCommand<object> PublicacaoSelectedCommand =>
            _publicacaoSelectedCommand != null ? _publicacaoSelectedCommand : (_publicacaoSelectedCommand = new DelegateCommand<object>(ItemTapped));

        #endregion

        #region Construtor
        public GrupoSelecionadoPageViewModel(INavigationService navigationService, IAzureServicePublication<Publicacao> clientePublication,
            IAzureServiceUser<Usuario> clienteUser, IAzureServiceAux<Auxiliar> clienteAuxiliar, IAzureServiceGroup<Grupo> clienteGroup,
            IAzureServiceComment<Comentario> clienteComment, IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            _clientePublication = clientePublication;
            _clienteUser = clienteUser;
            _clienteAuxiliar = clienteAuxiliar;
            _clienteGroup = clienteGroup;
            _clienteComment = clienteComment;

            Publication = new Publicacao();
            Grupos = new Grupo();
            Aux = new Auxiliar();

            PublicacoesGrupo = new ObservableCollection<object>();

        }
        #endregion

        #region Métodos
        private bool CondicaoAdicionarPublicacao()
        {
            return !string.IsNullOrWhiteSpace(Texto);
        }


        public bool CondicaoEditarGrupo()
        {
            return Administrador;

        }

        public bool CondicaoExcluirGrupo()
        {
            return Administrador;

        }

        public void AtualizarPublicacoes()
        {
            Atualizando = true;

            ExibirPublicacoesDisponiveis(Grupos.Id);
            
            Verificacao(Grupos.Id, UserId, true);

            Atualizando = false;
                
        }

        public async void ExibirPublicacoesDisponiveis(string codGrupo)
        {
            IsLoading = true;

            await PublicacoesDisponiveis(codGrupo);

            IsLoading = false;

        }

        public async void ExcluirGrupo()
        {
            using (var Dialog = UserDialogs.Instance.Loading("Excluindo...", null, null, true, MaskType.Black))
            {
                await RemoverGrupo();

            }

            UserDialogs.Instance.Toast("Este grupo e suas publicações foram excluídos", TimeSpan.FromSeconds(2));

            await _navigationService.GoBackAsync();

        }

        public async void Verificacao(string IdGrupo, string IdUsuario, bool atualizar)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var resultado = await _clienteAuxiliar.GetAuxiliar(IdGrupo, IdUsuario);

                    if (resultado.CodUsuario != IdUsuario)
                    {
                        Verificar = false;
                        Administrador = false;

                    }
                    else
                    {
                        Verificar = true;
                        Administrador = resultado.Adiministrador;

                        if (atualizar)
                        {
                            var aux = resultado;
                            aux.DtVisualizacao = DateTime.Now;
                            await _clienteAuxiliar.AtualizarTable(aux);

                        }
                        
                    }
                    VerificacaoRealizada = true;

                }
                else
                {
                    VerificacaoRealizada = false;
                    UserDialogs.Instance.Toast("Você está sem conexão.", TimeSpan.FromSeconds(2));

                }
            }
            catch (Exception)
            {
                VerificacaoRealizada = false;
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));
            }
        }

        public async void EditarGrupo()
        {

            var navigationParams = new NavigationParameters();
            navigationParams.Add("editar", "editar");
            navigationParams.Add("grupo", Grupos);

            await _navigationService.NavigateAsync("AdicionarGrupoPage", navigationParams, useModalNavigation: false);

        }

        public async void Seguir()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    //var resultado = await _clienteAuxiliar.ExisteSeguirAux(Grupos.Id, UserId);
                    //Verificacao(Grupos.Id, UserId);

                    //if (resultado == 0)
                    if (VerificacaoRealizada)
                    {
                        Verificacao(Grupos.Id, UserId, false);

                    }
                    
                    if (!Verificar)
                    {
                        await SeguirGrupo();
                        
                    }
                    else
                    {
                        await DeixarSeguirGrupo();
                        
                    }
                    Verificacao(Grupos.Id, UserId, false);

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

        public async Task SeguirGrupo()
        {
            Aux.CodGrupo = Grupos.Id;
            Aux.CodUsuario = UserId;
            Aux.Adiministrador = false;
            Aux.DtInscricao = DateTime.Now;
            Aux.DtVisualizacao = DateTime.Now;

            await _clienteAuxiliar.AdicionarTable(Aux);

            UserDialogs.Instance.Toast("Parabéns!! você agora " +
                "está seguindo este grupo.", TimeSpan.FromSeconds(2));

        }

        public async Task DeixarSeguirGrupo()
        {
            var resulSeguir = await _dialogService.DisplayAlertAsync("Seguindo Grupo", "Deseja deixar de seguir este grupo?", " Sim ", " Não ");
            if (resulSeguir)
            {
                var quantidade = await _clienteAuxiliar.QuantidadeRegistros(Grupos.Id);
                if (quantidade == 1 && Grupos.Temporario == false)
                {
                    var resulGrupo = await _dialogService.DisplayAlertAsync("Excluir Grupo", "Você é o último membro seguindo este grupo," +
                        " Deixando de segui-lo, este grupo e todas as suas publicações serão excluidas. Deseja Continuar?", " Sim ", " Não ");

                    if (resulGrupo)
                    {
                        using (var Dialog = UserDialogs.Instance.Loading("Excluindo...", null, null, true, MaskType.Black))
                        {
                            var resultadoAux = await _clienteAuxiliar.GetAuxiliar(Grupos.Id, UserId);

                            await RemoverGrupo();

                            await _clienteAuxiliar.RemoverTable(resultadoAux);

                        }
                        UserDialogs.Instance.Toast("Este grupo e suas publicações foram excluídos", TimeSpan.FromSeconds(2));

                        await _navigationService.GoBackAsync();

                    }
                }
                else
                {
                    var resultadoAux = await _clienteAuxiliar.GetAuxiliar(Grupos.Id, UserId);

                    await _clienteAuxiliar.RemoverTable(resultadoAux);

                    UserDialogs.Instance.Toast("Você deixou de seguir este grupo.", TimeSpan.FromSeconds(2));

                }
            }

        }

        public async void AdicionarPublicacao()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Publication.Id = Guid.NewGuid().ToString("N");
                    Publication.CodUsuario = UserId;
                    Publication.CodGrupo = Grupos.Id;
                    Publication.Texto = Texto;
                    Publication.DtPublicacao = DateTime.Now;
                    Publication.DtVisualizacao = DateTime.Now;

                    await _clientePublication.AdicionarTable(Publication);

                    Texto = string.Empty;

                    AtualizarPublicacoes();
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

        public async Task RemoverGrupo()
        {
            foreach (var item in PublicacoesGrupo)
            {
                var auxConversao = ConversaoAux(item);
                var pub = (Publicacao)auxConversao[0];

                var auxComment = await _clienteComment.Comentarios(pub.Id);

                foreach (var itens in auxComment)
                {
                    await _clienteComment.RemoverTable(itens);

                }
                await _clientePublication.RemoverTable(pub);

            }
            await _clienteGroup.RemoverTable(Grupos);
        }

        public async Task PublicacoesDisponiveis(string codGrupo)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    List<string> auxList = new List<string>();

                    var resulPublication = await _clientePublication.Publicacoes(codGrupo);

                    if (resulPublication.Count != 0)
                    {
                        Message = string.Empty;

                        foreach (var item in resulPublication)
                        {
                            if (!auxList.Contains(item.CodUsuario))
                            {
                                auxList.Add(item.CodUsuario);

                            }
                        }

                        var resulUser = await _clienteUser.Usuarios(auxList);

                        var resulatdo = resulPublication.Join(resulUser, p => p.CodUsuario, u => u.Id,
                                                                (p, u) => new { p.Id, p.CodGrupo, p.CodUsuario, p.Texto, p.DtPublicacao, p.DtVisualizacao, u.Nome, u.Foto });

                        PublicacoesGrupo.Clear();
                        foreach (var item in resulatdo)
                        {
                            PublicacoesGrupo.Add(item);

                        }
                    }
                    else
                    {
                        PublicacoesGrupo.Clear();
                        Message = "Este grupo ainda não possui nenhuma publicação.";

                    }
                }
                else
                {
                    PublicacoesGrupo.Clear();
                    Message = "Você está sem conexão.";

                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }

        }

        public async void ItemTapped(object args)
        {
            var aux = ConversaoAux(args);

            var publica = (Publicacao)aux[0];
            var nome = (string)aux[1];
            var foto = (string)aux[2];

            var navigationParams = new NavigationParameters();
            navigationParams.Add("publicacao", publica);
            navigationParams.Add("nome", nome);
            navigationParams.Add("foto", foto);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("PublicacaoSelecionadaPage", navigationParams, useModalNavigation: false);

        }

        public object[] ConversaoAux(object args)
        {
            object[] vetor = new object[3];
            var publica = new Publicacao();
            string Nome;
            string Foto;
            var aux = Conversao(args, new { Id = "", CodGrupo = "", CodUsuario = "", Texto = "", DtPublicacao = DateTime.Now, DtVisualizacao = DateTime.Now, Nome = "", Foto = "" });

            publica.Id = aux.Id;
            publica.CodGrupo = aux.CodGrupo;
            publica.CodUsuario = aux.CodUsuario;
            publica.Texto = aux.Texto;
            publica.DtPublicacao = aux.DtPublicacao;
            publica.DtVisualizacao = aux.DtVisualizacao;

            Nome = aux.Nome;
            Foto = aux.Foto;

            vetor[0] = publica;
            vetor[1] = Nome;
            vetor[2] = Foto;

            return vetor;

        }

        public T Conversao<T>(object objeto, T tipo)
        {
            return (T)objeto;

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                if (parameters.ContainsKey("grupo"))
                {
                    Grupos = (Grupo)parameters["grupo"];

                    ExibirPublicacoesDisponiveis(Grupos.Id);

                    Verificacao(Grupos.Id, UserId, true);
                }
            }
            
        }
        #endregion
    }
}
