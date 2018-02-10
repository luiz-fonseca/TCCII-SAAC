using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    [DataTable("TB_USUARIO")]
    public class Usuario
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "nome")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "foto")]
        public string Foto { get; set; }

        [JsonProperty(PropertyName = "dtNasci")]
        public DateTime DtNasci { get; set; }

        [JsonProperty(PropertyName = "sexo")]
        public bool Sexo { get; set; }

        [JsonProperty(PropertyName = "endereco")]
        public string Endereco { get; set; }

        //[JsonProperty(PropertyName = "endereco")]
        //public bool Administrador { get; set; }

        [Version]
        public string Version { get; set; }


    }
}
