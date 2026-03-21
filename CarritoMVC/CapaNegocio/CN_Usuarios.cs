using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private readonly CD_Usuarios objCapaDato = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }
        public int Registrar(Usuario obj, out string _mensaje)
        {
            _mensaje = string.Empty;

            if(string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                _mensaje = "El Nombre no puede ser vacio";
            }else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                _mensaje = "El Apellido no puede ser vacio";
            }else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                _mensaje = "El Correo no puede ser vacio";
            }

            if (string.IsNullOrEmpty(_mensaje))
            {
                string clave = CN_Recursos.GenerarClave();

                string asunto = "Creacion de Cuenta";
                string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!",clave);
                bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto,mensaje_correo);
                if (respuesta)
                {
                    obj.Clave = CN_Recursos.ConvertirSha256(clave);
                    return objCapaDato.Registrar(obj, out _mensaje);
                }
                else
                {
                    _mensaje = "No se puede enviar el correo";
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public bool Editar(Usuario obj, out string _mensaje)
        {

            _mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                _mensaje = "El Nombre no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                _mensaje = "El Apellido no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                _mensaje = "El Correo no puede ser vacio";
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

        public bool Eliminar(int IdUsuario, out string _mensaje)
        {
            return objCapaDato.Eliminar(IdUsuario, out _mensaje);
        }
    }
}
