using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmacyManagement.Core.Models;
namespace PharmacyManagement.Core.Interfaces
{
    public interface IAuthService
    {
        User Login(string username, string password);
        void Logout();

    }

}
