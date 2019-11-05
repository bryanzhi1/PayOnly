using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
         Console.Write("Enter your height(m): ");
         double height = Convert.ToDouble(Console.ReadLine());
         Console.Write("Enter your weight(kg): ");
         double weight = Convert.ToDouble(Console.ReadLine());
         double BMI = weight / (height * height);
            Console.WriteLine();
            Console.WriteLine($"Your BMI is {BMI}");
        }
    }
}
