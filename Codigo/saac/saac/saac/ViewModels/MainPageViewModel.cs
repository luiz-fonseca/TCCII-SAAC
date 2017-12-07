using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using saac.Models;
using saac.Services;

namespace saac.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private AzureService _cliente;
        public Usuario _user;

        private string _ex;

        public string Ex
        {
            get { return _ex; }
            set { SetProperty(ref _ex, value); }
        }


        public DelegateCommand CriarUsuarioCommand { get; set; }


        public MainPageViewModel(INavigationService navigationService) 
            : base (navigationService)
        {
            _cliente = new AzureService();
            
            CriarUsuarioCommand = new DelegateCommand(CriarUsuario);
        }

        private void CriarUsuario()
        {
            _user = new Usuario();
            _user.Id = "ac67";
            _user.Nome = "Luiz";
            _user.Foto = "Teste";
            _user.DtNasci = new DateTime(25 / 11 / 1991);
            _user.Sexo = true;
            _user.Endereco = "Pedra Mole";


            _cliente.addUsuario(_user);
        }
    }
}
