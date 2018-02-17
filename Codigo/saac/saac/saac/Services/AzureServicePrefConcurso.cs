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

        async Task<List<string>> IAzureServicePrefConcurso<T>.MeusConcursosPreferidos(PreferenciaUser user)
        {
            var itens = new List<string>();

            var query = _tablePrefConcurso
                .Where(PrefConcurso => (PrefConcurso.Ti == user.Ti && PrefConcurso.Ti == true)
                || (PrefConcurso.Licenciatura == user.Licenciatura && PrefConcurso.Licenciatura == true)
                || (PrefConcurso.Financas == user.Financas && PrefConcurso.Financas == true)
                || (PrefConcurso.Administracao == user.Administracao && PrefConcurso.Administracao == true)
                || (PrefConcurso.Saude == user.Saude && PrefConcurso.Saude == true)
                || (PrefConcurso.Militar == user.Militar && PrefConcurso.Militar == true))
                .Select(PrefConcurso => PrefConcurso.CodConcurso);

            itens = await query.ToListAsync();

            return itens;
        }
    }
}
