using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Services
{
    public class AzureServiceGroup<T>: AzureService<T>, IAzureServiceGroup<T>
    {
        private IMobileServiceTable<Grupo> _tableGroup;

        public AzureServiceGroup()
        {
            _tableGroup = _cliente.GetTable<Grupo>();

        }
    }
}
