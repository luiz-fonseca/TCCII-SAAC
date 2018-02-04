﻿using Microsoft.WindowsAzure.MobileServices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Models;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace saac.ViewModels
{
	public class PesquisarGrupoPageViewModel : ViewModelBase
    {
        #region Propriedades
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
            set { _message = value; }
        }

        private string _pesquisar;
        public string Pesquisar
        {
            get { return _pesquisar; }
            set
            {
                SetProperty(ref _pesquisar, value);
                //PesquisarGrupoCommand.RaiseCanExecuteChanged();
            }
        }
        
        private ObservableCollection<Grupo> _groups;
        public ObservableCollection<Grupo> Groups
        {
            get { return _groups; }
            set { SetProperty(ref _groups, value); }
        }

        private readonly IAzureServiceGroup<Grupo> _clienteGroup;

        private readonly INavigationService _navigationService;

        private DelegateCommand _pesquisarGrupoCommand;
        public DelegateCommand PesquisarGrupoCommand =>
            _pesquisarGrupoCommand ?? (_pesquisarGrupoCommand = new DelegateCommand(PesquisarGrupo));

        private DelegateCommand _atualizarCommand;
        public DelegateCommand AtualizarCommand =>
            _atualizarCommand ?? (_atualizarCommand = new DelegateCommand(AtualizarGrupos));

        private DelegateCommand<Grupo> _grupoSelectedCommand;
        public DelegateCommand<Grupo> GrupoSelectedCommand =>
            _grupoSelectedCommand != null ? _grupoSelectedCommand : (_grupoSelectedCommand = new DelegateCommand<Grupo>(ItemTapped));

        #endregion

        public PesquisarGrupoPageViewModel(INavigationService navigationService, IAzureServiceGroup<Grupo> clienteGroup) : base(navigationService)
        {
            _navigationService = navigationService;
            _clienteGroup = clienteGroup;

            Groups = new ObservableCollection<Grupo>();

        }

        /*private bool CondicaoPesquisarGrupo()
        {
            return !string.IsNullOrWhiteSpace(Pesquisar);

        }*/

        public void AtualizarGrupos()
        {
            Atualizando = true;

            ExibirGrupos();

            Atualizando = false;

        }

        public async void PesquisarGrupo()
        {
            var auxList = await _clienteGroup.PesquisarGrupos(Pesquisar);

            Groups.Clear();
            foreach (var item in auxList)
            {
                Groups.Add(item);

            }


        }

        public async void ExibirGrupos()
        {
            try
            {
                var resultado = await _clienteGroup.GetTable();
                if (resultado.Count != 0)
                {
                    Groups.Clear();
                    foreach (var item in resultado)
                    {
                        Groups.Add(item);

                    }

                }
                else
                {
                    Message = "Ainda não existem Grupos. Crie um ou Entre em algum";

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

            await _navigationService.NavigateAsync("GrupoSelecionadoPage", navigationParams);

        }


        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"]; ;
            }

            ExibirGrupos();
        }
    }
}