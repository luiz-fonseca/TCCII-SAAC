using Microsoft.WindowsAzure.MobileServices;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Services
{
    public class AzureServicePrefUser<T> : AzureService<T>, IAzureServicePrefUser<T>
    {
        private IMobileServiceTable<PreferenciaUser> _tablePrefUser;

        public AzureServicePrefUser()
        {
            _tablePrefUser = _cliente.GetTable<PreferenciaUser>();

        }
    }
}
