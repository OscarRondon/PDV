using EntidadesPDV.Usuario;
using System.Web.Mvc;
using System.Web.Routing;

namespace PDV.Controllers
{
    public abstract class ComunController : Controller
    {

        private const string _llaveUsuarioActualSesion = "UsuarioActivo";


        public UsuarioDV UsuarioActual
        {
            get
            {
                return Session[_llaveUsuarioActualSesion] as UsuarioDV;
            }
            set
            {
                Session[_llaveUsuarioActualSesion] = value;
            }
        }


        /// <summary>
        /// Initializes data that might not be available when the constructor is called.
        /// </summary>
        /// <param name="requestContext">The HTTP context and route data.</param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (UsuarioActual != null)
            {
                string perfil = string.Empty;
                if (UsuarioActual.ListaPerfil != null && UsuarioActual.ListaPerfil.Count > 0)
                {
                    UsuarioActual.ListaPerfil.ForEach(p =>
                    {
                        perfil = string.Format("{0}{1}", perfil, (perfil.Length > 0 ? ("-" + p.Nombre) : p.Nombre));
                    });
                }
                ViewData["NombreUsuarioConectado"] = UsuarioActual.Nombre;
                ViewData["PerfilUsuarioConectado"] = perfil;
                ViewData["RutUsuarioConectado"] = UsuarioActual.Login;
            }
        }
    }
}