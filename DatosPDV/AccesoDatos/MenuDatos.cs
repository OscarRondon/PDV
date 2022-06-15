using EntidadesPDV.Menu;
using EntidadesPDV.Perfil;
using EntidadesPDV.Usuario;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DatosPDV.AccesoDatos
{
    public class MenuDatos
    {
        SRMenu.MenuClient menuCl = new SRMenu.MenuClient();

        public List<MenuDV> GetListaMenuUsuario(UsuarioDV usuario)
        {
            using (new OperationContextScope(menuCl.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", usuario.Login } }
                };

                SRMenu.UsuarioType usuarioWS = new SRMenu.UsuarioType()
                {
                    Login = usuario.Login
                };
                return (from mn in menuCl.GetListaMenuUsuario(usuarioWS)
                        select new MenuDV()
                        {
                            Code = mn.Codigo,
                            Name = mn.Descripcion,
                            Padre = mn.MenuPadre,
                            Controlador = mn.Controlador,
                            Funcion = mn.Funcion,
                            Orden = mn.Orden
                        }
                        ).ToList();
            }
        }
    }
}
