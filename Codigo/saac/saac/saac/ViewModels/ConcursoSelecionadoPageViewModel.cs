using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class ConcursoSelecionadoPageViewModel : ViewModelBase
	{
        private readonly INavigationService _navigationService;

        public ConcursoSelecionadoPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                //UserId = (string)parameters["userId"];

                if (parameters.ContainsKey("concurso"))
                {
                    //Regiao = (string)parameters["concurso"];

                }

            }
        }
    }
}
