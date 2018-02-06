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
            set
            {
                SetProperty(ref _texto, value);
                SalvarPublicacaoCommand.RaiseCanExecuteChanged();
            }
        }


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

        private DelegateCommand _salvarPublicacaoCommand;
        public DelegateCommand SalvarPublicacaoCommand =>
            _salvarPublicacaoCommand ?? (_salvarPublicacaoCommand = new DelegateCommand(AdicionarPublicacao, CondicaoAdicionarPublicacao));

        private DelegateCommand _seguirGrupoCommand;
        public DelegateCommand SeguirGrupoCommand =>
            _seguirGrupoCommand ?? (_seguirGrupoCommand = new DelegateCommand(SeguirGrupo));


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

        public void AtualizarPublicacoes()
        {
            Atualizando = true;

            ExibirPublicacoes(Grupos.Id);

            Atualizando = false;

        }

        public async void SeguirGrupo()
        {
            var resultado = await _clienteAuxiliar.ExisteSeguirAux(Grupos.Id, UserId);

            if (resultado == 0)
            {
                Aux.CodGrupo = Grupos.Id;
                Aux.CodUsuario = UserId;
                Aux.Adiministrador = false;

                await _clienteAuxiliar.AdicionarTable(Aux);

                await _dialogService.DisplayAlertAsync("Seguindo Grupo", "Parabéns!! você agora " +
                   " está seguindo este grupo.", "OK");

            }
            else
            {
                var resulSeguir = await _dialogService.DisplayAlertAsync("Seguindo Grupo", "Deseja deixar de seguir este grupo?", " Sim ", " Não ");
                if (resulSeguir)
                {
                    var quantidade = await _clienteAuxiliar.QuantidadeRegistros(Grupos.Id);
                    if (quantidade == 1)
                    {
                        var resulGrupo = await _dialogService.DisplayAlertAsync("Excluir Grupo", "Você é a última pessoa a seguir este grupo," +
                            " se você deixar de segui-lo, este grupo e todas as suas publicações serão excluidas. Deseja Continuar?", " Sim ", " Não ");

                        if (resulGrupo)
                        {
                            var resultadoAux = await _clienteAuxiliar.GetAuxiliar(Grupos.Id, UserId);

                            await RemoverGrupo();

                            await _clienteAuxiliar.RemoverTable(resultadoAux);

                            await _dialogService.DisplayAlertAsync("Grupo", "Este grupo e suas publicações foram excluídos", "OK");

                            await _navigationService.GoBackAsync();

                        }

                    }
                    else
                    {
                        var resultadoAux = await _clienteAuxiliar.GetAuxiliar(Grupos.Id, UserId);

                        await _clienteAuxiliar.RemoverTable(resultadoAux);

                        await _dialogService.DisplayAlertAsync("Seguindo Grupo", "Você deixou de seguir este grupo.", "OK");

                    }

                }
            }

        }

        public async void AdicionarPublicacao()
        {
            Publication.Id = Guid.NewGuid().ToString("N");
            Publication.CodUsuario = UserId;
            Publication.CodGrupo = Grupos.Id;
            Publication.Texto = Texto;

            await _clientePublication.AdicionarTable(Publication);

            Texto = string.Empty;

            AtualizarPublicacoes();

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

        public async void ExibirPublicacoes(string codGrupo)
        {
            List<string> auxList = new List<string>();

            try
            {
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
                                                        (p, u) => new { p.Id, p.CodGrupo, p.CodUsuario, p.Texto, u.Nome });

                    PublicacoesGrupo.Clear();
                    foreach (var item in resulatdo)
                    {
                        PublicacoesGrupo.Add(item);

                    }

                }
                else
                {
                    PublicacoesGrupo.Clear();
                    Message = "Este grupo ainda não possui nehuma publicação.";

                }
            }
            catch (MobileServiceInvalidOperationException)
            {
                Message = "Ocorreu algum problema, por favor tente novamente mais tarde.";

            }

        }

        public async void ItemTapped(object args)
        {
            var aux = ConversaoAux(args);

            var publica = (Publicacao)aux[0];
            var nome = (string)aux[1];

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
        #endregion
    }
}
