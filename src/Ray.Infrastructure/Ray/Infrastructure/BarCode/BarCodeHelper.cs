using System;
using System.IO;
using System.Linq;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp;
using ZXing.SkiaSharp.Rendering;

namespace Ray.Infrastructure.BarCode;

public class BarCodeHelper
{
    #region 解码

    /// <summary>
    /// 解析二维码
    /// </summary>
    /// <param name="base64Str"></param>
    /// <returns></returns>
    public static Result DecodeByBase64Str(string base64Str)
    {
        var re = DecodeByBase64Str(base64Str, out Image<Rgba32> img);
        img.Dispose();
        return re;
    }

    /// <summary>
    /// 解析二维码（使用SkiaSharp）
    /// </summary>
    /// <param name="base64Str"></param>
    /// <param name="skBitmap">注意要自己Dispose掉</param>
    /// <returns></returns>
    public static Result DecodeByBase64Str(string base64Str, out SKBitmap skBitmap)
    {
        if (base64Str.Contains(";base64,"))
        {
            base64Str = base64Str.Split(";base64,").ToList().Last();
        }

        byte[] arr = Convert.FromBase64String(base64Str);
        using var ms = new MemoryStream(arr);

        skBitmap = SKBitmap.Decode(ms);

        var skiaReader = new BarcodeReader();
        Result skiaResult = skiaReader.Decode(skBitmap);

        return skiaResult;
    }

    /// <summary>
    /// 解析二维码（使用ImageSharp）
    /// </summary>
    /// <param name="base64Str"></param>
    /// <param name="img"></param>
    /// <returns></returns>
    public static Result DecodeByBase64Str(string base64Str, out Image<Rgba32> img)
    {
        if (base64Str.Contains(";base64,"))
        {
            base64Str = base64Str.Split(";base64,").ToList().Last();
        }

        var bytes = Convert.FromBase64String(base64Str);
        using var ms = new MemoryStream(bytes);
        Image image = Image.Load(ms);
        img = image.CloneAs<Rgba32>();

        var reader = new ZXing.ImageSharp.BarcodeReader<Rgba32>
        {
            Options = new DecodingOptions
            {
                //PossibleFormats = new[] { BarcodeFormat.QR_CODE },
                TryHarder = true,
                //PureBarcode = false
            },
        };

        Result result = reader.Decode(img);

        return result;
    }

    #endregion

    #region 编码

    /// <summary>
    /// 生成二维码（使用SkiaSharp）
    /// </summary>
    public static SKBitmap EncodeSkiaSharp(
        string text,
        BarcodeFormat barcodeFormat = BarcodeFormat.QR_CODE,
        Action<ZXing.Common.EncodingOptions> optionsAction = null
    )
    {
        var options = new ZXing.Common.EncodingOptions
        {
            Width = 20,
            Height = 20,
            Margin = 1,
        };
        optionsAction?.Invoke(options);

        var writer = new BarcodeWriter
        {
            Format = barcodeFormat,
            Options = options,
            Renderer = new SKBitmapRenderer(),
        };
        SKBitmap bitmap = writer.Write(text);
        return bitmap;
    }

    /// <summary>
    /// 生成二维码（使用ImageSharp）
    /// </summary>
    /// <param name="text"></param>
    /// <param name="barcodeFormat"></param>
    /// <param name="optionsAction"></param>
    /// <returns></returns>
    public static Image<Rgba32> EncodeByImageSharp(
        string text,
        BarcodeFormat barcodeFormat = BarcodeFormat.QR_CODE,
        Action<ZXing.Common.EncodingOptions> optionsAction = null
    )
    {
        var options = new ZXing.Common.EncodingOptions
        {
            Width = 300,
            Height = 300,
            Margin = 1,
        };
        optionsAction?.Invoke(options);

        var writer = new ZXing.ImageSharp.BarcodeWriter<Rgba32>
        {
            Format = barcodeFormat,
            Options = options,
        };

        Image<Rgba32> image = writer.WriteAsImageSharp<Rgba32>(text);
        return image;
    }

    #endregion

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

    public static bool[,] AdaptToPoints(Image<Rgba32> image)
    {
        const int threshold = 180;
        bool[,] points = new bool[image.Width, image.Height];

        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                var color = image[x, y];

                if (color.B <= threshold)
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

    public static void PrintQrCode(
        SKBitmap skBitmap,
        char pointChar = '\u2588', //默认"█"，全色方块（与底色相反）
        char emptyChar = ' ', //
        bool reverseColor = false, //反转颜色
        Action<string> onRowPrintProcess = null
    )
    {
        var points = AdaptToPoints(skBitmap);
        PrintQrCode(points, pointChar, emptyChar, reverseColor, onRowPrintProcess);
    }

    public static void PrintQrCode(
        Image<Rgba32> image,
        char pointChar = '\u2588', //默认"█"，全色方块（与底色相反）
        char emptyChar = ' ', //
        bool reverseColor = false, //反转颜色
        Action<string> onRowPrintProcess = null
    )
    {
        var points = AdaptToPoints(image);
        PrintQrCode(points, pointChar, emptyChar, reverseColor, onRowPrintProcess);
    }

    public static void PrintQrCode(
        bool[,] points,
        char pointChar = '\u2588',
        char emptyChar = ' ',
        bool reverseColor = false,
        Action<string> onRowPrintProcess = null
    )
    {
        var lightColorCharReal = reverseColor ? emptyChar : pointChar;
        var darkColorCharReal = reverseColor ? pointChar : emptyChar;

        var lightCharWidth = lightColorCharReal.GetCharLength();
        var darkCharWidth = darkColorCharReal.GetCharLength();

        onRowPrintProcess ??= Console.WriteLine;

        int width = points.GetLength(0);
        int height = points.GetLength(1);
        for (var y = 0; y < height; y++)
        {
            var row = new StringBuilder();
            for (var x = 0; x < width; x++)
            {
                var bit = points[x, y];
                var currentChar = bit ? lightColorCharReal : darkColorCharReal;
                var currentCharWidth = bit ? lightCharWidth : darkCharWidth;
                var wideCharWidth = 2;

                var alreadyPrintWidth = 0;
                while (alreadyPrintWidth < wideCharWidth)
                {
                    row.Append(currentChar);
                    alreadyPrintWidth += currentCharWidth;
                }
            }

            onRowPrintProcess(row.ToString());
        }
    }

    public static void PrintSmallQrCode(
        SKBitmap skBitmap,
        Action<string> onRowPrintProcess = null
    )
    {
        var points = AdaptToPoints(skBitmap);
        PrintSmallQrCode(points, onRowPrintProcess);
    }

    public static void PrintSmallQrCode(
        Image<Rgba32> image,
        Action<string> onRowPrintProcess = null
    )
    {
        var points = AdaptToPoints(image);
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