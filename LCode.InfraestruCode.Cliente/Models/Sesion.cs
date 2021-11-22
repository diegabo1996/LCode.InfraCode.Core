using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCode.InfraestruCode.Cliente.Models
{
    public class Sesion
    {
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
    }
    public class DatosSesion
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
    }
}
