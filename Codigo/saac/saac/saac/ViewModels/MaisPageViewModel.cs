using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class MaisPageViewModel : ViewModelBase
	{
        #region Propriedades
        protected bool HasInitialized { get; set; }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        //private readonly INavigationService _navigationService;
        #endregion

        #region Construtor
        public MaisPageViewModel(INavigationService navigationService) : base(navigationService)
        {
          

        }
        #endregion

        #region Métodos
        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            if (HasInitialized) return;
            HasInitialized = true;

            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"]; ;
            }

        }

        #endregion
    }
}
