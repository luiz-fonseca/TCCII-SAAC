﻿using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services
{
    public class AzureServiceUser<T> : AzureService<T>, IAzureServiceUser<T>
    {
        private IMobileServiceTable<Usuario> _tableUser;

        public AzureServiceUser()
        {
            _tableUser = _cliente.GetTable<Usuario>();

        }

        async Task<List<Usuario>> IAzureServiceUser<T>.Usuarios(List<string> codUser)
        {
            List<Usuario> itens = new List<Usuario>();

            foreach (var item in codUser)
            {
                var query = _tableUser
                .Where(Usuario => Usuario.Id == item);

                var resultado = await query.ToListAsync();

                foreach (var aux in resultado)
                {
                    itens.Add(aux);
                }

            }

            return itens;
        }

        async Task<int> IAzureServiceUser<T>.ExisteUsuario(string codUser)
        {
            var query = _tableUser
            .Where(Usuario => Usuario.Id == codUser);

            var aux = await query.ToListAsync();
            var resultado = aux.Count;

            return resultado;
        }

        async Task<bool> IAzureServiceUser<T>.VerificarAdministrador(string codUser)
        {
            bool resultado = false;

            var query = _tableUser
                 .Where(Usuario => Usuario.Id == codUser)
                 .Select(Usuario => Usuario.Administrador);

            var aux = await query.ToListAsync();

            foreach (var item in aux)
            {
                resultado = item;
            }

            return resultado;
        }

        async Task<Usuario> IAzureServiceUser<T>.UsuarioSelecionado(string codUser)
        {
            var resultado = new Usuario();

            var query = _tableUser
                .Where(Usuario => Usuario.Id == codUser);

            var aux = await query.ToListAsync();

            foreach (var item in aux)
            {
                resultado = item;

            }
            return resultado;

        }
    }
}
