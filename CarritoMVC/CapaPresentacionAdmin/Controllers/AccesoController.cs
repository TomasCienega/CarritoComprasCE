using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult ReestablecerClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Correo, string Clave)
        {
            var _oUsuario = new Usuario();

            _oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == Correo && u.Clave == CN_Recursos.ConvertirSha256(Clave)).FirstOrDefault();
            if (_oUsuario == null)
            {
                ViewBag.Error = "Correo o Contraseña no correcta";
                return View();
            }
            else
            {
                if (_oUsuario.Reestablecer)
                {
                    TempData["IdUsuario"] = _oUsuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }
                FormsAuthentication.SetAuthCookie(_oUsuario.Correo,false);
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string IdUsuario, string ClaveActual, string NuevaClave, string ConfirmarClave)
        {
            var _oUsuario = new Usuario();

            _oUsuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(IdUsuario)).FirstOrDefault();
            if (_oUsuario.Clave != CN_Recursos.ConvertirSha256(ClaveActual))
            {
                TempData["IdUsuario"] = IdUsuario;
                ViewData["vClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }else if (NuevaClave != ConfirmarClave)
            {
                TempData["IdUsuario"] = IdUsuario;
                ViewData["vClave"] = ClaveActual;
                ViewBag.Error = "Las nuevas contraseñas no coinciden";
                return View();
            }

            ViewData["vClave"] = "";
            NuevaClave = CN_Recursos.ConvertirSha256(NuevaClave);
            string _mensaje = string.Empty;

            bool _respuesta = new CN_Usuarios().CambiarClave(int.Parse(IdUsuario), NuevaClave, out _mensaje);
            if (_respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdUsuario"] = IdUsuario;
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
                return View("ReestablecerClave"); // <-- Nombre exacto de tu archivo .cshtml
            }

            // 2. SEGUNDO: Buscar al usuario
            var _oUsuario = new CN_Usuarios().Listar().Where(u => u.Correo == Correo).FirstOrDefault();

            if (_oUsuario == null)
            {
                ViewBag.Error = "No se encontró un usuario relacionado a ese correo";
                return View("ReestablecerClave"); // <-- Nombre exacto de tu archivo .cshtml
            }

            // 3. TERCERO: Procesar el reestablecimiento
            string _mensaje = string.Empty;
            bool _respuesta = new CN_Usuarios().ReestablecerClave(_oUsuario.IdUsuario, Correo, out _mensaje);

            if (_respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = _mensaje;
                return View("ReestablecerClave"); // <-- Nombre exacto de tu archivo .cshtml
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}