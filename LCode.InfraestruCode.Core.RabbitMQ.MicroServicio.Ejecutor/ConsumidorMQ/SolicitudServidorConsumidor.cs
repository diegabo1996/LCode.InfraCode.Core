using BCP.NETCore.Base;
using LCode.InfraestruCode.Core.BD.Contexto.v1_0;
using LCode.InfraestruCode.Core.BD.Contexto.v1_0.Modelos;
using LCode.InfraestruCode.Core.RabbitMQ.MicroServicio.Ejecutor.Comandos.VM;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LCode.InfraestruCode.Core.RabbitMQ.MicroServicio.Ejecutor.ConsumidorMQ
{
    public class SolicitudServidorConsumidor: IConsumer<ServidoresSolicitud>
    {
        private readonly LCODEContexto _context = new LCODEContexto();
        public IEstadoEnergia EstadoEnergia { get; set; }
        public Task Consume(ConsumeContext<ServidoresSolicitud> context)
        {
            RegistroLogs Logs = new RegistroLogs();
            try
            {
                var data = context.Message;
                Logs.RegistrarEvento(TipoEvento.Informativo, "Mensaje Captado!",data);
                data.EstadoSolicitud = EstadoSolicitud.Solicitud_Procesando;
                data.FechaHoraUltimaModificacion = DateTime.Now;
                data.Mensaje = "Procesando solicitud...";
                _context.Entry(data).State = EntityState.Modified;
                _context.SaveChanges();
                EstadoEnergia = new EstadoEnergia();
                switch (data.TipoSolicitud)
                {
                    case TipoSolicitud.Solicitud_Apagado:
                        EstadoEnergia.Detener(data);
                        break;
                    case TipoSolicitud.Solicitud_Encendido:
                        EstadoEnergia.Iniciar(data);
                        break;
                    case TipoSolicitud.Solicitud_Reinicio:
                        EstadoEnergia.Reiniciar(data);
                        break;
                }
                return Task.CompletedTask;
            }
            catch (Exception Ex)
            {
                Logs.RegistrarEvento(TipoEvento.Error, Ex);
                return Task.CompletedTask;
            }
        }

    }
}
