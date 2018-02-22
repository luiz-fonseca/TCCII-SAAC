using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Interfaces
{
    public interface IAzureServiceConcurso<T> : IAzureServiceBase<T>
    {
        Task<List<Concurso>> ConcursosDisponiveis(string regiao);
        Task<List<Concurso>> MeusConcursos(List<string> codConcurso);

    }
}
