using Acr.UserDialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace saac.ViewModels
{
	public class MaisPageViewModel : ViewModelBase
	{
        #region Propriedades
        protected bool HasInitialized { get; set; }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        private ObservableCollection<object> _minhasOpcoesDePerfil;
        public ObservableCollection<object> MinhasOpcoesDePerfil
        {
            get { return _minhasOpcoesDePerfil; }
            set { SetProperty(ref _minhasOpcoesDePerfil, value); }
        }

        private readonly INavigationService _navigationService;

        private DelegateCommand<object> _opcaoSelectedCommand;
        public DelegateCommand<object> OpcaoSelectedCommand =>
            _opcaoSelectedCommand != null ? _opcaoSelectedCommand : (_opcaoSelectedCommand = new DelegateCommand<object>(ItemTapped));

        #endregion

        #region Construtor
        public MaisPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            MinhasOpcoesDePerfil = new ObservableCollection<object>();

        }
        #endregion

        #region Métodos
        private async void ItemTapped(object obj)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var aux = ConversaoCategoria(obj);
                var resultado = OpcaoSelecionada(aux);

                var navigationParams = new NavigationParameters();
                navigationParams.Add("userId", UserId);
                navigationParams.Add(resultado[1], resultado[2]);

                await _navigationService.NavigateAsync(resultado[0], navigationParams, useModalNavigation: false);

            }
            else
            {
                UserDialogs.Instance.Toast("Você está sem conexão.", TimeSpan.FromSeconds(2));

            }
        }

        public string[] OpcaoSelecionada(string obj)
        {
            string[] resultado = new string[3];

            switch (obj)
            {
                case "Meu Perfil":
                    resultado[0] = "UsuarioSelecionadoPage";
                    return resultado;

                case "Minhas Preferências":
                    resultado[0] = "AdicionarPrefUserPage";
                    resultado[1] = "editar";
                    resultado[2] = "editar";
                    return resultado;

                default:
                    return resultado;

            }

        }

        public void ExibirOpcoes()
        {
            MinhasOpcoesDePerfil.Add(new { Nome = "Meu Perfil" });
            MinhasOpcoesDePerfil.Add(new { Nome = "Minhas Preferências" });
            
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

                ExibirOpcoes();

            }

        }

        #endregion
    }
}
