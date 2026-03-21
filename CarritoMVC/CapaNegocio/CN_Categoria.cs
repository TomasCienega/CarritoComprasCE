using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private readonly CD_Categoria objCapaDato = new CD_Categoria();

        public List<Categoria> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Categoria obj, out string _mensaje)
        {
            _mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                _mensaje = "La descripción de la categoria no puede ser vacio";
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
        public bool Editar(Categoria obj, out string _mensaje)
        {

            _mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                _mensaje = "La descripción de la categoria no puede ser vacio";
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

        public bool Eliminar(int IdCategoria, out string _mensaje)
        {
            return objCapaDato.Eliminar(IdCategoria, out _mensaje);
        }
    }
}
