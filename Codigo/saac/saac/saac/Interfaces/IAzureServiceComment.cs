﻿using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Interfaces
{
    public interface IAzureServiceComment<T> : IAzureServiceBase<T>
    {
        Task<List<Comentario>> Comentarios(string codPublicacao);
        Task<int> MeuCometario(string codComentario, string codUsuario);
        Task<int> QtdComentariosPendentes(string codPublicacao, DateTime dataVisualizacao, string codUsuario);

    }
}
