using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    [DataTable("TB_COMENTARIO")]
    public class Comentario
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "codPublicacao")]
        public int CodPublicaco { get; set; }

        [JsonProperty(PropertyName = "codGrupo")]
        public int CodGrupo { get; set; }

        [JsonProperty(PropertyName = "codPessoa")]
        public int CodPessoa { get; set; }

        [JsonProperty(PropertyName = "foto")]
        public string Foto { get; set; }

        [JsonProperty(PropertyName = "texto")]
        public string Texto { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
