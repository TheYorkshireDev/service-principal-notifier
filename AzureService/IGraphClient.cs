using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace SPN.Libraries.AzureService
{
    public interface IGraphClient
    {
        Task<IList<Application>> GetAllApplicationsAsync();
    }
}
