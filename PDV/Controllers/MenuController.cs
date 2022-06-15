using EntidadesPDV.Menu;
using NegocioPDV.Menu;
using NegocioPDV.Usuario;
using PDV.Filtros;
using System.Web.Mvc;

namespace PDV.Controllers
{
    public class MenuController : ComunController
    {
        [AuthorizePDV]
        public ActionResult _MenuParticial()
        {
            return PartialView(UsuarioActual.Menu);
        }
    }
}