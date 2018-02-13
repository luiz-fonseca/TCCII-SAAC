using Microsoft.WindowsAzure.MobileServices;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServicePrefUser<T> : AzureService<T>, IAzureServicePrefUser<T>
    {
        private IMobileServiceTable<PreferenciaUser> _tablePrefUser;

        public AzureServicePrefUser()
        {
            _tablePrefUser = _cliente.GetTable<PreferenciaUser>();

        }

        async Task<PreferenciaUser> IAzureServicePrefUser<T>.MinhasPreferencias(string codUsuario)
        {
            var resultado = new PreferenciaUser();

            var query = _tablePrefUser
               .Where(PreferenciaUser => PreferenciaUser.CodUsuario == codUsuario);

            var aux = await query.ToListAsync();

            foreach (var item in aux)
            {
                resultado = item;
            }

            return resultado;
        }
    }
}
