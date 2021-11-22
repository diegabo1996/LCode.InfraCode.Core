using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCore.Base._3._0.Entidades
{
    public class PowerShellEntidad
    {
        public class Respuesta
        {
            public bool Exito { get; set; }
            public List<string> Respuestas { get; set; }
            public List<string> Errores { get; set; }
        }
    }
}
