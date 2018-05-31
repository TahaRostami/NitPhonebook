using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nit.Phonebook.Logics
{
    public static class Helper
    {
        public static string AddHalfSpace(this string str)
        {
            string str2 = "";

            for (int i = str.Length - 1; i >= 0; i--)
            {
                str2 += str[i] + '\u2009'.ToString();
            }
            return str2;
        }

        public static string PreNumber { get; set; }

        public static string AddPreNumber(this string str)
        {
            string str2 = "";

            str2 = PreNumber + str;

            return str2;
        }


    }
}
