using System;
using System.Collections.Generic;
using System.IO;
using NETCore.Base._3._0.Auxiliares.PowerShell;
using System.Linq;
using System.Threading.Tasks;
using LCode.InfraestruCode.Core.BD.Contexto.v1_0.Modelos;
using NETCore.Base._3._0;
using LCode.InfraestruCode.Core.BD.Contexto.v1_0;
using Microsoft.EntityFrameworkCore;

namespace LCode.InfraestruCode.Core.RabbitMQ.MicroServicio.Ejecutor.Comandos.VM
{
    public interface IEstadoEnergia
    {
        void Iniciar(ServidoresSolicitud SolicitudVM);
        void Reiniciar(ServidoresSolicitud SolicitudVM);
        void Detener(ServidoresSolicitud SolicitudVM);
    }

    public class EstadoEnergia:IEstadoEnergia
    {
        private readonly string Servidor;
        private readonly string Usuario;
        private readonly string Contrasenia;
        private readonly LCODEContexto _context = new LCODEContexto();


        public EstadoEnergia()
        {
            BaseConfiguracion BC = new BaseConfiguracion();
            Servidor = BC.ObtenerValor("VMWare:Configuracion:Servidor");
            Usuario = BC.ObtenerValor("VMWare:Configuracion:Usuario");
            Contrasenia = BC.ObtenerValor("VMWare:Configuracion:Contrasenia");
        }
        public void Iniciar(ServidoresSolicitud SolicitudVM)
        {
            string scriptX = File.ReadAllText(@".\VM-Scripts\VM-Iniciar.ps1");
            scriptX = scriptX.Replace("$Servidor", Servidor).Replace("$Usuario", Usuario).Replace("$Contrasenia", Contrasenia).Replace("$NombreVM", SolicitudVM.Servidor);
            NETCore.Base._3._0.Auxiliares.PowerShell.Ejecutor ej = new NETCore.Base._3._0.Auxiliares.PowerShell.Ejecutor();
            var Resultado = ej.EjecucionCompleja(scriptX);
            if (!Resultado.Exito)
            {
                SolicitudVM.EstadoSolicitud = EstadoSolicitud.Solicitud_Error;
            }
            else
            {
                SolicitudVM.EstadoSolicitud = EstadoSolicitud.Solicitud_Procesada;
            }
            foreach (string Mensaje in Resultado.Respuestas)
            {
                SolicitudVM.Mensaje += "Info: " + Mensaje;
                SolicitudVM.Mensaje += Environment.NewLine;
            }

            foreach (string Mensaje in Resultado.Errores)
            {
                SolicitudVM.Mensaje += "Error: " + Mensaje;
                SolicitudVM.Mensaje += Environment.NewLine;
            }
            SolicitudVM.FechaHoraUltimaModificacion = DateTime.Now;
            _context.Entry(SolicitudVM).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Reiniciar(ServidoresSolicitud SolicitudVM)
        {
            string scriptX = File.ReadAllText(@".\VM-Scripts\VM-Reiniciar.ps1");
            scriptX = scriptX.Replace("$Servidor", Servidor).Replace("$Usuario", Usuario).Replace("$Contrasenia", Contrasenia).Replace("$NombreVM", SolicitudVM.Servidor);
            NETCore.Base._3._0.Auxiliares.PowerShell.Ejecutor ej = new NETCore.Base._3._0.Auxiliares.PowerShell.Ejecutor();
            var Resultado = ej.EjecucionCompleja(scriptX);
            if (!Resultado.Exito)
            {
                SolicitudVM.EstadoSolicitud = EstadoSolicitud.Solicitud_Error;
            }
            else
            {
                SolicitudVM.EstadoSolicitud = EstadoSolicitud.Solicitud_Procesada;
            }
            foreach (string Mensaje in Resultado.Respuestas)
            {
                SolicitudVM.Mensaje += "Info: " + Mensaje;
                SolicitudVM.Mensaje += Environment.NewLine;
            }

            foreach (string Mensaje in Resultado.Errores)
            {
                SolicitudVM.Mensaje += "Error: " + Mensaje;
                SolicitudVM.Mensaje += Environment.NewLine;
            }
            SolicitudVM.FechaHoraUltimaModificacion = DateTime.Now;
            _context.Entry(SolicitudVM).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Detener(ServidoresSolicitud SolicitudVM)
        {
            string scriptX = File.ReadAllText(@".\VM-Scripts\VM-Detener.ps1");
            scriptX = scriptX.Replace("$Servidor", Servidor).Replace("$Usuario", Usuario).Replace("$Contrasenia", Contrasenia).Replace("$NombreVM", SolicitudVM.Servidor);
            NETCore.Base._3._0.Auxiliares.PowerShell.Ejecutor ej = new NETCore.Base._3._0.Auxiliares.PowerShell.Ejecutor();
            var Resultado = ej.EjecucionCompleja(scriptX);
            if (!Resultado.Exito)
            {
                SolicitudVM.EstadoSolicitud = EstadoSolicitud.Solicitud_Error;
            }
            else
            {
                SolicitudVM.EstadoSolicitud = EstadoSolicitud.Solicitud_Procesada;
            }
            foreach (string Mensaje in Resultado.Respuestas)
            {
                SolicitudVM.Mensaje += "Info: " + Mensaje;
                SolicitudVM.Mensaje += Environment.NewLine;
            }

            foreach (string Mensaje in Resultado.Errores)
            {
                SolicitudVM.Mensaje += "Error: " + Mensaje;
                SolicitudVM.Mensaje += Environment.NewLine;
            }
            SolicitudVM.FechaHoraUltimaModificacion = DateTime.Now;
            _context.Entry(SolicitudVM).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
