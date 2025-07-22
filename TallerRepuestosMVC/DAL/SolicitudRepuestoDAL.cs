using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using TallerRepuestosMVC.Models;
namespace TallerRepuestosMVC.DAL

{
    public class SolicitudRepuestoDAL
    {
        private string conexion = ConfigurationManager.ConnectionStrings["BDTallerRepuestos"].ConnectionString;
             
        public void CrearNotificacion(int usuarioId, string mensaje)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = @"INSERT INTO Notificaciones (UsuarioId, Mensaje, Fecha, Leido) VALUES (@UsuarioId, @Mensaje, GETDATE(),0)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                cmd.Parameters.AddWithValue("@Mensaje", mensaje);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public bool AgregarSolicitud(SolicitudRepuesto solicitud)
        {
            bool resultado = false;
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = @"INSERT INTO Solicitudes (RepuestoId, Cantidad, FechaSolicitud, Estado, SolicitadoPor, UsuarioId) VALUES (@RepuestoId, @Cantidad, GETDATE(), 'Pendiente', @SolicitadoPor,@UsuarioId)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@RepuestoId", solicitud.RepuestoId);
                cmd.Parameters.AddWithValue("@Cantidad", solicitud.CantidadSolicitada);
                cmd.Parameters.AddWithValue("@SolicitadoPor", solicitud.Solicitante);
                cmd.Parameters.AddWithValue("@UsuarioId", solicitud.UsuarioId); // NUEVO CAMPO DE USUARIO ID

                conn.Open();
                int filas = cmd.ExecuteNonQuery();
                resultado = filas > 0;
                

            }
            return resultado;
        }
        public int ObtenerCantidadDisponible(int repuestoId)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = "SELECT Cantidad FROM Repuestos WHERE Id = @RepuestoId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@RepuestoId", repuestoId);

                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }


        // OBTENER TODAS LAS SOLICITUDES A ENTREGAR
        public List<SolicitudRepuesto> ObtenerSolicitudes()
        {
            List<SolicitudRepuesto> lista = new List<SolicitudRepuesto>();
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = @"SELECT s.*, r.Nombre AS NombreRepuesto, u.Nombre AS NombreUsuario
                       FROM Solicitudes s 
                       JOIN Repuestos r ON s.RepuestoId = r.Id
                       LEFT JOIN Usuarios u ON s.UsuarioId = u.Id"; // JOIN con Usuarios
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lista.Add(new SolicitudRepuesto
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        RepuestoId = Convert.ToInt32(rdr["RepuestoId"]),
                        NombreRepuesto = rdr["NombreRepuesto"].ToString(),
                        CantidadSolicitada = Convert.ToInt32(rdr["Cantidad"]),
                        FechaSolicitud = Convert.ToDateTime(rdr["FechaSolicitud"]),
                        Estado = rdr["Estado"].ToString(),
                        //Solicitante = rdr["SolicitadoPor"].ToString(),
                        Solicitante = rdr["NombreUsuario"]?.ToString(), // Revisar acá
                        FechaEntrega = rdr["FechaEntrega"] as DateTime?,
                        EntregadoPor = rdr["EntregadoPor"].ToString()
                    });
                }
            }
            return lista;
        }


        //REGISTRAR ENTREGA Y REDUCIR INVENTARIO
        public bool RegistrarEntrega(int solicitudId, string entregadoPor)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();
                SqlTransaction tx = conn.BeginTransaction();

                try
                {
                    // Obtener datos de la solicitud
                    SqlCommand getCmd = new SqlCommand("SELECT RepuestoId, Cantidad FROM Solicitudes WHERE Id = @Id", conn, tx);
                    getCmd.Parameters.AddWithValue("@Id", solicitudId);
                    SqlDataReader rdr = getCmd.ExecuteReader();
                    if (!rdr.Read()) return false;

                    int repuestoId = Convert.ToInt32(rdr["RepuestoId"]);
                    int cantidad = Convert.ToInt32(rdr["Cantidad"]);
                    rdr.Close();

                    // Disminuir inventario
                    SqlCommand updateInv = new SqlCommand("UPDATE Repuestos SET Cantidad = Cantidad - @Cantidad WHERE Id = @Id", conn, tx);
                    updateInv.Parameters.AddWithValue("@Cantidad", cantidad);
                    updateInv.Parameters.AddWithValue("@Id", repuestoId);
                    updateInv.ExecuteNonQuery();

                    // Marcar como entregado
                    SqlCommand updateSol = new SqlCommand(@"UPDATE Solicitudes 
                                                    SET Estado = 'Entregado', 
                                                        FechaEntrega = GETDATE(), 
                                                        EntregadoPor = @EntregadoPor 
                                                    WHERE Id = @Id", conn, tx);
                    updateSol.Parameters.AddWithValue("@Id", solicitudId);
                    updateSol.Parameters.AddWithValue("@EntregadoPor", entregadoPor);
                    updateSol.ExecuteNonQuery();

                    tx.Commit();
                    return true;
                }
                catch
                {
                    tx.Rollback();
                    return false;
                }
            }
        }
        //Obtener Solicitud por ID
        public SolicitudRepuesto ObtenerSolicitudPorId(int id)
        {
            SolicitudRepuesto solicitud = null;

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = @"SELECT s.*, r.Nombre AS NombreRepuesto, u.Nombre AS NombreUsuario
                       FROM Solicitudes s 
                       JOIN Repuestos r ON s.RepuestoId = r.Id 
                       LEFT JOIN Usuarios u ON s.UsuarioId = u.Id
                       WHERE s.Id = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    solicitud = new SolicitudRepuesto
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        RepuestoId = Convert.ToInt32(rdr["RepuestoId"]),
                        NombreRepuesto = rdr["NombreRepuesto"].ToString(),
                        CantidadSolicitada = Convert.ToInt32(rdr["Cantidad"]),
                        FechaSolicitud = Convert.ToDateTime(rdr["FechaSolicitud"]),
                        Estado = rdr["Estado"].ToString(),
                        Solicitante = rdr["NombreUsuario"].ToString(),
                        UsuarioId = Convert.ToInt32(rdr["UsuarioId"]), // AGREGADO AL INCLUIR USUARIO ID EN TABLAS
                        FechaEntrega = rdr["FechaEntrega"] as DateTime?,
                        EntregadoPor = rdr["EntregadoPor"].ToString()
                    };
                }
            }

            return solicitud;
        }


    }
}