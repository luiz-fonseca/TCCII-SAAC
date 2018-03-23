using Microsoft.WindowsAzure.MobileServices;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServicePrefConcurso<T>: AzureService<T>, IAzureServicePrefConcurso<T>
    {
        private IMobileServiceTable<PreferenciaConcurso> _tablePrefConcurso;

        public AzureServicePrefConcurso()
        {
            _tablePrefConcurso = _cliente.GetTable<PreferenciaConcurso>();

        }

        async Task<PreferenciaConcurso> IAzureServicePrefConcurso<T>.ConcursoPreferencia(string codConcurso)
        {
            var preferencia = new PreferenciaConcurso();

            var query = _tablePrefConcurso
                .Where(PrefConcurso => PrefConcurso.CodConcurso == codConcurso);

            var resultado = await query.ToListAsync();

            foreach (var item in resultado)
            {
                preferencia = item;
            }

            return preferencia;
        }

        async Task<List<string>> IAzureServicePrefConcurso<T>.MeusConcursosPreferidos(PreferenciaUser user)
        {
            var itens = new List<string>();

            var query = _tablePrefConcurso
                .Where(PrefConcurso => (PrefConcurso.Ti == user.Ti && PrefConcurso.Ti == true)
                || (PrefConcurso.Licenciatura == user.Licenciatura && PrefConcurso.Licenciatura == true)
                || (PrefConcurso.Financas == user.Financas && PrefConcurso.Financas == true)
                || (PrefConcurso.Administracao == user.Administracao && PrefConcurso.Administracao == true)
                || (PrefConcurso.Saude == user.Saude && PrefConcurso.Saude == true)
                || (PrefConcurso.Militar == user.Militar && PrefConcurso.Militar == true)
                || (PrefConcurso.Outra == user.Outra && PrefConcurso.Outra == true))
                .Select(PrefConcurso => PrefConcurso.CodConcurso);

            itens = await query.ToListAsync();

            return itens;
        }
    }
}
