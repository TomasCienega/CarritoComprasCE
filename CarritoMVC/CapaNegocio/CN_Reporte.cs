using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private readonly CD_Reporte objCapaDato = new CD_Reporte();

        public List<Reporte> Ventas(string FechaInicio, string FechaFin, string IdTransaccion)
        {
            return objCapaDato.Ventas(FechaInicio, FechaFin, IdTransaccion);
        }
        public DashBoard VerDashBoard()
        {
            return objCapaDato.VerDashBoard();
        }
    }
}
