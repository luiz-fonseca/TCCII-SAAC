using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    [DataTable("TB_PREFERENCIA_USUARIO")]
    public class PreferenciaUser : Preferencia
    {
        [JsonProperty(PropertyName = "codUsuario")]
        public string CodUsuario { get; set; }

        [Version]
        public string Version { get; set; }

    }
}
