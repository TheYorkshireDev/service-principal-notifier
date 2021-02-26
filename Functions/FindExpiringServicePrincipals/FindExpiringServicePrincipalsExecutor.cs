using SPN.Function.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SPN.Function
{
    public class FindExpiringServicePrincipalsExecutor : IFindExpiringServicePrincipalsExecutor
    {
        public void Execute()
        {
            throw new NotImplementedException();
            // TODO:
            // Get All Apps
            // Loop through and get Exipred and Exipring SPs
            // Compile SendGrid Email
        }
    }
}
