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

        public static NpgsqlDataReaderWithConection SELECT(string query)
        {
            NpgsqlConnection conexion = new NpgsqlConnection(DBConector.conectionString);
            conexion.Open();

            //Un select
            NpgsqlCommand comando = new NpgsqlCommand(query, conexion);
            NpgsqlDataReader resultQuery =  comando.ExecuteReader();
            NpgsqlDataReaderWithConection resultado = new NpgsqlDataReaderWithConection();

            resultado.conexion = conexion;
            resultado.lector = resultQuery;

            comando.Dispose();
            return resultado;

        }

        public static int UPDATE(string query)
        {
            NpgsqlConnection conexion = new NpgsqlConnection(DBConector.conectionString);
            conexion.Open();


            NpgsqlCommand comando = new NpgsqlCommand(query, conexion);
            int filasCambiadas = comando.ExecuteNonQuery();

            comando.Dispose();
            conexion.Close();
            return filasCambiadas;
        }

        public static int DELETE(string query)
        {
            NpgsqlConnection conexion = new NpgsqlConnection(DBConector.conectionString);
            conexion.Open();


            NpgsqlCommand comando = new NpgsqlCommand(query, conexion);
            int filasCambiadas = comando.ExecuteNonQuery();

            comando.Dispose();
            conexion.Close();
            return filasCambiadas;
        }

        public static int INSERT(string query)
        {
            NpgsqlConnection conexion = new NpgsqlConnection(DBConector.conectionString);
            conexion.Open();


            NpgsqlCommand comando = new NpgsqlCommand(query, conexion);
            int filasCambiadas = comando.ExecuteNonQuery();

            comando.Dispose();
            conexion.Close();
            return filasCambiadas;
        }


    }

    public class NpgsqlDataReaderWithConection
    {
        
        public NpgsqlConnection conexion;
        public NpgsqlDataReader lector;

        public void closeConection()
        {
            conexion.Close();
        }

        public bool Read()
        {
            return lector.Read();
        }

        public bool GetBoolean(int i)
        {
            return lector.GetBoolean(i);
        }

        public string GetString(int i)
        {
            return lector.GetString(i);
        }

        public int GetInt32(int i)
        {
            return lector.GetInt32(i);
        }

        public float GetFloat(int i)
        {
            return lector.GetFloat(i);
        }

        public double GetDouble(int i)
        {
            return lector.GetDouble(i);
        }

        public DateTime GetDateTime(int i)
        {
            return lector.GetDateTime(i);
        }

        public int GetOrdinal(string Name)
        {
            return lector.GetOrdinal(Name);
        }

        public void Dispose()
        {
            lector.Dispose();
        }

        public void Close()
        {
            lector.Close();
        }

        public bool HasRows
        {
            get
            {
                return lector.HasRows;
            }
        }
    }
}