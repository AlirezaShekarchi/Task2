using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication13.Entities;

namespace WebApplication13.Services.UserServices
{
   public interface IUserService
    {
        ViewModel.Vm_User Authenticate(string username, string password);
        IEnumerable<ViewModel.Vm_User> GetAll();
        string AddUser(User user);
    }
}
