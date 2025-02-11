using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VäderData
{
    internal class Filhantering
    {
        public static List<(string Date, double AvgTemp, double AvgHumidity)> GetDailyData(string Choice)
        {
            string text = File.ReadAllText("tempdata5-med fel.txt");
            string pattern = @"2016-(?<Month>[0-1][0-26-9])-(?<Day>[0-2][0-9]|[3][0-1]) (?<Time>[0-2]\d:[0-5]\d:[0-5]\d),(?<State>Inne|Ute),(?<Temp>[0-3][0-9].[0-9]),(?<humidity>\d{1,3})";

            Regex regex1 = new Regex(pattern);
            MatchCollection matches = regex1.Matches(text);


            if (Choice == "Inne")
            {
                var dailyAveragesInne = matches.Cast<Match>()
                .Where(m => m.Groups["State"].Value == "Inne")
                .GroupBy(m => $"{m.Groups["Month"].Value}-{m.Groups["Day"].Value}")
                .Select(g => (

                    Date: $"2016-{g.Key}",
                    AvgTemp: g.Average(m => double.Parse(m.Groups["Temp"].Value, CultureInfo.InvariantCulture)),
                    AvgHumidity: g.Average(m => int.Parse(m.Groups["humidity"].Value))
               ))
                .ToList();
                return dailyAveragesInne;
            }
            else if (Choice == "Ute")
            {

            var dailyAveragesUte = matches.Cast<Match>()
                .Where(m => m.Groups["State"].Value == "Ute")
                .GroupBy(m => $"{m.Groups["Month"].Value}-{m.Groups["Day"].Value}")
                .Select(g => (
                    Date: $"2016-{g.Key}",
                    AvgTemp: g.Average(m => double.Parse(m.Groups["Temp"].Value, CultureInfo.InvariantCulture)),
                    AvgHumidity: g.Average(m => int.Parse(m.Groups["humidity"].Value))
                ))
                .ToList();

                return dailyAveragesUte;
            }
            return null;          


        }

    }
}
