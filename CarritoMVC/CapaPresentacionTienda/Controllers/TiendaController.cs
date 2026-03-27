using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalleProducto(int IdProducto = 0)
        {
            // 1. Buscamos el producto real en la base de datos usando el ID
            // Suponiendo que tu capa de negocio se llama CN_Producto
            var _oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == IdProducto).FirstOrDefault();

            bool _conversion;

            if (_oProducto != null)
            {
                // 2. Solo intentamos convertir si hay una ruta válida en la BD
                if (!string.IsNullOrEmpty(_oProducto.RutaImagen) && !string.IsNullOrEmpty(_oProducto.NombreImagen))
                {
                    _oProducto.Base64 = CN_Recursos.ConvertitBase64(Path.Combine(_oProducto.RutaImagen, _oProducto.NombreImagen), out _conversion);
                    _oProducto.Extension = Path.GetExtension(_oProducto.NombreImagen);
                }
                else
                {
                    _oProducto.Base64 = ""; // O una imagen por defecto
                }
            }

            return View(_oProducto);
        }

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            var _lista = new List<Categoria>();
            _lista = new CN_Categoria().Listar();
            return Json(new { data = _lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListaMarcasPorCategorias(int IdCategoria)
        {
            var _lista = new List<Marca>();
            _lista = new CN_Marca().ListarMarcaPorCategoria(IdCategoria);
            return Json(new { data = _lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int IdCategoria, int IdMarca)
        {
            var _lista = new List<Producto>();

            bool conversion;

            _lista = new CN_Producto().Listar().Select(p => new Producto() {
                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                oMarca = p.oMarca,
                oCategoria = p.oCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertitBase64(Path.Combine(p.RutaImagen,p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo,
            }).Where(p =>
                p.oCategoria.IdCategoria == (IdCategoria == 0 ? p.oCategoria.IdCategoria : IdCategoria) &&
                p.oMarca.IdMarca == (IdMarca == 0 ? p.oMarca.IdMarca : IdMarca) &&
                p.Stock > 0 && p.Activo == true
            ).ToList();

            var _jsonResult = Json(new { data = _lista }, JsonRequestBehavior.AllowGet);
            _jsonResult.MaxJsonLength = int.MaxValue;
            return _jsonResult;
        }
    }
}