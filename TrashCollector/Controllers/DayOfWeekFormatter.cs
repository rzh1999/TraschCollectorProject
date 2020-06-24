using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollector.Controllers
{
    public class DayOfWeekFormatter
    {

        public static string FormatDay(string toformat)
        {
            //string allLower = toformat.ToLower();
            //string firstCapital = char.ToUpper(allLower[0]).ToString();

             return char.ToUpper(toformat[0]) + toformat.Substring(1);
        }
    }
}
