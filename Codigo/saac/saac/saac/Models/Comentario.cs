﻿using System;
using System.Collections.Generic;
using System.Text;

namespace saac.Models
{
    public class Comentario
    {
        public int CodComentario { get; set; }
        public int CodPublicaco { get; set; }
        public int CodGrupo { get; set; }
        public int Cod_Pessoa { get; set; }
        public string Foto { get; set; }
        public string Texto { get; set; }
    }
}