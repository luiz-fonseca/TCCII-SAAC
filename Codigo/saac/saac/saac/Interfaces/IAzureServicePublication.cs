using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Interfaces
{
    public interface IAzureServicePublication<T> : IAzureServiceBase<T>
    {
        Task<List<Publicacao>> Publicacoes(string codGrupo);
        Task<int> MinhaPublicaco(string codPublicao, string CodUsuario);
        Task<int> QtdPublicacoesPendentes(string codGrupo, string codUsuario, DateTime dataVisualizacao);
        Task<List<Publicacao>> MinhasPublicacoes(string codUsuario);
    }
}
