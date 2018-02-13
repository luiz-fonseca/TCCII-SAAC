using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    [DataTable("TB_GRUPO")]
    public class Grupo: BindableBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        private string _nome;
        [JsonProperty(PropertyName = "nome")]
        public string Nome
        {
            get { return _nome; }
            set { SetProperty(ref _nome, value); }
        }
        
        private string _descricao;
        [JsonProperty(PropertyName = "descricao")]
        public string Descricao
        {
            get { return _descricao; }
            set { SetProperty(ref _descricao, value); }
        }

        private string _categoria;
        [JsonProperty(PropertyName = "categoria")]
        public string Categoria
        {
            get { return _categoria; }
            set { SetProperty(ref _categoria, value); }
        }

        [JsonProperty(PropertyName = "temporario")]
        public bool Temporario { get; set; }

        [Version]
        public string Version { get; set; }

    }
}
