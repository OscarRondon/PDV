using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesPDV.Caja
{
    public enum EstadoAsignacion
    {
        NoIniciada,
        Rechazada,
        Iniciada,
        CerradaEfectivo,
        Cerrada
    }

    public static class EstadoAsignacionCaja
    {
        public static string ObtenerDescripcionPorTipo(EstadoAsignacion tipo)
        {
            string texto = "No Iniciada";
            switch (tipo)
            {
                case EstadoAsignacion.Rechazada:
                    texto = "Rechazada";
                    break;
                case EstadoAsignacion.Iniciada:
                    texto = "Iniciada";
                    break;
                case EstadoAsignacion.Cerrada:
                    texto = "Cerrada";
                    break;
                case EstadoAsignacion.CerradaEfectivo:
                    texto = "Cerrado Contado";
                    break;
                default:
                    break;
            }
            return texto;
        }
        public static string ObtenerCodigoPorTipo(EstadoAsignacion tipo)
        {
            string codigo = "NI";
            switch (tipo)
            {
                case EstadoAsignacion.Rechazada:
                    codigo = "RE";
                    break;
                case EstadoAsignacion.Iniciada:
                    codigo = "IC";
                    break;
                case EstadoAsignacion.CerradaEfectivo:
                    codigo = "CE";
                    break;
                case EstadoAsignacion.Cerrada:
                    codigo = "CC";
                    break;
                default:
                    break;
            }
            return codigo;
        }
        public static EstadoAsignacion ObtenerTipoPorCodigo(string codigo)
        {
            EstadoAsignacion salida = EstadoAsignacion.NoIniciada;
            if (codigo == "RE")
                salida = EstadoAsignacion.Rechazada;
            else if (codigo == "IC")
                salida = EstadoAsignacion.Iniciada;
            else if (codigo == "CC")
                salida = EstadoAsignacion.Cerrada;
            else if (codigo == "CE")
                salida = EstadoAsignacion.CerradaEfectivo;
            return salida;
        }
    }
}
