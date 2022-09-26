using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class TournamentExceptions : Exception
    {
        public TournamentExceptions() { }
        public TournamentExceptions(string message)
         : base(message)
        {
        }
        public TournamentExceptions(string message, Exception inner)
         : base(message, inner)
        {
        }
    }
}
