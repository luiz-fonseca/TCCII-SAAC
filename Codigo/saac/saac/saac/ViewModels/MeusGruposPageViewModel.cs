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

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
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
            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("AdicionarGrupoPage", navigationParams);
           
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (HasInitialized) return;
            HasInitialized = true;

            UserId = parameters.GetValue<string>("userId");
        }

    }
}
