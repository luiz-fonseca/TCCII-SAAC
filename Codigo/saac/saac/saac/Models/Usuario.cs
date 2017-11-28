using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    public class Usuario
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Foto { get; set; }
        public DateTime DtNasci { get; set; }
        public bool Sexo { get; set; }
        public string Endereco { get; set; }

    }
}
