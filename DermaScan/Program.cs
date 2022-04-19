using System;
using DermaScan.Functions;

namespace DermaScan
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("How many days in advance would u like to search?: ");
                var days = Console.ReadLine();

                Console.Write("How many minutes between each search do you want: ");
                var time = Console.ReadLine();

                Console.Write("What e-mail address do you want the appointment sent to?: ");
                var email = Console.ReadLine();

                if (days != null && time != null)
                {
                    Dermatoloog.Scraper(Int32.Parse(days), Int32.Parse(time), email);
            
                }
                else
                {
                    Console.WriteLine("Wrong input, try again");
                }
            }
        }
    }
}