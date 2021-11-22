using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Base._3._0
{
    public interface IPasswordHasher
    {
        string Hash(string Contrasena);

        bool Check(string hash, string Contrasena);
    }
}
