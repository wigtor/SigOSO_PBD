using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;

namespace SigOSO_PBD.classes
{
    public class DBConector
    {
        private static string conectionString = "Server=localhost;Port=5432;UserId=SigOSO_user;Password=pbd2012;Database=SigOSO";
        public static string msjError = "Error al realizar la petición a la base de datos";
        
        public static NpgsqlDataReader SELECT(string query) {
            NpgsqlConnection con = new NpgsqlConnection(DBConector.conectionString);
            con.Open();

            //Un select
            NpgsqlCommand comando = new NpgsqlCommand(query, con);
            NpgsqlDataReader resultQuery =  comando.ExecuteReader();
            
            return resultQuery;

        }

        public static int UPDATE(string query)
        {
            NpgsqlConnection con = new NpgsqlConnection(DBConector.conectionString);
            con.Open();

            
            NpgsqlCommand comando = new NpgsqlCommand(query, con);
            int filasCambiadas = comando.ExecuteNonQuery();

            comando.Dispose();
            con.Close();
            return filasCambiadas;
        }

        public static int DELETE(string query)
        {
            NpgsqlConnection con = new NpgsqlConnection(DBConector.conectionString);
            con.Open();


            NpgsqlCommand comando = new NpgsqlCommand(query, con);
            int filasCambiadas = comando.ExecuteNonQuery();

            comando.Dispose();
            con.Close();
            return filasCambiadas;
        }

        public static int INSERT(string query)
        {
            NpgsqlConnection con = new NpgsqlConnection(DBConector.conectionString);
            con.Open();


            NpgsqlCommand comando = new NpgsqlCommand(query, con);
            int filasCambiadas = comando.ExecuteNonQuery();

            comando.Dispose();
            con.Close();
            return filasCambiadas;
        }


    }
}