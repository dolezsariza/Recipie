using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.LogoutRepository.Interfaces
{
    public interface ILogoutRepository
    {
        Task<bool> Logout();
    }
}
