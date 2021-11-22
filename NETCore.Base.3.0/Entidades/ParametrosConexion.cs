using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace BCP.NETCore.Base.Entidades
{
    public class ParametrosConexion
    {
        public string Servidor { get; set; }
        public string BaseDatos { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
    }
}
