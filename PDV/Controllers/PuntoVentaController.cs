using EntidadesPDV;
using EntidadesPDV.Caja;
using EntidadesPDV.Garantia;
using EntidadesPDV.PuntoVenta;
using Newtonsoft.Json;
using PDV.Filtros;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Configuration;

namespace PDV.Controllers
{
    public class PuntoVentaController : ComunController
    {
        #region Punto de Venta
        [AuthorizePDV]
        public ActionResult PuntoDeVenta(string asignacion = "")
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            NegocioPDV.Lista.ListaGenericaBusiness NegListas = new NegocioPDV.Lista.ListaGenericaBusiness(UsuarioActual);

            try
            {
                AsignacionCaja salida = NegAsignacion.GetCajaAsignadaPorId(asignacion);
                if (salida.Estado != EstadoAsignacion.Iniciada &&
                    salida.Estado != EstadoAsignacion.CerradaEfectivo)
                {
                    return RedirectToAction("CajaNoValida", "Home");
                }
                else if (salida.IdUsuarioAsignado != UsuarioActual.Login)
                {
                    return RedirectToAction("CajaNoValida", "Home");
                }
                else if (!Clases.UtilesWeb.CajaValidaIp(salida.CajaAsignada.IpCaja))
                {
                    return RedirectToAction("CajaNoValida", "Home");
                }
                else
                {
                    ViewBag.ListaBancos = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Banco);
                    ViewBag.ListaTarjetas = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Tarjeta);
                    ViewBag.ListaMediosDePago = NegListas.ObtenerListado<MediosPagoEnt>(NegocioPDV.Lista.ListaDatosEnum.MedioDePago);

