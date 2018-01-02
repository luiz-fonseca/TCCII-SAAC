using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class GrupoSelecionadoPageViewModel : ViewModelBase
    {
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

        private readonly IAzureServicePublication<Publicacao> _clientePublication;

        private readonly INavigationService _navigationService;

        public GrupoSelecionadoPageViewModel(INavigationService navigationService, IAzureServicePublication<Publicacao> clientePublication) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientePublication = clientePublication;

        }

        public async void AdicionarPublicacao()
        {
            Publication.CodPublicaco = Guid.NewGuid().ToString("N");
            Publication.CodPessoa = UserId;
            Publication.CodGrupo = Grupos.Id;

            await _clientePublication.AdicionarTable(Publication);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("grupo"))
            {
                Grupos = (Grupo)parameters["grupo"];
            }

            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"]; ;
            }
                

        }
    }
}
