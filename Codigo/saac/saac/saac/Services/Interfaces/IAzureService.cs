using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace saac.Services.Interfaces
{
    public interface IAzureService<T>
    {
        void AdicionarTable(T t);
        void AtualizarTable(T t);
        void RemoverTable(T t);
        Task<IEnumerable<T>> GetTable();
    }
}
