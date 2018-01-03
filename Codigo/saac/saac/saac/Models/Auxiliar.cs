﻿using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    [DataTable("TB_USUARIO_GRUPO")]
    public class Auxiliar
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "codUsuario")]
        public string CodUsuario { get; set; }

        [JsonProperty(PropertyName = "codGrupo")]
        public string CodGrupo { get; set; }

        [JsonProperty(PropertyName = "administrador")]
        public bool Adiministrador { get; set; }

        [Version]
        public string Version { get; set; }
    }
}