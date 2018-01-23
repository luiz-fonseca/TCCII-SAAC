using Prism.Common;
using Prism.Navigation;
using Xamarin.Forms;

namespace saac.Views
{
    public partial class PrincipalPage : TabbedPage, INavigatingAware
    {
        public PrincipalPage()
        {
            InitializeComponent();
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            foreach (var child in Children)
            {
                PageUtilities.OnNavigatingTo(child, parameters);
            }
        }
    }
}
