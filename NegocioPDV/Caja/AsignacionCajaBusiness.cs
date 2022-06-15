using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatosPDV.AccesoDatos;
using DatosPDV.SRCaja;
using EntidadesPDV;
using EntidadesPDV.Caja;
using EntidadesPDV.Folio;
using EntidadesPDV.Usuario;

namespace NegocioPDV.Caja
{
    public class AsignacionCajaBusiness
    {
        AsignacionCajaDatos asignacionAD = new AsignacionCajaDatos();
        CajaDatos cajaAD = new DatosPDV.AccesoDatos.CajaDatos();
        NoObjectDatos cajasAD = new NoObjectDatos();
        CajaBusiness cajaBus = new CajaBusiness();

        public AsignacionCajaBusiness()
        {

        }
        public AsignacionCajaBusiness(UsuarioDV Info)
        {
            asignacionAD.UsuarioActual = Info;
            cajaAD.UsuarioActual = Info;
            cajasAD.UsuarioActual = Info;
        }

        #region Modificar Asignacion
        public bool AsignacionNueva(AsignacionCaja asignacion)
        {
            return asignacionAD.AsignacionNueva(asignacion);
        }
        public bool AsignacionModificar(AsignacionCaja asignacion)
        {
            return asignacionAD.AsignacionModificar(asignacion);
        }
        public RespuestaTransaccion AsignacionIniciarCaja(AsignacionCaja asignacion)
        {
            return asignacionAD.AsignacionIniciarCaja(asignacion);
        }
        public bool AsignacionModificarTransbank(AsignacionCaja asignacion)
        {
            return asignacionAD.AsignacionModificarTransbank(asignacion);
        }
        public bool AsignacionRechazarCaja(AsignacionCaja asignacion)
        {
            return asignacionAD.AsignacionRechazarCaja(asignacion);
        }
        public RespuestaTransaccion AsignacionAjusteMontoDiferencia(AsignacionCaja asignacion)
        {
            return asignacionAD.AsignacionAjusteMontoDiferencia(asignacion);
        }
        public RespuestaTransaccion AsignacionCerrarCaja(AsignacionCaja asignacion)
        {
            return asignacionAD.AsignacionCerrarCaja(asignacion);
        }

        public RespuestaTransaccion AsignacionNuevaMasivo(List<AsignacionCajaType> listaAsignaciones)
        {
            return asignacionAD.AsignacionNuevaMasivo(listaAsignaciones);
        }
        #endregion

        #region Obtener Asignacion
        public AsignacionCaja GetCajaAsignadaPorId(string idAsignacion)
        {
            return asignacionAD.ObtenerCajaAsignadaPorId(idAsignacion);
        }

        public List<AsignacionCaja> GetListAsignacionCajasPorSupervisor(string idSupervisor, DateTime? fechaAsignacion, bool vigente)
        {
            AsignacionCajaFiltro filtro = new AsignacionCajaFiltro();
            filtro.IdSupervisorCaja = idSupervisor;
            filtro.FechaAsignacionInicio = fechaAsignacion;
            filtro.FechaAsignacionFinal = fechaAsignacion;
            filtro.EsVigente = vigente;

            return asignacionAD.ObtenerCajasAsignadas(filtro);
        }

        public List<AsignacionCaja> GetListAsignacionCajasPorSupervisor(string idSupervisor, DateTime fechaAsignacionInicio, DateTime fechaAsignacionFin)
        {
            AsignacionCajaFiltro filtro = new AsignacionCajaFiltro();
            filtro.IdSupervisorCaja = idSupervisor;
            filtro.FechaAsignacionInicio = fechaAsignacionInicio;
            filtro.FechaAsignacionFinal = fechaAsignacionFin;

            return asignacionAD.ObtenerCajasAsignadas(filtro);
        }
        public List<AsignacionCaja> GetListAsignacionCajasPorAdmisionista(string idAdmisionista, DateTime fechaAsignacionInicio, DateTime fechaAsignacionFin)
        {
            AsignacionCajaFiltro filtro = new AsignacionCajaFiltro();
            filtro.IdUsuarioAsignado = idAdmisionista;
            filtro.FechaAsignacionInicio = fechaAsignacionInicio;
            filtro.FechaAsignacionFinal = fechaAsignacionFin;

            return asignacionAD.ObtenerCajasAsignadas(filtro);
        }
        public List<AsignacionCaja> GetListAsignacionCajasPorUsuarioAsignado(string idUsuario, bool vigente)
        {
            AsignacionCajaFiltro filtro = new AsignacionCajaFiltro();
            filtro.IdUsuarioAsignado = idUsuario;
            filtro.EsVigente = vigente;

            return asignacionAD.ObtenerCajasAsignadas(filtro);
        }

        public List<MontoCierreCaja> ObtenerCajaMontosCierre(string AsignacionId)
        {
            return asignacionAD.ObtenerCajaMontosCierre(AsignacionId);
        }
        #endregion
    }
}
