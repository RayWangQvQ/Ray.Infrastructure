using System;
using Ray.Infrastructure.BarCode;
using SkiaSharp;

namespace QrCodeTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var text = "Hello World";

            SKBitmap? bitmap = BarCodeHelper.EncodeSkiaSharp(text);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            var base64Str = Convert.ToBase64String(data.ToArray());

            var result = BarCodeHelper.DecodeByBase64Str(base64Str);

            var miniImage = BarCodeHelper.EncodeSkiaSharp(
                text,
                optionsAction: op =>
                {
                    op.Width = 22;
                    op.Height = 22;
                }
            );
            BarCodeHelper.PrintQrCode(miniImage, reverseColor: true);
            Console.WriteLine(string.Concat(System.Linq.Enumerable.Repeat(Environment.NewLine, 5)));
        }
    }
}
