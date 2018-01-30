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
        private ObservableCollection<object> _categorias;
        public ObservableCollection<object> Categorias
        {
            get { return _categorias; }
            set { SetProperty(ref _categorias, value); }
        }

        private readonly INavigationService _navigationService;

        private DelegateCommand<object> _categoriaSelectedCommand;
        public DelegateCommand<object> CategoriaSelectedCommand =>
            _categoriaSelectedCommand != null ? _categoriaSelectedCommand : (_categoriaSelectedCommand = new DelegateCommand<object>(ItemTapped));


        public CategoriasConcursoPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Categorias = new ObservableCollection<object>();

            ExibirCategorias();

        }

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
           
            await _navigationService.NavigateAsync("CategoriaSelecionadaPage", navigationParams, useModalNavigation: false);

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


    }
}
