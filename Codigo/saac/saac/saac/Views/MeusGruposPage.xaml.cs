using Xamarin.Forms;

namespace saac.Views
{
    public partial class MeusGruposPage : ContentPage
    {
        public MeusGruposPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
                adMobView.AdUnitId = "ca-app-pub-3940256099942544/6300978111";
        }
    }
}
