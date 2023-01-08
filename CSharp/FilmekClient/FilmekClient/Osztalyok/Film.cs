using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmekClient.Osztalyok
{
    class Film
    {
        uint id;

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        uint felhasznaloId;

        public uint FelhasznaloId
        {
            get { return felhasznaloId; }
            set { felhasznaloId = value; }
        }

        string filmCime;

        public string FilmCime
        {
            get { return filmCime; }
            set { filmCime = value; }
        }

        string rendezo;

        public string Rendezo
        {
            get { return rendezo; }
            set { rendezo = value; }
        }

        string mufaj;

        public string Mufaj
        {
            get { return mufaj; }
            set { mufaj = value; }
        }

        DateTime premierDatuma;

        public DateTime PremierDatuma
        {
            get { return premierDatuma; }
            set { premierDatuma = value; }
        }


        public Film()
        {
            
        }

        public Film(uint id, string filmCime, string rendezo, string mufaj, DateTime premierDatuma)
        {
            Id = id;
            FilmCime = filmCime;
            Rendezo = rendezo;
            Mufaj = mufaj;
            PremierDatuma = premierDatuma;
        }
    }
}
