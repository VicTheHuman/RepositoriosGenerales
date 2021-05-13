using System;
using System.Collections.Generic;

#nullable disable

namespace UnitTestProjectEF.Models
{
    public partial class Personal
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int? Edad { get; set; }
    }
}
