using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using saac.Models;

namespace saac.Services
{
    public class AzureService
    {
        private IMobileServiceClient _cliente;
        private IMobileServiceTable<Usuario> _tableUsuario;
        private IMobileServiceTable<Comentario> _tableComentario;
        private IMobileServiceTable<Grupo> _tableGrupo;
        private IMobileServiceTable<Publicacao> _tablePublicaco;
        private const string serviceUri = "http://saac.azurewebsites.net";


        public AzureService()
        {
            _cliente = new MobileServiceClient(serviceUri);
            _tableUsuario = _cliente.GetTable<Usuario>();
           // _tableComentario = _cliente.GetTable<Comentario>();
            //_tableGrupo = _cliente.GetTable<Grupo>();
            //_tablePublicaco = _cliente.GetTable<Publicacao>();

        }

        public async Task<IEnumerable<Usuario>> GetUsuario()
        {
            return await _tableUsuario.ToEnumerableAsync();
         
        }

        public async void addUsuario(Usuario user)
        {
            await _tableUsuario.InsertAsync(user);
        }
        

        
    }
}
