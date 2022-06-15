using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntidadesPDV.Folio;
using System.Data;
using System.Diagnostics;
using EntidadesPDV;
using PDV.Filtros;
using EntidadesPDV.Caja;
using Comunes;
using NegocioPDV.Lista;

namespace PDV.Controllers
{
    public class FolioController : ComunController
    {
        #region Administrar Folio Administrador
        [AuthorizePDV]
        public ActionResult AdministracionFolio()
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);
            NegocioPDV.Usuario.UsuarioBusiness NegUsuario = new NegocioPDV.Usuario.UsuarioBusiness(UsuarioActual);

            ViewBag.ListaDocumentos = NegFolio.ListaDocumentos();
            ViewBag.ListadoUsuarios = NegUsuario.GetListaUsuariosActivosSupervisor();

            return View();
        }
        [HttpPost]
        public ActionResult BuscarFolioPor(FolioFiltro folio)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                List<Folio> lista = NegFolio.ObtenerListadoFolio(folio);

                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener el listado de folio. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerListadoLimiteFolio()
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                respuesta.Data = NegFolio.ObtenerListadoLimiteFolio();
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener el listado. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ObtenerUltimoLimiteFolio()
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                respuesta.Data = NegFolio.ObtenerUltimoLimiteFolio();
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = string.IsNullOrEmpty(ex.Message) ? "Ocurrió un error al obtener el último folio. Vuelva a intentarlo mas tarde." : ex.Message;
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerUltimoFolio()
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                respuesta.Data = NegFolio.ObtenerUltimoFolio();
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = string.IsNullOrEmpty(ex.Message) ? "Ocurrió un error al obtener el último folio. Vuelva a intentarlo mas tarde." : ex.Message;
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarLimiteFolio(Folio folio)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                folio.IdUsuarioCreacion = this.UsuarioActual.Login;
                folio.NombreUsuarioCreacion = this.UsuarioActual.Nombre.ToUpper();

                respuesta = NegFolio.GuardarLimiteFolio(folio);
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al guardar el nuevo folio. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarFolioNuevo(Folio folio)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                folio.IdUsuarioCreacion = this.UsuarioActual.Login;
                folio.NombreUsuarioCreacion = this.UsuarioActual.Nombre.ToUpper();

                respuesta = NegFolio.GuardarFolio(folio);
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al guardar el nuevo folio. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Administrar Folio Supervisor
        [AuthorizePDV]
        public ActionResult AdministracionFolioSupervisor()
        {
            NegocioPDV.Caja.CajaBusiness NegCaja = new NegocioPDV.Caja.CajaBusiness(UsuarioActual);
            ViewBag.ListaCajasSupervisor = NegCaja.GetListaCajas(new EntidadesPDV.Caja.CajasEnt() { IdSupervisorCaja = UsuarioActual.Login });
            return View();
        }
        [HttpPost]
        public ActionResult BuscarFolioPorSupervisor(FolioFiltro folio)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                folio.IdUsuarioSupervisor = UsuarioActual.Login;
                List<Folio> lista = NegFolio.ObtenerListadoFolio(folio);

                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener el listado de folio. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BuscarSegmentosPorId(string IdFolio)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                List<FolioSegmento> lista = NegFolio.ObtenerListadoSegmentos(IdFolio);

                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener el listado de segmentos. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GuardarFolioSegmentoMasivo(List<Folio> lista)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                respuesta = NegFolio.GuardarFolioSegmentoMasivo(lista);
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al guardar el listado de segmentos. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AsignacionCajaSegmentoMasivo(List<FolioSegmento> lista)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                respuesta = NegFolio.AsignarCajaSegmentoMasivo(lista);
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al guardar las asignaciones. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Administrar Folio Cajero
        [AuthorizePDV]
        public ActionResult AdministracionFolioCaja()
        {
            return View();
        }
        [HttpGet]
        public ActionResult BuscarFolioPorAdmisionista()
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                List<FolioSegmento> lista = NegFolio.ObtenerListadoFolioCajaPorUsuario(UsuarioActual.Login);

                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener el listado de folio. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Anular Folio
        [HttpPost]
        public ActionResult AnularFolio(FolioAnulado folio)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                folio.IdUsuarioAnulacion = UsuarioActual.Login;
                folio.NombreUsuarioAnulacion = UsuarioActual.Nombre;
                respuesta = NegFolio.AnularFolio(folio);
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al anular el folio. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerMotivoAnulacion()
        {
            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                ListaGenericaBusiness NegLista = new ListaGenericaBusiness(UsuarioActual);
                respuesta.Data = NegLista.ObtenerListado<ItemList>(ListaDatosEnum.MotivoAnulacion);
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener los motivos de anulación. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Folio Anulados
        [AuthorizePDV]
        public ActionResult SolicitudesAnulacion()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BuscarFolioAnulados(FolioFiltro folio)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                List<FolioAnulado> lista = NegFolio.ObtenerListadoFolioAnulados(folio);

                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener el listado de folio. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Folios Anulados Supervisor
        [AuthorizePDV]
        public ActionResult SolicitudesAnulacionSupervisor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BuscarFolioAnuladosPorSupervisor(FolioFiltro folio)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                folio.IdUsuarioSupervisor = UsuarioActual.Login;

                List<FolioAnulado> lista = NegFolio.ObtenerListadoFolioAnuladosCajaPorSupervisor(folio);

                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener el listado de folios anulados. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}