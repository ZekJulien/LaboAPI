using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exceptions
{
    public class MembersExceptions : Exception
    {
        public MembersExceptions()
        {
        }

        public MembersExceptions(string? message) : base(message)
        {
        }
    }
}
