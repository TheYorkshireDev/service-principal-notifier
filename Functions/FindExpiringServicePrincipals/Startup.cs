using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using SPN.Libraries.AzureService;
using System.Net.Http.Headers;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(SPN.Startup))]
namespace SPN
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var client = GetGraphApiClient().Result;
            builder.Services.AddSingleton<IGraphServiceClient>(client);
            builder.Services.AddSingleton<IGraphClient, GraphClient>();
        }

        private static async Task<GraphServiceClient> GetGraphApiClient()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            string accessToken = await azureServiceTokenProvider
                .GetAccessTokenAsync("https://graph.microsoft.com/");

            var graphServiceClient = new GraphServiceClient(
                new DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage
                .Headers
                .Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                    return Task.CompletedTask;
                }));

            return graphServiceClient;
        }
    }
}
