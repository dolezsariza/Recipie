using Recipie.Models;
using Recipie.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.LoginRepository.Interfaces
{
    public interface ILoginRepository
    {
        Task<User> Login(LoginPostRequest login);
    }
}
