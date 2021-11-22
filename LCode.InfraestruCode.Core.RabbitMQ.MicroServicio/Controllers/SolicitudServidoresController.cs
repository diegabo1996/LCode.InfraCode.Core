using LCode.InfraestruCode.Core.BD.Contexto.v1_0.Modelos;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using NETCore.Base._3._0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LCode.InfraestruCode.Core.RabbitMQ.MicroServicio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudServidoresController : ControllerBase
    {
        private readonly IBus _bus;
        public SolicitudServidoresController(IBus bus)
        {
            _bus = bus;
        }
        // POST api/<SolicitudServidoresController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ServidoresSolicitud Solicitud)
        {
            if (Solicitud != null)
            {
                BaseConfiguracion BC = new BaseConfiguracion();
                Api Api = new Api();
                string ServicioVivo=Api.GetRaw(BC.ObtenerValor("ConfigMQ:ServicioEjecutor"));
                if (ServicioVivo!=null)
                { 
                    string ServidorMQ = BC.ObtenerValor("ConfigMQ:RabbitMQ:Servidor");
                    string MQ = BC.ObtenerValor("ConfigMQ:RabbitMQ:Cola");
                    Uri uri = new Uri("rabbitmq://"+ServidorMQ+"/"+MQ+"");
                    var endPoint = await _bus.GetSendEndpoint(uri);
                    await endPoint.Send<ServidoresSolicitud>(Solicitud);
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

    }
}
