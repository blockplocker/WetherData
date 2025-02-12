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
                Console.WriteLine(" 1: medel tempratur och luftfucktighet för vald dag. " +
                    "\n 2: Varmaste till kallaste dagen " +
                    "\n 3: torraste till fuktigaste dagen " +
                    "\n 4: Meterologisk höst " +
                    "\n 5: Meterologisk Vinter " +
                    "\n 6: Mögelrisk " +
                    "\n 7: Skapa logg fil");


                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        try
                        {
                            WeatherService.SearchDay();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        Helpers.PressKeyToContinue();
                        break;

                    case "2":
                        try
                        {
                            WeatherService.HottestDay();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        Helpers.PressKeyToContinue();

                        break;

                    case "3":
                        try
                        {
                            WeatherService.DryestDay();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        Helpers.PressKeyToContinue();
                        break;

                    case "4":
                        try
                        {
                            var dailyAverages3 = Filhantering.GetDataAverage("Ute");
                            var regex = new Regex(@"^2016-0[8-9]|2016-1[0-2]");
                            var atumDays = dailyAverages3.Where(d => regex.IsMatch(d.Date)).ToList();
                            Console.WriteLine($"Datum för meterologisk Höst: {WeatherService.DaysInARow(10, atumDays)} ");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        Helpers.PressKeyToContinue();
                        break;

                    case "5":
                        try
                        {
                            Console.WriteLine($"Datum för meterologisk Vinter: {WeatherService.DaysInARow(0, Filhantering.GetDataAverage("Ute"))} ");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        Helpers.PressKeyToContinue();
                        break;

                    case "6":
                        try
                        {
                            WeatherService.MoldRisk();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
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

    }
}


