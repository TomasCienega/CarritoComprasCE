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
    public class CD_Venta
    {
        public bool Registrar(Venta obj,DataTable DetalleVenta, out string _mensaje)
        {
            bool _respuesta = false;
            _mensaje = string.Empty;
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("usp_RegistrarVenta", _oConexion);
                    cmd.Parameters.AddWithValue("@IdCliente", obj.IdCliente);
                    cmd.Parameters.AddWithValue("@TotalProducto", obj.TotalProducto);
                    cmd.Parameters.AddWithValue("@MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("@Contacto", obj.Contacto);
                    cmd.Parameters.AddWithValue("@IdDistrito", obj.IdDistrito);
                    cmd.Parameters.AddWithValue("@Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("@Direccion", obj.Direccion);
                    cmd.Parameters.AddWithValue("@IdTransaccion", obj.IdTransaccion);
                    cmd.Parameters.AddWithValue("@DetalleVenta", DetalleVenta);

                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    _oConexion.Open();
                    cmd.ExecuteNonQuery();

                    _respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    _mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                _respuesta = false;
                _mensaje = ex.Message;
            }
            return _respuesta;
        }

        public List<DetalleVenta> ListarCompras(int IdCliente)
        {
            var _lista = new List<DetalleVenta>();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from fn_ListarCompra(@IdCliente)";

                    var cmd = new SqlCommand(query, _oConexion);
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cmd.CommandType = CommandType.Text;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _lista.Add(new DetalleVenta()
                            {
                                oProducto = new Producto()
                                {
                                    Nombre = dr["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),
                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString()
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-MX")),
                                IdTransaccion = dr["IdTransaccion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _lista = new List<DetalleVenta>();
            }
            return _lista;
        }
    }
}
