﻿using Prism.Common;
using Prism.Navigation;
using Xamarin.Forms;

namespace saac.Views
{
    public partial class NotificacoesPage : TabbedPage, INavigatingAware
    {
        public NotificacoesPage()
        {
            InitializeComponent();
            
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            foreach (var child in Children)
            {
                PageUtilities.OnNavigatingTo(child, parameters);
            }
        }
    }
}
