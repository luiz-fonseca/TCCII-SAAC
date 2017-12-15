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
    }
}
