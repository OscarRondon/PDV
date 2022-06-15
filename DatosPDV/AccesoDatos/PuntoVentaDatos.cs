using EntidadesPDV;
using EntidadesPDV.PuntoVenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DatosPDV.AccesoDatos
{
    public class PuntoVentaDatos : ComunDatos
    {
        SRVentas.VentasClient ventasCliente = new SRVentas.VentasClient();
        SRGarantia.GarantiaClient garantiaCliente = new SRGarantia.GarantiaClient();

        public TransaccionesEnt ObtenerDocumentoPorId(int documentoId)
        {
            using (new OperationContextScope(ventasCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                TransaccionesEnt boleta = new TransaccionesEnt();
                SRVentas.DocumentType documento = ventasCliente.GetDocumentoByDocEntry(documentoId);

                if (documento != null)
                {
                    boleta.IdTransaccion = documento.DocEntry.HasValue ? documento.DocEntry.Value : 0;
                    boleta.FechaTransaccion = documento.FechaDocumento;
                    boleta.IdTrack = documento.IdTrack;

                    boleta.Episodio = string.IsNullOrEmpty(documento.Episodio) ? "--" : documento.Episodio;
                    boleta.FichaClinica = string.IsNullOrEmpty(documento.FichaClinica) ? "--" : documento.FichaClinica;
                    boleta.NumeroBono = string.IsNullOrEmpty(documento.Bono) ? "--" : documento.Bono;
                    boleta.CodigoUnidad = string.IsNullOrEmpty(documento.CodigoUnidad) ? "--" : documento.CodigoUnidad;

                    boleta.RutPaciente = documento.PacienteRut;
                    boleta.PacienteNombre = documento.PacienteNombre;
                    boleta.DireccionPaciente = documento.DireccionPaciente;
                    boleta.ComunaPaciente = documento.ComunaPaciente;

                    boleta.RutResponsable = documento.ResponsableRut;
                    boleta.ResponsableNombre = documento.ResponsableNombre;
                    boleta.BoletaAsociada = documento.BoletaAsociada;

                    boleta.Categoria = documento.Categoria;
                    boleta.Convenio = documento.Convenio;

                    boleta.CodigoTarifaTransaccion = string.IsNullOrEmpty(documento.CodigoTarifa) ? "--" : documento.CodigoTarifa;
                    boleta.TarifaTransaccion = string.IsNullOrEmpty(documento.TarifaDetalle) ? "--" : documento.TarifaDetalle;
                    boleta.PrevisionResponsable = string.IsNullOrEmpty(documento.Prevision) ? "--" : documento.Prevision;

                    boleta.NombreMedico = string.IsNullOrEmpty(documento.NombreMedico) ? "--" : documento.NombreMedico;
                    boleta.EspecialidadMedico = string.IsNullOrEmpty(documento.EspecialidadMedico) ? "--" : documento.EspecialidadMedico;
                    boleta.ServicioAtencion = string.IsNullOrEmpty(documento.ServicioAtencion) ? "--" : documento.ServicioAtencion;
                    boleta.Impreso = documento.Impreso;
                    boleta.NroGarantia = string.IsNullOrEmpty(documento.NroGarantia) ? "" : documento.NroGarantia;

                    boleta.Tipo =
                            (documento.Origen == "PA" ? TipoTransaccion.Pagare :
                            (documento.Origen == "CO" ? TipoTransaccion.Convenio :
                            (documento.Origen == "OI" ? TipoTransaccion.Boleta :
                            (documento.Origen == "OR" ? TipoTransaccion.OrdenVenta :
                            (documento.Origen == "FA" ? TipoTransaccion.Factura : TipoTransaccion.Ninguno)))));

                    foreach (var item in documento.ListaArticulos)
                    {
                        boleta.DetalleAtencion.Add(
                            new TransaccionesEnt
                            {
                                CodigoPrestacion = item.CodigoArticulo ?? "--",
                                DescripcionPrestacion = item.DescripcionArticulo ?? "--",
                                CentroCosto = item.CentroCosto ?? "--",
                                Cantidad = Convert.ToInt32(item.Cantidad),
                                ValorUnitario = Convert.ToInt32(item.ValorUnitario),
                                SubTotal = Convert.ToInt32(item.SubTotal),
                                BonificacionFinanciador = Convert.ToDecimal(item.BonificacionFinanciador),
                                BonificacionComplemnentaria = Convert.ToInt32(item.BonificacionComplemnentaria),
                                CopagoBeneficiario = Convert.ToInt32(item.Precio)
                            });
                    }
                }
                return boleta;
            }
        }
        public List<TransaccionesEnt> ObtenerDocumentosPendientes(TransaccionFiltro filtro)
        {
            List<TransaccionesEnt> lista = new List<TransaccionesEnt>();

            SRVentas.DocumentFilterType entrada = new SRVentas.DocumentFilterType();
            entrada.AsignacionId = filtro.AsignacionId;
            entrada.Usuario = filtro.Usuario;
            entrada.Supervisor = filtro.Supervisor;
            entrada.Episodio = filtro.Episodio;
            entrada.FichaClinica = filtro.FichaClinica;
            entrada.NumeroFolio = filtro.NumeroFolio;
            entrada.NumeroComprobante = filtro.NumeroComprobante;
            entrada.FechaDesde = filtro.FechaDesde;
            entrada.FechaHasta = filtro.FechaHasta;
            entrada.Paciente = filtro.Paciente;
            entrada.Responsable = filtro.Responsable;
            entrada.SocioNegocio = filtro.SocioNegocio;
            entrada.TipoDocumento = filtro.TipoDocumento;

            switch (filtro.Estado)
            {
                case TransaccionEstado.Abierto:
                    entrada.Estado = SRVentas.DocumentoEstado.Abierto;
                    break;
                case TransaccionEstado.Cerrado:
                    entrada.Estado = SRVentas.DocumentoEstado.Cerrado;
                    break;
                default:
                    entrada.Estado = SRVentas.DocumentoEstado.Todos;
                    break;
            }
            switch (filtro.BuscarPor)
            {
                case TransaccionBuscar.Usuario:
                    entrada.BuscarPor = SRVentas.BuscarDocumento.Usuario;
                    break;
                default:
                    entrada.BuscarPor = SRVentas.BuscarDocumento.Todos;
                    break;
            }

            var salida = ventasCliente.GetListaDocumentosPendientes(entrada);

            foreach (var documento in salida)
            {
                TransaccionesEnt nuevo = new TransaccionesEnt();
                nuevo.DocEntry = documento.DocEntry;
                nuevo.DocNum = documento.DocNum;
                nuevo.IdTrack = documento.IdTrack;
                nuevo.NumeroFolio = documento.FolioAsignado;
                nuevo.NumeroComprobante = documento.NumeroComprobante;
                
                nuevo.Estado = documento.Estado;
                nuevo.Impreso = documento.Impreso;
                nuevo.Pagado = documento.Pagado;
                nuevo.FechaTransaccion = (documento.Origen == "CO") ? documento.FechaVencimientoDocumento.Value : documento.FechaDocumento;

                nuevo.Episodio = documento.Episodio ?? "--";
                nuevo.FichaClinica = documento.FichaClinica ?? "--";

                nuevo.RutPaciente = documento.PacienteRut ?? "";
                nuevo.PacienteNombre = documento.PacienteNombre ?? "";

                nuevo.RutResponsable = documento.ResponsableRut ?? "";
                nuevo.ResponsableNombre = documento.ResponsableNombre ?? "";

                nuevo.SocioNegocioCode = documento.SocioNegocioCodigo ?? "";
                nuevo.SocioNegocioName = documento.SocioNegocioNombre ?? "";

                nuevo.MontoPago = documento.MontoTotal;

                nuevo.AsientoNumero = documento.AsientoId;
                nuevo.AsientoLinea = documento.AsientoLinea;
                nuevo.AsientoLineaTotal = documento.AsientoLineaTotal;
                nuevo.TieneCuotaPendiente = documento.AsientoLinea.HasValue ? salida.Exists(p => p.AsientoId == documento.AsientoId && p.AsientoLinea < documento.AsientoLinea) : false;
                nuevo.BoletaAsociada = documento.BoletaAsociada;

                nuevo.Tipo =
                        (documento.Origen == "IM" ? TipoTransaccion.Imed :
                        (documento.Origen == "AN" ? TipoTransaccion.Anticipo :
                        (documento.Origen == "PA" ? TipoTransaccion.Pagare :
                        (documento.Origen == "CO" ? TipoTransaccion.Convenio :
                        (documento.Origen == "OI" ? TipoTransaccion.Boleta :
                        (documento.Origen == "OR" ? TipoTransaccion.OrdenVenta :
                        (documento.Origen == "FA" ? TipoTransaccion.Factura : TipoTransaccion.Ninguno)))))));
                lista.Add(nuevo);
            }

            return lista;
        }

        public RespuestaTransaccion FinalizarPago(Documento pago)
        {
            using (new OperationContextScope(ventasCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRVentas.DocumentType ventasType = CrearPago(pago);
                SRVentas.RespuestaType rsp = new SRVentas.RespuestaType();
                if (pago.TipoPago == TipoTransaccion.Anticipo)
                {
                    ventasType.TipoPago = SRVentas.TipoDocumento.Anticipo;
                    rsp = ventasCliente.PagarAnticipo(ventasType);
                }
                else if (pago.TipoPago == TipoTransaccion.Boleta)
                {
                    ventasType.TipoPago = SRVentas.TipoDocumento.Boleta;
                    rsp = ventasCliente.PagarBoleta(ventasType);
                }
                else if (pago.TipoPago == TipoTransaccion.Convenio || pago.TipoPago == TipoTransaccion.Pagare)
                {
                    ventasType.TipoPago = (pago.TipoPago == TipoTransaccion.Convenio) ?
                        SRVentas.TipoDocumento.Convenio :
                        SRVentas.TipoDocumento.Pagare;
                    rsp = ventasCliente.PagarAsiento(ventasType);
                }
                else if (pago.TipoPago == TipoTransaccion.Factura)
                {
                    ventasType.TipoPago = SRVentas.TipoDocumento.Factura;
                    rsp = ventasCliente.PagarFactura(ventasType);
                }
                else if (pago.TipoPago == TipoTransaccion.Imed)
                {
                    ventasType.TipoPago = SRVentas.TipoDocumento.Imed;
                    rsp = ventasCliente.PagarImed(ventasType);
                }
                else
                {
                    rsp.Error = true;
                    rsp.Mensaje = "Error al identificar el tipo de pago.";
                }

                RespuestaTransaccion respuesta = new RespuestaTransaccion();
                respuesta.EsError = rsp.Error;
                respuesta.Mensaje = rsp.Mensaje;

                return respuesta;
            }
        }

        private SRVentas.DocumentType CrearPago(Documento pago)
        {
            SRVentas.DocumentType ventasType = new SRVentas.DocumentType()
            {
                ListaArticulos = new List<SRVentas.DocumentTypeLines>(),
                ListaPago = new List<SRVentas.DocumentTypePago>()
            };
            pago.ListaPago.ForEach(p =>
            {
                SRVentas.DocumentTypePago nuevo = new SRVentas.DocumentTypePago();

                nuevo.CodigoBanco = p.CodigoBanco;

                //Cheque
                nuevo.ChequeEsValeVista = p.EsValeVista;
                nuevo.MontoCheque = p.MontoCheque;
                nuevo.NumeroCheque = p.NumeroCheque;
                nuevo.FechaVencimientoCheque = p.FechaVencimientoCheque;

                //Tarjeta
                nuevo.CodigoTarjeta = p.CodigoTarjeta;
                nuevo.NumeroTajeta = p.NumeroTajeta;
                nuevo.NumeroOperacionTarjeta = p.NumeroOperacionTarjeta;
                nuevo.NumeroCuotasTarjeta = p.NumeroCuotasTarjeta;
                nuevo.MontoTarjeta = p.MontoTarjeta;

                ventasType.ListaPago.Add(nuevo);
            });

            ventasType.FechaTransferencia = pago.FechaTransferencia;
            ventasType.MontoTransferencia = pago.MontoTransferencia;
            ventasType.NumeroTransferencia = pago.NumeroTransferencia;
            ventasType.BancoTransferencia = pago.BancoTransferencia;


            ventasType.MontoCheque = pago.MontoCheque;
            ventasType.MontoEfectivo = pago.MontoEfectivo;
            ventasType.MontoTarjeta = pago.MontoTarjeta;
            ventasType.MontoTotal = pago.MontoTotal;

            ventasType.DocEntry = pago.DocEntry;
            ventasType.IdTrack = pago.IdTrack;

            ventasType.ResponsableNombre = pago.ResponsableNombre;
            ventasType.ResponsableRut = pago.ResponsableRut;

            ventasType.AsientoId = pago.AsientoId;
            ventasType.AsientoLinea = pago.AsientoLinea;

            ventasType.FolioAsignado = (!string.IsNullOrEmpty(pago.FolioAsignado)) ? int.Parse(pago.FolioAsignado) : (int?) null;

            ventasType.CajaId = pago.IdCaja;
            ventasType.AsignacionCajaId = pago.IdAsignacion;
            ventasType.AsignacionCajaNombre = pago.CajaNombre;
            ventasType.UsuarioCajaId = pago.UsuarioCajaId;
            ventasType.UsuarioCajaNombre = pago.UsuarioCajaNombre;

            string strPago = Newtonsoft.Json.JsonConvert.SerializeObject(ventasType);

            return ventasType;
        }
        public RespuestaTransaccion AnularVenta(int IdTransaccion, string IdTrack, string episodio, string observacionAnulacion, string usuario, string asignacionId)
        {
            using (new OperationContextScope(ventasCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                RespuestaTransaccion result = new RespuestaTransaccion();

                SRVentas.PatientBillingEventERP boleta = new SRVentas.PatientBillingEventERP();
                boleta.BillingHeader = new SRVentas.BillingHeader();
                boleta.BillingHeader.BillNumber = IdTrack;
                boleta.BillingHeader.EpisodeNumber = episodio;

                SRVentas.RespuestaType rsp = ventasCliente.AnularVenta(boleta, IdTransaccion, observacionAnulacion, usuario, asignacionId);
                result.EsError = rsp.Error;
                result.Mensaje = rsp.Error && string.IsNullOrEmpty(rsp.Mensaje) ? "Ocurrió un problema al anular. Vuelva a intentarlo mas tarde." : rsp.Mensaje;

                return result;
            }
        }
        public RespuestaTransaccion CancelarPagoRecibido(int PagoId, string usuario, string observacion, string asignacionId)
        {
            using (new OperationContextScope(ventasCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                RespuestaTransaccion result = new RespuestaTransaccion();
                SRVentas.PagoCanceladoType cancelarPago = new SRVentas.PagoCanceladoType()
                {
                    AsignacionId = asignacionId,
                    ObservacionCancelacion = observacion,
                    PagoId = PagoId,
                    UsuarioId = usuario
                };

                SRVentas.RespuestaType rsp = ventasCliente.CancelarPagoRecibido(cancelarPago);
                result.EsError = rsp.Error;
                result.Mensaje = rsp.Error && string.IsNullOrEmpty(rsp.Mensaje) ? "Ocurrió un problema al cancelar el pago recibido. Vuelva a intentarlo mas tarde." : rsp.Mensaje;

                return result;
            }
        }

        public RespuestaTransaccion ImprimirDocumento(string idTrack, string idAsignacionCaja)
        {
            using (new OperationContextScope(ventasCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                RespuestaTransaccion result = new RespuestaTransaccion();
                try
                {
                    SRVentas.RespuestaType rsp = ventasCliente.ImprimirDocumento(idTrack, idAsignacionCaja);
                    result.EsError = rsp.Error;
                    result.Mensaje = rsp.Mensaje;
                }
                catch (Exception ex)
                {
                    result.EsError = true;
                    result.Mensaje = "Ocurrió un error al imprimir el documento. Vuelva a intentarlo mas tarde";
                }
                return result;
            }
        }

        public List<TransaccionesEnt> BoletasAbiertas(TransaccionesEnt boletaSearch)
        {
            using (new OperationContextScope(ventasCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<TransaccionesEnt> boletas = new List<TransaccionesEnt>();

                SRVentas.DocumentType documento = new SRVentas.DocumentType()
                {
                    DocEntry = boletaSearch.IdTransaccion,
                    IdTrack = boletaSearch.IdTrack
                };
                List<SRVentas.DocumentType> lista = ventasCliente.GetListaBoletasAbiertas(documento);
                if (lista != null)
                {
                    boletas = (from b in lista
                               select new TransaccionesEnt()
                               {
                                   IdTrack = b.IdTrack,
                                   IdTransaccion = b.DocEntry.HasValue ? b.DocEntry.Value : 0,
                                   Episodio = b.Episodio,
                                   FichaClinica = b.FichaClinica,
                                   RutPaciente = b.PacienteRut,
                                   PacienteNombre = b.PacienteNombre,
                                   RutResponsable = b.ResponsableRut,
                                   ResponsableNombre = b.ResponsableNombre,
                                   MontoPago = Convert.ToInt32(b.MontoTotal),
                                   ComunaPaciente = b.ComunaPaciente,
                                   DireccionPaciente = b.DireccionPaciente,
                                   ComunaResponsable = b.ComunaResponsable,
                                   DireccionResponsable = b.DireccionResponsable
                               }).ToList();
                }
                return boletas;
            }
        }

        public Documento ObtenerPagoRecibido(int PagoId)
        {
            using (new OperationContextScope(ventasCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                Documento pago = null;

                var salida = ventasCliente.GetPagoRecibido(PagoId);

                if (salida != null)
                {
                    pago = PagoParse(salida);
                }

                return pago;
            }
        }
        public List<Documento> ObtenerListaPagosRecibidos(int DocumentoId, int? DocumentoLinea, string TrackId)
        {
            using (new OperationContextScope(ventasCliente.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                List<Documento> lista = new List<Documento>();

                var salida = ventasCliente.GetListaPagoRecibido(DocumentoId, DocumentoLinea, TrackId);

                foreach (var documento in salida)
                {
                    lista.Add(PagoParse(documento));
                }

                return lista;
            }
        }
        private Documento PagoParse(SRVentas.DocumentType pago)
        {
            Documento ventasType = new Documento() { ListaPago = new List<DocumentoPago>() };
            ventasType.FechaDocumento = pago.FechaDocumento;
            ventasType.DocEntry = pago.DocEntry.Value;
            ventasType.DocNum = pago.DocNum.Value;

            ventasType.IdTrack = pago.IdTrack;

            ventasType.ResponsableNombre = pago.ResponsableNombre;
            ventasType.ResponsableRut = pago.ResponsableRut;

            ventasType.AsientoId = pago.AsientoId;
            ventasType.AsientoLinea = pago.AsientoLinea;

            ventasType.IdAsignacion = pago.AsignacionCajaId;


            pago.ListaPago.ForEach(p =>
            {
                DocumentoPago nuevo = new DocumentoPago();

                nuevo.CodigoBanco = p.CodigoBanco;
                nuevo.NombreBanco = p.NombreBanco;

                //Cheque
                nuevo.EsValeVista = p.ChequeEsValeVista;
                nuevo.MontoCheque = p.MontoCheque;
                nuevo.NumeroCheque = p.NumeroCheque;
                nuevo.FechaVencimientoCheque = p.FechaVencimientoCheque;

                //Tarjeta
                nuevo.CodigoTarjeta = p.CodigoTarjeta;
                nuevo.NombreTarjeta = p.NombreTarjeta;
                nuevo.NumeroTajeta = p.NumeroTajeta;
                nuevo.NumeroOperacionTarjeta = p.NumeroOperacionTarjeta;
                nuevo.NumeroCuotasTarjeta = p.NumeroCuotasTarjeta;
                nuevo.MontoTarjeta = p.MontoTarjeta;

                ventasType.ListaPago.Add(nuevo);
            });

            ventasType.FechaTransferencia = pago.FechaTransferencia;
            ventasType.MontoTransferencia = pago.MontoTransferencia;
            ventasType.NumeroTransferencia = pago.NumeroTransferencia;
            ventasType.BancoTransferencia = pago.BancoTransferencia;
            ventasType.BancoTransferenciaNombre = pago.BancoTransferenciaNombre;


            ventasType.MontoCheque = pago.MontoCheque;
            ventasType.MontoEfectivo = pago.MontoEfectivo;
            ventasType.MontoRedondeo = pago.MontoRedondeo;
            ventasType.MontoTarjeta = pago.MontoTarjeta;
            ventasType.MontoTotal = pago.MontoTotal;

            return ventasType;
        }
    }
}
