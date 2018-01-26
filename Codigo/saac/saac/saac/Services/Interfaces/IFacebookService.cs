using saac.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace saac.Services.Interfaces
{
    public interface IFacebookService
    {
        Task<Facebook> GetFacebookProfileAsync(string accessToken);
    }
}
