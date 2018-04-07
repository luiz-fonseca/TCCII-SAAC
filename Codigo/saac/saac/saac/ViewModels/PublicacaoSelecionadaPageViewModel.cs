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
using Acr.UserDialogs;

namespace saac.ViewModels
{
	public class PublicacaoSelecionadaPageViewModel : ViewModelBase
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

        private string _nome;
        public string Nome
        {
            get { return _nome; }
            set { SetProperty(ref _nome, value); }
        }

        private string _foto;
        public string Foto
        {
            get { return _foto; }
            set { SetProperty(ref _foto, value); }
        }

        private Comentario _comentarios;
        public Comentario Comentarios
        {
            get { return _comentarios; }
            set { SetProperty(ref _comentarios, value); }
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

        private string _texto;
        public string Texto
        {
            get { return _texto; }
            set
            {
                SetProperty(ref _texto, value);
                SalvarComentarioCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _verificarExcluirPublicaco = false;
        public bool VerificarExcluirPublicaco
        {
            get { return _verificarExcluirPublicaco; }
            set
            {
                SetProperty(ref _verificarExcluirPublicaco, value);
                ExcluirPublicacaoCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<object> _comentariosPublication;
        public ObservableCollection<object> ComentariosPublication
        {
            get { return _comentariosPublication; }
            set { SetProperty(ref _comentariosPublication, value); }

        }

        private readonly IAzureServiceUser<Usuario> _clienteUser;
        private readonly IAzureServicePublication<Publicacao> _clientePublication;
        private readonly IAzureServiceComment<Comentario> _clienteComment;

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private DelegateCommand _salvarComentarioCommand;
        public DelegateCommand SalvarComentarioCommand =>
            _salvarComentarioCommand ?? (_salvarComentarioCommand = new DelegateCommand(AdicionarComentario, CondicaoAdicionarComentario));

        private DelegateCommand _atualizarCommand;
        public DelegateCommand AtualizarCommand =>
            _atualizarCommand ?? (_atualizarCommand = new DelegateCommand(AtualizarComentarios));

        private DelegateCommand _excluirPublicacaoCommand;
        public DelegateCommand ExcluirPublicacaoCommand =>
            _excluirPublicacaoCommand ?? (_excluirPublicacaoCommand = new DelegateCommand(ExcluirPublicacao, CondicaoExcluirPublicacao));


        private DelegateCommand<object> _excluirComentarioSelectedCommand;
        public DelegateCommand<object> ExcluirComentarioSelectedCommand =>
            _excluirComentarioSelectedCommand != null ? _excluirComentarioSelectedCommand : (_excluirComentarioSelectedCommand = new DelegateCommand<object>(ExcluirComentarioSelecionado));
        
        #endregion

        #region Construtor
        public PublicacaoSelecionadaPageViewModel(INavigationService navigationService, IAzureServiceComment<Comentario> clienteComment,
                                            IAzureServiceUser<Usuario> clienteUser, IAzureServicePublication<Publicacao> clientePublication,
                                            IPageDialogService dialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            _clienteComment = clienteComment;
            _clienteUser = clienteUser;
            _clientePublication = clientePublication;

            Publication = new Publicacao();
            Comentarios = new Comentario();

            ComentariosPublication = new ObservableCollection<object>();

        }
        #endregion

        #region Métodos
        private bool CondicaoAdicionarComentario()
        {
            return !string.IsNullOrWhiteSpace(Texto);

        }

        public async void ExcluirComentarioSelecionado(object args)
        {
            var comment = ConversaoComentario(args);

            var auxResul = await _clienteComment.MeuCometario(comment.Id, UserId);
            if (auxResul != 0)
            {
                var resulComment = await _dialogService.DisplayAlertAsync("Excluir Comentário", "Deseja excluir este cometário?", " Sim ", " Não ");

                if (resulComment)
                {
                    ExcluirComentario(comment);
                    UserDialogs.Instance.Toast("Este comentário foi excluído", TimeSpan.FromSeconds(2));
                    AtualizarComentarios();

                }

            }

        }


        public void AtualizarComentarios()
        {
            Atualizando = true;

            Exibircomentario(Publication.Id);

            Atualizando = false;

        }

        public async void AdicionarComentario()
        {
            Comentarios.Id = Guid.NewGuid().ToString("N");
            Comentarios.CodUsuario = UserId;
            Comentarios.CodPublicacao = Publication.Id;
            Comentarios.Texto = Texto;

            await _clienteComment.AdicionarTable(Comentarios);

            Texto = string.Empty;

            AtualizarComentarios();


        }

        public async void ExcluirPublicacao()
        {
            var resulPublicacao = await _dialogService.DisplayAlertAsync("Excluir Publicação", "A exclusão desta publicação," +
                    " também irá excluir todos os comentários relacionados a está publicação. Deseja Continuar?", " Sim ", " Não ");

            if (resulPublicacao)
            {
                using (var Dialog = UserDialogs.Instance.Loading("Salvando...", null, null, true, MaskType.Black))
                {

                    foreach (var item in ComentariosPublication)
                    {
                        var comment = ConversaoComentario(item);
                        ExcluirComentario(comment);

                    }

                    await _clientePublication.RemoverTable(Publication);
                }
                UserDialogs.Instance.Toast("Está publicação e os seus comentários foram excluídos", TimeSpan.FromSeconds(2));
                await _navigationService.GoBackAsync();

            }
        }

        public bool CondicaoExcluirPublicacao()
        {
            return VerificarExcluirPublicaco;
        }

        public async void Verificacao(string idPublicacao, string idUsuario)
        {
            var resultado = await _clientePublication.MinhaPublicaco(idPublicacao, idUsuario);

            if (resultado != 0)
            {
                VerificarExcluirPublicaco = true;

            }
            else
            {
                VerificarExcluirPublicaco = false;

            }

        }

        public async void ExcluirComentario(Comentario args)
        {

            await _clienteComment.RemoverTable(args);

        }

        public async void Exibircomentario(string codPublicacao)
        {
            IsLoading = true;
            List<string> auxList = new List<string>();

            try
            {
                var resulComment = await _clienteComment.Comentarios(codPublicacao);

                if (resulComment.Count != 0)
                {
                    Message = string.Empty;

                    foreach (var item in resulComment)
                    {
                        if (!auxList.Contains(item.CodUsuario))
                        {
                            auxList.Add(item.CodUsuario);

                        }
                    }

                    var resulUser = await _clienteUser.Usuarios(auxList);

                    var resultado = resulComment.Join(resulUser, c => c.CodUsuario, u => u.Id,
                                                        (c, u) => new { c.Id, c.CodPublicacao, c.CodUsuario, c.Texto, u.Nome, u.Foto });

                    ComentariosPublication.Clear();
                    foreach (var item in resultado)
                    {
                        ComentariosPublication.Add(item);

                    }


                }
                else
                {
                    ComentariosPublication.Clear();

                    Message = "Está publicação ainda não possui nenhum comentário.";

                }

            }
            catch (MobileServiceInvalidOperationException)
            {
                Message = "Ocorreu algum problema, por favor tente novamente mais tarde.";

            }
            IsLoading = false;

        }

        public Comentario ConversaoComentario(object args)
        {
            var comment = new Comentario();

            var aux = Conversao(args, new { Id = "", CodPublicacao = "", CodUsuario = "", Texto = "", Nome = "", Foto = "" });

            comment.Id = aux.Id;
            comment.CodPublicacao = aux.CodPublicacao;
            comment.CodUsuario = aux.CodUsuario;
            comment.Texto = aux.Texto;

            return comment;



        }

        public T Conversao<T>(object objeto, T tipo)
        {
            return (T)objeto;

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                if (parameters.ContainsKey("publicacao"))
                {
                    Publication = (Publicacao)parameters["publicacao"];

                    if (parameters.ContainsKey("nome"))
                    {
                        Nome = (string)parameters["nome"]; ;

                    }

                    if (parameters.ContainsKey("foto"))
                    {
                        Foto = (string)parameters["foto"]; ;

                    }

                    Exibircomentario(Publication.Id);
                    Verificacao(Publication.Id, UserId);

                }
            }
        }
        #endregion
    }
}
