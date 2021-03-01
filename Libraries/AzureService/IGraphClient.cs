﻿using SPN.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SPN.Libraries.AzureService
{
    public interface IGraphClient
    {
        Task<IList<ActiveDirectoryApplication>> GetAllApplicationsAsync();
    }
}
