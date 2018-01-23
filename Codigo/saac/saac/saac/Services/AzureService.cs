using Microsoft.WindowsAzure.MobileServices;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        async Task<List<T>> IAzureServiceBase<T>.GetTable()
        {
            return await _table.ToListAsync();
        }

        async Task<T> IAzureServiceBase<T>.ExisteResgistro(string id)
        {
            return await _table.LookupAsync(id);

        }

    }

}
