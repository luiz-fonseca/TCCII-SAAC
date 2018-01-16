﻿using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServiceAux<T> : AzureService<T>, IAzureServiceAux<T>
    {
        private IMobileServiceTable<Auxiliar> _tableAux;

        public AzureServiceAux()
        {
            _tableAux = _cliente.GetTable<Auxiliar>();

        }

        async Task<int> IAzureServiceAux<T>.ExisteSeguirAux(string idGrupo, string idUser)
        {
            var query = _tableAux
                .Where(Auxiliar => Auxiliar.CodGrupo == idGrupo && Auxiliar.CodUsuario == idUser);

            var aux = await query.ToListAsync();
            int resultado = aux.Count;

            return resultado;
        }

        async Task<List<string>> IAzureServiceAux<T>.MeusGrupos(string id)
        {
            List<string> items = new List<string>();

            var query = _tableAux
                .Where(Auxiliar => Auxiliar.CodUsuario == id)
                .Select(Auxiliar => Auxiliar.CodGrupo);

            items = await query.ToListAsync();

            return items;
        }
    }
}
