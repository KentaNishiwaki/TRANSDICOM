using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace TRANSDICOM.Model
{
    public class CreateImageModel
    {
        public byte[] GetBlankImage(string text)
        {
            const string WatermarkFont = "Meiryo UI";
            FontFamily fontFamily;
            if (!SystemFonts.TryGet(WatermarkFont, out fontFamily))
            {
                throw new Exception("Font Err");
            }
            var font = fontFamily.CreateFont(22f, FontStyle.Regular);
            using (var bmp = new Image<Rgba32>(128, 128, Color.Black))
            {
                using (var stream = new MemoryStream())
                {
                    bmp.Mutate(i =>
                    {
                        i.DrawText(text, font, Color.Yellow, new PointF(10, 10));
                    });
                    var encoder = new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder();
                    bmp.Save(stream, encoder);
                    return stream.GetBuffer();

                }
            }

        }
    }
}
