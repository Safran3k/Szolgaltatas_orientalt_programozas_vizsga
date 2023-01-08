using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmekClient.Osztalyok
{
    public class PutRequest
    {
        public uint Id { get; set; }
        public string FelhasznaloNev { get; set; }
        public string Jelszo { get; set; }
        public uint FelhasznaloId { get; set; }
        public string FilmCime { get; set; }
        public string Rendezo { get; set; }
        public string Mufaj { get; set; }
        public string PremierDatuma { get; set; }

    }
}
