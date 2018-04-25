using saac.Helpers;
using Xamarin.Forms;

namespace saac.Views
{
    public partial class MaisPage : ContentPage
    {
        public MaisPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
                adMobView.AdUnitId = Settings.IdAdMobAndroid;

        }
    }
}
