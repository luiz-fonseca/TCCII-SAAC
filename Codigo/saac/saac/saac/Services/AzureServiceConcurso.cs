using Microsoft.WindowsAzure.MobileServices;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Services
{
    public class AzureServiceConcurso<T> : AzureService<T>, IAzureServiceConcurso<T>
    {
        private IMobileServiceTable<Concurso> _tableConcurso;

        public AzureServiceConcurso()
        {
            _tableConcurso = _cliente.GetTable<Concurso>();

        }
    }
}
