using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using BCP.NETCore.Base;
using NETCore.Base._3._0.Entidades;

namespace NETCore.Base._3._0
{
    public class WindowsServiceManager
    {
        RegistroLogs Logs = new RegistroLogs();
        RespuestaServicioWindows Respuesta;
        public RespuestaServicioWindows IniciarServicio(ServicioWindows Servicio)
        {
            ServiceController service = new ServiceController(Servicio.NombreServicio,Servicio.Servidor);
            Respuesta = new RespuestaServicioWindows();
            try
            {
                if (service.Status != ServiceControllerStatus.Running)
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(30000);
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        Respuesta.Exito = true;
                        Respuesta.Mensaje = "Se ha iniciado el servicio " + Servicio.NombreServicio + " de manera correcta";
                        Logs.RegistrarEvento(TipoEvento.Informativo, Servicio, Respuesta);
                    }
                    else
                    {
                        Respuesta.Exito = false;
                        Respuesta.Mensaje = "Ha ocurrido un error al iniciar el servicio " + Servicio.NombreServicio + ", por favor revise.";
                        Logs.RegistrarEvento(TipoEvento.Advertencia, Servicio, Respuesta);
                    }
                }
                else
                {
                    Respuesta.Exito = true;
                    Respuesta.Mensaje = "El servicio " + Servicio.NombreServicio + " ya se encuentra en estado 'INICIADO'.";
                    Logs.RegistrarEvento(TipoEvento.Advertencia, Servicio, Respuesta);
                }
            }
            catch (Exception Ex)
            {
                Respuesta.Exito = false;
                Respuesta.Mensaje = "Se ha presentado un error: "+Ex.ToString();
                Logs.RegistrarEvento(TipoEvento.Error, Servicio, Ex);
            }
            return Respuesta;
        }
        public RespuestaServicioWindows DetenerServicio(ServicioWindows Servicio)
        {
            ServiceController service = new ServiceController(Servicio.NombreServicio, Servicio.Servidor);
            Respuesta = new RespuestaServicioWindows();
            try
            {
                if (service.Status != ServiceControllerStatus.Stopped)
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(30000);
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    if (service.Status == ServiceControllerStatus.Stopped)
                    {
                        Respuesta.Exito = true;
                        Respuesta.Mensaje = "Se ha detenido el servicio " + Servicio.NombreServicio + " de manera correcta";
                        Logs.RegistrarEvento(TipoEvento.Informativo, Servicio, Respuesta);
                    }
                    else
                    {
                        Respuesta.Exito = false;
                        Respuesta.Mensaje = "Ha ocurrido un error al iniciar el servicio " + Servicio.NombreServicio + ", por favor revise.";
                        Logs.RegistrarEvento(TipoEvento.Advertencia, Servicio, Respuesta);
                    }
                }
                else
                {
                    Respuesta.Exito = true;
                    Respuesta.Mensaje = "El servicio " + Servicio.NombreServicio + " ya se encuentra en estado 'DETENIDO'.";
                    Logs.RegistrarEvento(TipoEvento.Advertencia, Servicio, Respuesta);
                }

            }
            catch (Exception Ex)
            {
                Respuesta.Exito = false;
                Respuesta.Mensaje = "Se ha presentado un error: " + Ex.ToString();
                Logs.RegistrarEvento(TipoEvento.Error, Servicio, Ex);
            }
            return Respuesta;
        }
        public RespuestaServicioWindows ReiniciarServicio(ServicioWindows Servicio)
        {
            Respuesta = new RespuestaServicioWindows();
            try
            {
                ServiceController service = new ServiceController(Servicio.NombreServicio, Servicio.Servidor);
                TimeSpan timeout = TimeSpan.FromMilliseconds(30000);
                if (service.Status != ServiceControllerStatus.Stopped)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    if (service.Status == ServiceControllerStatus.Stopped)
                    {
                        Respuesta.Exito = true;
                        Respuesta.Mensaje = "Se ha detenido el servicio " + Servicio.NombreServicio + " de manera correcta";
                        Logs.RegistrarEvento(TipoEvento.Informativo, Servicio, Respuesta);
                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            Respuesta.Exito = true;
                            Respuesta.Mensaje = "Se ha iniciado el servicio " + Servicio.NombreServicio + " de manera correcta";
                            Logs.RegistrarEvento(TipoEvento.Informativo, Servicio, Respuesta);
                        }
                        else
                        {
                            Respuesta.Exito = false;
                            Respuesta.Mensaje = "Ha ocurrido un error al iniciar el servicio " + Servicio.NombreServicio + ", por favor revise.";
                            Logs.RegistrarEvento(TipoEvento.Advertencia, Servicio, Respuesta);
                        }
                    }
                    else
                    {
                        Respuesta.Exito = false;
                        Respuesta.Mensaje = "Ha ocurrido un error al detener el servicio " + Servicio.NombreServicio + ", por favor revise.";
                        Logs.RegistrarEvento(TipoEvento.Advertencia, Servicio, Respuesta);
                    }
                }
                else
                {
                    Respuesta.Exito = true;
                    Respuesta.Mensaje = "El servicio " + Servicio.NombreServicio + " ya se encuentra en estado 'DETENIDO'.";
                    Logs.RegistrarEvento(TipoEvento.Informativo, Servicio, Respuesta);
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        Respuesta.Exito = true;
                        Respuesta.Mensaje = "Se ha iniciado el servicio " + Servicio.NombreServicio + " de manera correcta";
                        Logs.RegistrarEvento(TipoEvento.Informativo, Servicio, Respuesta);
                    }
                    else
                    {
                        Respuesta.Exito = false;
                        Respuesta.Mensaje = "Ha ocurrido un error al iniciar el servicio " + Servicio.NombreServicio + ", por favor revise.";
                        Logs.RegistrarEvento(TipoEvento.Advertencia, Servicio, Respuesta);
                    }
                }

            }
            catch (Exception Ex)
            {
                Respuesta.Exito = false;
                Respuesta.Mensaje = "Se ha presentado un error: " + Ex.ToString();
                Logs.RegistrarEvento(TipoEvento.Error, Servicio, Ex);
            }
            return Respuesta;
        }

    }
}
