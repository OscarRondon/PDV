using EntidadesPDV;
using EntidadesPDV.Usuario;
using System.Collections.Generic;
using Entidades = EntidadesPDV.Nomina;

namespace NegocioPDV.Nomina
{
    public class NominaBusiness
    {
        DatosPDV.AccesoDatos.NominaDatos nominaAD = new DatosPDV.AccesoDatos.NominaDatos();
        public NominaBusiness()
        {

        }
        public NominaBusiness(UsuarioDV Info)
        {
            nominaAD.UsuarioActual = Info;
        }
        #region Actualizar Nomina
        public RespuestaTransaccion AdministrarDetalleNominaMasivo(List<Entidades.NominaDetalle> detalles)
        {
            return nominaAD.AdministrarNominaDetalleMasivo(detalles);
        }
        public RespuestaTransaccion GenerarNominaSupervisor(Entidades.NominaFiltro filtro)
        {
            return nominaAD.GenerarNominaSupervisor(filtro);
        }
        #endregion

        #region Obtener Listados
        public Entidades.Nomina ObtenerNominaPorNumero(int docEntryNomina)
        {
            return nominaAD.ObtenerNominaPorNumero(docEntryNomina);
        }
        public Entidades.Nomina ObtenerNominaSupervisorPorNumero(int docEntryNomina)
        {
            return nominaAD.ObtenerNominaSupervisorPorNumero(docEntryNomina);
        }

        public List<Entidades.Nomina> ObtenerListadoNomina(Entidades.NominaFiltro filtro)
        {           
            return nominaAD.ObtenerListadoNomina(filtro);
        }

        public List<Entidades.Nomina> ObtenerListadoNominaSupervisor(Entidades.NominaFiltro filtro)
        {
            return nominaAD.ObtenerListadoNominaSupervisor(filtro);
        }
        public List<Entidades.NominaDetalle> ObtenerListadoDetalleAprobado(Entidades.NominaFiltro filtro)
        {
            return nominaAD.ObtenerListadoDetalleAprobado(filtro);
        }
        #endregion
    }
}
