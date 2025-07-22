using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using TallerRepuestosMVC.Models;
using System.Configuration;
using System.Web.Mvc;

namespace TallerRepuestosMVC.DAL
{
    public class NotificacionDAL
    {
       
        private string conexion = ConfigurationManager.ConnectionStrings["BDTallerRepuestos"].ConnectionString;

        public List<Notificacion> ObtenerNotificacionesPorUsuario(int usuarioId)
        {
            List<Notificacion> lista = new List<Notificacion>();

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = @"SELECT n.*, u.Nombre AS NombreUsuario
                       FROM Notificaciones n
                       LEFT JOIN Usuarios u ON n.UsuarioId = u.Id
                       WHERE n.UsuarioId = @UsuarioId AND n.Leido = 0
                       ORDER BY n.Fecha DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Notificacion
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        UsuarioId = Convert.ToInt32(reader["UsuarioId"]),
                        Mensaje = reader["Mensaje"].ToString(),
                        Fecha = Convert.ToDateTime(reader["Fecha"]),
                        Leido = Convert.ToBoolean(reader["Leido"]),
                        NombreUsuario = reader["NombreUsuario"]?.ToString()
                    });
                }
            }

            return lista;
        }
        
        public void MarcarComoLeidas(int usuarioId)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = "UPDATE Notificaciones SET Leido = 1 WHERE UsuarioId = @UsuarioId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}   