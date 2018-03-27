using Prism;
using Prism.Ioc;
using saac.ViewModels;
using saac.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.DryIoc;
using saac.Interfaces;
using saac.Models;
using saac.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace saac
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<PrincipalPage>();
            containerRegistry.RegisterForNavigation<PublicacaoSelecionadaPage>();
            containerRegistry.RegisterForNavigation<PesquisarGrupoPage>();
            containerRegistry.RegisterForNavigation<MeusGruposPage>();
            containerRegistry.RegisterForNavigation<GrupoSelecionadoPage>();
            containerRegistry.RegisterForNavigation<AdicionarGrupoPage>();
            containerRegistry.RegisterForNavigation<SobrePage>();
            containerRegistry.RegisterForNavigation<CategoriasConcursoPage>();
            containerRegistry.RegisterForNavigation<CategoriaSelecionadaPage>();
            containerRegistry.RegisterForNavigation<MaisPage>();
            containerRegistry.RegisterForNavigation<AdicionarConcursoPage>();
            containerRegistry.RegisterForNavigation<AdicionarPrefConcursoPage>();
            containerRegistry.RegisterForNavigation<AdicionarPrefUserPage>();
            containerRegistry.RegisterForNavigation<NotificacoesPage>();
            containerRegistry.RegisterForNavigation<ConcursoSelecionadoPage>();
            containerRegistry.RegisterForNavigation<GerenciarConcursoPage>();
            containerRegistry.RegisterForNavigation<ConcursoInscricoesFinalizadasPage>();
            containerRegistry.RegisterForNavigation<ConcursoFinalizadosPage>();
            containerRegistry.RegisterForNavigation<ConcursoEmEsperaPage>();
            containerRegistry.RegisterForNavigation<UsuarioSelecionadoPage>();
            containerRegistry.RegisterForNavigation<AdicionarUsuarioPage>();

            //registrando interfaces
            containerRegistry.Register<IAzureServiceUser<Usuario>, AzureServiceUser<Usuario>>();
            containerRegistry.Register<IAzureServiceGroup<Grupo>, AzureServiceGroup<Grupo>>();
            containerRegistry.Register<IAzureServiceAux<Auxiliar>, AzureServiceAux<Auxiliar>>();
            containerRegistry.Register<IAzureServicePublication<Publicacao>, AzureServicePublication<Publicacao>>();
            containerRegistry.Register<IAzureServiceComment<Comentario>, AzureServiceComment<Comentario>>();
            containerRegistry.Register<IFacebookService, FacebookService>();
            containerRegistry.Register<IAzureServiceAuxConcursoGrupo<AuxConcursoGrupo>, AzureServiceAuxConcursoGrupo<AuxConcursoGrupo>>();
            containerRegistry.Register<IAzureServiceConcurso<Concurso>, AzureServiceConcurso<Concurso>>();
            containerRegistry.Register<IAzureServicePrefConcurso<PreferenciaConcurso>, AzureServicePrefConcurso<PreferenciaConcurso>>();
            containerRegistry.Register<IAzureServicePrefUser<PreferenciaUser>, AzureServicePrefUser<PreferenciaUser>>();
            
        }
    }
}
