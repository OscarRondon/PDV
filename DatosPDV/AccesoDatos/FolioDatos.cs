using EntidadesPDV;
using EntidadesPDV.Folio;
using EntidadesPDV.Caja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DatosPDV.AccesoDatos
{
    public class FolioDatos : ComunDatos
    {
        SRFolio.FolioClient folioClient = new SRFolio.FolioClient();

        #region Actualizar Folio
        public RespuestaTransaccion GuardarLimiteFolio(Folio folio)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRFolio.FolioType folioType = new SRFolio.FolioType()
                {
                    IdFolio = folio.IdFolio,
                    FolioInicial = folio.FolioInicial,
                    FolioFinal = folio.FolioFinal,
                    IdUsuarioCreacion = folio.IdUsuarioCreacion,
                    NombreUsuarioCreacion = folio.NombreUsuarioCreacion,
                    EsAnulado = folio.EsAnulado,
                    MotivoAnulacion = folio.MotivoAnulacion
                };
                SRFolio.RespuestaType rsp = folioClient.AdministrarLimiteFolio(folioType);


                RespuestaTransaccion result = new RespuestaTransaccion();
                result.EsError = rsp.Error;
                result.Mensaje = rsp.Error ? rsp.Mensaje : "Folio guardado correctamente.";

                return result;
            }
        }
        public RespuestaTransaccion GuardarFolio(Folio folio)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRFolio.FolioType folioType = new SRFolio.FolioType()
                {
                    IdFolio = folio.IdFolio,
                    FolioInicial = folio.FolioInicial,
                    FolioFinal = folio.FolioFinal,
                    TipoDocumento = folio.TipoDocumento,
                    IdUsuarioCreacion = folio.IdUsuarioCreacion,
                    NombreUsuarioCreacion = folio.NombreUsuarioCreacion,
                    IdUsuarioSupervisor = folio.IdUsuarioSupervisor,
                    NombreUsuarioSupervisor = folio.NombreUsuarioSupervisor
                };
                SRFolio.RespuestaType rsp = folioClient.AdministrarFolio(folioType);


                RespuestaTransaccion result = new RespuestaTransaccion();
                result.EsError = rsp.Error;
                result.Mensaje = rsp.Error ? rsp.Mensaje : "Folio guardado correctamente.";

                return result;
            }
        }
        public RespuestaTransaccion GuardarFolioSegmentoMasivo(List<Folio> listaFolio)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRFolio.FolioType[] folioType = listaFolio.Select(f => new SRFolio.FolioType()
                {
                    IdFolio = f.IdFolio,
                    CantidadSegmento = f.CantidadSegmento
                }).ToArray();

                SRFolio.RespuestaType rsp = folioClient.AdministrarFolioSegmentoMasivo(folioType);

                RespuestaTransaccion result = new RespuestaTransaccion();
                result.EsError = rsp.Error;
                result.Mensaje = rsp.Error ? rsp.Mensaje : "Segmentos guardados correctamente";

                return result;
            }
        }
        public RespuestaTransaccion AsignarCajaSegmentoMasivo(List<FolioSegmento> listado)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRFolio.FolioSegmentoType[] listaParametros = listado.Select(s => new SRFolio.FolioSegmentoType()
                {
                    IdFolio = s.IdFolio,
                    LineaSegmento = s.LineaSegmento,
                    IdCaja = s.IdCaja
                }).ToArray();

                SRFolio.RespuestaType rsp = folioClient.AsignarCajaSegmentoMasivo(listaParametros);

                RespuestaTransaccion result = new RespuestaTransaccion();
                result.EsError = rsp.Error;
                result.Mensaje = rsp.Error ? rsp.Mensaje : "Cajas asignadas correctamente";

                return result;
            }
        }
        public RespuestaTransaccion AnularFolio(FolioAnulado folio)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRFolio.FolioAnuladoType folioType = new SRFolio.FolioAnuladoType()
                {
                    Anulado = false,
                    FolioFinal = folio.FolioFinal,
                    FolioInicial = folio.FolioInicial,
                    IdCaja = folio.IdCaja,
                    IdFolio = folio.IdFolio,
                    Motivo = folio.Motivo.ToUpper(),
                    IdUsuarioAnulacion = folio.IdUsuarioAnulacion,
                    NombreUsuarioAnulacion = folio.NombreUsuarioAnulacion
                };
                SRFolio.RespuestaType rsp = folioClient.AnularFolio(folioType);

                RespuestaTransaccion result = new RespuestaTransaccion();
                result.EsError = rsp.Error;
                result.Mensaje = rsp.Error ? rsp.Mensaje : "Anulación guardada correctamente";

                return result;
            }
        }
        public RespuestaTransaccion AutorizarAnulacionFolio(FolioAnulado folio)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRFolio.RespuestaType rsp = folioClient.AutorizarAnulacionFolio(new SRFolio.FolioAnuladoType()
                {
                    IdFolio = folio.IdFolio,
                    LineaAnulacion = folio.LineaAnulacion,
                    IdUsuarioAprobacion = folio.IdUsuarioAprobacion,
                    NombreUsuarioAprobacion = folio.NombreUsuarioAprobacion
                });

                RespuestaTransaccion result = new RespuestaTransaccion();
                result.EsError = rsp.Error;
                result.Mensaje = rsp.Error ? rsp.Mensaje : "Aprobación de anulación folio guardada correctamente";

                return result;
            }
        }
        #endregion

        #region Obtener Listados
        public int ObtenerUltimoLimiteFolio()
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                int folio = 1;

                int result = folioClient.GetUltimoLimiteFolio();
                folio = result + 1;

                return folio;
            }
        }
        public int ObtenerUltimoFolio()
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                int folio = 1;

                int result = folioClient.GetUltimoFolio();
                folio = result + 1;

                return folio;
            }
        }

        public List<Folio> ObtenerListadoLimiteFolio()
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<Folio> listado = new List<Folio>();
                try
                {
                    SRFolio.FolioType[] resultado = folioClient.GetListaLimiteFolio();
                    foreach (var item in resultado)
                    {
                        listado.Add(FolioParse(item));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return listado;
            }
        }
        public List<Folio> ObtenerListadoFolio(FolioFiltro filtro)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<Folio> listado = new List<Folio>();
                try
                {
                    if (filtro == null)
                        filtro = new FolioFiltro();

                    SRFolio.FolioType[] resultado = folioClient.GetListaFolios(new SRFolio.FolioFilterType()
                    {
                        FechaInicio = filtro.FechaInicio,
                        FechaFinal = filtro.FechaFin,
                        FolioInicial = filtro.FolioInicio,
                        FolioFinal = filtro.FolioFin,
                        IdUsuarioSupervisor = filtro.IdUsuarioSupervisor,
                        ObtenerSegmentos = filtro.ObtenerSegmentos,
                        IdFolio = filtro.IdFolio
                    });
                    foreach (var item in resultado)
                    {
                        listado.Add(FolioParse(item));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return listado;
            }
        }
        public List<FolioSegmento> ObtenerListadoSegmentos(string FolioId)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<FolioSegmento> listado = new List<FolioSegmento>();
                try
                {
                    SRFolio.FolioSegmentoType[] resultado = folioClient.GetListaSegmentosByFolio(FolioId);

                    foreach (var item in resultado)
                    {
                        listado.Add(FolioSegmentoParse(item));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return listado;
            }
        }

        public List<FolioSegmento> ObtenerListadoFolioCajaPorUsuario(string usuario)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<FolioSegmento> listado = new List<FolioSegmento>();
                try
                {
                    SRFolio.FolioSegmentoType[] resultado = folioClient.GetListaFoliosCajaByUsuario(usuario);
                    foreach (var item in resultado)
                    {
                        listado.Add(FolioSegmentoParse(item));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return listado;
            }
        }
        public List<FolioAnulado> ObtenerListadoFolioAnulados(FolioFiltro filtro)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<FolioAnulado> listado = new List<FolioAnulado>();
                try
                {
                    SRFolio.FolioAnuladoType[] resultado = folioClient.GetListaFoliosAnulados(new SRFolio.FolioFilterType()
                    {
                        FechaInicio = filtro.FechaInicio,
                        FechaFinal = filtro.FechaFin,
                    });
                    foreach (var item in resultado)
                    {
                        listado.Add(FolioAnuladoParse(item));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return listado;
            }
        }
        public List<FolioAnulado> ObtenerListadoFolioAnuladosCajaPorSupervisor(FolioFiltro filtro)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<FolioAnulado> listado = new List<FolioAnulado>();
                try
                {
                    SRFolio.FolioAnuladoType[] resultado = folioClient.GetListaFoliosAnuladosCajaPorSupervisor(new SRFolio.FolioFilterType()
                    {
                        FechaInicio = filtro.FechaInicio,
                        FechaFinal = filtro.FechaFin,
                        FolioInicial = filtro.FolioInicio,
                        FolioFinal = filtro.FolioFin,
                        IdUsuarioSupervisor = filtro.IdUsuarioSupervisor
                    });
                    foreach (var item in resultado)
                    {
                        listado.Add(FolioAnuladoParse(item));
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return listado;
            }
        }

        public List<FolioGlobal> ObtenerListadoFolioGlobal(int FolioIni, int FolioFin)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<FolioGlobal> listado = new List<FolioGlobal>();

                SRFolio.FolioGlobalType[] resultado = folioClient.GetListaGlobalFolio(FolioIni, FolioFin);
                foreach (var item in resultado)
                {
                    FolioGlobal nuevo = new FolioGlobal();
                    nuevo.CajaNombre = item.CajaNombre;
                    nuevo.Episodio = item.Episodio;
                    nuevo.Estado = item.Estado;
                    nuevo.Fecha = item.Fecha;
                    nuevo.Ficha = item.Ficha;
                    nuevo.Monto = item.Monto;
                    nuevo.NumeroFolio = item.Folio;
                    nuevo.Observacion = item.Observacion;
                    nuevo.Titular = item.Titular;
                    nuevo.Usuario = item.Usuario;

                    listado.Add(nuevo);
                }

                return listado;
            }
        }

        public FolioSegmento ObtenerSiguienteFolioPorCaja(string IdCaja)
        {
            using (new OperationContextScope(folioClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                FolioSegmento folioCaja = new FolioSegmento();
                try
                {
                    SRFolio.FolioSegmentoType folio = folioClient.GetSiguienteFolioByCaja(IdCaja);
                    folioCaja = new FolioSegmento()
                    {
                        FechaSegmento = folio.FechaSegmento,
                        FolioActual = folio.FolioActual,
                        FolioFinal = folio.FolioFinal,
                        FolioInicial = folio.FolioInicial,
                        IdCaja = folio.IdCaja,
                        IdFolio = folio.IdFolio,
                        LineaSegmento = folio.LineaSegmento,
                    };
                }
                catch (Exception ex)
                {
                    folioCaja = null;
                }
                return folioCaja;
            }
        }

        private Folio FolioParse(SRFolio.FolioType f)
        {
            Folio nuevo = new Folio();
            nuevo.CantidadSegmento = f.CantidadSegmento;
            nuevo.CantidadSegmentoPendiente = f.CantidadSegmentoPendientes;
            nuevo.FechaRegistro = f.FechaFolio;
            nuevo.FolioFinal = f.FolioFinal;
            nuevo.FolioInicial = f.FolioInicial;
            nuevo.IdFolio = f.IdFolio;
            nuevo.TipoDocumento = f.TipoDocumento;
            nuevo.IdUsuarioCreacion = f.IdUsuarioCreacion;
            nuevo.NombreUsuarioCreacion = f.NombreUsuarioCreacion;
            nuevo.IdUsuarioSupervisor = f.IdUsuarioSupervisor;
            nuevo.NombreUsuarioSupervisor = f.NombreUsuarioSupervisor;
            nuevo.EsUltimoFolio = f.EsUltimoRegistro;
            nuevo.EsAnulado = f.EsAnulado;
            nuevo.MotivoAnulacion = f.MotivoAnulacion;
            nuevo.TotalFolios = f.TotalFolios;
            nuevo.TotalFoliosAnulados = f.TotalFoliosAnulados;
            nuevo.TotalFoliosDisponibles = f.TotalFoliosDisponibles;
            nuevo.TotalFoliosUsados = f.TotalFoliosUsados;

            if (f.SegmentosFolio != null && f.SegmentosFolio.Length > 0)
            {
                foreach (var item in f.SegmentosFolio)
                {
                    nuevo.ListaSegmentos.Add(FolioSegmentoParse(item));
                }
            }

            return nuevo;
        }
        private FolioAnulado FolioAnuladoParse(SRFolio.FolioAnuladoType f)
        {
            FolioAnulado nuevo = new FolioAnulado();
            nuevo.Anulado = f.Anulado;
            nuevo.FechaAnulacion = f.FechaAnulacion;
            nuevo.FolioFinal = f.FolioFinal;
            nuevo.FolioInicial = f.FolioInicial;
            nuevo.IdCaja = f.IdCaja;
            nuevo.NombreCaja = f.NombreCaja;
            nuevo.IdFolio = f.IdFolio;
            nuevo.LineaAnulacion = f.LineaAnulacion;
            nuevo.Motivo = f.Motivo;
            nuevo.IdUsuarioAnulacion = f.IdUsuarioAnulacion;
            nuevo.NombreUsuarioAnulacion = f.NombreUsuarioAnulacion;
            nuevo.IdUsuarioAprobacion = f.IdUsuarioAprobacion;
            nuevo.NombreUsuarioAprobacion = f.NombreUsuarioAprobacion;

            return nuevo;
        }
        private FolioSegmento FolioSegmentoParse(SRFolio.FolioSegmentoType f)
        {
            FolioSegmento nuevo = new FolioSegmento();
            nuevo.FechaSegmento = f.FechaSegmento;
            nuevo.FolioActual = f.FolioActual;
            nuevo.FolioFinal = f.FolioFinal;
            nuevo.FolioInicial = f.FolioInicial;
            nuevo.IdCaja = f.IdCaja;
            nuevo.NombreCaja = f.NombreCaja;
            nuevo.IdFolio = f.IdFolio;
            nuevo.LineaSegmento = f.LineaSegmento;

            nuevo.TotalFolios = f.TotalFolios;
            nuevo.TotalFoliosAnulados = f.TotalFoliosAnulados;
            nuevo.TotalFoliosDisponibles = f.TotalFoliosDisponibles;
            nuevo.TotalFoliosUsados = f.TotalFoliosUsados;

            return nuevo;
        }
        #endregion
    }
}