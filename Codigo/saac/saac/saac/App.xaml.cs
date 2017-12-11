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

            //Registando entre interface e a implemantação
            Container.Register<IAzureService<Usuario>,AzureService<Usuario>>();
            Container.Register<IAzureService<Grupo>, AzureService<Grupo>>();
            Container.Register<IAzureService<Publicacao>, AzureService<Publicacao>>();
            Container.Register<IAzureService<Comentario>, AzureService<Comentario>>();


        }
    }
}
