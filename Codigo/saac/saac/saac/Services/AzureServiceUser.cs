using System;
using System.Collections.Generic;
using System.Text;


using Microsoft.WindowsAzure.MobileServices;
using saac.Models;


namespace saac.Services
{
    public class AzureServiceUser : AzureService, IAzureServiceUser
    {

        private IMobileServiceTable<Usuario> _tableUsuario;

        public AzureServiceUser()
        {
            _tableUsuario = _cliente.GetTable<Usuario>();
        }


        async void IAzureServiceUser.AddUsuario(Usuario user)
        {
            await _tableUsuario.InsertAsync(user);
        }
    }
}
