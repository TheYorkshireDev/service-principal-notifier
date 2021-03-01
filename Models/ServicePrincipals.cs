using System;
using System.Collections.Generic;
using System.Text;

namespace SPN.Models
{
    public class ServicePrincipals
    {
        public List<ActiveDirectoryApplication> Expiring { get; set; }
        public List<ActiveDirectoryApplication> Expired { get; set; }
    }
}
