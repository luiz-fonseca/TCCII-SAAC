using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Services
{
    public class AzureServicePublication<T>: AzureService<T>, IAzureServicePublication<T>
    {
        private IMobileServiceTable<Publicacao> _tablePublication;

        public AzureServicePublication()
        {
            _tablePublication = _cliente.GetTable<Publicacao>();

        }
    }
}
