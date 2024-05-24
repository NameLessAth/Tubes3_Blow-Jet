using System;
using System.Text.RegularExpressions;

namespace StringConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "m42w4Rd_4Njay";
            string pattern = "[0123456]|[a-z]";
            string resultangka = Regex.Replace(input, pattern, new MatchEvaluator(replacement));

            string resulttitle = Regex.Replace(resultangka.ToLower(), @"\b[a-z]", match => match.Value.ToUpper());

            Console.WriteLine("original string: " + input);
            Console.WriteLine("converted string: " + resulttitle);
            Console.ReadKey();
        }

        static string replacement(Match match)
        {
            switch (match.Value)
            {
                case "6":
                    return "g";
                case "1":
                    return "i";
                case "0":
                    return "o";
                case "5":
                    return "s";
                case "2":
                    return "z";
                case "4":
                    return "a";
                case "3":
                    return "e";
                case "[a-z]":
                    return "[a-z]";
                default:
                    return match.Value;
            }
        }
    }
}
