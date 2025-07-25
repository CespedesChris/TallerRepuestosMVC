using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TallerRepuestosMVC.Models;

namespace TallerRepuestosMVC.DAL
{
    public class UsuarioDAL
    {
        private string conexion = ConfigurationManager.ConnectionStrings["BDTallerRepuestos"].ConnectionString;

        //MÉTODO PARA OBTENER TODOS USUARIOS

        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = "SELECT * FROM Usuarios";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nombre = reader["Nombre"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        Contraseña = reader["Contraseña"].ToString(),
                        Rol = reader["Rol"].ToString()
                    };
                    lista.Add(usuario);
                }
            }

            return lista;
        }





        //MÉTODO PARA ACTUALIZAR PERFIL
        public bool ActualizarPerfil(Usuario u)
        {
            bool resultado = false;

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = "UPDATE Usuarios SET Nombre = @Nombre, Correo = @Correo, Contraseña = @Contraseña, Rol = @Rol WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                cmd.Parameters.AddWithValue("@Correo", u.Correo);
                cmd.Parameters.AddWithValue("@Contraseña", u.Contraseña);
                cmd.Parameters.AddWithValue("@Rol", u.Rol);
                cmd.Parameters.AddWithValue("@Id", u.Id);

                conn.Open();
                int filas = cmd.ExecuteNonQuery();

                resultado = filas > 0;
            }
            return resultado;
        }





        // Método para insertar usuarios
        public bool InsertarUsuario(Usuario usuario)
        {
            bool resultado = false;
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = "INSERT INTO Usuarios (Nombre, Correo, Contraseña, Rol) VALUES (@Nombre, @Correo, @Contraseña, @Rol)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@Correo", usuario.Correo);
                cmd.Parameters.AddWithValue("@Contraseña", usuario.Contraseña);
                cmd.Parameters.AddWithValue("@Rol", usuario.Rol);

                conn.Open();
                int filas = cmd.ExecuteNonQuery();
                resultado = filas > 0;
            }
            return resultado;
        }

        // METODO PARA OBTENER USUARIO POR CORREO
        public Usuario ObtenerPorCorreo(string correo)
        {
                Usuario usuario = null;
                using (SqlConnection conn = new SqlConnection(conexion))
                {
                    string sql = "SELECT * FROM Usuarios WHERE Correo = @Correo";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Correo", correo);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Contraseña = reader["Contraseña"].ToString(),
                            Rol = reader["Rol"].ToString()
                        };
                    }
                }
                return usuario;
        }

        //METODO PARA VERIFICAR USUARIO
        public Usuario VerificarUsuario(string correo, string contraseña)
        {
            Usuario usuario = null;
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string sql = "SELECT * FROM Usuarios WHERE Correo = @Correo AND Contraseña = @Contraseña";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Contraseña", contraseña);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nombre = reader["Nombre"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        Rol = reader["Rol"].ToString()
                    };
                }
            }
            return usuario;
        }
    }// cierra el public de UsuarioDAL
    

}