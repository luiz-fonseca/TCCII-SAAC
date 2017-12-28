using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServiceGroup<T>: AzureService<T>, IAzureServiceGroup<T>
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
    }
}
