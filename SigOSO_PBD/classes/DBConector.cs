using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Npgsql;

namespace SigOSO_PBD.classes
{


    public class DBConector
    {
        private static string conectionString = "Server=localhost;Port=5432;UserId=SigOSO_user;Password=pbd2012;Database=SigOSO";
        public static string msjError = "Error al realizar la petición a la base de datos";
        public static NpgsqlConnection con = null;
        public static Mutex mutexGetCon = new Mutex();
        public static NpgsqlConnection getConection() {
            mutexGetCon.WaitOne();
            try
            {
                if (DBConector.con == null)
                {
                    DBConector.con = new NpgsqlConnection(DBConector.conectionString);
                    DBConector.con.Open();
                }
                else if (DBConector.con.State != System.Data.ConnectionState.Open)
                {
                    DBConector.con.Open();
                }
            }
            catch (Exception ex)
            {
                mutexGetCon.ReleaseMutex();
                throw ex;

            }
            mutexGetCon.ReleaseMutex();
            return DBConector.con;
        }

        public static NpgsqlDataReader SELECT(string query) {
            NpgsqlConnection conexion = DBConector.getConection();
            //conexion.Open();

            //Un select
            NpgsqlCommand comando = new NpgsqlCommand(query, conexion);
            NpgsqlDataReader resultQuery =  comando.ExecuteReader();
            
            return resultQuery;

        }

        public static int UPDATE(string query)
        {
            NpgsqlConnection conexion = DBConector.getConection();
            //conexion.Open();


            NpgsqlCommand comando = new NpgsqlCommand(query, conexion);
            int filasCambiadas = comando.ExecuteNonQuery();

            comando.Dispose();
            //conexion.Close();
            return filasCambiadas;
        }

        public static int DELETE(string query)
        {
            NpgsqlConnection conexion = DBConector.getConection();
            //conexion.Open();


            NpgsqlCommand comando = new NpgsqlCommand(query, conexion);
            int filasCambiadas = comando.ExecuteNonQuery();

            comando.Dispose();
            //conexion.Close();
            return filasCambiadas;
        }

        public static int INSERT(string query)
        {
            NpgsqlConnection conexion = DBConector.getConection();
            //conexion.Open();


            NpgsqlCommand comando = new NpgsqlCommand(query, conexion);
            int filasCambiadas = comando.ExecuteNonQuery();

            comando.Dispose();
            //conexion.Close();
            return filasCambiadas;
        }


    }
}