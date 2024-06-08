using System;
using System.Collections.Generic;

namespace src {
    class BoyerMooreString {
        public static void Main(String[] args){
            // ini ngeconvert image ke 
            Console.WriteLine(Search("bajingan", "ngan"));
        }
        private static int NO_OF_CHARS = 256;

        private static void BadCharHeuristic(string str, int size, int[] badChar) {
            for (int i = 0; i < NO_OF_CHARS; i++)
                badChar[i] = -1;

            for (int i = 0; i < size; i++)
                badChar[(int)str[i]] = i;
        }

        public static int Search(string text, string pattern) {
            int m = pattern.Length;
            int n = text.Length;
            
            int[] badChar = new int[NO_OF_CHARS];
            BadCharHeuristic(pattern, m, badChar);

            int s = 0; // Initialize s to 0
            while (s <= (n - m)) {
                int j = m - 1;

                // Keep reducing index j of pattern while characters of
                // pattern and text are matching at this shift s
                while (j >= 0 && pattern[j] == text[s + j])
                    j--;

                // If the pattern is present at current shift, then index j
                // will become -1 after the above loop
                if (j < 0) {
                    return s; // Pattern found at index s

                    // Shift the pattern so that the next character in text
                    // aligns with the last occurrence of it in pattern.
                    // The condition s+m < n is necessary for the case when
                    // pattern occurs at the end of text
                    s += (s + m < n) ? m - badChar[text[s + m]] : 1;
                } else {
                    // Shift the pattern so that the bad character in text
                    // aligns with the last occurrence of it in pattern. The
                    // max function is used to make sure that we get a positive
                    // shift. We may get a negative shift if the last occurrence
                    // of bad character in pattern is on the right side of the
                    // current character.
                    s += Math.Max(1, j - badChar[text[s + j]]);
                }
            }

            return -1; // Pattern not found
        }
    }
}
