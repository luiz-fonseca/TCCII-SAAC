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
	public class CategoriaSelecionadaPageViewModel : ViewModelBase
	{
        #region Propriedades
        private bool _atualizando = false;
        public bool Atualizando
        {
            get { return _atualizando; }
            set { SetProperty(ref _atualizando, value); }
        }

        public string Regiao { get; set; }

        public string UserId { get; set; }

        private string _mensagem;
        public string Mensagem
        {
            get { return _mensagem; }
            set { SetProperty(ref _mensagem, value); }
        }

        private ObservableCollection<Group<string, Concurso>> _concursosAgrupados;
        public ObservableCollection<Group<string, Concurso>> ConcursosAgrupados
        {
            get { return _concursosAgrupados; }
            set { SetProperty(ref _concursosAgrupados, value); }
        }

        private readonly INavigationService _navigationService;

        private readonly IAzureServiceConcurso<Concurso> _clienteConcurso;

        private DelegateCommand _atualizarCommand;
        public DelegateCommand AtualizarCommand =>
            _atualizarCommand ?? (_atualizarCommand = new DelegateCommand(Atualizar));

        private DelegateCommand<Concurso> _concursoSelectedCommand;
        public DelegateCommand<Concurso> ConcursoSelectedCommand =>
            _concursoSelectedCommand != null ? _concursoSelectedCommand : (_concursoSelectedCommand = new DelegateCommand<Concurso>(ItemTapped));
        
        #endregion

        #region Construtor
        public CategoriaSelecionadaPageViewModel(INavigationService navigationService, IAzureServiceConcurso<Concurso> clienteConcurso) : base(navigationService)
        {
            _navigationService = navigationService;

            _clienteConcurso = clienteConcurso;

            ConcursosAgrupados = new ObservableCollection<Group<string, Concurso>>();

        }
        #endregion

        #region Métodos
        public void Atualizar()
        {
            Atualizando = true;

            ExibirConcursos(Regiao);

            Atualizando = false;
        }


        public void ExibirConcursos(string args)
        {
            ConcursosDisponiveis(args);

        }

        public async void ItemTapped(Concurso obj)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("concurso", obj);
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("ConcursoSelecionadoPage",navigationParams, useModalNavigation: false);
        }

        public async void ConcursosDisponiveis(string regiao)
        {
            var resultado = await _clienteConcurso.ConcursosDisponiveis(regiao);

            if (resultado.Count != 0)
            {
                var listaAgrupada = Agrupar(resultado);

                Converter(listaAgrupada);
            }
            else
            {
                Mensagem = "Nenhum Concurso Disponível";
            }

        }

        public IEnumerable<Group<string, Concurso>> Agrupar(List<Concurso> Concursos)
        {

            var resultado = from concurso in Concursos
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


        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                if (parameters.ContainsKey("categoria"))
                {
                    Regiao = (string)parameters["categoria"];

                    ExibirConcursos(Regiao);
                }

            }
        }
        #endregion
    }
}
