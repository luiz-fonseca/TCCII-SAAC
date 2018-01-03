using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    [DataTable("TB_PUBLICACAO")]
    public class Publicacao
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "codGrupo")]
        public string CodGrupo { get; set; }

        [JsonProperty(PropertyName = "codPessoa")]
        public string CodPessoa { get; set; }

        [JsonProperty(PropertyName = "texto")]
        public string Texto { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
