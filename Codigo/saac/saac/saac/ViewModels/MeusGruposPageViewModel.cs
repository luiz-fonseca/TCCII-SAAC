using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class MeusGruposPageViewModel : ViewModelBase
    {

        protected bool HasInitialized { get; set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }


        private readonly INavigationService _navigationService;
       

        public DelegateCommand AdicionarGrupoCommand { get; set; }

        public MeusGruposPageViewModel(INavigationService navigationService ):base(navigationService)
        {
            _navigationService = navigationService;
            AdicionarGrupoCommand = new DelegateCommand(AdicionarGrupo);
        }

        private async void AdicionarGrupo()
        {
            await _navigationService.NavigateAsync("AdicionarGrupoPage");
           
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (HasInitialized) return;
            HasInitialized = true;

            Message = parameters.GetValue<string>("message");
        }

    }
}
