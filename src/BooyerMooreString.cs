using System;
using System.Collections.Generic;

namespace src {
    class BoyerMooreString {
        private static int NO_OF_CHARS = 256;

        private static void BadCharHeuristic(string str, int size, int[] badChar) {
            for (int i = 0; i < NO_OF_CHARS; i++)
                badChar[i] = -1;

            for (int i = 0; i < size; i++)
                badChar[(int) str[i]] = i;
        }

        public static List<int> Search(string text, string pattern) {
            List<int> results = new List<int>();
            int m = pattern.Length;
            int n = text.Length;
            int[] badChar = new int[NO_OF_CHARS];
            BadCharHeuristic(pattern, m, badChar);

            while(s <= (n - m)) {
                int j = m - 1;
                while(j >= 0 && pattern[j] == text[s + j])
                    j--;

                if (j < 0) {
                    results.Add(s);
                    s += (s + m < n) ? m - badChar[text[s + m]] : 1;
                } else {
                    s += Math.Max(1, j - badChar[text[s + j]]);
                }
            }
            return results;
        }
    }
}
