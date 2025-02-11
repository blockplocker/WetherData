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
                //Console.Clear();
                Console.WriteLine(" 1: medel tempratur och luftfucktighet för vald dag. \n 2: Varmaste till kallaste dagen \n 3: torraste till fuktigaste dagen \n 4: Meterologisk höst");


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
                        var dailyAverages = Filhantering.GetDailyData(inOrOut == 1 ? "Inne" : "Ute");
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
                        var dailyAverages1 = Filhantering.GetDailyData(inOrOut2 == 1 ? "Inne" : "Ute");
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
                        var dailyAverages2 = Filhantering.GetDailyData(inOrOut3 == 1 ? "Inne" : "Ute");
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
                        var dailyAverages3 = Filhantering.GetDailyData("Ute");
                        var regex = new Regex(@"^2016-0[8-9]|2016-1[0-2]");
                        var atumDays = dailyAverages3.Where(d => regex.IsMatch(d.Date)).ToList();

                        int days = 0;
                        (string Date, double AvgTemp, double AvgHumidity)? startDay = null;
                        foreach (var day in atumDays)
                        {
                            if (days == 0 && day.AvgTemp < 10)
                            {
                                startDay = day;
                                days = 1;
                            }
                            if (days > 0 && day.AvgTemp < 10)
                            {
                                days++;
                            }
                            if (days > 0 && day.AvgTemp >= 10)
                            {
                                days = 0;
                            }
                            if (days == 5)
                            {
                                Console.WriteLine($"Datum för meterologisk höst: {day.Date} ");
                                Helpers.PressKeyToContinue();
                                break;
                            }
                        }
                        Console.WriteLine("Det finns inte meterologisk höst i datan");
                        Helpers.PressKeyToContinue();
                        break;
                    case "5":
                        //(day.AvgTemp >0 && day.AvgTemp <50 && day.AvgHumidity <80 )
                        break;

                }
            }
        }
    }
}


