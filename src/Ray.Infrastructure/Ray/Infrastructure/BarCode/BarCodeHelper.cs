using System;
using System.IO;
using System.Text;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp;
using ZXing.SkiaSharp.Rendering;

namespace Ray.Infrastructure.BarCode
{
    public class BarCodeHelper
    {
        /// <summary>
        /// 解析二维码
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static Result DecodeByBase64Str(string base64Str)
        {
            byte[] arr = Convert.FromBase64String(base64Str);
            using var ms = new MemoryStream(arr);

            using var skiaImage = SkiaSharp.SKBitmap.Decode(ms);

            var skiaReader = new ZXing.SkiaSharp.BarcodeReader();
            Result skiaResult = skiaReader.Decode(skiaImage);

            return skiaResult;
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        public static SKBitmap Encode(string text, BarcodeFormat barcodeFormat = BarcodeFormat.QR_CODE, Action<ZXing.Common.EncodingOptions> optionsAction = null)
        {
            var options = new ZXing.Common.EncodingOptions
            {
                Width = 20,
                Height = 20,
                Margin = 1
            };
            optionsAction?.Invoke(options);

            var writer = new BarcodeWriter
            {
                Format = barcodeFormat,
                Options = options,
                Renderer = new SKBitmapRenderer()
            };
            SKBitmap bitmap = writer.Write(text);
            return bitmap;
        }

        #region 打印二维码

        public static bool[,] AdaptToPoints(SKBitmap bitmap)
        {
            bool[,] points = new bool[bitmap.Width, bitmap.Height];

            var pixels = bitmap.Pixels;

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = pixels[y * bitmap.Width + x];

                    if (pixel == 0xFF000000) //黑色
                    {
                        points[x, y] = true;
                    }
                    else
                    {
                        points[x, y] = false;
                    }
                }
            }

            return points;
        }

        public static void PrintQrCode(SKBitmap skBitmap,
            char pointChar = '\u2588', //默认"█"，全色方块（与底色相反）
            char emptyChar = ' ', //
            bool reverseColor = false, //反转颜色
            Action<char> onCharPrintProcess = null,
            Action onRowPrintProcessed = null)
        {
            var points = AdaptToPoints(skBitmap);
            PrintQrCode(points, pointChar, emptyChar, reverseColor, onCharPrintProcess, onRowPrintProcessed);
        }
        public static void PrintQrCode(bool[,] points,
            char pointChar = '\u2588',
            char emptyChar = ' ',
            bool reverseColor = false,
            Action<char> onCharPrintProcess = null,
            Action onRowPrintProcessed = null)
        {
            var lightColorCharReal = reverseColor ? emptyChar : pointChar;
            var darkColorCharReal = reverseColor ? pointChar : emptyChar;

            var lightCharWidth = lightColorCharReal.GetCharLength();
            var darkCharWidth = darkColorCharReal.GetCharLength();

            onCharPrintProcess ??= Console.Write;
            onRowPrintProcessed ??= Console.WriteLine;

            int width = points.GetLength(0);
            int height = points.GetLength(1);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var bit = points[x, y];
                    var currentChar = bit ? lightColorCharReal : darkColorCharReal;
                    var currentCharWidth = bit ? lightCharWidth : darkCharWidth;
                    var wideCharWidth = 2;

                    var alreadyPrintWidth = 0;
                    while (alreadyPrintWidth < wideCharWidth)
                    {
                        onCharPrintProcess(currentChar);
                        alreadyPrintWidth += currentCharWidth;
                    }
                }
                onRowPrintProcessed();
            }
        }

        public static void PrintSmallQrCode(SKBitmap skBitmap, Action<string> onRowPrintProcess = null)
        {
            var points = AdaptToPoints(skBitmap);
            PrintSmallQrCode(points, onRowPrintProcess);
        }
        public static void PrintSmallQrCode(bool[,] points, Action<string> onRowPrintProcess = null)
        {
            onRowPrintProcess ??= Console.WriteLine;

            int width = points.GetLength(0);
            int height = points.GetLength(1);

            // if there is an odd number of rows, the last one needs special treatment
            for (var y = 0; y < height - 1; y += 2)
            {
                StringBuilder sb = new();

                for (var x = 0; x < width; x++)
                {
                    if (points[x, y] == points[x, y + 1])
                    {
                        sb.Append(points[x, y] ? '█' : ' ');
                    }
                    else
                    {
                        sb.Append(!points[x, y] ? '▄' : '▀');
                    }
                }

                onRowPrintProcess.Invoke(sb.ToString());
            }

            // special treatment for the last row if odd
            if (height % 2 == 1)
            {
                var y = height - 1;
                StringBuilder sb = new();
                for (var x = 0; x < width; x++)
                {
                    if (!points[x, y])
                    {
                        sb.Append(" ");
                    }
                    else
                    {
                        sb.Append('▀');
                    }
                }
                sb.AppendLine();
            }
        }
        #endregion
    }
}
