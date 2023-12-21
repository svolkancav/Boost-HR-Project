using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Project.Common
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(int companyId);
        Task<bool> ValidateTokenAsync(string token, int companyId);
    }
}
