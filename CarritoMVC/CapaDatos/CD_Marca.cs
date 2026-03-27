using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Marca
    {
        public List<Marca> Listar()
        {
            var _lista = new List<Marca>();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdMarca,Descripcion,Activo from Marca";

                    var cmd = new SqlCommand(query, _oConexion);
                    cmd.CommandType = CommandType.Text;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _lista.Add(new Marca()
                            {
                                IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _lista = new List<Marca>();
            }
            return _lista;
        }

        public int Registrar(Marca obj, out string _mensaje)
        {
            int _idAutoGenerado = 0;
            _mensaje = string.Empty;

            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_RegistrarMarca", _oConexion);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
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

        public bool Editar(Marca obj, out string _mensaje)
        {
            bool _resultado = false;
            _mensaje = string.Empty;
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_EditarMarca", _oConexion);
                    cmd.Parameters.AddWithValue("@IdMarca", obj.IdMarca);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion);
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

        public bool Eliminar(int IdMarca, out string _mensaje)
        {
            bool _resultado = false;
            _mensaje = string.Empty;
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_EliminarMarca", _oConexion);
                    cmd.Parameters.AddWithValue("@IdMarca", IdMarca);
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

        public List<Marca> ListarMarcaPorCategoria(int IdCategoria)
        {
            var _lista = new List<Marca>();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("select distinct m.IdMarca, m.Descripcion from Producto p");
                    sb.AppendLine("inner join Categoria c on c.IdCategoria = p.IdCategoria and c.Activo = 1");
                    sb.AppendLine("inner join Marca m on m.IdMarca = p.IdMarca and m.Activo = 1");
                    sb.AppendLine("where c.IdCategoria = IIF(@IdCategoria = 0, c.IdCategoria, @IdCategoria) and p.Activo = 1");

                    var cmd = new SqlCommand(sb.ToString(), _oConexion);
                    cmd.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                    cmd.CommandType = CommandType.Text;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _lista.Add(new Marca()
                            {
                                IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                Descripcion = dr["Descripcion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _lista = new List<Marca>();
            }
            return _lista;
        }
    }
}





