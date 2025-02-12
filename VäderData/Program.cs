using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VäderData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" 1: medel tempratur och luftfucktighet för vald dag. \n 2: Varmaste till kallaste dagen \n 3: torraste till fuktigaste dagen \n 4: Meterologisk höst \n 5: Meterologisk Vinter \n 6: Mögelrisk \n 7: Skapa logg fil");


                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("1 Inne eller 2 Ute: ");
                        int inOrOut = Helpers.GetValidIntegerMinMax(1, 2);
                        Console.Write("Månad: ");
                        int validInteger = Helpers.GetValidIntegerMinMax(6, 12);
                        string month = (validInteger > 9 ? "" + validInteger : "0" + validInteger);
                        Console.Write("Dag: ");
                        int validInteger1 = Helpers.GetValidIntegerMinMax(0, 31);
                        string day1 = (validInteger1 > 9 ? "" + validInteger1 : "0" + validInteger1);

                        string Date = $"2016-{month}-{day1}";
                        var dailyAverages = Filhantering.GetDataAverage(inOrOut == 1 ? "Inne" : "Ute");
                        var filteredData = dailyAverages.Where(m => m.Date == Date).ToList();
                        if (filteredData.Count == 0)
                        {
                            Console.WriteLine("finns ingen data för vald dag");
                        }
                        foreach (var data in filteredData)
                        {
                            Console.WriteLine($"Datum: {data.Date} | Medeltemperatur: {data.AvgTemp:F2}°C | Medelluftfuktighet: {data.AvgHumidity:F2}");
                        }
                        Helpers.PressKeyToContinue();
                        break;

                    case "2":
                        Console.WriteLine("1 Inne eller 2 Ute: ");
                        int inOrOut2 = Helpers.GetValidIntegerMinMax(1, 2);
                        var dailyAverages1 = Filhantering.GetDataAverage(inOrOut2 == 1 ? "Inne" : "Ute");
                        var SortedDataByTemp = (from d in dailyAverages1
                                                orderby d.AvgTemp descending
                                                select d).ToList();
                        foreach (var day in SortedDataByTemp)
                        {
                            Console.WriteLine($"Datum: {day.Date} | Medeltemperatur: {day.AvgTemp:F2}°C ");
                        }
                        Helpers.PressKeyToContinue();
                        break;

                    case "3":
                        Console.WriteLine("1 Inne eller 2 Ute: ");
                        int inOrOut3 = Helpers.GetValidIntegerMinMax(1, 2);
                        var dailyAverages2 = Filhantering.GetDataAverage(inOrOut3 == 1 ? "Inne" : "Ute");
                        var SortedDataByHumidity = (from d in dailyAverages2
                                                    orderby d.AvgHumidity
                                                    select d).ToList();
                        foreach (var day in SortedDataByHumidity)
                        {
                            Console.WriteLine($"Datum: {day.Date} | Medelluftfuktighet: {day.AvgHumidity:F2} ");
                        }
                        Helpers.PressKeyToContinue();
                        break;

                    case "4":
                        var dailyAverages3 = Filhantering.GetDataAverage("Ute");
                        var regex = new Regex(@"^2016-0[8-9]|2016-1[0-2]");
                        var atumDays = dailyAverages3.Where(d => regex.IsMatch(d.Date)).ToList();
                        Console.WriteLine($"Datum för meterologisk Höst: {DaysInARow(10, atumDays)} ");
                        Helpers.PressKeyToContinue();
                        break;

                    case "5":
                        Console.WriteLine($"Datum för meterologisk Vinter: {DaysInARow(0, Filhantering.GetDataAverage("Ute"))} ");
                        Helpers.PressKeyToContinue();
                        break;

                    case "6":
                        Console.WriteLine("1 Inne eller 2 Ute: ");
                        int inOrOut4 = Helpers.GetValidIntegerMinMax(1, 2);
                        var dailyAverages4 = Filhantering.GetDataAverage(inOrOut4 == 1 ? "Inne" : "Ute");
                        var SortedDataByMoldRisk = (from d in dailyAverages4
                                                    orderby d.MoldRisk
                                                    select d).ToList();
                        foreach (var day in SortedDataByMoldRisk)
                        {
                            Console.WriteLine($"Datum: {day.Date} | Mögelrisk: {day.MoldRisk:F2}% ");
                        }
                        Helpers.PressKeyToContinue();
                        break;

                    case "7":
                        try
                        {
                            Filhantering.CreateFile();
                            Console.WriteLine(" Filen skapad");
                            Helpers.PressKeyToContinue();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;


                }
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
            return  "Hittades inte men närmaste var " + longestStreak + " med " + streak + " dagar i sträck";

        }
    }
}


