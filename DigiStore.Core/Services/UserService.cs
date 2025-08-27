using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.DataAccessLayer.Context;
using DigiStore.DataAccessLayer.Entities;
using DigiStore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace DigiStore.Core.Services
{
    public class UserService : IUser
    {
        private DatabaseContext _context;

        public UserService(DatabaseContext context)
        {
            _context = context; 
        }

        public void ActiveMailAddress(string mailAddress)
        {
            Store store = _context.Stores.FirstOrDefault(s => s.Mail == mailAddress);
            store.MailActivate = true;
            _context.SaveChanges();
        }

        public void ActiveMobileNumber(string mobileNumber)
        {
            Store store = _context.Stores.Include(s => s.User).FirstOrDefault(s => s.User.Mobile == mobileNumber);
            store.MobileActivate = true;
            _context.SaveChanges();

        }

        public bool ExistsMailActivate(string username, string code)
        {
            return _context.Stores.Include(s => s.User).Any(s => s.User.Mobile == username && s.MailActivateCode == code);
        }

        public bool ExistsMobileActivate(string username, string code)
        {
         //   return _context.Users.Include(u => u.Mobile).Any(u => u.Mobile == username && u.ActiveCode == code);
            return _context.Users.Any(u => u.Mobile == username && u.ActiveCode == code);


        }

        public bool ExistsPermission(int permissionID,int roleID)
        {
            return _context.RolePermissions.Any(r => r.RoleId == roleID && r.PermissionId == permissionID);
        }

        public string GetFullName(string username)
        {
            return _context.Users.Where(u => u.Mobile == username).Select(u => u.FullName).FirstOrDefault();
        }

        public int GetUserId(string username)
        {
            return _context.Users.Where(u => u.Mobile == username).FirstOrDefault().Id;
        }

        public int GetUserRole(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Mobile == username).RoleId;
        
        }

        public string GetUserRoleName(string username)
        {
            return _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Mobile == username).Role.Name;

        }

        public Store GetUserStore(string username)
        {
            return _context.Stores.Include(s => s.User).FirstOrDefault(s => s.User.Mobile == username);
        }
    }
}
