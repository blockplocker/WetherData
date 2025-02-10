using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VäderData
{
    internal class Filhantering
    {



        public static void demo()
        {

        string text = File.ReadAllText("tempdata5-med fel.txt");
        string regex = @"2016-(?<Month>0|1[0-26-9])-(?<Day>[0-3][0-9]) (?<Time>[0-2]\d:[0-5]\d:[0-5]\d),(?<State>Inne|Ute),(?<Temp>[0-3][0-9].[0-9]),(?<humidity>\d{1,3})";

            foreach (Match m in Regex.Matches(text, regex))
            {
                if (m.Groups.Count > 0)
                {
                    foreach (Group group in m.Groups)
                    {
                        Console.WriteLine(group.Name + ": " + group.Value);
                    }
                }
                Console.WriteLine(m.Value + " hittad på index " + m.Index);
                Console.ReadKey();
            }
        }

    }
}
