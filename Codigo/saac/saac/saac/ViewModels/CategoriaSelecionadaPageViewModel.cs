using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class CategoriaSelecionadaPageViewModel : ViewModelBase
	{
        private string _web;
        public string Web
        {
            get { return _web; }
            set { SetProperty(ref _web, value); }
        }

        private readonly INavigationService _navigationService;

        public CategoriaSelecionadaPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

        }

        public void exibir(string args)
        {
            if (args.Contains("Nacional"))
            {
                Web = "https://www.pciconcursos.com.br/concursos/nacional/";

            }
            else if (args.Contains("Nordeste"))
            {
                Web = "https://www.pciconcursos.com.br/concursos/nordeste/";

            }
            else if (args.Contains("Norte"))
            {
                Web = "https://www.pciconcursos.com.br/concursos/norte/";

            }
            else if (args.Contains("Sudeste"))
            {
                Web = "https://www.pciconcursos.com.br/concursos/sudeste/";

            }
            else if (args.Contains("Sul"))
            {
                Web = "https://www.pciconcursos.com.br/concursos/sul/";

            }
            else if (args.Contains("Centro-Oeste"))
            {
                Web = "https://www.pciconcursos.com.br/concursos/centrooeste/";

            }
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            var resultado  = parameters.GetValue<string>("categoria");   
            exibir(resultado);

        }
    }
}
