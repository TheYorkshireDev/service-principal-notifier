using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class ServicePrincipal
    {
        public string DisplayName { get; set; }
        public DateTimeOffset? StartDateTime { get; set; }
        public DateTimeOffset? EndDateTime { get; set; }
    }
}
