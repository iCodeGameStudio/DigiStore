using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiStore.Core.Classes
{
    public static class NumberConvertor
    {
        public static string ToEnglishNumber(this string persianStr)
        {
            if (string.IsNullOrEmpty(persianStr))
                return persianStr;

            persianStr = persianStr.Replace("۰", "0");
            persianStr = persianStr.Replace("۱", "1");
            persianStr = persianStr.Replace("۲", "2");
            persianStr = persianStr.Replace("۳", "3");
            persianStr = persianStr.Replace("۴", "4");
            persianStr = persianStr.Replace("۵", "5");
            persianStr = persianStr.Replace("۶", "6");
            persianStr = persianStr.Replace("۷", "7");
            persianStr = persianStr.Replace("۸", "8");
            persianStr = persianStr.Replace("۹", "9");

            return persianStr;
        }
        public static string ToPersianNumber(this string englishStr)
        {
            if (string.IsNullOrEmpty(englishStr))
                return englishStr;

            englishStr = englishStr.Replace("0", "۰");
            englishStr = englishStr.Replace("1", "۱");
            englishStr = englishStr.Replace("2", "۲");
            englishStr = englishStr.Replace("3", "۳");
            englishStr = englishStr.Replace("4", "۴");
            englishStr = englishStr.Replace("5", "۵");
            englishStr = englishStr.Replace("6", "۶");
            englishStr = englishStr.Replace("7", "۷");
            englishStr = englishStr.Replace("8", "۸");
            englishStr = englishStr.Replace("9", "۹");

            return englishStr;
        }

    }
}
