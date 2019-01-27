using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServiceGroup<T> : AzureService<T>, IAzureServiceGroup<T>
    {
        private IMobileServiceTable<Grupo> _tableGroup;

        public AzureServiceGroup()
        {
            _tableGroup = _cliente.GetTable<Grupo>();

        }

        async Task<List<Grupo>> IAzureServiceGroup<T>.MeusGrupos(List<string> meusCodigos)
        {
            List<Grupo> items = new List<Grupo>();

            foreach (var item in meusCodigos)
            {
                var query = _tableGroup
                .Where(Grupo => Grupo.Id == item);

                var resultado = await query.ToListAsync();

                foreach (var aux in resultado)
                {
                    items.Add(aux);
                }

            }

            return items;
        }

        async Task<List<Grupo>> IAzureServiceGroup<T>.PesquisarGrupos(string pesquisarGrupo)
        {
            var itens = new List<Grupo>();

            var query = _tableGroup
                .Where(Grupo => Grupo.Nome.ToLower().Contains(pesquisarGrupo.ToLower()) || 
                                Grupo.Descricao.ToLower().Contains(pesquisarGrupo.ToLower()));

            itens = await query.ToListAsync();

            return itens;

        }

        async Task<List<Grupo>> IAzureServiceGroup<T>.PesquisarMeusGrupos(List<string> meusCodigos, string pesquisarGrupo)
        {
            List<Grupo> items = new List<Grupo>();

            foreach (var item in meusCodigos)
            {
                var query = _tableGroup
                .Where(Grupo => Grupo.Id == item && (Grupo.Nome.ToLower().Contains(pesquisarGrupo.ToLower()) ||
                                                     Grupo.Descricao.ToLower().Contains(pesquisarGrupo.ToLower())));

                var resultado = await query.ToListAsync();

                foreach (var aux in resultado)
                {
                    items.Add(aux);
                }

            }

            return items;
        }
    }
}
