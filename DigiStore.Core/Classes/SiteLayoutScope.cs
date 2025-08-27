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
    public class SiteLayoutScope
    {
        private ITemp _temp;

        public SiteLayoutScope(ITemp temp)
        {
            _temp = temp;
        }

        public bool CheckBanner(int id)
        {
            return _temp.CheckBannerImg(id);
        }

        public BannerDetails GetBanner(int id)
        {
            return _temp.GetBannerDetails(id);
        }
    }
}
