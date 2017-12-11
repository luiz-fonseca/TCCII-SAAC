using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using saac.Services.Interfaces;

namespace saac.Services
{
    public class AzureService<T>: IAzureService<T>
    {
        private IMobileServiceClient _cliente;
        private IMobileServiceTable<T> _table;
        private const string serviceUri = "http://saac.azurewebsites.net";

        public AzureService()
        {
            _cliente = new MobileServiceClient(serviceUri);
            _table = _cliente.GetTable<T>();

        }

        async void IAzureService<T>.AdicionarTable(T t)
        {
            await _table.InsertAsync(t);
        }

        async void IAzureService<T>.AtualizarTable(T t)
        {
            await _table.UpdateAsync(t);
        }

        async void IAzureService<T>.RemoverTable(T t)
        {
            await _table.DeleteAsync(t);
        }

        async Task<IEnumerable<T>> IAzureService<T>.GetTable()
        {
            return await _table.ToEnumerableAsync();
        }
    }
}
