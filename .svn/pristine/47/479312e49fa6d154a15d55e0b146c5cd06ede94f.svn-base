using EntidadesPDV;
using EntidadesPDV.Caja;
using EntidadesPDV.Folio;
using EntidadesPDV.Usuario;
using System.Collections.Generic;
using Entidades = EntidadesPDV.Folio;

namespace NegocioPDV.Folio
{
    public class FolioBusiness
    {
        DatosPDV.AccesoDatos.FolioDatos FolioAD = new DatosPDV.AccesoDatos.FolioDatos();
        public FolioBusiness()
        {

        }
        public FolioBusiness(UsuarioDV Info)
        {
            FolioAD.UsuarioActual = Info;
        }
        #region Actualizar Folio
        public RespuestaTransaccion GuardarLimiteFolio(Entidades.Folio folio)
        {
            return FolioAD.GuardarLimiteFolio(folio);
        }
        public RespuestaTransaccion GuardarFolio(Entidades.Folio folio)
        {
            return FolioAD.GuardarFolio(folio);
        }
        public RespuestaTransaccion GuardarFolioSegmentoMasivo(List<Entidades.Folio> listaFolio)
        {
            return FolioAD.GuardarFolioSegmentoMasivo(listaFolio);
        }
        public RespuestaTransaccion AsignarCajaSegmentoMasivo(List<FolioSegmento> listado)
        {
            return FolioAD.AsignarCajaSegmentoMasivo(listado);
        }
        public RespuestaTransaccion AnularFolio(FolioAnulado folio)
        {
            return FolioAD.AnularFolio(folio);
        }
        public RespuestaTransaccion AutorizarAnulacionFolio(FolioAnulado folio)
        {
            return FolioAD.AutorizarAnulacionFolio(folio);
        }
        #endregion

        #region Obtener Listados
        public Dictionary<string, string> ListaDocumentos()
        {
            Dictionary<string, string> lista = new Dictionary<string, string>();
            lista.Add("38", "Boleta Exenta");
            return lista;
        }
        public int ObtenerUltimoLimiteFolio()
        {
            return FolioAD.ObtenerUltimoLimiteFolio();
        }
        public int ObtenerUltimoFolio()
        {
            return FolioAD.ObtenerUltimoFolio();
        }

        public List<Entidades.Folio> ObtenerListadoLimiteFolio()
        {
            return FolioAD.ObtenerListadoLimiteFolio();
        }
        public List<Entidades.Folio> ObtenerListadoFolio(FolioFiltro filtro)
        {
            return FolioAD.ObtenerListadoFolio(filtro);
        }
        public List<Entidades.FolioSegmento> ObtenerListadoSegmentos(string FolioId)
        {
            return FolioAD.ObtenerListadoSegmentos(FolioId);
        }

        public List<FolioSegmento> ObtenerListadoFolioCajaPorUsuario(string usuario)
        {
            return FolioAD.ObtenerListadoFolioCajaPorUsuario(usuario);
        }
        public List<FolioAnulado> ObtenerListadoFolioAnulados(FolioFiltro filtro)
        {
            return FolioAD.ObtenerListadoFolioAnulados(filtro);
        }
        public List<FolioAnulado> ObtenerListadoFolioAnuladosCajaPorSupervisor(FolioFiltro filtro)
        {
            return FolioAD.ObtenerListadoFolioAnuladosCajaPorSupervisor(filtro);
        }

        public List<FolioGlobal> ObtenerListadoFolioGlobal(int FolioIni, int FolioFin)
        {
            return FolioAD.ObtenerListadoFolioGlobal(FolioIni, FolioFin);
        }

        public FolioSegmento ObtenerSiguienteFolioPorCaja(string IdCaja)
        {
            return FolioAD.ObtenerSiguienteFolioPorCaja(IdCaja);
        }
        #endregion
    }
}
