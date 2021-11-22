using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace BCP.NETCore.Base
{
    public class BaseDatos
    {
        private DbContext context;
        public BaseDatos(DbContext ContextoBD)
        {
            context = ContextoBD;

        }
        public BaseDatos()
        {

        }
        public DataTable ObtenerTabla(DbContext context, string sqlQuery)
        {
            RegistroLogs Lg = new RegistroLogs();
            try
            {
                DbProviderFactory dbFactory = DbProviderFactories.GetFactory(context.Database.GetDbConnection());

                using (var cmd = dbFactory.CreateCommand())
                {
                    cmd.Connection = context.Database.GetDbConnection();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 120;
                    using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = cmd;

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        cmd.Connection.Close();
                        Lg.RegistrarEvento(TipoEvento.Informativo, dt);
                        return dt;
                    }
                }
            }
            catch (Exception Ex)
            {
                Lg.RegistrarEvento(TipoEvento.Error, "Fallo la ejecución del query \"" + sqlQuery + "\", revise y re-intente nuevamente!", Ex, "Datos de conexion: " + Newtonsoft.Json.JsonConvert.SerializeObject(context.Database.GetDbConnection()));
                throw Ex;
            }
        }
        public DataTable ObtenerTabla(string sqlQuery)
        {
            RegistroLogs Lg = new RegistroLogs();
            try
            {
                DbProviderFactory dbFactory = DbProviderFactories.GetFactory(context.Database.GetDbConnection());

                using (var cmd = dbFactory.CreateCommand())
                {
                    cmd.Connection = context.Database.GetDbConnection();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 120;
                    using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = cmd;

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        cmd.Connection.Close();
                        //Lg.RegistrarEvento(TipoEvento.Informativo, dt);
                        return dt;
                    }
                }
            }
            catch (Exception Ex)
            {
                Lg.RegistrarEvento(TipoEvento.Error, "Fallo la ejecución del query \"" + sqlQuery + "\", revise y re-intente nuevamente!", Ex, "Datos de conexion: " + Newtonsoft.Json.JsonConvert.SerializeObject(context.Database.GetDbConnection()));
                throw Ex;
            }
        }
        public static string ObtenerValor(DbContext context, string sqlQuery)
        {
            RegistroLogs Lg = new RegistroLogs();
            try
            {
                DbProviderFactory dbFactory = DbProviderFactories.GetFactory(context.Database.GetDbConnection());

                using (var cmd = dbFactory.CreateCommand())
                {
                    string dt = "";
                    cmd.Connection = context.Database.GetDbConnection();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 120;
                    cmd.Connection.Open();
                    var Objeto = cmd.ExecuteScalar();
                    dt = Objeto==null?"": Objeto.ToString();
                    cmd.Connection.Close();
                    Lg.RegistrarEvento(TipoEvento.Informativo, dt);
                    return dt;
                }
            }
            catch (Exception Ex)
            {
                Lg.RegistrarEvento(TipoEvento.Error, "Fallo la ejecución del query \"" + sqlQuery + "\", revise y re-intente nuevamente!", Ex, "Datos de conexion: " + Newtonsoft.Json.JsonConvert.SerializeObject(context.Database.GetDbConnection()));
                throw Ex;
            }
        }
        public string ObtenerValor(string sqlQuery)
        {
            RegistroLogs Lg = new RegistroLogs();
            try
            {
                DbProviderFactory dbFactory = DbProviderFactories.GetFactory(context.Database.GetDbConnection());

                using (var cmd = dbFactory.CreateCommand())
                {
                    string dt = "";
                    cmd.Connection = context.Database.GetDbConnection();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sqlQuery;
                    cmd.CommandTimeout = 120;
                    cmd.Connection.Open();
                    var Objeto = cmd.ExecuteScalar();
                    dt = Objeto == null ? "" : Objeto.ToString();
                    cmd.Connection.Close();
                    Lg.RegistrarEvento(TipoEvento.Informativo, dt);
                    return dt;
                }
            }
            catch (Exception Ex)
            {
                Lg.RegistrarEvento(TipoEvento.Error, "Fallo la ejecución del query \"" + sqlQuery + "\", revise y re-intente nuevamente!", Ex, "Datos de conexion: " + Newtonsoft.Json.JsonConvert.SerializeObject(context.Database.GetDbConnection()));
                throw Ex;
            }
        }
    }
}
