using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using BCP.NETCore.Base;
using NETCore.Base._3._0;

public class EnvioMailsGn
{
    RegistroLogs Logs = new RegistroLogs();
    BaseConfiguracion BConf = new BaseConfiguracion();
    private Attachment[] Adjuntos = new Attachment[0];
    public bool EnviarMail(string[] Destino, string[] Copia, string Asunto, string Mensaje)
    {
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient(BConf.ObtenerValor("ParametrosCorreo:ServidorCorreo"));
        mail.From = new MailAddress(BConf.ObtenerValor("ParametrosCorreo:CorreoEmisor"), BConf.ObtenerValor("ParametrosCorreo:NombreRemitente"));
        bool Resultado;
        try
        {
            try
            {
                for (int i = 0; i < Destino.Length; i++)
                {
                    try
                    {
                        mail.To.Add(Destino[i].ToLower());
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch { }
            try
            {
                for (int i = 0; i < Copia.Length; i++)
                {
                    try
                    {
                        mail.CC.Add(Copia[i]);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch { }
            try
            {
                for (int i = 0; i < Adjuntos.Length; i++)
                {
                    try
                    {
                        mail.Attachments.Add(Adjuntos[i]);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch { }
            mail.Bcc.Add("dleon@bancred.com.bo");
            mail.Subject = Asunto;
            mail.Body = Mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpServer.Send(mail);
            Logs.RegistrarEvento(TipoEvento.Informativo, null, "Se ha enviado un correo de notificacion con exito.");
            Resultado = true;
        }
        catch (Exception ex)
        {
            try
            {
                mail.Subject = Asunto;
                mail.Body = Mensaje;
                mail.Priority = MailPriority.High;
                SmtpServer.Send(mail);
            }
            catch (Exception exa)
            {
                Logs.RegistrarEvento(TipoEvento.Error, exa);
            }
            Resultado = false;
        }
        finally
        {
            Adjuntos = new Attachment[0];
        }
        return Resultado;
    }
    public bool AgregarAdjuntos(byte[] Archivo, string Nombre)
    {
        try
        {
            //byte[] ArchivoFinal;
            Array.Resize(ref Adjuntos, Adjuntos.Length + 1);
            //if (ArregloBytes) ArchivoFinal = Encoding.Default.GetBytes(Archivo);
            //else ArchivoFinal = File.ReadAllBytes(Archivo);
            Stream ArchivoStream = new MemoryStream(Archivo);
            Adjuntos[Adjuntos.Length - 1] = new Attachment(ArchivoStream, Nombre);
            return true;
        }
        catch (Exception ex)
        {
            Logs.RegistrarEvento(TipoEvento.Error, ex);
            return false;
        }
    }
}
