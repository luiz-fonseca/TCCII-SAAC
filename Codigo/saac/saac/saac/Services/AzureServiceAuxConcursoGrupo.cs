using Microsoft.WindowsAzure.MobileServices;
using saac.Interfaces;
using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Services
{
    public class AzureServiceAuxConcursoGrupo<T>: AzureService<T>, IAzureServiceAuxConcursoGrupo<T>
    {
        private IMobileServiceTable<AuxConcursoGrupo> _tableAuxConcursoGrupo;

        public AzureServiceAuxConcursoGrupo()
        {
            _tableAuxConcursoGrupo = _cliente.GetTable<AuxConcursoGrupo>();

        }
    }
}
