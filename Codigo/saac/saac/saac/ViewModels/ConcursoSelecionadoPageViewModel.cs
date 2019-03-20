using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace saac.ViewModels
{
	public class ConcursoSelecionadoPageViewModel : ViewModelBase
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

        private bool _verificadorAdm = false;
        public bool VerificadorAdm
        {
            get { return _verificadorAdm; }
            set { SetProperty(ref _verificadorAdm, value); }
        }

        public bool VerificacaoRealizada { get; set; }

        private string _mensagem;
        public string Mensagem
        {
            get { return _mensagem; }
            set { SetProperty(ref _mensagem, value); }
        }

        private Concurso _concurso;
        public Concurso Concursos
        {
            get { return _concurso; }
            set { SetProperty(ref _concurso, value); }
        }

        private ObservableCollection<Grupo> _grupos;
        public ObservableCollection<Grupo> Grupos
        {
            get { return _grupos; }
            set { SetProperty(ref _grupos, value); }
        }

        public string UserId { get; set; }

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private readonly IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> _clienteConcursoGrupo;
        private readonly IAzureServiceGroup<Grupo> _clienteGrupo;
        private readonly IAzureServiceUser<Usuario> _clienteUser;
        private readonly IAzureServicePublication<Publicacao> _clientePublicacao;
        private readonly IAzureServiceComment<Comentario> _clienteComentario;
        private readonly IAzureServiceAux<Auxiliar> _clienteAuxiliar;
        private readonly IAzureServicePrefConcurso<PreferenciaConcurso> _clientePreferencia;
        private readonly IAzureServiceConcurso<Concurso> _clienteConcurso;

        private DelegateCommand _editarPreferenciaConcurso;
        public DelegateCommand EditarPreferenciaConcursoCommand =>
            _editarPreferenciaConcurso ?? (_editarPreferenciaConcurso = new DelegateCommand(EditarPreferenciaConcurso, CondicaoAdministrador)).ObservesProperty(() => VerificadorAdm);

        private DelegateCommand _editarConcurso;
        public DelegateCommand EditarConcursoCommand =>
            _editarConcurso ?? (_editarConcurso = new DelegateCommand(EditarConcurso, CondicaoAdministrador)).ObservesProperty(() => VerificadorAdm);

        private DelegateCommand _excluirConcurso;
        public DelegateCommand ExcluirConcursoCommand =>
            _excluirConcurso ?? (_excluirConcurso = new DelegateCommand(ExcluirConcurso, CondicaoAdministrador)).ObservesProperty(() => VerificadorAdm);

        private DelegateCommand _adicionarGrupoCommand;
        public DelegateCommand AdicionarGrupoCommand =>
            _adicionarGrupoCommand ?? (_adicionarGrupoCommand = new DelegateCommand(AdicionarGrupo));

        private DelegateCommand _atualizarCommand;
        public DelegateCommand AtualizarCommand =>
            _atualizarCommand ?? (_atualizarCommand = new DelegateCommand(AtualizarGrupos));

        private DelegateCommand<Grupo> _grupoSelectedCommand;
        public DelegateCommand<Grupo> GrupoSelectedCommand =>
            _grupoSelectedCommand != null ? _grupoSelectedCommand : (_grupoSelectedCommand = new DelegateCommand<Grupo>(ItemTapped));
        #endregion

        #region Construtor
        public ConcursoSelecionadoPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> clienteConcursoGrupo,
            IAzureServiceGroup<Grupo> clienteGrupo, IAzureServiceUser<Usuario> clienteUser, IAzureServicePublication<Publicacao> clientePublicacao,
            IAzureServiceComment<Comentario> clienteComentario, IAzureServiceAux<Auxiliar> clienteAuxiliar, IAzureServicePrefConcurso<PreferenciaConcurso> clientePreferencia,
            IAzureServiceConcurso<Concurso> clienteConcurso) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            _clienteConcursoGrupo = clienteConcursoGrupo;
            _clienteGrupo = clienteGrupo;
            _clienteUser = clienteUser;
            _clientePublicacao = clientePublicacao;
            _clienteComentario = clienteComentario;
            _clienteAuxiliar = clienteAuxiliar;
            _clientePreferencia = clientePreferencia;
            _clienteConcurso = clienteConcurso;

            Grupos = new ObservableCollection<Grupo>();
        }
        #endregion

        #region Métodos
        public void AtualizarGrupos()
        {
            Atualizando = true;

            ExibirGruposConcursos(Concursos.Id);

            if (VerificacaoRealizada == false)
            {
                Verificacao(UserId);

            }
            Atualizando = false;
            
        }

        public async void ExibirGruposConcursos(string codConcurso)
        {
            IsLoading = true;

            await GruposConcursos(codConcurso);

            IsLoading = false;

        }

        public async void AdicionarGrupo()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("adicionar", "adicionar");
            navigationParams.Add("concursoId", Concursos.Id);
            navigationParams.Add("userId", UserId);
            navigationParams.Add("temporario", true);

            await _navigationService.NavigateAsync("AdicionarGrupoPage", navigationParams, useModalNavigation: false);

        }

        public async Task GruposConcursos(string codConcurso)
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    Mensagem = string.Empty;

                    var resultado = await _clienteConcursoGrupo.GruposConcursos(codConcurso);

                    if (resultado.Count != 0)
                    {
                        var resulGrupos = await _clienteGrupo.MeusGrupos(resultado);

                        Grupos.Clear();
                        foreach (var item in resulGrupos)
                        {
                            Grupos.Add(item);

                        }
                    }
                }
                else
                {
                    Mensagem = "Você está sem conexão";

                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }
        }

        private async void EditarConcurso()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("Concursos", Concursos);
            navigationParams.Add("editar", "Editar");

            await _navigationService.NavigateAsync("AdicionarConcursoPage", navigationParams, useModalNavigation: false);

        }

        private async void ExcluirConcurso()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var resultado = await _dialogService.DisplayAlertAsync("Excluir Concurso", "Deseja realmente remover este concurso?", "Sim", "Não");

                    if (resultado)
                    {
                        using (var Dialog = UserDialogs.Instance.Loading("Excluindo...", null, null, true, MaskType.Black))
                        {
                            await RemoverGrupo();

                            var Preferencia = await _clientePreferencia.ConcursoPreferencia(Concursos.Id);

                            var ListaAuxConcurso = await _clienteConcursoGrupo.ListaGruposConcursos(Concursos.Id);
                            foreach (var itemListaAuxConcurso in ListaAuxConcurso)
                            {
                                await _clienteConcursoGrupo.RemoverTable(itemListaAuxConcurso);

                            }

                            await _clientePreferencia.RemoverTable(Preferencia);
                            await _clienteConcurso.RemoverTable(Concursos);

                        }

                        UserDialogs.Instance.Toast("Este concurso foi excluído", TimeSpan.FromSeconds(2));
                        await _navigationService.GoBackAsync();
                    }
                    else
                    {
                        UserDialogs.Instance.Toast("Você cancelou está opção", TimeSpan.FromSeconds(2));

                    }

                }
                else
                {
                    UserDialogs.Instance.Toast("Você está sem conexão", TimeSpan.FromSeconds(2));

                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }
        }

        private async void EditarPreferenciaConcurso()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("Concursos", Concursos);
            navigationParams.Add("editar", "editar");

            await _navigationService.NavigateAsync("AdicionarPrefConcursoPage", navigationParams, useModalNavigation: false);

        }


        public async Task RemoverGrupo()
        {
            foreach (var itemGrupo in Grupos)
            {
                var Publicacoes = await _clientePublicacao.Publicacoes(itemGrupo.Id);

                foreach (var itemPublicaco in Publicacoes)
                {
                    var Comentarios = await _clienteComentario.Comentarios(itemPublicaco.Id);

                    foreach (var itemComentario in Comentarios)
                    {
                        await _clienteComentario.RemoverTable(itemComentario);

                    }

                    await _clientePublicacao.RemoverTable(itemPublicaco);
                }

                var Aux = await _clienteAuxiliar.SeguidoresGrupo(itemGrupo.Id);

                foreach (var itemAux in Aux)
                {
                    await _clienteAuxiliar.RemoverTable(itemAux);

                }

                await _clienteGrupo.RemoverTable(itemGrupo);

            }
        }



        private bool CondicaoAdministrador()
        {
            return VerificadorAdm;

        }

        public async void Verificacao(string id)
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    VerificadorAdm = await _clienteUser.VerificarAdministrador(id);

                    VerificacaoRealizada = true;

                }
                else
                {
                    VerificacaoRealizada = false;

                }
            }
            catch (Exception)
            {
                VerificacaoRealizada = false;

            }
        }


        public async void ItemTapped(Grupo args)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("grupo", args);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("GrupoSelecionadoPage", navigationParams, useModalNavigation: false);

        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                if (parameters.ContainsKey("concurso"))
                {
                    Concursos = (Concurso)parameters["concurso"];

                    ExibirGruposConcursos(Concursos.Id);

                    Verificacao(UserId);

                }

            }
        }
        #endregion
    }
}
