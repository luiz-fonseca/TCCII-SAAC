using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServiceAux<T> : AzureService<T>, IAzureServiceAux<T>
    {
        private IMobileServiceTable<Auxiliar> _tableAux;

        public AzureServiceAux()
        {
            _tableAux = _cliente.GetTable<Auxiliar>();

        }

        Task<List<string>> IAzureServiceAux<T>.MeusGrupos(string id)
        {
            throw new NotImplementedException();
        }
    }
}
