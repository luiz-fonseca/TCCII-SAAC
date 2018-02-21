using Microsoft.WindowsAzure.MobileServices;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServiceConcurso<T> : AzureService<T>, IAzureServiceConcurso<T>
    {
        private IMobileServiceTable<Concurso> _tableConcurso;

        public AzureServiceConcurso()
        {
            _tableConcurso = _cliente.GetTable<Concurso>();

        }

        async Task<List<Concurso>> IAzureServiceConcurso<T>.ConcursosDisponiveis(string regiao)
        {
            var itens = new List<Concurso>();

            var query = _tableConcurso
                .Where(Concurso => Concurso.Regiao == regiao && Concurso.Visibilidade == true);

            itens = await query.ToListAsync();

            return itens;

        }
    }
}
