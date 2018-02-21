using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace saac.ViewModels
{
	public class AdicionarConcursoPageViewModel : ViewModelBase
	{
        #region Propriedades
        private Concurso _concursos;
        public Concurso Concursos
        {
            get { return _concursos; }
            set { SetProperty(ref _concursos, value); }
        }

        private ObservableCollection<string> _regioes;
        public ObservableCollection<string> Regioes
        {
            get { return _regioes; }
            set { SetProperty(ref _regioes, value); }
        }

        private ObservableCollection<string> _estados;
        public ObservableCollection<string> Estados
        {
            get { return _estados; }
            set { SetProperty(ref _estados, value); }
        }

        private int _itemRegioes;
        public int ItemRegioes
        {
            get { return _itemRegioes; }
            set
            {
                if (_itemRegioes != value)
                {
                    SetProperty(ref _itemRegioes, value);
                    Concursos.Regiao = Regioes[_itemRegioes];
                    InicializarEstados(_itemRegioes);
                }
                
            }
        }

        private int _itemEstados;
        public int ItemEstados
        {
            get { return _itemEstados; }
            set
            {
                if (_itemEstados != value)
                {
                    SetProperty(ref _itemEstados, value);
                    Concursos.Estado = Estados[_itemEstados];

                }
                
            }
        }

        private DelegateCommand _proximoCommand;
        public DelegateCommand ProximoCommand =>
            _proximoCommand ?? (_proximoCommand = new DelegateCommand(Proximo, CondicaoProximo))
            .ObservesProperty(() => Concursos.Titulo).ObservesProperty(() => Concursos.Descricao).ObservesProperty(() => Concursos.Detalhes)
            .ObservesProperty(() => Concursos.Regiao).ObservesProperty(() => Concursos.Estado);

        private readonly INavigationService _navigationService;
        #endregion

        #region Construtor
        public AdicionarConcursoPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            Concursos = new Concurso();
            Regioes = new ObservableCollection<string>();
            Estados = new ObservableCollection<string>();

            InicializarRegioes();
        }
        #endregion

        #region Métodos
        private async void Proximo()
        {
            Concursos.Id = Guid.NewGuid().ToString("N");
            Concursos.Visibilidade = true;

            var navigationParams = new NavigationParameters();
            navigationParams.Add("Concursos", Concursos);
            await _navigationService.NavigateAsync("AdicionarPrefConcursoPage", navigationParams);

        }

        private bool CondicaoProximo()
        {
            return !string.IsNullOrWhiteSpace(Concursos.Titulo) &&
                !string.IsNullOrWhiteSpace(Concursos.Descricao) &&
                !string.IsNullOrWhiteSpace(Concursos.Detalhes) &&
                !string.IsNullOrWhiteSpace(Concursos.Regiao) &&
                !string.IsNullOrWhiteSpace(Concursos.Estado);
        }

        public void InicializarRegioes()
        {
            Regioes.Add("Nacional");
            Regioes.Add("Nordeste");
            Regioes.Add("Norte");
            Regioes.Add("Sudeste");
            Regioes.Add("Sul" );
            Regioes.Add("Centro-Oeste");

        }

        public void InicializarEstados(int item)
        {
            Estados.Clear();

            if (item == 0)
            {
                Estados.Add("");
                Estados.Add("Padrão");
            }
            else if (item == 1)
            { 
                Estados.Add("Alagoas");
                Estados.Add("Bahia");
                Estados.Add("Ceará");
                Estados.Add("Maranhão");
                Estados.Add("Paraíba");
                Estados.Add("Pernambuco");
                Estados.Add("Piauí");
                Estados.Add("Rio Grande do Norte");
                Estados.Add("Sergipe");

            }
            else if (item == 2)
            {
                Estados.Add("Acre");
                Estados.Add("Amapá");
                Estados.Add("Amazonas");
                Estados.Add("Pará");
                Estados.Add("Rondônia");
                Estados.Add("Roraima");
                Estados.Add("Tocantins");
            }
            else if (item == 3)
            {
                Estados.Add("Espírito Santo");
                Estados.Add("Minas Gerais");
                Estados.Add("Rio de Janeiro");
                Estados.Add("São Paulo");
            }
            else if (item == 4)
            {
                Estados.Add("Paraná");
                Estados.Add("Santa Catarina");
                Estados.Add("Rio Grande do Sul");
            }
            else if (item == 5)
            {
                Estados.Add("Distrito Federal");
                Estados.Add("Goiás");
                Estados.Add("Mato Grosso");
                Estados.Add("Mato Grosso do Sul");
            }
        }
        #endregion
    }
}
