using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace saac.ViewModels
{
	public class CategoriasConcursoPageViewModel : ViewModelBase
	{
        private ObservableCollection<string> _categorias;
        public ObservableCollection<string> Categorias
        {
            get { return _categorias; }
            set { SetProperty(ref _categorias, value); }
        }

        private readonly INavigationService _navigationService;

        private DelegateCommand<string> _categoriaSelectedCommand;
        public DelegateCommand<string> CategoriaSelectedCommand =>
            _categoriaSelectedCommand != null ? _categoriaSelectedCommand : (_categoriaSelectedCommand = new DelegateCommand<string>(ItemTapped));


        public CategoriasConcursoPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            ExibirCategorias();

        }

        public void ExibirCategorias()
        {
            Categorias = new ObservableCollection<string> {"Nacionais", "Nordeste",
                "Norte", "Centro-Oeste", "Sudeste", "Sul" };
            
        }


        private async void ItemTapped(string obj)
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("categoria", obj);
           
            //await _navigationService.NavigateAsync("GrupoSelecionadoPage", navigationParams, useModalNavigation: false);

        }


    }
}
