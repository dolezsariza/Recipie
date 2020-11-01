using Recipie.Models;
using Recipie.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipie.Repositories.RegisterRepository.Interfaces
{
    public interface IRegisterRepository
    {
        Task<User> Register(RegisterPostRequest registerRequest);
    }
}
