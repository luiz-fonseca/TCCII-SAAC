using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace saac.Services.Interfaces
{
    public interface IAzureServiceBase<T>
    {
        void AdicionarTable(T t);
        void AtualizarTable(T t);
        void RemoverTable(T t);
        Task<IEnumerable<T>> GetTable();
        Task<T> ExisteResgistro(string id);
    }
}
