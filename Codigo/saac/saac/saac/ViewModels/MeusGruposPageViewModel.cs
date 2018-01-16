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
using System.Threading.Tasks;

namespace saac.ViewModels
{
	public class MeusGruposPageViewModel : ViewModelBase
    {
        protected bool HasInitialized { get; set; }

        private bool _atualizando = false;
        public bool Atualizando
        {
            get { return _atualizando; }
            set { SetProperty(ref _atualizando, value); }
        }


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
        public DelegateCommand PesquisarGrupoCommand { get; set; }
        public DelegateCommand AtualizarCommand { get; set; }


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
            PesquisarGrupoCommand = new DelegateCommand(PesquisarGrupo);
            AtualizarCommand = new DelegateCommand(AtualizarGrupos);

        }

        public void AtualizarGrupos()
        {
            Atualizando = true;

            ExibirMeusGrupos(UserId);

            Atualizando = false;

        }

        public async void ExibirMeusGrupos(string id)
        {
            try
            {
               var aux = await _clienteAux.MeusGrupos(id);
                if (aux.Count == 0)
                {
                    Message = "Você ainda não possui Grupos. Crie um ou Entre em algum";

                }
                else
                {
                    var resultado = await _clienteGroup.MeusGrupos(aux);

                    MeusGroups.Clear();
                    foreach (var item in resultado)
                    {
                        MeusGroups.Add(item);

                    }
                }
            }
            catch (MobileServiceInvalidOperationException)
            {
                Message = "Ocorreu algum problema, por favor tente mais tarde";

            }

        }

        public async void ItemTapped(Grupo args)
        {   
            var navigationParams = new NavigationParameters();
            navigationParams.Add("grupo", args);
            navigationParams.Add("userId", UserId);

            await  _navigationService.NavigateAsync("GrupoSelecionadoPage", navigationParams, useModalNavigation: false);

        }

        public async void AdicionarGrupo()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("AdicionarGrupoPage", navigationParams, useModalNavigation: false);
           
        }

        public async void PesquisarGrupo()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("userId", UserId);

            await _navigationService.NavigateAsync("PesquisarGrupoPage", navigationParams, useModalNavigation: false);

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
