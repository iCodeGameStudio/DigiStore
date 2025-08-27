using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.DataAccessLayer.Entities;

namespace DigiStore.Core.Interfaces
{
    public interface IUser
    {
        bool ExistsPermission(int permissionID,int roleID);
        int GetUserRole(string username);
        string GetUserRoleName(string username);
        Store GetUserStore(string username);
        bool ExistsMailActivate(string username, string code);
        bool ExistsMobileActivate(string username, string code);
        void ActiveMailAddress(string mainAddress);
        void ActiveMobileNumber(string mobileNumber);

        string GetFullName(string username);

        int GetUserId(string username);
    }
}
