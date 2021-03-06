﻿using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Prism.Mvvm;
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

        
        private DateTime _dtNasci;
        [JsonProperty(PropertyName = "dtNasci")]
        public DateTime DtNasci
        {
            get { return _dtNasci; }
            set { _dtNasci = value; }
        }

        [JsonProperty(PropertyName = "sexo")]
        public bool Sexo { get; set; }

        [JsonProperty(PropertyName = "endereco")]
        public string Endereco { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "administrador")]
        public bool Administrador { get; set; }

        [Version]
        public string Version { get; set; }


    }
}
