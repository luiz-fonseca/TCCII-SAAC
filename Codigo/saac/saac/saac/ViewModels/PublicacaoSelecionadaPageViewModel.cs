using Microsoft.WindowsAzure.MobileServices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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

        private ObservableCollection<object> _comentariosPublication;
        public ObservableCollection<object> ComentariosPublication
        {
            get { return _comentariosPublication; }
            set { SetProperty(ref _comentariosPublication, value); }

        }

        private DelegateCommand _salvarComentarioCommand;
        public DelegateCommand SalvarComentarioCommand =>
            _salvarComentarioCommand ?? (_salvarComentarioCommand = new DelegateCommand(AdicionarComentario, CondicaoAdicionarComentario));

        private DelegateCommand _atualizarCommand;
        public DelegateCommand AtualizarCommand =>
            _atualizarCommand ?? (_atualizarCommand = new DelegateCommand(AtualizarComentarios));

        private DelegateCommand _excluirPublicacaoCommand;
        public DelegateCommand ExcluirPublicacaoCommand =>
            _excluirPublicacaoCommand ?? (_excluirPublicacaoCommand = new DelegateCommand(ExcluirPublicacao));


        private DelegateCommand<object> _comentarioSelectedCommand;
        public DelegateCommand<object> ComentarioSelectedCommand =>
            _comentarioSelectedCommand != null ? _comentarioSelectedCommand : (_comentarioSelectedCommand = new DelegateCommand<object>(ItemTapped));


        private readonly IAzureServiceUser<Usuario> _clienteUser;
        private readonly IAzureServicePublication<Publicacao> _clientePublication;
        private readonly IAzureServiceComment<Comentario> _clienteComment;

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        #endregion

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

        private bool CondicaoAdicionarComentario()
        {
            return !string.IsNullOrWhiteSpace(Texto);
        }

        public async void ItemTapped(object args)
        {
            var comment = ConversaoComentario(args);

            var auxResul = await _clienteComment.MeuCometario(comment.Id, UserId);
            if (auxResul != 0)
            {
                var resulComment = await _dialogService.DisplayAlertAsync("Apagar Comentário", "Deseja excluir este cometario?", " Sim ", " Não ");

                if (resulComment)
                {
                    ExcluirComentario(comment);

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
            var aux = await _clientePublication.MinhaPublicaco(Publication.Id, UserId);
            if (aux != 0)
            {
                var resulPublicacao = await _dialogService.DisplayAlertAsync("Excluir Publicação?", "Se você excluir está publicação," +
                    " todos os comentários relacionados também serão excluidos. Deseja Continuar?", " Sim ", " Não ");

                if (resulPublicacao)
                {
                    foreach (var item in ComentariosPublication)
                    {
                        var comment = ConversaoComentario(item);
                        ExcluirComentario(comment);

                    }

                    await _clientePublication.RemoverTable(Publication);

                    await _navigationService.GoBackAsync();

                }
            }

        }

        public async void ExcluirComentario(Comentario args)
        {

            await _clienteComment.RemoverTable(args);

        }

        public async void Exibircomentario(string codPublicacao)
        {
            List<string> auxList = new List<string>();

            try
            {
                var resulComment = await _clienteComment.Comentarios(codPublicacao);

                if (resulComment.Count != 0)
                {
                    Message = "";

                    foreach (var item in resulComment)
                    {
                        if (!auxList.Contains(item.CodUsuario))
                        {
                            auxList.Add(item.CodUsuario);

                        }
                    }

                    var resulUser = await _clienteUser.Usuarios(auxList);

                    var resultado = resulComment.Join(resulUser, c => c.CodUsuario, u => u.Id,
                                                        (c, u) => new { c.Id, c.CodPublicacao, c.CodUsuario, c.Texto, u.Nome });

                    ComentariosPublication.Clear();
                    foreach (var item in resultado)
                    {
                        ComentariosPublication.Add(item);

                    }


                }
                else
                {
                    ComentariosPublication.Clear();
                    Message = "Esta publicacao ainda não possui nehum comentario";

                }

            }
            catch (MobileServiceInvalidOperationException)
            {
                Message = "Ocorreu algum problema, por favor tente mais tarde";

            }

        }

        public Comentario ConversaoComentario(object args)
        {
            var comment = new Comentario();

            var aux = Conversao(args, new { Id = "", CodPublicacao = "", CodUsuario = "", Texto = "", Nome = "" });

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
            if (parameters.ContainsKey("publicacao"))
            {
                Publication = (Publicacao)parameters["publicacao"];

            }

            if (parameters.ContainsKey("nome"))
            {
                Nome = (string)parameters["nome"]; ;

            }

            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"]; ;

            }

            Exibircomentario(Publication.Id);
        }

    }
}
