using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    public class Facebook
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
        
        private DateTime _dtNascimento;
        [JsonProperty("birthday")]
        public DateTime DtNascimento
        {
            get { return _dtNascimento; }
            set { _dtNascimento = value; }
        }

        [JsonProperty("picture")]
        public Picture Picture { get; set; }

    }

    public class Picture
    {
        public Data Data { get; set; }

    }

    public class Data
    {
        public bool IsSilhouette { get; set; }
        public string Url { get; set; }
    }

}
