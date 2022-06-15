using EntidadesPDV.Caja;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace DatosPDV.AccesoDatos
{
    public class NoObjectDatos : ComunDatos
    {
        SRNoObject.NoObjectClient noObjectClient = new SRNoObject.NoObjectClient();
        SRUsuario.UsuarioClient usuarioCliente = new SRUsuario.UsuarioClient();

        public SRNoObject.NoObjectListType GetListas()
        {
            using (new OperationContextScope(noObjectClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                SRNoObject.NoObjectListType listas = noObjectClient.GetListas();
                return listas;
            }
        }

        public List<CajasEnt> GetListaCajas()
        {
            CajaDatos cajaDatos = new CajaDatos();
            return cajaDatos.ObtenerCajas(new EntidadesPDV.Caja.CajasEnt());
        }

        public List<UsuarioCajaEnt> GetListaUsuarios()
        {
            using (new OperationContextScope(noObjectClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                {
                    Headers = { { "PDV-UserCode", this.UsuarioActual.Login } }
                };

                return (from usuario in usuarioCliente.GetListaUsuarios(true)
                        select new UsuarioCajaEnt()
                        {
                            IdUsuarioCaja = usuario.Login,
                            NombreUsuario = usuario.Nombre
                        }).ToList();
            }
        }
    }
}
