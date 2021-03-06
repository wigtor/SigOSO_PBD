﻿using System;
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
            if (lector.IsDBNull(i))
                return false;
            return lector.GetBoolean(i);
        }

        public string GetString(int i)
        {
            if (lector.IsDBNull(i))
                return "";
            return lector.GetString(i);
        }

        public int GetInt32(int i)
        {
            if (lector.IsDBNull(i))
                return 0;
            return lector.GetInt32(i);
        }

        public float GetFloat(int i)
        {
            if (lector.IsDBNull(i))
                return 0;
            return lector.GetFloat(i);
        }

        public double GetDouble(int i)
        {
            if (lector.IsDBNull(i))
                return 0;
            return lector.GetDouble(i);
        }

        public DateTime GetDateTime(int i)
        {
            if (lector.IsDBNull(i))
                return new DateTime();
            return lector.GetDateTime(i);
        }

        public int GetOrdinal(string Name)
        {
            return lector.GetOrdinal(Name);
        }

        public bool IsDBNull(int i)
        {
            return lector.IsDBNull(i);
        }

        public string this[string nombreColumna]
        {
            get
            {
                object resultado = lector[nombreColumna];
                if (resultado == null)
                {
                    return "";
                }
                return resultado.ToString();
            }
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
        public void CloseTodo()
        {
            lector.Dispose();
            lector.Close();
            conexion.Close();
        }
    }
}