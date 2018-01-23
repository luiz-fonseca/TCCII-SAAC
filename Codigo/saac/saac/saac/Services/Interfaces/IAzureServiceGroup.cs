using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services.Interfaces
{
    public interface IAzureServiceGroup<T> : IAzureServiceBase<T>
    {
        Task<List<Grupo>> MeusGrupos(List<string> meusCodigos);
        Task<List<Grupo>> PesquisarGrupos(string nomeGrupo);

    }
}
