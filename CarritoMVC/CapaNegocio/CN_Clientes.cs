using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Clientes
    {

        private readonly CD_Clientes objCapaDato = new CD_Clientes();

        public List<Cliente> Listar()
        {
            return objCapaDato.Listar();
        }
        public int Registrar(Cliente obj, out string _mensaje)
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
                obj.Clave = CN_Recursos.ConvertirSha256(obj.Clave);
                return objCapaDato.Registrar(obj, out _mensaje);

            }
            else
            {
                return 0;
            }
        }

        public bool CambiarClave(int IdCliente, string NuevaClave, out string _mensaje)
        {
            return objCapaDato.CambiarClave(IdCliente, NuevaClave, out _mensaje);
        }

        public bool ReestablecerClave(int IdCliente, string Correo, out string _mensaje)
        {
            _mensaje = string.Empty;
            string _nuevaClave = CN_Recursos.GenerarClave();
            bool _resultado = objCapaDato.ReestablecerClave(IdCliente, CN_Recursos.ConvertirSha256(_nuevaClave), out _mensaje);

            if (_resultado)
            {
                string _asunto = "Contraseña Reestablecida";
                string _mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !clave!</p>";
                _mensaje_correo = _mensaje_correo.Replace("!clave!", _nuevaClave);

                bool _respuesta = CN_Recursos.EnviarCorreo(Correo, _asunto, _mensaje_correo);

                if (_respuesta)
                {

                    return true;
                }
                else
                {
                    _mensaje = "No se pudo reestablecer el correo";
                    return false;
                }
            }
            else
            {
                _mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }
        }
    }
}
