using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View(new Cliente());
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Correo, string Clave)
        {
            Cliente oCliente = null;
            oCliente = new CN_Clientes().Listar().Where(c => c.Correo == Correo && c.Clave == CN_Recursos.ConvertirSha256(Clave)).FirstOrDefault();

            if (oCliente == null)
            {
                ViewBag.Error = "El usuario o contraseña no son correctas";
                return View();
            }
            else
            {
                if (oCliente.Reestablecer)
                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);

                    Session["Cliente"] = oCliente;
                    ViewBag.Error = null;

                    return RedirectToAction("Index", "Tienda");
                }
            }
        }

        [HttpPost]
        public ActionResult Registrar(Cliente obj)
        {

            int _resultado;
            string _mensaje = string.Empty;
            
            ViewData["Nombres"] = string.IsNullOrEmpty(obj.Nombres) ?"": obj.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(obj.Apellidos) ? "" : obj.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(obj.Correo) ? "" : obj.Correo;

            if (string.IsNullOrEmpty(obj.Clave) || string.IsNullOrEmpty(obj.ConfirmarClave))
            {
                ViewBag.Error = "Debe completar todos los campos de contraseña";
                return View(obj); // Detenemos aquí la ejecución
            }

            if (obj.Clave != obj.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View(obj);
            }


            _resultado  =  new CN_Clientes().Registrar(obj, out _mensaje);
            
            if (_resultado>0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index","Acceso");
            }
            else
            {
                ViewBag.Error = _mensaje;
                return View(obj);
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string IdCliente, string ClaveActual, string NuevaClave, string ConfirmarClave)
        {
            var _oCliente = new Cliente();

            _oCliente = new CN_Clientes().Listar().Where(c => c.IdCliente == int.Parse(IdCliente)).FirstOrDefault();
            if (_oCliente.Clave != CN_Recursos.ConvertirSha256(ClaveActual))
            {
                TempData["IdCliente"] = IdCliente;
                ViewData["vClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (NuevaClave != ConfirmarClave)
            {
                TempData["IdCliente"] = IdCliente;
                ViewData["vClave"] = ClaveActual;
                ViewBag.Error = "Las nuevas contraseñas no coinciden";
                return View();
            }

            ViewData["vClave"] = "";
            NuevaClave = CN_Recursos.ConvertirSha256(NuevaClave);
            string _mensaje = string.Empty;

            bool _respuesta = new CN_Clientes().CambiarClave(int.Parse(IdCliente), NuevaClave, out _mensaje);
            if (_respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = IdCliente;
                ViewBag.Error = _mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string Correo)
        {

            // 1. PRIMERO: Validar si el campo está vacío
            if (string.IsNullOrEmpty(Correo))
            {
                ViewBag.Error = "Por favor, ingrese su correo electrónico";
                return View(); // <-- Nombre exacto de tu archivo .cshtml
            }

            var _oCliente = new Cliente();
            // 2. SEGUNDO: Buscar al Cliente
            _oCliente = new CN_Clientes().Listar().Where(c => c.Correo == Correo).FirstOrDefault();

            if (_oCliente == null)
            {
                ViewBag.Error = "No se encontró un Cliente relacionado a ese correo";
                return View(); // <-- Nombre exacto de tu archivo .cshtml
            }

            // 3. TERCERO: Procesar el reestablecimiento
            string _mensaje = string.Empty;
            bool _respuesta = new CN_Clientes().ReestablecerClave(_oCliente.IdCliente, Correo, out _mensaje);

            if (_respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = _mensaje;
                return View(); // <-- Nombre exacto de tu archivo .cshtml
            }
        }

        public ActionResult CerrarSesion()
        {
            Session["Cliente"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}