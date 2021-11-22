using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCP.NETCore.Base;
using System.Linq;
using NETCore.Base._3._0.Auxiliares.PowerShell;
using System.IO;

namespace LCode.InfraestruCode.Core.Test.CoreNet
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DirectorioActivo DA = new DirectorioActivo();
            bool respuesta=DA.ValidarUsrContrasena("S74543", "Diegabo147259");
            if (respuesta)
            {
                var One=DA.ObtenerInformacion("S74543","Diegabo147259");
                string NombreUsuario = One.Properties["samaccountname"][0].ToString();
                string Nombre = One.Properties["givenname"][0].ToString();
                string Apellidos = One.Properties["sn"][0].ToString();
                string Email = One.Properties["userprincipalname"][0].ToString();
                bool Desarrollo = Grupo(One.Properties["memberof"],"DESARROLLO");
                bool Certificacion = Grupo(One.Properties["memberof"], "CERTIFICACION");
                bool ServidoresMonitoreo = Grupo(One.Properties["memberof"], "GDMonitoreo");
                bool ok = respuesta;
            }
        }


        [TestMethod]
        public void PowerShellExec()
        {
            string scriptX = File.ReadAllText(@".\VM-Scripts\VM-Detener.ps1");
            Ejecutor ej = new Ejecutor();
            var Resultado=ej.EjecucionCompleja(scriptX);
        }


        public bool Grupo(System.DirectoryServices.ResultPropertyValueCollection ColeccionGrupos, string GrupoBusqueda)
        {
            if (ColeccionGrupos!=null&&ColeccionGrupos.Count>0)
            {
                for (int index = 0; index < ColeccionGrupos.Count; ++index)
                {
                    if (((string)ColeccionGrupos[index]).Contains(GrupoBusqueda))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
