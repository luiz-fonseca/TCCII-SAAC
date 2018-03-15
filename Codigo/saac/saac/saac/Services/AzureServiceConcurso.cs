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

        async Task<List<Concurso>> IAzureServiceConcurso<T>.ConcursosEmEspera(DateTime dataAutual)
        {
            var itens = new List<Concurso>();

            var query = _tableConcurso
                .Where(Concurso => Concurso.DtRealizacao >= dataAutual && Concurso.Visibilidade == false);

            itens = await query.ToListAsync();

            return itens;

        }

        async Task<List<Concurso>> IAzureServiceConcurso<T>.ConcursosFinalizados(DateTime dataAtual)
        {
            var itens = new List<Concurso>();

            var query = _tableConcurso
                .Where(Concurso => Concurso.DtRealizacao < dataAtual && Concurso.Visibilidade == false);

            itens = await query.ToListAsync();

            return itens;

        }

        async Task<List<Concurso>> IAzureServiceConcurso<T>.InscricoesFinalizadas(DateTime dataAtual)
        {
            var itens = new List<Concurso>();

            var query = _tableConcurso
                .Where(Concurso => Concurso.DtInscricao < dataAtual && Concurso.Visibilidade == true);

            itens = await query.ToListAsync();

            return itens;

        }

        async Task<List<Concurso>> IAzureServiceConcurso<T>.MeusConcursos(List<string> codConcurso)
        {
            var concursos = new List<Concurso>();

            foreach (var item in codConcurso)
            {
                var query = _tableConcurso
                    .Where(Concurso => Concurso.Id == item && Concurso.Visibilidade == true);

                var resultado = await query.ToListAsync();

                foreach (var aux in resultado)
                {
                    concursos.Add(aux);
                }

            }

            return concursos;
        }
    }
}
