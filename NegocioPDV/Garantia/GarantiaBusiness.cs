using EntidadesPDV;
using EntidadesPDV.Garantia;
using EntidadesPDV.Usuario;
using System.Collections.Generic;

namespace NegocioPDV.Garantia
{
    public class GarantiaBusiness
    {
        DatosPDV.AccesoDatos.GarantiaDatos datos = new DatosPDV.AccesoDatos.GarantiaDatos();
        public GarantiaBusiness()
        {

        }
        public GarantiaBusiness(UsuarioDV Info)
        {
            datos.UsuarioActual = Info;
        }
        public RespuestaTransaccion IngresoGarantia(GarantiaEnt garantia)
        {
            return datos.IngresoGarantia(garantia);
        }
        public GarantiaEnt GetGarantiasPorId(int idGarantia)
        {
            return datos.GetGarantiasPorId(idGarantia);
        }

        public List<GarantiaEnt> GetListaGarantiasPorParametros(GarantiaFiltro filtro)
        {
            return datos.GetListaGarantiasPorParametros(filtro);
        }

        public RespuestaTransaccion AgregarComentario(string numeroGarantia, string Comentario)
        {
            RespuestaTransaccion resp = new RespuestaTransaccion();
            resp = datos.AgregarComentario(numeroGarantia, Comentario);
            return resp;
        }

        public RespuestaTransaccion CambiarEstadoGarantia(int idGarantia, EstadoGarantia estado, string observacion)
        {
            return datos.CambiarEstadoGarantia(idGarantia, estado, observacion);
        }

        public RespuestaTransaccion AplicarAnticipoBoleta(GarantiaEnt garantia)
        {
            return datos.AplicarAnticipoBoleta(garantia);
        }
    }
}
