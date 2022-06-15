using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatosPDV.AccesoDatos;
using EntidadesPDV;
using EntidadesPDV.Caja;
using EntidadesPDV.Usuario;

namespace NegocioPDV.Caja
{
    public class CajaBusiness
    {
        CajaDatos cajaAD = new DatosPDV.AccesoDatos.CajaDatos();
        NoObjectDatos noODataAD = new NoObjectDatos();

        public CajaBusiness()
        {

        }
        public CajaBusiness(UsuarioDV Info)
        {
            cajaAD.UsuarioActual = Info;
            noODataAD.UsuarioActual = Info;
        }


        public List<CajasEnt> GetListaCajas(CajasEnt entrada)
        {
            return cajaAD.ObtenerCajas(entrada);
        }
        public Boolean CajaNueva(CajasEnt CajaEBD)
        {
            return cajaAD.CajaNueva(CajaEBD);
        }

        public CajasEnt GetCaja(CajasEnt cajaID)
        {
            return cajaAD.ObtenerCaja(cajaID);
        }

        public RespuestaTransaccion ModificarCaja(CajasEnt cajaEBD, Boolean HabCaja)
        {
            CajasEnt caja = this.cajaAD.ObtenerCaja(cajaEBD);
            caja.HabilitaCaja = HabCaja;
            return cajaAD.CajaModificar(caja);
        }
        public Boolean AsignarSupervisorCaja(CajasEnt caja)
        {
            return cajaAD.CajaModificarSupervisor(caja);
        }
        public RespuestaTransaccion AsignarSupervisorVariasCajas(List<string> lstCajasId, string idSupervisorCaja, string supervisorCaja)
        {
            RespuestaTransaccion salida = new RespuestaTransaccion();

            if (lstCajasId != null && lstCajasId.Count > 0)
            {
                salida = cajaAD.CajaModificarSupervisorMasivo(lstCajasId, idSupervisorCaja, supervisorCaja);
            }
            else
            {
                salida.EsError = true;
                salida.Mensaje = "No ha seleccionado ninguna caja.";
            }
            return salida;
        }

        public RespuestaTransaccion EditarCaja(CajasEnt cajaEBD)
        {
            return cajaAD.CajaModificar(cajaEBD);
        }
    }
}
