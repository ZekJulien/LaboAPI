using DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace LaboAPI.Validators
{
public class PseudoCheckAttribute : ValidationAttribute
{
        //private readonly IMembersServices _membersServices;

        //public PseudoCheckAttribute(IMembersServices membersServices)
        //{
        //    _membersServices = membersServices;
        //}

        //public override bool IsValid(object? value)
        //{
        //    int check = _membersServices.PseudoCheck(value);
        //    if (check != 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}
