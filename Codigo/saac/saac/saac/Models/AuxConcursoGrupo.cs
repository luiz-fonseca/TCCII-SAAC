using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    [DataTable("TB_CONCURSO_GRUPO")]
    public class AuxConcursoGrupo
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "codConcurso")]
        public string CodConcurso { get; set; }

        [JsonProperty(PropertyName = "codGrupo")]
        public string CodGrupo { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
