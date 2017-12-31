using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class GrupoSelecionadoPageViewModel : ViewModelBase
    {
        private Grupo _grupos;

        public Grupo Grupos
        {
            get { return _grupos; }
            set { SetProperty(ref _grupos, value); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }


        private readonly INavigationService _navigationService;

        public GrupoSelecionadoPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("grupo"))

                Grupos = (Grupo)parameters["grupo"];

        }
    }
}
