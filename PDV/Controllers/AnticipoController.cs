using EntidadesPDV;
using EntidadesPDV.Garantia;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PDV.Controllers
{
    public class AnticipoController : ComunController
    {

        NegocioPDV.Garantia.GarantiaBusiness NegGarantia = new NegocioPDV.Garantia.GarantiaBusiness();

        //JM[HttpPost]
        //public ActionResult IngresoGarantia(GarantiaEnt garantia)
        //{
        //    RespuestaTransaccion msg = new RespuestaTransaccion();
        //    try
        //    {

        //        msg = NegGarantia.IngresoGarantia(garantia);
        //        msg.Mensaje = msg.EsError ? "No se logró guardar la garantía." : "Registro guardado correctamente con el número " + msg.DocNum;

        //    }
        //    catch (System.Exception ex)
        //    {
        //        msg.EsError = true;
        //        msg.Mensaje = "Ocurrió un error al intentar guardar la información. Vuelva a intentarlo mas tarde.";
        //    }
        //    return Json(msg, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult GetListPagosbyRut(string RutResponsable)
        //{
        //    List<GarantiaEnt> lista = NegGarantia.GetListaGarantiasPorParametros(new GarantiaFiltro() { ResponsableRut = RutResponsable });
        //    var jsonData = new
        //    {
        //        data = lista
        //    };
        //    return Json(jsonData, JsonRequestBehavior.AllowGet);
        //}
    }
}
