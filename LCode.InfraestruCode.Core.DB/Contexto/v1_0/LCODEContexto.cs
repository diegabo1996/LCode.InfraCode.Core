using System;
using System.Collections.Generic;
using System.Text;
using BCP.NETCore.Base;
using LCode.InfraestruCode.Core.BD.Contexto.v1_0.Modelos;
using Microsoft.EntityFrameworkCore;

namespace LCode.InfraestruCode.Core.BD.Contexto.v1_0
{
    public class LCODEContexto: ConexionBD
    {
        string Contexto = "LCODE";
        public LCODEContexto()
        {
            Iniciar(Contexto);
        }

        public DbSet<ServidoresSolicitud> SolicitudesServidores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("lcode");
        }
    }
}
