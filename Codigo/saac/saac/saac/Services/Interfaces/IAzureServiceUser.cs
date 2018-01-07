using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services.Interfaces
{
    public interface IAzureServiceUser<T> : IAzureServiceBase<T>
    {
        Task<List<Usuario>> Usuarios(List<string> codUser);
    }
}
