﻿using Acr.UserDialogs;
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
    public class ConcursoInscricoesFinalizadasPageViewModel : ViewModelBase
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

        private readonly IAzureServiceConcurso<Concurso> _clienteConcurso;

        private DelegateCommand _alterarCommand;
        public DelegateCommand AlterarCommand =>
            _alterarCommand ?? (_alterarCommand = new DelegateCommand(Alterar));

        private DelegateCommand<Concurso> _concursoSelectedCommand;
        public DelegateCommand<Concurso> ConcursoSelectedCommand =>
            _concursoSelectedCommand != null ? _concursoSelectedCommand : (_concursoSelectedCommand = new DelegateCommand<Concurso>(ItemTapped));


        #endregion

        #region Construtor
        public ConcursoInscricoesFinalizadasPageViewModel(INavigationService navigationService, IAzureServiceConcurso<Concurso> clienteConcurso) : base(navigationService)
        {
            _navigationService = navigationService;
           
            _clienteConcurso = clienteConcurso;

            ListaConcursos = new ObservableCollection<Concurso>();

        }
        #endregion

        #region Métodos
        public void AtualizarConcursos()
        {
            Atualizando = true;

            ExibirInscricoesFinalizadas();

            Atualizando = false;

        }

        public async void ExibirInscricoesFinalizadas()
        {
            IsLoading = true;

            await InscricoesFinalizadas();

            IsLoading = false;

        }

        public async void ItemTapped(Concurso obj)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("concurso", obj);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("ConcursoSelecionadoPage", navigationParams, useModalNavigation: false);

        }

        public async Task InscricoesFinalizadas()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
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
                        Mensagem = "Não contém nenhum concurso";

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

        public async void Alterar()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    using (var Dialog = UserDialogs.Instance.Loading("Alterando...", null, null, true, MaskType.Black))
                    {
                        foreach (var item in ListaConcursos)
                        {
                            item.Visibilidade = false;
                            await _clienteConcurso.AtualizarTable(item);
                        }
                    }
                    UserDialogs.Instance.Toast("As inscrições destes concursos foram finalizadas", TimeSpan.FromSeconds(2));
                    await _navigationService.GoBackAsync();

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

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                ExibirInscricoesFinalizadas();

            }

        }
        #endregion
    }
}
