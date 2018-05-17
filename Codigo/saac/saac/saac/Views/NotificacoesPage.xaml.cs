using Prism.Common;
using Prism.Navigation;
using saac.Helpers;
using Xamarin.Forms;

namespace saac.Views
{
    public partial class NotificacoesPage : TabbedPage, INavigatingAware
    {
        public NotificacoesPage()
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
