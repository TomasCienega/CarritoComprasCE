using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private readonly CD_Marca objCapaDato = new CD_Marca();

        public List<Marca> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Marca obj, out string _mensaje)
        {
            _mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                _mensaje = "La descripción de la marca no puede ser vacio";
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
        public bool Editar(Marca obj, out string _mensaje)
        {

            _mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                _mensaje = "La descripción de la marca no puede ser vacio";
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

        public bool Eliminar(int IdMarca, out string _mensaje)
        {
            return objCapaDato.Eliminar(IdMarca, out _mensaje);
        }

        public List<Marca> ListarMarcaPorCategoria(int IdCategoria)
        {
            return objCapaDato.ListarMarcaPorCategoria(IdCategoria);
        }
    }
}
