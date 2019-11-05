using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRIC
{
    class Program
    {
        static void Main(string[] args)
        {
                //Main Program
                bool isValid;
                Console.Write("Enter the NRIC (not FIN) to be verified: ");
                string userNRIC = Console.ReadLine().ToUpper(); 

                //Check correct length
                int length = userNRIC.Length;
                if (length != 9)
                {
                    isValid = false;
                    Console.WriteLine($"The validity of the IC: {isValid}");
                    return;
                }

                //Verify first letter
                if (Convert.ToString(userNRIC[0]) == "S")
                {
                    isValid = true;
                }
                else if (Convert.ToString(userNRIC[0]) == "T")
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                    Console.WriteLine($"The validity of the IC: {isValid}");
                    return; //To exit the program if NRIC is not valid
                }

                //Convert from char to string
                string one = Convert.ToString(userNRIC[1]);
                string two = Convert.ToString(userNRIC[2]);
                string three = Convert.ToString(userNRIC[3]);
                string four = Convert.ToString(userNRIC[4]);
                string five = Convert.ToString(userNRIC[5]);
                string six = Convert.ToString(userNRIC[6]);
                string seven = Convert.ToString(userNRIC[7]);

            //Calculate numbers to obtain checksum number & check if inputs are numeric
            int sum = 0;
            try
            {
                sum = (Convert.ToInt32(one) * 2) +
                        (Convert.ToInt32(two) * 7) +
                        (Convert.ToInt32(three) * 6) +
                        (Convert.ToInt32(four) * 5) +
                        (Convert.ToInt32(five) * 4) +
                        (Convert.ToInt32(six) * 3) +
                        (Convert.ToInt32(seven) * 2);
            }

            catch (Exception)
            {
                isValid = false;
                Console.WriteLine($"The validity of the IC: {isValid}");
                return;
            }
                if (Convert.ToString(userNRIC[0]) == "T")
                {
                    sum = sum + 4;
                }

                int remainder = sum % 11;

                //Check checksum letter
                List<string> checksum = new List<string> { "J", "Z", "I", "H", "G", "F", "E", "D", "C", "B", "A" };
                if (Convert.ToString(userNRIC[8]) == checksum[remainder])
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
            }
            Console.WriteLine($"The validity of the IC: {isValid}");
            }
    }
}
