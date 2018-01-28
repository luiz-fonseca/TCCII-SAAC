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
        #region Propriedades
        private readonly IAzureServiceGroup<Grupo> _clienteGrupo;
        private readonly IAzureServiceAux<Auxiliar> _clienteAuxiliar;

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _dialogService;

        private Grupo _grupos;
        public Grupo Grupos
        {
            get { return _grupos; }
            set { SetProperty(ref _grupos, value); }
        }

        public Auxiliar _aux;

        public Auxiliar Aux
        {
            get { return _aux; }
            set { SetProperty(ref _aux, value); }
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        private string _nome;
        public string Nome
        {
            get { return _nome; }
            set
            {
                SetProperty(ref _nome, value);
                SalvarGrupoCommand.RaiseCanExecuteChanged();
            }
        }

        private string _descricao;
        public string Descricao
        {
            get { return _descricao; }
            set
            {
                SetProperty(ref _descricao, value);
                SalvarGrupoCommand.RaiseCanExecuteChanged();
            }
        }

        private string _categoria;
        public string Categoria
        {
            get { return _categoria; }
            set
            {
                SetProperty(ref _categoria, value);
                SalvarGrupoCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _salvarGrupoCommand;
        public DelegateCommand SalvarGrupoCommand =>
            _salvarGrupoCommand ?? (_salvarGrupoCommand = new DelegateCommand(SalvarGrupo, CondicaoSalvarGrupo));

        #endregion

        public AdicionarGrupoPageViewModel(INavigationService navigationService, IPageDialogService dialogService,
            IAzureServiceGroup<Grupo> clienteGrupo, IAzureServiceAux<Auxiliar> clienteAuxiliar
            ) : base(navigationService)
        {
            _clienteGrupo = clienteGrupo;
            _clienteAuxiliar = clienteAuxiliar;

            Grupos = new Grupo();
            Aux = new Auxiliar();

            _navigationService = navigationService;
            _dialogService = dialogService;

        }

        private bool CondicaoSalvarGrupo()
        {
            return !string.IsNullOrWhiteSpace(Nome) &&
                !string.IsNullOrWhiteSpace(Descricao) &&
                !string.IsNullOrWhiteSpace(Categoria);
        }

        private async void SalvarGrupo()
        {
            Grupos.Id = Guid.NewGuid().ToString("N");
            Grupos.Nome = Nome;
            Grupos.Descricao = Descricao;
            Grupos.Categoria = Categoria;

            await _clienteGrupo.AdicionarTable(Grupos);

            SalvarAuxiliar(Grupos.Id, true);

            Nome = string.Empty;
            Descricao = string.Empty;
            Categoria = string.Empty;

            await _dialogService.DisplayAlertAsync("Grupo Cadastrado", "Parabéns!! O cadastro" +
                " do seu grupo foi realizado.", "OK");
            await _navigationService.GoBackAsync();
        }

        private async void SalvarAuxiliar(string IdGrupo, bool adm)
        {
            Aux.CodGrupo = IdGrupo;
            Aux.CodUsuario = UserId;
            Aux.Adiministrador = adm;

            await _clienteAuxiliar.AdicionarTable(Aux);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            UserId = parameters.GetValue<string>("userId");

        }

    }
}
