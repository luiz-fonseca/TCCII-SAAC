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
        private readonly IAzureServiceUser _cliente;
        public Usuario _user;

        public DelegateCommand CriarUsuarioCommand { get; set; }


        public MainPageViewModel(INavigationService navigationService, IAzureServiceUser cliente) 
            : base (navigationService)
        {
            _cliente = cliente;
            CriarUsuarioCommand = new DelegateCommand(CriarUsuario);

        }

        private void CriarUsuario()
        {
            _user = new Usuario();
            _user.Id = "5yh";
            _user.Nome = "Gilon";
            _user.Foto = "Teste";
            _user.DtNasci = new DateTime(25 / 11 / 1991);
            _user.Sexo = true;
            _user.Endereco = "Pedra Mole";

            _cliente.AddUsuario(_user);
        }
    }
}
