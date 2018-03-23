using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    public class Preferencia: BindableBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        private bool _ti;
        [JsonProperty(PropertyName = "ti")]
        public bool Ti
        {
            get { return _ti; }
            set { SetProperty(ref _ti, value); }
        }

        public bool _saude;
        [JsonProperty(PropertyName = "saude")]
        public bool Saude
        {
            get { return _saude; }
            set { SetProperty(ref _saude, value); }
        }

        public bool _licenciatura;
        [JsonProperty(PropertyName = "licenciatura")]
        public bool Licenciatura
        {
            get { return _licenciatura; }
            set { SetProperty(ref _licenciatura, value); }
        }

        public bool _financas;
        [JsonProperty(PropertyName = "financas")]
        public bool Financas
        {
            get { return _financas; }
            set { SetProperty(ref _financas, value); }
        }

        public bool _administracao;
        [JsonProperty(PropertyName = "administracao")]
        public bool Administracao
        {
            get { return _administracao; }
            set { SetProperty(ref _administracao, value); }
        }

        public bool _militar;
        [JsonProperty(PropertyName = "militar")]
        public bool Militar
        {
            get { return _militar; }
            set { SetProperty(ref _militar, value); }
        }

    }
}
