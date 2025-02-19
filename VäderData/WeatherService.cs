using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VäderData
{
    internal class WeatherService
    {
        public static int InOrOut()
        {
            Console.WriteLine("1 Inne eller 2 Ute: ");
            return Helpers.GetValidIntegerMinMax(1, 2);
        }
        public static void SearchDay()
        {
            Console.Write("Månad: ");
            int validInteger = Helpers.GetValidIntegerMinMax(6, 12);
            string month = (validInteger > 9 ? "" + validInteger : "0" + validInteger);
            Console.Write("Dag: ");
            int validInteger1 = Helpers.GetValidIntegerMinMax(0, 31);
            string day1 = (validInteger1 > 9 ? "" + validInteger1 : "0" + validInteger1);

            string Date = $"2016-{month}-{day1}";
            var dailyAverages = Filhantering.GetDataAverage(InOrOut() == 1 ? "Inne" : "Ute");
            var filteredData = dailyAverages.Where(m => m.Date == Date).ToList();
            if (filteredData.Count == 0)
            {
                Console.WriteLine("finns ingen data för vald dag");
            }
            foreach (var data in filteredData)
            {
                Console.WriteLine($"Datum: {data.Date} | Medeltemperatur: {data.AvgTemp:F2}°C | Medelluftfuktighet: {data.AvgHumidity:F2}");
            }
        }

        public static void HottestDay()
        {
            var dailyAverages1 = Filhantering.GetDataAverage(InOrOut() == 1 ? "Inne" : "Ute");
            var SortedDataByTemp = (from d in dailyAverages1
                                    orderby d.AvgTemp descending
                                    select d).ToList();
            foreach (var day in SortedDataByTemp)
            {
                Console.WriteLine($"Datum: {day.Date} | Medeltemperatur: {day.AvgTemp:F2}°C ");
            }
        }

        public static void DryestDay()
        {
            var dailyAverages2 = Filhantering.GetDataAverage(InOrOut() == 1 ? "Inne" : "Ute");
            var SortedDataByHumidity = (from d in dailyAverages2
                                        orderby d.AvgHumidity
                                        select d).ToList();
            foreach (var day in SortedDataByHumidity)
            {
                Console.WriteLine($"Datum: {day.Date} | Medelluftfuktighet: {day.AvgHumidity:F2} ");
            }
        }

        public static void MoldRisk()
        {
            var dailyAverages4 = Filhantering.GetDataAverage(InOrOut() == 1 ? "Inne" : "Ute");
            var SortedDataByMoldRisk = (from d in dailyAverages4
                                        orderby d.MoldRisk
                                        select d).ToList();
            foreach (var day in SortedDataByMoldRisk)
            {
                Console.WriteLine($"Datum: {day.Date} | Mögelrisk: {day.MoldRisk:F2}% ");
            }
        }

        public static string DaysInARow(int temp, List<(string Date, double AvgTemp, double AvgHumidity, double MoldRisk)> data)
        {
            int streak = 0;
            string longestStreak = "";
            int days = 0;
            string startDay = "";
            foreach (var day in data)
            {
                if (days == 0 && day.AvgTemp < temp)
                {
                    startDay = day.Date;
                    days = 1;
                }
                else if (days > 0 && day.AvgTemp < temp)
                {
                    days++;
                }
                if (days > 0 && day.AvgTemp >= temp)
                {
                    days = 0;
                }
                if (days == 5)
                {
                    return startDay;
                }
                if (days > streak)
                {
                    streak = days;
                    longestStreak = startDay;
                }
            }
            return "Hittades inte men närmaste var " + longestStreak + " med " + streak + " dagar i sträck";

        }
    }
}
