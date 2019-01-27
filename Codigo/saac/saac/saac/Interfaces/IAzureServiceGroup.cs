using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Interfaces
{
    public interface IAzureServiceGroup<T> : IAzureServiceBase<T>
    {
        Task<List<Grupo>> MeusGrupos(List<string> meusCodigos);
        Task<List<Grupo>> PesquisarGrupos(string pesquisarGrupo);
        Task<List<Grupo>> PesquisarMeusGrupos(List<string> meusCodigos, string pesquisarGrupo);

    }
}
