using System;
using System.Drawing;

namespace src{
    class LCS{
        public static int lcsDP(char[] X, char[] Y, int m, int n, int[, ] L) {
            if (m == 0 || n == 0) return 0;
            if (L[m, n] != -1) return L[m, n];
            if (X[m - 1] == Y[n - 1]) {
                L[m, n] = 1 + lcsDP(X, Y, m - 1, n - 1, L);
                return L[m, n];
            }

            L[m, n] = max(lcsDP(X, Y, m, n - 1, L), lcsDP(X, Y, m - 1, n, L));
            return L[m, n];
        }

        static int max(int a, int b) { 
            return (a > b) ? a : b; 
        }

        // Contoh Penggunaan

        // public static void Main()
        // {
        //     String s1 = "AGGTAB";
        //     String s2 = "GXTXAYB";

        //     char[] X = s1.ToCharArray();
        //     char[] Y = s2.ToCharArray();
        //     int m = X.Length;
        //     int n = Y.Length;
        //     int[, ] L = new int[m + 1, n + 1];
        //     for (int i = 0; i <= m; i++) {
        //         for (int j = 0; j <= n; j++) {
        //             L[i, j] = -1;
        //         }
        //     }
        //     Console.Write("Length of LCS is" + " " + lcsDP(X, Y, m, n, L));
        // }
    }

}