using SPN.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace SPN.Libraries.AzureService
{
    public class GraphClient : IGraphClient
    {
        private readonly IGraphServiceClient _graphServiceClient;

        public GraphClient(IGraphServiceClient graphServiceClient)
        {
            if (graphServiceClient is null)
            {
                throw new ArgumentNullException(nameof(graphServiceClient));
            }

            _graphServiceClient = graphServiceClient;
        }

        public async Task<IList<ActiveDirectoryApplication>> GetAllApplicationsAsync()
        {
            // NOTE: We are specifically specifying the return items from the request
            // to reduce data bandwith
            var applicationFirstPage = await _graphServiceClient.Applications.Request()
                .Select(a =>
                new
                {
                    a.AppId,
                    a.DisplayName,
                    a.PasswordCredentials
                })
                .Top(999)
                .GetAsync();

            var applications = new List<ActiveDirectoryApplication>();
            var pageIterator = PageIterator<Application>
                .CreatePageIterator(_graphServiceClient, applicationFirstPage, (a) =>
                {
                    var servicePrincipals = new List<Models.ServicePrincipal> { };
                    foreach (var sp in a.PasswordCredentials)
                    {
                        servicePrincipals.Add(new Models.ServicePrincipal
                        {
                            DisplayName = sp.DisplayName,
                            StartDateTime = sp.StartDateTime,
                            EndDateTime = sp.EndDateTime
                        });
                    }
                    applications.Add(new ActiveDirectoryApplication
                    {
                        Id = a.AppId,
                        DisplayName = a.DisplayName,
                        ServicePrincipals = servicePrincipals
                    });
                    return true;
                });

            await pageIterator.IterateAsync();

            return applications;
        }
    }
}
