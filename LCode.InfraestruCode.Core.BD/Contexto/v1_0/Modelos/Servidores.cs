using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LCode.InfraestruCode.Core.BD.Contexto.v1_0.Modelos
{
    public class Servidores
    {
        public class Solicitud
        {
            [Key]
            public string GuidSolicitud { get; set; }
            [Required]
            [Column(TypeName = "varchar(5)")]
            public string Servidor { get; set; }
            public TipoSolicitud TipoSolicitud { get; set; }
            [Column(TypeName = "varchar(150)")]
            public string Observacion { get; set; }
            [Column(TypeName = "varchar(50)")]
            public string UsuarioSolicita { get; set; }
            public DateTime FechaHoraRegistro { get; set; }
            public EstadoSolicitud EstadoSolicitud { get; set; }
            [Column(TypeName = "varchar(max)")]
            public string Mensaje { get; set; }
            public DateTime FechaHoraUltimaModificacion { get; set; }
        }
        public enum EstadoSolicitud
        {
            Solicitud_Iniciada,
            Solicitud_Error,
            Solicitud_Procesada
        }
        public enum TipoSolicitud
        {
            Solicitud_Encendido,
            Solicitud_Reinicio,
            Solicitud_Apagado
        }
    }
}
