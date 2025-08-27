using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Core.Interfaces;
using DigiStore.Core.Services;
using DigiStore.DataAccessLayer.Entities;

namespace DigiStore.Core.Classes
{
    public class PanelLayoutScope
    {
        private IUser _user;
        private IStore _store;
        private IAdmin _admin;

        public PanelLayoutScope(IUser user,IStore store,IAdmin admin)
        {
            _user = user;
            _store = store;
            _admin = admin;
        }

        public string GetUserRoleName(string username)
        {
           return _user.GetUserRoleName(username);
        }

        public int GetBrandsCount()
        {
            return _admin.GetBrandNotShowCount();
        }

        public string GetLogoStore(string username)
        {
            return _store.GetLogoStore(username);
        }

        public string GetFullName(string username)
        {
            return _user.GetFullName(username);
        }

        public int GetUserId(string username)
        {
            return _user.GetUserId(username);
        }

        public string GetNameStore(int UserId)
        {
            return _store.GetNameStore(UserId);
        }

        public bool GetActivateStore(int UserId)
        {
            return _store.CheckActivateStore(UserId);
        }

        public bool ExistsFieldCategory(int id, int categoryId)
        {
            return _admin.ExistsFieldCategory(id, categoryId);
        }

        public ProductField GetProductField(int id,int pid)
        {
            return _store.GetProductField(id,pid);
        }

        public int GetProductSeen(int id)
        {
            return _store.GetProductSeen(id);
        }


    }
}
