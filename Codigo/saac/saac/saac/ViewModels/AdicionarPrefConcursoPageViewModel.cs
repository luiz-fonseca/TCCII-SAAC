using Acr.UserDialogs;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
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
        #region Propriedades
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

        public string Opcao { get; set; }

        private readonly INavigationService _navigationService;

        private readonly IAzureServiceGroup<Grupo> _clienteGrupo;
        private readonly IAzureServiceConcurso<Concurso> _clienteConcurso;
        private readonly IAzureServicePrefConcurso<PreferenciaConcurso> _clientePreferencia;
        private readonly IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo> _clienteAux;

        private DelegateCommand _salvarCommand;
        public DelegateCommand SalvarCommand =>
            _salvarCommand ?? (_salvarCommand = new DelegateCommand(OpcaoSelecionada, CondicaoOpcaoSelecionada))
            .ObservesProperty(() => Preferencias.Administracao).ObservesProperty(() => Preferencias.Financas)
            .ObservesProperty(() => Preferencias.Licenciatura).ObservesProperty(() => Preferencias.Militar)
            .ObservesProperty(() => Preferencias.Saude).ObservesProperty(() => Preferencias.Ti)
            .ObservesProperty(() => Preferencias.Outra);

        private DelegateCommand _voltarCommand;
        public DelegateCommand VoltarCommand =>
            _voltarCommand ?? (_voltarCommand = new DelegateCommand(Voltar));
        #endregion

        #region Construtor
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
        #endregion

        #region Métodos
        public async void OpcaoSelecionada()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (Opcao.Contains("editar"))
                {
                    await Alterar();

                }
                else if (Opcao.Contains("adicionar"))
                {
                    await Salvar();

                }
            }
            else
            {
                UserDialogs.Instance.Toast("Você está sem conexão.", TimeSpan.FromSeconds(2));

            }
        }

        private bool CondicaoOpcaoSelecionada()
        {
            return Preferencias.Administracao || Preferencias.Financas
                || Preferencias.Licenciatura || Preferencias.Militar
                || Preferencias.Saude || Preferencias.Ti || Preferencias.Outra;

        }

        public async Task Salvar()
        {
            try
            {
                using (var Dialog = UserDialogs.Instance.Loading("Salvando...", null, null, true, MaskType.Black))
                {
                    await _clienteConcurso.AdicionarTable(Concursos);

                    await SalvarPreferencias();
                    await SalvarGrupo();
                    await SalvarAux();

                }

                UserDialogs.Instance.Toast("Concurso Salvo", TimeSpan.FromSeconds(2));

                var navigationParams = new NavigationParameters();
                navigationParams.Add("voltar", "");
                await _navigationService.GoBackAsync(navigationParams);

            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }

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

        public async Task Alterar()
        {
            try
            {
                await _clientePreferencia.AtualizarTable(Preferencias);

                UserDialogs.Instance.Toast("A preferência do concurso foi atualizada", TimeSpan.FromSeconds(2));
                await _navigationService.GoBackAsync();

            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }

        }

        public async void ConcursoPreferencia(string codConcurso)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Preferencias = await _clientePreferencia.ConcursoPreferencia(codConcurso);

                }
                else
                {
                    UserDialogs.Instance.Toast("Você está sem conexão.", TimeSpan.FromSeconds(2));

                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.Toast("Ops! Ocorreu algum problema", TimeSpan.FromSeconds(2));

            }
        }

        public async void Voltar()
        {
            await _navigationService.GoBackAsync();

        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("Concursos"))
            {
                Concursos = (Concurso)parameters["Concursos"];

                if (parameters.ContainsKey("adicionar"))
                {
                    Opcao = (string)parameters["adicionar"];

                }
                else if (parameters.ContainsKey("editar"))
                {
                    Opcao = (string)parameters["editar"];
              
                    ConcursoPreferencia(Concursos.Id);

                }

            }

        }
        #endregion
    }
}
