using EntidadesPDV;
using EntidadesPDV.Caja;
using EntidadesPDV.Folio;
using EntidadesPDV.Usuario;
using PDV.Clases;
using PDV.Filtros;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web.Mvc;

namespace PDV.Controllers
{
    public class ReportsController : ComunController
    {
        [HttpGet]
        public ActionResult ListaBoletabyRut(string impresora, string idHana)
        {
            //Comunes.ImpresionDocumentos.ExportReport(Server.MapPath("~/Reports/"), "BoletaServicios.rpt", impresora, idHana);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListaOrdenbyRut(string impresora, string idHana)
        {
            //Comunes.ImpresionDocumentos.ExportReport(Server.MapPath("~/Reports/"), "OrdenAtencion.rpt", impresora, idHana);
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult PartidasAbiertas()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReporteDocumentosAbiertos(string fecha)
        {
            DateTime dia = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Dictionary<string, string> parametros = new Dictionary<string, string>
            {
                { "DocKey@", dia.ToString("yyyyMMdd")}
            };


            RespuestaTransaccion exportReport = new RespuestaTransaccion() { EsError = true, Mensaje = "Impresión no generada" };
            try
            {
                exportReport = Comunes.ImpresionDocumentos.ExportReport(Server.MapPath(@"\Reports"), "DocPendientes.rpt", "", parametros, UtilesWeb.ConeccionReporte);
            }
            catch
            {

            }
            var jsonData = new
            {
                result = exportReport
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #region ReporteBoletaFolioCaja
        [AuthorizePDV]
        public ActionResult BoletasEmitidas()
        {
            ViewBag.Usuario = UsuarioActual.Login;
            if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.JefeAdmisión))
            {
                ViewBag.TipoPerfil = 3;
            }
            else if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.SupervisorCaja))
            {
                ViewBag.TipoPerfil = 2;
            }
            else if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.Admisionista))
            {
                ViewBag.TipoPerfil = 1;
            }
            else
            {
                ViewBag.TipoPerfil = 0;
            }

