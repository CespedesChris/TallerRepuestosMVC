using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using TallerRepuestosMVC.Models;

namespace TallerRepuestosMVC.DAL
{
    public class SolicitudDAL
    {
        public bool MarcarComoEntregado(int idSolicitud)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();

                // Iniciar transacción para asegurar consistencia
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Obtener datos de la solicitud (RepuestoId y Cantidad)
                    string querySolicitud = "SELECT RepuestoId, Cantidad FROM Solicitud WHERE Id = @Id";
                    SqlCommand cmdSolicitud = new SqlCommand(querySolicitud, conn, transaction);
                    cmdSolicitud.Parameters.AddWithValue("@Id", idSolicitud);

                    int repuestoId = 0;
                    int cantidadSolicitada = 0;

                    using (SqlDataReader reader = cmdSolicitud.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            repuestoId = Convert.ToInt32(reader["RepuestoId"]);
                            cantidadSolicitada = Convert.ToInt32(reader["Cantidad"]);
                        }
                        else
                        {
                            transaction.Rollback();
                            return false; // No existe solicitud
                        }
                    }

                    // 2. Verificar que haya suficiente inventario
                    string queryInventario = "SELECT Cantidad FROM Repuesto WHERE Id = @RepuestoId";
                    SqlCommand cmdInventario = new SqlCommand(queryInventario, conn, transaction);
                    cmdInventario.Parameters.AddWithValue("@RepuestoId", repuestoId);

                    int cantidadDisponible = Convert.ToInt32(cmdInventario.ExecuteScalar());

                    if (cantidadDisponible < cantidadSolicitada)
                    {
                        transaction.Rollback();
                        return false; // No hay suficiente inventario
                    }

                    // 3. Disminuir el inventario
                    string queryActualizarRepuesto = "UPDATE Repuestos SET Cantidad = Cantidad - @Cantidad WHERE Id = @RepuestoId";
                    SqlCommand cmdActualizar = new SqlCommand(queryActualizarRepuesto, conn, transaction);
                    cmdActualizar.Parameters.AddWithValue("@Cantidad", cantidadSolicitada);
                    cmdActualizar.Parameters.AddWithValue("@RepuestoId", repuestoId);
                    cmdActualizar.ExecuteNonQuery();

                    // 4. Actualizar el estado de la solicitud
                    string queryActualizarSolicitud = "UPDATE Solicitud SET Estado = 'Entregado', FechaEntrega = GETDATE() WHERE Id = @Id";
                    SqlCommand cmdEntrega = new SqlCommand(queryActualizarSolicitud, conn, transaction);
                    cmdEntrega.Parameters.AddWithValue("@Id", idSolicitud);
                    cmdEntrega.ExecuteNonQuery();

                    // 5. Confirmar cambios
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        //GUARDAR LA SOLICITUD
        private string conexion = ConfigurationManager.ConnectionStrings["BDTallerRepuestos"].ConnectionString;
        public bool GuardarSolicitud(SolicitudRepuesto solicitud)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = @"INSERT INTO Solicitudes (RepuestoId, Cantidad, FechaSolicitud, Estado, SolicitadoPor)
                       VALUES (@RepuestoId, @Cantidad, GETDATE(), 'Pendiente', @SolicitadoPor)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@RepuestoId", solicitud.RepuestoId);
                cmd.Parameters.AddWithValue("@Cantidad", solicitud.CantidadSolicitada);
                cmd.Parameters.AddWithValue("@SolicitadoPor", solicitud.Solicitante);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

    }
}