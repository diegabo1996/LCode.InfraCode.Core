using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace BCP.NETCore.Base
{
    public class DirectorioActivo
    {
        public bool ValidarUsrContrasena(string Usuario, string Contrasena)
        {
            if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(Contrasena))
            {
                return false;
            }
            else
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "bancred.com.bo"))
                {
                    return pc.ValidateCredentials(Usuario, Contrasena);
                }
            }
        }
        public SearchResult ObtenerInformacion(string Usuario, string Contrasena)
        {
            DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://bancred.com.bo",Usuario,Contrasena);
            DirectorySearcher directorySearcher = new DirectorySearcher();
            directorySearcher.SearchRoot = directoryEntry;
            directorySearcher.Filter = "(SAMAccountName=" + Usuario.ToUpper()+")";
            //directorySearcher.Filter = "(SAMAccountName=S60546)";
            SearchResult one = directorySearcher.FindOne();
            return one;
        }
        public static bool ValidacionGrupo(System.DirectoryServices.ResultPropertyValueCollection ColeccionGrupos, string GrupoBusqueda)
        {
            if (ColeccionGrupos != null && ColeccionGrupos.Count > 0)
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
