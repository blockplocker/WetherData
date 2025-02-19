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

                List<string> list = new List<string>()
                {
                    " medel tempratur och luftfucktighet för vald dag. ",
                    " Varmaste till kallaste dagen ",
                    " torraste till fuktigaste dagen ",
                    " Meterologisk höst ",
                    " Meterologisk Vinter ",
                    " Mögelrisk ",
                    " Skapa logg fil"
                };
                list.NumberedList();


                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Helpers.RunWithErrorHandling(WeatherService.SearchDay);
                        break;

                    case "2":
                        Helpers.RunWithErrorHandling(WeatherService.HottestDay);
                        break;

                    case "3":
                        Helpers.RunWithErrorHandling(WeatherService.DryestDay);
                        break;

                    case "4":

                        var dailyAverages3 = Filhantering.GetDataAverage("Ute");
                        var regex = new Regex(@"^2016-0[8-9]|2016-1[0-2]");
                        var atumDays = dailyAverages3.Where(d => regex.IsMatch(d.Date)).ToList();
                        Helpers.PrintWithErrorHandling($"Datum för meterologisk Höst: {WeatherService.DaysInARow(10, atumDays)} ");
                        break;

                    case "5":
                        Helpers.PrintWithErrorHandling($"Datum för meterologisk Vinter: {WeatherService.DaysInARow(0, Filhantering.GetDataAverage("Ute"))} ");
                        break;

                    case "6":
                        Helpers.RunWithErrorHandling(WeatherService.MoldRisk);
                        break;

                    case "7":
                        Helpers.RunWithErrorHandling(Filhantering.CreateFile);
                        break;
                }
            }

        }

    }
}


