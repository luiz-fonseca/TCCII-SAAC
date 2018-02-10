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
        public bool Ti { get; set; } = false;

        [JsonProperty(PropertyName = "saude")]
        public bool Saude { get; set; } = false;

        [JsonProperty(PropertyName = "licenciatura")]
        public bool Licenciatura { get; set; } = false;

        [JsonProperty(PropertyName = "financas")]
        public bool Financas { get; set; } = false;

        [JsonProperty(PropertyName = "administracao")]
        public bool Administracao { get; set; } = false;

        [JsonProperty(PropertyName = "militar")]
        public bool Militar { get; set; } = false;


    }
}
