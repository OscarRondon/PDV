using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesPDV.Menu
{
    public class MenuDV
    {
        public string Name { set; get; }
        public string Code { set; get; }
        public string Padre { set; get; }
        public string Controlador { set; get; }
        public string Funcion { set; get; }
        public int Orden { set; get; }
        public List<MenuDV> ListaMenusPadres { set; get; }
        public Dictionary<string, List<MenuDV>> ListaMenusHijos { set; get; }
    }
}
