using System.Web.Mvc;
using EntidadesPDV;
using EntidadesPDV.Caja;
using EntidadesPDV.Folio;
using System;
using System.Collections.Generic;
using System.Linq;
using EntidadesPDV.Usuario;
using System.ServiceModel;
using System.ServiceModel.Channels;
using DatosPDV.SRCaja;

namespace DatosPDV.AccesoDatos
{
    public class AsignacionCajaDatos : ComunDatos
    {
        SRCaja.CajaClient cajaCliente = new SRCaja.CajaClient();

        #region Actualizar Asignacion
        public bool AsignacionNueva(AsignacionCaja asignacion)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                try
                {
                    SRCaja.AsignacionCajaType asignacionNueva = new SRCaja.AsignacionCajaType();
                    asignacionNueva.IdCaja = asignacion.CajaAsignada.IdCaja;
                    asignacionNueva.IdUsuarioAsignado = asignacion.IdUsuarioAsignado;
                    asignacionNueva.NombreUsuarioAsignado = asignacion.UsuarioAsignado;
                    asignacionNueva.MontoInicial = asignacion.MontoFijo;
                    asignacionNueva.EstadoAsignacion = EstadoAsignacionCaja.ObtenerCodigoPorTipo(EstadoAsignacion.NoIniciada);

                    asignacionNueva.IdTipo = asignacion.CajaAsignada.IdFuncionalidadCaja;
                    asignacionNueva.TipoDescripcion = asignacion.CajaAsignada.FuncionalidadCaja;


                    SRCaja.RespuestaType rsp = cajaCliente.AsignarCaja(asignacionNueva);
                    if (rsp.Error)
                    {
                        throw new ExceptionControlada("Ocurrió un error al guardar la nueva asignacion. Vuelva a intentarlo mas tarde.");
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return false;
            }
        }
        public bool AsignacionModificar(AsignacionCaja asignacion)
        {
            try
            {
                SRCaja.AsignacionCajaType asignacionActualizar = ObtenerAsignacionBase(asignacion.IdAsignacion);
                asignacionActualizar.IdUsuarioAsignado = asignacion.IdUsuarioAsignado;
                asignacionActualizar.NombreUsuarioAsignado = asignacion.UsuarioAsignado;
                asignacionActualizar.MontoInicial = asignacion.MontoFijo;

                asignacionActualizar.IdTipo = asignacion.CajaAsignada.IdFuncionalidadCaja;
                asignacionActualizar.TipoDescripcion = asignacion.CajaAsignada.FuncionalidadCaja;
                using (new OperationContextScope(cajaCliente.InnerChannel))
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                    {
                        Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                    };

                    SRCaja.RespuestaType rsp = cajaCliente.AsignarCaja(asignacionActualizar);
                    if (rsp.Error)
                    {
                        throw new ExceptionControlada("Ocurrió un error al guardar la nueva asignacion. Vuelva a intentarlo mas tarde.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        public RespuestaTransaccion AsignacionIniciarCaja(AsignacionCaja asignacion)
        {
            SRCaja.AsignacionCajaType asignacionActualizar = ObtenerAsignacionBase(asignacion.IdAsignacion);
            asignacionActualizar.FechaInicioCaja = DateTime.Now;
            asignacionActualizar.NumeroMaquinaTransbank = asignacion.NumeroMaquinaTransbank;
            asignacionActualizar.EstadoAsignacion = EstadoAsignacionCaja.ObtenerCodigoPorTipo(EstadoAsignacion.Iniciada);

            SRCaja.RespuestaType rsp = null;
            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            if (asignacionActualizar.HabilitaCaja)
            {
                using (new OperationContextScope(cajaCliente.InnerChannel))
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                    {
                        Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                    };

                    rsp = cajaCliente.AsignarCaja(asignacionActualizar);

                    respuesta.EsError = rsp.Error;
                    respuesta.Mensaje = rsp.Mensaje;
                }
            }
            else
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Caja no esta habilitada para iniciar.";
            }

            return respuesta;
        }
        public bool AsignacionModificarTransbank(AsignacionCaja asignacion)
        {
            SRCaja.AsignacionCajaType asignacionActualizar = ObtenerAsignacionBase(asignacion.IdAsignacion);
            asignacionActualizar.NumeroMaquinaTransbank = asignacion.NumeroMaquinaTransbank;

            SRCaja.RespuestaType rsp = null;
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                rsp = cajaCliente.AsignarCaja(asignacionActualizar);
            }
            return rsp.Error;
        }
        public bool AsignacionRechazarCaja(AsignacionCaja asignacion)
        {
            SRCaja.AsignacionCajaType asignacionActualizar = ObtenerAsignacionBase(asignacion.IdAsignacion);
            asignacionActualizar.FechaRechazoCaja = DateTime.Now;
            asignacionActualizar.ObservacionRechazoCaja = asignacion.ObservacionRechazo.ToUpper();
            asignacionActualizar.EstadoAsignacion = EstadoAsignacionCaja.ObtenerCodigoPorTipo(EstadoAsignacion.Rechazada);

            SRCaja.RespuestaType rsp = null;
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                rsp = cajaCliente.AsignarCaja(asignacionActualizar);
                if (!rsp.Error)
                {
                    SRCaja.CajaType cajaActualizar = cajaCliente.GetCajaByParams(new SRCaja.CajaType() { IdCaja = asignacionActualizar.IdCaja });
                    cajaActualizar.HabilitaCaja = true;
                    SRCaja.RespuestaType rspCaja = cajaCliente.AdministrarCaja(cajaActualizar);
                    if (rspCaja.Error)
                    {
                        throw new ExceptionControlada("Ocurrió un error al actualizar la caja asignada. Vuelva a intentarlo mas tarde.");
                    }
                }
            }
            return rsp.Error;
        }
        public RespuestaTransaccion AsignacionAjusteMontoDiferencia(AsignacionCaja asignacion)
        {
            SRCaja.AsignacionCajaType asignacionActualizar = new SRCaja.AsignacionCajaType();
            asignacionActualizar.IdAsignacion = asignacion.IdAsignacion;
            asignacionActualizar.MontoDiferencia = asignacion.MontoDiferencia;

            SRCaja.RespuestaType rsp = cajaCliente.AsignarCajaAjusteCierre(asignacionActualizar);
            RespuestaTransaccion salida = new RespuestaTransaccion();

            salida.EsError = rsp.Error;
            salida.Mensaje = rsp.Mensaje;
            return salida;
        }
        public RespuestaTransaccion AsignacionCerrarCaja(AsignacionCaja asignacion)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRCaja.AsignacionCajaType asignacionActualizar = new SRCaja.AsignacionCajaType()
                {
                    IdAsignacion = asignacion.IdAsignacion,
                    MontoCierre = asignacion.MontoCierre,
                    MontoRecaudado = asignacion.MontoRecaudado,
                    MontoDiferencia = asignacion.MontoDiferencia,
                    ObservacionDiferencia = asignacion.ObservacionDiferencia,
                    EstadoAsignacion = EstadoAsignacionCaja.ObtenerCodigoPorTipo(asignacion.Estado)
                };

