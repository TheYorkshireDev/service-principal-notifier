using SPN.Function.Interfaces;
using SPN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPN.Function.Services
{
    public class FilterServicePrincipals : IFilterServicePrincipals
    {
        public ServicePrincipals GetExpiringAndExpired(List<ActiveDirectoryApplication> applications)
        {
            var expired = new List<ActiveDirectoryApplication>();
            var expiring = new List<ActiveDirectoryApplication>();

            foreach (var app in applications)
            {
                var expiredServicePrincipals = new List<ServicePrincipal>();
                var expiringServicePrincipals = new List<ServicePrincipal>();
                foreach (var sp in app.ServicePrincipals)
                {
                    if (sp.EndDateTime < DateTime.UtcNow)
                    {
                        expiredServicePrincipals.Add(sp);
                        continue;
                    }

                    if (sp.EndDateTime <= DateTime.UtcNow.AddDays(30))
                    {
                        expiringServicePrincipals.Add(sp);
                    }
                }

                if (expiredServicePrincipals.Count > 0)
                {
                    expired.Add(new ActiveDirectoryApplication
                    {
                        Id = app.Id,
                        DisplayName = app.DisplayName,
                        ServicePrincipals = expiredServicePrincipals
                    });
                }

                if (expiringServicePrincipals.Count > 0)
                {
                    expiring.Add(new ActiveDirectoryApplication
                    {
                        Id = app.Id,
                        DisplayName = app.DisplayName,
                        ServicePrincipals = expiringServicePrincipals
                    });
                }
            }

            return new ServicePrincipals
            {
                Expired = expired,
                Expiring = expiring
            };
        }
    }
}
