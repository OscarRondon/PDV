using EntidadesPDV;
using EntidadesPDV.Garantia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DatosPDV.AccesoDatos
{
    public class GarantiaDatos : ComunDatos
    {
        SRGarantia.GarantiaClient GarantiaClient = new SRGarantia.GarantiaClient();


        /// <summary>
        /// Método para ingresar anticipo o garantia
        /// </summary>
        /// <param name="AntGar"></param>
        /// <returns></returns>
        public RespuestaTransaccion IngresoGarantia(GarantiaEnt AntGar)
        {
            using (new OperationContextScope(GarantiaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                RespuestaTransaccion rspServicio = new RespuestaTransaccion();

                SRGarantia.GarantiaType AnGaType = new SRGarantia.GarantiaType()
                {
                    IdAsignacionCaja = AntGar.GarIdAsignacion,
                    Fecha = DateTime.Now,
                    FechaVencimiento = DateTime.Now.AddDays(5),
                    FichaClinica = AntGar.GarFichaClinica,
                    Episodio = AntGar.GarEpisodio,
                    GenerarCartaDescuento = "N",
                    Impreso = "N",
                    Monto = AntGar.GarMonto,
                    Indicador = AntGar.GarIndicador,
                    Entidad = AntGar.GarEntidad,
                    NroCheque = AntGar.GarNumeroCheque,
                    TipoDocumento = AntGar.IdTipoDocumento,
                    ResponsableRUT = AntGar.GarRutResponsable,
                    ResponsablePasaporte = AntGar.PasResponsable,
                    ResponsableNombre = AntGar.NombreResponsable,
                    ResponsableDirecc = AntGar.DireccionResponsable,
                    ResponsableComuna = AntGar.ComunaResponsable,
                    PacienteRUT = AntGar.GarRutPaciente,
                    PacientePasaporte = AntGar.PasPaciente,
                    PacienteNombre = AntGar.NombrePaciente,
                    PacienteDirecc = AntGar.DireccionPaciente,
                    PacienteComuna = AntGar.ComunaPaciente,
                    IdTrack = AntGar.IdTrack,
                    Usuario = this.UsuarioActual.Login
                };
                string ad = Newtonsoft.Json.JsonConvert.SerializeObject(AnGaType);
                SRGarantia.RespuestaType rsp = GarantiaClient.AdministrarGarantia(AnGaType);

                return rspServicio = new RespuestaTransaccion() { DocEntry = rsp.DocEntry, DocNum = rsp.DocNum, EsError = rsp.Error, Mensaje = rsp.Mensaje };
            }
        }
        
        public List<GarantiaEnt> GetListaGarantiasPorParametros(GarantiaFiltro filtro)
        {
            using (new OperationContextScope(GarantiaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRGarantia.GarantiaFilterType AnGaType = new SRGarantia.GarantiaFilterType()
                {
                    DocEntry = filtro.DocEntry,
                    DocNum = filtro.DocNum,
                    IdAsignacionCaja = filtro.IdAsignacion,
                    FechaInicio = filtro.FechaInicio,
                    FechaFinal = filtro.FechaFin,
                    ResponsableRUT = filtro.ResponsableRut,
                    PacienteRUT = filtro.PacienteRut,
                    Episodio = filtro.Episodio,
                    TipoDocumento = filtro.TipoDocumento,
                    Estado = (SRGarantia.EstadoGarantia)((int)filtro.Estado),
                };

                SRGarantia.GarantiaType[] ListaGarantias = GarantiaClient.GetListaGarantiaByFiltro(AnGaType);

                List<GarantiaEnt> list = new List<GarantiaEnt>();

                foreach (var AntiGaran in ListaGarantias)
                {
                    list.Add(GarantiaParse(AntiGaran));
                }
                //JMif (filtro.Estado == EstadoGarantia.Activo)
                //{
                //    list = list.Where(p => p.Vencido == true).ToList();
                //}

                return list;
            }
        }
        public GarantiaEnt GetGarantiasPorId(int idGarantia)
        {
            using (new OperationContextScope(GarantiaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRGarantia.GarantiaType garantia = GarantiaClient.GetGarantiaById(idGarantia);

                return GarantiaParse(garantia);
            }
        }

        private GarantiaEnt GarantiaParse(SRGarantia.GarantiaType garantia)
        {
            GarantiaEnt nuevo = new GarantiaEnt();
            nuevo.GarIdAsignacion = garantia.IdAsignacionCaja;
            nuevo.GarDocEntry = garantia.DocEntry;
            nuevo.GarDocNum = garantia.DocNum.HasValue ? garantia.DocNum.Value : 0;
            nuevo.GarFechaIngreso = garantia.Fecha.HasValue ? garantia.Fecha.Value : DateTime.MinValue;
            nuevo.GarFechaFin = garantia.FechaVencimiento.HasValue ? garantia.FechaVencimiento.Value : DateTime.MinValue;
            nuevo.DiasVencimiento = garantia.DiasVencimiento;
            //JMnuevo.Vencido = garantia.DiasVencimiento.HasValue ? (garantia.DiasVencimiento.Value < 0) : new bool?();
            nuevo.GarFichaClinica = garantia.FichaClinica;
            nuevo.GarEstado = garantia.Estado == SRGarantia.EstadoGarantia.Anulado ? EstadoGarantia.Anulado :
                                                    garantia.Estado == SRGarantia.EstadoGarantia.Activo ? EstadoGarantia.Activo :
                                                    garantia.Estado == SRGarantia.EstadoGarantia.Aplicado ? EstadoGarantia.Aplicado :
                                                    garantia.Estado == SRGarantia.EstadoGarantia.Devuelto ? EstadoGarantia.Devuelto :
                                                    garantia.Estado == SRGarantia.EstadoGarantia.Lleno ? EstadoGarantia.Lleno :
                                                    garantia.Estado == SRGarantia.EstadoGarantia.Protesto ? EstadoGarantia.Protesto : EstadoGarantia.NoAsignado;
            nuevo.GarEpisodio = garantia.Episodio;
            nuevo.GarIndicador = garantia.Indicador;
            nuevo.GarEntidad = garantia.Entidad;
            nuevo.GarEntidadDescripcion = garantia.EntidadNombre;
            nuevo.GarNumeroCheque = garantia.NroCheque;
            nuevo.GarMonto = garantia.Monto.HasValue ? garantia.Monto.Value : 0;
            nuevo.IdTipoDocumento = garantia.TipoDocumento;
            //Datos responsable
            nuevo.GarRutResponsable = garantia.ResponsableRUT;
            nuevo.PasResponsable = garantia.ResponsablePasaporte;
            nuevo.NombreResponsable = garantia.ResponsableNombre;
            nuevo.DireccionResponsable = garantia.ResponsableDirecc;
            nuevo.ComunaResponsable = garantia.ResponsableComuna;
            //Datos paciente
            nuevo.GarRutPaciente = garantia.PacienteRUT;
            nuevo.PasPaciente = garantia.PacientePasaporte;
            nuevo.NombrePaciente = garantia.PacienteNombre;
            nuevo.DireccionPaciente = garantia.PacienteDirecc;
            nuevo.ComunaPaciente = garantia.PacienteComuna;
            nuevo.NumPagRecib = garantia.U_NumPagRecib;
            nuevo.Comentario = garantia.Comentario;

            return nuevo;
        }

        public RespuestaTransaccion AgregarComentario(string numeroGarantia, string Comentario)
        {
            using (new OperationContextScope(GarantiaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRGarantia.GarantiaType garantia = GarantiaClient.GetGarantiaById(int.Parse(numeroGarantia));
                garantia.Comentario = Comentario;

                SRGarantia.RespuestaType resp = GarantiaClient.AdministrarGarantia(garantia);

                return new RespuestaTransaccion() { EsError = resp.Error, Mensaje = resp.Mensaje };
            }
        }

        public RespuestaTransaccion CambiarEstadoGarantia(int idGarantia, EstadoGarantia estado, string observacion)
        {
            using (new OperationContextScope(GarantiaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRGarantia.RespuestaType rsp = GarantiaClient.CambiarEstadoGarantia(idGarantia,
                (SRGarantia.EstadoGarantia)((int)estado),
                observacion);

                return new RespuestaTransaccion() { EsError = rsp.Error, Mensaje = rsp.Mensaje };
            }
        }

        public RespuestaTransaccion AplicarAnticipoBoleta(GarantiaEnt AntGar)
        {
            using (new OperationContextScope(GarantiaClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                RespuestaTransaccion rspServicio = new RespuestaTransaccion();

                SRGarantia.GarantiaType AnGaType = new SRGarantia.GarantiaType()
                {
                    DocEntry = AntGar.GarDocEntry,
                    U_NumPagRecib = AntGar.NumPagRecib,
                    NumDocBoleta = AntGar.NumDocBoleta,
                    IdTrack = AntGar.IdTrack,
                    IdAsignacionCaja = AntGar.IdAsignacion,
                    IdCaja = AntGar.IdCaja,
                    Monto = AntGar.GarMonto
                };
                string ad = Newtonsoft.Json.JsonConvert.SerializeObject(AnGaType);
                SRGarantia.RespuestaType rsp = GarantiaClient.AplicarAnticipoBoleta(AnGaType);

                return rspServicio = new RespuestaTransaccion() { DocEntry = rsp.DocEntry, DocNum = rsp.DocNum, EsError = rsp.Error, Mensaje = rsp.Mensaje };
            }
        }
    }
}
