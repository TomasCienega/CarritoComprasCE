using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private readonly CD_Producto objCapaDato = new CD_Producto();

        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Producto obj, out string _mensaje)
        {
            _mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                _mensaje = "El nombre del Producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                _mensaje = "La descripcion del Producto no puede ser vacio";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                _mensaje = "Debe seleccionar una Marca";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                _mensaje = "Debe seleccionar una Categoria";
            }
            else if (obj.Precio == 0)
            {
                _mensaje = "Debe ingresar el Precio del producto";
            }
            else if (obj.Stock == 0)
            {
                _mensaje = "Debe ingresar el Stock deel producto";
            }


            if (string.IsNullOrEmpty(_mensaje))
            {

                return objCapaDato.Registrar(obj, out _mensaje);

            }
            else
            {
                return 0;
            }
        }
        public bool Editar(Producto obj, out string _mensaje)
        {

            _mensaje = string.Empty;

            if (obj.IdProducto == 0)
            {
                _mensaje = "Error al identificar el producto para editar";
            }
            else if(string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                _mensaje = "El nombre del Producto no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                _mensaje = "La descripcion del Producto no puede ser vacio";
            }
            else if (obj.IdProducto == 0)
            {
                _mensaje = "Debe seleccionar un Producto";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                _mensaje = "Debe seleccionar una Marca";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                _mensaje = "Debe seleccionar una Categoria";
            }
            else if (obj.Precio == 0)
            {
                _mensaje = "Debe ingresar el Precio del producto";
            }
            else if (obj.Stock == 0)
            {
                _mensaje = "Debe ingresar el Stock deel producto";
            }

            if (string.IsNullOrEmpty(_mensaje))
            {

                return objCapaDato.Editar(obj, out _mensaje);
            }
            else
            {
                return false;
            }

        }

        public bool GuardarInfoImagen(Producto obj, out string _mensaje)
        {
            return objCapaDato.GuardarDatosImagen(obj, out _mensaje);
        }

        public bool Eliminar(int IdProducto, out string _mensaje)
        {
            return objCapaDato.Eliminar(IdProducto, out _mensaje);
        }
    }
}
