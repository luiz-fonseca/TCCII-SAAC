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

namespace saac.ViewModels
{
    public class ConcursoInscricoesFinalizadasPageViewModel : ViewModelBase
    {
        #region Propriedades
        private bool _atualizando = false;
        public bool Atualizando
        {
            get { return _atualizando; }
            set { SetProperty(ref _atualizando, value); }
        }

        private string _mensagem;
        public string Mensagem
        {
            get { return _mensagem; }
            set { SetProperty(ref _mensagem, value); }
        }

        public string UserId { get; set; }

        private ObservableCollection<Concurso> _listaConcursos;
        public ObservableCollection<Concurso> ListaConcursos
        {
            get { return _listaConcursos; }
            set { SetProperty(ref _listaConcursos, value); }
        }

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private readonly IAzureServiceConcurso<Concurso> _clienteConcurso;

        private DelegateCommand _alterarCommand;
        public DelegateCommand AlterarCommand =>
            _alterarCommand ?? (_alterarCommand = new DelegateCommand(Alterar));

        private DelegateCommand<Concurso> _concursoSelectedCommand;
        public DelegateCommand<Concurso> ConcursoSelectedCommand =>
            _concursoSelectedCommand != null ? _concursoSelectedCommand : (_concursoSelectedCommand = new DelegateCommand<Concurso>(ItemTapped));


        #endregion

        #region Construtor
        public ConcursoInscricoesFinalizadasPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IAzureServiceConcurso<Concurso> clienteConcurso) : base(navigationService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;

            _clienteConcurso = clienteConcurso;

            ListaConcursos = new ObservableCollection<Concurso>();

        }
        #endregion

        #region Métodos
        public void AtualizarConcursos()
        {
            Atualizando = true;

            InscricoesFinalizadas();

            Atualizando = false;

        }

        public async void ItemTapped(Concurso obj)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("concurso", obj);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("ConcursoSelecionadoPage", navigationParams, useModalNavigation: false);

        }

        public async void InscricoesFinalizadas()
        {
            var dataAtual = DateTime.Now.Date;
            var lista = await _clienteConcurso.InscricoesFinalizadas(dataAtual);

            if (lista.Count != 0)
            {
                ListaConcursos.Clear();
                foreach (var item in lista)
                {
                    ListaConcursos.Add(item);

                }
            }
            else
            {
                Mensagem = "0";

            }

        }

        public async void Alterar()
        {
            foreach (var item in ListaConcursos)
            {
                item.Visibilidade = false;
                await _clienteConcurso.AtualizarTable(item);
            }

            await _dialogService.DisplayAlertAsync("Alterados", "Os concursos foram alterados", "Ok");
            await _navigationService.GoBackAsync();
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                InscricoesFinalizadas();

            }

        }
        #endregion
    }
}
