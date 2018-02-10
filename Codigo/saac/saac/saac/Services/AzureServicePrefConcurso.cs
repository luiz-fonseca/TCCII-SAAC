using Microsoft.WindowsAzure.MobileServices;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Services
{
    public class AzureServicePrefConcurso<T>: AzureService<T>, IAzureServicePrefConcurso<T>
    {
        private IMobileServiceTable<PreferenciaConcurso> _tablePrefConcurso;

        public AzureServicePrefConcurso()
        {
            _tablePrefConcurso = _cliente.GetTable<PreferenciaConcurso>();

        }
    }
}
