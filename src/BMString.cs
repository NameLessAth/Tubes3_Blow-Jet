namespace src{
    class BMString{
        public static List<int> BuildBM(String pattern){
            List<int> temp = []; List<Boolean> diisi = [];
            for (int i = 0; i < 128; i++){temp.Add(pattern.Length); diisi.Add(false);}
            for (int i = 0; i < pattern.Length; i++){
                if (i != pattern.Length-1) temp[(int) pattern[i]] = pattern.Length-i-1;
            } return temp;
        }
        public static int BMmatching(String arg, String pattern){
            List<int> temp = BMString.BuildBM(pattern);
            if (pattern.Length > arg.Length) return -1;
            int i = pattern.Length-1, j = pattern.Length-1, iStart = i;
            while (iStart < arg.Length && j >= 0){
                if (arg[i] == pattern[j]){
                    i--; j--;
                } else{
                    iStart += temp[(int) arg[i]];
                    i = iStart; j = pattern.Length-1;
                }
            } if (j < 0) return i+1;
            else return -1;
        }
    }
}