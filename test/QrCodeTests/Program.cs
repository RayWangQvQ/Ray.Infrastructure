using System.Diagnostics;
using Ray.Infrastructure.BarCode;

namespace QrCodeTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var text = "Hello World";
            var bitmap = BarCodeHelper.Encode(text);

            BarCodeHelper.PrintQrCode(bitmap, reverseColor: true);
            Console.WriteLine(string.Concat(Enumerable.Repeat(Environment.NewLine, 5)));

            BarCodeHelper.PrintQrCode(bitmap);
            Console.WriteLine(string.Concat(Enumerable.Repeat(Environment.NewLine, 5)));

            BarCodeHelper.PrintSmallQrCode(bitmap);
        }
    }
}