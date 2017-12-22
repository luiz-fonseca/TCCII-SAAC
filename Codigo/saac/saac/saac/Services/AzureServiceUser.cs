using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServiceUser<T> : AzureService<T>, IAzureServiceUser<T>
    {
        private IMobileServiceTable<Usuario> _tableUser;

        public AzureServiceUser()
        {      
            _tableUser = _cliente.GetTable<Usuario>();

        }

      
    }
}
