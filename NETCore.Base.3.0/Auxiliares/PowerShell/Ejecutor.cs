using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Diagnostics;
using NETCore.Base._3._0.Entidades;

namespace NETCore.Base._3._0.Auxiliares.PowerShell
{
    public class Ejecutor
    {
        public string EjecucionSimple(string script)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",
                script)
                {
                    WorkingDirectory = Environment.CurrentDirectory,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                }
            };
            process.Start();

            var reader = process.StandardOutput;
            process.Kill();
            return reader.ReadToEnd();
        }
        public PowerShellEntidad.Respuesta EjecucionCompleja(string command)
        {
            PowerShellEntidad.Respuesta Resp = new PowerShellEntidad.Respuesta();
            Resp.Respuestas = new List<string>();
            Resp.Errores = new List<string>();
            try
            { 
                using (var ps = System.Management.Automation.PowerShell.Create())
                {

                    ps.AddCommand("Set-ExecutionPolicy")
                      .AddParameter("Scope", "Process")
                      .AddParameter("ExecutionPolicy", "Bypass")
                      .Invoke();
                    ps.AddScript(command, false);

                    var Rest=ps.Invoke();

                    ps.Commands.Clear();

                    foreach (var result in Rest)
                    {

                        Resp.Respuestas.Add(result.BaseObject.ToString());
                    }
                        Resp.Exito = true;

                    foreach (var error in ps.Streams.Error)
                    {
                        Resp.Errores.Add(error.ToString());
                        Resp.Exito = false;
                    }
                }
                return Resp;
            }
            catch (Exception ex)
            {
                Resp.Errores.Add(ex.ToString());
                Resp.Exito = false;
                return Resp;
            }

        }

    }
}