                RespuestaTransaccion result = new RespuestaTransaccion();
                SRCaja.RespuestaType rsp = cajaCliente.AsignarCajaCierre(asignacionActualizar);

                result.EsError = rsp.Error;
                result.Mensaje = rsp.Mensaje;

                return result;
            }
        }
        public RespuestaTransaccion AsignacionNuevaMasivo(List<AsignacionCajaType> listaAsignaciones)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                RespuestaTransaccion salida = new RespuestaTransaccion();
                SRCaja.RespuestaType rsp = cajaCliente.AsignarCajaMasivo(listaAsignaciones);

                salida.EsError = rsp.Error;
                salida.Mensaje = rsp.Mensaje;
                return salida;
            }
        }
        #endregion

        #region Obtener Asignacion
        public AsignacionCaja ObtenerCajaAsignadaPorId(string AsignacionId)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRCaja.AsignacionCajaType asignacion = this.ObtenerAsignacionBase(AsignacionId);

                AsignacionCaja salida = AsignacionParse(asignacion);

                return salida;
            }
        }
        public List<AsignacionCaja> ObtenerCajasAsignadas(AsignacionCajaFiltro filtro)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRCaja.AsignacionCajaFilterType asigCaja = new SRCaja.AsignacionCajaFilterType();
                asigCaja.EsVigente = filtro.EsVigente;
                asigCaja.FechaAsignacionFinal = filtro.FechaAsignacionFinal;
                asigCaja.FechaAsignacionInicio = filtro.FechaAsignacionInicio;
                asigCaja.FechaCierreFinal = filtro.FechaCierreFinal;
                asigCaja.FechaCierreInicio = filtro.FechaCierreInicio;
                asigCaja.IdAsignacion = filtro.IdAsignacion;
                asigCaja.IdCaja = filtro.IdCaja;
                asigCaja.IdSupervisorCaja = filtro.IdSupervisorCaja;
                asigCaja.IdUsuarioAsignado = filtro.IdUsuarioAsignado;


                List<SRCaja.AsignacionCajaType> listaAsignadas = cajaCliente.GetListaCajaAsignadas(asigCaja).ToList();
                List<AsignacionCaja> listaSalida = new List<AsignacionCaja>();
                listaAsignadas.ForEach(a =>
                {
                    AsignacionCaja salida = AsignacionParse(a);
                    listaSalida.Add(salida);
                });

                return listaSalida;
            }
        }

        public List<MontoCierreCaja> ObtenerCajaMontosCierre(string AsignacionId)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<MontoCierreCaja> salida = new List<MontoCierreCaja>();
                List<SRCaja.MontoCierreType> lista = cajaCliente.GetCajaMontoCierre(AsignacionId);

                if (lista != null && lista.Count > 0)
                {
                    lista.ForEach(e =>
                    {
                        MontoCierreCaja nuevo = new MontoCierreCaja();
                        nuevo.Descripcion = e.Descripcion;
                        nuevo.Monto = e.Monto;
                        nuevo.Total = e.Total;
                        nuevo.Tipo = (int)e.Tipo;
                        salida.Add(nuevo);
                    });
                }

                return salida;
            }
        }

        private SRCaja.AsignacionCajaType ObtenerAsignacionBase(string AsignacionId)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRCaja.AsignacionCajaType resultado = cajaCliente.GetCajaAsignadaById(AsignacionId);
                return resultado;
            }
        }
        private AsignacionCaja AsignacionParse(SRCaja.AsignacionCajaType a)
        {
            AsignacionCaja salida = new AsignacionCaja();

            if (a != null)
            {
                #region Asignacion
                salida.IdAsignacion = a.IdAsignacion;
                salida.FechaCreacion = a.FechaAsignacion;
                salida.FechaAsignacion = a.FechaAsignacionDia;
                salida.IdUsuarioAsignado = a.IdUsuarioAsignado;
                salida.UsuarioAsignado = a.NombreUsuarioAsignado;
                salida.NumeroMaquinaTransbank = a.NumeroMaquinaTransbank;
                salida.Estado = EstadoAsignacionCaja.ObtenerTipoPorCodigo(a.EstadoAsignacion);

                salida.FechaInicio = a.FechaInicioCaja;
                salida.FechaCierreEfectivo = a.FechaCierreEfectivo;
                salida.FechaCierreCaja = a.FechaCierreCaja;
                salida.FechaRechazo = a.FechaRechazoCaja;
                salida.ObservacionRechazo = a.ObservacionRechazoCaja;

                salida.MontoFijo = a.MontoInicial;
                salida.MontoCierre = a.MontoCierre;
                salida.MontoRecaudado = a.MontoRecaudado;
                salida.MontoDiferencia = a.MontoDiferencia;
                salida.ObservacionDiferencia = a.ObservacionDiferencia;

                salida.FolioCaja = a.RangoFolio;
                salida.FolioRestante = a.RangoFolioRestante;
                #endregion
                #region Caja
                salida.CajaAsignada.IdCaja = a.IdCaja;
                salida.CajaAsignada.IdentificacionCaja = a.NombreCaja;
                salida.CajaAsignada.IdFuncionalidadCaja = a.IdTipo;
                salida.CajaAsignada.FuncionalidadCaja = a.TipoDescripcion;
                salida.CajaAsignada.IdFuncionCaja = a.IdFuncion;
                salida.CajaAsignada.FuncionCaja = a.FuncionDescripcion;
                salida.CajaAsignada.IdSeccionCaja = a.IdSeccion;
                salida.CajaAsignada.SeccionCaja = a.SeccionDescripcion;
                salida.CajaAsignada.IdPisoCaja = a.IdPiso;
                salida.CajaAsignada.PisoCaja = a.PisoDescripcion;
                salida.CajaAsignada.IdSupervisorCaja = a.IdSupervisorCaja;
                salida.CajaAsignada.SupervisorCaja = a.NombreSupervisor;

                salida.CajaAsignada.IpCaja = a.NumeroIP;
                salida.CajaAsignada.MacCaja = a.NumeroMAC;

                salida.CajaAsignada.HabilitaCaja = a.HabilitaCaja;

                salida.CajaAsignada.FechaCreacion = a.FechaCreacion;
                #endregion
            }

            return salida;
        }
        #endregion
    }
}
