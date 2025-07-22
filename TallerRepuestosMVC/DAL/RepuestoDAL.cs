using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using TallerRepuestosMVC.Models;


namespace TallerRepuestosMVC.DAL
{
    public class RepuestoDAL
    {
        private string conexion = ConfigurationManager.ConnectionStrings["BDTallerRepuestos"].ConnectionString;


        public List<Repuesto> ObtenerTodos()
        {
            List<Repuesto> lista = new List<Repuesto>();

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = "SELECT * FROM Repuestos";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Repuesto r = new Repuesto();
                    r.Id = Convert.ToInt32(rdr["Id"]);
                    r.Nombre = rdr["Nombre"].ToString();
                    lista.Add(r);
                }
            }

            return lista;
        }





        public bool InsertarRepuesto(Repuesto r)
        {
            bool resultado = false;
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = "INSERT INTO Repuestos (Nombre, Descripcion, Cantidad, Precio) VALUES (@Nombre, @Descripcion, @Cantidad, @Precio)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nombre", r.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", r.Descripcion ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cantidad", r.Cantidad);
                cmd.Parameters.AddWithValue("@Precio", r.Precio);
                conn.Open();
                int filas = cmd.ExecuteNonQuery();
                resultado = filas > 0;
               // return cmd.ExecuteNonQuery() > 0;
            }
            return resultado;
        }
    }
}   