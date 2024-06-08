using System.Collections;

namespace src{
    class KMPString{
        public static List<int> BuildKMP(String arg){
            List<int> temp = new List<int>();   
            temp.Add(0);
            int j = 0, i = 1;
            while (i < arg.Length){
                if (arg[j] == arg[i]){
                    temp.Add(j + 1);
                    i++; j++;
                } else {
                    if (j == 0){
                        temp.Add(0);
                        i++;
                    } else j = temp[j-1];
                }
            } return temp;
        }
        public static int KMPmatching(String arg, String pattern){
            List<int> temp = BuildKMP(pattern);
            int i = 0, j = 0;
            while (i < arg.Length && j < pattern.Length){
                if (arg[i] != pattern[j]){
                    if (j == 0) i++;
                    else j = temp[j-1];
                } else{
                    i++; j++;
                }
            } 
            if (j == pattern.Length) return i-j;
            else return -1;
        }
    }
}