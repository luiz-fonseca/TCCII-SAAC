using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    public class Preferencia
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "ti")]
        public bool Ti { get; set; }

        [JsonProperty(PropertyName = "saude")]
        public bool Saude { get; set; }

        [JsonProperty(PropertyName = "licenciatura")]
        public bool Licenciatura { get; set; }

        [JsonProperty(PropertyName = "financas")]
        public bool Financas { get; set; }

        [JsonProperty(PropertyName = "administracao")]
        public bool Administracao { get; set; }

        [JsonProperty(PropertyName = "militar")]
        public bool Militar { get; set; }


    }
}
