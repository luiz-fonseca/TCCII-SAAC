using Microsoft.WindowsAzure.MobileServices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private ObservableCollection<Grupo> _meusGroups;
        public ObservableCollection<Grupo> MeusGroups
        {
            get { return _meusGroups; }
            set { SetProperty(ref _meusGroups, value); }
        }

        private readonly IAzureServiceAux<Auxiliar> _clienteAux;
        private readonly IAzureServiceGroup<Grupo> _clienteGroup;

        private readonly INavigationService _navigationService;
       
        public DelegateCommand AdicionarGrupoCommand { get; set; }


        private DelegateCommand<Grupo> _grupoSelectedCommand;

        public DelegateCommand<Grupo> GrupoSelectedCommand => 
            _grupoSelectedCommand != null ? _grupoSelectedCommand : (_grupoSelectedCommand = new DelegateCommand<Grupo>(ItemTapped));


        public MeusGruposPageViewModel(INavigationService navigationService, IAzureServiceAux<Auxiliar> clienteAux, 
            IAzureServiceGroup<Grupo> clienteGroup) :base(navigationService)
        {
            _navigationService = navigationService;
            _clienteAux = clienteAux;
            _clienteGroup = clienteGroup;

            MeusGroups = new ObservableCollection<Grupo>();

            AdicionarGrupoCommand = new DelegateCommand(AdicionarGrupo);

        }

        
        public async void ExibirMeusGrupos(string id)
        {
            try
            {
               var aux = await _clienteAux.MeusGrupos(id);
               var resultado = await _clienteGroup.MeusGrupos(aux);

                MeusGroups.Clear();
                foreach (var item in resultado)
                {
                    MeusGroups.Add(item);

                }
                
            }
            catch (MobileServiceInvalidOperationException)
            {
               Message = "Você ainda não possui Grupos. Crie um ou Entre em algum";

            }
            

        }

        public async void ItemTapped(Grupo args)
        {   
            var navigationParams = new NavigationParameters();
            navigationParams.Add("grupo", args);
            navigationParams.Add("userId", UserId);

            await  _navigationService.NavigateAsync("NavigationPage/GrupoSelecionadoPage", navigationParams);

        }

        public async void AdicionarGrupo()
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

            ExibirMeusGrupos(UserId);

        }

    }
}
