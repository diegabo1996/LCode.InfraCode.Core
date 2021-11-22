using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Base._3._0.Entidades
{
    public class ServicioWindows
    {
        public bool Remoto { get; set; }
        public CredencialesWindows CerdencialesWindows { get; set; }
        public string Servidor { get; set; }
        public string NombreServicio { get; set; }
    }
    public class RespuestaServicioWindows
    { 
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
    }
}
