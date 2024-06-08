using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using Microsoft.EntityFrameworkCore;

namespace src{
    class ConverterImage{
        public static (String, double) SelectBerkasFromFingerprint(String imagePath, String algo){
            // ini ngeconvert ke 30 ascii querynya
            String asciiFull = ImageToBinaryString(imagePath);
            String asciiQuery = FindMostUniqueSubstring(asciiFull);
            // buat result (image path yang akan dipilih terakhir, nantinya jadi nama orangnya)
            (String, String) result = ("", "");
            // buat persentase kecocokan
            double cocok = 0;
            // temporary, harusnya list of `berkas_citra` di sql nya
            List<(String, String)> listSidikJari = DatabaseManager.GetSidikJari(); 
            // coba pake kmp/bm dulu
            foreach ((String, String) imageItr in listSidikJari){
                if (algo == "KMP" && KMPString.KMPmatching(ImageToBinaryString(imageItr.Item1), asciiQuery) != -1){
                    result = imageItr;
                    cocok = 1;
                } else if (algo == "BM" && BoyerMooreString.Search(ImageToBinaryString(imageItr.Item1), asciiQuery) != -1){
                    result = imageItr;
                    cocok = 1;
                }
            } 
            // kalo kmp/bm gadapet, baru nyoba pake lcs, cari yang paling panjang
            if (result.Item1 == ""){
                int longestcs = -1;
                foreach((String, String) imageItr in listSidikJari){
                    String temp = ImageToBinaryString(imageItr.Item1);
                    int lcsRes = LCS.lcsDP(temp, asciiFull);
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

        public static String ImageToBinaryString(string imagePath){
            using Image<Rgba32> image = Image.Load<Rgba32>(imagePath);
            String binaryTemp = "";

            int width = image.Width;
            int height = image.Height;

            // Define a threshold for detecting altered regions
            int threshold = 50;

            // looping
            for (int y = 1; y < height - 1; y++){
                for (int x = 1; x < width - 1; x++){
                    Rgba32 pixel = image[x, y];
                    int grayValue = (int)(pixel.R * 0.299 + pixel.G * 0.587 + pixel.B * 0.114);

                    // Check neighboring pixels
                    int[] neighbors = new int[8];
                    neighbors[0] = GetGrayValue(image[x - 1, y - 1]);
                    neighbors[1] = GetGrayValue(image[x, y - 1]);
                    neighbors[2] = GetGrayValue(image[x + 1, y - 1]);
                    neighbors[3] = GetGrayValue(image[x - 1, y]);
                    neighbors[4] = GetGrayValue(image[x + 1, y]);
                    neighbors[5] = GetGrayValue(image[x - 1, y + 1]);
                    neighbors[6] = GetGrayValue(image[x, y + 1]);
                    neighbors[7] = GetGrayValue(image[x + 1, y + 1]);

                    int avgNeighborValue = 0;
                    for (int i = 0; i < neighbors.Length; i++) avgNeighborValue += neighbors[i];

                    avgNeighborValue /= neighbors.Length;

                    if (Math.Abs(grayValue - avgNeighborValue) > threshold){
                        image[x, y] =  new Rgba32((byte)avgNeighborValue, (byte)avgNeighborValue, (byte)avgNeighborValue);
                    }
                }
            } image.Save("../coba/halo.png");
            for (int i = 0; i < image.Height; i++){
                for (int j = 0; j < image.Width; j++){
                    Rgba32 pixelColor = image[j, i];
                    int grayscaleValue = Convert.ToInt32(pixelColor.R * 0.299f + pixelColor.G * 0.587f + pixelColor.B * 0.114f);
                    binaryTemp += Convert.ToString((char)grayscaleValue);
                }
            }
            return binaryTemp;
        }    
        static int GetGrayValue(Rgba32 pixelColor){
            return Convert.ToInt32(pixelColor.R * 0.299f + pixelColor.G * 0.587f + pixelColor.B * 0.114f);
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