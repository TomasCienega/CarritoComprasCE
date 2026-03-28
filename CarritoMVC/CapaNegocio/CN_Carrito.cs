using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Carrito
    {
        private readonly CD_Carrito objCapaDato = new CD_Carrito();
        public bool ExisteCarrito(int IdCliente, int IdProducto)
        {
            return objCapaDato.ExisteCarrito(IdCliente, IdProducto);
        }

        public bool OperacionCarrito(int IdCliente, int IdProducto, bool Sumar, out string _mensaje)
        {
            return objCapaDato.OperacionCarrito(IdCliente,IdProducto, Sumar, out _mensaje);
        }

        public int CantidadEnCarrito(int IdCliente)
        {
            return objCapaDato.CantidadEnCarrito(IdCliente);
        }

        public List<Carrito> ListarProducto(int IdCliente)
        {
            return objCapaDato.ListarProducto(IdCliente);
        }

        public bool EliminarCarrito(int IdCliente, int IdProducto)
        {
            return objCapaDato.EliminarCarrito(IdCliente, IdProducto);
        }
    }
}
