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
	public class NotificacoesPageViewModel : ViewModelBase
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

        protected bool HasInitialized { get; set; }

        public string UserId { get; set; }

        private ObservableCollection<Group<string, Concurso>> _concursosAgrupados;
        public ObservableCollection<Group<string, Concurso>> ConcursosAgrupados
        {
            get { return _concursosAgrupados; }
            set { SetProperty(ref _concursosAgrupados, value); }
        }

        private readonly INavigationService _navigationService;

        private readonly IAzureServicePrefUser<PreferenciaUser> _clientePrefUser;
        private readonly IAzureServicePrefConcurso<PreferenciaConcurso> _clientePrefConcurso;
        private readonly IAzureServiceConcurso<Concurso> _clienteConcurso;

        private DelegateCommand _atualizarCommand;
        public DelegateCommand AtualizarCommand =>
            _atualizarCommand ?? (_atualizarCommand = new DelegateCommand(Atualizar));

        private DelegateCommand<Concurso> _concursoSelectedCommand;
        public DelegateCommand<Concurso> ConcursoSelectedCommand =>
            _concursoSelectedCommand != null ? _concursoSelectedCommand : (_concursoSelectedCommand = new DelegateCommand<Concurso>(ItemTapped));

        #endregion

        #region Construtor
        public NotificacoesPageViewModel(INavigationService navigationService, IAzureServicePrefUser<PreferenciaUser> clientePrefUser,
            IAzureServicePrefConcurso<PreferenciaConcurso> clientePrefConcurso, IAzureServiceConcurso<Concurso> clienteConcurso) : base(navigationService)
        {
            _navigationService = navigationService;

            _clientePrefUser = clientePrefUser;
            _clientePrefConcurso = clientePrefConcurso;
            _clienteConcurso = clienteConcurso;

            ConcursosAgrupados = new ObservableCollection<Group<string, Concurso>>();

        }
        #endregion

        #region Métodos
        public void Atualizar()
        {
            Atualizando = true;

            MeusConcursosPreferidos(UserId);

            Atualizando = false;

        }

        public async void ItemTapped(Concurso obj)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("concurso", obj);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("ConcursoSelecionadoPage", navigationParams, useModalNavigation: false);
        }

        public async void MeusConcursosPreferidos(string codUsuario)
        {
            var resulPrefUser = await _clientePrefUser.MinhasPreferencias(codUsuario);
            var resulPrefConcurso = await _clientePrefConcurso.MeusConcursosPreferidos(resulPrefUser);

            if (resulPrefConcurso.Count != 0)
            {
                var listaConcurso = await _clienteConcurso.MeusConcursos(resulPrefConcurso);

                if (listaConcurso.Count != 0)
                {
                    var resulAgrupar = Agrupar(listaConcurso);
                    Converter(resulAgrupar);
                }
                else
                {
                    ConcursosAgrupados.Clear();
                    Mensagem = "Ainda não existe concursos baseado em suas preferências";

                }
                
            }
            else
            {
                ConcursosAgrupados.Clear();
                Mensagem = "Ainda não existe concursos baseado em suas preferências";

            }


        }

        public IEnumerable<Group<string, Concurso>> Agrupar(List<Concurso> concursos)
        {

            var resultado = from concurso in concursos
                            orderby concurso.Estado
                            group concurso by concurso.Estado into grupos
                            select new Group<string, Concurso>(grupos.Key, grupos);

            return resultado;


        }

        public void Converter(IEnumerable<Group<string, Concurso>> listaAgrupada)
        {
            ConcursosAgrupados.Clear();
            foreach (var item in listaAgrupada)
            {
                ConcursosAgrupados.Add(item);

            }

        }


        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (HasInitialized) return;
            HasInitialized = true;

            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                MeusConcursosPreferidos(UserId);

            }

        }
        #endregion
    }
}
