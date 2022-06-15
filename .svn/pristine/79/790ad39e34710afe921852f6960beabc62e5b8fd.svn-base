using EntidadesPDV;
using EntidadesPDV.Nomina;
using EntidadesPDV.Usuario;
using Newtonsoft.Json;
using PDV.Filtros;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PDV.Controllers
{
    public class NominaController : ComunController
    {
        [AuthorizePDV]
        public ActionResult AdministrarNominas()
        {
            return View();
        }
        public ActionResult PanelFiltroBusqueda()
        {
            NegocioPDV.Caja.CajaBusiness NegCaja = new NegocioPDV.Caja.CajaBusiness(UsuarioActual);
            NegocioPDV.Lista.ListaGenericaBusiness NegListas = new NegocioPDV.Lista.ListaGenericaBusiness(UsuarioActual);

            ViewBag.Usuario = UsuarioActual;


            ViewBag.ListaFunciones = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Funcion);
            ViewBag.ListaPisos = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Piso);
            ViewBag.ListaSecciones = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Seccion);

            ViewBag.ListaTipoNomina = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.TipoNominaAdmisionista);
            ViewBag.ListaEstadosNominaDetalle = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.EstadoNominaDetalle);
            ViewBag.ListaCajasSupervisor = NegCaja.GetListaCajas(new EntidadesPDV.Caja.CajasEnt() { IdSupervisorCaja = UsuarioActual.Login });


            return PartialView("~/Views/Shared/Nomina/_PartialNominaBusqueda.cshtml");
        }

        [AuthorizePDV]
        public ActionResult AdministrarNominasSupervisor()
        {
            NegocioPDV.Lista.ListaGenericaBusiness NegListas = new NegocioPDV.Lista.ListaGenericaBusiness(UsuarioActual);

            ViewBag.ListaTipoNomina = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.TipoNominaSupervisor);
            ViewBag.ListaEstadosNominaDetalle = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.EstadoNominaDetalle);

            return View();
        }

        [AuthorizePDV]
        public ActionResult GenerarNomina()
        {
            NegocioPDV.Lista.ListaGenericaBusiness NegListas = new NegocioPDV.Lista.ListaGenericaBusiness(UsuarioActual);

            ViewBag.ListaFunciones = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.Funcion);
            ViewBag.ListaTipoNomina = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.TipoNominaSupervisor);
            ViewBag.ListaTipoDocumento = NegListas.ObtenerListado<ItemList>(NegocioPDV.Lista.ListaDatosEnum.TipoDocumentoDetalle);

            return View();
        }

        [HttpPost]
        public ActionResult ListadoNominasAprobadas(NominaFiltro filtro)
        {
            NegocioPDV.Nomina.NominaBusiness NegNomina = new NegocioPDV.Nomina.NominaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                filtro.UsuarioId = UsuarioActual.Login;
                filtro.UsuarioPerfil = UsuarioActual.NombrePerfilStoreProcedure();

                List<NominaDetalle> lista = NegNomina.ObtenerListadoDetalleAprobado(filtro);
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
        public ActionResult GenerarNominaSupervisor(NominaFiltro filtro)
        {
            NegocioPDV.Nomina.NominaBusiness NegNomina = new NegocioPDV.Nomina.NominaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                filtro.UsuarioId = UsuarioActual.Login;
                filtro.UsuarioPerfil = UsuarioActual.NombrePerfilStoreProcedure();
                filtro.SupervisorNombre = UsuarioActual.Nombre;

                respuesta = NegNomina.GenerarNominaSupervisor(filtro);
                respuesta.EsError = false;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al generar la nómina. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BuscarNominas(NominaFiltro filtro)
        {
            NegocioPDV.Nomina.NominaBusiness NegNomina = new NegocioPDV.Nomina.NominaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                filtro.UsuarioId = UsuarioActual.Login;
                filtro.UsuarioPerfil = UsuarioActual.NombrePerfilStoreProcedure();

                List<Nomina> lista = NegNomina.ObtenerListadoNomina(filtro);
                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener el listado de nóminas. Vuelva a intentarlo mas tarde.";
            }

            var json = Json(respuesta, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
            //return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BuscarDetalleNomina(int docEntryNomina)
        {
            NegocioPDV.Nomina.NominaBusiness NegNomina = new NegocioPDV.Nomina.NominaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                Nomina resultado = NegNomina.ObtenerNominaPorNumero(docEntryNomina);

                respuesta.EsError = false;
                respuesta.Data = resultado;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener la nómina. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BuscarNominasSupervisor(NominaFiltro filtro)
        {
            NegocioPDV.Nomina.NominaBusiness NegNomina = new NegocioPDV.Nomina.NominaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                filtro.UsuarioId = UsuarioActual.Login;

                List<Nomina> lista = NegNomina.ObtenerListadoNominaSupervisor(filtro);

                respuesta.EsError = false;
                respuesta.Data = lista;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener el listado de nóminas. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult BuscarDetalleNominaSupervisor(int docEntryNomina)
        {
            NegocioPDV.Nomina.NominaBusiness NegNomina = new NegocioPDV.Nomina.NominaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                Nomina resultado = NegNomina.ObtenerNominaSupervisorPorNumero(docEntryNomina);

                respuesta.EsError = false;
                respuesta.Data = resultado;
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al obtener la nómina. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarDetalleNomina(string listaDetalle)
        {
            NegocioPDV.Nomina.NominaBusiness NegNomina = new NegocioPDV.Nomina.NominaBusiness(UsuarioActual);

            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                List<NominaDetalle> lista = JsonConvert.DeserializeObject<List<NominaDetalle>>(listaDetalle);
                respuesta = NegNomina.AdministrarDetalleNominaMasivo(lista);
            }
            catch (Exception ex)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ocurrió un error al guardar el detalle de la nómina. Vuelva a intentarlo mas tarde.";
            }
            return Json(respuesta, JsonRequestBehavior.AllowGet);
        }
    }
}