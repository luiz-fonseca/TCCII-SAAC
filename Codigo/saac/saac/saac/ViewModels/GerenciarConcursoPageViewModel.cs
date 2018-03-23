using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace saac.ViewModels
{
	public class GerenciarConcursoPageViewModel : ViewModelBase
	{
        #region Propriedades
        public string UserId { get; set; }

        private ObservableCollection<object> _gerenciarConcursos;
        public ObservableCollection<object> GerenciarConcursos
        {
            get { return _gerenciarConcursos; }
            set { SetProperty(ref _gerenciarConcursos, value); }
        }

        private readonly INavigationService _navigationService;

        private DelegateCommand<object> _opcaoSelectedCommand;
        public DelegateCommand<object> OpcaoSelectedCommand =>
            _opcaoSelectedCommand != null ? _opcaoSelectedCommand : (_opcaoSelectedCommand = new DelegateCommand<object>(ItemTapped));
        #endregion

        #region Construtor
        public GerenciarConcursoPageViewModel(INavigationService navigationService) : base (navigationService)
        {
            _navigationService = navigationService;

            GerenciarConcursos = new ObservableCollection<object>();

            ExibirOpcoes();

        }
        #endregion

        #region Métodos
        private async void ItemTapped(object obj)
        {
            var aux = ConversaoCategoria(obj);
            var resultado = OpcaoSelecionada(aux);

            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", UserId);
            navigationParams.Add(resultado[1], resultado[2]);

            await _navigationService.NavigateAsync(resultado[0], navigationParams, useModalNavigation: false);

        }

        public string[] OpcaoSelecionada(string obj)
        {
            string[] resultado = new string[3];
            
            switch (obj)
            {
                case "Novo Concurso":
                    resultado[0] = "AdicionarConcursoPage";
                    resultado[1] = "proximo";
                    resultado[2] = "Próximo";
                    return resultado;

                case "Inscrições Finalizadas":
                    resultado[0] = "ConcursoInscricoesFinalizadasPage";
                    resultado[1] = "";
                    resultado[2] = "";
                    return resultado;

                case "Concursos em Espera":
                    resultado[0] = "ConcursoEmEsperaPage";
                    resultado[1] = "";
                    resultado[2] = "";
                    return resultado;

                case "Concursos Finalizados":
                    resultado[0] = "ConcursoFinalizadosPage";
                    resultado[1] = "";
                    resultado[2] = "";
                    return resultado;

                default:
                    return resultado;
                    
            }

        }

        public void ExibirOpcoes()
        {
            GerenciarConcursos.Add(new { Nome= "Novo Concurso" });
            GerenciarConcursos.Add(new { Nome = "Inscrições Finalizadas" });
            GerenciarConcursos.Add(new { Nome = "Concursos em Espera"});
            GerenciarConcursos.Add(new { Nome = "Concursos Finalizados" });

        }

        public string ConversaoCategoria(object args)
        {
            var aux = Conversao(args, new { Nome = "" });

            var resutado = aux.Nome;

            return resutado;

        }

        public T Conversao<T>(object objeto, T tipo)
        {
            return (T)objeto;

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

            }

        }

        #endregion
    }
}
