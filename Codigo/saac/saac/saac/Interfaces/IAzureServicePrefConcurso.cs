﻿using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Interfaces
{
    public interface IAzureServicePrefConcurso<T> : IAzureServiceBase<T>
    {
        Task<List<string>> MeusConcursosPreferidos(PreferenciaUser user);
        Task<PreferenciaConcurso> ConcursoPreferencia(string codConcurso);
    }
}
