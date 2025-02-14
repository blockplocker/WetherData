using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VäderData
{
    internal static class MyExtentions
    {
        public static void Cw(this string str)
        {
            Console.WriteLine(str);
        }
        
        public static void NumberedList<T>(this List<T> items)
        {
            for (int i = 1; i <= items.Count; i++)
            {
                (i + ". " + items[i - 1]).Cw();
            }
        }
    }
}
