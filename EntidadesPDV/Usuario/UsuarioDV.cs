using EntidadesPDV.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesPDV.Usuario
{
    public class UsuarioDV
    {
        public string Login { set; get; }
        public string Nombre { set; get; }
        public bool Activo { set; get; }
        public string Password { set; get; }
        public string IpCaja { set; get; }
        public string MacCaja { get; set; }
        public MenuDV Menu { get; set; }

        public bool ExisteError { get; set; }
        public string ErrorMensaje { get; set; }
        public string ErrorCode { get; set; }

        #region "Listados"
        public List<Perfil.PerfilDV> ListaPerfil { get; set; }
        #endregion

        public UsuarioDV()
        {
            ListaPerfil = new List<Perfil.PerfilDV>();
        }

        public bool TienePerfil(PerfilUsuario tipo)
        {
            bool valido = false;
            if (this.ListaPerfil != null && this.ListaPerfil.Count > 0 && tipo != PerfilUsuario.Ninguno)
            {
                string nombre = string.Empty;
                switch (tipo)
                {
                    case PerfilUsuario.Admisionista:
                        nombre = "admisionista";
                        break;
                    case PerfilUsuario.AnalistaAprobacion:
                        nombre = "analista_aprobacion";
                        break;
                    case PerfilUsuario.SupervisorCaja:
                        nombre = "supervisor_caja";
                        break;
                    case PerfilUsuario.SupervisorTesoreria:
                        nombre = "supervisor_tesoreria";
                        break;
                    case PerfilUsuario.SupervisorGarantia:
                        nombre = "supervisor_garantia";
                        break;
                    case PerfilUsuario.SupervisorRecaudador:
                        nombre = "supervisor_recaudador";
                        break;
                    case PerfilUsuario.SupervisorCuentaHospitalaria:
                        nombre = "supervisor_cuentahospital";
                        break;
                    case PerfilUsuario.JefeAdmisión:
                        nombre = "jefe_admision";
                        break;
                    case PerfilUsuario.Administrador:
                        nombre = "administrador";
                        break;
                }

                valido = this.ListaPerfil.Exists(p => p.Nombre.Trim().ToUpper() == nombre.Trim().ToUpper());
            }
            return valido;
        }

        public string NombrePerfilStoreProcedure()
        {
            //ADMISIONISTA
            //SUPERVISOR
            //RECAUDADORCENTRAl
            //ANALISTAAPROBACION
            //SUPERVISORGARANTIA
            //CUENTAHOSPITALARIA

            string nombrePerfil = string.Empty;
            if (this.TienePerfil(PerfilUsuario.SupervisorCuentaHospitalaria))
            {
                nombrePerfil = "CUENTAHOSPITALARIA";
            }
            else if (this.TienePerfil(PerfilUsuario.SupervisorCaja))
            {
                nombrePerfil = "SUPERVISOR";
            }
            else if (this.TienePerfil(PerfilUsuario.Admisionista))
            {
                nombrePerfil = "ADMISIONISTA";
            }
            else if (this.TienePerfil(PerfilUsuario.SupervisorRecaudador))
            {
                nombrePerfil = "RECAUDADORCENTRAl";
            }
            else if (this.TienePerfil(PerfilUsuario.AnalistaAprobacion))
            {
                nombrePerfil = "ANALISTAAPROBACION";
            }
            else if (this.TienePerfil(PerfilUsuario.SupervisorGarantia))
            {
                nombrePerfil = "SUPERVISORGARANTIA";
            }
            
            return nombrePerfil;
        }

        public bool TieneAcceso(string controller, string action)
        {
            bool acceso = false;
            if (this.Menu != null)
            {
                if (this.Menu.ListaMenusHijos != null && this.Menu.ListaMenusHijos.Count > 0)
                {
                    if (this.Menu.ListaMenusHijos.ToList().Exists(e => e.Value.Exists(d => d.Controlador == controller)))
                    {
                        acceso = true;
                    }
                }
            }
            return acceso;
        }
    }
}