            if (ViewBag.TipoPerfil == 1) //Admisionista
            {
                return View("BoletasFolioCaja");
            }
            else
            {
                return View();
            }
        }

        [AuthorizePDV]
        public ActionResult BoletasFolioCaja()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ListaCajasAsignadas(string FechaInicio, string FechaFin)
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            RespuestaTransaccion mensaje = new RespuestaTransaccion();

            try
            {
                string[] fIni = FechaInicio.Replace('-', '/').Split('/');
                string[] fFin = FechaFin.Replace('-', '/').Split('/');
                DateTime fechaInicio = new DateTime(int.Parse(fIni[2]), int.Parse(fIni[1]), int.Parse(fIni[0]));
                DateTime fechafin = new DateTime(int.Parse(fFin[2]), int.Parse(fFin[1]), int.Parse(fFin[0]));

                List<AsignacionCaja> lista = new List<AsignacionCaja>();
                if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.SupervisorCaja))
                {
                    lista = NegAsignacion.GetListAsignacionCajasPorSupervisor(UsuarioActual.Login, fechaInicio, fechafin);
                }
                else if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.Admisionista))
                {
                    lista = NegAsignacion.GetListAsignacionCajasPorAdmisionista(UsuarioActual.Login, fechaInicio, fechafin);
                }


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
        #endregion

        #region ReporteResumenCaja
        [AuthorizePDV]
        public ActionResult ResumenArqueoCaja()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ListaCajasCerradas(string FechaInicio, string FechaFin)
        {
            NegocioPDV.Caja.AsignacionCajaBusiness NegAsignacion = new NegocioPDV.Caja.AsignacionCajaBusiness(UsuarioActual);
            RespuestaTransaccion mensaje = new RespuestaTransaccion();

            try
            {
                string[] fIni = FechaInicio.Replace('-', '/').Split('/');
                string[] fFin = FechaFin.Replace('-', '/').Split('/');
                DateTime fechaInicio = new DateTime(int.Parse(fIni[2]), int.Parse(fIni[1]), int.Parse(fIni[0]));
                DateTime fechafin = new DateTime(int.Parse(fFin[2]), int.Parse(fFin[1]), int.Parse(fFin[0]));

                List<AsignacionCaja> lista = new List<AsignacionCaja>();
                if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.SupervisorCaja))
                {
                    lista = NegAsignacion.GetListAsignacionCajasPorSupervisor(UsuarioActual.Login, fechaInicio, fechafin);
                }
                else if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.Admisionista))
                {
                    lista = NegAsignacion.GetListAsignacionCajasPorAdmisionista(UsuarioActual.Login, fechaInicio, fechafin);
                }


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
        #endregion

        public ActionResult DescargarReporteForPrint(string reportName, string paramName, string paramValue)
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();

            parametros.Add(paramName, string.Format("'{0}'", paramValue));

            byte[] reportBytes = Comunes.ImpresionDocumentos.ExportReportByte(Server.MapPath(@"\Reports"), reportName, parametros, UtilesWeb.ConeccionReporte);

            using (MemoryStream ms = new MemoryStream(reportBytes))
            {
                return File(ms.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, "Boleta.rpt");
            }
                //return File(new MemoryStream(reportBytes).ToArray(),
                //System.Net.Mime.MediaTypeNames.Application.Octet, "Boleta.rpt");
        }

        [AuthorizePDV]
        public ActionResult ReporteGlobalFolio()
        {
            ViewBag.Usuario = UsuarioActual.Login;
            if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.JefeAdmisión))
            {
                ViewBag.TipoPerfil = 3;
                return View();
            }
            else
            {
                return RedirectToAction("Home/UsuarioSinAcceso");
            }
        }

        [AuthorizePDV]
        public ActionResult ReporteAnulacionGarantias()
        {
            ViewBag.Usuario = UsuarioActual.Login;
            if (UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.JefeAdmisión))
            {
                ViewBag.TipoPerfil = 3;
                return View();
            }
            else
            {
                return RedirectToAction("Home/UsuarioSinAcceso");
            }
        }

        [HttpGet]
        public ActionResult ListaGlobalFolio(int? folioInicio, int? folioFin)
        {
            NegocioPDV.Folio.FolioBusiness NegFolio = new NegocioPDV.Folio.FolioBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                int folioDesde = folioInicio.HasValue ? folioInicio.Value : 0;
                int folioHasta = folioFin.HasValue ? folioFin.Value : 0;
                if (folioDesde > 0 && folioHasta > 0)
                {
                    List<FolioGlobal> lista = NegFolio.ObtenerListadoFolioGlobal(folioDesde, folioHasta);
                    respuesta.EsError = false;
                    respuesta.Data = lista;
                }
                else
                {
                    respuesta.EsError = true;
                    respuesta.Mensaje = "Folio ingresado no puede ser cero.";
                }
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = string.IsNullOrEmpty(ex.Message) ? "Ocurrió un error al obtener la información. Vuelva a intentarlo mas tarde." : ex.Message;
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [AuthorizePDV]
        public ActionResult ReportePagoAnulado()
        {
            NegocioPDV.Caja.CajaBusiness NegCaja = new NegocioPDV.Caja.CajaBusiness(UsuarioActual);
            NegocioPDV.Usuario.UsuarioBusiness NegUsuario = new NegocioPDV.Usuario.UsuarioBusiness(UsuarioActual);

            List<UsuarioDV> listUser = NegUsuario.GetListaUsuariosActivos();
            List<CajasEnt> listCaja = NegCaja.GetListaCajas(new CajasEnt());
            ViewBag.ListaAdmisionista = listUser != null ? listUser.FindAll(u => u.TienePerfil(PerfilUsuario.Admisionista)) : null;
            ViewBag.ListaSupervisor = listUser != null ? listUser.FindAll(u => u.TienePerfil(PerfilUsuario.SupervisorCaja)) : null;
            ViewBag.ListaCaja = listCaja;

            ViewBag.Usuario = UsuarioActual.Login;
            ViewBag.EsJefe = UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.JefeAdmisión);
            ViewBag.EsSupervisor = UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.SupervisorCaja);
            ViewBag.EsAdmision = UsuarioActual.TienePerfil(EntidadesPDV.Usuario.PerfilUsuario.Admisionista);

            if (ViewBag.EsJefe)
            {
                ViewBag.TipoPerfil = 3;
            }
            else if (ViewBag.EsSupervisor)
            {
                ViewBag.ListaCaja = listCaja.FindAll(c => c.IdSupervisorCaja == UsuarioActual.Login);
                ViewBag.TipoPerfil = 2;
            }
            else if (ViewBag.EsAdmision)
            {
                ViewBag.TipoPerfil = 1;
            }
            else
            {
                ViewBag.TipoPerfil = 0;
            }
            return View();
        }
    }
}