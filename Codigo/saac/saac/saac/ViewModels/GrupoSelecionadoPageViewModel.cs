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

        private ObservableCollection<object> _publicacoesGrupo;
        public ObservableCollection<object> PublicacoesGrupo
        {
            get { return _publicacoesGrupo; }
            set { SetProperty(ref _publicacoesGrupo, value); }

        }


        private readonly IAzureServiceUser<Usuario> _clienteUser;
        private readonly IAzureServicePublication<Publicacao> _clientePublication;

        private readonly INavigationService _navigationService;

        public DelegateCommand SalvarPublicacaoCommand { get; set; }

        private DelegateCommand<object> _publicacaoSelectedCommand;

        public DelegateCommand<object> PublicacaoSelectedCommand =>
            _publicacaoSelectedCommand != null ? _publicacaoSelectedCommand : (_publicacaoSelectedCommand = new DelegateCommand<object>(ItemTapped));


        public GrupoSelecionadoPageViewModel(INavigationService navigationService, IAzureServicePublication<Publicacao> clientePublication,
            IAzureServiceUser<Usuario> clienteUser) : base(navigationService)
        {
            _navigationService = navigationService;
            _clientePublication = clientePublication;
            _clienteUser = clienteUser;

            Publication = new Publicacao();
            Grupos = new Grupo();

            PublicacoesGrupo = new ObservableCollection<object>();

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
            List<string> auxList = new List<string>();

            try
            {
                var resulPublication = await _clientePublication.Publicacoes(codGrupo);

                if (resulPublication.Count != 0)
                {
                    foreach (var item in resulPublication)
                    {
                        if (!auxList.Contains(item.CodUsuario))
                        {
                            auxList.Add(item.CodUsuario);

                        }
                    }

                    var resulUser = await _clienteUser.Usuarios(auxList);
                        
                    var resulatdo = resulPublication.Join(resulUser, p => p.CodUsuario, u => u.Id,
                                                        (p, u) => new {p.Id, p.CodGrupo, p.CodUsuario, p.Texto, u.Nome });

                    PublicacoesGrupo.Clear();
                    foreach (var item in resulatdo)
                    {
                        PublicacoesGrupo.Add(item);

                    }

                }
                else
                {
                    Message = "Este grupo ainda não possui nehuma publicação";

                }
            }
            catch (MobileServiceInvalidOperationException)
            {
                Message = "Ocorreu algum problema, por favor tente mais tarde";

            }
            
        }

        public async void ItemTapped(object args)
        {
            var publica = new Publicacao();
            string nome;

            var aux = ConversaoAux(args);

            publica = (Publicacao)aux[0];
            nome = (string)aux[1];

            var navigationParams = new NavigationParameters();
            navigationParams.Add("publicacao", publica);
            navigationParams.Add("nome", nome);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("PublicacaoSelecionadaPage", navigationParams);

        }

        public object[] ConversaoAux(object args)
        {
            object[] vetor = new object[2];
            var publica = new Publicacao();
            string Nome;
            var aux = Conversao(args, new { Id = "", CodGrupo = "", CodUsuario = "", Texto = "", Nome = "" });

            publica.Id = aux.Id;
            publica.CodGrupo = aux.CodGrupo;
            publica.CodUsuario = aux.CodUsuario;
            publica.Texto = aux.Texto;
            Nome = aux.Nome;

            vetor[0] = publica;
            vetor[1] = Nome;

            return vetor;

        }

        public T Conversao<T>(object objeto, T tipo)
        {
            return (T)objeto;

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
