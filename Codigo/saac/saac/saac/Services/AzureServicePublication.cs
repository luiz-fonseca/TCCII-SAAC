using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServicePublication<T>: AzureService<T>, IAzureServicePublication<T>
    {
        private IMobileServiceTable<Publicacao> _tablePublication;

        public AzureServicePublication()
        {
            _tablePublication = _cliente.GetTable<Publicacao>();

        }

        async Task<List<Publicacao>> IAzureServicePublication<T>.Publicacoes(string codGrupo)
        {
            List<Publicacao> itens = new List<Publicacao>();

            var query = _tablePublication
                .Where(Publicacao => Publicacao.CodGrupo == codGrupo);

            itens = await query.ToListAsync();

            return itens;

        }
    }
}
