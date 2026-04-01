using CapaEntidad;
using CapaEntidad.Paypal;
using CapaNegocio;
using CapaPresentacionTienda.Filter;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                    IdProducto = oc.oProducto.IdProducto,
                    Nombre = oc.oProducto.Nombre,
                    oMarca = oc.oProducto.oMarca,
                    Precio = oc.oProducto.Precio,
                    RutaImagen = oc.oProducto.RutaImagen,
                    Base64 = CN_Recursos.ConvertitBase64(Path.Combine(oc.oProducto.RutaImagen,oc.oProducto.NombreImagen), out _conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad
            }).ToList();

            return Json(new { data = _oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int IdProducto, bool Sumar)
        {
            int IdCLiente = ((Cliente)Session["Cliente"]).IdCliente;
            bool _respuesta = false;
            string _mensaje = string.Empty;

                _respuesta = new CN_Carrito().OperacionCarrito(IdCLiente, IdProducto, Sumar, out _mensaje);

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

        [ValidarSession]
        [Authorize]
        public ActionResult Carrito()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        {
            decimal total = 0;

            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("en-MX");
            detalle_venta.Columns.Add("IdProducto",typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(int));

            List<Item> oListaItem = new List<Item>();

            foreach (Carrito oCarrito in oListaCarrito)
            {
                decimal subtotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.oProducto.Precio;

                total += subtotal;

                oListaItem.Add(new Item()
                {
                    name = oCarrito.oProducto.Nombre,
                    quantity = oCarrito.Cantidad.ToString(),
                    unit_amount = new UnitAmount()
                    {
                        currency_code = "USD",
                        value = oCarrito.oProducto.Precio.ToString("G", new CultureInfo("es-MX"))
                    }
                });

                detalle_venta.Rows.Add(new object[]
                {
                    oCarrito.oProducto.IdProducto,
                    oCarrito.Cantidad,
                    subtotal
                });
            }

            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = total.ToString("G", new CultureInfo("es-MX")),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal()
                        {
                            currency_code = "USD",
                            value = total.ToString("G", new CultureInfo("es-MX")),
                        }
                    }
                },
                description = "Compra de articulo de mi tienda",
                items = oListaItem
            };

            Checkout_Order oCheckOutOrder = new Checkout_Order()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit> { purchaseUnit },
                application_context = new ApplicationContext()
                {
                    brand_name = "MiTienda.com",
                    landing_page = "NO_PREFERENCE",
                    return_url = "https://localhost:44399/Tienda/PagoEfectuado",
                    cancel_url = "https://localhost:44399/Tienda/Carrito"
                }
            };

            oVenta.MontoTotal = total;
            oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;


            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            CN_Paypal oPaypal = new CN_Paypal();

            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();
            response_paypal = await oPaypal.CrearSolicitud(oCheckOutOrder);

            return Json(response_paypal,JsonRequestBehavior.AllowGet);
        }

        [ValidarSession]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            string token = Request.QueryString["token"];

            CN_Paypal oPaypal = new CN_Paypal();
            Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();
            response_paypal = await oPaypal.AprobarPago(token);

            ViewData["Status"] = response_paypal.Status;

            if (response_paypal.Status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];
                oVenta.IdTransaccion = response_paypal.Response.purchase_units[0].payments.captures[0].id;
                string mensaje = string.Empty;
                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta,out mensaje);
                ViewData["IdTransaccion"] = oVenta.IdTransaccion;
            }

            return View();
        }

        [ValidarSession]
        [Authorize]
        public ActionResult MisCompras()
        {
            int IdCliente = ((Cliente)Session["Cliente"]).IdCliente;
            var _oLista = new List<DetalleVenta>();
            bool _conversion;
            _oLista = new CN_Venta().ListarCompras(IdCliente).Select(oc => new DetalleVenta
            {
                oProducto = new Producto()
                {
                    Nombre = oc.oProducto.Nombre,
                    Precio = oc.oProducto.Precio,
                    Base64 = CN_Recursos.ConvertitBase64(Path.Combine(oc.oProducto.RutaImagen, oc.oProducto.NombreImagen), out _conversion),
                    Extension = Path.GetExtension(oc.oProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad,
                Total = oc.Total,
                IdTransaccion = oc.IdTransaccion
            }).ToList();

            return View(_oLista);
        }
    }
}