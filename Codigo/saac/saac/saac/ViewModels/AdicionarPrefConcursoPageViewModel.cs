using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace saac.ViewModels
{
    public class AdicionarPrefConcursoPageViewModel : ViewModelBase
    {
        private Concurso _concursos;
        public Concurso Concursos
        {
            get { return _concursos; }
            set { SetProperty(ref _concursos, value); }
        }

        private PreferenciaConcurso _preferencias;
        public PreferenciaConcurso Preferencias
        {
            get { return _preferencias; }
            set { SetProperty(ref _preferencias, value); }
        }

        private Grupo _grupos;
        public Grupo Grupos
        {
            get { return _grupos; }
            set { SetProperty(ref _grupos, value); }
        }

        private AuxConcursoGrupo _auxiliar;
        public AuxConcursoGrupo Auxiliar
        {
            get { return _auxiliar; }
            set { SetProperty(ref _auxiliar, value); }
        }

        private readonly INavigationService _navigationService;

        private readonly IAzureServiceGroup<Grupo> _clienteGrupo;
        private readonly IAzureServiceConcurso<Concurso> _clienteConcurso;
        private readonly IAzureServicePrefConcurso<PreferenciaConcurso> _clientePreferencia;
        private readonly IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> _clienteAux;

        private DelegateCommand _salvarCommand;
        public DelegateCommand SalvarCommand =>
            _salvarCommand ?? (_salvarCommand = new DelegateCommand(Salvar));

        private DelegateCommand _voltarCommand;
        public DelegateCommand VoltarCommand =>
            _voltarCommand ?? (_voltarCommand = new DelegateCommand(Voltar));
   
        

        public AdicionarPrefConcursoPageViewModel(INavigationService navigationService, IAzureServiceGroup<Grupo> clienteGrupo,
           IAzureServiceConcurso<Concurso> clienteConcurso, IAzureServicePrefConcurso<PreferenciaConcurso> clientePreferencia,
           IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> clienteAux) : base(navigationService)
        {
            _navigationService = navigationService;
            _clienteGrupo = clienteGrupo;
            _clienteConcurso = clienteConcurso;
            _clientePreferencia = clientePreferencia;
            _clienteAux = clienteAux;

            Concursos = new Concurso();
            Preferencias = new PreferenciaConcurso();
            Grupos = new Grupo();
            Auxiliar = new AuxConcursoGrupo();

        }

        public async void Salvar()
        {
            await _clienteConcurso.AdicionarTable(Concursos);

            await SalvarPreferencias();
            await SalvarGrupo();
            await SalvarAux();

        }

        public async Task SalvarPreferencias()
        {
            Preferencias.Id = Guid.NewGuid().ToString("N");
            Preferencias.CodConcurso = Concursos.Id;

            await _clientePreferencia.AdicionarTable(Preferencias);
        }

        public async Task SalvarGrupo()
        {
            Grupos.Id = Guid.NewGuid().ToString("N");
            Grupos.Nome = Concursos.Titulo;
            Grupos.Descricao = Concursos.Descricao;
            Grupos.Categoria = "Padrão";
            Grupos.Temporario = true;

            await _clienteGrupo.AdicionarTable(Grupos);
        }

        public async Task SalvarAux()
        {
            Auxiliar.Id = Guid.NewGuid().ToString("N");
            Auxiliar.CodConcurso = Concursos.Id;
            Auxiliar.CodGrupo = Grupos.Id;

            await _clienteAux.AdicionarTable(Auxiliar);
        }

        public async void Voltar()
        {
            await _navigationService.GoBackAsync();
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("Concursos"))
            {
                Concursos = (Concurso)parameters["Concursos"];

            }
        }
    }
}
