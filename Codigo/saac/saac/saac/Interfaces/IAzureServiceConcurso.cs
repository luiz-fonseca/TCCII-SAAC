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
        Task<List<Concurso>> ConcursosDisponiveis();
        Task<List<Concurso>> PesquisarConcursos(string regiao, string pesquisarPublicacao);
        Task<List<Concurso>> PesquisarConcursos(string pesquisarPublicacao);
        Task<List<Concurso>> MeusConcursos(List<string> codConcurso);
        Task<List<Concurso>> InscricoesFinalizadas(DateTime dataAtual);
        Task<List<Concurso>> ConcursosFinalizados(DateTime dataAutual);
        Task<List<Concurso>> ConcursosEmEspera(DateTime dataAutual);


    }
}
