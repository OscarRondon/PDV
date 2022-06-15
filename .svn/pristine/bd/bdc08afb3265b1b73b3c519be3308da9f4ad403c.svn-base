using System;
using System.Collections.Generic;

namespace EntidadesPDV.Caja
{
    public class AsignacionCaja
    {
        private string textoNulo;
        public string IdAsignacion { get; set; }
        public string IdUsuarioAsignado { get; set; }
        public string UsuarioAsignado { get; set; }
        public string NumeroMaquinaTransbank { get; set; }
        public EstadoAsignacion Estado { get; set; }
        public string EstadoDescripcion { get { return EstadoAsignacionCaja.ObtenerDescripcionPorTipo(Estado); } }

        public double? MontoFijo { get; set; }
        public double? MontoCierre { get; set; }
        public double? MontoRecaudado { get; set; }
        public double? MontoDiferencia { get; set; }
        public string ObservacionDiferencia { get; set; }

        public DateTime? FechaCreacion { get; set; }
        public string FechaCreacionTexto { get { return FechaCreacion.HasValue ? FechaCreacion.Value.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }
        public string FechaCreacionFiltro { get { return FechaCreacion.HasValue ? FechaCreacion.Value.ToString("yyyyMMddHHmm") : textoNulo; } }

        public DateTime? FechaAsignacion { get; set; }
        public string FechaAsignacionTexto { get { return FechaAsignacion.HasValue ? FechaAsignacion.Value.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }
        public string FechaAsignacionFiltro { get { return FechaAsignacion.HasValue ? FechaAsignacion.Value.ToString("yyyyMMddHHmm") : textoNulo; } }

        public DateTime? FechaInicio { get; set; }
        public string FechaInicioTexto { get { return FechaInicio.HasValue ? FechaInicio.Value.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }
        public string FechaInicioFiltro { get { return FechaInicio.HasValue ? FechaInicio.Value.ToString("yyyyMMddHHmm") : textoNulo; } }
        public DateTime? FechaCierreEfectivo { get; set; }
        public string FechaCierreEfectivoTexto { get { return FechaCierreEfectivo.HasValue ? FechaCierreEfectivo.Value.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }
        public string FechaCierreEfectivoFiltro { get { return FechaCierreEfectivo.HasValue ? FechaCierreEfectivo.Value.ToString("yyyyMMddHHmm") : textoNulo; } }
        public DateTime? FechaCierreCaja { get; set; }
        public string FechaCierreCajaTexto { get { return FechaCierreCaja.HasValue ? FechaCierreCaja.Value.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }
        public string FechaCierreCajaFiltro { get { return FechaCierreCaja.HasValue ? FechaCierreCaja.Value.ToString("yyyyMMddHHmm") : textoNulo; } }
        public DateTime? FechaRechazo { get; set; }
        public string FechaRechazoTexto { get { return FechaRechazo.HasValue ? FechaRechazo.Value.ToString("dd/MM/yyyy HH:mm") : textoNulo; } }
        public string FechaRechazoFiltro { get { return FechaRechazo.HasValue ? FechaRechazo.Value.ToString("yyyyMMddHHmm") : textoNulo; } }
        public string ObservacionRechazo { get; set; }

        public CajasEnt CajaAsignada { get; set; }

        public string IdFolioAsignado { get; set; }
        public Int64 NoFolio { get; set; }

        public string FolioCaja { get; set; }
        public string FolioRestante { get; set; }

        public string IdUsuarioCierre { get; set; }
        public bool RespuestaUsuarioCierre { get; set; }

        public AsignacionCaja()
        {
            CajaAsignada = new CajasEnt();
        }
    }

}
