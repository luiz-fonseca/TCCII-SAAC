using Acr.UserDialogs;
using Plugin.Connectivity;
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
using Xamarin.Forms;

namespace saac.ViewModels
{
    public class NotificacoesGrupoPageViewModel : ViewModelBase
    {
        #region Métodos

        public struct AuxPublicacao
        {
            public string Id { get; set; }
            public string CodGrupo { get; set; }
            public string CodUsuario { get; set; }
            public string Texto { get; set; }
            public DateTime DtPublicacao { get; set; }
            public DateTime DtVisualizacao { get; set; }
            public string Nome { get; set; }
            public string Foto { get; set; }
            public int QtdComenPendente { get; set; }

        }

        public AuxPublicacao _auxPublicacao;

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

        private string _mensagem;
        public string Mensagem
        {
            get { return _mensagem; }
            set { SetProperty(ref _mensagem, value); }
        }

        public bool HasInitialized { get; set; }

        public string UserId { get; set; }

        private ObservableCollection<AuxPublicacao> _comentariosPendente;
        public ObservableCollection<AuxPublicacao> ComentariosPendente
        {
            get { return _comentariosPendente; }
            set { SetProperty(ref _comentariosPendente, value); }

        }

        private readonly INavigationService _navigationService;

        private readonly IAzureServicePublication<Publicacao> _clientePublication;
        private readonly IAzureServiceComment<Comentario> _clinteComment;
        private readonly IAzureServiceUser<Usuario> _clienteUser;

        private DelegateCommand _atualizarCommand;
        public DelegateCommand AtualizarCommand =>
            _atualizarCommand ?? (_atualizarCommand = new DelegateCommand(Atualizar));

        private DelegateCommand<object> _publicacaoSelectedCommand;
        public DelegateCommand<object> PublicacaoSelectedCommand =>
            _publicacaoSelectedCommand != null ? _publicacaoSelectedCommand : (_publicacaoSelectedCommand = new DelegateCommand<object>(ItemTapped));

        #endregion

        #region Construtor
        public NotificacoesGrupoPageViewModel(INavigationService navigationService, IAzureServicePublication<Publicacao> clientePublication,
            IAzureServiceComment<Comentario> clinteComment, IAzureServiceUser<Usuario> clienteUser) : base(navigationService)
        {
            _navigationService = navigationService;

            _clientePublication = clientePublication;
            _clinteComment = clinteComment;
            _clienteUser = clienteUser;

            ComentariosPendente = new ObservableCollection<AuxPublicacao>();

        }
        #endregion

        #region Métodos
        public void Atualizar()
        {
            Atualizando = true;

            ExibirRespostasPendentes(UserId);

            Atualizando = false;

        }

        public async void ExibirRespostasPendentes(string codUsuario)
        {
            IsLoading = true;

            await RespostasPublicacoes(codUsuario);

            IsLoading = false;

        }

        public async Task RespostasPublicacoes(string idUser)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {

                    var resulPublicacao = await _clientePublication.MinhasPublicacoes(idUser);
                    var resulUser = await _clienteUser.UsuarioSelecionado(idUser);

                    if (resulPublicacao.Count != 0)
                    {
                        ComentariosPendente.Clear();

                        int count = 0;

                        foreach (var item in resulPublicacao)
                        {
                            var resulComentario = await _clinteComment.QtdComentariosPendentes(item.Id, item.DtVisualizacao, idUser);

                            if (resulComentario != 0)
                            {
                                Mensagem = string.Empty;

                                _auxPublicacao.Id = item.Id;
                                _auxPublicacao.CodGrupo = item.CodGrupo;
                                _auxPublicacao.CodUsuario = item.CodUsuario;
                                _auxPublicacao.Texto = item.Texto;
                                _auxPublicacao.DtPublicacao = item.DtPublicacao;
                                _auxPublicacao.DtVisualizacao = item.DtVisualizacao;
                                _auxPublicacao.Nome = resulUser.Nome;
                                _auxPublicacao.Foto = resulUser.Foto;
                                _auxPublicacao.QtdComenPendente = resulComentario;

                                ComentariosPendente.Add(_auxPublicacao);

                                count += resulComentario;
                            }
                        }

                        MessengerCenter(count);

                        if (ComentariosPendente.Count == 0)
                        {
                            ComentariosPendente.Clear();
                            Mensagem = "Não existe nenhuma resposta a sua publicação";

                        }
                    }
                    else
                    {
                        ComentariosPendente.Clear();
                        Mensagem = "Você ainda não possui nenhuma publicação";

                    }
                }
                else
                {
                    ComentariosPendente.Clear();
                    Mensagem = "Você está sem conexão.";

                }
            }catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }
        }

        public void MessengerCenter(int aux)
        {
            MessagingCenter.Send(new Message
            {
                Value = aux

            }, "Notificacoes");

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

            var aux = (AuxPublicacao)args;

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

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (HasInitialized) return;
            HasInitialized = true;

            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                ExibirRespostasPendentes(UserId);

            }
            
        }
        #endregion
    }
}
