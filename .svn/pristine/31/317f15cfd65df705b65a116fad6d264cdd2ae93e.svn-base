using EntidadesPDV;
using EntidadesPDV.Caja;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DatosPDV.AccesoDatos
{
    public class CajaDatos : ComunDatos
    {
        SRCaja.CajaClient cajaCliente = new SRCaja.CajaClient();

        #region Actualizar Caja
        public bool CajaNueva(CajasEnt caja)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRCaja.CajaType nuevaCaja = new SRCaja.CajaType();
                nuevaCaja.NombreCaja = caja.IdentificacionCaja;
                nuevaCaja.IdTipo = caja.IdFuncionalidadCaja;
                nuevaCaja.TipoDescripcion = caja.FuncionalidadCaja;
                nuevaCaja.IdFuncion = caja.IdFuncionCaja;
                nuevaCaja.FuncionDescripcion = caja.FuncionCaja;
                nuevaCaja.IdSeccion = caja.IdSeccionCaja;
                nuevaCaja.SeccionDescripcion = caja.SeccionCaja;
                nuevaCaja.IdPiso = caja.IdPisoCaja;
                nuevaCaja.PisoDescripcion = caja.PisoCaja;
                nuevaCaja.HabilitaCaja = caja.HabilitaCaja;
                nuevaCaja.NumeroIP = caja.IpCaja;
                nuevaCaja.NumeroMAC = caja.MacCaja;
                nuevaCaja.IdSupervisorCaja = caja.IdSupervisorCaja;
                nuevaCaja.NombreSupervisor = caja.SupervisorCaja;

                SRCaja.RespuestaType rsp = cajaCliente.AdministrarCaja(nuevaCaja);
                return rsp.Error;
            }
        }
        public RespuestaTransaccion CajaModificar(CajasEnt caja)
        {
            RespuestaTransaccion resultado = new RespuestaTransaccion();
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRCaja.CajaType cajaActualizar = ObtenerCajaBase(caja);
                cajaActualizar.NombreCaja = caja.IdentificacionCaja;
                cajaActualizar.IdTipo = caja.IdFuncionalidadCaja;
                cajaActualizar.TipoDescripcion = caja.FuncionalidadCaja;
                cajaActualizar.IdFuncion = caja.IdFuncionCaja;
                cajaActualizar.FuncionDescripcion = caja.FuncionCaja;
                cajaActualizar.IdSeccion = caja.IdSeccionCaja;
                cajaActualizar.SeccionDescripcion = caja.SeccionCaja;
                cajaActualizar.IdPiso = caja.IdPisoCaja;
                cajaActualizar.PisoDescripcion = caja.PisoCaja;
                cajaActualizar.HabilitaCaja = caja.HabilitaCaja;
                cajaActualizar.NumeroIP = caja.IpCaja;
                cajaActualizar.NumeroMAC = caja.MacCaja;
                cajaActualizar.IdSupervisorCaja = string.IsNullOrEmpty(caja.IdSupervisorCaja) ? string.Empty : caja.IdSupervisorCaja;
                cajaActualizar.NombreSupervisor = string.IsNullOrEmpty(caja.SupervisorCaja) ? string.Empty : caja.SupervisorCaja;

                SRCaja.RespuestaType rsp = cajaCliente.AdministrarCaja(cajaActualizar);
                resultado.EsError = rsp.Error;
                resultado.Mensaje = rsp.Mensaje;

                return resultado;
            }
        }
        public bool CajaModificarSupervisor(CajasEnt caja)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRCaja.CajaType editar = ObtenerCajaBase(caja);
                editar.IdSupervisorCaja = caja.IdSupervisorCaja;
                editar.NombreSupervisor = caja.SupervisorCaja;
                SRCaja.RespuestaType rsp = cajaCliente.AdministrarCaja(editar);
                return rsp.Error;
            }
        }
        public RespuestaTransaccion CajaModificarSupervisorMasivo(List<string> lstCajasId, string idSupervisorCaja, string supervisorCaja)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<SRCaja.CajaType> lista = new List<SRCaja.CajaType>();
                foreach (var item in lstCajasId)
                {
                    SRCaja.CajaType editar = new SRCaja.CajaType();
                    editar.IdCaja = item;
                    editar.IdSupervisorCaja = idSupervisorCaja;
                    editar.NombreSupervisor = supervisorCaja;
                    lista.Add(editar);
                }

                SRCaja.RespuestaType rsp = cajaCliente.AdministrarCajaMasivoSupervisor(lista);
                RespuestaTransaccion salida = new RespuestaTransaccion();
                salida.EsError = rsp.Error;
                salida.Mensaje = rsp.Mensaje;
                return salida;
            }
        }
        #endregion

        #region Obtener Caja
        public CajasEnt ObtenerCaja(CajasEnt caja)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRCaja.CajaType cajaResultado = ObtenerCajaBase(caja);
                CajasEnt cajaObtener = CajaParse(cajaResultado);

                return cajaObtener;
            }
        }
        public List<CajasEnt> ObtenerCajas(CajasEnt entrada)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<CajasEnt> lista = new List<CajasEnt>();

                cajaCliente.GetListaCajas(new SRCaja.CajaType() { IdSupervisorCaja = entrada.IdSupervisorCaja }).ToList().ForEach(c =>
                {
                    CajasEnt caja = CajaParse(c);
                    lista.Add(caja);
                });

                return lista;
            }
        }

        private SRCaja.CajaType ObtenerCajaBase(CajasEnt caja)
        {
            using (new OperationContextScope(cajaCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRCaja.CajaType cajaResultado = cajaCliente.GetCajaByParams(new SRCaja.CajaType() { IdCaja = caja.IdCaja });
                return cajaResultado;
            }
        }
        private CajasEnt CajaParse(SRCaja.CajaType c)
        {
            CajasEnt salida = new CajasEnt();
            salida.IdCaja = c.IdCaja;
            salida.FechaCreacion = c.FechaCreacion;
            salida.IdSupervisorCaja = c.IdSupervisorCaja;
            salida.SupervisorCaja = c.NombreSupervisor;
            salida.IdentificacionCaja = c.NombreCaja;
            salida.IdFuncionalidadCaja = c.IdTipo;
            salida.FuncionalidadCaja = c.TipoDescripcion;
            salida.IdFuncionCaja = c.IdFuncion;
            salida.FuncionCaja = c.FuncionDescripcion;
            salida.IdSeccionCaja = c.IdSeccion;
            salida.SeccionCaja = c.SeccionDescripcion;
            salida.IdPisoCaja = c.IdPiso;
            salida.PisoCaja = c.PisoDescripcion;
            salida.HabilitaCaja = c.HabilitaCaja;
            salida.IpCaja = c.NumeroIP;
            salida.MacCaja = c.NumeroMAC;
            return salida;
        }
        #endregion
    }
}
