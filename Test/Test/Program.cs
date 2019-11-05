using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string userNRIC = Console.ReadLine();

            string one = Convert.ToString(userNRIC[1]);
            string two = Convert.ToString(userNRIC[2]);
            string three = Convert.ToString(userNRIC[3]);
            string four = Convert.ToString(userNRIC[4]);
            string five = Convert.ToString(userNRIC[5]);
            string six = Convert.ToString(userNRIC[6]);
            string seven = Convert.ToString(userNRIC[7]);

            int sum = (Convert.ToInt32(one) * 2) +
                (Convert.ToInt32(two) * 7) +
                (Convert.ToInt32(three) * 6) +
                (Convert.ToInt32(four) * 5) +
                (Convert.ToInt32(five) * 4) +
                (Convert.ToInt32(six) * 3) +
                (Convert.ToInt32(seven) * 2);

            Console.WriteLine(sum);
            Console.WriteLine(userNRIC);
            Console.WriteLine(one);
            Console.WriteLine(two);
            Console.WriteLine(three);
            Console.WriteLine(four);
            Console.WriteLine(five);
            Console.WriteLine(six);
            Console.WriteLine(seven);

        }
    }
}
