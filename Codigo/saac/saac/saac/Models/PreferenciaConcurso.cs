using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    [DataTable("TB_PREFERENCIA_CONCURSO")]
    public class PreferenciaConcurso : Preferencia
    {
        [JsonProperty(PropertyName = "codConcurso")]
        public string CodConcurso { get; set; }

        [Version]
        public string Version { get; set; }

    }
}
