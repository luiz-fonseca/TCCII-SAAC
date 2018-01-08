using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace saac.Services.Interfaces
{
    public interface IAzureServiceBase<T>
    {
        Task AdicionarTable(T t);
        Task AtualizarTable(T t);
        Task RemoverTable(T t);
        Task<List<T>> GetTable();
        Task<T> ExisteResgistro(string id);
    }
}
