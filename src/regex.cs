using System;
using System.Text.RegularExpressions;

namespace StringConversion
{
    class Program
    {
        // Regex.match to convert angka to char
        public static string replacement(Match match)
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
                case "8":
                    return "b";
                case "12":
                    return "r";
                case "13":
                    return "b";
                case " [a-z]":
                    return " [A-Z]";
                default:
                    return match.Value;
            }
        }
    }
}
