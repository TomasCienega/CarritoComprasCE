using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            var _oLista = new List<Usuario>();
            _oLista = new CN_Usuarios().Listar();

            return Json(new { data = _oLista },JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarUsuario(Usuario objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int IdUsuario)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Usuarios().Eliminar(IdUsuario, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListaReporte(string FechaInicio, string FechaFin, string IdTransaccion)
        {
            var oLista = new List<Reporte>();
            oLista = new CN_Reporte().Ventas(FechaInicio, FechaFin, IdTransaccion);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult VistaDashBoard()
        {
            DashBoard objeto = new CN_Reporte().VerDashBoard();
            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVenta(string FechaInicio, string FechaFin, string IdTransaccion)
        {
            var oLista = new List<Reporte>();
            oLista = new CN_Reporte().Ventas(FechaInicio, FechaFin, IdTransaccion);

            if (oLista == null) oLista = new List<Reporte>();

            var dt = new DataTable();

            dt.Locale = new CultureInfo("es-MX");
            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("ID Transaccion", typeof(string));

            foreach(Reporte rp in oLista)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }
            dt.TableName = "Datos";

            using (var wb = new XLWorkbook())
            {
                var sheet = wb.Worksheets.Add(dt); 
                sheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(
                        stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "ReporteVenta_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".xlsx"
                     );
                }
            }
        }
    }
}