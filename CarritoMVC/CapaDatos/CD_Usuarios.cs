using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            var _lista = new List<Usuario>();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdUsuario,Nombres,Apellidos,Correo,Clave,Reestablecer,Activo from Usuario";

                    var cmd = new SqlCommand(query, _oConexion);
                    cmd.CommandType = CommandType.Text;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _lista.Add(new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                Activo = Convert.ToBoolean(dr["Activo"])

                            });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _lista = new List<Usuario>();
            }
            return _lista;
        }

        public int Registrar(Usuario obj, out string _mensaje)
        {
            int _idAutoGenerado = 0;
            _mensaje = string.Empty;

            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_RegistrarUsuario", _oConexion);
                    cmd.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("@Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    _oConexion.Open();
                    cmd.ExecuteNonQuery();

                    _idAutoGenerado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    _mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }

            }catch(Exception ex)
            {
                _idAutoGenerado = 0;
                _mensaje += ex.Message;
            }
            return _idAutoGenerado;
        }

        public bool Editar(Usuario obj, out string _mensaje)
        {
            bool _resultado = false;
            _mensaje = string.Empty;
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_EditarUsuario", _oConexion);
                    cmd.Parameters.AddWithValue("IdUsuario",obj.IdUsuario);
                    cmd.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    _oConexion.Open();
                    cmd.ExecuteNonQuery();

                    _resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    _mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }

            }catch(Exception ex)
            {
                _resultado = false;
                _mensaje = ex.Message;
            }
            return _resultado;
        }

        public bool Eliminar(int IdUsuario, out string _mensaje)
        {
            bool _resultado = false;
            _mensaje = string.Empty;
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("delete top (1) from Usuario where IdUsuario = @IdUsuario", _oConexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                    cmd.CommandType = CommandType.Text;
                    _oConexion.Open();
                    _resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch(Exception ex)
            {
                _resultado = false;
                _mensaje = ex.Message;
            }
            return _resultado;
        }
    }
}
