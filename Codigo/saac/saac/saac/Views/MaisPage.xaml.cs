using Xamarin.Essentials;
using Xamarin.Forms;

namespace saac.Views
{
    public partial class MaisPage : ContentPage
    {
        public MaisPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
                adMobView.AdUnitId = Preferences.Get("publicidade", "");

        }
    }
}
