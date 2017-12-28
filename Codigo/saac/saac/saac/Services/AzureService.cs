using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Services.Interfaces;

namespace saac.Services
{
    public class AzureService<T> : IAzureServiceBase<T>
    {
        protected IMobileServiceClient _cliente;

        private IMobileServiceTable<T> _table; 

        private const string serviceUri = "http://saac.azurewebsites.net";

        public AzureService()
        {
            _cliente = new MobileServiceClient(serviceUri);
            _table = _cliente.GetTable<T>();

        }

        async Task IAzureServiceBase<T>.AdicionarTable(T t)
        {
            await _table.InsertAsync(t);
        }

        async Task IAzureServiceBase<T>.AtualizarTable(T t)
        {
            await _table.UpdateAsync(t);
        }

        async Task IAzureServiceBase<T>.RemoverTable(T t)
        {
            await _table.DeleteAsync(t);
        }

        async Task<IEnumerable<T>> IAzureServiceBase<T>.GetTable()
        {
            return await _table.ToEnumerableAsync();
        }

        async Task<T> IAzureServiceBase<T>.ExisteResgistro(string id)
        {  
           return await _table.LookupAsync(id);
          
        }

    }
}
