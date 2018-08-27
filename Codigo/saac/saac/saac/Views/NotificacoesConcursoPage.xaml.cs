using Xamarin.Essentials;
using Xamarin.Forms;

namespace saac.Views
{
    public partial class NotificacoesConcursoPage : ContentPage
    {
        public NotificacoesConcursoPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
                adMobView.AdUnitId = Preferences.Get("publicidade", "");

        }
    }
}
