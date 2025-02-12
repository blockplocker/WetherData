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
        public static List<(string Date, double AvgTemp, double AvgHumidity, double MoldRisk)> GetDataAverage(string Choice, int DayOrMonth = 1)
        {
            string text = File.ReadAllText("tempdata5-med fel.txt");
            string pattern = @"2016-(?<Month>[0-1][0-26-9])-(?<Day>[0-2][0-9]|[3][0-1]) (?<Time>[0-2]\d:[0-5]\d:[0-5]\d),(?<State>Inne|Ute),(?<Temp>[0-3][0-9]\.[0-9]|[0-9]\.[0-9]|-[0-9]\.[0-9]),(?<humidity>\d{1,3})";

            Regex regex1 = new Regex(pattern);
            MatchCollection matches = regex1.Matches(text);


            var dailyAveragesInne = matches.Cast<Match>()
            .Where(m => m.Groups["State"].Value == (Choice == "Inne" ? "Inne" : Choice == "Ute" ? "Ute" : ""))// Ternary operator to choose inne or ute
            .GroupBy(m => $"{m.Groups["Month"].Value}{(DayOrMonth == 1 ? "-" + m.Groups["Day"].Value : "")}") // Ternary operator to choose day or month
            .Select(g => (

                Date: $"2016-{g.Key}",
                AvgTemp: g.Average(m => double.Parse(m.Groups["Temp"].Value, CultureInfo.InvariantCulture)),
                AvgHumidity: g.Average(m => int.Parse(m.Groups["humidity"].Value)),
                //MoldRisk: (g.Average(m => double.Parse(m.Groups["Temp"].Value, CultureInfo.InvariantCulture)) > 50 | g.Average(m => double.Parse(m.Groups["Temp"].Value, CultureInfo.InvariantCulture)) < 0 | g.Average(m => int.Parse(m.Groups["humidity"].Value)) < 80 ? 0 : (g.Average(m => int.Parse(m.Groups["humidity"].Value)) - 80) * (5 - (g.Average(m => double.Parse(m.Groups["Temp"].Value, CultureInfo.InvariantCulture)) / 10)) )
                MoldRisk: (g.Average(m => double.Parse(m.Groups["Temp"].Value, CultureInfo.InvariantCulture)) > 50 | g.Average(m => double.Parse(m.Groups["Temp"].Value, CultureInfo.InvariantCulture)) < 0 | g.Average(m => int.Parse(m.Groups["humidity"].Value)) < 80 ? 0 : (g.Average(m => int.Parse(m.Groups["humidity"].Value)) - 78) * (g.Average(m => double.Parse(m.Groups["Temp"].Value, CultureInfo.InvariantCulture)) / 15) / 0.22)
                ))
            .ToList();
            return dailyAveragesInne;
            return null;
        }

        public static void CreateFile()
        {
            File.Create("LoggFile.txt").Dispose();

            for (int i = 0; i < 2; i++)
            {
                var monthlyData = GetDataAverage((i == 0 ? "Inne" : "Ute"), 2);
                using (StreamWriter sw = File.AppendText("LoggFile.txt"))
                {
                    foreach (var monthly in monthlyData)
                    {
                        sw.WriteLine($"{(i == 0 ? "Inne" : "Ute")} Månad: {monthly.Date} | Medeltemperatur: {monthly.AvgTemp:F2}°C | Medelluftfuktighet: {monthly.AvgHumidity:F2} | Mögelrisk: {monthly.MoldRisk:F2}%");
                    }
                }
            }

            using (StreamWriter sw = File.AppendText("LoggFile.txt"))
            {
                var dailyAverages3 = Filhantering.GetDataAverage("Ute");
                var regex = new Regex(@"^2016-0[8-9]|2016-1[0-2]");
                var atumDays = dailyAverages3.Where(d => regex.IsMatch(d.Date)).ToList();
                sw.WriteLine($"Datum för meterologisk Höst: {WeatherService.DaysInARow(10, atumDays)} ");
                sw.WriteLine($"Datum för meterologisk Vinter: {WeatherService.DaysInARow(0, Filhantering.GetDataAverage("Ute"))} ");

                sw.WriteLine("((luftfuktighet -78) * (Temp/15))/0,22");
            }
        }

    }
}