                    return View(salida);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("CajaNoValida", "Home");
            }
        }

        [HttpGet]
        public ActionResult ListaDocumentosUsuario()
        {
            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                List<TransaccionesEnt> lista = this.ObtenerDocumentosUsuario();
                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener la información. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        private List<TransaccionesEnt> ObtenerDocumentosUsuario()
        {
            NegocioPDV.PuntoVenta.VentasBusiness NegPuntoVenta = new NegocioPDV.PuntoVenta.VentasBusiness(UsuarioActual);

            TransaccionFiltro filtro = new TransaccionFiltro();
            filtro.BuscarPor = TransaccionBuscar.Usuario;

            if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.SupervisorCaja))
            {
                filtro.Supervisor = UsuarioActual.Login;
            }
            else
            {
                filtro.Usuario = UsuarioActual.Login;
            }
            return NegPuntoVenta.ObtenerDocumentosPendientes(filtro);
        }

        [HttpPost]
        public ActionResult ListaDocumentosFiltro(TransaccionFiltro filtro)
        {
            NegocioPDV.PuntoVenta.VentasBusiness NegPuntoVenta = new NegocioPDV.PuntoVenta.VentasBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                filtro.BuscarPor = TransaccionBuscar.Todo;
                if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.SupervisorCaja))
                {
                    filtro.Supervisor = UsuarioActual.Login;
                }
                else if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.SupervisorGarantia))
                {
                    filtro.Usuario = "";
                }
                else
                {
                    filtro.Usuario = UsuarioActual.Login;
                }

                List<TransaccionesEnt> lista = NegPuntoVenta.ObtenerDocumentosPendientes(filtro);
                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener la información. Vuelva a intentarlo mas tarde.";
            }

            // codigo para setear el largo maximo de la serializacion de json (Caso de una lista de documentos muy grande)
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;

            var json = Json(respuesta, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
            //return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TraerDatosVenta(int idTransaccion)
        {
            NegocioPDV.PuntoVenta.VentasBusiness NegPuntoVenta = new NegocioPDV.PuntoVenta.VentasBusiness(UsuarioActual);

            RespuestaTransaccion mensaje = new RespuestaTransaccion();
            try
            {
                TransaccionesEnt data = NegPuntoVenta.ObtenerDocumentoPorId(idTransaccion);
                mensaje.Data = data;
            }
            catch (Exception e)
            {
                mensaje.EsError = true;
                mensaje.Mensaje = "Ocurrió un error al traer el detalle de la Transacción. Vuelva a intentarlo mas tarde";
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AnularVentaTransaccion(int idTransaccion, string idTrack, string episodio, string observacion, string asignacionId)
        {
            NegocioPDV.PuntoVenta.VentasBusiness NegPuntoVenta = new NegocioPDV.PuntoVenta.VentasBusiness(UsuarioActual);

            RespuestaTransaccion mensaje = new RespuestaTransaccion();
            try
            {
                mensaje = NegPuntoVenta.AnularVenta(idTransaccion, idTrack, episodio, observacion, UsuarioActual.Login, asignacionId);
            }
            catch (Exception e)
            {
                mensaje.EsError = true;
                mensaje.Mensaje = "Ocurrió un error al Anular la Transacción. Vuelva a intentarlo mas tarde.";
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AnularVentaAnticipo(int idTransaccion, string observacion)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            RespuestaTransaccion mensaje = new RespuestaTransaccion();
            try
            {
                mensaje = NegGarantia.CambiarEstadoGarantia(idTransaccion, EstadoGarantia.Devuelto, observacion);
            }
            catch (Exception e)
            {
                mensaje.EsError = true;
                mensaje.Mensaje = "Ocurrió un error al Anular la Transacción. Vuelva a intentarlo mas tarde.";
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AnularPagoRecibido(TipoTransaccion tipoDocumento, int pagoId, string observacion, string asignacionId)
        {
            NegocioPDV.PuntoVenta.VentasBusiness NegPuntoVenta = new NegocioPDV.PuntoVenta.VentasBusiness(UsuarioActual);

            RespuestaTransaccion mensaje = new RespuestaTransaccion();
            try
            {
                mensaje = NegPuntoVenta.CancelarPagoRecibido(pagoId, UsuarioActual.Login, observacion, asignacionId);
            }
            catch (Exception e)
            {
                mensaje.EsError = true;
                mensaje.Mensaje = "Ocurrió un error al Anular pago recibido. Vuelva a intentarlo mas tarde.";
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult IngresarGarantia(GarantiaEnt antGar)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                respuesta = NegGarantia.IngresoGarantia(antGar);
                respuesta.Mensaje = respuesta.EsError ? "No se logró guardar la garantía." : "Garantía guardada con el número " + respuesta.DocNum;
            }
            catch (System.Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult FinalizarPago(Documento pago)
        {
            NegocioPDV.PuntoVenta.VentasBusiness NegPuntoVenta = new NegocioPDV.PuntoVenta.VentasBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                pago.UsuarioCajaId = this.UsuarioActual.Login;
                pago.UsuarioCajaNombre = this.UsuarioActual.Nombre;

                respuesta = NegPuntoVenta.FinalizarPago(pago);
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al finalizar el pago. Vuelva a intentarlo mas tarde.";
            }

            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ImpresionOrdenAtencion(string imprimir)
        {
            NegocioPDV.PuntoVenta.VentasBusiness NegPuntoVenta = new NegocioPDV.PuntoVenta.VentasBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                DocumentoImpreso impresion = JsonConvert.DeserializeObject<DocumentoImpreso>(imprimir);
                NegPuntoVenta.ImprimirDocumento(impresion.IdTrack, impresion.IdAsignacionCaja);
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ObtenerPagoRecibido(int PagoId)
        {
            NegocioPDV.PuntoVenta.VentasBusiness NegPuntoVenta = new NegocioPDV.PuntoVenta.VentasBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                List<Documento> lista = new List<Documento>();
                Documento pago = NegPuntoVenta.ObtenerPagoRecibido(PagoId);

                if (pago != null)
                {
                    lista.Add(pago);
                }

                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener la información. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ObtenerListaPagos(TipoTransaccion tipoDocumento, int DocumentoId, int? DocumentoLinea = null, string TrackId = null)
        {
            NegocioPDV.PuntoVenta.VentasBusiness NegPuntoVenta = new NegocioPDV.PuntoVenta.VentasBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                if (tipoDocumento == TipoTransaccion.Imed)
                {
                    DocumentoId = 0;
                    DocumentoLinea = null;
                }
                else
                {
                    TrackId = null;
                }
                List<Documento> lista = NegPuntoVenta.ObtenerListaPagosRecibidos(DocumentoId, DocumentoLinea, TrackId);

                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener la información. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cierre Caja
        [HttpGet]
        public ActionResult MontoCierreCaja(string IdAsignacionCaja)
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                List<MontoCierreCaja> lista = NegAsignacion.ObtenerCajaMontosCierre(IdAsignacionCaja);
                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener la información. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AjusteMontoDiferencia(AsignacionCaja asignacion)
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                respuesta = NegAsignacion.AsignacionAjusteMontoDiferencia(asignacion);
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al guardar el ajuste de contado. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CierreCajaEfectivo(AsignacionCaja asignacion)
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                List<TransaccionesEnt> lista = this.ObtenerDocumentosUsuario();
                if (lista != null && lista.Count > 0 && lista.Exists(t => t.Tipo == TipoTransaccion.Boleta))
                {
                    respuesta.EsError = true;
                    respuesta.Mensaje = "Para realizar el cierre en efectivo, no debe existir transacciones pendientes de pago.";
                }
                else
                {
                    asignacion.Estado = EstadoAsignacion.CerradaEfectivo;
                    respuesta = NegAsignacion.AsignacionCerrarCaja(asignacion);
                }
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al intentar cerrar caja. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CierreCajaFinal(AsignacionCaja asignacion)
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            NegocioPDV.Menu.MenuBusiness menuBusiness = new NegocioPDV.Menu.MenuBusiness();

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                List<TransaccionesEnt> lista = this.ObtenerDocumentosUsuario();
                if (lista != null && lista.Count > 0)
                {
                    respuesta.EsError = true;
                    respuesta.Mensaje = "Para realizar el cierre, no debe existir transacciones pendientes pago o impresión.";
                }
                else
                {
                    asignacion.Estado = EstadoAsignacion.Cerrada;
                    respuesta = NegAsignacion.AsignacionCerrarCaja(asignacion);
                    UsuarioActual.Menu = menuBusiness.GetListaMenuUsuario(UsuarioActual); //Cargamos nuevamente el Menu para eliminar el link de caja
                }
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al intentar cerrar caja. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public ActionResult AplicarAnticipoBoleta(GarantiaEnt antGar)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                respuesta = NegGarantia.AplicarAnticipoBoleta(antGar);

                respuesta.Mensaje = respuesta.EsError ? "No se logró aplicar el anticipo." : "Anticipo aplicado con éxito";
            }
            catch (System.Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
    }
}