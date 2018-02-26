using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace saac.ViewModels
{
	public class ConcursoSelecionadoPageViewModel : ViewModelBase
	{
        private bool _atualizando = false;
        public bool Atualizando
        {
            get { return _atualizando; }
            set { SetProperty(ref _atualizando, value); }
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

        private readonly IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> _clienteConcursoGrupo;
        private readonly IAzureServiceGroup<Grupo> _clienteGrupo;

        private DelegateCommand _adicionarGrupoCommand;
        public DelegateCommand AdicionarGrupoCommand =>
            _adicionarGrupoCommand ?? (_adicionarGrupoCommand = new DelegateCommand(AdicionarGrupo));

        private DelegateCommand _atualizarCommand;
        public DelegateCommand AtualizarCommand =>
            _atualizarCommand ?? (_atualizarCommand = new DelegateCommand(AtualizarGrupos));

        private DelegateCommand<Grupo> _grupoSelectedCommand;
        public DelegateCommand<Grupo> GrupoSelectedCommand =>
            _grupoSelectedCommand != null ? _grupoSelectedCommand : (_grupoSelectedCommand = new DelegateCommand<Grupo>(ItemTapped));


        public ConcursoSelecionadoPageViewModel(INavigationService navigationService, IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> clienteConcursoGrupo,
            IAzureServiceGroup<Grupo> clienteGrupo) : base(navigationService)
        {
            _navigationService = navigationService;

            _clienteConcursoGrupo = clienteConcursoGrupo;
            _clienteGrupo = clienteGrupo;

            Grupos = new ObservableCollection<Grupo>();
        }

        public void AtualizarGrupos()
        {
            Atualizando = true;

            GruposConcursos(Concursos.Id);

            Atualizando = false;
        }

        public async void AdicionarGrupo()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("concursoId", Concursos.Id);
            navigationParams.Add("userId", UserId);
            navigationParams.Add("temporario", true);

            await _navigationService.NavigateAsync("AdicionarGrupoPage", navigationParams, useModalNavigation: false);


        }

        public async void GruposConcursos(string codConcurso)
        {
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

        public async void ItemTapped(Grupo args)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("grupo", args);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("GrupoSelecionadoPage", navigationParams, useModalNavigation: false);

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                if (parameters.ContainsKey("concurso"))
                {
                    Concursos = (Concurso)parameters["concurso"];

                    GruposConcursos(Concursos.Id);

                }

            }
        }
    }
}
