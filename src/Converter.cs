using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace src{
    class Converter{
        // public static void Main(String[] args){
        //     // ini ngeconvert image ke 
        //     Console.WriteLine(ConvertImageTo30Ascii("../test/SOCOFing/Real/1__M_Left_index_finger.BMP"));
        // }

        static String ImageToBinaryString(string imagePath){
            using Image<Rgba32> image = Image.Load<Rgba32>(imagePath);
            String binaryTemp = "";
            
            for (int i = 0; i < image.Height; i++){
                for (int j = 0; j < image.Width; j++){
                    Rgba32 pixelColor = image[j, i];
                    float grayscaleValue = pixelColor.R * 0.299f + pixelColor.G * 0.587f + pixelColor.B * 0.114f;
                    if (grayscaleValue >= 128) binaryTemp += '1';
                    else binaryTemp += '0';
                }
            }

            return binaryTemp;
        }

        public static String BinaryToASCII(String binaryArg){
            String binary = binaryArg;
            while (binary.Length % 8 != 0){
                binary = binary.Substring(0, binary.Length-1);
            } 
            String temp = "";
            for (int i=0; i<binary.Length; i+=8){
                byte asciitemp = Convert.ToByte(binary.Substring(i,8), 2);
                temp += (char)asciitemp;
            } 
            return temp;
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
            return FindMostUniqueSubstring(BinaryToASCII(ImageToBinaryString(imagepath)));
        }
    }
}