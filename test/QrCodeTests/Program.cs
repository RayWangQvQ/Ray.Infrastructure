using System.Diagnostics;
using Ray.Infrastructure.BarCode;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace QrCodeTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var text = "Hello World";


            //var bitmap = BarCodeHelper.Encode(text);

            //BarCodeHelper.PrintQrCode(bitmap, reverseColor: true);
            //Console.WriteLine(string.Concat(Enumerable.Repeat(Environment.NewLine, 5)));

            //BarCodeHelper.PrintQrCode(bitmap);
            //Console.WriteLine(string.Concat(Enumerable.Repeat(Environment.NewLine, 5)));

            //BarCodeHelper.PrintSmallQrCode(bitmap);


            Image<Rgba32>? bitmap= BarCodeHelper.EncodeByImageSharp(text);
            var base64Str = bitmap.ToBase64String(PngFormat.Instance);

            var result = BarCodeHelper.DecodeByBase64Str(base64Str);

            var miniImage = BarCodeHelper.EncodeByImageSharp(text,optionsAction: op =>
            {
                op.Width = 22;
                op.Height = 22;
            });
            BarCodeHelper.PrintQrCode(miniImage, reverseColor: true);
            Console.WriteLine(string.Concat(Enumerable.Repeat(Environment.NewLine, 5)));
        }
    }
}