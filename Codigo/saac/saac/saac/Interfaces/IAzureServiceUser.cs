﻿using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Interfaces
{
    public interface IAzureServiceUser<T> : IAzureServiceBase<T>
    {
        Task<List<Usuario>> Usuarios(List<string> codUser);
        Task<int> ExisteUsuario(string codUser);
        Task<bool> VerificarAdministrador(string codUser);
        Task<Usuario> UsuarioSelecionado(string codUser);

    }
}
