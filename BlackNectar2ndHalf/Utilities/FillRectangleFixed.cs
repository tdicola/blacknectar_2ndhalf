using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BlackNectar2ndHalf
{
    // Fix a bug in WriteableBitmapEx where FillRect adds an extra pixel on the first line
    public static partial class WriteableBitmapExtensions
    {
        private const int SizeOfArgb = 4;

        public static void FillRectangleFixed(this WriteableBitmap bmp, int x1, int y1, int x2, int y2, Color color)
        {
            // Add one to use mul and cheap bit shift for multiplicaltion
            var a = color.A + 1;
            var col = (color.A << 24)
                     | ((byte)((color.R * a) >> 8) << 16)
                     | ((byte)((color.G * a) >> 8) << 8)
                     | ((byte)((color.B * a) >> 8));
            bmp.FillRectangleFixed(x1, y1, x2, y2, col);
        }

        public static void FillRectangleFixed(this WriteableBitmap bmp, int x1, int y1, int x2, int y2, int color)
        {
            // Use refs for faster access (really important!) speeds up a lot!
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int[] pixels = bmp.Pixels;

            // Check boundaries
            if (x1 < 0) { x1 = 0; }
            if (y1 < 0) { y1 = 0; }
            if (x2 < 0) { x2 = 0; }
            if (y2 < 0) { y2 = 0; }
            if (x1 >= w) { x1 = w - 1; }
            if (y1 >= h) { y1 = h - 1; }
            if (x2 >= w) { x2 = w - 1; }
            if (y2 >= h) { y2 = h - 1; }


            // Fill first line
            int startY = y1 * w;
            int startYPlusX1 = startY + x1;
            int endOffset = startY + x2;
            for (int x = startYPlusX1; x < endOffset; x++)
            {
                pixels[x] = color;
            }

            // Copy first line
            int len = (x2 - x1) * SizeOfArgb;
            int srcOffsetBytes = startYPlusX1 * SizeOfArgb;
            int offset2 = y2 * w + x1;
            for (int y = startYPlusX1 + w; y < offset2; y += w)
            {
                Buffer.BlockCopy(pixels, srcOffsetBytes, pixels, y * SizeOfArgb, len);
            }
        }
    }
}
