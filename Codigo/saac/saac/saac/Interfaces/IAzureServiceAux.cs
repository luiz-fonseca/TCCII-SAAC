using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Interfaces
{
    public interface IAzureServiceAux<T> : IAzureServiceBase<T>
    {
        Task<List<string>> MeusGrupos(string id);
        Task<int> ExisteSeguirAux(string idGrupo, string idUser);
        Task<Auxiliar> GetAuxiliar(string idGrupo, string idUser);
        Task<int> QuantidadeRegistros(string idGrupo);

    }
}
