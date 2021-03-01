using SPN.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPN.Function.Interfaces
{
    public interface IFilterServicePrincipals
    {
        ServicePrincipals GetExpiringAndExpired(List<ActiveDirectoryApplication> applications);
    }
}
