using EntidadesPDV;
using EntidadesPDV.Nomina;
using EntidadesPDV.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace DatosPDV.AccesoDatos
{
    public class NominaDatos : ComunDatos
    {
        SRNomina.NominaClient nominaClient = new SRNomina.NominaClient();

        #region Administrar Nomina
        public RespuestaTransaccion AdministrarNominaDetalleMasivo(List<NominaDetalle> detalles)
        {
            using (new OperationContextScope(nominaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRNomina.NominaDetalleType[] listaParametros = detalles.Select(s => new SRNomina.NominaDetalleType()
                {
                    EstadoSupervisor = s.EstadoSupervisor,
                    ObservacionSupervisor = s.ObservacionSupervisor,

                    EstadoResponsable = s.EstadoResponsable,
                    ObservacionResponsable = s.ObservacionResponsable,

                    LineaDetalle = s.LineaDetalle,
                    DocEntry = s.DocEntry
                }).ToArray();

                SRNomina.RespuestaType rsp = new SRNomina.RespuestaType();
                if (listaParametros.Length > 50)
                {
                    //Se hace un ciclo por 50 item debido a que estaba dando problemas al enviar mas de 50 registros.

                    int saltar = 0;
                    int tomar = 50;
                    int limite = listaParametros.Length;

                    while (limite > 0)
                    {
                        rsp = nominaClient.AdministrarNominaDetalleMasivo(listaParametros.OrderBy(x => x.LineaDetalle).Skip(saltar).Take(tomar).ToArray());

                        saltar = saltar + tomar;
                        limite = limite - tomar;
                        tomar = limite < tomar ? limite : tomar;
                    }
                }
                else
                {
                    rsp = nominaClient.AdministrarNominaDetalleMasivo(listaParametros);
                }

                RespuestaTransaccion result = new RespuestaTransaccion();
                result.EsError = rsp.Error;
                result.Mensaje = rsp.Error ? rsp.Mensaje : "Detalle nómina actualizado correctamente";

                return result;
            }
        }
        public RespuestaTransaccion GenerarNominaSupervisor(NominaFiltro filtro)
        {
            using (new OperationContextScope(nominaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRNomina.NominaFilterType busqueda = new SRNomina.NominaFilterType()
                {
                    FechaDesde = filtro.FechaDesde,
                    FechaHasta = filtro.FechaHasta,
                    NumeroDesde = filtro.NumeroDesde,
                    NumeroHasta = filtro.NumeroHasta,
                    TipoAtencion = filtro.TipoAtencion,
                    TipoNomina = filtro.TipoNomina,
                    EstadoDetalleNomina = filtro.EstadoDetalleNomina,

                    SupervisorNombre = filtro.SupervisorNombre,
                    UsuarioId = filtro.UsuarioId,
                    UsuarioPerfil = filtro.UsuarioPerfil,
                    OcultarAprobados = filtro.OcultarAprobados,
                    CajaId = filtro.CajaId
                };

                SRNomina.RespuestaType rsp = nominaClient.GenerarNominaSupervisor(busqueda);

                RespuestaTransaccion result = new RespuestaTransaccion();
                result.EsError = rsp.Error;
                result.Mensaje = rsp.Error ? rsp.Mensaje : "Nómina creada con éxito. N° Nómina: " + rsp.Code;

                return result;
            }
        }
        #endregion

        #region Obtener Listados
        public Nomina ObtenerNominaPorNumero(int docEntryNomina)
        {
            using (new OperationContextScope(nominaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                Nomina nomina = new Nomina();
                try
                {
                    SRNomina.NominaType resultado = nominaClient.GetNominaByParam(docEntryNomina);
                    if (resultado != null)
                    {
                        nomina = NominaParse(resultado);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return nomina;
            }
        }
        public Nomina ObtenerNominaSupervisorPorNumero(int docEntryNomina)
        {
            using (new OperationContextScope(nominaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                Nomina nomina = new Nomina();
                try
                {
                    SRNomina.NominaType resultado = nominaClient.GetNominaSupervisorByParam(docEntryNomina);
                    if (resultado != null)
                    {
                        nomina = NominaParse(resultado);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return nomina;
            }
        }

        public List<Nomina> ObtenerListadoNomina(NominaFiltro filtro)
        {
            using (new OperationContextScope(nominaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<Nomina> listado = new List<Nomina>();
                try
                {
                    if (filtro == null)
                        filtro = new NominaFiltro();

                    SRNomina.NominaType[] resultado = nominaClient.GetListaNominas(new SRNomina.NominaFilterType()
                    {
                        FechaDesde = filtro.FechaDesde,
                        FechaHasta = filtro.FechaHasta,
                        NumeroDesde = filtro.NumeroDesde,
                        NumeroHasta = filtro.NumeroHasta,
                        TipoAtencion = filtro.TipoAtencion,
                        TipoNomina = filtro.TipoNomina,
                        EstadoDetalleNomina = filtro.EstadoDetalleNomina,
                        UsuarioId = filtro.UsuarioId,
                        UsuarioPerfil = filtro.UsuarioPerfil,
                        OcultarAprobados = filtro.OcultarAprobados,
                        CajaId = filtro.CajaId,
                        PisoId = filtro.PisoId,
                        SeccionId = filtro.SeccionId
                    });

                    foreach (var item in resultado)
                    {
                        listado.Add(NominaParse(item));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return listado;
            }
        }
        public List<Nomina> ObtenerListadoNominaSupervisor(NominaFiltro filtro)
        {
            using (new OperationContextScope(nominaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<Nomina> listado = new List<Nomina>();
                try
                {
                    if (filtro == null)
                        filtro = new NominaFiltro();

                    SRNomina.NominaType[] resultado = nominaClient.GetListaNominaSupervisor(new SRNomina.NominaFilterType()
                    {
                        FechaDesde = filtro.FechaDesde,
                        FechaHasta = filtro.FechaHasta,
                        NumeroDesde = filtro.NumeroDesde,
                        NumeroHasta = filtro.NumeroHasta,
                        TipoAtencion = filtro.TipoAtencion,
                        TipoNomina = filtro.TipoNomina,
                        EstadoDetalleNomina = filtro.EstadoDetalleNomina,
                        UsuarioId = filtro.UsuarioId,
                        UsuarioPerfil = filtro.UsuarioPerfil,
                        OcultarAprobados = filtro.OcultarAprobados,
                        CajaId = filtro.CajaId
                    });

                    foreach (var item in resultado)
                    {
                        listado.Add(NominaParse(item));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return listado;
            }
        }
        public List<NominaDetalle> ObtenerListadoDetalleAprobado(NominaFiltro filtro)
        {
            using (new OperationContextScope(nominaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<NominaDetalle> listado = new List<NominaDetalle>();
                try
                {
                    if (filtro == null)
                        filtro = new NominaFiltro();

                    SRNomina.NominaDetalleType[] resultado = nominaClient.GetListaNominaDetalleAprobados(new SRNomina.NominaFilterType()
                    {
                        FechaDesde = filtro.FechaDesde,
                        FechaHasta = filtro.FechaHasta,
                        NumeroDesde = filtro.NumeroDesde,
                        NumeroHasta = filtro.NumeroHasta,
                        TipoAtencion = filtro.TipoAtencion,
                        TipoNomina = filtro.TipoNomina,
                        EstadoDetalleNomina = filtro.EstadoDetalleNomina,
                        UsuarioId = filtro.UsuarioId,
                        UsuarioPerfil = filtro.UsuarioPerfil,
                        OcultarAprobados = filtro.OcultarAprobados,
                        CajaId = filtro.CajaId
                    });

                    foreach (var item in resultado)
                    {
                        listado.Add(NominaDetalleParse(item));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return listado;
            }
        }

        private Nomina NominaParse(SRNomina.NominaType n)
        {
            Nomina nuevo = new Nomina();
            nuevo.DocEntry = n.DocEntry;
            nuevo.NumeroNomina = n.NumeroNomina;
            nuevo.AsignacionId = n.AsignacionId;
            nuevo.CajaId = n.CajaId;
            nuevo.CajaNombre = n.CajaNombre;
            nuevo.Fecha = n.Fecha;
            nuevo.SupervisorId = n.SupervisorId;
            nuevo.SupervisorNombre = n.SupervisorNombre;
            nuevo.TipoAntencion = n.TipoAntencion;
            nuevo.TipoAntencionDescripcion = n.TipoAntencionDescripcion;
            nuevo.TipoNomina = n.TipoNomina;
            nuevo.TipoNominaDescripcion = n.TipoNominaDescripcion;
            nuevo.UsuarioId = n.UsuarioId;
            nuevo.UsuarioNombre = n.UsuarioNombre;

            nuevo.TotalDetalle = n.TotalDetalle;
            nuevo.TotalDetalleAprobado = n.TotalDetalleAprobado;
            nuevo.TotalDetallePendiente = n.TotalDetallePendiente;
            nuevo.TotalDetalleRechazado = n.TotalDetalleRechazado;
            nuevo.TotalDetalleAnulado = n.TotalDetalleAnulado;

            if (n.DetalleNomina != null && n.DetalleNomina.Length > 0)
            {
                foreach (var item in n.DetalleNomina)
                {
                    nuevo.DetalleNomina.Add(NominaDetalleParse(item));
                }
            }

            return nuevo;
        }
        private NominaDetalle NominaDetalleParse(SRNomina.NominaDetalleType d)
        {
            NominaDetalle nuevo = new NominaDetalle();
            nuevo.DocEntry = d.DocEntry;
            nuevo.NumeroNomina = d.NumeroNomina;
            nuevo.NumeroNominaSupervisor = d.NumeroNominaSupervisor;
            nuevo.LineaDetalle = d.LineaDetalle;

            nuevo.CajaNombre = d.CajaNombre;
            nuevo.AdmisionistaNombre = d.AdmisionistaNombre;

            nuevo.TipoDocumento = d.TipoDocumento;
            nuevo.TipoDocumentoDescripcion = d.TipoDocumentoDescripcion;
            nuevo.SubTipoDocumento = d.SubTipoDocumento;
            nuevo.NumeroDocumento = d.NumeroDocumento;
            nuevo.FechaDocumento = d.FechaDocumento;

            nuevo.EstadoResponsable = d.EstadoResponsable;
            nuevo.FechaAprobacionResponsable = d.FechaAprobacionResponsable;
            nuevo.ObservacionResponsable = d.ObservacionResponsable;

            nuevo.FechaAprobacionSupervisor = d.FechaAprobacionSupervisor;
            nuevo.EstadoSupervisor = d.EstadoSupervisor;
            nuevo.ObservacionSupervisor = d.ObservacionSupervisor;

            nuevo.FechaOrdenAtencion = d.FechaOrdenAtencion;
            nuevo.NumeroOrdenAtencion = d.NumeroOrdenAtencion;
            nuevo.MontoPago = d.MontoPago;

            nuevo.PacienteCategoria = d.PacienteCategoria;
            nuevo.PacienteNombre = d.PacienteNombre;
            nuevo.PacienteRut = d.PacienteRut;
            nuevo.TitularNombre = d.TitularNombre;
            nuevo.TitularRut = d.TitularRut;

            nuevo.FichaClinica = d.FichaClinica;
            nuevo.Episodio = d.Episodio;

            nuevo.UnidadCobroEpisodio = d.UnidadCobroEpisodio;
            nuevo.UnidadCobroPaciente = d.UnidadCobroPaciente;

            nuevo.TipoDocumentoSAP = d.TipoDocumentoSAP;
            nuevo.NumeroSAP = d.NumeroSAP;

            return nuevo;
        }
        #endregion
    }
}
