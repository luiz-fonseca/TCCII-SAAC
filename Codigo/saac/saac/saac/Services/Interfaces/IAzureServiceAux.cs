﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services.Interfaces
{
    public interface IAzureServiceAux<T>: IAzureServiceBase<T>
    {
        Task<List<string>> MeusGrupos(string id);
    }
}