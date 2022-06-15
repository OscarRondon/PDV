using EntidadesPDV.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PDV.Filtros
{
    public class AuthorizePDV : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.RouteData.Values["controller"].ToString() != "Login")
            {
                UsuarioDV usuario = ((UsuarioDV)filterContext.HttpContext.Session["UsuarioActivo"]);

                if (usuario == null)
                {
                    var routeValues = new RouteValueDictionary(new
                    {
                        action = "SessionExpire",
                        controller = "Login",
                    });
                    filterContext.Result = new RedirectToRouteResult(routeValues);
                }
                else if (filterContext.RouteData.Values["controller"].ToString() != "Home" &&
                    filterContext.RouteData.Values["controller"].ToString() != "Menu")
                {
                    if (!usuario.TieneAcceso(filterContext.RouteData.Values["controller"].ToString(),
                        filterContext.RouteData.Values["action"].ToString()))
                    {
                        var routeValues = new RouteValueDictionary(new
                        {
                            action = "UsuarioSinAcceso",
                            controller = "Home",
                        });
                        filterContext.Result = new RedirectToRouteResult(routeValues);
                    }
                }
            }
        }
    }
}