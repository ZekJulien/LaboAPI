using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IMembersServices
    {
        void Add(MembersEntities Member);

        bool PseudoCheck(string value);
        bool EmailCheck(string value);
        MembersRegisteredEntities? GetDetails(Guid Id);

    }
}
