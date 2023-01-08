using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmekClient.Osztalyok
{
    public class LoginResponse
    {
        private int error;

        public int Error
        {
            get { return error; }
            set { error = value; }
        }


        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }


        private List<Felhasznalo> felhasznalo;

        public List<Felhasznalo> Felhasznalo
        {
            get { return felhasznalo; }
            set { felhasznalo = value; }
        }
    }
}
