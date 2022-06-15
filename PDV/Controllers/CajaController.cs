using DatosPDV.SRCaja;
using EntidadesPDV;
using EntidadesPDV.Caja;
using EntidadesPDV.Folio;
using EntidadesPDV.Usuario;
using PDV.Filtros;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace PDV.Controllers
{
    public class CajaController : ComunController
    {
        #region Inicio Caja
        [AuthorizePDV]
        public ActionResult InicioCaja()
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            try
            {
                string usuario = UsuarioActual.Login;
                string sIP = Request.UserHostAddress;

                List<AsignacionCaja> asignaciones = NegAsignacion.GetListAsignacionCajasPorUsuarioAsignado(usuario, true);
                if (asignaciones == null || asignaciones.Count == 0)
                {
                    ViewBag.Mensaje = "Usuario sin Caja Asignada.";
                    ViewBag.VolverHome = true;
                }
                ViewBag.ListaCajasAsignadas = asignaciones;
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Ocurrio un error al obtener la información. Vuelva a intentarlo mas tarde.";
            }
            return View();
        }

        [HttpGet]
        public ActionResult IniciarAsignacionCaja(string asignacion = "", string numeroTransbank = "", string cajaip = "")
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            NegocioPDV.Menu.MenuBusiness menuBusiness = new NegocioPDV.Menu.MenuBusiness();

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                if (!string.IsNullOrEmpty(cajaip) &&
                    Clases.UtilesWeb.CajaValidaIp(cajaip))
                {
                    respuesta = NegAsignacion.AsignacionIniciarCaja(new AsignacionCaja() { IdAsignacion = asignacion, NumeroMaquinaTransbank = numeroTransbank });
                    if (respuesta.EsError)
                    {
                        respuesta.EsError = true;
                        respuesta.Mensaje = string.IsNullOrEmpty(respuesta.Mensaje) ? "No se pudo iniciar caja. Vuelva a intentarlo mas tarde" : respuesta.Mensaje;
                    }
                    else
                    {
                        respuesta.EsError = false; ;
                        respuesta.Mensaje = "La caja asignada se inició correctamente.";
                        UsuarioActual.Menu = menuBusiness.GetListaMenuUsuario(UsuarioActual); //Cargamos nuevamente el Menu para agregar el link de caja
                    }
                }
                else
                {
                    respuesta.EsError = true;
                    respuesta.Mensaje = "La caja que intenta iniciar, no es la misma a la cual se encuentra conectado(a).";
                }
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al intentar iniciar caja. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ModificarAsignacionCaja(string asignacion = "", string numeroTransbank = "")
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                bool esError = NegAsignacion.AsignacionModificarTransbank(new AsignacionCaja() { IdAsignacion = asignacion, NumeroMaquinaTransbank = numeroTransbank });
                if (esError)
                {
                    respuesta.EsError = true;
                    respuesta.Mensaje = "No se pudo guardar la información. Vuelva a intentarlo mas tarde";
                }
                else
                {
                    respuesta.EsError = false; ;
                    respuesta.Mensaje = "La información se guardó correctamente.";
                }
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult RechazarAsignacionCaja(string asignacion = "", string observacion = "")
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                bool esError = NegAsignacion.AsignacionRechazarCaja(new AsignacionCaja() { IdAsignacion = asignacion, ObservacionRechazo = observacion });
                if (esError)
                {
                    respuesta.EsError = true;
                    respuesta.Mensaje = "No se pudo rechazar caja. Vuelva a intentarlo mas tarde";
                }
                else
                {
                    respuesta.EsError = false; ;
                    respuesta.Mensaje = "La caja asignada se rechazo correctamente.";
                }
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al intentar rechazar caja. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Administrar Caja Administrador
        [AuthorizePDV]
        public ActionResult AdministracionCajas()
        {
            NegocioPDV.Lista.ListaGenericaBusiness NegListas = new NegocioPDV.Lista.ListaGenericaBusiness(UsuarioActual);
            NegocioPDV.Usuario.UsuarioBusiness NegUsuario = new NegocioPDV.Usuario.UsuarioBusiness(UsuarioActual);

            ViewBag.ListaFuncionalidades = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Funcionalidad);
            ViewBag.ListaFunciones = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Funcion);
            ViewBag.ListaSecciones = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Seccion);
            ViewBag.ListaPisos = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Piso);

            ViewBag.ListadoUsuarios = NegUsuario.GetListaUsuariosActivosSupervisor();

            return View();
        }

        [HttpGet]
        public ActionResult ListaCajasAdministrador()
        {
            NegocioPDV.Caja.CajaBusiness NegCaja = new NegocioPDV.Caja.CajaBusiness(UsuarioActual);

            List<CajasEnt> lista = NegCaja.GetListaCajas(new CajasEnt());
            var jsonData = new { data = lista };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarCajaModal(CajasEnt caja)
        {
            NegocioPDV.Caja.CajaBusiness NegCaja = new NegocioPDV.Caja.CajaBusiness(UsuarioActual);
            RespuestaTransaccion msg = new RespuestaTransaccion();
            try
            {
                if (string.IsNullOrEmpty(caja.IdCaja)) //Nueva Caja
                {
                    msg.EsError = NegCaja.CajaNueva(caja);
                    msg.Mensaje = msg.EsError ? "No se logró agregar la caja." : "Caja ingresada correctamente.";
                }
                else//Editar Caja
                {
                    RespuestaTransaccion salida = NegCaja.EditarCaja(caja);
                    msg.EsError = salida.EsError;
                    msg.Mensaje = msg.EsError ? (string.IsNullOrEmpty(msg.Mensaje) ? "No se logró modificar." : msg.Mensaje) : "Caja modificada correctamente.";
                }
            }
            catch (System.Exception ex)
            {
                msg.EsError = true;
                msg.Mensaje = "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde.";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AsignarSupervisorMasivoModal(List<string> lstCajasId, string idSupervisorCaja, string supervisorCaja)
        {
            NegocioPDV.Caja.CajaBusiness NegCaja = new NegocioPDV.Caja.CajaBusiness(UsuarioActual);
            RespuestaTransaccion msg = new RespuestaTransaccion();
            try
            {
                msg = NegCaja.AsignarSupervisorVariasCajas(lstCajasId, idSupervisorCaja, supervisorCaja);
                msg.Mensaje = msg.EsError ? "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde." : "Supervisor asignado correctamente.";
            }
            catch (Exception ex)
            {
                msg.EsError = true;
                msg.Mensaje = "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde.";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Administrar Caja Supervisor
        [AuthorizePDV]
        public ActionResult AdministracionCajasSupervisor()
        {
            NegocioPDV.Lista.ListaGenericaBusiness NegListas = new NegocioPDV.Lista.ListaGenericaBusiness(UsuarioActual);
            NegocioPDV.Usuario.UsuarioBusiness NegUsuario = new NegocioPDV.Usuario.UsuarioBusiness(UsuarioActual);

            ViewBag.ListaUsuarios = NegUsuario.GetListaUsuariosActivosCajero().OrderBy(x => x.Nombre).ToList();
            ViewBag.ListaFuncionalidades = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Funcionalidad);
            return View();
        }

        [HttpGet]
        public ActionResult ListaCajasSupervisor()
        {
            NegocioPDV.Caja.CajaBusiness NegCaja = new NegocioPDV.Caja.CajaBusiness(UsuarioActual);
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);

            List<CajasEnt> lstCajas = NegCaja.GetListaCajas(new CajasEnt() { IdSupervisorCaja = UsuarioActual.Login });
            List<AsignacionCaja> lstAsignacion = NegAsignacion.GetListAsignacionCajasPorSupervisor(UsuarioActual.Login, null, true);
            List<AsignacionCaja> listaFinal = new List<AsignacionCaja>();

            foreach (var item in lstCajas)
            {
                AsignacionCaja final = new AsignacionCaja();

                if (lstAsignacion.Exists(a => a.CajaAsignada.IdCaja == item.IdCaja))
                {
                    final = lstAsignacion.Find(a => a.CajaAsignada.IdCaja == item.IdCaja);
                }
                else if (string.IsNullOrEmpty(final.IdAsignacion))
                {
                    final.CajaAsignada = item;
                }

                listaFinal.Add(final);
            }
            var jsonData = new { data = listaFinal };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListaCajasSupervisorAsignadas(string FechaInicio, string FechaFin)
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            RespuestaTransaccion mensaje = new RespuestaTransaccion();

            try
            {
                string[] fIni = FechaInicio.Replace('-', '/').Split('/');
                string[] fFin = FechaFin.Replace('-', '/').Split('/');
                DateTime fechaInicio = new DateTime(int.Parse(fIni[2]), int.Parse(fIni[1]), int.Parse(fIni[0]));
                DateTime fechafin = new DateTime(int.Parse(fFin[2]), int.Parse(fFin[1]), int.Parse(fFin[0]));

                List<AsignacionCaja> lista = NegAsignacion.GetListAsignacionCajasPorSupervisor(UsuarioActual.Login, fechaInicio, fechafin);

                mensaje.EsError = false;
                mensaje.Data = lista;
            }
            catch (Exception ex)
            {
                mensaje.EsError = true;
                mensaje.Mensaje = "Ocurrió un error al obtener la información. Vuelva a intentarlo mas tarde.";
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListaCajasSupervisorAsignadasPorDia(string FechaAsignacion)
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            RespuestaTransaccion mensaje = new RespuestaTransaccion();
            try
            {
                string[] fecha = FechaAsignacion.Replace('-', '/').Split('/');
                DateTime fechaAsignacion = new DateTime(int.Parse(fecha[2]), int.Parse(fecha[1]), int.Parse(fecha[0]));

                List<AsignacionCaja> lista = NegAsignacion.GetListAsignacionCajasPorSupervisor(UsuarioActual.Login, fechaAsignacion, false);

                mensaje.EsError = false;
                mensaje.Data = lista;
            }
            catch (Exception ex)
            {
                mensaje.EsError = true;
                mensaje.Mensaje = "Ocurrió un error al obtener la información. Vuelva a intentarlo mas tarde.";
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarCajaAsignadaSupervisor(CajasEnt caja)
        {
            NegocioPDV.Caja.CajaBusiness NegCaja = new NegocioPDV.Caja.CajaBusiness(UsuarioActual);
            RespuestaTransaccion msg = NegCaja.EditarCaja(caja);
            msg.Mensaje = msg.EsError ? (string.IsNullOrEmpty(msg.Mensaje) ? "No se logro modificar." : msg.Mensaje) : "Caja modificada correctamente.";

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReAsignarCajasAsignadas(List<AsignacionCajaType> ListasAsignaciones)
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            RespuestaTransaccion msg = new RespuestaTransaccion();
            try
            {
                msg = NegAsignacion.AsignacionNuevaMasivo(ListasAsignaciones);
                msg.Mensaje = msg.EsError ? "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde." : "La información se guardói correctamente.";
            }
            catch (Exception ex)
            {
                msg.EsError = true;
                msg.Mensaje = "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde.";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Asignar Caja
        [HttpPost]
        public ActionResult GuardarCajaAsignadaCajero(AsignacionCaja asignacion)
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            RespuestaTransaccion msg = new RespuestaTransaccion();
            try
            {
                if (string.IsNullOrEmpty(asignacion.IdAsignacion))
                {
                    msg.EsError = NegAsignacion.AsignacionNueva(asignacion);
                }
                else
                {
                    msg.EsError = NegAsignacion.AsignacionModificar(asignacion);
                }
                msg.Mensaje = msg.EsError ? "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde." : "Asignación de caja guardada correctamente.";
            }
            catch (System.Exception ex)
            {
                if (ex.GetType().Name == "ExceptionControlada")
                {
                    msg.EsError = true;
                    msg.Mensaje = ex.Message;
                }
                else
                {
                    msg.EsError = true;
                    msg.Mensaje = "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde.";
                }
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TraerFolioCaja(string idCaja)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion resultado = new RespuestaTransaccion();
            try
            {
                FolioSegmento folio = NegFolio.ObtenerSiguienteFolioPorCaja(idCaja);
                if (folio != null)
                {
                    resultado.EsError = false;
                    resultado.Data = folio;
                }
                else
                {
                    resultado.EsError = true;
                    resultado.Mensaje = "La caja seleccionada no tiene folio asignado.";
                }
            }
            catch (Exception)
            {
                resultado.EsError = true;
                resultado.Mensaje = "La caja seleccionada no tiene folio asignado.";

            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Usuarios Caja

        [HttpGet]
        public ActionResult ListaUsuariosCaja()
        {
            NegocioPDV.Usuario.UsuarioBusiness UsuarioBusiness = new NegocioPDV.Usuario.UsuarioBusiness(UsuarioActual);
            RespuestaTransaccion mensaje = new RespuestaTransaccion();
            try
            {
                List<UsuarioDV> lista = UsuarioBusiness.GetListaUsuariosActivosCajero();

                mensaje.EsError = false;
                mensaje.Data = lista.OrderBy(x => x.Nombre);
            }
            catch (Exception ex)
            {
                mensaje.EsError = true;
                mensaje.Mensaje = "Ocurrió un error al obtener la información. Vuelva a intentarlo mas tarde.";
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
