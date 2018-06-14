using Acr.UserDialogs;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace saac.ViewModels
{
	public class NotificacoesPageViewModel : BindableBase
	{
        private string _titulo = "Notificações";
        public string Titulo
        {
            get { return _titulo; }
            set { SetProperty(ref _titulo, value); }
        }

        #region Construtor
        public NotificacoesPageViewModel()
        {
            MessengerCenter();

        }
        #endregion
        
        public void MessengerCenter()
        {
            MessagingCenter.Subscribe<Message>(this, "Notificacoes", message =>
            {
                if (message.Value != 0)
                {
                    Titulo = "Notificações " + message.Value;

                }
                else
                {
                    Titulo = "Notificações";
                }
            });
        }

    }
}
