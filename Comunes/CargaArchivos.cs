using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comunes
{
    public class CargaArchivos
    {
        public static string Zero(string dato, string relleno, string tipoDato, string direccion)
        {
            //Resta del largo del campo (entregado) menos el length del dato
            int largo = LargoCampos(tipoDato) - dato.Length;

            if (direccion.ToUpper() == "I")
            {
                for (int i = 1; i <= largo; i++)
                {
                    dato = relleno + dato;
                }
            }
            else
            {
                for (int i = 1; i <= largo; i++)
                {
                    dato = dato + relleno;
                }
            }
            return dato;
        }

        private static int LargoCampos(string campo)
        {
            int largo = 0;

            switch (campo)
            {
                case "DP1117":
                    largo = 6;
                    break;
                case "NCuenta":
                    largo = 9;
                    break;
                case "Rut":
                    largo = 14;
                    break;
                case "C":
                    largo = 1;
                    break;
                case "Monto":
                    largo = 13;
                    break;
                case "Cien":
                    largo = 14;
                    break;
                case "FechaEnvio":
                    largo = 21;
                    break;
                case "No":
                    largo = 4;
                    break;
            };
            return largo;
        }

        public static string ZeroFNCabecera(string dato, string relleno, string tipoDato, string direccion)
        {
            //Resta del largo del campo (entregado) menos el length del dato
            int largo = LCFNCabecera(tipoDato) - dato.Length;
            dato = dato.PadLeft(largo, '0');
            return dato;
        }

        private static int LCFNCabecera(string campoRegistro)
        {
            int campo = 0;
            switch (campoRegistro.ToUpper())
            {

                case "NUMERO":
                    campo = 12;
                    break;
                case "FECHA":
                    campo = 10;
                    break;
                case "DOCUMENTOSCANTIDAD":
                    campo = 5;
                    break;
                case "DOCUMENTOSVALOR":
                    campo = 10;
                    break;
                case "PRESTADORRUT":
                    campo = 12;
                    break;
            }
            return campo;
        }

        public static string ZeroFNDetalle(string dato, string relleno, string tipoDato, string direccion)
        {
            //Resta del largo del campo (entregado) menos el length del dato
            int largo = LCFormato1N(tipoDato) - dato.Length;
            dato = dato.PadLeft(largo, '0');
            return dato;
        }
        
        private static int LCFormato1N(string campoRegistro)
        {
            int campo = 0;
            switch (campoRegistro.ToUpper())
            {
                case "CODIGO":
                    campo = 2;
                    break;
                case "NUMERO":
                    campo = 10;
                    break;
                case "FECHA":
                    campo = 10;
                    break;
                case "TIPODOCUMENTO":
                    campo = 2;
                    break;
                case "RUNPACIENTE":
                    campo = 12;
                    break;
                case "RUNTITULAR":
                    campo = 12;
                    break;
                case "TRANSACCIONID":
                    campo = 15;
                    break;
                case "TRANSACCIONFECHA":
                    campo = 16;
                    break;
                case "ATENCIONID":
                    campo = 15;
                    break;
                case "PRESTACION":
                    campo = 13;
                    break;
                case "TIPOPRESTACION":
                    campo = 1;
                    break;
                case "BONIFICACION":
                    campo = 10;
                    break;
                case "SEGUROS":
                    campo = 10;
                    break;
                case "COPAGO":
                    campo = 10;
                    break;
            }
            return campo;
        }

        public static string ZeroFAntiguo(string dato, string relleno, string tipoDato, string direccion)
        {
            //Resta del largo del campo (entregado) menos el length del dato
            int largo = LCFormatoAnt(tipoDato) - dato.Length;
            dato = dato.PadLeft(largo, '0');
            return dato;
        }

        private static int LCFormatoAnt(string campoRegistro)
        {
            int campo = 0;
            switch (campoRegistro.ToUpper())
            {
                case "RUNPACIENTE": campo = 12; break;
                case "PAO": campo = 1; break;
                case "RUNTITULAR": campo = 9; break;
                case "NUMEROFICHA": campo = 10; break;
                case "RUTPACIENTEDV": campo = 9; break;
                case "NOMBREPACIENTE": campo = 30; break;
                case "RUTNN": campo = 10; break;
                case "ITEMPRESTACION": campo = 5; break;
                case "CEROS": campo = 9; break;
                case "TIPOPRESTACION": campo = 1; break;
                case "VALORUNITARIO": campo = 10; break;
                case "CANTIDAD": campo = 5; break;
                case "BASEGURADOR": campo = 9; break;
                case "BADICIONAL": campo = 9; break;
                case "COPAGO": campo = 9; break;
                case "COPAGOFORMULADO": campo = 9; break;
                case "NUMERODOCUMENTO": campo = 10; break;
                case "FECHAATENCION": campo = 8; break;
                case "HORARIOINHABIL": campo = 1; break;
                case "AMBHOS": campo = 1; break;
                case "FECHADOCUMENTO": campo = 8; break;
                case "NUMEROGUIA": campo = 10; break;
                case "NUMEROFACTURA": campo = 10; break;
                case "FECHAFACTURA": campo = 10; break;
            }
            return campo;
        }

        public static string ZeroFCSV(string dato, string relleno, string tipoDato, string direccion)
        {
            //Resta del largo del campo (entregado) menos el length del dato
            int largo = LCFormatoCSV(tipoDato) - dato.Length;
            dato = dato.PadLeft(largo, '0');
            return dato;
        }

        private static int LCFormatoCSV(string campoRegistro)
        {
            int campo = 0;
            switch (campoRegistro.ToUpper())
            {
                case "cuenta": campo = 12; break;
                case "tipotitular": campo = 1; break;
                case "runresponsable": campo = 9; break;
                case "numeroficha": campo = 10; break;
                case "runpaciente": campo = 9; break;
                case "nombrepaciente": campo = 30; break;
                case "rutprestador": campo = 10; break;
                case "itemprestacion": campo = 5; break;
                case "codigoprestacion": campo = 9; break;
                case "tipoprestacion": campo = 1; break;
                case "valorunitario": campo = 10; break;
                case "cantidad": campo = 5; break;
                case "asegurador": campo = 9; break;
                case "fosafe": campo = 9; break;
                case "pagoefectivo": campo = 9; break;
                case "descuentohms": campo = 9; break;
                case "numerodocumento": campo = 10; break;
                case "fechaatencion": campo = 8; break;
                case "horario": campo = 1; break;
                case "tipoatencion": campo = 1; break;
                case "fechafactura": campo = 8; break;
                case "numeroguia": campo = 10; break;
                case "numerofactura": campo = 10; break;
                case "fechafactura2": campo = 10; break;
            }

            return campo;
        }
    }
}
