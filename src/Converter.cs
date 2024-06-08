using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.EntityFrameworkCore;

namespace src{
    class Converter{
        public static String selectBerkasFromFingerprint(String imagePath, String algo){
            // ini ngeconvert ke 30 ascii querynya
            String asciiQuery = ConvertImageTo30Ascii(imagePath);
            // buat result (image path yang akan dipilih terakhir, nantinya jadi nama orangnya)
            String result = "";
            // temporary, harusnya list of `berkas_citra` di sql nya
            List<String> a = []; 
            // coba pake kmp/bm dulu
            foreach (String imageItr in a){
                if (algo == "KMP" && KMPString.KMPmatching(ImageToBinaryString(imageItr), asciiQuery) != -1){
                    result = imageItr;
                } else if (algo == "BM" && BMString.BMmatching(ImageToBinaryString(imageItr), asciiQuery) != -1){
                    result = imageItr;
                }
            } 
            // kalo kmp/bm gadapet, baru nyoba pake lcs, cari yang paling panjang
            int longestcs = -1;
            foreach(String imageItr in a){
                String temp = ConvertImageTo30Ascii(imageItr);
                int lcsRes = LCS.lcsDP(temp.ToCharArray(), asciiQuery.ToCharArray(), temp.Length, asciiQuery.Length, new int[temp.Length+1, asciiQuery.Length+1]);
                if (lcsRes > longestcs){
                    longestcs = lcsRes; result = imageItr;
                }
            }
            // di sini query resultnya, terus return nama yang ada di resultnya
            // placeholder
            return result;
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