using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Carrito
    {
        public bool ExisteCarrito(int IdCliente, int IdProducto)
        {
            bool _resultado = true;

            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_ExisteCarrito", _oConexion);
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cmd.Parameters.AddWithValue("@IdProducto", IdProducto);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    _oConexion.Open();
                    cmd.ExecuteNonQuery();

                    _resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }

            }
            catch 
            {
                _resultado = false;
            }
            return _resultado;
        }

        public bool OperacionCarrito(int IdCliente, int IdProducto, bool Sumar, out string _mensaje)
        {
            bool _resultado = true;
            _mensaje = string.Empty;

            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_OperacionCarrito", _oConexion);
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cmd.Parameters.AddWithValue("@IdProducto", IdProducto);
                    cmd.Parameters.AddWithValue("@Sumar", Sumar);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    _oConexion.Open();
                    cmd.ExecuteNonQuery();

                    _resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    _mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                _resultado = false;
                _mensaje += ex.Message;
            }
            return _resultado;
        }

        public int CantidadEnCarrito(int IdCliente)
        {
            int _resultado = 0;
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("select count(*) from Carrito where IdCliente = @IdCliente", _oConexion);
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cmd.CommandType = CommandType.Text;
                    _oConexion.Open();
                    _resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch
            {
                _resultado = 0;
            }
            return _resultado;
        }

        public List<Carrito> ListarProducto(int IdCliente)
        {
            var _lista = new List<Carrito>();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from fn_ObtenerCarritoCliente(@IdCliente)";

                    var cmd = new SqlCommand(query, _oConexion);
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cmd.CommandType = CommandType.Text;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _lista.Add(new Carrito()
                            {
                                oProducto = new Producto()
                                {
                                    IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    oMarca = new Marca()
                                    {
                                        Descripcion = dr["DesMarca"].ToString()
                                    },
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),
                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString()
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _lista = new List<Carrito>();
            }
            return _lista;
        }

        public bool EliminarCarrito(int IdCliente, int IdProducto)
        {
            bool _resultado = true;

            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_EliminarCarrito", _oConexion);
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cmd.Parameters.AddWithValue("@IdProducto", IdProducto);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    _oConexion.Open();
                    cmd.ExecuteNonQuery();

                    _resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }

            }
            catch
            {
                _resultado = false;
            }
            return _resultado;
        }
    }
}
