using EntidadesPDV;
using EntidadesPDV.Garantia;
using EntidadesPDV.PuntoVenta;
using PDV.Clases;
using PDV.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PDV.Controllers
{
    public class GarantiaController : ComunController
    {
        [AuthorizePDV]
        public ActionResult Index()
        {
            NegocioPDV.Lista.ListaGenericaBusiness NegListas = new NegocioPDV.Lista.ListaGenericaBusiness(UsuarioActual);

            ViewBag.ListaTipoGarantia = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.TipoGarantia);
            ViewBag.ListaEstadoGarantia = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.EstadoGarantia);
            return View();
        }
        [HttpGet]
        public ActionResult TraerDatosBanco()
        {
            NegocioPDV.Lista.ListaGenericaBusiness NegListas = new NegocioPDV.Lista.ListaGenericaBusiness(UsuarioActual);

            List<SelectListItem> ListaBancos = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Banco).Select(s => new SelectListItem() { Value = s.Codigo, Text = s.Descripcion }).ToList();
            var jsonData = new
            {
                data = ListaBancos
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TraerDatosComunas()
        {
            NegocioPDV.Lista.ListaGenericaBusiness NegListas = new NegocioPDV.Lista.ListaGenericaBusiness(UsuarioActual);

            List<SelectListItem> LstComunas = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Comuna).Select(s => new SelectListItem() { Value = s.Codigo, Text = s.Descripcion }).ToList();
            var jsonData = new
            {
                data = LstComunas
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult ListaBoletasActivas()
        {
            NegocioPDV.PuntoVenta.VentasBusiness NegPuntoVenta = new NegocioPDV.PuntoVenta.VentasBusiness(UsuarioActual);

            List<TransaccionesEnt> lista = NegPuntoVenta.BoletasAbiertas(new TransaccionesEnt());
            List<SelectListItem> lst = lista.Select(s => new SelectListItem() { Value = s.IdTransaccion.ToString(), Text = s.IdTrack }).ToList();
            var jsonData = new
            {
                data = lst
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ObtenerGarantiaPorId(string idGarantia)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            RespuestaTransaccion resultado = new RespuestaTransaccion();
            try
            {
                GarantiaEnt garantia = NegGarantia.GetGarantiasPorId(Convert.ToInt32(idGarantia));
                resultado.Data = garantia;
            }
            catch (Exception)
            {
                resultado.EsError = true;
                resultado.Mensaje = "Ocurrió un error al traer la garantía. Vuelva a intentarlo mas tarde.";
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListadoGarantiasRut(string rutResponsable, string fechaInicio)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                DateTime fechaInicioDate = DateTime.MinValue;
                if (!String.IsNullOrEmpty(fechaInicio))
                {
                    string[] dtIni = fechaInicio.Split('-');
                    fechaInicioDate = new DateTime(Convert.ToInt32(dtIni[2]), Convert.ToInt32(dtIni[1]), Convert.ToInt32(dtIni[0]));
                }
                List<GarantiaEnt> lista = NegGarantia.GetListaGarantiasPorParametros(new GarantiaFiltro() { ResponsableRut = rutResponsable, FechaInicio = fechaInicioDate });
                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al traer el listado de garantías. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListadoGarantiasFechas(string fechaInicio, string fechaFin, string estado, string numero, string responsable, string paciente, string episodio, string tipo)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                int estadoGarantia = string.IsNullOrEmpty(estado) ? 0 : int.Parse(estado.Trim());
                DateTime? FechaInicio = new DateTime?();
                DateTime? FechaFin = new DateTime?();
                if (!string.IsNullOrEmpty(fechaInicio))
                {
                    string[] dtIni = fechaInicio.Replace("/", "-").Split('-');
                    FechaInicio = new DateTime(Convert.ToInt32(dtIni[2]), Convert.ToInt32(dtIni[1]), Convert.ToInt32(dtIni[0]));
                }
                if (!string.IsNullOrEmpty(fechaFin))
                {
                    string[] dtFin = fechaFin.Replace("/", "-").Split('-');
                    FechaFin = new DateTime(Convert.ToInt32(dtFin[2]), Convert.ToInt32(dtFin[1]), Convert.ToInt32(dtFin[0]));
                }

                GarantiaFiltro filtro = new GarantiaFiltro();
                filtro.FechaInicio = FechaInicio;
                filtro.FechaFin = FechaFin;
                filtro.Estado = (EstadoGarantia)estadoGarantia;
                filtro.DocNum = string.IsNullOrEmpty(numero) ? new int?() : int.Parse(numero);
                filtro.ResponsableRut = responsable;
                filtro.PacienteRut = paciente;
                filtro.Episodio = episodio;
                filtro.TipoDocumento = tipo;

                List<GarantiaEnt> lista = NegGarantia.GetListaGarantiasPorParametros(filtro);

                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al traer el listado de garantías. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DevolucionGarantias()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DevolverGarantia(string DocEntry, string estado)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            RespuestaTransaccion resp = NegGarantia.CambiarEstadoGarantia(
                int.Parse(DocEntry),
                (EstadoGarantia)(int.Parse(estado)),
                string.Empty);

            var jsonData = new
            {
                resultGarantia = resp
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Imprimir(string DocEntry)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            Dictionary<string, string> parametros = new Dictionary<string, string>
            {
                { "Param01", DocEntry}
            };
            GarantiaEnt garantia = NegGarantia.GetGarantiasPorId(Convert.ToInt32(DocEntry));

            RespuestaTransaccion exportReport = new RespuestaTransaccion() { EsError = true, Mensaje = "Impresión no generada" };
            if (garantia != null)
            {
                switch (garantia.TipoDocumentoTexto.ToUpper())
                {
                    case "PAGARÉ":
                        if (garantia.GarEstado != EstadoGarantia.Devuelto)
                        {
                            Dictionary<string, string> listParam = new Dictionary<string, string>
                            {
                                { "Param01", garantia.GarDocNum.ToString()}
                            };
                            exportReport = Comunes.ImpresionDocumentos.ExportReport(Server.MapPath(@"\Reports"), "PagareImpresion.rpt", "", listParam, UtilesWeb.ConeccionReporte);
                        }
                        else
                        {
                            exportReport = Comunes.ImpresionDocumentos.ExportReport(Server.MapPath(@"\Reports"), "DevolucionPagare.rpt", "", parametros, UtilesWeb.ConeccionReporte);
                        }
                        break;
                    default:
                        parametros = new Dictionary<string, string>
                                {
                                    { "DocEntry", DocEntry}
                                };
                        if (garantia.GarEstado == EstadoGarantia.Activo)
                        {
                            exportReport = Comunes.ImpresionDocumentos.ExportReport(Server.MapPath(@"\Reports"), "ReciboGarantia.rpt", "", parametros, UtilesWeb.ConeccionReporte);
                        }
                        else if (garantia.GarEstado == EstadoGarantia.Aplicado)
                        {
                            exportReport = Comunes.ImpresionDocumentos.ExportReport(Server.MapPath(@"\Reports"), "AplicacionGarantia.rpt", "", parametros, UtilesWeb.ConeccionReporte);
                        }
                        else if (garantia.GarEstado == EstadoGarantia.Devuelto)
                        {
                            exportReport = Comunes.ImpresionDocumentos.ExportReport(Server.MapPath(@"\Reports"), "DevolucionGarantia.rpt", "", parametros, UtilesWeb.ConeccionReporte);

                        }
                        break;
                }
            }

            var jsonData = new
            {
                resultGarantia = exportReport
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AgregarComentario(string DocEntry, string Comentario)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            RespuestaTransaccion resp = NegGarantia.AgregarComentario(DocEntry, Comentario);
            var jsonData = new
            {
                resultGarantia = resp
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult TraerDatosBoleta(string idTrans, string idTrack)
        {
            NegocioPDV.PuntoVenta.VentasBusiness NegPuntoVenta = new NegocioPDV.PuntoVenta.VentasBusiness(UsuarioActual);

            TransaccionesEnt doc = new TransaccionesEnt()
            {
                IdTrack = string.IsNullOrEmpty(idTrack) ? string.Empty : idTrack,
                IdTransaccion = string.IsNullOrEmpty(idTrans) ? 0 : int.Parse(idTrans)
            };
            List<TransaccionesEnt> lista = NegPuntoVenta.BoletasAbiertas(doc);
            var jsonData = new
            {
                data = lista.FirstOrDefault()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult IngresoGarantia(GarantiaEnt garantia)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            RespuestaTransaccion msg = new RespuestaTransaccion();
            try
            {

                msg = NegGarantia.IngresoGarantia(garantia);
                msg.Mensaje = msg.EsError ? "No se logró guardar la garantía." : "Registro guardado correctamente con el número " + msg.DocNum;

            }
            catch (System.Exception ex)
            {
                msg.EsError = true;
                msg.Mensaje = "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetListPagosbyRut(string RutResponsable)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            List<GarantiaEnt> lista = NegGarantia.GetListaGarantiasPorParametros(new GarantiaFiltro() { ResponsableRut = RutResponsable });
            var jsonData = new
            {
                data = lista
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ListadoGarantiasCaja(string asignacion)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                List<GarantiaEnt> lista = NegGarantia.GetListaGarantiasPorParametros(new GarantiaFiltro() { IdAsignacion = asignacion });
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

        [HttpGet]
        public ActionResult BuscarAnticipoEpisodio(string episodio, string rutRes)
        {
            NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness(UsuarioActual);

            GarantiaFiltro garantia = new GarantiaFiltro()
            {
                TipoDocumento = "A",
                Episodio = episodio,
                ResponsableRut = rutRes,
                Estado = EstadoGarantia.Activo
            };

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {

                var jsonData = new
                {
                    data = NegGarantia.GetListaGarantiasPorParametros(garantia),
                    EsError = false
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al traer el detalle de los anticipos. Vuelva a intentarlo mas tarde";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
    }
}
