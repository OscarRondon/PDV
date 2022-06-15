using EntidadesPDV;
using EntidadesPDV.Usuario;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DatosPDV.AccesoDatos
{
    public class UsuarioDatos : ComunDatos
    {
        SRUsuario.UsuarioClient usuarioCliente = new SRUsuario.UsuarioClient();

        public UsuarioDV GetUsuario(UsuarioDV usuario)
        {
            using (new OperationContextScope(usuarioCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", usuario.Login } }
                };

                UsuarioDV salida = null;
                SRUsuario.UsuarioType resultado = usuarioCliente.GetUsuario(new SRUsuario.UsuarioType() { Login = usuario.Login });
                if (resultado != null)
                {
                    salida = ObtenerUsuarioBase(resultado);
                }
                return salida;
            }
        }
        public List<UsuarioDV> GetListaUsuariosActivos()
        {
            using (new OperationContextScope(usuarioCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<UsuarioDV> listaUsuario = new List<UsuarioDV>();
                List<SRUsuario.UsuarioType> usuarios = usuarioCliente.GetListaUsuarios(true);

                if (usuarios != null)
                {
                    usuarios.ForEach(u =>
                    {
                        UsuarioDV usuario = ObtenerUsuarioBase(u);
                        listaUsuario.Add(usuario);
                    });

                }
                return listaUsuario;
            }
        }

        public UsuarioDV ValidarUsuario(UsuarioDV usuario)
        {
            using (new OperationContextScope(usuarioCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", usuario.Login } }
                };

                UsuarioDV respuesta = new UsuarioDV();
                SRUsuario.UsuarioType usuarioFiltro = new SRUsuario.UsuarioType();
                usuarioFiltro.Login = usuario.Login;
                usuarioFiltro.Password = usuario.Password;

                SRUsuario.UsuarioType result = usuarioCliente.ValidarUsuario(usuarioFiltro);
                if (!result.ExisteError)
                {
                    respuesta = ObtenerUsuarioBase(result);
                }
                else
                {
                    respuesta.ExisteError = true;
                    respuesta.ErrorMensaje = result.ErrorMensaje;
                    if (result.ErrorCode == "-1") //Excepcion
                    {

                    }
                    else if (result.ErrorCode == "1") //Usuario y Password no Existen.
                    {

                    }
                    else if (result.ErrorCode == "2") //Usuario no vigente.
                    {

                    }
                }

                return respuesta;
            }
        }

        private UsuarioDV ObtenerUsuarioBase(SRUsuario.UsuarioType usuario)
        {
            UsuarioDV salida = new UsuarioDV();
            salida.Nombre = usuario.Nombre;
            salida.Login = usuario.Login;
            salida.Password = usuario.Password;
            salida.Activo = usuario.Activo;

            usuario.ListaPerfil.ForEach(us =>
            {
                salida.ListaPerfil.Add(new EntidadesPDV.Perfil.PerfilDV()
                {
                    Activo = us.Activo,
                    Descripcion = us.Descripcion,
                    Nombre = us.Nombre
                });
            });
            return salida;
        }
    }
}
