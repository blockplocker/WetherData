using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VäderData
{
    internal class Helpers
    {
        public static int GetValidIntegerMinMax( int min = int.MinValue, int max = int.MaxValue)
        {
            int result;
            while (true)
            {
                try
                {
                    if (int.TryParse(Console.ReadLine(), out result) && result >= min && result <= max)
                        return result;

                    Console.WriteLine($"Ogiltig inmatning. Vänligen ange ett nummer mellan {min} och {max}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade: {ex.Message}");
                }
            }
        }
        public static void PressKeyToContinue()
        {
            //Console.WriteLine("Press any key to continue...");
            Console.WriteLine("tryck en valfri knapp för att fortsätta");
            Console.ReadKey();
        }
        public static void PrintWithErrorHandling(string input)
        {
            try
            {
                Console.WriteLine(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            PressKeyToContinue();
        }
        public static void RunWithErrorHandling(MyDelegates.ActionDelegate action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            PressKeyToContinue();
        }
    }
}
