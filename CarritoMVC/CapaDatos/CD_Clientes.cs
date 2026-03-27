using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Clientes
    {
        public List<Cliente> Listar()
        {
            var _lista = new List<Cliente>();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdCliente,Nombres,Apellidos,Correo,Clave,Reestablecer from Cliente";

                    var cmd = new SqlCommand(query, _oConexion);
                    cmd.CommandType = CommandType.Text;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _lista.Add(new Cliente()
                            {
                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"])

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _lista = new List<Cliente>();
            }
            return _lista;
        }

        public int Registrar(Cliente obj, out string _mensaje)
        {
            int _idAutoGenerado = 0;
            _mensaje = string.Empty;

            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_RegistrarCliente", _oConexion);
                    cmd.Parameters.AddWithValue("@Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("@Clave", obj.Clave);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    _oConexion.Open();
                    cmd.ExecuteNonQuery();

                    _idAutoGenerado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    _mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                _idAutoGenerado = 0;
                _mensaje += ex.Message;
            }
            return _idAutoGenerado;
        }

        public bool CambiarClave(int IdCliente, string NuevaClave, out string _mensaje)
        {
            bool _resultado = false;
            _mensaje = string.Empty;
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("update Cliente set Clave = @NuevaClave, Reestablecer = 0 where IdCliente = @IdCliente", _oConexion);
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cmd.Parameters.AddWithValue("@NuevaClave", NuevaClave);
                    cmd.CommandType = CommandType.Text;
                    _oConexion.Open();
                    _resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                _resultado = false;
                _mensaje = ex.Message;
            }
            return _resultado;
        }

        public bool ReestablecerClave(int IdCliente, string Clave, out string _mensaje)
        {
            bool _resultado = false;
            _mensaje = string.Empty;
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("update Cliente set Clave = @Clave, Reestablecer = 1 where IdCliente = @IdCliente", _oConexion);
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cmd.Parameters.AddWithValue("@Clave", Clave);
                    cmd.CommandType = CommandType.Text;
                    _oConexion.Open();
                    _resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                _resultado = false;
                _mensaje = ex.Message;
            }
            return _resultado;
        }
    }
}
