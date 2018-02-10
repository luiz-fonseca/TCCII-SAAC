﻿using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    [DataTable("TB_CONCURSO")]
    public class Concurso: BindableBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        private string _titulo;
        [JsonProperty(PropertyName = "titulo")]
        public string Titulo
        {
            get { return _titulo; }
            set { SetProperty(ref _titulo, value); }
        }

        private string _descricao;
        [JsonProperty(PropertyName = "descricao")]
        public string Descricao
        {
            get { return _descricao; }
            set { SetProperty(ref _descricao, value); }
        }

        private string _detalhes;
        [JsonProperty(PropertyName = "detalhes")]
        public string Detalhes
        {
            get { return _detalhes; }
            set { SetProperty(ref _detalhes, value); }
        }


        //[JsonProperty(PropertyName = "estado")]
        //public string Estado { get; set; }

        //[JsonProperty(PropertyName = "regioes")]
        //public string Regioes { get; set; }


        [Version]
        public string Version { get; set; }
    }
}
