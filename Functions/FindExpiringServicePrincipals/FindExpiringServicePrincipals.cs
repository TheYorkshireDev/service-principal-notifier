using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SPN.Libraries.AzureService;

namespace SPN.Function
{
    public class FindExpiringServicePrincipals
    {
        private readonly IGraphClient _graphServiceClient;

        public FindExpiringServicePrincipals(IGraphClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        // TODO: Update Timer Frequency
        [FunctionName(nameof(FindExpiringServicePrincipals))]
        public async Task RunAsync([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var applicationFirstPage = await _graphServiceClient.GetAllApplicationsAsync();

            log.LogInformation($"Number of Apps: {applicationFirstPage.Count}");
            foreach (var app in applicationFirstPage)
            {
                log.LogInformation("--------");
                log.LogInformation($"{app.Id}");
                log.LogInformation($"{app.DisplayName}");
                foreach (var sp in app.ServicePrincipals)
                {
                    log.LogInformation($"  {sp.DisplayName}");
                    log.LogInformation($"  {sp.StartDateTime}");
                    log.LogInformation($"  {sp.EndDateTime}");
                }
            }
        }
    }
}
