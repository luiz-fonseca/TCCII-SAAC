using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace saac.ViewModels
{
	public class PublicacaoSelecionadaPageViewModel : ViewModelBase
	{

        private Publicacao _publication;
        public Publicacao Publication
        {
            get { return _publication; }
            set { SetProperty(ref _publication, value); }
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private readonly INavigationService _navigationService;

        public PublicacaoSelecionadaPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("publicacao"))
            {
                Publication = (Publicacao)parameters["publicacao"];
            }

            if (parameters.ContainsKey("userId"))
            {
                UserId = (string)parameters["userId"]; ;
            }
        }
    }
}
