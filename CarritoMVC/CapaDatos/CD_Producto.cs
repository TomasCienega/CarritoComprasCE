using CapaEntidad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Producto
    {
        public List<Producto> Listar()
        {
            var _lista = new List<Producto>();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select p.IdProducto,p.Nombre,p.Descripcion,");
                    sb.AppendLine("m.IdMarca,m.Descripcion[DesMarca],");
                    sb.AppendLine("c.IdCategoria,c.Descripcion[DesCategoria],");
                    sb.AppendLine("p.Precio,p.Stock,p.RutaImagen,p.NombreImagen,p.Activo");
                    sb.AppendLine("from Producto p");
                    sb.AppendLine("inner join Marca m on m.IdMarca = p.IdMarca");
                    sb.AppendLine("inner join Categoria c on c.IdCategoria = p.IdCategoria");

                    var cmd = new SqlCommand(sb.ToString(), _oConexion);
                    cmd.CommandType = CommandType.Text;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                oMarca = new Marca()
                                {
                                    IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                    Descripcion = dr["DesMarca"].ToString()
                                },
                                oCategoria = new Categoria()
                                {
                                    IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                    Descripcion = dr["DesCategoria"].ToString()

                                },
                                Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),
                                Stock = Convert.ToInt32(dr["Stock"]),
                                RutaImagen = dr["RutaImagen"].ToString(),
                                NombreImagen = dr["NombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _lista = new List<Producto>();
            }
            return _lista;
        }

        public int Registrar(Producto obj, out string _mensaje)
        {
            int _idAutoGenerado = 0;
            _mensaje = string.Empty;

            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_RegistrarProducto", _oConexion);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("@IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("@Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("@Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
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

        public bool Editar(Producto obj, out string _mensaje)
        {
            bool _resultado = false;
            _mensaje = string.Empty;
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_EditarProducto", _oConexion);
                    cmd.Parameters.AddWithValue("@IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("@IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("@Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("@Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
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
                _mensaje = ex.Message;
            }
            return _resultado;
        }

        public bool GuardarDatosImagen(Producto obj, out string _mensaje)
        {
            bool _resultado = false;
            _mensaje = string.Empty;

            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "update Producto set RutaImagen = @RutaImagen, NombreImagen = @NombreImagen where IdProducto = @IdProducto";

                    var cmd = new SqlCommand(query, _oConexion);
                    cmd.Parameters.AddWithValue("@IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("@NombreImagen", obj.NombreImagen);
                    cmd.Parameters.AddWithValue("@RutaImagen", obj.RutaImagen);
                    cmd.CommandType = CommandType.Text;

                    _oConexion.Open();

                    if (cmd.ExecuteNonQuery()>0)
                    {
                        _resultado = true;
                    }
                    else
                    {
                        _mensaje = "No se pudo actualizar la imagen";
                    }

                }

            }
            catch (Exception ex)
            {
                _resultado = false;
                _mensaje += ex.Message;
            }
            return _resultado;
        }

        public bool Eliminar(int IdProducto, out string _mensaje)
        {
            bool _resultado = false;
            _mensaje = string.Empty;
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_EliminarProducto", _oConexion);
                    cmd.Parameters.AddWithValue("@IdProducto", IdProducto);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    _oConexion.Open();
                    _resultado = cmd.ExecuteNonQuery() > 0 ? true : false;


                    _resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    _mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
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
