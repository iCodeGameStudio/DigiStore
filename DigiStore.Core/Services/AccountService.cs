using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.DataAccessLayer.Entities;
using DigiStore.DataAccessLayer.Context;
using DigiStore.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DigiStore.Core.Classes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;

namespace DigiStore.Core.Services
{
    public class AccountService : IAccount
    {
        private DatabaseContext _context;
        public AccountService(DatabaseContext context)
        {
            _context = context;
        }

        public bool ActivateUser(string code)
        {
            User user = _context.Users.FirstOrDefault(u => u.IsActive == false && u.ActiveCode == code);
            if (user != null)
            {
                user.IsActive = true;
                user.ActiveCode = CodeGenerators.ActiveCode();
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool ExistMobileNumber(string mobileNumber)
        {
            return _context.Users.Any(u => u.Mobile == mobileNumber);
        }

        public int GetMaxRole()
        {
            return _context.Roles.Max(r => r.Id);
        }

        public string GetUserActiveCode(string mobileNumber)
        {
            User user = _context.Users.FirstOrDefault(u => u.Mobile == mobileNumber);
            if (user != null)
            {
                user.ActiveCode = CodeGenerators.ActiveCode();
            }
            _context.SaveChanges();
            return user.ActiveCode;
        }

        public User LoginUser(string mobileNumber, string password)
        {
            return _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Mobile == mobileNumber && u.Password == password);
        }

        public void AddStore(Store store)
        {
                _context.Stores.Add(store);
                _context.SaveChanges();
        }

        public bool ResetPassword(string code, string password)
        {
            User user = _context.Users.FirstOrDefault(u => u.ActiveCode == code);
            if (user != null)
            {
                user.Password = HashGenerators.MD5Encoding(password);
                user.ActiveCode = CodeGenerators.ActiveCode();
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateUserRole(string mobileNumber)
        {
            User user = _context.Users.FirstOrDefault(u => u.Mobile == mobileNumber);
            Role role = _context.Roles.FirstOrDefault(r => r.Name == "فروشگاه");
            user.RoleId = role.Id;
            user.IsActive = false;
            _context.SaveChanges();
        }

        public int GetStoreRole()
        {
            return _context.Roles.FirstOrDefault(r => r.Name == "فروشگاه").Id;
        }

        public int GetUserID(string mobileNumber)
        {
            return _context.Users.FirstOrDefault(u => u.Mobile == mobileNumber).Id;
        }

        public bool ExistsEmailAddress(string mail)
        {
            return _context.Stores.Any(s => s.Mail == mail);
        }
    }
}
