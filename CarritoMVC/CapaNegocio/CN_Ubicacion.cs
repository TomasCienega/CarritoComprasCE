using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private readonly CD_Ubicacion objCapaDato = new CD_Ubicacion();

        public List<Departamento> ObtenerDepartamento()
        {
            return objCapaDato.ObtenerDepartamento();
        }

        public List<Provincia> ObtenerProvincia(string IdDepartamento)
        {
            return objCapaDato.ObtenerProvincia(IdDepartamento);
        }
        public List<Distrito> ObtenerDistrito(string IdProvincia, string IdDepartamento)
        {
            return objCapaDato.ObtenerDistrito(IdProvincia,IdDepartamento);
        }
    }
}
