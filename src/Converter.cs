using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.EntityFrameworkCore;

namespace src{
    class Converter{
        public static (String, double) selectBerkasFromFingerprint(String imagePath, String algo){
            // ini ngeconvert ke 30 ascii querynya
            String asciiFull = ImageToBinaryString(imagePath);
            String asciiQuery = FindMostUniqueSubstring(asciiFull);
            // buat result (image path yang akan dipilih terakhir, nantinya jadi nama orangnya)
            (String, String) result = ("", "");
            // buat persentase kecocokan
            double cocok = 0;
            // temporary, harusnya list of `berkas_citra` di sql nya
            List<(String, String)> a = DatabaseManager.GetSidikJari(); 
            // coba pake kmp/bm dulu
            foreach ((String, String) imageItr in a){
                if (algo == "KMP" && KMPString.KMPmatching(ImageToBinaryString(imageItr.Item1), asciiQuery) != -1){
                    result = imageItr;
                    cocok = 1;
                } else if (algo == "BM" && BMString.BMmatching(ImageToBinaryString(imageItr.Item1), asciiQuery) != -1){
                    result = imageItr;
                    cocok = 1;
                }
            } 
            // kalo kmp/bm gadapet, baru nyoba pake lcs, cari yang paling panjang
            if (result.Item1 == ""){
                int longestcs = -1;
                foreach((String, String) imageItr in a){
                    String temp = imageItr.Item1;
                    int[, ] L = new int[temp.Length + 1, asciiFull.Length + 1];
                    for (int i = 0; i <= temp.Length; i++) {
                        for (int j = 0; j <= asciiFull.Length; j++) {
                            L[i, j] = -1;
                        }
                    } 
                    int lcsRes = LCS.lcsDP(temp.ToCharArray(), asciiFull.ToCharArray(), temp.Length, asciiFull.Length, L);
                    if (lcsRes > longestcs){
                        longestcs = lcsRes; result = imageItr;
                    } 
                } 
                cocok = longestcs/Convert.ToDouble(Math.Min(asciiFull.Length, result.Item1.Length)); 
                
            }
            // di sini query resultnya, terus return nama yang ada di resultnya
            // placeholder
            
            return (result.Item2, cocok);
        }

        static String ImageToBinaryString(string imagePath){
            using Image<Rgba32> image = Image.Load<Rgba32>(imagePath);
            String binaryTemp = "";
            
            for (int i = 0; i < image.Height; i++){
                for (int j = 0; j < image.Width; j++){
                    Rgba32 pixelColor = image[j, i];
                    int grayscaleValue = Convert.ToInt32(pixelColor.R * 0.299f + pixelColor.G * 0.587f + pixelColor.B * 0.114f);
                    binaryTemp += Convert.ToString((char)grayscaleValue);
                }
            }
            return binaryTemp;
        }    

        public static string FindMostUniqueSubstring(string input){
            Dictionary<char, int> charCount = new Dictionary<char, int>();
            int maxUnique = 0;
            int bestI = 0;

            for (int i = 0; i < 30; i++){
                if (!charCount.ContainsKey(input[i])) charCount[input[i]] = 0;
                charCount[input[i]]++;
            }
            maxUnique = charCount.Count;

            for (int i = 30; i < input.Length; i++){
                char outgoingChar = input[i - 30];
                charCount[outgoingChar]--;
                if (charCount[outgoingChar] == 0) charCount.Remove(outgoingChar);
                char incomingChar = input[i];
                if (!charCount.ContainsKey(incomingChar)) charCount[incomingChar] = 0;
                charCount[incomingChar]++;
                
                if (charCount.Count > maxUnique) {
                    maxUnique = charCount.Count;
                    bestI = i - 30 + 1;
                }
            }

            return input.Substring(bestI, 30);
        }

        public static string ConvertImageTo30Ascii(string imagepath){
            return FindMostUniqueSubstring(ImageToBinaryString(imagepath));
        }
    }
}