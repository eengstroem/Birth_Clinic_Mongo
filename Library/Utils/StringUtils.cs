using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Utils
{
    public class StringUtils
    {
        public static string DateTimeListToCommaSeparatedString(List<DateTime> dates)
        {
            string result = "";
            for (int i = 0; i < dates.Count; i++)
            {
                result += dates[i].ToShortDateString() + " " + dates[i].ToShortTimeString();
                if (i<dates.Count)
                {
                    result += ", ";
                } else
                {
                    result += ".";
                }
            }
            return result;
        }
    }
}
