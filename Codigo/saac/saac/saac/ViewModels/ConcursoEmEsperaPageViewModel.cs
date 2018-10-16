using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
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
	public class ConcursoEmEsperaPageViewModel : ViewModelBase
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

        private DelegateCommand<Concurso> _concursoSelectedCommand;
        public DelegateCommand<Concurso> ConcursoSelectedCommand =>
            _concursoSelectedCommand != null ? _concursoSelectedCommand : (_concursoSelectedCommand = new DelegateCommand<Concurso>(ItemTapped));

        #endregion

        public ConcursoEmEsperaPageViewModel(INavigationService navigationService, IAzureServiceConcurso<Concurso> clienteConcurso) : base (navigationService)
        {
            _navigationService = navigationService;

            _clienteConcurso = clienteConcurso;

            ListaConcursos = new ObservableCollection<Concurso>();

        }

        #region Métodos
        public void AtualizarConcursos()
        {
            Atualizando = true;

            ExibirConcursosEmEspera();

            Atualizando = false;

        }

        public async void ExibirConcursosEmEspera()
        {
            IsLoading = true;

            await ConcursosEmEspera();

            IsLoading = false;
        }

        public async void ItemTapped(Concurso obj)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("concurso", obj);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("ConcursoSelecionadoPage", navigationParams, useModalNavigation: false);

        }

        public async Task ConcursosEmEspera()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var dataAtual = DateTime.Now.Date;
                    var lista = await _clienteConcurso.ConcursosEmEspera(dataAtual);

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

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                ExibirConcursosEmEspera();

            }

        }
        #endregion
    }
}
