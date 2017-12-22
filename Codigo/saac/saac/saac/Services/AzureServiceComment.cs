using Microsoft.WindowsAzure.MobileServices;
using saac.Models;
using saac.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Services
{
    public class AzureServiceComment<T> : AzureService<T>, IAzureServiceComment<T>
    {
        private IMobileServiceTable<Comentario> _tableComment;

        public AzureServiceComment()
        {
            _tableComment = _cliente.GetTable<Comentario>();

        }
    }
}
