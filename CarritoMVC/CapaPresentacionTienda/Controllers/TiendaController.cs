using CapaEntidad;
using CapaNegocio;
using Microsoft.Ajax.Utilities;
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

        [HttpPost]
         public JsonResult AgregarCarrito(int IdProducto)
        {
            int IdCLiente = ((Cliente)Session["Cliente"]).IdCliente;
            bool _existe = new CN_Carrito().ExisteCarrito(IdCLiente, IdProducto);
            bool _respuesta = false;
            string _mensaje = string.Empty;

            if (_existe)
            {
                _mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                _respuesta = new CN_Carrito().OperacionCarrito(IdCLiente,IdProducto,true, out _mensaje);
            }

            return Json(new{ respuesta = _respuesta, mensaje = _mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidadEnCarrito()
        {
            int IdCLiente = ((Cliente)Session["Cliente"]).IdCliente;
            int _cantidad = new CN_Carrito().CantidadEnCarrito(IdCLiente);

            return Json(new { cantidad = _cantidad }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;
            var _oLista = new List<Carrito>();
            bool _conversion;
            _oLista = new CN_Carrito().ListarProducto(IdCliente).Select(oc => new Carrito
            {
                oProducto = new Producto()
                {
                    IdProducto = oc.IdCarrito,
                    Nombre = oc.oProducto.Nombre,
                    oMarca = oc.oProducto.oMarca,
                    Precio = oc.oProducto.Precio,
                    RutaImagen = oc.oProducto.RutaImagen,
                    Base64 = CN_Recursos.ConvertitBase64(Path.Combine(oc.oProducto.RutaImagen,oc.oProducto.NombreImagen), out _conversion),
                },
                Cantidad = oc.Cantidad
            }).ToList();

            return Json(new { data = _oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int IdProducto, bool sumar)
        {
            int IdCLiente = ((Cliente)Session["Cliente"]).IdCliente;
            bool _respuesta = false;
            string _mensaje = string.Empty;

                _respuesta = new CN_Carrito().OperacionCarrito(IdCLiente, IdProducto, true, out _mensaje);

            return Json(new { respuesta = _respuesta, mensaje = _mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int IdProducto)
        {
            int IdCLiente = ((Cliente)Session["Cliente"]).IdCliente;
            bool _respuesta = false;
            string _mensaje = string.Empty;

            _respuesta = new CN_Carrito().EliminarCarrito(IdCLiente,IdProducto);

            return Json(new { respuesta = _respuesta, mensaje = _mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            var _olista = new List<Departamento>();
            _olista = new CN_Ubicacion().ObtenerDepartamento();
            return Json(new { lista = _olista},JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerProvincia(string IdDepartamento)
        {
            var _olista = new List<Provincia>();
            _olista = new CN_Ubicacion().ObtenerProvincia(IdDepartamento);
            return Json(new { lista = _olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDistrito(string IdProvincia, string IdDepartamento)
        {
            var _olista = new List<Distrito>();
            _olista = new CN_Ubicacion().ObtenerDistrito(IdProvincia, IdDepartamento);
            return Json(new { lista = _olista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Carrito()
        {
            return View();
        }
    }
}