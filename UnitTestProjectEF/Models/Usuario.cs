using System;
using System.Collections.Generic;

#nullable disable

namespace UnitTestProjectEF.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public int Nivel { get; set; }
        public bool Activo { get; set; }
    }
}
