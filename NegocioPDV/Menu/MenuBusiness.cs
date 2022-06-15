using DatosPDV.AccesoDatos;
using EntidadesPDV.Menu;
using EntidadesPDV.Perfil;
using EntidadesPDV.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioPDV.Menu
{
    public class MenuBusiness
    {
        MenuDatos menuad = new MenuDatos();       

        public MenuDV GetListaMenuUsuario(UsuarioDV usuario)
        {
            MenuDV menu = new MenuDV();

            List<MenuDV> listaMenus = menuad.GetListaMenuUsuario(usuario);
            List<MenuDV> listaMenuPadre = listaMenus.Where(m => string.IsNullOrEmpty(m.Padre)).ToList();
            List<MenuDV> listaMenuHijos = listaMenus.Where(m => string.IsNullOrEmpty(m.Padre) == false).ToList();
            menu.ListaMenusPadres = listaMenuPadre;
            menu.ListaMenusHijos = new Dictionary<string, List<MenuDV>>();
            foreach (MenuDV menuPa in listaMenuPadre)
            {
                menu.ListaMenusHijos.Add(menuPa.Code, listaMenus.Where(mp => mp.Padre == menuPa.Code).OrderBy(mp => mp.Orden).ToList());
            }
            return menu;
        }
    }
}
