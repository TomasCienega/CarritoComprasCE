using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private readonly CD_Venta objCapaDato = new CD_Venta();

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string _mensaje)
        {
            return objCapaDato.Registrar(obj, DetalleVenta, out _mensaje);
        }
        public List<DetalleVenta> ListarCompras(int IdCliente)
        {
            return objCapaDato.ListarCompras(IdCliente);
        }
    }
}
