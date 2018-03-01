using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace saac.ViewModels
{
	public class CategoriasConcursoPageViewModel : ViewModelBase
	{
        #region Propriedades
        protected bool HasInitialized { get; set; }

        private bool _verificadorAdm = false;
        public bool VerificadorAdm 
        {
            get { return _verificadorAdm; }
            set
            {
                SetProperty(ref _verificadorAdm, value);
                AdministradorCommand.RaiseCanExecuteChanged();
            }
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        private ObservableCollection<object> _categorias;
        public ObservableCollection<object> Categorias
        {
            get { return _categorias; }
            set { SetProperty(ref _categorias, value); }
        }

        private readonly INavigationService _navigationService;
        private readonly IAzureServiceUser<Usuario> _clienteUser;

        private DelegateCommand _administradorCommand;
        public DelegateCommand AdministradorCommand =>
            _administradorCommand ?? (_administradorCommand = new DelegateCommand(Administrador, CondicaoAdministrador));

        private DelegateCommand<object> _categoriaSelectedCommand;
        public DelegateCommand<object> CategoriaSelectedCommand =>
            _categoriaSelectedCommand != null ? _categoriaSelectedCommand : (_categoriaSelectedCommand = new DelegateCommand<object>(ItemTapped));
        #endregion

        #region Construtor
        public CategoriasConcursoPageViewModel(INavigationService navigationService, IAzureServiceUser<Usuario> clienteUser) : base(navigationService)
        {
            _navigationService = navigationService;
            _clienteUser = clienteUser;

            Categorias = new ObservableCollection<object>();

            ExibirCategorias();

        }
        #endregion

        #region Métodos
        public void ExibirCategorias()
        {
            Categorias.Add(new  { Nome="Nacional"});
            Categorias.Add(new  { Nome = "Nordeste" });
            Categorias.Add(new  { Nome = "Norte" });
            Categorias.Add(new  { Nome = "Sudeste" });
            Categorias.Add(new  { Nome = "Sul" });
            Categorias.Add(new  { Nome = "Centro-Oeste"});

        }


        private async void ItemTapped(object obj)
        {
            var args = ConversaoCategoria(obj);
            var navigationParams = new NavigationParameters();
            navigationParams.Add("categoria", args);
            navigationParams.Add("userId", UserId);
           
            await _navigationService.NavigateAsync("CategoriaSelecionadaPage", navigationParams, useModalNavigation: false);

        }

        private async void Administrador()
        {
            await _navigationService.NavigateAsync("GerenciarConcursoPage", useModalNavigation: false);
        }

        private bool CondicaoAdministrador()
        {
            return VerificadorAdm;

        }

        public async void Verificacao(string id)
        {
           VerificadorAdm = await _clienteUser.VerificarAdministrador(id);

        }

        
        public string ConversaoCategoria(object args)
        {
            var aux = Conversao(args, new { Nome = "" });

            var resutado = aux.Nome;

            return resutado;
            
        }

        public T Conversao<T>(object objeto, T tipo)
        {
            return (T)objeto;

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (HasInitialized) return;
            HasInitialized = true;

            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"];

                Verificacao(UserId);

            }
        }

        #endregion
    }
}
