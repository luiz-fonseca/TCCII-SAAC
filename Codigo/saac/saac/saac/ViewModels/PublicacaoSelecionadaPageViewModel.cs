using Microsoft.WindowsAzure.MobileServices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
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

        private ObservableCollection<object> _comentariosPublication;
        public ObservableCollection<object> ComentariosPublication
        {
            get { return _comentariosPublication; }
            set { SetProperty(ref _comentariosPublication, value); }

        }

        public DelegateCommand SalvarComentarioCommand { get; set; }
        public DelegateCommand AtualizarCommand { get; set; }

        private readonly IAzureServiceUser<Usuario> _clienteUser;
        private readonly IAzureServiceComment<Comentario> _clienteComment;

        private readonly INavigationService _navigationService;

        public PublicacaoSelecionadaPageViewModel(INavigationService navigationService, IAzureServiceComment<Comentario> clienteComment,
                                            IAzureServiceUser<Usuario> clienteUser) : base(navigationService)
        {
            _navigationService = navigationService;

            _clienteComment = clienteComment;
            _clienteUser = clienteUser;

            Publication = new Publicacao();
            Comentarios = new Comentario();
   
            ComentariosPublication = new ObservableCollection<object>();

            SalvarComentarioCommand = new DelegateCommand(AdicionarComentario);
            AtualizarCommand = new DelegateCommand(AtualizarComentarios);

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

            await _clienteComment.AdicionarTable(Comentarios);


        }

        public async void Exibircomentario(string codPublicacao)
        {
            List<string> auxList = new List<string>();

            try
            {
                var resulComment = await _clienteComment.Comentarios(codPublicacao);

                if (resulComment.Count != 0)
                {
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
                    Message = "Esta publicacao ainda não possui nehum comentario";

                }

            }
            catch (MobileServiceInvalidOperationException)
            {
                Message = "Ocorreu algum problema, por favor tente mais tarde";

            }

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
