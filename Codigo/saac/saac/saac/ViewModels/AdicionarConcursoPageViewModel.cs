using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class AdicionarConcursoPageViewModel : ViewModelBase
	{
        private Concurso _concursos;
        public Concurso Concursos
        {
            get { return _concursos; }
            set { SetProperty(ref _concursos, value); }
        }

        private DelegateCommand _proximoCommand;
        public DelegateCommand ProximoCommand =>
            _proximoCommand ?? (_proximoCommand = new DelegateCommand(Proximo, CondicaoProximo))
            .ObservesProperty(() => Concursos.Titulo).ObservesProperty(() => Concursos.Descricao).ObservesProperty(() => Concursos.Detalhes);

        private readonly INavigationService _navigationService;
        
        public AdicionarConcursoPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            Concursos = new Concurso();
        }

        private async void Proximo()
        {
            Concursos.Id = Guid.NewGuid().ToString("N");

            var navigationParams = new NavigationParameters();
            navigationParams.Add("Concursos", Concursos);
            await _navigationService.NavigateAsync("AdicionarPrefConcursoPage", navigationParams);

        }

        private bool CondicaoProximo()
        {
            return !string.IsNullOrWhiteSpace(Concursos.Titulo) &&
                !string.IsNullOrWhiteSpace(Concursos.Descricao) &&
                !string.IsNullOrWhiteSpace(Concursos.Detalhes);
        }
    }
}
