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
    public class CD_Reporte
    {
        public DashBoard VerDashBoard()
        {
            var _objeto = new DashBoard();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {
                    var cmd = new SqlCommand("sp_ReporteDashboard", _oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _objeto = new DashBoard()
                            {
                                TotalCliente = Convert.ToInt32(dr["TotalCliente"]),
                                TotalVenta = Convert.ToInt32(dr["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(dr["TotalProducto"]),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _objeto = new DashBoard();
            }
            return _objeto;
        }

        public List<Reporte> Ventas(string FechaInicio, string FechaFin, string IdTransaccion)
        {
            var _lista = new List<Reporte>();
            try
            {
                using (var _oConexion = new SqlConnection(Conexion.cn))
                {

                    var cmd = new SqlCommand("sp_ReporteVenta", _oConexion);
                    cmd.Parameters.AddWithValue("@FechaInicio", FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", FechaFin);
                    cmd.Parameters.AddWithValue("@IdTransaccion", IdTransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    _oConexion.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _lista.Add(new Reporte()
                            {
                                FechaVenta = dr["FechaVenta"].ToString(),
                                Cliente = dr["Cliente"].ToString(),
                                Producto = dr["Producto"].ToString(),
                                Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),
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
                _lista = new List<Reporte>();
            }
            return _lista;
        }
    }
}
