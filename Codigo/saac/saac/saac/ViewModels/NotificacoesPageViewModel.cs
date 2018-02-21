using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class NotificacoesPageViewModel : ViewModelBase
	{
        #region Propriedades
        private readonly INavigationService _navigationService;
        #endregion

        #region Construtor
        public NotificacoesPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

        }
        #endregion

        #region Métodos
        #endregion
    }
}
