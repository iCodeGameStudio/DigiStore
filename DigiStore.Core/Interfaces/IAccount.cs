using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.DataAccessLayer.Entities;

namespace DigiStore.Core.Interfaces
{
    public interface IAccount
    {
        bool ExistMobileNumber(string mobileNumber);

        void AddUser(User user);

        int GetMaxRole();

        int GetStoreRole();

        bool ActivateUser(string code);

        User LoginUser(string mobileNumber, string password);

        bool ResetPassword(string code, string password);

        string GetUserActiveCode(string mobileNumber);

        void AddStore(Store store);

        void UpdateUserRole(string mobileNumber);

        int GetUserID(string mobileNumber);

        bool ExistsEmailAddress(string mail);
    }
}
