using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class AuthExceptions : Exception
    {
        public AuthExceptions() { }
        public AuthExceptions(string message)
         : base(message)
        {
        }
        public AuthExceptions(string message, Exception inner)
         : base(message, inner)
        {
        }
    }
}
