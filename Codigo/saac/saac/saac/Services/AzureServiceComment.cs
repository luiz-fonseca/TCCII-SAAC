using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServiceComment<T> : AzureService<T>, IAzureServiceComment<T>
    {
        private IMobileServiceTable<Comentario> _tableComment;

        public AzureServiceComment()
        {
            _tableComment = _cliente.GetTable<Comentario>();

        }

        async Task<List<Comentario>> IAzureServiceComment<T>.Comentarios(string codPublicacao)
        {
            List<Comentario> itens = new List<Comentario>();

            var query = _tableComment
                .Where(Comentario => Comentario.CodPublicacao == codPublicacao);

            itens = await query.ToListAsync();

            return itens;
        }

        async Task<int> IAzureServiceComment<T>.MeuCometario(string codComentario, string codUsuario)
        {
            var query = _tableComment
                 .Where(Comentario => Comentario.Id == codComentario && Comentario.CodUsuario == codUsuario);

            var aux = await query.ToListAsync();
            var resultado = aux.Count;

            return resultado;
        }
    }
}
