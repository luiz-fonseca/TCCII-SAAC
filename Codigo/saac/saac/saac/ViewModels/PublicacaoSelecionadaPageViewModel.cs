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
        private Publicacao _publication;
        public Publicacao Publication
        {
            get { return _publication; }
            set { SetProperty(ref _publication, value); }
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

        private ObservableCollection<Comentario> _comentariosPublication;
        public ObservableCollection<Comentario> ComentariosPublication
        {
            get { return _comentariosPublication; }
            set { SetProperty(ref _comentariosPublication, value); }

        }

        public DelegateCommand SalvarComentarioCommand { get; set; }

        private readonly IAzureServiceComment<Comentario> _clienteComment;

        private readonly INavigationService _navigationService;

        public PublicacaoSelecionadaPageViewModel(INavigationService navigationService, IAzureServiceComment<Comentario> clienteComment) : base(navigationService)
        {
            _navigationService = navigationService;

            _clienteComment = clienteComment;

            Publication = new Publicacao();
            Comentarios = new Comentario();

            ComentariosPublication = new ObservableCollection<Comentario>();

            SalvarComentarioCommand = new DelegateCommand(AdicionarComentario);

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
            try
            {
                var resultado = await _clienteComment.Comentarios(codPublicacao);

                if (resultado.Count == 0)
                {
                    Message = "Esta publicacao ainda não possui nehum comentario";
                }
                else
                {
                    ComentariosPublication.Clear();
                    foreach (var item in resultado)
                    {
                        ComentariosPublication.Add(item);

                    }
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

            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"]; ;
            }

            Exibircomentario(Publication.Id);
        }

    }
}
