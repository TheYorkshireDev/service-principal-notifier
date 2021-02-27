using System;
using System.Collections.Generic;

namespace Models
{
    public class ActiveDirectoryApplication
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public List<ServicePrincipal> ServicePrincipals { get; set; }
    }
}
