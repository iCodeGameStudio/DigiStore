using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiStore.Core.Classes
{
    public class CodeGenerators
    {
        //برای تولید کد فعالسازی
        public static string ActiveCode()
        {
            Random random = new Random();
            return random.Next(100000, 999000).ToString();
        }

        //برای تولید کد شماره سفارش
        public static string FactorCode()
        {
            Random random = new Random();
            return random.Next(10000000, 99990000).ToString();
        }

        //برای تولید کد برای نامگذاری عکس ها
        public static string FileCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
