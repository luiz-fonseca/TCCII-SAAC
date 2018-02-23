using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Interfaces
{
    public interface IAzureServiceAuxConcursoGrupo<T> : IAzureServiceBase<T>
    {
        Task<List<string>> GruposConcursos(string codConcurso);
    }
}
