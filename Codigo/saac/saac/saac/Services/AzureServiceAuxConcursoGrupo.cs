using Microsoft.WindowsAzure.MobileServices;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServiceAuxConcursoGrupo<T>: AzureService<T>, IAzureServiceAuxConcursoGrupo<T>
    {
        private IMobileServiceTable<AuxConcursoGrupo> _tableAuxConcursoGrupo;

        public AzureServiceAuxConcursoGrupo()
        {
            _tableAuxConcursoGrupo = _cliente.GetTable<AuxConcursoGrupo>();

        }

        async Task<List<string>> IAzureServiceAuxConcursoGrupo<T>.GruposConcursos(string codConcurso)
        {
            var lista = new List<string>();

            var query = _tableAuxConcursoGrupo
                .Where(Auxiliar => Auxiliar.CodConcurso == codConcurso)
                .Select(Auxiliar => Auxiliar.CodGrupo);

            var resultado = await query.ToListAsync();

            foreach (var item in resultado)
            {
                lista.Add(item);
            }
            return lista;

        }

        async Task<List<AuxConcursoGrupo>> IAzureServiceAuxConcursoGrupo<T>.ListaGruposConcursos(string codConcurso)
        {
            var lista = new List<AuxConcursoGrupo>();

            var query = _tableAuxConcursoGrupo
                .Where(Auxiliar => Auxiliar.CodConcurso == codConcurso);

            lista = await query.ToListAsync();
            
            return lista;

        }
    }
}
