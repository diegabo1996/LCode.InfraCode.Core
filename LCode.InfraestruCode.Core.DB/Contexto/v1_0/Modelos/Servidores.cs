using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LCode.InfraestruCode.Core.BD.Contexto.v1_0.Modelos
{
        public class ServidoresSolicitud
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public string GuidSolicitud { get; set; }
            [Required]
            [Column(TypeName = "varchar(8)")]
            [MaxLength(8), MinLength(1)]
            [DisplayName("Nombre del Servidor")]
            public string Servidor { get; set; }
            [DisplayName("Tipo de Solicitud")]
            public TipoSolicitud TipoSolicitud { get; set; }
            [Column(TypeName = "varchar(150)")]
            [MaxLength(150), MinLength(0)]
            [DisplayName("Observaciones")]
            public string Observacion { get; set; }
            [Column(TypeName = "varchar(50)")]
        [DisplayName("Usuario de Solicitud")]
        public string UsuarioSolicita { get; set; }
        [DisplayName("Fecha y Hora Solicitud")]
        public DateTime FechaHoraRegistro { get; set; }
        [DisplayName("Estado de Solicitud")]
        public EstadoSolicitud EstadoSolicitud { get; set; }
            [Column(TypeName = "varchar(max)")]
            public string Mensaje { get; set; }
        [DisplayName("Ultima Modificación")]
        public DateTime FechaHoraUltimaModificacion { get; set; }
        }
        public enum EstadoSolicitud
        {
            Solicitud_Iniciada,
            Solicitud_Procesando,
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
