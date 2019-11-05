using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            do
            {
                DisplayMenu();
                Console.Write("Enter your option: ");
                choice = Convert.ToInt32(Console.ReadLine());
                Console.Write($"You have selected option {choice}");
                Console.WriteLine();
                Console.WriteLine();
            } while (choice > 0);
        }
        static void DisplayMenu()
        {
            Console.WriteLine("ADMIN MENU");
            Console.WriteLine("==========");
            Console.WriteLine("[1] Read bicycle info from file");
            Console.WriteLine("[2] Display all bicycle info with servicing indication");
            Console.WriteLine("[3] Display selected bicycle info");
            Console.WriteLine("[4] Add a bicycle");
            Console.WriteLine("[5] Perform bicycle maintainence");
            Console.WriteLine("[0] Exit");
        }
    }
}
