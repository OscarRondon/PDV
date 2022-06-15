using EntidadesPDV;
using EntidadesPDV.Usuario;
using NegocioPDV.Menu;
using NegocioPDV.Usuario;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace PDV.Controllers
{
    public class LoginController : ComunController
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        UsuarioBusiness usuarioBusiness = new UsuarioBusiness();
        MenuBusiness menuBusiness = new MenuBusiness();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SessionExpire()
        {
            UsuarioActual = null;
            FormsAuthentication.SignOut();

            ViewBag.Mensaje = "Su sesión de usuario a expirado, ingrese nuevamente.";
            return View("Index");
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            try
            {
                ViewBag.UserName = form["username"];

                UsuarioDV usuario = new UsuarioDV();
                usuario.Login = form["username"];
                usuario.Password = form["password"];


                UsuarioDV salida = usuarioBusiness.UsuarioValido(usuario);

                if (!salida.ExisteError)
                {
                    FormsAuthentication.SetAuthCookie(usuario.Login, true);
                    UsuarioActual = salida;
                    UsuarioActual.Menu = menuBusiness.GetListaMenuUsuario(UsuarioActual);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Mensaje = "Usuario o contraseña inválidos. Intente nuevamente.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Ocurrió un error. Vuelva a intentarlo mas tarde.";
            }

            return View();
        }





        /// <summary>
        /// Redirecciona a una url local
        /// </summary>
        /// <param name="returnUrl">Url Local</param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string ReturnUrl)
        {
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        /// <summary>
        /// Cerrar Sesion
        /// </summary>
        /// <returns> </returns>
        public ActionResult LogOff()
        {
            UsuarioActual = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

    }
}