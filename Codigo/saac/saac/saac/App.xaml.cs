using saac.ViewModels;
using saac.Views;
using DryIoc;
using Prism.DryIoc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using saac.Services;
using saac.Services.Interfaces;
using saac.Models;

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

            //página inicial
            await NavigationService.NavigateAsync("MainPage");
        }

        protected override void RegisterTypes()
        {
            //Registrando para a navegação
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<PrincipalPage>();
            Container.RegisterTypeForNavigation<MeusGruposPage>();
            Container.RegisterTypeForNavigation<AdicionarGrupoPage>();
            Container.RegisterTypeForNavigation<GrupoSelecionadoPage>();

            //Registando entre interface e a implemantação
            Container.Register<IAzureServiceUser<Usuario>, AzureServiceUser<Usuario>>();
            Container.Register<IAzureServiceGroup<Grupo>, AzureServiceGroup<Grupo>>();
            Container.Register<IAzureServiceAux<Auxiliar>, AzureServiceAux<Auxiliar>>();
            Container.Register<IAzureServicePublication<Publicacao>, AzureServicePublication<Publicacao>>();
            Container.Register<IAzureServiceComment<Comentario>, AzureServiceComment<Comentario>>();


            Container.RegisterTypeForNavigation<PublicacaoSelecionadaPage>();
        }
    }
}
