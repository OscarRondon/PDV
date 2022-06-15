using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntidadesPDV
{
    public class RespuestaTransaccion
    {
        public bool EsError { get; set; }
        public string Mensaje { get; set; }
        public object Data { get; set; }
        public string DocEntry { set; get; }
        public string DocNum { set; get; }
        public object DataByte { get; set; }
    }
}