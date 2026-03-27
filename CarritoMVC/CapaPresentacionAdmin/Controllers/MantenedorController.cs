using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categoria()
        {
            return View();
        }
        public ActionResult Marca()
        {
            return View();
        }
        public ActionResult Producto()
        {
            return View();
        }


        /********** CATEGORIA **********/
        #region
        [HttpGet]
        public JsonResult ListarCategoria()
        {
            var _oLista = new List<Categoria>();
            _oLista = new CN_Categoria().Listar();

            return Json(new { data = _oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdCategoria == 0)
            {
                resultado = new CN_Categoria().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int IdCategoria)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Categoria().Eliminar(IdCategoria, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /********** CATEGORIA **********/

        /********** MARCA **********/
        #region
        [HttpGet]
        public JsonResult ListarMarca()
        {
            var _oLista = new List<Marca>();
            _oLista = new CN_Marca().Listar();

            return Json(new { data = _oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            if (objeto.IdMarca == 0)
            {
                resultado = new CN_Marca().Registrar(objeto, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int IdMarca)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Marca().Eliminar(IdMarca, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /********** MARCA **********/

        /********** PRODUCTO **********/
        #region
        [HttpGet]
        public JsonResult ListarProducto()
        {
            var _oLista = new List<Producto>();
            _oLista = new CN_Producto().Listar();

            return Json(new { data = _oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            var oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal precio;

            if (decimal.TryParse(oProducto.PrecioTexto,NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands,new CultureInfo("es-MX"), out precio))
            {
                oProducto.Precio = precio;
            }
            else
            {
                return Json(new {operacionExitosa = false, mensaje = "El formato del precio debe ser 0,000.00"}, JsonRequestBehavior.AllowGet);
            }

            if (oProducto.IdProducto == 0)
            {
                int idProductoGenerado = new CN_Producto().Registrar(oProducto, out mensaje);

                if (idProductoGenerado != 0)
                {
                    oProducto.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }

            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.IdProducto.ToString(),extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar,nombre_imagen));

                    }catch (Exception ex)
                    {
                        string msg = ex.Message;
                        guardar_imagen_exito = false;
                    }
                    if (guardar_imagen_exito)
                    {
                        oProducto.RutaImagen = ruta_guardar;
                        oProducto.NombreImagen = nombre_imagen;
                        bool rspta = new CN_Producto().GuardarInfoImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto pero hubo problemas con la imagen";
                    }
                }
            }
            return Json(new { operacionExitosa = operacion_exitosa,idGenerado = oProducto.IdProducto, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ImagenProducto(int idProducto)
        {
            bool conversion;
            Producto oProducto = new CN_Producto().Listar().Where(p => p.IdProducto == idProducto).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertitBase64(Path.Combine(oProducto.RutaImagen,oProducto.NombreImagen),out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oProducto.NombreImagen)
            },
            JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]
        public JsonResult EliminarProducto(int IdProducto)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Producto().Eliminar(IdProducto, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        /********** PRODUCTO **********/
    }
}