using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services.Interfaces
{
    public interface IAzureServicePublication<T>: IAzureServiceBase<T>
    {
        Task<List<Publicacao>> Publicacoes(string codGrupo);
    }
}
