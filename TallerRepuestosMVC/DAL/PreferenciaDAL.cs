using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using TallerRepuestosMVC.Models;

namespace TallerRepuestosMVC.DAL
{
    public class PreferenciaDAL
    {
        private string conexion = ConfigurationManager.ConnectionStrings["BDTallerRepuestos"].ConnectionString;

        public string ObtenerTema(int usuarioId)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = "SELECT Tema FROM Preferencias WHERE UsuarioId = @uId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uId", usuarioId);
                conn.Open();
                var valor = cmd.ExecuteScalar();
                return valor != null ? valor.ToString() : "Claro";
            }
        }

        public bool GuardarTema(int usuarioId, string tema)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = "IF EXISTS (SELECT 1 FROM Preferencias WHERE UsuarioId = @uId) " +
                             "UPDATE Preferencias SET Tema = @Tema WHERE UsuarioId = @uId " +
                             "ELSE INSERT INTO Preferencias (UsuarioId, Tema) VALUES (@uId, @Tema)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uId", usuarioId);
                cmd.Parameters.AddWithValue("@Tema", tema);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}