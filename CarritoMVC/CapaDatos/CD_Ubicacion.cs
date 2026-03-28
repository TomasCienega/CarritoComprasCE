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
    public class CD_Ubicacion
    {
        public List<Departamento> ObtenerDepartamento()
        {
            var _lista = new List<Departamento>();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from Departamento";

                    var cmd = new SqlCommand(query, _oConexion);
                    cmd.CommandType = CommandType.Text;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _lista.Add(new Departamento()
                            {
                                IdDepartamento = dr["IdDepartamento"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _lista = new List<Departamento>();
            }
            return _lista;
        }

        public List<Provincia> ObtenerProvincia(string IdDepartamento)
        {
            var _lista = new List<Provincia>();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from Provincia where IdDepartamento = @IdDepartamento";

                    var cmd = new SqlCommand(query, _oConexion);
                    cmd.Parameters.AddWithValue("@IdDepartamento", IdDepartamento);
                    cmd.CommandType = CommandType.Text;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _lista.Add(new Provincia()
                            {
                                IdProvincia = dr["IdProvincia"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _lista = new List<Provincia>();
            }
            return _lista;
        }
        public List<Distrito> ObtenerDistrito(string IdProvincia, string IdDepartamento)
        {
            var _lista = new List<Distrito>();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from Distrito where IdProvincia = @IdProvincia and IdDepartamento = @IdDepartamento";

                    var cmd = new SqlCommand(query, _oConexion);
                    cmd.Parameters.AddWithValue("@IdProvincia", IdProvincia);
                    cmd.Parameters.AddWithValue("@IdDepartamento", IdDepartamento);
                    cmd.CommandType = CommandType.Text;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _lista.Add(new Distrito()
                            {
                                IdDistrito = dr["IdDistrito"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _lista = new List<Distrito>();
            }
            return _lista;
        }
    }
}
