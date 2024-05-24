using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace src{
    class Converter{
        public static void Main(String[] args){
            Console.WriteLine(KMPString.KMPmatching("halohalohalo", "hall"));
        }

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
    }
}