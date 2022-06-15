using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesPDV.Usuario;
using EntidadesPDV.Menu;
using EntidadesPDV.Perfil;
using DatosPDV.AccesoDatos;
using EntidadesPDV;

namespace NegocioPDV.Usuario
{
    public class UsuarioBusiness
    {
        UsuarioDatos usuarioAD = new UsuarioDatos();
        public UsuarioBusiness()
        {

        }
        public UsuarioBusiness(UsuarioDV Info)
        {
            usuarioAD.UsuarioActual = Info;
        }
        public List<UsuarioDV> GetListaUsuariosActivos()
        {
            return usuarioAD.GetListaUsuariosActivos();
        }

        public List<UsuarioDV> GetListaUsuariosActivosSupervisor()
        {
            return usuarioAD.GetListaUsuariosActivos().Where(u => u.TienePerfil(PerfilUsuario.SupervisorCaja)).ToList();
        }
        public List<UsuarioDV> GetListaUsuariosActivosCajero()
        {
            return usuarioAD.GetListaUsuariosActivos().Where(u => u.TienePerfil(PerfilUsuario.Admisionista)).ToList();
        }

        public UsuarioDV GetUsuario(UsuarioDV usuario)
        {
            return usuarioAD.GetUsuario(usuario);
        }
        public UsuarioDV UsuarioValido(UsuarioDV usuario)
        {
            UsuarioDV usuarioEntrada = new UsuarioDV();
            usuarioEntrada.Login = usuario.Login;
            usuarioEntrada.Password = Comunes.Encriptar.EncriptarString(usuario.Password);

            UsuarioDV salida = usuarioAD.ValidarUsuario(usuarioEntrada);

            return salida;
        }
    }
}
