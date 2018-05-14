using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServicePublication<T> : AzureService<T>, IAzureServicePublication<T>
    {
        private IMobileServiceTable<Publicacao> _tablePublication;

        public AzureServicePublication()
        {
            _tablePublication = _cliente.GetTable<Publicacao>();

        }

        async Task<int> IAzureServicePublication<T>.MinhaPublicaco(string codPublicao, string CodUsuario)
        {
            var query = _tablePublication
                .Where(Publicacao => Publicacao.Id == codPublicao && Publicacao.CodUsuario == CodUsuario);

            var aux = await query.ToListAsync();
            var resultado = aux.Count;

            return resultado;
        }

       
        async Task<List<Publicacao>> IAzureServicePublication<T>.Publicacoes(string codGrupo)
        {
            List<Publicacao> itens = new List<Publicacao>();

            var query = _tablePublication
                .Where(Publicacao => Publicacao.CodGrupo == codGrupo);

            itens = await query.ToListAsync();

            return itens;

        }

        async Task<int> IAzureServicePublication<T>.QtdPublicacoesPendentes(string codGrupo, string codUsuario, DateTime dataVerificacao)
        {
            var query = _tablePublication
                .Where(Publicacao => Publicacao.CodGrupo == codGrupo && Publicacao.DtPublicacao > dataVerificacao
                       && Publicacao.CodUsuario != codUsuario);

            var aux = await query.ToListAsync();
            var resultado = aux.Count;

            return resultado;

        }

        async Task<List<Publicacao>> IAzureServicePublication<T>.MinhasPublicacoes(string codUsuario)
        {
            var query = _tablePublication
                .Where(Publicacao => Publicacao.CodUsuario == codUsuario);

            var resultado = await query.ToListAsync();

            return resultado;
        }

    }
}
