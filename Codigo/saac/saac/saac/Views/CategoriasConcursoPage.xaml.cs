using Xamarin.Essentials;
using Xamarin.Forms;

namespace saac.Views
{
    public partial class CategoriasConcursoPage : ContentPage
    {
        public CategoriasConcursoPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
                adMobView.AdUnitId = Preferences.Get("publicidade", "");

        }
    }
}
