using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class AdicionarGrupoPageViewModel : ViewModelBase
	{
        private readonly IAzureServiceBase<Grupo> _cliente;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        public Grupo _grupos;
       
        public Grupo Grupos
        {
            get { return _grupos; }
            set { SetProperty(ref _grupos, value); }
        }


        public DelegateCommand SalvarGrupoCommand { get; set; }

        public AdicionarGrupoPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            IAzureServiceBase<Grupo> cliente
            ):base(navigationService)
        {
            _cliente = cliente;
            _grupos = new Grupo();

            _navigationService = navigationService;
            _dialogService = dialogService;

            SalvarGrupoCommand = new DelegateCommand(SalvarGrupo);
        }

        private async void SalvarGrupo()
        {
           // await _cliente.AdicionarTable(_grupos);

            await _dialogService.DisplayAlertAsync("Grupo Cadastrado", "Parabéns!! O cadastro" +
                "do seu grupo foi realizado.", "OK");
            await _navigationService.GoBackAsync();
        }
    }
}
