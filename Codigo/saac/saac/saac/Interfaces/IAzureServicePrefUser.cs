using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Interfaces
{
    public interface IAzureServicePrefUser<T> : IAzureServiceBase<T>
    {
        Task<PreferenciaUser> MinhasPreferencias(string codUsuario);
    }
}
