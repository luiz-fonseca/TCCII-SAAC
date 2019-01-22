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

        [JsonProperty(PropertyName = "codUsuario")]
        public string CodUsuario { get; set; }

        [JsonProperty(PropertyName = "texto")]
        public string Texto { get; set; }

        [JsonProperty(PropertyName = "resolvido")]
        public bool Resolvido { get; set; }

        private DateTime _dtPublicacao;
        [JsonProperty(PropertyName = "dtPublicacao")]
        public DateTime DtPublicacao
        {
            get { return _dtPublicacao; }
            set { _dtPublicacao = value; }
        }

        private DateTime _dtVisualizacao;
        [JsonProperty(PropertyName = "dtVisualizacao")]
        public DateTime DtVisualizacao
        {
            get { return _dtVisualizacao; }
            set { _dtVisualizacao = value; }
        }

        [Version]
        public string Version { get; set; }
    }
}
