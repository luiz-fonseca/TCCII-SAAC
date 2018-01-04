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

        private ObservableCollection<Publicacao> _publicacoesGrupo;
        public ObservableCollection<Publicacao> PublicacoesGrupo
        {
            get { return _publicacoesGrupo; }
            set { SetProperty(ref _publicacoesGrupo, value); }

        }

        private readonly IAzureServicePublication<Publicacao> _clientePublication;

        private readonly INavigationService _navigationService;

        public DelegateCommand SalvarPublicacaoCommand { get; set; }

        private DelegateCommand<Publicacao> _publicacaoSelectedCommand;

        public DelegateCommand<Publicacao> PublicacaoSelectedCommand =>
            _publicacaoSelectedCommand != null ? _publicacaoSelectedCommand : (_publicacaoSelectedCommand = new DelegateCommand<Publicacao>(ItemTapped));


        public GrupoSelecionadoPageViewModel(INavigationService navigationService, IAzureServicePublication<Publicacao> clientePublication) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientePublication = clientePublication;

            Publication = new Publicacao();
            Grupos = new Grupo();

            PublicacoesGrupo = new ObservableCollection<Publicacao>();

            SalvarPublicacaoCommand = new DelegateCommand(AdicionarPublicacao);

        }

        public async void AdicionarPublicacao()
        {
            Publication.Id = Guid.NewGuid().ToString("N");
            Publication.CodUsuario = UserId;
            Publication.CodGrupo = Grupos.Id;

            await _clientePublication.AdicionarTable(Publication);

        }

        public async void ExibirPublicacoes(string codGrupo)
        {

            try
            {
                var resultado = await _clientePublication.Publicacoes(codGrupo);

                if (resultado.Count == 0)
                {
                    Message = "Este grupo ainda não possui nehuma publicação";
                }
                else
                {
                    PublicacoesGrupo.Clear();
                    foreach (var item in resultado)
                    {
                        PublicacoesGrupo.Add(item);

                    }
                }
            }
            catch(MobileServiceInvalidOperationException)
            {
                Message = "Ocorreu algum problema, por favor tente mais tarde";

            }

        }

        public async void ItemTapped(Publicacao args)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("publicacao", args);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("PublicacaoSelecionadaPage", navigationParams);

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

            ExibirPublicacoes(Grupos.Id);
        }
    }
}
