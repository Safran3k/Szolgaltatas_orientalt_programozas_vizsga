using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmekClient.Osztalyok
{
    public class Felhasznalo
    {
        uint id;

        public uint ID
        {
            get { return id; }
            set { id = value; }
        }

        string felhasznaloNev;

        public string FelhasznaloNev
        {
            get { return felhasznaloNev; }
            set { felhasznaloNev = value; }
        }

        string jelszo;

        public string Jelszo
        {
            get { return jelszo; }
            set { jelszo = value; }
        }
    }
}
